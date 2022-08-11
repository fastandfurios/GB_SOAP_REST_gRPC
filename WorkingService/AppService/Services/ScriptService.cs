using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Serilog;
using WorkingService.AppService.Interfaces;
using WorkingService.AppService.Interfaces.Settings;

namespace WorkingService.AppService.Services
{
    public class ScriptService : IScriptService
    {
        private readonly ISettingsService _settingsService;
        private readonly IStatisticsService _statisticsService;
        private readonly IWorkingServiceCallback _serviceCallback;
        private CompilerResults _compilerResults;

        public ScriptService(ISettingsService settingsService,
            IStatisticsService statisticsService,
            IWorkingServiceCallback serviceCallback)
        {
            _settingsService = settingsService;
            _statisticsService = statisticsService;
            _serviceCallback = serviceCallback;
        }

        public bool Compile()
        {
            try
            {
                var compilerParameters = GetCompilerParameters();

                var stringBuilder = new StringBuilder();

                using (var fileStream = File.Open(_settingsService.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var streamReader = new StreamReader(fileStream);
                    while (streamReader.ReadLine() is string entry)
                        stringBuilder.AppendLine(entry);
                }
                
                var provider = new CSharpCodeProvider();
                _compilerResults = provider.CompileAssemblyFromSource(compilerParameters, stringBuilder.ToString());

                return !IsContainsErrors();
            }
            catch (Exception e)
            {
                Logger.Log.Error(e, $"{DateTime.Now} | {e.Source} | {e.Message}");
                return false;
            }
        }

        public void Run()
        {
            if (_compilerResults == null || (_compilerResults.Errors != null && _compilerResults.Errors.Count != 0))
                if (!Compile())
                    return;

            var type = _compilerResults.CompiledAssembly.GetType("WorkingService.Service");
            if (type == null) return;

            var entryPointMethod = type.GetMethod("EntryPoint");
            if (entryPointMethod == null) return;

            Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (entryPointMethod.Invoke(Activator.CreateInstance(type), new object[] { }) is bool)
                            _statisticsService.SuccessTacts++;
                        else
                            _statisticsService.ErrorTacts++;

                        _statisticsService.AllTacts++;
                        _serviceCallback.UpdateStatistics(_statisticsService as StatisticsService);
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, $"{DateTime.Now} | {e.Source} | {e.Message}");
                }
            });
        }

        private CompilerParameters GetCompilerParameters()
        {
            return new CompilerParameters
            {
                GenerateInMemory = true,
                ReferencedAssemblies =
                {
                    "System.dll",
                    "System.Core.dll",
                    "System.Data.dll",
                    "System.Windows.Forms.dll",
                    "Microsoft.CSharp.dll",
                    $"{Assembly.GetExecutingAssembly().Location}"
                }
            };
        }

        private bool IsContainsErrors()
        {
            if (_compilerResults.Errors != null && _compilerResults.Errors.Count != 0)
            {
                var compileErrors = new StringBuilder();
                for (int i = 0; i < _compilerResults.Errors.Count; i++)
                    compileErrors.AppendLine($"{_compilerResults.Errors[i].FileName} | {_compilerResults.Errors[i].ErrorNumber} | {_compilerResults.Errors[i].ErrorText}");
                
                Log.Logger.Information(compileErrors.ToString());
                return false;
            }

            return true;
        }
    }
}

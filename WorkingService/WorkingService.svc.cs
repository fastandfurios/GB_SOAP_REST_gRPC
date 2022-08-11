using System.ServiceModel;
using Serilog;
using WorkingService.AppService.Interfaces;
using WorkingService.AppService.Interfaces.Settings;
using WorkingService.AppService.Services;
using WorkingService.AppService.Services.Settings;

namespace WorkingService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WorkingService : IWorkingService
    {
        private readonly IScriptService _scriptService;
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _settingsService;

        public WorkingService()
        {
            _statisticsService = new StatisticsService();
            _settingsService = new SettingsService();
            _scriptService = new ScriptService(_settingsService, _statisticsService, Callback);
        }

        IWorkingServiceCallback Callback =>
            OperationContext.Current.GetCallbackChannel<IWorkingServiceCallback>();

        public void RunScript() => _scriptService.Run();

        public void UpdateAndCompileScript(string fileName)
        {
            _settingsService.FileName = fileName;
            _scriptService.Compile();

        }

        public void CompileScript() => _scriptService.Compile();
    }
}

using Serilog;

namespace WorkingService.AppService
{
    public static class Logger
    {
        public static Serilog.Core.Logger Log => new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("C:\\logs\\log_file.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}

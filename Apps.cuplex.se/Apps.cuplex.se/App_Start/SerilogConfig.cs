using Serilog;

namespace Apps.cuplex.se
{
    public static class SerilogConfig
    {
        public static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
          
                .WriteTo.File("logs/apps.cuplex_Log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
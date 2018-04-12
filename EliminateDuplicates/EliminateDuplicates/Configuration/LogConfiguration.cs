using GeneralToolkitLib.Configuration;
using Serilog;
using Serilog.Events;

namespace DeleteDuplicateFiles.Configuration
{
    public static class LogConfiguration
    {
        public static bool IsInitialized { get; private set; }

        public static void InitLogConfig()
        {
            if (IsInitialized)
            {
                Log.Warning("An atempt was made to Initialize the log configuration after it was already initialized!");
                return;
            }

            var logLevel = LogEventLevel.Debug;
            if (!ApplicationBuildConfig.DebugMode)
            {
                logLevel = LogEventLevel.Warning;
            }

            Log.Logger = new LoggerConfiguration()
                //.WriteTo.LiterateConsole(LogEventLevel.Debug, standardErrorFromLevel: LogEventLevel.Error, formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.RollingFile(ApplicationBuildConfig.ApplicationLogFilePath(true),
                    fileSizeLimitBytes: 1048576,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: logLevel,
                    buffered: false,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ff} [{Level}] {Message}{NewLine}{Exception}{Data}")
                .Enrich.FromLogContext()
                .MinimumLevel.Is(logLevel)
                .CreateLogger();

            Log.Debug("LogConfiguration completed init.");
           IsInitialized = true;
        }
    }
}

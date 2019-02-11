using Autofac;
using AutofacSerilogIntegration;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using Serilog;
using Serilog.Events;

namespace DeleteDuplicateFiles.Library.AutofacModules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var logLevel = LogEventLevel.Verbose;
            if (!ApplicationBuildConfig.DebugMode)
            {
                logLevel = LogEventLevel.Warning;
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(GlobalSettings.GetApplicationLogFilePath(),
                    fileSizeLimitBytes: 1048576,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: logLevel,
                    buffered: false,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ff} [{Level}] {Message}{NewLine}{Exception}{Data}")
                .Enrich.FromLogContext()
                .MinimumLevel.Is(logLevel)
                .CreateLogger();

            builder.RegisterLogger();
        }
    }
}

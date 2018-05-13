using Autofac;
using AutofacSerilogIntegration;
using FileSystemRules.Configuration;
using Serilog;
using Serilog.Events;

namespace FileSystemCore.Library.Modules
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
                         .WriteTo.ColoredConsole(logLevel, "{Timestamp:yyyy-MM-dd HH:mm:ss.ff} [{Level}] {Message}{NewLine}{Exception}{Data}",null, null, null)
                         .Enrich.WithThreadId()
                         .Enrich.FromLogContext()
                         .MinimumLevel.Is(logLevel)
                         .CreateLogger();

            builder.RegisterLogger();
        }
    }
}
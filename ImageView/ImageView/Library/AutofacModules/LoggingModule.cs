using System;
using System.Globalization;
using Autofac;
using AutofacSerilogIntegration;
using ImageView.Configuration;
using Serilog;
using Serilog.Events;

namespace ImageView.Library.AutofacModules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole(LogEventLevel.Debug, standardErrorFromLevel: LogEventLevel.Error, formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.RollingFile(ApplicationBuildConfig.ApplicationLogFilePath(true),
                    fileSizeLimitBytes: 1048576,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    buffered: false,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ff} [{Level}] {Message}{NewLine}{Exception}{Data}")
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .CreateLogger();

            builder.RegisterLogger();
        }
    }
}
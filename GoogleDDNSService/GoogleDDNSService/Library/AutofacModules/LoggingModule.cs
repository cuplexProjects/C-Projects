using System;
using System.Reflection;
using Autofac;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Module = Autofac.Module;

namespace GoogleDDNSService.Library.AutofacModules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Create config
            var config = new LoggingConfiguration();
         
            // EventLog
            string productName = Assembly.GetCallingAssembly().GetName().Name;
            var eventLogTarget = new EventLogTarget(productName)
            {
                EntryType = "EventLog"
                , Layout = "${longdate} ${level} ${message}  ${exception}"
                , Category = "Layout"
                , EventId = "Layout"
                , MachineName = Environment.MachineName
            };

            // File Log
            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = "${basedir}/file.txt",
                Layout = "${longdate} ${level} ${message}  ${exception}"
            };
            config.AddTarget(fileTarget);

            // Define rules
            config.AddRuleForOneLevel(LogLevel.Warn, fileTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, eventLogTarget);

            // Activate the configuration
            LogManager.Configuration = config;
        }
    }
}
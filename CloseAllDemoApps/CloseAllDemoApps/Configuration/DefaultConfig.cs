using System;
using System.IO;
using System.Reflection;
using CloseAllDemoApps.Models;

namespace CloseAllDemoApps.Configuration
{
    public static class DefaultConfig
    {
        public static ApplicationSettings GetDefaultApplicationSettings()
        {
            ApplicationSettings appSettings = new ApplicationSettings
            {
                AlwaysOntop = false,
                LogLevel = ApplicationLogLevels.Error,
                MinimizeToSystemTray = true,
                ProcessDescriptionFilter = "Toyota.TMHE.OrderOptimizer",
            };

            return appSettings;
        }
    }

    public static class GlobalSettings
    {
        private static readonly string AssemblyName;
        private const string AppSettingsFilename = "config.ini";

        static GlobalSettings()
        {
            AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        }

        public static string GetApplicationSettingsFilePath()
        {
            string appSettingsIniFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + AssemblyName + "\\";

            if (!Directory.Exists(appSettingsIniFilePath))
                Directory.CreateDirectory(appSettingsIniFilePath);

            return appSettingsIniFilePath + AppSettingsFilename;
        }
    }
}
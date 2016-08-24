using System;
using CloseAllDemoApps.Models;

namespace CloseAllDemoApps.Configuration
{
    public class AppSettingsService : IDisposable
    {
        private static AppSettingsService _instance;
        private readonly IniConfigFileManager _iniConfigFileManager;
        private readonly ApplicationSettings _applicationSettings;
        private readonly object _threadLock = new object();
        private readonly string _appSettingsFilePath;
        private const string ApplicationSettingsSectionName = "General";

        private AppSettingsService()
        {
            _applicationSettings = DefaultConfig.GetDefaultApplicationSettings();
            _appSettingsFilePath = GlobalSettings.GetApplicationSettingsFilePath();
            _iniConfigFileManager = new IniConfigFileManager();
        }

        public void Save()
        {
            IniConfigFileSection iniConfigFileSection = new IniConfigFileSection();

            iniConfigFileSection.ConfigItems.Add("AlwaysOntop", _applicationSettings.AlwaysOntop.ToString());
            iniConfigFileSection.ConfigItems.Add("MinimizeToSystemTray", _applicationSettings.MinimizeToSystemTray.ToString());
            iniConfigFileSection.ConfigItems.Add("LogLevel", _applicationSettings.LogLevel.ToString());

            _iniConfigFileManager.ConfigurationData.ConfigSections.Clear();
            _iniConfigFileManager.ConfigurationData.ConfigItems.Clear();
            _iniConfigFileManager.ConfigurationData.ConfigSections.Add(ApplicationSettingsSectionName, iniConfigFileSection);
            _iniConfigFileManager.SaveConfigFile(_appSettingsFilePath);
        }

        public void Load()
        {
            if (!_iniConfigFileManager.LoadConfigFile(_appSettingsFilePath))
                return;

            var configItems = _iniConfigFileManager.ConfigurationData.ConfigSections[ApplicationSettingsSectionName].ConfigItems;

            string configValue = configItems.Get("AlwaysOntop");
            _applicationSettings.AlwaysOntop = bool.Parse(configValue);

            configValue = configItems.Get("MinimizeToSystemTray");
            _applicationSettings.MinimizeToSystemTray = bool.Parse(configValue);

            configValue = configItems.Get("LogLevel");
            if (configValue != null)
                _applicationSettings.LogLevel =
                    (ApplicationLogLevels)Enum.Parse(typeof(ApplicationLogLevels), configValue);
        }

        public ApplicationSettings Settings
        {
            get
            {
                lock (_threadLock)
                {
                    return _applicationSettings;
                }
            }
        }

        public static AppSettingsService SettingsService
        {
            get { return _instance ?? (_instance = new AppSettingsService()); }
        }

        public void Dispose()
        {
            _instance = null;
        }
    }
}

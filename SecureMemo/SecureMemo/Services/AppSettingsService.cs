using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;
using SecureMemo.DataModels;
using SecureMemo.Utility;

namespace SecureMemo.Services
{
    public class AppSettingsService
    {
        private static AppSettingsService _instance;
        private readonly SecureMemoAppSettings _defaultAppSettings;
        private readonly IniConfigFileManager _iniConfigFileManager;
        private readonly string _iniConfigFilePath;

        private AppSettingsService()
        {
            _iniConfigFileManager = new IniConfigFileManager();
            Settings = ConfigHelper.GetDefaultSettings();
            CreateAppDataDirectoryIfDirNotFound(ConfigSpecificSettings.GetSettingsFolderPath(false));
            _defaultAppSettings = ConfigHelper.GetDefaultSettings();
            _iniConfigFilePath = ConfigSpecificSettings.GetSettingsFolderPath(false) + "\\ApplicationSettings.ini";
        }

        public SecureMemoAppSettings Settings { get; }

        public static AppSettingsService Instance => _instance ?? (_instance = new AppSettingsService());

        private void CreateAppDataDirectoryIfDirNotFound(string settingsFolderPath)
        {
            try
            {
                if (!Directory.Exists(settingsFolderPath))
                    Directory.CreateDirectory(settingsFolderPath);
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error when creating app data folder", ex);
            }
        }

        public void LoadSettings()
        {
            try
            {
                if (!File.Exists(_iniConfigFilePath))
                {
                    SaveSettings();
                    return;
                }

                if (!_iniConfigFileManager.LoadConfigFile(_iniConfigFilePath))
                    throw new Exception("Unable to load application settings");

                int winSize;
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                IniConfigFileSection generalConfigFileSection = _iniConfigFileManager.ConfigurationData.ConfigSections["General"];
                Settings.DefaultEmptyTabPages = int.Parse(generalConfigFileSection.ConfigItems["DefaultEmptyTabPages"]);
                Settings.ApplicationSaltValue = generalConfigFileSection.ConfigItems["ApplicationSaltValue"];
                Settings.PasswordDerivedString = generalConfigFileSection.ConfigItems["PasswordDerivedString"];
                Settings.AlwaysOntop = bool.Parse(generalConfigFileSection.ConfigItems["AlwaysOntop"]);

                if (int.TryParse(generalConfigFileSection.ConfigItems["MainWindowWith"], out winSize))
                    Settings.MainWindowWith = winSize;

                if (int.TryParse(generalConfigFileSection.ConfigItems["MainWindowHeight"], out winSize))
                    Settings.MainWindowHeight = winSize;

                if (Settings.MainWindowWith < _defaultAppSettings.MainWindowWith)
                    Settings.MainWindowWith = _defaultAppSettings.MainWindowWith;

                if (Settings.MainWindowWith > screenWidth)
                    Settings.MainWindowWith = screenWidth;

                if (Settings.MainWindowHeight < _defaultAppSettings.MainWindowHeight)
                    Settings.MainWindowHeight = _defaultAppSettings.MainWindowHeight;

                if (Settings.MainWindowHeight > screenHeight)
                    Settings.MainWindowHeight = screenHeight;

                if (Settings.ApplicationSaltValue == null || Settings.ApplicationSaltValue.Length != 1024)
                    throw new Exception("ApplicationSaltValue Length must be 1024 characters");

                // Shared Database folder
                bool useSharedSyncFolder = false;
                if (generalConfigFileSection.ConfigItems["UseSharedSyncFolder"] != null)
                    bool.TryParse(generalConfigFileSection.ConfigItems["UseSharedSyncFolder"], out useSharedSyncFolder);

                Settings.UseSharedSyncFolder = useSharedSyncFolder;
                Settings.SyncFolderPath = generalConfigFileSection.ConfigItems["SyncFolderPath"];

                // Font settings
                IniConfigFileSection value;
                if (_iniConfigFileManager.ConfigurationData.ConfigSections.TryGetValue("FontSettings", out value))
                {
                    IniConfigFileSection fontConfigFileSection = value;
                    var fontSettings = new SecureMemoFontSettings
                    {
                        FontSize = Convert.ToSingle(fontConfigFileSection.ConfigItems["FontSize"]),
                        FontFamilyName = fontConfigFileSection.ConfigItems["FontFamilyName"],
                        Style = (FontStyle) Enum.Parse(typeof (FontStyle), fontConfigFileSection.ConfigItems["Style"])
                    };
                    fontSettings.FontFamily = new Font(fontConfigFileSection.ConfigItems["FontFamily"], fontSettings.FontSize, fontSettings.Style).FontFamily;

                    Settings.FontSettings = fontSettings;
                }
                else
                    Settings.FontSettings = _defaultAppSettings.FontSettings;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Failed to load application settings", ex);
                throw;
            }
        }

        public void SaveSettings()
        {
            try
            {
                if (!_iniConfigFileManager.ConfigurationData.ConfigSections.ContainsKey("General"))
                    _iniConfigFileManager.ConfigurationData.ConfigSections.Add("General", new IniConfigFileSection());

                IniConfigFileSection generalConfigFileSection = _iniConfigFileManager.ConfigurationData.ConfigSections["General"];
                generalConfigFileSection.ConfigItems["DefaultEmptyTabPages"] = Settings.DefaultEmptyTabPages.ToString();
                generalConfigFileSection.ConfigItems["ApplicationSaltValue"] = Settings.ApplicationSaltValue;
                generalConfigFileSection.ConfigItems["PasswordDerivedString"] = Settings.PasswordDerivedString;
                generalConfigFileSection.ConfigItems["AlwaysOntop"] = Settings.AlwaysOntop ? "True" : "False";
                generalConfigFileSection.ConfigItems["MainWindowWith"] = Settings.MainWindowWith.ToString();
                generalConfigFileSection.ConfigItems["MainWindowHeight"] = Settings.MainWindowHeight.ToString();

                // Shared Database folder
                generalConfigFileSection.ConfigItems["UseSharedSyncFolder"] = Settings.UseSharedSyncFolder ? "True" : "False";
                generalConfigFileSection.ConfigItems["SyncFolderPath"] = Settings.SyncFolderPath;

                if (!_iniConfigFileManager.ConfigurationData.ConfigSections.ContainsKey("FontSettings"))
                    _iniConfigFileManager.ConfigurationData.ConfigSections.Add("FontSettings", new IniConfigFileSection());

                IniConfigFileSection fontConfigFileSection = _iniConfigFileManager.ConfigurationData.ConfigSections["FontSettings"];
                fontConfigFileSection.ConfigItems["FontFamily"] = Settings.FontSettings.FontFamily.Name;
                fontConfigFileSection.ConfigItems["FontFamilyName"] = Settings.FontSettings.FontFamilyName;
                fontConfigFileSection.ConfigItems["Style"] = Settings.FontSettings.Style.ToString();
                fontConfigFileSection.ConfigItems["FontSize"] = Settings.FontSettings.FontSize.ToString();

                _iniConfigFileManager.SaveConfigFile(_iniConfigFilePath);
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error in AppSettings Save", ex);
            }
        }
    }
}
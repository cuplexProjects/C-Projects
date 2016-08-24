using System;
using System.IO;
using System.Reflection;
using GeneralToolkitLib.ConfigHelper;
using GenerateOvpnFile.Misc;
using GenerateOvpnFile.Models;

namespace GenerateOvpnFile.Settings
{
    public class ConfigurationManager
    {
        private static ConfigurationManager _instance;
        private readonly AppConfigSettings _appConfigSettings;
        private readonly IniConfigFileManager _iniConfigFileManager;
        private string iniConfigFilePath;
        private string folderPath;

        private ConfigurationManager()
        {
            _iniConfigFileManager = new IniConfigFileManager();
            _appConfigSettings = new AppConfigSettings();
            Initialize();
        }

        private void Initialize()
        {
            iniConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name + "\\ApplicationSettings.ini";
            folderPath = Converters.GetDirectoryNameFromPath(iniConfigFilePath);
            try
            {
                if(!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
            catch(Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            }
        }

        public void LoadSettings()
        {
            try
            {
                if (!File.Exists(iniConfigFilePath))
                    return;

                if(_iniConfigFileManager.LoadConfigFile(iniConfigFilePath))
                {
                    var GeneralConfigSection = _iniConfigFileManager.ConfigurationData.ConfigSections["General"];
                    _appConfigSettings.Host = GeneralConfigSection.ConfigItems["Host"];
                    _appConfigSettings.Name = GeneralConfigSection.ConfigItems["Name"];
                }

            }
            catch(Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            }
        }

        public void SaveSettings()
        {
            try
            {
                _iniConfigFileManager.ConfigurationData.ConfigItems.Clear();
                _iniConfigFileManager.ConfigurationData.ConfigSections.Clear();

                IniConfigFileSection confFileSection = new IniConfigFileSection();
                confFileSection.ConfigItems.Add("Host", _appConfigSettings.Host);
                confFileSection.ConfigItems.Add("Name", _appConfigSettings.Name);
                _iniConfigFileManager.ConfigurationData.ConfigSections.Add("General", confFileSection);

                _iniConfigFileManager.SaveConfigFile(iniConfigFilePath);

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            } 
        }

        public AppConfigSettings AppConfigSettings
        {
            get { return this._appConfigSettings; }
        }

        public static ConfigurationManager Instance
        {
            get { return _instance ?? (_instance = new ConfigurationManager()); }
        }
    }
}

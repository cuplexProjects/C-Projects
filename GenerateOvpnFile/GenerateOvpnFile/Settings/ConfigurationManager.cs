using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GenerateOvpnFile.Misc;
using GenerateOvpnFile.Models;

namespace GenerateOvpnFile.Settings
{
    public class ConfigurationManager
    {
        private static ConfigurationManager _instance;
        private readonly AppConfigSettings _appConfigSettings;
        private readonly IniConfigFileManager _iniConfigFileManager;
        private string _iniConfigFilePath;
        private string _folderPath;

        private ConfigurationManager()
        {
            _iniConfigFileManager = new IniConfigFileManager();
            _appConfigSettings = new AppConfigSettings();
            Initialize();
        }

        private void Initialize()
        {
#if DEBUG
            _iniConfigFilePath = Converters.GetDirectoryNameFromPath(Application.ExecutablePath) + "ApplicationSettings.ini";
            _folderPath = Converters.GetDirectoryNameFromPath(Application.ExecutablePath);
#else
            _iniConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name + "\\ApplicationSettings.ini";
            _folderPath = Converters.GetDirectoryNameFromPath(_iniConfigFilePath);
#endif



            try
            {
                if(!Directory.Exists(_folderPath))
                    Directory.CreateDirectory(_folderPath);
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
                if (!File.Exists(_iniConfigFilePath))
                    return;

                if(_iniConfigFileManager.LoadConfigFile(_iniConfigFilePath))
                {
                    var generalConfigSection = _iniConfigFileManager.ConfigurationData.ConfigSections["General"];
                    _appConfigSettings.Host = generalConfigSection.ConfigItems["Host"];
                    _appConfigSettings.Name = generalConfigSection.ConfigItems["Name"];

                    _appConfigSettings.Interface = generalConfigSection.ConfigItems["Interface"];
                    _appConfigSettings.ServerPort = generalConfigSection.ConfigItems["ServerPort"].ToString();
                    _appConfigSettings.Protocol = generalConfigSection.ConfigItems["Protocol"];
                    _appConfigSettings.ExtraHmac = generalConfigSection.ConfigItems["ExtraHmac"];
                    _appConfigSettings.Compression = generalConfigSection.ConfigItems["Compression"];
                    _appConfigSettings.Cipher = generalConfigSection.ConfigItems["Cipher"];
                    _appConfigSettings.CaFileName = generalConfigSection.ConfigItems["CAPath"];
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

                confFileSection.ConfigItems.Add("Interface", _appConfigSettings.Interface);
                confFileSection.ConfigItems.Add("ServerPort", _appConfigSettings.ServerPort);
                confFileSection.ConfigItems.Add("Protocol", _appConfigSettings.Protocol);
                confFileSection.ConfigItems.Add("ExtraHmac", _appConfigSettings.ExtraHmac);
                confFileSection.ConfigItems.Add("Compression", _appConfigSettings.Compression);
                confFileSection.ConfigItems.Add("Cipher", _appConfigSettings.Cipher);
                confFileSection.ConfigItems.Add("CAFileName", _appConfigSettings.CaFileName);

                _iniConfigFileManager.ConfigurationData.ConfigSections.Add("General", confFileSection);

                _iniConfigFileManager.SaveConfigFile(_iniConfigFilePath);

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            } 
        }

        public AppConfigSettings AppConfigSettings => _appConfigSettings;

        public static ConfigurationManager Instance => _instance ?? (_instance = new ConfigurationManager());
    }
}

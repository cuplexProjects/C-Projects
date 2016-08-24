using System;
using System.IO;
using System.Reflection;

namespace PriceCalculator.Settings
{
    public class SettingsService
    {
        private static SettingsService _instance;
        private static string _appDataPath;
        private readonly IniConfigFileManager _iniConfigFileManager;
        private PriceCalcSettings _settings;
        private readonly string _appSettingsFilename;
        private const string CONFIG_FILENAME = "Settings.ini";

        private SettingsService()
        {
            _iniConfigFileManager = new IniConfigFileManager();
            _settings = PriceCalcSettings.GetDefaultSettings();
            _appSettingsFilename = ApplicationDataFolder + "\\" + CONFIG_FILENAME;
            CreateAppDataFolderIfMissing();
        }

        private void CreateAppDataFolderIfMissing()
        {
            if(!Directory.Exists(ApplicationDataFolder))
                Directory.CreateDirectory(ApplicationDataFolder);
        }

        public bool LoadSettings()
        {
            try
            {
                if (!File.Exists(_appSettingsFilename))
                    return false;

                if(_iniConfigFileManager.LoadConfigFile(_appSettingsFilename))
                {
                    _settings = new PriceCalcSettings();

                    IniConfigFileSection exchangeRateConfigFileSection = _iniConfigFileManager.ConfigurationData.ConfigSections["ExchangeRates"];
                    _settings.BTC_Price = double.Parse(exchangeRateConfigFileSection.ConfigItems["BTC_Price"]);
                    _settings.SEK_USD_Rate = double.Parse(exchangeRateConfigFileSection.ConfigItems["SEK_USD_Rate"]);
                    _settings.Commision = double.Parse(exchangeRateConfigFileSection.ConfigItems["Commision"]);

                    IniConfigFileSection uiConfigFileSection = _iniConfigFileManager.ConfigurationData.ConfigSections["UserInterface"];
                    _settings.Topmost = uiConfigFileSection.ConfigItems["Topmost"] == "1";
                    _settings.AutofocusPrice = uiConfigFileSection.ConfigItems["AutofocusPrice"] == "1";

                    if(uiConfigFileSection.ConfigItems["StartupPosX"] != null)
                    {
                        int startX = int.Parse(uiConfigFileSection.ConfigItems["StartupPosX"]);
                        int startY = int.Parse(uiConfigFileSection.ConfigItems["StartupPosY"]);
                        _settings.StartPosition = new Coordinate(startX, startY);
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {
                
            }
            return false;
        }

        public void SaveSettings()
        {
            _iniConfigFileManager.ConfigurationData.ConfigItems.Clear();
            _iniConfigFileManager.ConfigurationData.ConfigSections.Clear();

            // Exchange rates
            IniConfigFileSection configSection = new IniConfigFileSection();
            configSection.ConfigItems.Add("BTC_Price", _settings.BTC_Price.ToString());
            configSection.ConfigItems.Add("SEK_USD_Rate", _settings.SEK_USD_Rate.ToString());
            configSection.ConfigItems.Add("Commision", _settings.Commision.ToString());
            _iniConfigFileManager.ConfigurationData.ConfigSections.Add("ExchangeRates", configSection);

            // General settings
            configSection = new IniConfigFileSection();
            configSection.ConfigItems.Add("Topmost", _settings.Topmost ? "1" : "0");
            configSection.ConfigItems.Add("AutofocusPrice", _settings.AutofocusPrice ? "1" : "0");
            if(_settings.StartPosition != null)
            {
                configSection.ConfigItems.Add("StartupPosX", _settings.StartPosition.X.ToString());
                configSection.ConfigItems.Add("StartupPosY", _settings.StartPosition.Y.ToString());    
            }
            
            _iniConfigFileManager.ConfigurationData.ConfigSections.Add("UserInterface", configSection);
            _iniConfigFileManager.SaveConfigFile(_appSettingsFilename);
        }

        public PriceCalcSettings Settings
        {
            get { return _settings; }
        }

        public static SettingsService Instance
        {
            get { return _instance ?? (_instance = new SettingsService()); }
        }

        public static string ApplicationDataFolder
        {
            get { return _appDataPath ?? (_appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name); }
        }
    }
}
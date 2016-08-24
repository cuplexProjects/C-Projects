using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SecureChat.Common.ConfigManagement;
using SecureChat.DAL.Linq;
using Toyota.TMHE.TPC.DAL.Linq;

namespace SecureChat.InternalServices.ApplicationSettingsServiceImplementation
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        private IServerConfiguration _serverConfiguration;
        private bool _initialized;

        public IServerConfiguration GetServerConfiguration()
        {
            if(_initialized)
            {
                return _serverConfiguration;
            }

            try
            {
                _serverConfiguration = GetServerConfigurationFromDb();
                _initialized = true;
            }
            catch(Exception ex)
            {
                _serverConfiguration = new ServerConfiguration();
            }

            return _serverConfiguration;
        }

        private IServerConfiguration GetServerConfigurationFromDb()
        {
            ServerConfiguration serverConf = new ServerConfiguration();
            using (var db = SecureChatDataContext.Create()){

                var configProperties = serverConf.GetType().GetProperties();
                foreach (PropertyInfo configProperty in configProperties)
                {
                    var setting = db.ServerSettings.FirstOrDefault(k => k.KeyName == configProperty.Name);
                    if (setting != null)
                    {
                        SettingDataTypes settingDataType = (SettingDataTypes)Enum.Parse(typeof(SettingDataTypes), setting.DataType.ToString());
                        switch (settingDataType)
                        {
                            case SettingDataTypes.sInt:
                                int iTmp = int.Parse(setting.Value);
                                configProperty.SetValue(serverConf, iTmp);
                                break;
                            case SettingDataTypes.sString:
                                configProperty.SetValue(serverConf, setting.Value);
                                break;
                            case SettingDataTypes.sBool:
                                bool bTmp = bool.Parse(setting.Value);
                                configProperty.SetValue(serverConf, bTmp);
                                break;
                            case SettingDataTypes.sDatetime:
                                DateTime sDateTmp = DateTime.Parse(setting.Value);
                                configProperty.SetValue(serverConf, sDateTmp);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            return serverConf;
        }

        public void SaveServerConfiguration(IServerConfiguration serverConfiguration)
        {
            serverConfiguration.LastSave = DateTime.Now;
            using (var db = SecureChatDataContext.Create())
            {
                var configProperties = serverConfiguration.GetType().GetProperties();
                foreach (PropertyInfo configProperty in configProperties)
                {
                    var setting = db.ServerSettings.FirstOrDefault(k => k.KeyName == configProperty.Name);
                    if(setting == null)
                    {
                        setting = new ServerSetting();
                        db.ServerSettings.InsertOnSubmit(setting);
                    }

                    setting.KeyName = configProperty.Name;
                    setting.Value = configProperty.GetValue(serverConfiguration).ToString();

                    if(configProperty.GetMethod.ReturnType == typeof(int))
                    {
                        setting.DataType = (int)SettingDataTypes.sInt;
                    }
                    else if(configProperty.GetMethod.ReturnType == typeof(string))
                    {
                        setting.DataType = (int)SettingDataTypes.sString;
                    }
                    else if(configProperty.GetMethod.ReturnType == typeof(bool))
                    {
                        setting.DataType = (int)SettingDataTypes.sBool;
                    }
                    else if(configProperty.GetMethod.ReturnType == typeof(DateTime))
                    {
                        setting.DataType = (int)SettingDataTypes.sDatetime;
                    }
                    else
                    {
                        setting.DataType = (int)SettingDataTypes.sUnknown;
                    }

                    db.SubmitChanges();
                }
            }
        }

        public async Task<bool> CanConnectToDatabase()
        {
            try
            {
                return await Task.Run(() =>
                {
                    using (var db = SecureChatDataContext.Create())
                    {
                        return db.DatabaseExists();
                    }
                });

            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(Assembly.GetExecutingAssembly().GetName().FullName, "Database connection failed: " + ex.Message, EventLogEntryType.Error);
            }   
            return false;
        }

        public async Task<bool> HasValidSettingsAsync()
        {
            try
            {
                return await Task.Run(() => HasValidSettings());

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(Assembly.GetExecutingAssembly().GetName().FullName, "Database connection failed: " + ex.Message, EventLogEntryType.Error);
            }
            return false;
        }

        public bool HasValidSettings()
        {
            using (var db = SecureChatDataContext.Create())
            {
                var configProperties = typeof(IServerConfiguration).GetProperties();
                if(configProperties.Any(configProperty => !db.ServerSettings.Any(s => s.KeyName == configProperty.Name)))
                {
                    return false;
                }
            }
            return true;
        }

        public enum SettingDataTypes
        {
            sUnknown = 0,
            sInt = 1,
            sString = 2,
            sBool = 3,
            sDatetime = 4
        }
    }
}

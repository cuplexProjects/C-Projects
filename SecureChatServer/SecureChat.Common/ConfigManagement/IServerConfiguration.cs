using System;

namespace SecureChat.Common.ConfigManagement
{
    public interface IServerConfiguration
    {
        string ServerName { get; set; }
        int WebservicePort { get; set; }
        bool UserDefaultHidden { get; set; }
        bool SaveUserProfileData { get; set; }
        string ServerInstanceGuid { get; set; }
        string ServerGlobalSalt { get; set; }
        DateTime LastSave { get; set; }
    }
}

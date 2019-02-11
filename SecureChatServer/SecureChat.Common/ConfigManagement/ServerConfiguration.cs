using System;

namespace SecureChat.Common.ConfigManagement
{
    public class ServerConfiguration : IServerConfiguration
    {
        public string ServerName { get; set; }
        public int WebservicePort { get; set; }
        public bool UserDefaultHidden { get; set; }
        public bool SaveUserProfileData { get; set; }
        public string ServerInstanceGuid { get; set; }
        public string ServerGlobalSalt { get; set; }
        public DateTime LastSave { get; set; }
    }
}
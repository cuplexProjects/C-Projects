using System;

namespace SecureChat.DataModels
{
    [Serializable]
    public class GlobalAppSettings
    {
        public string APIHostName { get; set; }
        public bool UserHasBeenCreated { get; set; }
        public string LastDirectoryForUserData { get; set; }
    }
}

using System;

namespace SecureChatSharedComponents.ApiResponce
{
    [Serializable]
    public class CreateNewUser
    {
        public bool Successful { get; set; }
        public ResonceCodes APiResponceCode { get; set; }
        public string UserProfileEncodedData { get; set; }
        public string EncodedAESKey { get; set; }
    }

    [Serializable]
    public enum ResonceCodes
    {
        UserCreated = 1,
        UsernameAlreeadyExists = 2,
        Error = 128
    }

    [Serializable]
    public enum UserStatus
    {
        Offline = 1,
        Online = 2,
    }
}

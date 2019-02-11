using System;
using System.Runtime.Serialization;

namespace SecureChatSharedComponents.DataModels
{
    [Serializable]
    [DataContract(Name = "SecureChatPublicUserInfo")]
    public class SecureChatPublicUserInfo
    {
        [DataMember(Name = "Nickname", Order = 1)]
        public string Nickname { get; set; }

        [DataMember(Name = "SaltValue", Order = 2)]
        public string SaltValue { get; set; }

        [DataMember(Name = "PasswordHash", Order = 3)]
        public string PasswordHash { get; set; }

        [DataMember(Name = "EncryptedUserData", Order = 4)]
        public string EncryptedUserData { get; set; }
    }
}

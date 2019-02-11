using System;
using System.Runtime.Serialization;

namespace SecureChatSharedComponents.ApiDtos
{
    [Serializable]
    [DataContract(Name = "CreateNewUser")]
    public class CreateNewUser
    {
        [DataMember(Name = "Successful", Order = 1)]
        public bool Successful { get; set; }

        [DataMember(Name = "APiResponceCode", Order = 2)]
        public ApiResonceCodes APiResponceCode { get; set; }

        [DataMember(Name = "UserProfileData", Order = 3)]
        public string UserProfileData { get; set; }

        [DataMember(Name = "EncodedAESKey", Order = 4)]
        public string EncodedAESKey { get; set; }

        [DataMember(Name = "ServerSignature", Order = 4)]
        public string ServerSignature { get; set; }

        [DataMember(Name = "ServerSignaturePublicKey", Order = 5)]
        public string ServerSignaturePublicKey { get; set; }
    }

    [Serializable]
    [DataContract(Name = "ApiResonceCodes")]
    public enum ApiResonceCodes
    {
        UserCreated = 1,
        UsernameAlreeadyExists = 2,
        Error = 128
    }

    [Serializable]
    [DataContract(Name = "UserStatus")]
    public enum UserStatus
    {
        Offline = 1,
        Online = 2,
    }
}
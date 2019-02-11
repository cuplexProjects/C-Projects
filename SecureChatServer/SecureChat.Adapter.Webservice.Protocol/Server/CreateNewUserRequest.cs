using System;
using System.Runtime.Serialization;

namespace SecureChat.Adapter.Webservice.Protocol.Server
{
    [Serializable]
    [DataContract(Name = "CreateNewUser")]
    public class CreateNewUserRequest
    {
        [DataMember(Name = "Successful", Order = 1)]
        public bool Successful { get; set; }

        [DataMember(Name = "EncodedUserProfile", Order = 2)]
        public string UserProfileData { get; set; }

        [DataMember(Name = "DecodingKey", Order = 3)]
        public string EncodedAESKey { get; set; }

        [DataMember(Name = "ServerSignature", Order = 4)]
        public string ServerSignature { get; set; }
    }
}
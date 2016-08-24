using System;
using System.Runtime.Serialization;

namespace SecureChatServerModule.DtoModels
{
    [Serializable]
    [DataContract(Name = "ServerConfigurationDto")]
    public class ServerConfigurationDto
    {
        [DataMember(Name = "ServerName", Order = 1)]
        public string ServerName { get; set; }

        [DataMember(Name = "PrivateKeyPasswordDerivative", Order = 2)]
        public byte[] PrivateKeyPasswordDerivative { get; set; }

        [DataMember(Name = "PasswordSalt", Order = 3)]
        public byte[] PasswordSalt { get; set; }

        [DataMember(Name = "InitTimestamp", Order = 4)]
        public DateTime InitTimestamp { get; set; }

        [DataMember(Name = "PublicKeyBytes", Order = 5)]
        public byte[] PublicKeyBytes { get; set; }

        [DataMember(Name = "PrivateKeyBytes", Order = 6)]
        public byte[] PrivateKeyBytes { get; set; }
    }
}

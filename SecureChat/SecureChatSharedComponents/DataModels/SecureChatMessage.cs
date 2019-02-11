using System;
using System.Runtime.Serialization;

// AES Encrypted class
namespace SecureChatSharedComponents.DataModels
{
    [Serializable]
    [DataContract(Name = "SecureChatMessage")]
    public class SecureChatMessage
    {
        [DataMember(Name = "SenderGuid", Order = 1)]
        public string SenderGuid { get; set; }

        [DataMember(Name = "SenderNickname", Order = 2)]
        public string SenderNickname { get; set; }

        [DataMember(Name = "TextContent", Order = 3)]
        public string TextContent { get; set; }

        [DataMember(Name = "FileDataContent", Order = 4)]
        public byte[] FileDataContent { get; set; }

        [DataMember(Name = "DataContentHash", Order = 5)]
        public string DataContentHash { get; set; }

        [DataMember(Name = "MessageDataContentType", Order = 6)]
        public MessageDataContentTypes MessageDataContentType { get; set; }

        [DataMember(Name = "TimeStamp", Order = 7)]
        public DateTime TimeStamp { get; set; }

        [DataMember(Name = "ServerSalt", Order = 8)]
        public string ServerSalt { get; set; }

        [DataMember(Name = "ServerSignature", Order = 9)]
        public string ServerSignature { get; set; }
    }

    [Serializable]
    [DataContract(Name = "MessageDataContentTypes")]
    public enum MessageDataContentTypes
    {
        Text,
        File,
    }
}

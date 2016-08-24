using System;
using System.Runtime.Serialization;

namespace SecureChatSharedComponents.ApiDtos
{
    [Serializable]
    [DataContract]
    public class SecureChatMessage
    {
        [DataMember(Name = "SenderGuid", Order = 1)]
        public string SenderGuid { get; set; }

        [DataMember(Name = "ReceiverGuid", Order = 2)]
        public string ReceiverGuid { get; set; }
        
        [DataMember(Name = "MessageData", Order = 3)]
        public string MessageData { get; set; }

        [DataMember(Name = "MessageDataHash", Order = 4)]
        public string MessageDataHash { get; set; }

        [DataMember(Name = "ServerSignature", Order = 5)]
        public string ServerSignature { get; set; }
        
        [DataMember(Name = "MessageDataAESKeyComponent", Order = 6)]
        public string MessageDataAESKeyComponent { get; set; }

        [DataMember(Name = "MesageDate", Order = 7)]
        public DateTime MesageDate { get; set; }
    }
}
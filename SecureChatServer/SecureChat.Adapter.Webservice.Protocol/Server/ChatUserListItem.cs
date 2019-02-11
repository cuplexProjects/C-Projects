using System;
using System.Runtime.Serialization;

namespace SecureChat.Adapter.Webservice.Protocol.Server
{
    [Serializable]
    [DataContract(Name = "ChatUserListItem")]
    public class ChatUserListItem
    {
        [DataMember(Name = "GUID", Order = 1)]
        public string GUID { get; set; }

        [DataMember(Name = "NickName", Order = 2)]
        public string NickName { get; set; }

        [DataMember(Name = "PublicKey", Order = 3)]
        public string PublicKey { get; set; }

        [DataMember(Name = "Status", Order = 4)]
        public int Status { get; set; }
    }
}

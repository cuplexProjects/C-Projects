using System;
using System.Runtime.Serialization;

namespace SecureChatSharedComponents.DataModels
{
    [Serializable]
    [DataContract]
    public class SecureChatUser
    {
        [DataMember(Name = "GUID", Order = 1)]
        public string GUID { get; set; }

        [DataMember(Name = "NickName", Order = 2)]
        public string NickName { get; set; }

        [DataMember(Name = "IsVisibleToSearch", Order = 3)]
        public bool IsVisibleToSearch { get; set; }

        [DataMember(Name = "UserInfo", Order = 4)]
        public SecureChatUserInfo UserInfo { get; set; }

        [DataMember(Name = "Server_Certificate", Order = 5)]
        public string Server_Certificate { get; set; }

        [DataMember(Name = "User_Certificate", Order = 6)]
        public string User_Certificate { get; set; }

        [DataMember(Name = "User_PrivateKey", Order = 7)]
        public string User_PrivateKey { get; set; }
    }
}
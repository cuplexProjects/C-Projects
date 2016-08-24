using System;

namespace SecureChatSharedComponents.DataModels
{
    [Serializable]
    public class SecureChatCreateUserRequest
    {
        public string Nickname { get; set; }
        public string RSA_PublicKey { get; set; }
    }
}

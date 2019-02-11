using GeneralToolkitLib.Encryption;
using SecureChatSharedComponents.DataModels;

namespace SecureChatSharedComponents.RSA
{
    public class UserProfileManager
    {
        public static SecureChatUser CreateChatUser(string nickname, string guid)
        {
            SecureChatUser user = new SecureChatUser {GUID = guid, NickName = nickname, IsVisibleToSearch = false};
            user.UserInfo = new SecureChatUserInfo();

            RSA_AsymetricEncryption rsaAsymetricEncryption= new RSA_AsymetricEncryption();
            RSAKeySetIdentity rsaKeySet = rsaAsymetricEncryption.GenerateRSAKeyPair(RSA_AsymetricEncryption.RSAKeySize.b4096);

            user.User_Certificate = rsaKeySet.RSA_PublicKey;
            user.User_PrivateKey = rsaKeySet.RSA_PrivateKey;
            user.Server_Certificate = ServerKeyManager.KeyManager.ServerPublicKey;

            return user;
        }
    }
}

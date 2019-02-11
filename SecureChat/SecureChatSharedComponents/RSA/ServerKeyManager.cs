using System.Security.Cryptography;
using GeneralToolkitLib.Encryption;

namespace SecureChatSharedComponents.RSA
{
    public class ServerKeyManager
    {
        private static ServerKeyManager _instance;
        private bool _initialized = false;
        private RSAKeySetIdentity _serveRsaKeySetIdentity;
        private RSAParameters _rsaParameters;
        private readonly RSA_AsymetricEncryption _rsaAsymetricEncryption;

        private ServerKeyManager()
        {
            _rsaAsymetricEncryption = new RSA_AsymetricEncryption();
        }

        public void Initialize(RSAKeySetIdentity serveRsaKeySetIdentity)
        {
            if(serveRsaKeySetIdentity == null)
                return;

            _serveRsaKeySetIdentity = serveRsaKeySetIdentity;
            _initialized = true;
            ServerPublicKey = _serveRsaKeySetIdentity.RSA_PublicKey;

            _rsaParameters = _rsaAsymetricEncryption.ParseRSAKeyInfo(serveRsaKeySetIdentity);
        }

        public RSAParameters GetRsaParameters()
        {
            return _rsaParameters;
        }

        public string ServerPublicKey { get; private set; }
        public bool Initialized
        {
            get { return _initialized; }
        }

        public static ServerKeyManager KeyManager
        {
            get { return _instance ?? (_instance = new ServerKeyManager()); }
        }
    }
}

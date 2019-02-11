using System.Security.Cryptography;

namespace SecureChatServerModule.Models
{
    public class DHClientConfig
    {
        private byte[] _publicKey;
        private byte[] _sekretKey;
        private readonly byte[] _serverPublicKey;
        private bool _initialized;

        public DHClientConfig(byte[] serverPublicKey)
        {
            _serverPublicKey = serverPublicKey;
        }

        public void Initialize()
        {
            using (ECDiffieHellmanCng bob = new ECDiffieHellmanCng())
            {

                bob.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                bob.HashAlgorithm = CngAlgorithm.Sha256;
                _publicKey = bob.PublicKey.ToByteArray();
                _sekretKey = bob.DeriveKeyMaterial(CngKey.Import(_serverPublicKey, CngKeyBlobFormat.EccPublicBlob));
            }

            _initialized = true;
        }

        public bool Initialized
        {
            get { return _initialized; }
        }
    }
}

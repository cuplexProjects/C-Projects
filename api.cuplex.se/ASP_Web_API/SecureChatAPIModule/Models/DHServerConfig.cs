using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace SecureChatServerModule.Models
{
    public class DHServerConfig
    {
        private byte[] _publicKey;
        private bool _initialized;

        public DHServerConfig()
        {
        }

        public void GenerateKeys(byte[] receiverPublicKey)
        {
            if(_initialized)
                return;

            using (ECDiffieHellmanCng dhCng = new ECDiffieHellmanCng())
            {

                dhCng.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                dhCng.HashAlgorithm = CngAlgorithm.Sha256;
                _publicKey = dhCng.PublicKey.ToByteArray();
                byte[] clientKey = dhCng.DeriveKeyMaterial(CngKey.Import(receiverPublicKey, CngKeyBlobFormat.EccPublicBlob));
                _initialized = true;
            }
        }

        public bool Initialized
        {
            get { return _initialized; }
        }
        public byte[] PublicKey
        {
            get { return _publicKey; }
        }
        //public byte[] SecretKey
        //{
        //    get { return _sekretKey; }
        //}
    }
}
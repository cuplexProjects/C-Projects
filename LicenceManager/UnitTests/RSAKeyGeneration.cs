using GeneralToolkitLib.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class RSATest
    {
        [TestMethod]
        [Ignore]
        public void GenerateKeyPair()
        {
            RSA_AsymetricEncryption rsaAsymetricEncryption = new RSA_AsymetricEncryption();
            RSAKeySetIdentity keySetIdentity = rsaAsymetricEncryption.GenerateRSAKeyPair(RSA_AsymetricEncryption.RSAKeySize.b4096);

            string privateKey = keySetIdentity.RSA_PrivateKey;
            string publicKey = keySetIdentity.RSA_PublicKey;
        }
    }
}
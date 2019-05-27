using GeneralToolkitLib.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class RsaTest
    {
        [TestMethod]
        [Ignore]
        public void GenerateKeyPair()
        {
            RsaAsymetricEncryption rsaAsymetricEncryption = new RsaAsymetricEncryption();
            RsaKeySetIdentity keySetIdentity = rsaAsymetricEncryption.GenerateRsaKeyPair(RsaAsymetricEncryption.RsaKeySize.B4096);

            string privateKey = keySetIdentity.RsaPrivateKey;
            string publicKey = keySetIdentity.RsaPublicKey;
        }
    }
}
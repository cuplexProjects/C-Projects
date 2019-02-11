using System.Security.Cryptography;
using GeneralToolkitLib.Encryption;
using LicenceManagerLib.Licence;

namespace LicenceManagerLib.Encryption
{
    internal static class RSALocalCryptoKeyManager
    {
        public static RSAParameters GetAssemblyRsaParameters()
        {
            RSA_AsymetricEncryption rsaAsymetricEncryption = new RSA_AsymetricEncryption();
            RSAParameters rsaParameters = rsaAsymetricEncryption.ParseRSAKeyInfo(GetLocalKeySetIdentity());

            return rsaParameters;
        }

        private static RSAKeySetIdentity GetLocalKeySetIdentity()
        {
            RSAKeySetIdentity rsaKeySetIdentity = new RSAKeySetIdentity(LicenceGeneratorStaticData.PrivateKeys.GeneralToolkitLib,
               LicenceGeneratorStaticData.PublicKeys.GeneralToolkitLib);
            return rsaKeySetIdentity;
        }
    }
}
using System.Security.Cryptography;
using GeneralToolkitLib.Encryption;
using LicenceManagerLib.License;

namespace LicenceManagerLib.Encryption
{
    internal static class RSALocalCryptoKeyManager
    {
        public static RSAParameters GetAssemblyRsaParameters()
        {
            RsaAsymetricEncryption rsaAsymetricEncryption = new RsaAsymetricEncryption();
            RSAParameters rsaParameters = rsaAsymetricEncryption.ParseRsaKeyInfo(GetLocalKeySetIdentity());

            return rsaParameters;
        }

        private static RsaKeySetIdentity GetLocalKeySetIdentity()
        {
            RsaKeySetIdentity rsaKeySetIdentity = new RsaKeySetIdentity(LicenseGeneratorStaticData.PrivateKeys.GeneralToolkitLib,
               LicenseGeneratorStaticData.PublicKeys.GeneralToolkitLib);
            return rsaKeySetIdentity;
        }
    }
}
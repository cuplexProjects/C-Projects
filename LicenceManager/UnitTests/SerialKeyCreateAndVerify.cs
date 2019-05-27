using System;
using System.Linq;
using System.Security.Cryptography;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Encryption.Licence.DataModels;
using GeneralToolkitLib.Encryption.Licence.StaticData;
using LicenceManagerLib.License;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegKeyCreator.ApplicationKeys;


namespace UnitTests
{
    /// <summary>
    /// Summary description for SerialKeyCreateAndVerify
    /// </summary>
    [TestClass]
    public class SerialKeyCreateAndVerify
    {
        [TestMethod]
        public void GenerateRegistrationKey()
        {
            SerialNumberGenerator serialNumberGenerator = this.CreateSerialNumberGenerator();
            string serial = this.GenerateSerialFromModel(serialNumberGenerator);
            Assert.IsTrue(serial != null && serial.Length == 34, "Serial was not correct");
        }

        [TestMethod]
        public void VerifyRegistrationKey()
        {
            SerialNumberGenerator serialNumberGenerator = this.CreateSerialNumberGenerator();
            string serial = this.GenerateSerialFromModel(serialNumberGenerator);

            bool serialOk = serialNumberGenerator.VerifyRegistrationKey(serial);
            Assert.IsTrue(serialOk, "Serial was not correct");
        }

        private SerialNumberGenerator CreateSerialNumberGenerator()
        {
            var rsaPublicKeyIdentity = new RsaKeySetIdentity(null, RSAKeys.PublicKeys.GetKeyString());
            RsaAsymetricEncryption rsaAsymmetricEncryption = new RsaAsymetricEncryption();
            RSAParameters pubKeyParams = rsaAsymmetricEncryption.ParseRsaPublicKeyOnlyInfo(rsaPublicKeyIdentity);

            SerialNumberGenerator serialNumberGenerator = new SerialNumberGenerator(pubKeyParams, SerialNumbersSettings.ProtectedApp.SecureMemo);

            RegistrationDataModel registrationData = new RegistrationDataModel
            {
                Company = "Doe",
                Salt = GeneralConverters.GetRandomHexValue(256),
                ValidTo = DateTime.Now.AddYears(1),
                VersionName = LicenseGeneratorStaticData.SecureMemo.Versions.First()
            };
            serialNumberGenerator.LicenseData.RegistrationData = registrationData;

            return serialNumberGenerator;
        }

        private string GenerateSerialFromModel(SerialNumberGenerator serialNumberGenerator)
        {
            var rsaPrivateKeyIdentity = new RsaKeySetIdentity(RSAKeys.PrivateKeys.GetKeyString(), RSAKeys.PublicKeys.GetKeyString());
            var licenseKey = serialNumberGenerator.GenerateLicenseData(rsaPrivateKeyIdentity);

            return licenseKey;
        }
    }
}
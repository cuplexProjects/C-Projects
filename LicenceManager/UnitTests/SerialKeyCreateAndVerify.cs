using System;
using System.Linq;
using System.Security.Cryptography;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Encryption.Licence.DataModels;
using GeneralToolkitLib.Encryption.Licence.StaticData;
using LicenceManagerLib.Licence;
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
            SerialNumberGenerator SerialNumberGenerator = this.CreateSerialNumberGenerator();
            string serial = this.GenerateSerialFromModel(SerialNumberGenerator);
            Assert.IsTrue(serial != null && serial.Length == 34, "Serial was not correct");
        }

        [TestMethod]
        public void VerifyRegistrationKey()
        {
            SerialNumberGenerator SerialNumberGenerator = this.CreateSerialNumberGenerator();
            string serial = this.GenerateSerialFromModel(SerialNumberGenerator);

            bool serialOk = SerialNumberGenerator.VerifyRegistrationKey(serial);
            Assert.IsTrue(serialOk, "Serial was not correct");
        }

        private SerialNumberGenerator CreateSerialNumberGenerator()
        {
            var rsaPublicKeyIdentity = new RSAKeySetIdentity(null, RSAKeys.PublicKeys.SecureMEMO);
            RSA_AsymetricEncryption rsaAsymetricEncryption = new RSA_AsymetricEncryption();
            RSAParameters pubKeyParams = rsaAsymetricEncryption.ParseRSAPublicKeyOnlyInfo(rsaPublicKeyIdentity);

            SerialNumberGenerator SerialNumberGenerator = new SerialNumberGenerator(pubKeyParams, SerialNumbersSettings.ProtectedApp.SecureMemo);

            RegistrationDataModel registrationData = new RegistrationDataModel
            {
                Company = "Doe",
                Salt = GeneralConverters.GetRandomHexValue(256),
                ValidTo = DateTime.Now.AddYears(1),
                VersionName = LicenceGeneratorStaticData.SecureMemo.Versions.First()
            };
            SerialNumberGenerator.LicenceData.RegistrationData = registrationData;

            return SerialNumberGenerator;
        }

        private string GenerateSerialFromModel(SerialNumberGenerator SerialNumberGenerator)
        {
            var rsaPrivateKeyIdentity = new RSAKeySetIdentity(RSAKeys.PrivateKeys.SecureMEMO, RSAKeys.PublicKeys.SecureMEMO);
            var licenceKey = SerialNumberGenerator.GenerateLicenceData(rsaPrivateKeyIdentity);

            return licenceKey;
        }
    }
}
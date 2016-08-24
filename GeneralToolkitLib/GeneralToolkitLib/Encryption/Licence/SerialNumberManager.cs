using System;
using System.Security.Cryptography;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption.Licence.DataConverters;
using GeneralToolkitLib.Encryption.Licence.DataModels;
using GeneralToolkitLib.Encryption.Licence.StaticData;

namespace GeneralToolkitLib.Encryption.Licence
{
    public class SerialNumberManager
    {
        private const string KeyFormat = "XXXX-XXXX-XXXX-XXXX-XXXX-XXXX-XXXX"; //28 - 34
        private const int VALIDATION_HASH_LENTH = 512;
        private readonly SerialNumbersSettings.ProtectedApp _app;
        private readonly RSAParameters _rsaPubKey;
        private LicenceDataModel _licenceData;

        public LicenceDataModel LicenceData
        {
            get { return _licenceData; }
            set { _licenceData = value; }
        }

        public SerialNumberManager(RSAParameters pubRSAKey, SerialNumbersSettings.ProtectedApp app)
        {
            _app = app;
            _rsaPubKey = pubRSAKey;
            _licenceData = new LicenceDataModel();
        }

        public bool ValidateRegistrationData()
        {
            if (_licenceData == null)
                return false;

            if (string.IsNullOrEmpty(_licenceData.RegistrationKey) || _licenceData.ValidationHash == null || _licenceData.ValidationHash.Length != VALIDATION_HASH_LENTH)
                return false;

            byte[] licenceDataBytes = ObjectSerializer.SerializeDataContract(LicenceData.RegistrationData);
            return VerifySignedHash(licenceDataBytes, _licenceData.ValidationHash, _rsaPubKey);
        }

        public bool VerifyRegistrationKey(string registrationKey)
        {
            if (registrationKey == null || registrationKey.Length != KeyFormat.Length)
                return false;

            byte[] licenceDataBytes = ObjectSerializer.SerializeDataContract(LicenceData.RegistrationData);
            return registrationKey == CreateRegistrationKey(licenceDataBytes);
        }

        #region Conversion Methods

        private string CreateRegistrationKey(byte[] licenceBytes)
        {
            string regKeyData = GeneralConverters.ByteArrayToBase64(licenceBytes);
            regKeyData = SerialNumbersSettings.ProtectedApplications.SaltData.GeneralToolkit + regKeyData + SerialNumbersSettings.ProtectedApplications.SaltData.GeneralToolkit + this._app;
            byte[] buffer = null;
            EncryptionManager.EncodeString(ref buffer, regKeyData, "064lMPnLyjI6sqfXm5KhSE4R0FDU0AchClDyxpAWKkJgFyih59IkhX598sveO7vdbuEKgbEQjDRcLtx0FbcJtASEqHZE8bLX2CIq2LwYZC4OWZGWzx7dv0dxp1h6dcck");
            return ConvertToBase32SerialNumber(buffer);
        }

        private string ConvertToBase32SerialNumber(byte[] hashBytes)
        {
            byte[] halfHashBytes = SHA256Cng.Create().ComputeHash(hashBytes);
            string base32Str = Base32.ToBase32String(halfHashBytes);

            string key = "";
            for (int i = 0; i < base32Str.Length / 2; i += 4)
                key += base32Str.Substring(i, 4) + "-";

            return key.TrimEnd('-');
        }

        #endregion

        #region RSA Hash Methods

        public byte[] HashAndSign(byte[] encrypted, RSAParameters rsaPrivateParams)
        {
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider(4096);
            var hashAlgorithm = SHA1Managed.Create();

            rsaCSP.ImportParameters(rsaPrivateParams);

            byte[] hashedData = hashAlgorithm.ComputeHash(encrypted);
            return rsaCSP.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA1"));
        }

        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the  
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(4096);
                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider 
                // to specify the use of SHA1 for hashing. 
                return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        private static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the  
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider 
                // to specify the use of SHA1 for hashing. 
                return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        #endregion
    }
}
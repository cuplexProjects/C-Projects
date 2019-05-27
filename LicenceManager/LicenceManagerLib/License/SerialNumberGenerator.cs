using System;
using System.IO;
using System.Security.Cryptography;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Encryption.Licence.DataConverters;
using GeneralToolkitLib.Encryption.Licence.DataModels;
using GeneralToolkitLib.Encryption.Licence.StaticData;
using Serilog;

namespace LicenceManagerLib.License
{
    public class SerialNumberGenerator
    {
        private const string KeyFormat = "XXXX-XXXX-XXXX-XXXX-XXXX-XXXX-XXXX"; //28 - 34
        private const int VALIDATION_HASH_LENTH = 512;
        private readonly SerialNumbersSettings.ProtectedApp _app;
        private readonly RSAParameters _rsaPubKey;
        private LicenseDataModel _licenseData;

        public SerialNumberGenerator(RSAParameters pubRSAKey, SerialNumbersSettings.ProtectedApp app)
        {
            this._app = app;
            this._rsaPubKey = pubRSAKey;
            this._licenseData = new LicenseDataModel();
        }

        public LicenseDataModel LicenseData
        {
            get { return this._licenseData; }
            set { this._licenseData = value; }
        }

        public string GenerateLicenseData(RsaKeySetIdentity rsaPrivateKeySetIdentity)
        {
            RsaAsymetricEncryption rsaAsymetricEncryption = new RsaAsymetricEncryption();
            RSAParameters privateRSAKeyParameters = rsaAsymetricEncryption.ParseRsaKeyInfo(rsaPrivateKeySetIdentity);

            byte[] licenseDataBytes = ObjectSerializer.SerializeDataContract(this.LicenseData.RegistrationData);
            byte[] signedData = HashAndSignBytes(licenseDataBytes, privateRSAKeyParameters);

            this._licenseData.ValidationHash = signedData;
            this._licenseData.RegistrationKey = this.CreateRegistrationKey(licenseDataBytes);

            return this._licenseData.RegistrationKey;
        }

        public bool ValidateRegistrationData()
        {
            if (string.IsNullOrEmpty(this._licenseData.RegistrationKey) || this._licenseData.ValidationHash == null || this._licenseData.ValidationHash.Length != VALIDATION_HASH_LENTH)
                return false;

            byte[] licenseDataBytes = ObjectSerializer.SerializeDataContract(this.LicenseData.RegistrationData);
            return VerifySignedHash(licenseDataBytes, this._licenseData.ValidationHash, this._rsaPubKey);
        }

        public bool VerifyRegistrationKey(string registrationKey)
        {
            if (registrationKey == null || registrationKey.Length != KeyFormat.Length)
                return false;

            byte[] licenseDataBytes = ObjectSerializer.SerializeDataContract(this.LicenseData.RegistrationData);
            return registrationKey == this.CreateRegistrationKey(licenseDataBytes);
        }

        #region Conversion Methods

        private string CreateRegistrationKey(byte[] licenseBytes)
        {
            string regKeyData = GeneralConverters.ByteArrayToBase64(licenseBytes);
            regKeyData = LicenseGeneratorStaticData.SaltData.GeneralToolkit + regKeyData + LicenseGeneratorStaticData.SaltData.GeneralToolkit + this._app;
            byte[] buffer = new byte[0];
            EncryptionManager.EncodeString(ref buffer,regKeyData, regKeyData);
            return this.ConvertToBase32SerialNumber(buffer);
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

        public RegistrationDataModel ReadRegistrationDataFromFile(string filename)
        {
            FileStream fs = null;

            try
            {
                fs = File.OpenRead(filename);
                TextReader tr = new StreamReader(fs);
                string requestInBase64 = tr.ReadToEnd();
                fs.Close();
                return ObjectSerializer.DeserializeRegistrationDataFromString(requestInBase64);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to open {filename}");
            }
            finally
            {
                fs?.Close();
            }
            return null;
        }

        public void CreateLicenseFile(string filename)
        {
            FileStream fs = null;

            try
            {
                if (_licenseData == null)
                    return;

                fs = File.OpenWrite(filename);
                TextWriter textWriter = new StreamWriter(fs);

                textWriter.Write(Convert.ToBase64String(ObjectSerializer.SerializeDataContract((_licenseData))));
                textWriter.Flush();
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"CreateLicenseFile error for file: {filename}");
            }
            finally
            {
                fs?.Close();
            }
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

        public bool VerifyHash(RSAParameters rsaParams, byte[] signedData, byte[] signature)
        {
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            var hashAlgorithm = SHA1.Create();

            rsaCSP.ImportParameters(rsaParams);
            bool dataOK = rsaCSP.VerifyData(signedData, CryptoConfig.MapNameToOID("SHA1"), signature);
            byte[] hashedData = hashAlgorithm.ComputeHash(signedData);
            return rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA1"), signature);
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

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
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

        #region Not used

        private static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;

                //Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This only needs 
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.   
                    //OAEP padding is only available on Microsoft Windows XP or 
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        private static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;

                //Create a new instance of RSACryptoServiceProvider. 
                using (var RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs 
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.   
                    //OAEP padding is only available on Microsoft Windows XP or 
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        #endregion
    }
}
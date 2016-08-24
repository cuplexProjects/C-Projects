using System;
using System.IO;
using System.Security.Cryptography;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Log;
using SecureMemo.DataModels;

namespace SecureMemo.Services
{
    public class OTPConfigService : IDisposable
    {
        private static OTPConfigService _instance;
        private OTPSettings _otpSettings;
        private string PasswordHash { get; set; }
        const int SaltByteLength = 64;
        public bool Initialized { get; private set; }
        


        private OTPConfigService()
        {
            _otpSettings = null;
        }
        /// <summary>
        ///  Create a new instance of otpSettings
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Password hash</returns>
        public string Create(string password)
        {
            
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                byte[] salt1Bytes= new byte[SaltByteLength];
                byte[] salt2Bytes= new byte[SaltByteLength];
                byte[] passwordBytes = GeneralConverters.GetByteArrayFromString(password);

                randomNumberGenerator.GetBytes(salt1Bytes);
                randomNumberGenerator.GetBytes(salt2Bytes);

                PasswordHash = CreatePasswordHash(salt1Bytes, salt2Bytes, passwordBytes);
                _otpSettings = new OTPSettings(GeneralToolkitLib.OTP.Authenticator.GenerateKey(), salt1Bytes, salt2Bytes);
                Initialized = true;
                return PasswordHash;
            }
        }

        /*
            File structure
            Byte 1 to 4 = ConfigOffsetLength int32            
            Section2: Salt1 
            Section3: Salt2
            Section4: PasswordHash 64 bytes                                                
            section5: EncodedBlockSize 4 byte
            section6: Sha256 EncodedDataValidation 256 bytes
            section7: encodedBlock to EOF


            Byte 
        */

        public bool LoadSettings(string filename, string password)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(filename);
                int bytesRead = 0;

                var buffer = new byte[4];
                bytesRead = fs.Read(buffer, 0, 4);
                int configOffsetBytes = BitConverter.ToInt32(buffer, 0);
                if (configOffsetBytes <= 0 || configOffsetBytes > SaltByteLength*2 + 64)
                    throw new DataMisalignedException("configOffsetBytes is invalid");

                buffer = new byte[configOffsetBytes];
                bytesRead += fs.Read(buffer, 0, buffer.Length);

                byte[] salt1Bytes = new byte[SaltByteLength];
                byte[] salt2Bytes = new byte[SaltByteLength];
                byte[] passwordHashBytes = new byte[64];

                Buffer.BlockCopy(buffer, 0, salt1Bytes, 0, SaltByteLength);
                Buffer.BlockCopy(buffer, SaltByteLength, salt2Bytes, 0, SaltByteLength);
                Buffer.BlockCopy(buffer, SaltByteLength*2, passwordHashBytes, 0, passwordHashBytes.Length);

                buffer = new byte[4];
                bytesRead += fs.Read(buffer, 0, 4);
                int encodedBlockSize = BitConverter.ToInt32(buffer, 0);

                if (encodedBlockSize <= 0 || encodedBlockSize > fs.Length - bytesRead)
                    throw new DataMisalignedException("configOffsetBytes is invalid");

                buffer = new byte[encodedBlockSize];
                fs.Read(buffer, 0, buffer.Length);

                _otpSettings = new OTPSettings(null, salt1Bytes, salt2Bytes);
                if (_otpSettings.Decode(buffer, password))
                    Initialized = true;


            }
            catch (Exception ex)
            {
                LogWriter.LogError("Exception in LoadSettings()", ex);
                return false;
            }
            finally
            {
                fs?.Close();
            }
            return true;

        }

        public bool SaveSettings(string filename, string password)
        {
            if (_otpSettings == null)
                return false;

            FileStream fs = null;
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                fs = File.OpenWrite(filename);
                
                fs.Seek(4, 0);
                
                fs.Write(_otpSettings.Salt1, 0, _otpSettings.Salt1.Length);
                fs.Write(_otpSettings.Salt2, 0, _otpSettings.Salt2.Length);

                byte[] passwordBytes = GeneralConverters.GetByteArrayFromString(password);
                string passwordHash = CreatePasswordHash(_otpSettings.Salt1, _otpSettings.Salt2, passwordBytes);
                passwordBytes = GeneralConverters.GetByteArrayFromString(passwordHash);

                fs.Write(passwordBytes, 0, passwordBytes.Length);

                byte[] encodedBytes = _otpSettings.Encode(password);
                int encodedBlockSize = encodedBytes.Length;
                byte[] buffer = BitConverter.GetBytes(encodedBlockSize);

                fs.Write(buffer, 0, buffer.Length);
                fs.Write(encodedBytes, 0, encodedBytes.Length);

                // Write configOffsetBytes
                fs.Seek(0, SeekOrigin.Begin);
                int configOffsetBytes = _otpSettings.Salt1.Length + _otpSettings.Salt2.Length + passwordBytes.Length;
                buffer = BitConverter.GetBytes(configOffsetBytes);
                fs.Write(buffer, 0, buffer.Length);
                fs.Seek(0, SeekOrigin.End);
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Exception in SaveSettings()", ex);
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            finally
            {
                fs?.Close();
            }
            return true;
        }

        private string CreatePasswordHash(byte[] salt1Bytes, byte[] salt2Bytes, byte[]passwordBytes)
        {
            int hashDataLength = SaltByteLength * 2 + passwordBytes.Length;

            byte[] hashData = new byte[hashDataLength];

            Buffer.BlockCopy(salt1Bytes, 0, hashData, 0, salt1Bytes.Length);
            Buffer.BlockCopy(salt2Bytes, 0, hashData, SaltByteLength, salt2Bytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, hashData, SaltByteLength * 2, passwordBytes.Length);

            return GeneralToolkitLib.Hashing.SHA512.GetSHA512HashAsHexString(hashData);
        }

        public bool ValidatePassword(string password)
        {
            if (_otpSettings == null)
                throw new InvalidOperationException("OTPConfigService must be Initialized before password validation");

            byte[] passwordBytes = GeneralConverters.GetByteArrayFromString(password);
            return CreatePasswordHash(_otpSettings.Salt1, _otpSettings.Salt2, passwordBytes) == PasswordHash;
        }


        public static OTPConfigService Service => _instance ?? (_instance = new OTPConfigService());

        public void Dispose()
        {
            _instance = null;
        }
    }
}
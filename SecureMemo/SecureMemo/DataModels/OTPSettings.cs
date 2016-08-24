using System;
using System.IO;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Utility.RandomGenerator;
using SHA512 = System.Security.Cryptography.SHA512;

namespace SecureMemo.DataModels
{
    //[Serializable]
    //[DataContract(Name = "OTPSettings")]
    //public class OTPSettings
    //{
    //    protected OTPSettings()
    //    {

    //    }

    //    public OTPSettings(string sharedSecret)
    //    {
    //        SharedSecret = sharedSecret;
    //        CreatedTime = DateTime.Now;
    //        LastUseTime = DateTime.Now;
    //    }

    //    [DataMember(Name = "SharedSecret", Order = 1)]
    //    public string SharedSecret { get; protected set; }

    //    [DataMember(Name = "LastUseTime", Order = 2)]
    //    public DateTime LastUseTime { get; set; }

    //    [DataMember(Name = "CreatedTime", Order = 3)]
    //    public DateTime CreatedTime { get; protected set; }
    //}
    public class OTPSettings
    {
        public OTPSettings(string secret, byte[] salt1, byte[] salt2)
        {
            SharedSecret = secret;
            Salt1 = salt1;
            Salt2 = salt2;
        }

        public byte[] Salt1 { get; private set; }
        public byte[] Salt2 { get; private set; }
        public string SharedSecret { get; private set; }

        public byte[] Encode(string password)
        {
            var secureRandom = new SecureRandomGenerator();
            var msBlock = new MemoryStream();
            var msContent = new MemoryStream();
            int leftPaddingLength = secureRandom.GetRandomInt(64, 512);
            int rightPaddingLength = secureRandom.GetRandomInt(64, 512);
            byte[] sharedSecretBytes = GeneralConverters.GetByteArrayFromString(SharedSecret);
            
            byte[] buffer= BitConverter.GetBytes(leftPaddingLength);
            msBlock.Write(buffer, 0, buffer.Length);

            buffer = BitConverter.GetBytes(rightPaddingLength);
            msBlock.Write(buffer, 0, buffer.Length);

            buffer = BitConverter.GetBytes(leftPaddingLength + rightPaddingLength + sharedSecretBytes.Length);
            msBlock.Write(buffer, 0, buffer.Length);

            msBlock.Write(secureRandom.GetRandomData(leftPaddingLength), 0, leftPaddingLength);
            msBlock.Write(sharedSecretBytes, 0, sharedSecretBytes.Length);
            msBlock.Write(secureRandom.GetRandomData(rightPaddingLength), 0, rightPaddingLength);

            byte[] encodeBytes = msBlock.ToArray();

            encodeBytes = EncryptionManager.EncryptData(encodeBytes, password);
            byte[] hashBytes = SHA512.Create().ComputeHash(encodeBytes, 0, encodeBytes.Length);

            buffer = BitConverter.GetBytes(encodeBytes.Length);
            msContent.Write(buffer,0, buffer.Length);

            msBlock.WriteTo(msContent);

            buffer = BitConverter.GetBytes(hashBytes.Length);
            msContent.Write(buffer, 0, buffer.Length);
            msContent.Write(hashBytes, 0, hashBytes.Length);

            return msContent.ToArray();
        }

        public bool Decode(byte[] data, string password)
        {
            var msEncoded = new MemoryStream(data);
            byte[] buffer = new byte[4];

            msEncoded.Read(buffer, 0, 4);
            int encodedBytes = BitConverter.ToInt32(buffer, 0);

            buffer= new byte[encodedBytes];
            msEncoded.Read(buffer, 0, buffer.Length);
            byte[] decodedBytes = EncryptionManager.DecryptData(buffer, password);

            msEncoded.Read(buffer, 0, 4);
            int hashByteLength = BitConverter.ToInt32(buffer, 0);

            if (hashByteLength > 512)
                return false;

            byte[] hashBytesFromData = new byte[hashByteLength];
            msEncoded.Read(hashBytesFromData, 0, hashBytesFromData.Length);
            byte[] hashBytesFromDecodedDataBytes = SHA512.Create().ComputeHash(decodedBytes, 0, decodedBytes.Length);

            bool hashCompareResult = GeneralConverters.ByteArrayToBase64(hashBytesFromData) == GeneralConverters.ByteArrayToBase64(hashBytesFromDecodedDataBytes);
            if (!hashCompareResult)
                return false;

            var msDecoded= new MemoryStream(decodedBytes);
            buffer = new byte[4];

            msDecoded.Read(buffer, 0, buffer.Length);
            int leftPaddingLength = BitConverter.ToInt32(buffer, 0);
            
            msDecoded.Read(buffer, 0, buffer.Length);
            int rightPaddingLength = BitConverter.ToInt32(buffer, 0);

            msDecoded.Read(buffer, 0, buffer.Length);
            int totalLength = BitConverter.ToInt32(buffer, 0);

            byte[] decodedDataBlock=new byte[totalLength];
            msDecoded.Read(decodedDataBlock, 0, decodedDataBlock.Length);

            byte[] sharedSecretBytes= new byte[totalLength- leftPaddingLength- rightPaddingLength];
            Buffer.BlockCopy(decodedDataBlock, leftPaddingLength, sharedSecretBytes, 0, sharedSecretBytes.Length);

            SharedSecret = GeneralConverters.GetStringFromByteArray(sharedSecretBytes);

            return true;
        }

       

    }
}

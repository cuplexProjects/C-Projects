using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SecureChat.Common.Security
{
    public class RandomGenerator
    {
        private const string SPECIAL_CHAR_POOL = @"!@#&$%=?+-<>_-[]{}^*/\|";
        private const string STANDARD_CHAR_POOL = "abcdefghijklmnopqrstuvwxyz";
        private const string NUMERIC_CHAR_POOL = "0123456789";
        private const string ALPHANUMERIC_CHAR_POOL = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly string completeSpecialCharPool;

        static RandomGenerator()
        {
            if(completeSpecialCharPool == null)
            {
                completeSpecialCharPool = SPECIAL_CHAR_POOL + STANDARD_CHAR_POOL + STANDARD_CHAR_POOL.ToUpper() + NUMERIC_CHAR_POOL;
            }
        }

        public string GenerateRandomHexString(int length)
        {
            int noBytes = length;
            if(noBytes % 2 != 0)
                noBytes += 1;

            noBytes = noBytes / 2;
            byte[] rndData = new byte[noBytes];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(rndData);
            }
            return ConvertByteArrayToHexString(rndData, length);
        }

        private string ConvertByteArrayToHexString(IEnumerable<byte> data, int length)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte t in data)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString().Substring(0, length);
        }

        private string ConvertByteArrayToBase64String(byte[] data, int length)
        {
            return Convert.ToBase64String(data).Substring(0, length);
        }

        public string GenerateRandomBase64String(int length)
        {
            int noBytes = length;

            if(noBytes % 8 != 0)
                noBytes += (16 - (noBytes % 8));

            byte[] rndData = new byte[noBytes];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(rndData);
            }

            return ConvertByteArrayToBase64String(rndData, length);

        }

        public string GenerateRandomSpecialCharString(int length)
        {
            int noBytes = length * 4;
            byte[] rndData = new byte[noBytes];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(rndData);
            }

            StringBuilder sb = new StringBuilder();
            int poolLength = completeSpecialCharPool.Length;
            for (int i = 0; i < noBytes; i += 4)
            {
                int randomArrayPos = Math.Abs(BitConverter.ToInt32(rndData, i)) % poolLength;
                sb.Append(completeSpecialCharPool[randomArrayPos]);
            }

            return sb.ToString();
        }

        public string GenerateRandomNumericString(int length)
        {
            byte[] rndData = new byte[4096];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                StringBuilder sb = new StringBuilder();
                while(sb.Length < length)
                {
                    randomGenerator.GetBytes(rndData);
                    for (int i = 0; i < rndData.Length; i += 2)
                    {
                        UInt16 rndUint = BitConverter.ToUInt16(rndData, i);
                        sb.Append(rndUint % 10);

                        if(sb.Length == length)
                            break;
                    }
                }

                return sb.ToString();
            }
        }

        public string GetAlphanmumericRandom(int length)
        {
            int noBytes = length * 4;
            byte[] rndData = new byte[noBytes];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(rndData);
            }

            StringBuilder sb = new StringBuilder();
            int poolLength = ALPHANUMERIC_CHAR_POOL.Length;
            for (int i = 0; i < noBytes; i += 4)
            {
                int randomArrayPos = Math.Abs(BitConverter.ToInt32(rndData, i)) % poolLength;
                sb.Append(ALPHANUMERIC_CHAR_POOL[randomArrayPos]);
            }

            return sb.ToString();
        }

        public byte[] GenerateRandomByteArray(int noBytes)
        {
            byte[] rndData = new byte[noBytes];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(rndData);
            }

            return rndData;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FileGetDbLib
{
    public class SHA2Hash
    {
        public static string GetSHA2Hash(HashBits bits, string message)
        {
            HashAlgorithm ha = null;

            switch (bits)
            {
                case HashBits.n256:
                    ha = SHA256.Create();
                    break;
                case HashBits.n384:
                    ha = SHA384.Create();
                    break;
                case HashBits.n512:
                    ha = SHA512.Create();
                    break;
            }

            byte[] data = ha.ComputeHash(Encoding.Default.GetBytes(message));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }
        public enum HashBits
        {
            n256,
            n384,
            n512,
        }
    }
    public class RndGenerator
    {
        public RndGenerator()
        {
        }
        public static string GetRandomHex(int length)
        {
            RandomNumberGenerator r = RNGCryptoServiceProvider.Create();
            byte[] data = new byte[length];
            r.GetBytes(data);

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }
    }
}
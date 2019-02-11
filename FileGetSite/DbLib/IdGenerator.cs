using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace FileGetDbLib
{
    public class IdGenerator
    {
        public IdGenerator()
        {
            //
            // TODO: Add constructor logic here
            //

        }
        // 64 chars random string
        public static string GenerateFileId()
        {
            RandomNumberGenerator gen = RandomNumberGenerator.Create();
            string pool = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            byte[] buffer = new byte[256];
            StringBuilder sb = new StringBuilder();

            gen.GetBytes(buffer);

            for (int i = 0; i < buffer.Length; i += 4)
            {
                int rndVal = Math.Abs(BitConverter.ToInt32(buffer, i));
                sb.Append(pool[rndVal % pool.Length]);
            }
            return sb.ToString();
        }
    }
}
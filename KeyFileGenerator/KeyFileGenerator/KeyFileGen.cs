using System;
using System.IO;
using System.Security.Cryptography;

namespace KeyFileGenerator
{
    class KeyFileGen
    {
        private const int MAX_MEM_ALOC = 268435456; //256 mb
        public static void GenerateKeyFile(string path, Int64 bytesToGenerate)
        {
            RandomNumberGenerator rnd = RandomNumberGenerator.Create();
            FileStream fs = File.OpenWrite(path);
            fs.SetLength(0);

            int bufferSize = 0;
            bufferSize = bytesToGenerate > Int32.MaxValue ? MAX_MEM_ALOC : Math.Min(MAX_MEM_ALOC, (int)bytesToGenerate);

            byte[] buffer = new byte[bufferSize];
            Int64 bytesGenerated = 0;

            while (bytesGenerated < bytesToGenerate)
            {
                rnd.GetBytes(buffer);
                bytesGenerated += buffer.Length;

                if(bytesGenerated > bytesToGenerate)
                {
                    fs.Write(buffer, 0, (int)(bytesToGenerate - bytesGenerated));
                }
                else
                {
                    fs.Write(buffer, 0, buffer.Length);    
                }
            }
            fs.Close();
            buffer = null;
            GC.Collect();
        }        
    }
}

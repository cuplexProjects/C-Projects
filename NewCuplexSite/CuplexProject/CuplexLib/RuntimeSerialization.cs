using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CuplexLib
{
    public class RuntimeSerialization //Martin Dahl 2009-10-13
    {
        public static string SerializeDataStructure(ISerializable serializableDataStructure)
        {
            MemoryStream ms = new MemoryStream();
            MemoryStream ms2 = new MemoryStream();
            StringCompressor sc = new StringCompressor();
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(ms, serializableDataStructure);
                sc.Compress(ms, ms2);

                return Convert.ToBase64String(ms2.ToArray());
            }
            catch { return null; }
        }

        public static object DeSerializeDataStructure(string serializationString)
        {
            byte[] serializationData = Convert.FromBase64String(serializationString);
            MemoryStream ms = new MemoryStream();
            MemoryStream ms2 = new MemoryStream();
            StringCompressor sc = new StringCompressor();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                ms.Write(serializationData, 0, serializationData.Length);
                sc.Decompress(ms, ms2);
                ms2.Position = 0;

                return formatter.Deserialize(ms2);
            }
            catch { return null; }

        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }
        public static string ByteArrayToStr(byte[] byteArray)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetString(byteArray);
        }
        internal class StringCompressor
        {
            public StringCompressor()
            {
            }
            public void Compress(Stream inputStream, Stream outputStream)
            {
                byte[] buffer = new byte[262144];
                int bytesRead;
                inputStream.Position = 0;
                outputStream.Position = 0;
                GZipStream compressedzipStream = new GZipStream(outputStream, CompressionMode.Compress, true);

                do
                {
                    bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                    compressedzipStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);
                compressedzipStream.Close();
            }
            public void Decompress(Stream inputStream, Stream outputStream)
            {
                byte[] buffer = new byte[262144];
                int bytesRead;
                inputStream.Position = 0;
                GZipStream compressedzipStream = new GZipStream(inputStream, CompressionMode.Decompress, true);

                do
                {
                    bytesRead = compressedzipStream.Read(buffer, 0, buffer.Length);
                    outputStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);
                compressedzipStream.Close();
            }
        }
    }
}

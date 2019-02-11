using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SynchronizeTime.Common
{
    public class GeneralConverters
    {
        internal struct StaticLongFileSizes
        {
             public static string[] FileSizeTypesStrings = { "b", "kB", "Mb", "Gb", "Tb", "Pbyte" };
        }

        public static int GetSecondsFromDateTime(DateTime date)
        {
            return (date.Hour * 3600) + (date.Minute * 60) + date.Second;
        }

        public static string GetFileNameFromPath(string path)
        {
            if(string.IsNullOrEmpty(path))
                throw new ArgumentException("Path can not be null or empty");

            int iPos = path.LastIndexOf('\\') + 1;
            if (iPos > 1 && iPos != path.Length)
                return path.Substring(iPos, path.Length - iPos);
            
            return path;
        }

        public static string GetDirectoryNameFromPath(string path, bool trailingSlash = true)
        {
            int lastBackSlash = path.LastIndexOf('\\');
            
            if (lastBackSlash > 0)
            {
                if (trailingSlash)
                    lastBackSlash++;
                return path.Substring(0, lastBackSlash);
            }
            return path;
        }

        public static DateTime GetDateTimeFromUInt(UInt32 u32Date)
        {
            DateTime converteDateTime = new DateTime(1900, 01, 01).AddSeconds(Convert.ToDouble(u32Date));

            return converteDateTime;
        }

        public static string FormatFileSizeToString(long fileSize)
        {
            var fileSizeType = StaticLongFileSizes.FileSizeTypesStrings;
            int iCnt = 0;
            decimal dblRes = Convert.ToDecimal(fileSize);
            while (dblRes > 1024)
            {
                iCnt++;
                dblRes = dblRes / 1024;
            }
            dblRes = Math.Round(dblRes, 2);
            if (iCnt < fileSizeType.Length)
                return dblRes.ToString() + " " + fileSizeType[iCnt];

            return dblRes.ToString();
        }

        public static byte[] StringToByteArray(string inputString)
        {
            return Encoding.UTF8.GetBytes(inputString);
        }

        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
                sb.AppendFormat("{0:X2}", b);

            return sb.ToString();
        }

        public static string GetRandomHexValue(int length)
        {
            if(length < 2)
                throw new ArgumentException("Data length must be atleast 2");

            using (var rndGen = RNGCryptoServiceProvider.Create())
            {
                byte[] buffer = new byte[length / 2];
                rndGen.GetBytes(buffer);
                return ByteArrayToHexString(buffer);
            }
        }

        public static string FileAttributesToString(FileAttributes fileAttributes)
        {
            string attributes = "";
            if(fileAttributes.HasFlag(FileAttributes.ReadOnly))
                attributes += "R";
            if (fileAttributes.HasFlag(FileAttributes.Hidden))
                attributes += "H";
            if (fileAttributes.HasFlag(FileAttributes.System))
                attributes += "S";
            if (fileAttributes.HasFlag(FileAttributes.Directory))
                attributes += "D";
            if (fileAttributes.HasFlag(FileAttributes.Archive))
                attributes += "A";
            if (fileAttributes.HasFlag(FileAttributes.Device))
                attributes += " DV ";
            if (fileAttributes.HasFlag(FileAttributes.Normal))
                attributes += "";
            if (fileAttributes.HasFlag(FileAttributes.Temporary))
                attributes += "TMP";
            if (fileAttributes.HasFlag(FileAttributes.Compressed))
                attributes += "C";
            if (fileAttributes.HasFlag(FileAttributes.Encrypted))
                attributes += " ENC ";

            return attributes.Trim();
        }

        public static class FileSizeToStringFormater
        {
            internal static readonly string[] FileSizeTypesStrings = { " B", " KB", " MB", " GB", " TB", " PByte" };
            
            public enum FileSizeSteps : long
            {
                Byte = 0x0,
                KiloByte = 0x400,
                MegaByte = 0x100000,
                GigaByte = 0x40000000,
                PetaByte = 0x10000000000,
            }

            public struct OffsetRange
            {
                public FileSizeSteps FileSizeStep;
                public long Max;
            }

            public static string ConvertFileSizeToString(long fileSize)
            {
                if(fileSize > (long)FileSizeSteps.KiloByte)
                {
                    int arrayPos = 0;
                    do
                    {
                        fileSize = fileSize >> 0xA;
                        arrayPos++;
                    } while (fileSize > 0x400);
                    return fileSize + FileSizeTypesStrings[arrayPos];
                }
                return fileSize + FileSizeTypesStrings[0];
            }

            public static string ConvertFileSizeToString(long fileSize, int decimals)
            {
                if (fileSize > (long)FileSizeSteps.KiloByte)
                {
                    if(decimals > 8 || decimals < 0)
                        decimals = 8;

                    double decData = fileSize;
                    int arrayPos = 0;
                    do
                    {
                        decData = decData/1024d;
                        arrayPos++;
                    } while (decData > 0x400);


                    return Math.Round(decData, decimals) + FileSizeTypesStrings[arrayPos];
                }
                return fileSize + FileSizeTypesStrings[0];
            }

            public static string ConvertFileSizeToString(long fileSize, ushort decimales, params OffsetRange[] offsetRanges)
            {
                throw new NotImplementedException();
            }
        }
    }
}

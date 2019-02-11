using System;

namespace PrefetchFile
{
    public static class FileSizeToStringFormater
    {
        private static readonly string[] FileSizeTypesStrings = { " B", " KB", " MB", " GB", " TB", " PByte" };

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
            if (fileSize > (long)FileSizeSteps.KiloByte)
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
                if (decimals > 8 || decimals < 0)
                    decimals = 8;

                double decData = fileSize;
                int arrayPos = 0;
                do
                {
                    decData = decData / 1024d;
                    arrayPos++;
                } while (decData > 0x400);


                return Math.Round(decData, decimals) + FileSizeTypesStrings[arrayPos];
            }
            return fileSize + FileSizeTypesStrings[0];
        }
    }
}
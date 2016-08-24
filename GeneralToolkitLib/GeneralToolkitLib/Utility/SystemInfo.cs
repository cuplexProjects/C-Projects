using System;
using System.Linq;
using System.Management;
using System.Text;
using GeneralToolkitLib.Log;

namespace GeneralToolkitLib.Utility
{
    internal static class SystemInfo
    {
        #region -> Private Variables

        public static bool UseProcessorID;
        public static bool UseBaseBoardProduct;
        public static bool UseBaseBoardManufacturer;
        public static bool UseDiskDriveSignature;
        public static bool UseVideoControllerCaption;
        public static bool UsePhysicalMediaSerialNumber;
        public static bool UseBiosVersion;
        public static bool UseBiosManufacturer;
        public static bool UseWindowsSerialNumber;

        #endregion

        public static string GetSystemInfo(string SoftwareName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SoftwareName);

            if (UseProcessorID)
                sb.AppendLine(RunQuery("Processor", "ProcessorId"));

            if (UseBaseBoardProduct)
                sb.AppendLine(RunQuery("BaseBoard", "Product"));

            if (UseBaseBoardManufacturer)
                sb.AppendLine(RunQuery("BaseBoard", "Manufacturer"));

            if (UseDiskDriveSignature)
                sb.AppendLine(RunQuery("DiskDrive", "Signature"));

            if (UseVideoControllerCaption)
                sb.AppendLine(RunQuery("VideoController", "Caption"));

            if (UsePhysicalMediaSerialNumber)
                sb.AppendLine(RunQuery("PhysicalMedia", "SerialNumber"));

            if (UseBiosVersion)
                sb.AppendLine(RunQuery("BIOS", "Version"));

            if (UseWindowsSerialNumber)
                sb.AppendLine(RunQuery("OperatingSystem", "SerialNumber"));

            return sb.ToString();
        }

        private static string RunQuery(string TableName, string MethodName)
        {
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_" + TableName);
            foreach (var MO in MOS.Get().Cast<ManagementObject>())
            {
                try
                {
                    return MO[MethodName].ToString();
                }
                catch (Exception e)
                {
                    LogWriter.LogError("SystemInfo.RunQuery()", e);
                }
            }
            return "";
        }
    }
}
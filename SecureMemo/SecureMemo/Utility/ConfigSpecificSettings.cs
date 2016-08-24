using System;
using System.Reflection;

namespace SecureMemo.Utility
{
    internal static class ConfigSpecificSettings
    {
        public static string GetSettingsFolderPath(bool apppendBackslash)
        {
#if DEBUG
            return Environment.CurrentDirectory + "\\" + Assembly.GetExecutingAssembly().GetName().Name + (apppendBackslash ? "\\" : "");
#else
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name + (apppendBackslash ? "\\" : "");
#endif
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace SetTimeAndDate
{
    public class RegistrySettings
    {
        private const string basePath = "MD Software";
        private const string appName = "SetTimeAndDate";

        public static void Write(string keyName, string value)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);

            key.CreateSubKey(basePath);
            key = key.OpenSubKey(basePath, true);

            key.CreateSubKey(appName);
            key = key.OpenSubKey(appName, true);

            key.SetValue(keyName, value);
        }

        public static string Read(string keyName)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
            
            key.CreateSubKey(basePath);
            key = key.OpenSubKey(basePath, true);

            key.CreateSubKey(appName);
            key = key.OpenSubKey(appName, true);

            return key.GetValue(keyName) as string;
        }
    }
}

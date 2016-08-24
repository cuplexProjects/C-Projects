using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Microsoft.Win32;

namespace LoopiaDNSTools.ServiceInstaller
{
    internal static class ServiceHelper
    {
        public static string[] GetServicesWithPath(string path)
        {
            var services = new List<string>();
            using (RegistryKey rkLm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            {
                using (RegistryKey keyStart = rkLm.OpenSubKey(@"System\CurrentControlSet\Services\"))
                {
                    foreach (string serviceName in keyStart.GetSubKeyNames())
                    {
                        using (RegistryKey serviceKey = keyStart.OpenSubKey(serviceName))
                        {
                            string imagePath = (serviceKey.GetValue("ImagePath") as string) ?? "";
                            if (imagePath.Contains(path))
                                services.Add(serviceName);
                        }
                    }
                }
            }
            return services.ToArray();
        }

        public static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            var sc = new ServiceController(serviceName);
            return sc.Status;
        }

        public static string GetInstalledServiceName()
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            return GetServicesWithPath(assemblyPath).FirstOrDefault();
        }
    }
}
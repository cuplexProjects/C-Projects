using System.Collections.Generic;
using System.ServiceProcess;

namespace SynchronizeTime.ServiceConfig
{
    public static class ServiceHelper
    {
        public static string[] GetServicesWithPath(string path)
        {
            List<string> services = new List<string>();
            using (var rkLm = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Default))
            {
                using (var keyStart = rkLm.OpenSubKey(@"System\CurrentControlSet\Services\"))
                {
                    foreach (var serviceName in keyStart.GetSubKeyNames())
                    {
                        using (var serviceKey = keyStart.OpenSubKey(serviceName))
                        {
                            var imagePath = (serviceKey.GetValue("ImagePath") as string) ?? "";
                            if (imagePath.Contains(path))
                            {
                                services.Add(serviceName);
                            }
                        }
                    }
                }
            }
            return services.ToArray();
        }

        public static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            return sc.Status;
        }
    }
}

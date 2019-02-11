using System;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;

namespace SynchronizeTime.ServiceConfig
{
    [RunInstaller(true)]
    public class SynchronizeTimeServiceInstaller : Installer
    {
        public SynchronizeTimeServiceInstaller(string serviceName)
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            string text;

            ServiceAccount account = ServiceAccount.LocalSystem;
            text = ConfigurationManager.AppSettings["ServiceAccount"] ?? "LocalSystem";
            if (text != null)
                account = (ServiceAccount)Enum.Parse(typeof(System.ServiceProcess.ServiceAccount), text);

            if (string.IsNullOrEmpty(serviceName))
                serviceName = ConfigurationManager.AppSettings["ServiceName"] ?? "SecoService";
            if (serviceName == null)
                throw new Exception("No ServiceName set");

            string[] servicesDependedOn;
            text = ConfigurationManager.AppSettings["ServicesDependedOn"] ?? null;
            if (text != null)
                servicesDependedOn = text.Split(',');
            else
                servicesDependedOn = new string[0];

            ServiceStartMode startType = ServiceStartMode.Manual;
            text = ConfigurationManager.AppSettings["ServiceStartType"] ?? "Manual";
            if (text != null)
                startType = (ServiceStartMode)Enum.Parse(typeof(ServiceStartMode), text);

            processInstaller.Account = account;
            serviceInstaller.ServiceName = serviceName;
            serviceInstaller.DisplayName = serviceName;
            serviceInstaller.ServicesDependedOn = servicesDependedOn;
            serviceInstaller.StartType = startType;

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}

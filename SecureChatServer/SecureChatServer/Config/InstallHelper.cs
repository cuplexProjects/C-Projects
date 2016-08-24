using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace SecureChatServer.Config
{
    static class InstallHelper
    {
        internal static void RunInstaller(bool install, string serviceName)
        {
            TransactedInstaller tinstaller = new TransactedInstaller();
            SecureChatServiceInstaller cfinstaller = new SecureChatServiceInstaller(serviceName);
            tinstaller.Installers.Add(cfinstaller);
            tinstaller.Context = new InstallContext("", new string[] {string.Format("/assemblypath={0}", Assembly.GetExecutingAssembly().Location)});
            ;
            if(install)
            {
                tinstaller.Install(new Hashtable());
            }
            else
            {
                tinstaller.Uninstall(null);
            }
        }

        [RunInstaller(true)]
        class SecureChatServiceInstaller : Installer
        {
            public SecureChatServiceInstaller(string serviceName)
            {
                ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
                ServiceInstaller serviceInstaller = new ServiceInstaller();

                ServiceAccount account = ServiceAccount.LocalSystem;
                string text = ConfigurationManager.AppSettings["ServiceAccount"] ?? "LocalSystem";
                if (text != null)
                    account = (ServiceAccount)Enum.Parse(typeof(ServiceAccount), text);

                if (string.IsNullOrEmpty(serviceName))
                    serviceName = ConfigurationManager.AppSettings["ServiceName"] ?? Constants.SERVICE_DEFAULT_NAME;
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
}
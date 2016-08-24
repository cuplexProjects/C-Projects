using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace SecureChatServer
{
    static class Program
    {
        static Mutex installerMutex;
        static Mutex serviceMutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // Normal exe launch 
            if (Environment.UserInteractive)
            {
                if (!IsSingleInstanceInstaller())
                {
                    MessageBox.Show("Another instance detected, terminating app.");
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new InstallerForm());
                return;
            }

            // Service launch
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SecureChatSrvImpl() 
            };
            if (IsSingleInstanceService())
                ServiceBase.Run(ServicesToRun);
        }

        static bool IsSingleInstanceInstaller()
        {
            try
            {
                Mutex.OpenExisting("SecureChatServiceInstaller");
            }
            catch
            {
                installerMutex = new Mutex(true, "SecureChatServiceInstaller");
                return true;
            }
            return false;
        }

        static bool IsSingleInstanceService()
        {
            try
            {
                Mutex.OpenExisting("SecureChatService");
            }
            catch
            {
                serviceMutex = new Mutex(true, "SecureChatService");
                return true;
            }
            return false;
        }
    }
}
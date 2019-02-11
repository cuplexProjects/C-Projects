using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace DataCopyService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if(Environment.UserInteractive)
            {
                Application.EnableVisualStyles();
                Application.Run(new SettingsForm());
                return;
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

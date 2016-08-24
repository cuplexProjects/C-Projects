using System;
using System.Windows.Forms;
using GeneralToolkitLib.ConfigHelper;

namespace FileSystemImage
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //Set log path
            GlobalSettings.Initialize(Application.ProductName, true);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
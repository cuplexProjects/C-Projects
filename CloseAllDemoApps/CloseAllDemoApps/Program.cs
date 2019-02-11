using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace CloseAllDemoApps
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        private static Mutex singleInstanceMutex;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
//#if DEBUG
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new FormMain());
//            return;
//#endif
            if (!IsSingleInstance())
            {
                FindAndFocusExistingInstance();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormMain formMain = new FormMain();
            formMain.FormClosed += formMain_FormClosed;
            Application.Run(formMain);
        }

        static void formMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            singleInstanceMutex.ReleaseMutex();
        }

        private static bool IsSingleInstance()
        {
            try
            {
                Mutex.OpenExisting("CloseAllDemoAppsSingleInstranceMutex");
            }
            catch
            {
                singleInstanceMutex = new Mutex(true, "CloseAllDemoAppsSingleInstranceMutex");
                return true;
            }
            return false;
        }
        private static void FindAndFocusExistingInstance()
        {
            var processList = Process.GetProcessesByName(Assembly.GetCallingAssembly().GetName().Name);
            if (processList.Length <= 0) return;
            IntPtr winIntPtr = processList[0].MainWindowHandle;
            SwitchToThisWindow(winIntPtr, true);
        }
    }
}
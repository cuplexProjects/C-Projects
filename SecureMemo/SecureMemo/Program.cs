using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;

namespace SecureMemo
{
    internal static class Program
    {
        private const string singleInstanceMutexname = "SecureMemoInstance";
        private static Mutex instanceMutex;

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if DEBUG
            GlobalSettings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, false);
            LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Debug);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            return;
#else
            GlobalSettings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, true);
            LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Warning);
#endif

#pragma warning disable 162
            if (!IsFirstInstance())
            {
                FindAndFocusExistingInstance();
                return;
            }

            RegisterMutex();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            ReleaseMutex();
#pragma warning restore 162
        }

        private static void RegisterMutex()
        {
            instanceMutex = new Mutex(true, singleInstanceMutexname);
        }

        private static void ReleaseMutex()
        {
            if (instanceMutex == null) return;
            instanceMutex.GetAccessControl();
            instanceMutex.WaitOne();
            instanceMutex.ReleaseMutex();
        }

        private static bool IsFirstInstance()
        {
            return !Mutex.TryOpenExisting(singleInstanceMutexname, out instanceMutex);
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
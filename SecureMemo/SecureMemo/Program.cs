using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;
using SecureMemo.Configuration;

namespace SecureMemo
{
    internal static class Program
    {
        private const string SingleInstanceMutexname = "SecureMemoInstance";
        private static Mutex _instanceMutex;
        private static IContainer Container { get; set; }

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            InitializeAutofac();
#if DEBUG
            GlobalSettings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, false);
            LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Debug);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var scope = Container.BeginLifetimeScope())
            {
                FormMain frmMain = scope.Resolve<FormMain>();
                Application.Run(frmMain);
            }

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
            using (var scope = Container.BeginLifetimeScope())
            {
                FormMain frmMain = scope.Resolve<FormMain>();
                Application.Run(frmMain);
            }
            ReleaseMutex();
#pragma warning restore 162
        }

        private static void RegisterMutex()
        {
            _instanceMutex = new Mutex(true, SingleInstanceMutexname);
        }

        private static void ReleaseMutex()
        {
            if (_instanceMutex == null) return;
            _instanceMutex.GetAccessControl();
            _instanceMutex.WaitOne();
            _instanceMutex.ReleaseMutex();
        }

        private static bool IsFirstInstance()
        {
            return !Mutex.TryOpenExisting(SingleInstanceMutexname, out _instanceMutex);
        }

        private static void FindAndFocusExistingInstance()
        {
            var processList = Process.GetProcessesByName(Assembly.GetCallingAssembly().GetName().Name);
            if (processList.Length <= 0) return;
            IntPtr winIntPtr = processList[0].MainWindowHandle;
            SwitchToThisWindow(winIntPtr, true);
        }

        private static void InitializeAutofac()
        {
            Container = AutofacConfig.CreateContainer();
        }
    }
}
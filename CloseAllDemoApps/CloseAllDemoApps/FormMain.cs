using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CloseAllDemoApps.Models;
using CloseAllDemoApps.Storage.Registry;

namespace CloseAllDemoApps
{
    public partial class FormMain : Form, IEventLogSource
    {
        private delegate void RefreshListDelegate(int pid);
        private readonly RefreshListDelegate _refreshListDelegate;
        private readonly List<Process> _waitingForTerminateProcessList;
        private readonly object _processListLockObj;
        private readonly object _eventLogListLockObj;
        private bool _runcloseProcessesThread = true;
        private readonly ManualResetEvent _closeProcessesThreadResetEvent;
        private readonly List<string> _eventLogList = new List<string>();
        public event EventHandler OnEventLogUpdated;
        private ApplicationSettings applicationSettings;

        public List<Process> WaitingForTerminateProcessList
        {
            get
            {
                lock (_processListLockObj)
                {
                    return _waitingForTerminateProcessList;
                }
            }
        }

        public List<string> EventLogList
        {
            get
            {
                lock (_eventLogListLockObj)
                {
                    return _eventLogList;
                }
            }
        }

        public FormMain()
        {
            InitializeComponent();
            _waitingForTerminateProcessList = new List<Process>();
            _refreshListDelegate = RemoveItemFromProcessList;
            _processListLockObj = new object();
            _eventLogListLockObj = new object();
            _closeProcessesThreadResetEvent = new ManualResetEvent(true);
            Thread closeProcessesThread = new Thread(CloseSelectedProcesses);
            closeProcessesThread.Start();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            RegistryAccess registryAccess = new RegistryAccess(Application.ProductName);
            try
            {
                applicationSettings = registryAccess.ReadObjectFromRegistry<ApplicationSettings>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                applicationSettings = new ApplicationSettings();
            }

            UpdateGUIFromSettings();

            sysTrayNotifyIcon.BalloonTipText = @"Application is now minimized to system tray";
            sysTrayNotifyIcon.BalloonTipTitle = @"Minimized to system tray";
            sysTrayNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
        }

        private void UpdateGUIFromSettings()
        {
            txtProcessDescriptionFilter.Text = applicationSettings.ProcessDescriptionFilter;
        }

        private void SaveApplicationSettings()
        {
            RegistryAccess registryAccess = new RegistryAccess(Application.ProductName);
            applicationSettings.ProcessDescriptionFilter = txtProcessDescriptionFilter.Text;

            try
            {
                registryAccess.SaveObjectToRegistry(applicationSettings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RemoveItemFromProcessList(int pid)
        {
            var dataSource = ProcessListbox.DataSource as List<ProcessListboxItem>;
            if (dataSource != null)
            {
                dataSource.RemoveAll(p => p.PID == pid);
                ProcessListbox.DataSource = dataSource;
            }
        }

        private void RefreshData()
        {
            string filter = txtProcessDescriptionFilter.Text;

            try
            {
                Process[] localAll = Process.GetProcesses();
                ProcessListbox.DataSource = null;
                var processList = (from process in localAll
                    where filter == "" || process.ProcessName.Contains(filter)
                    select new ProcessListboxItem
                    {
                        PID = process.Id, MainWindowTitle = process.MainWindowTitle, ProcessName = process.ProcessName
                    }).ToList();

                ProcessListbox.DataSource = processList;
                SelectAllProcesses();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectAllProcesses()
        {
            for (int i = 0; i < ProcessListbox.Items.Count; i++)
            {
                ProcessListbox.SetSelected(i, true);
            }
        }

        private void btnTerminate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in ProcessListbox.SelectedItems)
                {
                    ProcessListboxItem listItem = item as ProcessListboxItem;
                    if (listItem == null) 
                        continue;

                    Process localById = Process.GetProcessById(listItem.PID);

                    if (!WaitingForTerminateProcessList.Contains(localById))
                        WaitingForTerminateProcessList.Add(localById);
                }
                _closeProcessesThreadResetEvent.Set();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseSelectedProcesses()
        {
            while (_runcloseProcessesThread)
            {
                _closeProcessesThreadResetEvent.Reset();
                _closeProcessesThreadResetEvent.WaitOne();

                var terminatedPidList = new List<int>();

                foreach (var process in WaitingForTerminateProcessList)
                {
                    try
                    {
                        if (!process.CloseMainWindow())
                            process.Kill();

                        terminatedPidList.Add(process.Id);
                        RefreshDataInvoker(process.Id);
                        AddEventLog("Terminated process: " + process.ProcessName);
                    }
                    catch (Exception ex)
                    {
                        AddEventLog(ex.Message);
                    }
                }

                WaitingForTerminateProcessList.RemoveAll(p => terminatedPidList.Contains(p.Id));
            }
        }

        private void AddEventLog(string message)
        {
            EventLogList.Add(DateTime.Now + " : " + message);
            if (OnEventLogUpdated != null)
                OnEventLogUpdated.Invoke(this, new EventArgs());
        }

        private void RefreshDataInvoker(int pid)
        {
            Invoke(_refreshListDelegate, pid);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _runcloseProcessesThread = false;
            _closeProcessesThreadResetEvent.Set();
        }

        public List<string> GetLogsList()
        {
            return EventLogList;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveApplicationSettings();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                sysTrayNotifyIcon.Visible = true;
                sysTrayNotifyIcon.ShowBalloonTip(500);
                ShowInTaskbar = false;
            }

            else if (FormWindowState.Normal == WindowState)
            {
                sysTrayNotifyIcon.Visible = false;
                ShowInTaskbar = true;
            }
        }

        private void sysTrayNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            sysTrayNotifyIcon.Visible = false;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new FormSettings().ShowDialog(this);
        }
    }
}
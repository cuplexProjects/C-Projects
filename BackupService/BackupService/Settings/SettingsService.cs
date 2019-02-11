using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.ComponentModel;
using BackupService.Storage;
using System.Windows.Forms;
using BackupService.Utils;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;

namespace BackupService.Settings
{
    public class SettingsService
    {
        private static SettingsService _instance;
        private readonly BackupSettings _backupSettings;
        private readonly string _logDirectory;
        private readonly string _applicationName;
        private readonly object threadLock = new object();
        private readonly Queue<BackupFolder> backUpFolderUpdateQueue = new Queue<BackupFolder>();
        public event EventHandler OnUpdateDataSourceAttribute;

        private SettingsService()
        {
            _backupSettings = new BackupSettings();
            _backupSettings.BackupFolders = new SortableBindingList<BackupFolder>();
            _backupSettings.UsingDefaultPassword = true;
            _logDirectory = ConfigurationManager.AppSettings["LogDirectory"];
            _backupSettings.BackupFolders.ListChanged += BackupFolders_ListChanged;

            if (string.IsNullOrWhiteSpace(_logDirectory) || !Directory.Exists(_logDirectory))            
                _logDirectory = AppDomain.CurrentDomain.BaseDirectory;

            _applicationName = Application.ProductName;            
        }

        void BackupFolders_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                foreach (BackupFolder bc in _backupSettings.BackupFolders)
                {
                    if (bc.RequireUpdate)
                        DirectoryComputeService.Instance.ComputeBackupFolderData(bc);
                }
            }
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                if (sender is SortableBindingList<BackupFolder>)
                {
                    SortableBindingList<BackupFolder> backupFolderList = sender as SortableBindingList<BackupFolder>;
                    foreach (BackupFolder bc in backupFolderList)
                    {
                        bc.SetAsDeleted();
                        DirectoryComputeService.Instance.CancelWorkItem(bc);
                    }
                }
                else
                {
                    BackupFolder bc = sender as BackupFolder;
                    bc.SetAsDeleted();
                    DirectoryComputeService.Instance.CancelWorkItem(bc);
                }
                
            }
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {

            }
        }      

        public void LoadSettings()
        {
            try
            {
                
            }
            catch(Exception ex)
            {

            }

        }

        public bool SaveSettings()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!path.EndsWith("\\"))
                path += "\\";
            path += _applicationName + "\\BackupServiceSettings.dat";

            int threadCount = Environment.ProcessorCount;
            StorageManager storageManager =
                new StorageManager(new StorageManagerSettings
                {
                    NumberOfThreads = threadCount,
                    Password = "",
                    UseEncryption = false,
                    UseMultithreading = true
                });
            return storageManager.SerializeObjectToFile(_backupSettings, path, null);
        }

        public BackupSettings Settings => _backupSettings;
        public string LogDirectoryPath => _logDirectory;

        public static SettingsService Instance => _instance ?? (_instance = new SettingsService());

        public void HandleUpdateOnNativeThread()
        {
            lock (threadLock)
            {
                while(backUpFolderUpdateQueue.Count>0)
                {
                    backUpFolderUpdateQueue.Dequeue().TriggerFullUpdateByNativeThread();
                }
            }
        }

        internal void BackupFolderItemUpdated(BackupFolder backupFolder)
        {
            lock(threadLock)
            {
                backUpFolderUpdateQueue.Enqueue(backupFolder);                
            }
            if (OnUpdateDataSourceAttribute != null)
                OnUpdateDataSourceAttribute.Invoke(this, new EventArgs());
        }
    }
}

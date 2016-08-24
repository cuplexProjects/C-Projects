using BackupService.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BackupService.Utils;

namespace BackupService.Storage
{
    public class DirectoryComputeService : IDisposable
    {
        private static DirectoryComputeService _instance;
        private readonly Queue<BackupFolder> backupFolderUpdateQueue;        
        private bool runWorkerThread;        
        private readonly Thread workerThread;
        private readonly ManualResetEvent resetEvent;
        private readonly ManualResetEvent threadPoolResetEvent;
        private readonly HashSet<ulong> currentWorkItems;
        private readonly object threadLock = new object();
        private readonly object workItemsThreadLock = new object();
        private readonly AccessHelper accessHelper;
        private const int MaxThreadPoolQueue = 10;
        

        private DirectoryComputeService()
        {
            accessHelper = new AccessHelper();
            currentWorkItems = new HashSet<ulong>();
            backupFolderUpdateQueue = new Queue<BackupFolder>();
            runWorkerThread = true;
            resetEvent = new ManualResetEvent(false);
            threadPoolResetEvent = new ManualResetEvent(false);
            workerThread = new Thread(ProcessBackupFolderUpdateQueue);
            workerThread.Start();
        }

        private int ThreadPoolQueueUsage { get; set; }

        private void ProcessBackupFolderUpdateQueue()
        {
            while(runWorkerThread)
            {
                while (backupFolderUpdateQueue.Count > 0)
                {
                    BackupFolder bc = null;
                    lock (threadLock)
                    {
                        bc = backupFolderUpdateQueue.Dequeue();
                    }
                     
                    if (bc != null && !bc.WasDeleted)
                    {
                        if (ThreadPoolQueueUsage >= MaxThreadPoolQueue)
                        {
                            threadPoolResetEvent.Reset();
                            threadPoolResetEvent.WaitOne();
                        }                        
                        ThreadPool.QueueUserWorkItem(ProcessDirectoryDataCallBack, bc);
                    }
                }

                resetEvent.Reset();
                resetEvent.WaitOne();
            }
        }

        private void ProcessDirectoryDataCallBack(object state)
        {
            BackupFolder bc = state as BackupFolder;
            DirectoryData dc = new DirectoryData();
            ThreadPoolQueueUsage++;

            lock (workItemsThreadLock)
            {
                currentWorkItems.Add(bc.UniqueId);
            }

            ComputeDirecoryData(bc.Directory, dc, bc.UniqueId);            

            bc.Files = dc.FilesTotal;
            bc.DataSize = dc.TotalFileSize;
            bc.SubFolders = dc.SubDirectories;
            bc.RequireUpdate = false;
            bc.TriggerFullUpdate();
            ThreadPoolQueueUsage--;

            if(workerThread.ThreadState == ThreadState.WaitSleepJoin && ThreadPoolQueueUsage < MaxThreadPoolQueue)
                threadPoolResetEvent.Set();
        }

        private void ComputeDirecoryData(string path, DirectoryData dc, ulong uniqueId)
        {
            lock (workItemsThreadLock)
            {
                if (!currentWorkItems.Contains(uniqueId))
                    return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (accessHelper.UserHasReadAccessToDirectory(directoryInfo))
            {
                FileInfo[] directoryFiles;
                try
                {
                    directoryFiles = directoryInfo.GetFiles();
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLog("Error in class DirectoryData.GetDirecoryData() - " + ex.Message);
                    return;
                }
                dc.FilesTotal += directoryInfo.GetFiles().Length;

                foreach (FileInfo di in directoryFiles)
                {
                    dc.TotalFileSize += di.Length;
                }

                DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
                dc.SubDirectories += subDirectories.Length;

                foreach (DirectoryInfo subDir in subDirectories)
                {
                    ComputeDirecoryData(subDir.FullName, dc, uniqueId);
                }
            }       
        }

        public void ComputeBackupFolderData(BackupFolder bc)
        {
            lock(threadLock)
            {
                backupFolderUpdateQueue.Enqueue(bc);
            }
            resetEvent.Set();
        }

        internal void CancelWorkItem(BackupFolder bc)
        {
            lock (workItemsThreadLock)
            {
                if (currentWorkItems.Contains(bc.UniqueId))
                    currentWorkItems.Remove(bc.UniqueId);                
            }
        }

        public static DirectoryComputeService Instance
        {
            get { return _instance ?? (_instance = new DirectoryComputeService()); }
        }

        public void Dispose()
        {
            threadPoolResetEvent.Set();
            resetEvent.Set();
            runWorkerThread = false;
            lock (workItemsThreadLock)
            {
                currentWorkItems.Clear();
            }
            _instance = null;
        }        
    }
}

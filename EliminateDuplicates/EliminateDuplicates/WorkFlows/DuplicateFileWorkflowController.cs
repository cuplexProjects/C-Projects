using System;
using System.Collections.Generic;
using DeleteDuplicateFiles.Delegates;
using DeleteDuplicateFiles.Managers;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.WorkFlows.Interface;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.WorkFlows
{
    [UsedImplicitly]
    public class DuplicateFileWorkflowController : WorkFlowBase, IWorkflowController
    {
        private readonly AppSettingsManager _appSettingsManager;
        public bool IsRunning { get; private set; }
        public bool IsReady { get; private set; }

        public event SearchProgressEventHandler OnProgressUpdate;
        public event EventHandler<EventArgs> OnSearchComplete;
        public event EventHandler<EventArgs> OnSearchBegin;
        public event EventHandler<FileHashEventArgs> OnBeginNewFileHash;
        public event EventHandler<FileHashEventArgs> OnCompleteFileHash;
        public event EventHandler<FileHashRemovalEventArgs> OnCompletedRemovalOfDeletedFiles;

        private bool _searchingRequested;

        public DuplicateFileWorkflowController(AppSettingsManager appSettingsManager)
        {
            _appSettingsManager = appSettingsManager;
            IsReady = true;
        }

        public bool StartDuplicateSearch(SearchProfileModel searchProfile)
        {
            if (IsRunning)
            {
                return false;
            }

            IsRunning = true;
            _searchingRequested = true;
            return true;
        }

        public void StopSearching()
        {
            _searchingRequested = false;
        }

        public IEnumerable<ScanFolderModel> GetDuplicateFiles(ISearchSettings settings)
        {
            yield break;
        }
       
    }
}
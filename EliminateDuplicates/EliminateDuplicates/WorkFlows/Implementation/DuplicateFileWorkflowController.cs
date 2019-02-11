using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeleteDuplicateFiles.Delegates;
using DeleteDuplicateFiles.Managers;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.WorkFlows.Interface;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.WorkFlows.Implementation
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

        public async Task<bool> StartDuplicateSearchAsync(SearchProfileModel searchProfile)
        {
            if (IsRunning)
            {
                return false;
            }

            IsRunning = true;
            _searchingRequested = true;


            // Find all relevant files in scanfolderList
            //List<DuplicateFileModel> duplicateFileCandidateList= new List<DuplicateFileModel>();
            ParallelOptions parallelOptions = new ParallelOptions();
            var cancelationToken = parallelOptions.CancellationToken;
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;
            ISearchSettings searchSettings = new SearchSettings { DirectoryList = searchProfile.ScanFolderList, FileExtentionFilter = searchProfile.FileNameFilter, IncludeSubdirs = searchProfile.IncludeSubfolders };
            DistributedFileScanner fileScanner = new DistributedFileScanner();
            BlockingCollection<List<DuplicateFileModel>> duplicateFileCandidateList = new BlockingCollection<List<DuplicateFileModel>>();


            int index = 0;
            OnSearchBegin?.Invoke(this, EventArgs.Empty);

            Parallel.ForEach(searchSettings.DirectoryList, parallelOptions, folder =>
            {
                Console.WriteLine(Interlocked.Increment(ref index));
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                var result = fileScanner.GetFilesFromBaseDirectory(folder.FullPath, searchSettings,OnProgressUpdate);
                duplicateFileCandidateList.Add(result, cancelationToken);
            });


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
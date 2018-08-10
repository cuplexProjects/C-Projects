using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.WorkFlows.Interface;

namespace DeleteDuplicateFiles.WorkFlows.Implementation
{
    public class DistributedFileScanner : WorkFlowBase, IWorkflowComponenFileSearch
    {
        public DistributedFileScanner()
        {
            FileCount = 0;
        }

        public int FileCount { get; private set; }
        private bool RunningFileCount { get; set; }

        public void CalculateFileCount()
        {
            RunningFileCount = false;
        }

        public List<DuplicateFileModel> GetFilesFromBaseDirectory(string fullDirectoryPath, ISearchSettings searchSettings, SearchProgressEventHandler searchProgress)
        {
            if (!RunningFileCount)
            {
                RunningFileCount = true;
                Task.Factory.StartNew(async () => FileCount = await GetAproximateFileCount(fullDirectoryPath, searchSettings, 0));
            }

            var duplicateFiles = new List<DuplicateFileModel>();
            string filter = string.IsNullOrWhiteSpace(searchSettings.FileExtentionFilter) ? "*.*" : searchSettings.FileExtentionFilter;
            var fileNames = Directory.GetFiles(fullDirectoryPath, filter);


            duplicateFiles.AddRange(from fileName in fileNames
                                    let fileInfo = new FileInfo(fileName)
                                    select new DuplicateFileModel
                                    {
                                        Name = fileInfo.Name,
                                        FullName = fileInfo.FullName,
                                        LastWriteTime = fileInfo.LastWriteTime,
                                        CreationTime = fileInfo.CreationTime,
                                        FileSize = fileInfo.Length
                                    });

            searchProgress?.Invoke(this, new SearchProgressEventArgs { PercentComplete = 10, ProgressMessage = "Scanning files" });
            if (searchSettings.IncludeSubdirs)
            {

                var directories = Directory.GetDirectories(fullDirectoryPath);
                foreach (string directory in directories)
                {
                    duplicateFiles.AddRange(GetFilesFromBaseDirectory(directory, searchSettings, searchProgress));
                }

            }


            return duplicateFiles;
        }

        private async Task<int> GetAproximateFileCount(string rootFolder, ISearchSettings searchSettings, int depth)
        {
            string filter = string.IsNullOrWhiteSpace(searchSettings.FileExtentionFilter) ? "*.*" : searchSettings.FileExtentionFilter;
            int fileCount = Directory.GetFiles(rootFolder, filter).Length;
            var directories = Directory.GetDirectories(rootFolder);

            await Task.Factory.StartNew(async () =>
            {
                foreach (string directory in directories)
                {
                    fileCount += await GetAproximateFileCount(directory, searchSettings, depth + 1);
                }
            });

            return fileCount;
        }
    }
}

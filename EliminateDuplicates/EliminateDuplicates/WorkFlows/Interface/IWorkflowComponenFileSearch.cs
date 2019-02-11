using System;
using System.Collections.Generic;
using DeleteDuplicateFiles.Models;

namespace DeleteDuplicateFiles.WorkFlows.Interface
{
    /// <summary>
    /// IWorkflowComponenFileSearch for listing files
    /// </summary>
    public interface IWorkflowComponenFileSearch
    {
        /// <summary>
        /// Gets the files from base directory.
        /// </summary>
        /// <param name="fullDirectoryPath">The full directory path.</param>
        /// <param name="searchSettings">The search settings.</param>
        /// <param name="searchProgress">The search progress.</param>
        /// <returns></returns>
        List<DuplicateFileModel> GetFilesFromBaseDirectory(string fullDirectoryPath, ISearchSettings searchSettings, SearchProgressEventHandler searchProgress);
    }
}

using System.Collections.Generic;
using DeleteDuplicateFiles.Models;

namespace DeleteDuplicateFiles.WorkFlows.Interface
{
    /// <summary>
    /// SearchSettings interface
    /// </summary>
    public interface ISearchSettings
    {
        /// <summary>
        /// Gets or sets the directory list.
        /// </summary>
        /// <value>
        /// The directory list.
        /// </value>
        IEnumerable<ScanFolderModel> DirectoryList { get; set; }

        /// <summary>
        /// Gets or sets the file extention filter.
        /// </summary>
        /// <value>
        /// The file extention filter.
        /// </value>
        string FileExtentionFilter { get; set; }

        /// <summary>
        /// Gets a value indicating whether [include subdirs].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include subdirs]; otherwise, <c>false</c>.
        /// </value>
        bool IncludeSubdirs { get; set; }
    }
}
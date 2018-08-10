using System.Collections.Generic;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.WorkFlows.Interface;

namespace DeleteDuplicateFiles.WorkFlows.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso>
    ///     <cref>DeleteDuplicateFiles.WorkFlows.Interface.ISearchSettings</cref>
    /// </seealso>
    public class SearchSettings : ISearchSettings
    {
        /// <summary>
        /// Gets or sets the directory list.
        /// </summary>
        /// <value>
        /// The directory list.
        /// </value>
        public IEnumerable<ScanFolderModel> DirectoryList { get; set; }

        /// <summary>
        /// Gets or sets the file extention filter.
        /// </summary>
        /// <value>
        /// The file extention filter.
        /// </value>
        public string FileExtentionFilter { get; set; }

        /// <summary>
        /// Gets a value indicating whether [include subdirs].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include subdirs]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeSubdirs { get; set; }
    }
}
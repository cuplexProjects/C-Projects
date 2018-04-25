using System;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;

namespace DeleteDuplicateFiles.Models
{
    /// <summary>
    /// ComputedFileHash Model
    /// </summary>
    public class ComputedFileHashModel : ComputedFileHashDataModel
    {
       
        /// <summary>
        /// Gets the drive letter.
        /// </summary>
        /// <value>
        /// The drive letter.
        /// </value>
        public string DriveLetter
        {
            get
            {
                if (FullPath != null && FullPath.Length > 3)
                    return FullPath.Substring(0, 3);
                return "";
            }
        }
    }
}
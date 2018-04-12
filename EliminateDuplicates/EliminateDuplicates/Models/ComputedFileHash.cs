#region

using System;
using System.Runtime.Serialization;
using DeleteDuplicateFiles.DataModels;

#endregion

namespace DeleteDuplicateFiles.Models
{
    public class ComputedFileHash : ComputedFileHashDataModel
    {
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
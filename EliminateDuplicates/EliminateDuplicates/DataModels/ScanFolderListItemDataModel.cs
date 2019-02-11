using System;
using System.Runtime.Serialization;

namespace DeleteDuplicateFiles.DataModels
{
    [Serializable]
    [DataContract]
    public class ScanFolderListItemDataModel
    {
        [DataMember(Order = 1, Name = "FullPath")]
        public string FullPath;

        [DataMember(Order = 2, Name = "SortOrder")]
        public int SortOrder;
    }
}   
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DeleteDuplicateFiles.DataModels
{
    [DataContract]
    [Serializable]
    public class SearchProfileDataModel
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ScanFolderList")]
        public List<SearchProfileDataModel> ScanFolderList { get; set; }

        [DataMember(Order = 2, IsRequired = true, Name = "PreferredDirecoryList")]
        public List<SearchProfileDataModel> PreferredDirecoryList { get; set; }

        [DataMember(Order = 3, IsRequired = true, Name = "FileNameFilter")]
        public string FileNameFilter { get; set; }

        [DataMember(Order = 4, IsRequired = true, Name = "ProfileName")]
        public string ProfileName { get; set; }
    }
}

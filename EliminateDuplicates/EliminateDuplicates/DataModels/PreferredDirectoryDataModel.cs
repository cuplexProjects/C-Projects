using System;
using System.Runtime.Serialization;

namespace DeleteDuplicateFiles.DataModels
{
    [Serializable]
    [DataContract(Name = "PreferredDirectory")]
    public class PreferredDirectoryDataModel
    {
        [DataMember(Name = "SortOrder", Order = 1)]
        public int SortOrder { get; set; }

        [DataMember(Name = "Path", Order = 2)]
        public string Path { get; set; }

        public override string ToString()
        {
            return Path;
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace DeleteDuplicateFiles.Models
{
    [Serializable]
    [DataContract(Name = "PreferredDirectory")]
    public class PreferredDirectory
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
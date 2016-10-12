using System;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "Bookmark")]
    public class Bookmark : ImageReference
    {
        [DataMember(Name = "SortOrder", Order = 1)]
        public int SortOrder { get; set; }

        [DataMember(Name = "BoookmarkName", Order = 2)]
        public string BoookmarkName { get; set; }

        [DataMember(Name = "ParentFolderId", Order = 3)]
        public string ParentFolderId { get; set; }
    }
}
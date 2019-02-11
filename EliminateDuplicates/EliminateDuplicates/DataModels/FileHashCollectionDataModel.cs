using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DeleteDuplicateFiles.DataModels
{
    [DataContract]
    [Serializable]
    public class FileHashCollectionDataModel
    {
        [DataMember(Name = "FileHashDictionary", Order = 1)]
        public Dictionary<string, ComputedFileHashDataModel> FileHashDictionary { get; set; }

        [DataMember(Name = "LastModified", Order = 2)]
        public DateTime LastModified { get; set; }

        [DataMember(Name = "CollectionId", Order = 3)]
        public Guid CollectionId { get; set; }
    }
}

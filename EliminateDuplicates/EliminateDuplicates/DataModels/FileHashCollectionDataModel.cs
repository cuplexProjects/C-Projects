using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DeleteDuplicateFiles.DataModels
{
    [DataContract]
    public class FileHashCollectionDataModel
    {
        [DataMember(Name = "FileHashDictionary", Order = 1)]
        public Dictionary<string, ComputedFileHashDataModel> FileHashDictionary { get; set; }
    }
}

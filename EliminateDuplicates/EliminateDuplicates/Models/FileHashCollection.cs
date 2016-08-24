#region

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace DeleteDuplicateFiles.Models
{
    [DataContract]
    public class FileHashCollection
    {
        public FileHashCollection()
        {
            FileHashDictionary = new Dictionary<string, ComputedFileHash>();
        }

        [DataMember(Name = "FileHashDictionary", Order = 1)]
        public Dictionary<string, ComputedFileHash> FileHashDictionary { get; set; }
    }
}
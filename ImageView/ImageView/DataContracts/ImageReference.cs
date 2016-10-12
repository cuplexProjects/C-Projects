using System;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "ImageReference")]
    public class ImageReference
    {
        [DataMember(Name = "Directory", Order = 1)]
        public string Directory { get; set; }

        [DataMember(Name = "FileName", Order = 2)]
        public string FileName { get; set; }

        [DataMember(Name = "CompletePath", Order = 3)]
        public string CompletePath { get; set; }

        [DataMember(Name = "Size", Order = 4)]
        public long Size { get; set; }

        [DataMember(Name = "CreationTime", Order = 5)]
        public DateTime CreationTime { get; set; }

        [DataMember(Name = "LastWriteTime", Order = 6)]
        public DateTime LastWriteTime { get; set; }

        [DataMember(Name = "LastAccessTime", Order = 7)]
        public DateTime LastAccessTime { get; set; }
    }
}
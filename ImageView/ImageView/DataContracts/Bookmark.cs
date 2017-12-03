using System;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "Bookmark")]
    public class Bookmark
    {
        [DataMember(Name = "SortOrder", Order = 1)]
        public int SortOrder { get; set; }

        [DataMember(Name = "BoookmarkName", Order = 2)]
        public string BoookmarkName { get; set; }

        [DataMember(Name = "ParentFolderId", Order = 3)]
        public string ParentFolderId { get; set; }

        [DataMember(Name = "Directory", Order = 4)]
        public string Directory { get; set; }

        [DataMember(Name = "FileName", Order = 5)]
        public string FileName { get; set; }

        [DataMember(Name = "CompletePath", Order = 6)]
        public string CompletePath { get; set; }

        [DataMember(Name = "Size", Order = 7)]
        public long Size { get; set; }

        [DataMember(Name = "CreationTime", Order = 8)]
        public DateTime CreationTime { get; set; }

        [DataMember(Name = "LastWriteTime", Order = 9)]
        public DateTime LastWriteTime { get; set; }

        [DataMember(Name = "LastAccessTime", Order = 10)]
        public DateTime LastAccessTime { get; set; }

        public string SizeFormated => GeneralToolkitLib.Converters.GeneralConverters.FormatFileSizeToString(Size);
    }
}
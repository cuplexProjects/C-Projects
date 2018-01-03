using System;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "ThumbnailEntry")]
    public class ThumbnailEntry
    {
        public ThumbnailEntry()
        {
            UniqueId = Guid.NewGuid();
        }
        [DataMember(Name = "FileName", Order = 1)]
        public string FileName { get; set; }

        [DataMember(Name = "Directory", Order = 2)]
        public string Directory { get; set; }

        [DataMember(Name = "FilePosition", Order = 3)]
        public long FilePosition { get; set; }

        [DataMember(Name = "Length", Order = 4)]
        public int Length { get; set; }

        [DataMember(Name = "Date", Order = 5)]
        public DateTime Date { get; set; }

        [DataMember(Name = "SourceImageDate", Order = 6)]
        public DateTime SourceImageDate { get; set; }

        [DataMember(Name = "SourceImageLength", Order = 7)]
        public long SourceImageLength { get; set; }

        [DataMember(Name = "UniqueId", Order = 8)]
        public Guid UniqueId { get; set; }
    }
}

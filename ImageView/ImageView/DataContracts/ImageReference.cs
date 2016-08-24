using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "ImageReference", Namespace = "ImageView.DataContracts")]
    [ProtoContract(AsReferenceDefault = true, DataMemberOffset = 0, Name = "ImageReference", UseProtoMembersOnly = true)]
    public class ImageReference
    {
        [DataMember(Name = "Directory", Order = 1)]
        [ProtoMember(1, AsReference = true, IsRequired = true, Name = "Directory")]
        public string Directory { get; set; }

        [DataMember(Name = "FileName", Order = 2, IsRequired = true)]
        [ProtoMember(2, AsReference = true, IsRequired = true, Name = "FileName")]
        public string FileName { get; set; }

        [DataMember(Name = "CompletePath", Order = 3)]
        [ProtoMember(3, AsReference = true, IsRequired = true, Name = "CompletePath")]
        public string CompletePath { get; set; }

        [DataMember(Name = "Size", Order = 4)]
        [ProtoMember(4, AsReference = true, IsRequired = true, Name = "Size")]
        public long Size { get; set; }

        [DataMember(Name = "CreationTime", Order = 5)]
        [ProtoMember(5, AsReference = true, IsRequired = true, Name = "CreationTime")]
        public DateTime CreationTime { get; set; }

        [DataMember(Name = "LastWriteTime", Order = 6)]
        [ProtoMember(6, AsReference = true, IsRequired = true, Name = "LastWriteTime")]
        public DateTime LastWriteTime { get; set; }

        [DataMember(Name = "LastAccessTime", Order = 7)]
        [ProtoMember(7, AsReference = true, IsRequired = true, Name = "LastAccessTime")]
        public DateTime LastAccessTime { get; set; }
    }
}
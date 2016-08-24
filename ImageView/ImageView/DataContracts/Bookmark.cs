using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "Bookmark", Namespace = "ImageView.DataContracts")]
    [ProtoContract(AsReferenceDefault = true,DataMemberOffset = 0,Name = "Bookmark",UseProtoMembersOnly = true)]
    public class Bookmark : ImageReference
    {
        [DataMember(Name = "SortOrder", Order = 1)]
        [ProtoMember(1, AsReference = true, IsRequired = true, Name = "SortOrder")]
        public int SortOrder { get; set; }

        [DataMember(Name = "BoookmarkName", Order = 2)]
        [ProtoMember(2, AsReference = true, IsRequired = true, Name = "BoookmarkName")]
        public string BoookmarkName { get; set; }

        [DataMember(Name = "ParentFolder", Order = 3)]
        [ProtoMember(3, AsReference = true, IsRequired = true, Name = "ParentFolder")]
        public BookmarkFolder ParentFolder { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ImageView.Models.Interface;
using ProtoBuf;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "BookmarkFolder", Namespace = "ImageView.DataContracts")]
    [ProtoContract(AsReferenceDefault = true, DataMemberOffset = 0, Name = "BookmarkFolder", UseProtoMembersOnly = true)]
    public class BookmarkFolder : IBookmarkFolder
    {
        [DataMember(Name = "BookmarkFolderId", Order = 1)]
        [ProtoMember(1, AsReference = true, IsRequired = true, Name = "BookmarkFolderId")]
        public string BookmarkFolderId { get; protected set; }

        [DataMember(Name = "BookmarkFolders", Order = 2)]
        [ProtoMember(2, AsReference = true, IsRequired = true, Name = "BookmarkFolders")]
        public List<BookmarkFolder> BookmarkFolders { get; protected set; }

        [DataMember(Name = "ParentFolder", Order = 3)]
        [ProtoMember(3, AsReference = true, IsRequired = true, Name = "ParentFolder")]
        public BookmarkFolder ParentFolder { get; set; }

        [DataMember(Name = "Name", Order = 4)]
        [ProtoMember(4, AsReference = true, IsRequired = true, Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "SortOrder", Order = 5)]
        [ProtoMember(5, AsReference = true, IsRequired = true, Name = "SortOrder")]
        public int SortOrder { get; set; }

        [DataMember(Name = "Bookmarks", Order = 6)]
        [ProtoMember(6, AsReference = true, IsRequired = true, Name = "Bookmarks")]
        public List<Bookmark> Bookmarks { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
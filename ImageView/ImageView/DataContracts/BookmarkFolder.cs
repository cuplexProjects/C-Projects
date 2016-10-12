using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "BookmarkFolder")]
    public class BookmarkFolder
    {
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        [DataMember(Name = "BookmarkFolders", Order = 2, IsRequired = true)]
        public List<BookmarkFolder> BookmarkFolders { get; set; }

        [DataMember(Name = "ParentFolderId", Order = 3)]
        public string ParentFolderId { get; set; }

        [DataMember(Name = "Name", Order = 4)]
        public string Name { get; set; }

        [DataMember(Name = "SortOrder", Order = 5)]
        public int SortOrder { get; set; }

        [DataMember(Name = "Bookmarks", Order = 6, IsRequired = true)]
        public List<Bookmark> Bookmarks { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
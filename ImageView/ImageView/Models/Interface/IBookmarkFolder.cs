using System.Collections.Generic;
using ImageView.DataContracts;

namespace ImageView.Models.Interface
{
    public interface IBookmarkFolder
    {
        string BookmarkFolderId { get; }
        List<BookmarkFolder> BookmarkFolders { get; }
        BookmarkFolder ParentFolder { get; }
        string Name { get; set; }
        int SortOrder { get; set; }
        List<Bookmark> Bookmarks { get; }
    }
}
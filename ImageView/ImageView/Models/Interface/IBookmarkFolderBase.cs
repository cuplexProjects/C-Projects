using ImageView.DataContracts;
using ImageView.Events;
using ImageView.Models.Implementation;

namespace ImageView.Models.Interface
{
    internal interface IBookmarkFolderBase
    {
        void AddBookmarkFolder(BookmarkFolder bookmarkFolder);
        void AddBookmarkFromImageReference(string boookmarkName, ImageReferenceElement imgRef);
        bool DeleteBookmark(Bookmark bookmarkToDelete);
        bool DeleteBookmarkFolder(BookmarkFolder bookmarkFolder);
        event BookmarkUpdatedEventHandler OnBookmarksUpdate;
        void BookmarkUpdate(BookmarkUpdatedEventArgs e);
    }
}
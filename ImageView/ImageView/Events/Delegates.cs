using System;
using System.Drawing;

namespace ImageView.Events
{
    public delegate void TransitionImageUpdateEventHandler(object sender, TransitionImageUpdateEventArgs e);

    public class TransitionImageUpdateEventArgs : EventArgs
    {
        public Image TransitionImage { get; set; }
    }

    public delegate void IntervalChangedDeligate(object sender, IntervalEventArgs e);

    public class IntervalEventArgs : EventArgs
    {
        public IntervalEventArgs()
        {
        }

        public IntervalEventArgs(int interval)
        {
            Interval = interval;
        }

        public int Interval { get; set; }
    }

    public delegate void BookmarkUpdatedEventHandler(object sender, BookmarkUpdatedEventArgs e);

    public class BookmarkUpdatedEventArgs
    {
        public BookmarkUpdatedEventArgs(BookmarkActions bookmarkAction, Type bookmarkType)
        {
            BookmarkAction = bookmarkAction;
            BookmarkType = bookmarkType;
        }


        public BookmarkActions BookmarkAction { get; private set; }
        public Type BookmarkType { get; private set; }
    }

    public enum BookmarkActions
    {
        CreatedBookmark,
        CreatedBookmarkFolder,
        DeletedBookmark,
        DeletedBookmarkFolder,
        LoadedNewDataSource,
    }
}
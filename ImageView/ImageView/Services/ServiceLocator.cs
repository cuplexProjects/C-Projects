using ImageView.Managers;

namespace ImageView.Services
{
    public static class ServiceLocator
    {
        private static BookmarkService _bookmarkService;


        public static BookmarkService GetBookmarkService()
        {
            return _bookmarkService ?? (_bookmarkService = new BookmarkService());
        }

        public static void Clear()
        {
            _bookmarkService = null;
        }
    }
}

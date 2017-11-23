using ImageView.Managers;

namespace ImageView.Services
{
    public static class ServiceLocator
    {
        private static BookmarkService _bookmarkService;
        

        public static void Clear()
        {
            if (_bookmarkService == null)
                return;

            _bookmarkService.Dispose();
            _bookmarkService = null;
        }
    }
}

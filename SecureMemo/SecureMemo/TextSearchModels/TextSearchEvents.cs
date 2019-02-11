namespace SecureMemo.TextSearchModels
{
    public static class TextSearchEvents
    {
        public delegate void TextSearchEventHandler(object sender, TextSearchEventArgs eventArgs);

        public enum SearchDirection
        {
            Up = 1,
            Down = 2
        }
    }
}
using System;

namespace SecureMemo.TextSearchModels
{
    public class TextSearchEventArgs : EventArgs
    {
        public TextSearchEventArgs(bool searchAllTabs, TextSearchProperties textSearchProperties)
        {
            SearchAllTabs = searchAllTabs;
            SearchProperties = textSearchProperties;
        }

        public bool SearchAllTabs { get; set; }
        public TextSearchProperties SearchProperties { get; private set; }
    }

    public class TextSearchProperties
    {
        public string SearchText { get; set; }
        public bool CaseSensitive { get; set; }
        public TextSearchEvents.SearchDirection SearchDirection { get; set; }

        public bool LoopSearch { get; set; }
        public bool SearchAllTabs { get; set; }
    }
}
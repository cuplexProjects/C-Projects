namespace SecureMemo.TextSearchModels
{
    public class TextSearchState
    {
        public int MatchesFound { get; set; }
        public int TabIndex { get; set; }
        public int StartPosDown { get; set; }
        public int StartPosUp { get; set; }
        public int InitialTabIndex { get; set; }
        public int InitialStartPos { get; set; }
        public int TabCount { get; set; }
        public int TabPageSearchCount { get; set; }
    }
}
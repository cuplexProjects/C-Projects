namespace SecureMemo.TextSearchModels
{
    public class TextSearchResult
    {
        public int TabIndex { get; set; }
        public int StartPos { get; set; }
        public int Length { get; set; }
        public bool SearchTextFound { get; set; }
    }
}
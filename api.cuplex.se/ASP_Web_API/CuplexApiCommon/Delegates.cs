using System.IO;

namespace CuplexApiCommon
{
    public static class Delegates
    {
        public delegate void SetProgress(double percentProgress);
        public delegate void SetProgressText(string info);
        public delegate void LogEventHandler(object sender, ErrorEventArgs error);
    }
}

using System;

namespace Apps.cuplex.se.Models
{
    public class AppViewModel
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Version { get; set; }
        public Uri DownloadUrl { get; set; }
    }
}
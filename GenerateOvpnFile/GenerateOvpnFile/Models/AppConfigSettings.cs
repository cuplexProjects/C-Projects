using System;

namespace GenerateOvpnFile.Models
{
    [Serializable]
    public class AppConfigSettings
    {
        public string Name { get; set; }
        public string Host { get; set; }
    }
}

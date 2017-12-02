using System;

namespace GenerateOvpnFile.Models
{
    [Serializable]
    public sealed class AppConfigSettings
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Interface { get; set; }
        public string ServerPort { get; set; }
        public string Protocol { get; set; }
        public string ExtraHmac { get; set; }
        public string Compression { get; set; }
        public string Cipher { get; set; }
        public string CaFileName { get; set; }
    }
}

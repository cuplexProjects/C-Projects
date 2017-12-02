using System;

namespace GenerateOvpnFile.Models
{
    [Serializable]
    public class OpenVPNConnection
    {
        public string  ConnectionName { get; set; }
        public ConfigEnums.InterfaceType InterfaceType { get; set; }
        public ConfigEnums.Protocol Protocol { get; set; }
        public ConfigEnums.Compression Compression { get; set; }
        public ConfigEnums.TLS_Auth TlsAuth { get; set; }
        public string Cipher { get; set; }
        public string Hostname { get; set; }
        public int ServerPort { get; set; }
        public string CaCertFileData { get; set; }
        public string UserCertFileData { get; set; }
        public string UserCertPrivateKeyData { get; set; }
        public string TaKeyData { get; set; }
    }
}

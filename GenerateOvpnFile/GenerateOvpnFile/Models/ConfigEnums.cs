using System;

namespace GenerateOvpnFile.Models
{
    [Serializable]
    public static class ConfigEnums
    {
        public static readonly string[] CipherAlgorithms =
        {
            "None", "AES-128-CBC", "AES-192-CBC", "AES-256-CBC", "BF-CBC", "CAST5-CBC", "CAMELLIA-128-CBC", "CAMELLIA-192-CBC",
            "CAMELLIA-256-CBC", "DES-CBC", "DES-EDE-CBC", "DES-EDE3-CBC", "DESX-CBC", "IDEA-CBC", "RC2-40-CBC", "RC2-64-CBC", "RC2-CBC", "RC5-CBC", "SEED-CBC",
        };

        public enum InterfaceType
        {
            TUN,
            TAP
        }
        public enum Protocol
        {
            UDP,
            TCP
        }

        public enum Compression
        {
            Enabled,
            Adaptive,
            None,
            
        } 
        public enum TLS_Auth
        {
            Enabled,
            Disabled,
        }
        
    }
}

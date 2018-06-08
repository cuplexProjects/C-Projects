using System;
using System.Security.Permissions;

namespace GoogleDDNS.ConfigLib.Models.Json
{
    public class DDNSRecord : SyntheticRecord
    {
        public HostConfigAuthentication Authentication { get; set; }    

        public DateTime?  LastModified { get; set; }

        public bool Offline { get; set; }
    }
}
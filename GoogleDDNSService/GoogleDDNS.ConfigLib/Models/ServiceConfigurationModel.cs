using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDDNS.ConfigLib.Models
{
    public class ServiceConfigurationModel
    {
        public int UpdateCheckInterval { get; set; }



        public string DDNSJsonConfigDirectory { get; set; }
    }
}

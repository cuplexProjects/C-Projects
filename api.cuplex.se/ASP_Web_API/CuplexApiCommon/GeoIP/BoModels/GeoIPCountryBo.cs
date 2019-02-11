using System.Collections.Generic;
using CuplexApiCommon.BulkCopyModels;

namespace CuplexApiCommon.GeoIP.BoModels
{
    public class GeoIPCountryBo : IBulkCopyItem
    {
        public long IPFrom { get; set; }
        public long IPTo { get; set; }
        public string IPAddressFrom { get; set; }
        public string IPAddressTo {get; set; }
        public string CountryCode {get; set; }
        public string CountryName {get; set; }

        public IEnumerable<object> RowObjects { get; set; }
    }
}

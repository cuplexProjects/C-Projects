using System.Collections.Generic;
using CuplexApiCommon.BulkCopyModels;

namespace CuplexApiCommon.GeoIP.BoModels
{
    public class GeoIPBlockBo:IBulkCopyItem
    {
        public long LocationId { get; set; }
        public long IPFrom  { get; set; }
        public long IPTo  { get; set; }
        public IEnumerable<object> RowObjects { get; set; }
    }
}

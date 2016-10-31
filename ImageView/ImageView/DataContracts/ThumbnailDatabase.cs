using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "ThumbnailDatabase")]
    public class ThumbnailDatabase
    {
        [DataMember(Name = "ThumbnailEntries", Order = 1)]
        public List<ThumbnailEntry> ThumbnailEntries { get; set; }

        [DataMember(Name = "DatabaseId", Order = 2)]
        public string DatabaseId { get; set; }

        [DataMember(Name = "DataStroragePath", Order = 3)]
        public string DataStroragePath { get; set; }

        [DataMember(Name = "LasteUpdate", Order = 4)]
        public DateTime LasteUpdate { get; set; }
    }
}

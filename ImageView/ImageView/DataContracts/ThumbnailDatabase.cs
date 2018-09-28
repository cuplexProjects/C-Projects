using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "ThumbnailDatabase")]
    public class ThumbnailDatabase
    {
        /// <summary>
        /// Gets or sets the thumbnail entries.
        /// </summary>
        /// <value>
        /// The thumbnail entries.
        /// </value>
        [DataMember(Name = "ThumbnailEntries", Order = 1)]
        public List<ThumbnailEntry> ThumbnailEntries { get; set; }

        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        [DataMember(Name = "DatabaseId", Order = 2)]
        public string DatabaseId { get; set; }

        /// <summary>
        /// Gets or sets the data strorage path.
        /// </summary>
        /// <value>
        /// The data strorage path.
        /// </value>
        [DataMember(Name = "DataStroragePath", Order = 3)]
        public string DataStroragePath { get; set; }

        /// <summary>
        /// Gets or sets the laste update.
        /// </summary>
        /// <value>
        /// The laste update.
        /// </value>
        [DataMember(Name = "LasteUpdate", Order = 4)]
        public DateTime LasteUpdate { get; set; }
    }
}

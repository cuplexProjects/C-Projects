namespace GoogleDDNS.ConfigLib.Models.Json
{
    public class SyntecticHostConfig
    {
        /// <summary>
        /// Gets or sets the name.
        /// A domain, subdomain, or host. See Resource Records for formatting examples.
        /// The default is @.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the rwcord.
        /// The record's type. For example, the A record or MX record.
        /// See Resource Records for a list of supported record types.
        /// </summary>
        /// <value>
        /// The type of the rwcord.
        /// </value>
        public string RecordType { get; set; }

        public int  TTL { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// The record's data, which varies depending on the record type.
        /// For example, a host's IPv4 address for the A record type.
        /// See Resource Records for data examples.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string  Data { get; set; }
    }
}
using System;

namespace ASP_Web_API.Models
{
    /// <summary>
    /// Geo ip location
    /// </summary>
    [Serializable]
    public class GeoIpCountry
    {
        public string IPAddressFrom { get; set; }
        public string IPAddressTo { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
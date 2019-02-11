namespace CuplexApiCommon.GeoIP.DtoModels
{
    /// <summary>
    /// Geo ip location
    /// </summary>

    public class GeoIPCountryDto
    {
        public string IPAddressFrom { get; set; }
        public string IPAddressTo { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
namespace CuplexApiCommon.GeoIP.DtoModels
{
    public class GeoIPCityDto
    {
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public long? MetroCode { get; set; }
        public string AreaCode { get; set; }
    }
}

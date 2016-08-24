using System.Text.RegularExpressions;
using CuplexApiCommon.Converters;
using CuplexApiCommon.GeoIP.BoModels;
using CuplexApiCommon.GeoIP.DtoModels;
using DatabaseLib.Linq;

namespace InternalServices.GeoIp
{
    public class GeoIPManager
    {
        private readonly GeoIPRepository _geoIpRepository;
        public GeoIPManager()
        {
            _geoIpRepository= new GeoIPRepository();
            ObjectMapper.Bind<GeoIPCountry, GeoIPCountryBo>((source, target) => target.IPFrom = source.IPFrom);
            ObjectMapper.Bind<GeoIPCountryBo, GeoIPCountryDto>((source, target) => target.CountryName = source.CountryName,(source, target) => target.CountryCode = source.CountryCode);
            ObjectMapper.Bind<GeoIPCityBo, GeoIPCityDto>((source, target) => target.CountryCode = source.CountryCode);
        }
        public GeoIPCountryBo GetGeoIpCountry(string ip)
        {
            long ipNumber = ConvertIpToLong(ip);
            return _geoIpRepository.GetGeoIpCountry(ipNumber);
        }

        public GeoIPCityBo GetGeoIpCity(string ip)
        {
            long ipNumber = ConvertIpToLong(ip);
            return _geoIpRepository.GetGeoIpCity(ipNumber);
         }

        public GeoIPCountryDto ConvertToGeoIPCountryDto(GeoIPCountryBo geoIPCountryBo)
        {
            return ObjectMapper.Map<GeoIPCountryBo, GeoIPCountryDto>(geoIPCountryBo);
        } 
        public GeoIPCityDto ConvertToGeoIpCityDto(GeoIPCityBo geoIpCityBo)
        {
            return ObjectMapper.Map<GeoIPCityBo, GeoIPCityDto>(geoIpCityBo);
        }

        public static long ConvertIpToLong(string ip)
        {
            long ipLng = 0;
            try
            {
                string[] ipArr = ip.Split(".".ToCharArray());
                if (ipArr.Length == 4)
                {
                    ipLng = long.Parse(ipArr[3]);
                    ipLng += long.Parse(ipArr[2]) * 256;
                    ipLng += long.Parse(ipArr[1]) * 65536;
                    ipLng += long.Parse(ipArr[0]) * 16777216;
                }
            }
            catch { return 0; }
            return ipLng;
        }
        public static bool VerifyIpNumber(string ip)
        {
            Regex ipMatch = new Regex(@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
            return ipMatch.IsMatch(ip);
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using CuplexApiCommon.GeoIP.BoModels;
using Elmah;
using InternalServices.GeoIp;

namespace ASP_Web_API.Controllers
{
    /// <summary>
    /// </summary>
    public class GeoLocationController : ApiController
    {
        private readonly GeoIPCityBo _geoIpCityNACacheItem;
        private readonly GeoIPManager _geoIpManager;

        /// <summary>
        /// </summary>
        public GeoLocationController()
        {
            _geoIpManager = new GeoIPManager();
            _geoIpCityNACacheItem = new GeoIPCityBo();
        }

        /// <summary>
        ///     Get Geo location country information from ip adress
        /// </summary>
        /// <param name="IPV4Adress">IPv4 address in string format</param>
        /// <returns></returns>
        [HttpGet]
        [Route(@"api/GeoLocation/getGeoIpCountryFromIpAddress/{IPV4Adress:regex(.+)}")]
        public HttpResponseMessage getGeoIpCountryFromIpAddress(string IPV4Adress)
        {
            try
            {
                if (!GeoIPManager.VerifyIpNumber(IPV4Adress))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                var geoIpCountry = HttpRuntime.Cache["getGeoIpCountryFromIpAddress:" + IPV4Adress] as GeoIPCountryBo;

                if (geoIpCountry == null)
                {
                    geoIpCountry = _geoIpManager.GetGeoIpCountry(IPV4Adress);
                    HttpRuntime.Cache.Add("getGeoIpCountryFromIpAddress:" + IPV4Adress, geoIpCountry, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }

                return geoIpCountry == null ? Request.CreateResponse(HttpStatusCode.NotFound) : Request.CreateResponse(HttpStatusCode.OK, _geoIpManager.ConvertToGeoIPCountryDto(geoIpCountry));
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        ///     Get Geo location city information from ip adress
        /// </summary>
        /// <param name="IPV4Adress">IPv4 address in string format</param>
        /// <returns></returns>
        [HttpGet]
        [Route(@"api/GeoLocation/getGeoIpCityFromIpAddress/{IPV4Adress:regex(.+)}")]
        public HttpResponseMessage getGeoIpCityFromIpAddress(string IPV4Adress)
        {
            try
            {
                if (!GeoIPManager.VerifyIpNumber(IPV4Adress))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                var geoIpCity = HttpRuntime.Cache["getGeoIpCityFromIpAddress:" + IPV4Adress] as GeoIPCityBo;
                if (geoIpCity == null)
                {
                    geoIpCity = _geoIpManager.GetGeoIpCity(IPV4Adress);

                    // Not found in database
                    HttpRuntime.Cache.Add("getGeoIpCityFromIpAddress:" + IPV4Adress, geoIpCity ?? _geoIpCityNACacheItem, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
                else if (geoIpCity == _geoIpCityNACacheItem)
                    geoIpCity = null;

                return geoIpCity == null ? Request.CreateResponse(HttpStatusCode.NotFound) : Request.CreateResponse(HttpStatusCode.OK, _geoIpManager.ConvertToGeoIpCityDto(geoIpCity));
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace ASP_Web_API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class GetExternalIpController : ApiController
    {
        readonly int cacheTime = 60;
        /// <summary>
        /// 
        /// </summary>
        public GetExternalIpController()
        {
            string cacheTimeStr = System.Configuration.ConfigurationManager.AppSettings["RequestCacheTime"];
            if (cacheTimeStr != null)
            {
                int.TryParse(cacheTimeStr, out cacheTime);
            }
        }

        /// <summary>
        /// An external IP address is the address assigned to you by your Internet Service Provider that is how the 
        /// Internet and all other computers outside your local network see you.
        /// </summary>
        /// <returns>The Webservers external IP number</returns>
        [HttpGet]
        [Route("api/getExternalServerIp")]
        public HttpResponseMessage GetExternalServerIp()
        {
            string result = GetCacheStrValue("api/getExternalServerIp");
            if (result == null)
            {
                result = GetExternalIpStr();
                setCacheStrValue("api/getExternalServerIp", result);
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(result, Encoding.UTF8, "text/plain")};
            return resp;
        }

        /// <summary>
        /// Returning your external IP address, if local request then the return value will be the same as GetExternalServerIp
        /// </summary>
        /// <returns>Your external IP adress</returns>
        [HttpGet]
        [Route("api/getExternalIp")]
        public HttpResponseMessage GetExternalIp()
        {
            string result = GetCacheStrValue("api/getExternalIp");
            if (result == null)
            {
                if (HttpContext.Current.Request.IsLocal)
                    result = GetExternalIpStr();
                else
                    result = HttpContext.Current.Request.UserHostAddress;

                setCacheStrValue("api/getExternalIp", result);
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(result, Encoding.UTF8, "text/plain") };
            return resp;
        }

        private string GetCacheStrValue(string key)
        {
            return HttpRuntime.Cache.Get(key) as string;
        }

        private void setCacheStrValue(string key, string value)
        {
            HttpRuntime.Cache.Add(key, value, null, DateTime.Now.AddSeconds(cacheTime), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        private string GetExternalIpStr()
        {
            try
            {
                string externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch { return null; }
        }

    }
}

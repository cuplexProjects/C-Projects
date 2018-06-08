using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;

namespace GoogleDDNSService.Components
{
    public abstract class NetQueryBase
    {
        private readonly ILogger _logger;

        protected NetQueryBase(ILogger logger)
        {
            _logger = logger;
        }

        public abstract Task<IPAddress> GetExternalIpAddress();

        protected virtual async Task<IPAddress> GetExternalIpAddress(string url)
        {
            try
            {
                string externalIp;

                using (var client = new WebClient())
                {
                    externalIp = await client.DownloadStringTaskAsync(url);
                }

                externalIp = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Matches(externalIp)[0].ToString();
                return IPAddress.Parse(externalIp);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get external IP");
            }

            return null;
        }
    }
}

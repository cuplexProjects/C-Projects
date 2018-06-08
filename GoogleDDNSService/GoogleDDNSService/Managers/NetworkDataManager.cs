using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using GoogleDDNSService.Components;
using GoogleDDNSService.Extensions;
using JetBrains.Annotations;
using NLog;

namespace GoogleDDNSService.Managers
{
    [UsedImplicitly]
    public class NetworkDataManager : ManagerBase
    {
        private readonly ObjectCache _cache;
        private readonly ILogger _logger;
        private readonly NetHelpComponent _netHelp;
        private const string IPCacheKey = "ExternalIpAddress";

        public NetworkDataManager(ObjectCache cache, ILogger logger, NetHelpComponent netHelp)
        {
            _cache = cache;
            _logger = logger;
            _netHelp = netHelp;
        }

        public IPAddress GetCachedExternalIpAddress()
        {
            var externalIp = _cache.GetOrAdd<IPAddress>(IPCacheKey, () =>  _netHelp.GetExternalIpAddress().Result, () => TenMinuteCacheItemPolicy);
            return externalIp;
        }

        public async Task<IPAddress> GetExternalIpAsync()
        {
            return await _netHelp.GetExternalIpAddress();
        }
    }
}
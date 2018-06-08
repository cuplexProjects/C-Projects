using System;
using System.Runtime.Caching;

namespace GoogleDDNSService.Managers
{
    public abstract class ManagerBase
    {
        protected static CacheItemPolicy TenMinuteCacheItemPolicy => new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(10)) };
    }
}
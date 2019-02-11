using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace GoogleDDNSService.Extensions
{
    public static class ObjectCacheExtensions
    {
        /// <summary>
        /// Gets the or add.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        /// <returns>T.</returns>
        [DebuggerStepThrough]
        public static T GetOrAdd<T>(this ObjectCache cache, string key, Func<T> valueFactory, Func<CacheItemPolicy> cacheItemPolicy)
        {
            if (cache.Contains(key))
            {
                return (T)cache.Get(key);
            }

            var value = valueFactory.Invoke();
            if (value != null)
            {
                cache.Add(key, value, cacheItemPolicy.Invoke());
            }

            return value;
        }
    }
}
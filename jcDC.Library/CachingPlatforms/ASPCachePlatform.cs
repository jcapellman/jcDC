using System;
using System.Runtime.Caching;

using jcDC.Library.Enums;
using jcDC.Library.Objects;

namespace jcDC.Library.CachingPlatforms {
    public class ASPCachePlatform : BaseCachePlatform {
        public override void AddToCache<T>(string key, T value) {
            ObjectCache cache = MemoryCache.Default;

            var cacheItem = new jcCACHEItem(value);

            cache.Add(key, cacheItem, DateTimeOffset.MaxValue);
        }

        public override CACHINGPLATFORMS GetCachingPlatformType() {
            return CACHINGPLATFORMS.ASP;
        }

        public override jcCACHEItem GetFromCache(string key) {
            ObjectCache cache = MemoryCache.Default;

            var cacheItem = cache[key];

            if (cacheItem == null) {
                return null;
            }

            return (jcCACHEItem)cacheItem;
        }
    }
}
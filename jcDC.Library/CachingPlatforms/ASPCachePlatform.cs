using System;
using System.Runtime.Caching;

using jcDC.Library.Enums;
using jcDC.Library.Objects;

namespace jcDC.Library.CachingPlatforms {
    public class ASPCachePlatform : BaseCachePlatform {
        private ObjectCache _cache;

        public ASPCachePlatform() {
            _cache = MemoryCache.Default;
        }

        public override void AddToCache<T>(string key, T value) {
            var cacheItem = new jcCACHEItem(value);

            var existingItem = _cache[key];

            if (existingItem == null) {
                _cache.Add(key, cacheItem, DateTimeOffset.MaxValue);
            } else {
                _cache[key] = cacheItem;
            }
        }

        public override CACHINGPLATFORMS GetCachingPlatformType() {
            return CACHINGPLATFORMS.ASP;
        }

        public override jcCACHEItem GetFromCache(string key) {
            var cacheItem = _cache[key];

            if (cacheItem == null) {
                return null;
            }

            return (jcCACHEItem)cacheItem;
        }

        public override void RemoveDependencies(string[] dependencies) {
            if (dependencies == null) {
                return;
            }

            foreach (var item in dependencies) {
                _cache.Remove(item);
            }
        }
    }
}
using jcDC.Library.Enums;
using jcDC.Library.Objects;

using System;

namespace jcDC.Library.CachingPlatforms {
    public abstract class BaseCachePlatform {
        public abstract CACHINGPLATFORMS GetCachingPlatformType();

        public abstract void AddToCache<T>(string key, T value, DateTime expiration);

        public abstract jcCACHEItem GetFromCache(string key);

        public abstract void RemoveDependencies(string[] dependencies);

        public abstract void RemoveFromCache(string key);
    }
}
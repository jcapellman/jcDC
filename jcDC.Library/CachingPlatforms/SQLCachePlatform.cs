using System;

using jcDC.Library.Enums;
using jcDC.Library.Objects;

namespace jcDC.Library.CachingPlatforms {
    public class SQLCachePlatform : BaseCachePlatform {
        public override void AddToCache<T>(string key, T value) {
            throw new NotImplementedException();
        }

        public override CACHINGPLATFORMS GetCachingPlatformType() {
            return CACHINGPLATFORMS.SQL;
        }

        public override jcCACHEItem GetFromCache(string key) {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Runtime.Caching;
using System.Web.Http;

namespace jcDC.Library {
    public class jcCACHEController : ApiController {
        public T Return<T>(string key, T value, bool cacheObject = true) {
            if (cacheObject) {
                ObjectCache cache = MemoryCache.Default;

                cache.Add(key, value, DateTimeOffset.MaxValue);
            }

            return value;
        }
    }
}
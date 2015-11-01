using jcDC.Library.Objects;

using System;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Web.Http;

namespace jcDC.Library {
    public class jcCACHEController : ApiController {
        private T Return<T>(T value, string key, bool cacheObject) {
            if (cacheObject) {
                ObjectCache cache = MemoryCache.Default;

                var cacheItem = new jcCACHEItem(value);

                cache.Add(key, cacheItem, DateTimeOffset.MaxValue);
            }

            return value;
        }

        public T Return<T>(T value, bool cacheObject = true, [CallerMemberName] string callingMember = "", [CallerFilePath] string callingPath = "") {
            var key = string.Empty;

            if (cacheObject) {
                key = "/api/" + callingPath.Substring(callingPath.LastIndexOf("\\") + 1).Replace("Controller.cs", "");
            }

            return Return(value, key, cacheObject);
        }

        public T Return<T, TK>(T value, TK key, bool cacheObject = true) {
            return Return(value, key.ToString(), cacheObject);
        }
    }
}
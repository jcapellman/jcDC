using jcDC.Library.Objects;

using System;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Web.Http;

namespace jcDC.Library {
    public class jcCACHEController : ApiController {
        public T Return<T>(T value, bool cacheObject = true, [CallerMemberName] string callingMember = "", [CallerFilePath] string callingPath = "") {
            if (cacheObject) {
                ObjectCache cache = MemoryCache.Default;

                var cacheItem = new jcCACHEItem(value);

                var key = "/api/" + callingPath.Substring(callingPath.LastIndexOf("\\") + 1).Replace("Controller.cs", "");

                cache.Add(key, cacheItem, DateTimeOffset.MaxValue);
            }

            return value;
        }
    }
}
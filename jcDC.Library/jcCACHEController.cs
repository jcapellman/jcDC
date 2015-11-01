using System.Runtime.CompilerServices;
using System.Web.Http;

namespace jcDC.Library {
    public class jcCACHEController : ApiController {
        private T Return<T>(T value, string key, bool cacheObject) {
            if (cacheObject) {
                jcCACHEAttribute.CurrentCachePlatform.AddToCache<T>(key, value);
            }

            return value;
        }

        protected T Return<T>(T value, bool cacheObject = true, [CallerMemberName] string callingMember = "", [CallerFilePath] string callingPath = "") {
            var key = string.Empty;

            if (cacheObject) {
                key = "/api/" + callingPath.Substring(callingPath.LastIndexOf("\\") + 1).Replace("Controller.cs", "");
            }

            return Return(value, key, cacheObject);
        }

        protected T Return<T, TK>(T value, TK key, bool cacheObject = true) {
            return Return(value, key.ToString(), cacheObject);
        }
    }
}
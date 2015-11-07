using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Http;

namespace jcDC.Library {
    public class jcCACHEController : ApiController {
        private T Return<T>(T value, string key, int expiration, bool cacheObject) {
            if (cacheObject) {
                jcCACHEAttribute.CurrentCachePlatform.AddToCache<T>(key, value, DateTime.Now.AddMinutes(expiration));
            }

            return value;
        }

        protected T Return<T>(T value, bool cacheObject = true, [CallerMemberName] string callingMember = "", [CallerFilePath] string callingPath = "") {
            var key = string.Empty;

            if (cacheObject) {
                key = "/api/" + callingPath.Substring(callingPath.LastIndexOf("\\") + 1).Replace("Controller.cs", "");
            }

            return Return(value, key, getExpirationInMinutes(), cacheObject);
        }

        private int getExpirationInMinutes() {
            StackTrace stackTrace = new StackTrace();

            MethodBase method = stackTrace.GetFrame(2).GetMethod();

            var attr = (jcCACHEAttribute)method.GetCustomAttributes(typeof(jcCACHEAttribute), true)[0];

            return attr.ExpirationInMinutes;
        }

        protected T Return<T, TK>(T value, TK key, bool cacheObject = true) {
            return Return(value, key.ToString(), getExpirationInMinutes(), cacheObject);
        }
    }
}
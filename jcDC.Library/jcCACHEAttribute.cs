using jcDC.Library.CachingPlatforms;
using jcDC.Library.Enums;

using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http.Controllers;

namespace jcDC.Library {
    public class jcCACHEAttribute : System.Web.Http.Filters.ActionFilterAttribute {
        private string _key;
        private string[] _dependencies;
        private int _expirationInMinutes;

        public int ExpirationInMinutes {
            get { return _expirationInMinutes; }
        }

        public jcCACHEAttribute() { _key = null; _dependencies = null;  _expirationInMinutes = int.MaxValue; }

        public jcCACHEAttribute(object key, string[] dependencies = null, int expirationInMinutes = int.MaxValue) {
            _key = key.ToString();
            _dependencies = dependencies;
            _expirationInMinutes = expirationInMinutes;
        }

        private static BaseCachePlatform _currentCachePlatform;

        public static BaseCachePlatform CurrentCachePlatform {
            get {
                if (_currentCachePlatform != null) {
                    return _currentCachePlatform;
                }

                var selectedCachePlatform = Common.Constants.DefaultCachePlatform;

                var selectedCachePlatformStr = ConfigurationManager.AppSettings[Common.Constants.CONFIG_CACHE_PLATFORM_TYPE];

                if (selectedCachePlatformStr != null) {
                    selectedCachePlatform = (CACHINGPLATFORMS)Enum.Parse(typeof(CACHINGPLATFORMS), selectedCachePlatformStr);
                }

                foreach (Type type in Assembly.GetAssembly(typeof(BaseCachePlatform)).GetTypes().Where(a => a.IsClass && !a.IsAbstract && a.IsSubclassOf(typeof(BaseCachePlatform)))) {
                    var classItem = (BaseCachePlatform)Activator.CreateInstance(type, null);

                    if (classItem.GetCachingPlatformType() != selectedCachePlatform) {
                        continue;
                    }

                    _currentCachePlatform = classItem;
                }
                
                return _currentCachePlatform;
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext) {
            if (_dependencies != null) {
                CurrentCachePlatform.RemoveDependencies(_dependencies);
            }

            if (_key == Common.Constants.NO_CACHE_STR) {
                base.OnActionExecuting(actionContext);

                return;
            }
            
            if (string.IsNullOrEmpty(_key)) {
                _key = actionContext.RequestContext.Url.Request.RequestUri.LocalPath;
            }

            if (actionContext.ActionArguments.Any()) {
                _key = _key + "_";

                foreach (var item in actionContext.ActionArguments.Keys) {
                    _key += item + "=" + actionContext.ActionArguments[item];
                }
            }

            var cacheItem = CurrentCachePlatform.GetFromCache(_key);

            if (cacheItem != null && cacheItem.Expiration < DateTime.Now) {
                CurrentCachePlatform.RemoveFromCache(_key);

                base.OnActionExecuting(actionContext);

                return;
            }

            if (cacheItem != null) {
                var response = new HttpResponseMessage();

                response.Content = new ObjectContent(cacheItem.ItemType, cacheItem.ItemValue, new JsonMediaTypeFormatter());

                actionContext.Response = response;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
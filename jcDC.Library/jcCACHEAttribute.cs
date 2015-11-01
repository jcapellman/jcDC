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

        public jcCACHEAttribute() { _key = null; _dependencies = null; }

        public jcCACHEAttribute(object key, string[] dependencies = null) {
            _key = key.ToString();
            _dependencies = dependencies;
        }

        private static BaseCachePlatform _currentCachePlatform;

        public static BaseCachePlatform CurrentCachePlatform {
            get {
                if (_currentCachePlatform != null) {
                    return _currentCachePlatform;
                }

                var selectedCachePlatform = Common.Constants.DefaultCachePlatform;

                var selectedCachePlatformStr = ConfigurationManager.AppSettings["jcDC_CachePlatform"];

                if (selectedCachePlatformStr != null) {
                    selectedCachePlatform = (CACHINGPLATFORMS)Enum.Parse(typeof(CACHINGPLATFORMS), selectedCachePlatformStr);
                }

                foreach (Type type in Assembly.GetAssembly(typeof(BaseCachePlatform)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BaseCachePlatform)))) {
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
            if (_key == Common.Constants.NO_CACHE_STR) {
                CurrentCachePlatform.RemoveDependencies(_dependencies);

                base.OnActionExecuting(actionContext);

                return;
            }

            if (string.IsNullOrEmpty(_key)) {
                _key = actionContext.RequestContext.Url.Request.RequestUri.LocalPath;
            }

            var cacheItem = CurrentCachePlatform.GetFromCache(_key);

            if (cacheItem != null) {
                var response = new HttpResponseMessage();

                response.Content = new ObjectContent(cacheItem.ItemType, cacheItem.ItemValue, new JsonMediaTypeFormatter());

                actionContext.Response = response;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
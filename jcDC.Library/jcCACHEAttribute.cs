using jcDC.Library.CachingPlatforms;
using jcDC.Library.Enums;

using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
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

                var selectedCachePlatformStr = ConfigurationManager.AppSettings["CachePlatform"];

                if (selectedCachePlatformStr != null) {
                    selectedCachePlatform = (CACHINGPLATFORMS)Enum.Parse(typeof(CACHINGPLATFORMS), selectedCachePlatformStr);
                }

                // todo replace with reflection
                switch(selectedCachePlatform) {
                    case CACHINGPLATFORMS.ASP:
                        _currentCachePlatform = new ASPCachePlatform();
                        break;
                    case CACHINGPLATFORMS.SQL:
                        _currentCachePlatform = new SQLCachePlatform();
                        break;
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
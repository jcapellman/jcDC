using jcDC.Library.Objects;

using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Caching;
using System.Web.Http.Controllers;

namespace jcDC.Library {
    public class jcCACHEAttribute : System.Web.Http.Filters.ActionFilterAttribute {
        public override void OnActionExecuting(HttpActionContext actionContext) {
            ObjectCache cache = MemoryCache.Default;
            
            var request = actionContext.RequestContext.Url.Request.RequestUri.LocalPath;
            
            var cacheItem = cache[request];

            if (cacheItem != null) {
                var response = new HttpResponseMessage();

                response.Content = new ObjectContent(((jcCACHEItem)cacheItem).ItemType, ((jcCACHEItem)cacheItem).ItemValue, new JsonMediaTypeFormatter());

                actionContext.Response = response;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http.Controllers;

namespace jcDC.Library {
    public class jcCACHEAttribute : System.Web.Http.Filters.ActionFilterAttribute {
        public override void OnActionExecuting(HttpActionContext actionContext) {
            ObjectCache cache = MemoryCache.Default;
            
            var request = actionContext.RequestContext.Url.Request.RequestUri.LocalPath;
            
            if (cache[request] != null) {
                actionContext.Response.Content = new StringContent(cache[request]);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
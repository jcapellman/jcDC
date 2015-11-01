using System.Collections.Generic;
using System.Web.Http;

using jcDC.Library;

namespace jcDC.Tests.WebAPI.Controllers {
    public class ValuesController : jcCACHEController {
        public enum REQUESTS {
            NO_CACHE,
            VALUES_GET,
            VALUES_ADD
        }

        public static List<int> val;

        [jcCACHE(REQUESTS.VALUES_GET)]
        public IEnumerable<int> Get() {
            return Return(val, REQUESTS.VALUES_GET, cacheObject: true);
        }

        public ValuesController() {
            if (val == null) {
                val = new List<int>();
            }
        }

        [HttpGet]
        [jcCACHE(REQUESTS.NO_CACHE, new string[] { "VALUES_GET" })]
        public bool Add(int a) {
            val.Add(a);

            return Return(true, REQUESTS.VALUES_ADD);
        }
    }
}
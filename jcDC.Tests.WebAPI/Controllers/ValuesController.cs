using System.Collections.Generic;
using System.Web.Http;

using jcDC.Library;

namespace jcDC.Tests.WebAPI.Controllers {
    public class ValuesController : jcCACHEController {
        public enum REQUESTS {
            NO_CACHE,
            VALUES_GET,
            VALUES_GET_ONE,
            VALUES_ADD,
            VALUES_DELETE
        }

        public static List<int> val;

        [jcCACHE(REQUESTS.VALUES_GET)]
        public IEnumerable<int> Get() {
            return Return(val, REQUESTS.VALUES_GET, cacheObject: true);
        }

        [jcCACHE(REQUESTS.VALUES_GET_ONE)]
        public int Get(int a) {
            return Return(val.IndexOf(a), REQUESTS.VALUES_GET_ONE, cacheObject: true);
        }

        public ValuesController() {
            if (val == null) {
                val = new List<int>();
            }
        }

        [HttpPost]
        [jcCACHE(REQUESTS.NO_CACHE, new string[] { "VALUES_GET" })]
        public bool Add(int a) {
            val.Add(a);

            return Return(true, REQUESTS.VALUES_ADD);
        }

        [HttpDelete]
        [jcCACHE(REQUESTS.NO_CACHE, new string[] { "VALUES_GET" })]
        public bool DELETE(int idx) {
            val.RemoveAt(idx);

            return Return(true, REQUESTS.VALUES_DELETE);
        }
    }
}
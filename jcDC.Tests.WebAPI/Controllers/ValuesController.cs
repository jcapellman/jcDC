using System;
using System.Collections.Generic;

using jcDC.Library;

namespace jcDC.Tests.WebAPI.Controllers {
    public class ValuesController : jcCACHEController {
        public enum REQUESTS {
            VALUES_GET
        }

        [jcCACHE(REQUESTS.VALUES_GET)]
        public IEnumerable<string> Get() {
            return Return(new string[] { DateTime.Now.ToString(), "value2" }, cacheObject: true);
        }
    }
}
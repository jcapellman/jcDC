using System;
using System.Collections.Generic;

using jcDC.Library;

namespace jcDC.Tests.WebAPI.Controllers {
    public class ValuesController : jcCACHEController {
        [jcCACHE]
        public IEnumerable<string> Get() {
            return Return(new string[] { DateTime.Now.ToString(), "value2" }, cacheObject: true);
        }
    }
}
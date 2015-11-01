using System;
using System.Collections.Generic;

using jcDC.Library;

namespace jcDC.Tests.WebAPI.Controllers {
    public class ValuesController : jcCACHEController {
        [jcCACHE]
        public IEnumerable<string> Get() {
            return Return("/api/Values", new string[] { DateTime.Now.ToString(), "value2" });
        }
    }
}
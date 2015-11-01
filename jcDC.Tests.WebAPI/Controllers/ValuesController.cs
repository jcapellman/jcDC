using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using jcDC.Library;

namespace jcDC.Tests.WebAPI.Controllers {
    public class ValuesController : jcCACHEController {
        // GET api/values
        [jcCACHE]
        public IEnumerable<string> Get() {
            return Return("/api/Values", new string[] { DateTime.Now.ToString(), "value2" });
        }
    }
}

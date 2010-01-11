<%@ WebHandler language="C#" class="%%NamespaceName%%.%%ClassName%%" %>

using System;
using System.Web;

namespace %%NamespaceName%% {

    public class %%ClassName%% : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            // TODO: Write request handling code here
        }

        public bool IsReusable {
            get {
                return true;
            }
        }
    }
}

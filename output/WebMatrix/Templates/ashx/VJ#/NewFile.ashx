<%@ WebHandler language="VJ#" class="%%NamespaceName%%.%%ClassName%%" %>

package %%NamespaceName%%;

import System.*;
import System.Web.*;

public class %%ClassName%% implements IHttpHandler {

    public void ProcessRequest(HttpContext context) {
        // TODO: Write request handling code here
    }

    public boolean get_IsReusable() {
        return true;
    }
}

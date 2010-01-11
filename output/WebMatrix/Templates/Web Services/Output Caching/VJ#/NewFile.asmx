<%@ WebService language="VJ#" class="%%ClassName%%" %>

import System.*;
import System.Web.Services.*;
import System.Xml.Serialization.*;

public class %%ClassName%% {

    /** @attribute WebMethod(CacheDuration=30)
    */
    public String TimeStampForOutputCache() {
        return "Output Cached at: " + DateTime.get_Now().ToString("r");
    }
}
<%@ WebService language="VJ#" class="%%ClassName%%" %>

import System.*;
import System.Web.Services.*;
import System.Xml.Serialization.*;

public class %%ClassName%% {

    /** @attribute WebMethod() 
    */
    public int Add(int a, int b) {
        return a + b;
    }
}

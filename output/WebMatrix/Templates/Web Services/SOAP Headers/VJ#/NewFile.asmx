<%@ WebService language="VJ#" class="%%ClassName%%" %>

import System.*;
import System.Web.Services.*;
import System.Xml.Serialization.*;
import System.Web.Services.Protocols.*;

public class SimpleHeader extends SoapHeader {
    public String Value;
}

public class %%ClassName%% {
  public SimpleHeader soapHeader;

    /** 
     * @attribute WebMethod()
     * @attribute SoapHeader("soapHeader")
     */
    public String GetValueOfSoapHeader(){
        if (null == soapHeader)
            return "SOAP Header is empty";
        else
            return soapHeader.Value;
    }
}
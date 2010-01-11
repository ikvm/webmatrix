<%@ WebService language="C#" class="%%ClassName%%" %>

using System;
using System.Web.Services;
using System.Xml.Serialization;
using System.Web.Services.Protocols;

public class SimpleHeader : SoapHeader {
    public String Value;
}

public class %%ClassName%% {
  public SimpleHeader soapHeader;

    [WebMethod]
    [SoapHeader("soapHeader")]
    public string GetValueOfSoapHeader(){
        if (null == soapHeader)
            return "SOAP Header is empty";
        else
            return soapHeader.Value;
    }
}
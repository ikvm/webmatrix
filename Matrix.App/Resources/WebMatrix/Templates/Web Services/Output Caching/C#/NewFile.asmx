<%@ WebService language="C#" class="%%ClassName%%" %>

using System;
using System.Web.Services;
using System.Xml.Serialization;

public class %%ClassName%% {

    [WebMethod(CacheDuration=30)]
    public string TimeStampForOutputCache() {
        return "Output Cached at: " + DateTime.Now.ToString("r");
    }
}
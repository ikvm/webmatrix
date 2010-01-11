<%@ WebService language="C#" class="%%ClassName%%" %>

using System;
using System.Web.Services;
using System.Xml.Serialization;

public class %%ClassName%% {

    [WebMethod]
    public int Add(int a, int b) {
        return a + b;
    }
}

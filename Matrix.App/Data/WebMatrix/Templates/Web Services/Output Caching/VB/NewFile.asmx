<%@ WebService language="VB" class="%%ClassName%%" %>

Imports System
Imports System.Web.Services
Imports System.Xml.Serialization

Public Class %%ClassName%%

    <WebMethod(CacheDuration:=30)> Public Function TimeStampForOutputCache() 
        Return "Output Cached at: " & DateTime.Now.ToString("r")
    End Function

End Class
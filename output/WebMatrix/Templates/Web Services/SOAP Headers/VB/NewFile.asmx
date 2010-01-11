<%@ WebService language="VB" class="%%ClassName%%" %>

Imports System
Imports System.Web.Services
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols

Public Class SimpleHeader
    Inherits SoapHeader

    Public Value As String
End Class

Public Class %%ClassName%%
   Public soapHeader As SimpleHeader

    <WebMethod, SoapHeader("soapHeader")> Public Function GetValueOfSoapHeader() 
        If (soapHeader Is Nothing)
            Return "SOAP Header is empty"
        Else
            Return soapHeader.Value
        End If
    End Function

End Class
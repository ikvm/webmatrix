<%@ WebService language="VB" class="%%ClassName%%" %>

Imports System
Imports System.Web.Services
Imports System.Xml.Serialization

Public Class %%ClassName%%

    <WebMethod> Public Function Add(a As Integer, b As Integer) As Integer 
        Return a + b
    End Function

End Class
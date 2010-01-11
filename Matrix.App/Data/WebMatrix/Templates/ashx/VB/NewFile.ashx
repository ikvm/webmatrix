<%@ WebHandler language="VB" class="%%NamespaceName%%.%%ClassName%%" %>

Imports System
Imports System.Web

Namespace %%NamespaceName%%

    Public Class %%ClassName%% : Implements IHttpHandler

        Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
            ' TODO: Write request handling code here
        End Sub

        Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

    End Class

End Namespace

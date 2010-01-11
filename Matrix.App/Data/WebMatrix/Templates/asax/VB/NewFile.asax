<%@ Application language="VB"%%ClassName, ClassName="{0}"%% %>

<script runat="server">

    Sub Application_Start(Sender As Object, E As EventArgs) 
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(Sender As Object, E As EventArgs) 
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(Sender As Object, E As EventArgs) 
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(Sender As Object, E As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(Sender As Object, E As EventArgs)
        ' Code that runs when a session ends
    End Sub
       
</script>

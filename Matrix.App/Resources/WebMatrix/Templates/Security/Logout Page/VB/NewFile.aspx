<%@ Page language="VB"%%ClassName, ClassName="{0}"%% %>

<script runat=server>

    Sub Page_Load(Sender As Object, E As EventArgs)

        If (Request.IsAuthenticated = true) Then
            Status.Text = "User " & User.Identity.Name & " is currently logged in."
        End If
        
    End Sub

    Sub LogOffBtn_Click(Sender As Object, E As EventArgs)

        FormsAuthentication.SignOut()
        Status.Text = "Not authenticated."

    End Sub

</script>

<html>
    <body style="font-family:arial">
        <form runat="server" ID="Form1">
            <h2>
                Log Off Page
            </h2>
            <hr size="1">
            <p>
                Status: <asp:Label id="Status" ForeColor="red" runat="server"></asp:Label>
            </p>
            <asp:button id="LogOffBtn" text="Log Off" OnClick="LogOffBtn_Click" runat="server"></asp:button>
        </form>
    </body>
</html>

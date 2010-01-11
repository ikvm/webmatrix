<%@ Page language="VB"%%ClassName, ClassName="{0}"%% %>

<script runat=server>

    Sub LoginBtn_Click(Sender As Object, E As EventArgs)

        If Page.IsValid Then
            If (UserName.Text = "jdoe@somewhere.com") And (UserPass.Text = "password") Then
                FormsAuthentication.RedirectFromLoginPage(UserName.Text, true)
            Else
                Msg.Text = "Invalid Credentials: Please try again"
            End If
        End If

    End Sub

</script>

<html>
    <body style="font-family:arial">
        <form runat="server">
            <h2>
                Login Page
            </h2>
            <hr size="1">
            <table>
                <tr>
                    <td>Username:</td>
                    <td><asp:TextBox id="UserName" runat="server"></asp:TextBox></td>
                    <td><asp:RequiredFieldValidator ControlToValidate="UserName" Display="Static" ErrorMessage="*" runat="server" ID="Requiredfieldvalidator1"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td><asp:TextBox id="UserPass" TextMode="Password" runat="server"></asp:TextBox></td>
                    <td><asp:RequiredFieldValidator ControlToValidate="UserPass" Display="Static" ErrorMessage="*" runat="server" ID="Requiredfieldvalidator2"></asp:RequiredFieldValidator></td>
                </tr>
            </table>
            <asp:button id="LoginBtn" text="Login" OnClick="LoginBtn_Click" runat="server"></asp:button>
            <p>
                <asp:Label id="Msg" ForeColor="red" runat="server"></asp:Label>
            </p>
        </form>
    </body>
</html>

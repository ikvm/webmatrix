<%@ Page language="C#"%%ClassName, ClassName="{0}"%% %>

<script runat="server">

    void LoginBtn_Click(Object sender, EventArgs e) {

        if (Page.IsValid) {
            if ((UserName.Text == "jdoe@somewhere.com") && (UserPass.Text == "password")) {
                FormsAuthentication.RedirectFromLoginPage(UserName.Text, true);
            }
            else {
                Msg.Text = "Invalid Credentials: Please try again";
            }
        }
    }

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
                    <td><asp:RequiredFieldValidator ControlToValidate="UserName" Display="Static" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td><asp:TextBox id="UserPass" TextMode="Password" runat="server"></asp:TextBox></td>
                    <td><asp:RequiredFieldValidator ControlToValidate="UserPass" Display="Static" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
                </tr>
            </table>
            <asp:button id="LoginBtn" text="Login" OnClick="LoginBtn_Click" runat="server"></asp:button>
            <p>
                <asp:Label id="Msg" ForeColor="red" runat="server"></asp:Label>
            </p>
        </form>
    </body>
</html>

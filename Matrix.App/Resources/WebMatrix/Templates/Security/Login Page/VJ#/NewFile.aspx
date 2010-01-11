<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>

<script runat="server">

    void LoginBtn_Click(Object sender, System.EventArgs e) {

        if (get_Page().get_IsValid()) {
            if ((String.Compare(UserName.get_Text(), "jdoe@somewhere.com") == 0 ) && (String.Compare(UserPass.get_Text(), "password")== 0)) {
                FormsAuthentication.RedirectFromLoginPage(UserName.get_Text(), true);
            }
            else {
                Msg.set_Text("Invalid Credentials: Please try again");
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

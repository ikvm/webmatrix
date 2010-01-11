<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>

<script runat=server>

    void Page_Load(Object sender, System.EventArgs e) {

        if (get_Request().get_IsAuthenticated()) {
            Status.set_Text("User " + get_User().get_Identity().get_Name() + " is currently logged in.");
        }
    }

    void LogOffBtn_Click(Object sender, System.EventArgs e) {

        FormsAuthentication.SignOut();
        Status.set_Text("Not authenticated.");
    }

</script>

<html>
    <body style="font-family:arial">
        <form runat="server">
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

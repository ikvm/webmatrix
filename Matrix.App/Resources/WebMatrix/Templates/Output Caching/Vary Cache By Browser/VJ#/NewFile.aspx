<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>
<%@ OutputCache Duration="10" VaryByParam="none" VaryByCustom="browser" %>
<script runat="server">

    public void Page_Load(Object sender, System.EventArgs e) {

        BrowserDetails.set_Text(get_Request().get_Browser().get_Browser() + " " + ((System.Int32)get_Request().get_Browser().get_MajorVersion()).ToString());
        TimestampCreated.set_Text(DateTime.get_Now().ToString("r"));
        TimestampExpires.set_Text(DateTime.get_Now().AddSeconds(10).ToString("r"));
    }

</script>
<html>
    <body style="font-family:arial">
        <h2>
            Vary Cache by Browser
        </h2>
        <hr size="1">
        <p>
            Varying output by browser:
            <asp:Label id="BrowserDetails" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
            <br>
            Output Cache created:
            <asp:Label id="TimestampCreated" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
            <br>
            Output Cache expires:
            <asp:Label id="TimestampExpires" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
        </p>
    </body>
</html>

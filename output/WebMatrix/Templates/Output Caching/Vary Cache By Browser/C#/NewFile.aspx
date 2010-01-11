<%@ Page language="C#"%%ClassName, ClassName="{0}"%% %>
<%@ OutputCache Duration="10" VaryByParam="none" VaryByCustom="browser" %>
<script runat="server">

    public void Page_Load(Object sender, EventArgs e) {

        BrowserDetails.Text = Request.Browser.Browser + " " + Request.Browser.MajorVersion.ToString();
        TimestampCreated.Text = DateTime.Now.ToString("r");
        TimestampExpires.Text = DateTime.Now.AddSeconds(10).ToString("r");
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

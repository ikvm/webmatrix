<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>
<%@ OutputCache Duration="10" VaryByParam="none"%>

<script runat="server">

    void Page_Load(Object sender, System.EventArgs e) {

        TimestampCreated.set_Text(DateTime.get_Now().ToString("r"));
        TimestampExpires.set_Text(DateTime.get_Now().AddSeconds(10).ToString("r"));
    }

</script>

<html>
    <body style="font-family:arial">
        <h2>
            Output caching for 10 seconds...
        </h2>
        <hr size="1">
        <p>
            Output Cache created:
            <asp:Label id="TimestampCreated" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
            <br>
            Output Cache expires:
            <asp:Label id="TimestampExpires" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
        </p>
    </body>
</html>

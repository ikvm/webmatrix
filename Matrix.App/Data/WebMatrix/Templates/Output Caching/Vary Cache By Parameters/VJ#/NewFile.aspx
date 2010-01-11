<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>
<%@ OutputCache Duration="100" VaryByParam="Category"%>
<script runat="server">

    void Page_Load(Object sender, System.EventArgs e) {

        TimestampCreated.set_Text(DateTime.get_Now().ToString("r"));
        TimestampExpires.set_Text(DateTime.get_Now().AddSeconds(10).ToString("r"));
    }

    void Button1_Click(Object sender, System.EventArgs e) {

        CategoryItem.set_Text("You selected: " + Category.get_SelectedItem().get_Text());
    }

</script>

<html>
    <body style="font-family:arial">
        <form runat="server">
            <h2>
                Vary Cache By Parameters
            </h2>
            <asp:Label id="CategoryItem" runat="server"></asp:Label>
            <hr size="1">
            Category:
            <asp:DropDownList id="Category" runat="server">
                <asp:ListItem value="default">-- Select Category --</asp:ListItem>
                <asp:ListItem>psychology</asp:ListItem>
                <asp:ListItem>business</asp:ListItem>
                <asp:ListItem value="Popular Computer">popular_comp</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="Button1" Text="Lookup" OnClick="Button1_Click" runat="server"></asp:Button>
            <p>
                Output Cache created:
                <asp:Label id="TimestampCreated" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
                <br>
                Output Cache expires:
                <asp:Label id="TimestampExpires" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
            </p>
        </form>
    </body>
</html>
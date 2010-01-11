<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    void Page_Load(Object sender, System.EventArgs e) {

		if (!get_Page().get_IsPostBack()) 
		{
    
			// Databind the filter dropdown on the first request only
			// (viewstate will restore these values on subsequent postbacks).
    
			// TODO: update the ConnectionString and CommandText values for your application
			String ConnectionString = "server=(local);database=pubs;trusted_connection=true";
			String CommandText = "select distinct au_lname from Authors";
    
			SqlConnection myConnection = new SqlConnection(ConnectionString);
			SqlCommand myCommand = new SqlCommand(CommandText, myConnection);
    
			// TODO: Update the DataTextField value
			DropDownList1.set_DataTextField("au_lname");
    
			myConnection.Open();
    
			DropDownList1.set_DataSource(myCommand.ExecuteReader(CommandBehavior.CloseConnection));
			DropDownList1.DataBind();
    
			// insert an "All" item at the beginning of the list
			DropDownList1.get_Items().Insert(0, "-- All Authors --");
		}
	}
    
	void ApplyFilter_Click(Object sender, System.EventArgs e) {
    
		// TODO: update the ConnectionString value for your application
		String ConnectionString = "server=(local);database=pubs;trusted_connection=true";
		String CommandText;
    
		// get the filter value from the DropDownList
		String filterValue = DropDownList1.get_SelectedItem().get_Text().Replace("'", "''");
    
		// TODO: update the CommandText value for your application
		if (String.Compare(filterValue,"-- All Authors --") == 0)
			CommandText = "select distinct title as Title, price as Price, ytd_sales as [YTD Sales] from titleview";
		else
			CommandText = "select title as Title, price as Price, ytd_sales as [YTD Sales] from titleview where au_lname = '" + filterValue + "'";
    
		SqlConnection myConnection = new SqlConnection(ConnectionString);
		SqlCommand myCommand = new SqlCommand(CommandText, myConnection);
    
		myConnection.Open();
    
		DataGrid1.set_DataSource(myCommand.ExecuteReader(CommandBehavior.CloseConnection));
		DataGrid1.DataBind();
	}


</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Filtered Data Report
        </h2>
        <hr size="1">
        <form runat="server">
            <p>
                Select an Author: 
                <asp:DropDownList id="DropDownList1" runat="server"></asp:DropDownList>
                &nbsp;
                <asp:Button id="Button1" onclick="ApplyFilter_Click" Text="Show Titles" runat="server"></asp:Button>
            </p>
            <asp:datagrid id="DataGrid1" EnableViewState="False" runat="server" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1">
                <HeaderStyle Font-Bold="True" ForeColor="white" BackColor="#4A3C8C"></HeaderStyle>
                <ItemStyle BackColor="#DEDFDE"></ItemStyle>
            </asp:datagrid>
        </form>
    </body>
</html>

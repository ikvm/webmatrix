<%@ Page language="C#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    void Page_Load(object sender, EventArgs e) {

        if (!Page.IsPostBack) {
    
            // Databind the filter dropdown on the first request only
            // (viewstate will restore these values on subsequent postbacks).
    
            // TODO: update the ConnectionString and CommandText values for your application
            string ConnectionString = "server=(local);database=pubs;trusted_connection=true";
            string CommandText = "select distinct au_lname from Authors";
    
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlCommand myCommand = new SqlCommand(CommandText, myConnection);
    
            // TODO: Update the DataTextField value
            DropDownList1.DataTextField = "au_lname";
    
            myConnection.Open();
    
            DropDownList1.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            DropDownList1.DataBind();
    
            // insert an "All" item at the beginning of the list
            DropDownList1.Items.Insert(0, "-- All Authors --");
        }
    }
    
    void ApplyFilter_Click(Object sender, EventArgs e) {
    
        // TODO: update the ConnectionString value for your application
        string ConnectionString = "server=(local);database=pubs;trusted_connection=true";
        string CommandText;
    
        // get the filter value from the DropDownList
        string filterValue = DropDownList1.SelectedItem.Text.Replace("'", "''");
    
        // TODO: update the CommandText value for your application
        if (filterValue == "-- All Authors --")
            CommandText = "select distinct title as Title, price as Price, ytd_sales as [YTD Sales] from titleview";
        else
            CommandText = "select title as Title, price as Price, ytd_sales as [YTD Sales] from titleview where au_lname = '" + filterValue + "'";
    
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        SqlCommand myCommand = new SqlCommand(CommandText, myConnection);
    
        myConnection.Open();
    
        DataGrid1.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
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

<%@ Page language="C#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    void Page_Load(object sender, EventArgs e) {

        if (!Page.IsPostBack) {
        
            // Databind the data grid on the first request only
            // (on postback, rebind only in paging command)
            
            BindGrid();
        }
    }
    
    void DataGrid_Page(object sender, DataGridPageChangedEventArgs e) {
    
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        BindGrid();
    }

    void BindGrid() {
    
        // TODO: update the ConnectionString and CommandText values for your application
        string ConnectionString = "server=(local);database=pubs;trusted_connection=true";
        string CommandText = "select au_lname, au_fname, address, city, state from Authors order by au_lname";
        
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        SqlDataAdapter myCommand = new SqlDataAdapter(CommandText, myConnection);

        DataSet ds = new DataSet();
        myCommand.Fill(ds);

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();   
    }
    
</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Data Report with Paging
        </h2>
        <hr size="1">
        <form runat="server">
            <asp:datagrid id="DataGrid1" AllowPaging="true" PageSize="6" OnPageIndexChanged="DataGrid_Page" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1" width="80%" runat="server">
                <HeaderStyle Font-Bold="True" ForeColor="white" BackColor="#4A3C8C"></HeaderStyle>
                <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" Mode="NumericPages"></PagerStyle>
                <ItemStyle BackColor="#DEDFDE"></ItemStyle>
            </asp:datagrid>
        </form>
    </body>
</html>

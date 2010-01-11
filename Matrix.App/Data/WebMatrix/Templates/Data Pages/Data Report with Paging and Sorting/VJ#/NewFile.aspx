<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    void Page_Load(Object sender, System.EventArgs e) {

        if (!get_Page().get_IsPostBack()) {
        
            // Databind the data grid on the first request only
            // (on postback, rebind only in paging and sorting commands)
            
            BindGrid();
        }
    }
    
    void DataGrid_Page(Object sender, DataGridPageChangedEventArgs e) {
    
        DataGrid1.set_CurrentPageIndex(e.get_NewPageIndex());
        BindGrid();
    }

    void DataGrid_Sort(Object sender, DataGridSortCommandEventArgs e) {
    
        DataGrid1.set_CurrentPageIndex(0);       
        this.set_SortField(e.get_SortExpression());
        BindGrid();
    }
    
    //---------------------------------------------------------
    //
    // Helpers
    //
    // use a property to keep track of the sort field, and
    // save it in viewstate between postbacks

	/** @property
	*/
    protected String get_SortField(){

            Object o = get_ViewState().get_Item("SortField");
            return (o == null) ? System.String.Empty : (String)o;
    }
    /** @property
	*/
    protected void set_SortField(String value) {

            get_ViewState().set_Item("SortField", value);
    }

    void BindGrid() {
    
        // TODO: update the ConnectionString value for your application
        String ConnectionString = "server=(local);database=pubs;trusted_connection=true";
        String CommandText;
        
        // TODO: update the CommandText value for your application
        if (this.get_SortField() == System.String.Empty)
            CommandText = "select au_lname, au_fname, address, city, state from Authors order by au_lname";
        else
            CommandText = "select au_lname, au_fname, address, city, state from Authors order by " + this.get_SortField();
        
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        SqlDataAdapter myCommand = new SqlDataAdapter(CommandText, myConnection);

        DataSet ds = new DataSet();
        myCommand.Fill(ds);

        DataGrid1.set_DataSource(ds);
        DataGrid1.DataBind();   
    }
    
</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Data Report with Paging and Sorting
        </h2>
        <hr size="1">
        <form runat="server">
            <asp:datagrid id="DataGrid1" AllowSorting="true" OnSortCommand="DataGrid_Sort" AllowPaging="true" PageSize="6" OnPageIndexChanged="DataGrid_Page" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1" width="80%" runat="server">
                <HeaderStyle Font-Bold="True" ForeColor="white" BackColor="#4A3C8C"></HeaderStyle>
                <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" Mode="NumericPages"></PagerStyle>
                <ItemStyle BackColor="#DEDFDE"></ItemStyle>
            </asp:datagrid>
        </form>
    </body>
</html>

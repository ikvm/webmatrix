<%@ Page language="C#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    private void Page_Load(object sender, EventArgs e) {

        if (!Page.IsPostBack) {
            // Databind the master grid on the first request only
            // (viewstate will restore these values on subsequent postbacks).
            
            MasterGrid.SelectedIndex = 0;
            BindMasterGrid();
            BindDetailGrid();
        }
    }
    
    private void MasterGrid_SelectedIndexChanged(object sender, EventArgs e) {
        BindDetailGrid();
    }
    
    private void MasterGrid_PageIndexChanged(object sender, DataGridPageChangedEventArgs e) {
        if (MasterGrid.SelectedIndex != -1) {
            // unset the selection, details view
            MasterGrid.SelectedIndex = -1;
            BindDetailGrid();
        }

        MasterGrid.CurrentPageIndex = e.NewPageIndex;
        BindMasterGrid();
    }
    
    private void BindMasterGrid() {

        // TODO: Update the ConnectionString and CommandText values for your application
        string ConnectionString = "server=(local);database=pubs;trusted_connection=true";
        string CommandText = "select au_lname as [Last Name], au_fname as [First Name], Address, City, State from Authors order by [Last Name]";
        
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        SqlDataAdapter myCommand = new SqlDataAdapter(CommandText, myConnection);

        DataSet ds = new DataSet();
        myCommand.Fill(ds);

        MasterGrid.DataSource = ds;
        MasterGrid.DataBind();   
    }

    private void BindDetailGrid() {
        // get the filter value from the master Grid's DataKeys collection
        if (MasterGrid.SelectedIndex != -1) {
            // TODO: update the ConnectionString value for your application
            string ConnectionString = "server=(local);database=pubs;trusted_connection=true";
    
            // TODO: update the CommandText value for your application
            string filterValue = ((string)MasterGrid.DataKeys[MasterGrid.SelectedIndex]).Replace("'", "''");
            string CommandText = "select title as Title, price as Price, ytd_sales as [YTD Sales] from titleview where au_lname = '" + filterValue + "'";
    
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlCommand myCommand = new SqlCommand(CommandText, myConnection);
        
            myConnection.Open();
        
            DetailsGrid.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }
        
        DetailsGrid.DataBind();
    }

</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Master - Detail Grids
        </h2>
        <hr size="1">
        <form runat="server">
            <p>
            <asp:datagrid id="MasterGrid" DataKeyField="Last Name" OnSelectedIndexChanged="MasterGrid_SelectedIndexChanged" AllowPaging="true" PageSize="6" OnPageIndexChanged="MasterGrid_PageIndexChanged" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1" width="80%" runat="server">
                <HeaderStyle Font-Bold="True" ForeColor="white" BackColor="#4A3C8C"></HeaderStyle>
                <SelectedItemStyle ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
                <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" Mode="NumericPages"></PagerStyle>
                <ItemStyle BackColor="#DEDFDE"></ItemStyle>
                <Columns>
                    <asp:buttoncolumn text="Show details" commandname="Select" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="smaller"></asp:buttoncolumn>
                </Columns>               
            </asp:datagrid>
            <br><br>
            <asp:datagrid id="DetailsGrid" EnableViewState="False" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1" width="80%" runat="server">
                <HeaderStyle Font-Bold="True" ForeColor="white" BackColor="#4A3C8C"></HeaderStyle>
                <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" Mode="NumericPages"></PagerStyle>
                <ItemStyle BackColor="#DEDFDE"></ItemStyle>
            </asp:datagrid>
            </p>
        </form>
    </body>
</html>

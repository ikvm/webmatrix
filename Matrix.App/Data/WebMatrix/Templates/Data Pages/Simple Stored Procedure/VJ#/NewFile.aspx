<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    void Page_Load(Object sender, System.EventArgs e) {

		// TODO: Update the ConnectionString for your application
		String ConnectionString = "server=(local);database=Northwind;trusted_connection=true";

		// TODO: Updatd the name of the Stored Procedure for your application
		String CommandText = "CustOrdersDetail";
        
		SqlConnection myConnection = new SqlConnection(ConnectionString);
		SqlCommand myCommand = new SqlCommand(CommandText, myConnection);
		
		myCommand.set_CommandType(CommandType.StoredProcedure);

		// TODO: Set the input parameter, if necessary, for your application
		myCommand.get_Parameters().Add("@OrderId", SqlDbType.Int).set_Value((System.Int32)11077);



		myConnection.Open();

		DataGrid1.set_DataSource(myCommand.ExecuteReader(CommandBehavior.CloseConnection));
		DataGrid1.DataBind();   
	}
    
</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Simple Stored Procedure
        </h2>
        <hr size="1">
        <form runat="server">
            <asp:datagrid id="DataGrid1" EnableViewState="False" runat="server" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1">
                <HeaderStyle Font-Bold="True" ForeColor="white" BackColor="#4A3C8C"></HeaderStyle>
                <ItemStyle BackColor="#DEDFDE"></ItemStyle>
            </asp:datagrid>
        </form>
    </body>
</html>

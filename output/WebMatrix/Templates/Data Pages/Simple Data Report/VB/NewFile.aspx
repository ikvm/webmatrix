<%@ Page Language="VB"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    Sub Page_Load(Sender As Object, E As EventArgs)

        ' TODO: Update the ConnectionString and CommandText values for your application
        Dim ConnectionString As String = "server=(local);database=pubs;trusted_connection=true"
        Dim CommandText As String = "select au_lname as [Last Name], au_fname as [First Name], Address, City, State from Authors"
        
        Dim myConnection As New SqlConnection(ConnectionString)
        Dim myCommand As New SqlCommand(CommandText, myConnection)

        myConnection.Open()

        DataGrid1.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
        DataGrid1.DataBind()

    End Sub
    
</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Simple Data Report
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

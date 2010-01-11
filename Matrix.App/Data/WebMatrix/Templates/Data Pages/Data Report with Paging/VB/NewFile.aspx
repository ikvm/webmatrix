<%@ Page language="VB"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    Sub Page_Load(Sender As Object, E As EventArgs)

        If Not Page.IsPostBack Then

            ' Databind the data grid on the first request only
            ' (on postback, rebind only in paging command)

            BindGrid()

        End If

    End Sub

    Sub DataGrid_Page(Sender As Object, e As DataGridPageChangedEventArgs)

        DataGrid1.CurrentPageIndex = e.NewPageIndex
        BindGrid()

    End Sub

    Sub BindGrid()

        ' TODO: update the ConnectionString and CommandText values for your application
        Dim ConnectionString As String = "server=(local);database=pubs;trusted_connection=true"
        Dim CommandText As String = "select au_lname, au_fname, address, city, state from Authors order by au_lname"

        Dim myConnection As New SqlConnection(ConnectionString)
        Dim myCommand As New SqlDataAdapter(CommandText, myConnection)

        Dim ds As New DataSet()
        myCommand.Fill(ds)

        DataGrid1.DataSource = ds
        DataGrid1.DataBind()

    End Sub

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

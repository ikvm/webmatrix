<%@ Page language="VB"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    Sub Page_Load(Sender As Object, E As EventArgs)
    
        If Not Page.IsPostBack Then
    
            ' Databind the master grid on the first request only
            ' (viewstate will restore these values on subsequent postbacks).
            
            MasterGrid.SelectedIndex = 0
            BindMasterGrid()
            BindDetailGrid()
            
        End If

    End Sub
    
    Sub MasterGrid_Select(Sender As Object, E As EventArgs)
        BindDetailGrid()
    End Sub
    
    Sub MasterGrid_Page(Sender As Object, E As DataGridPageChangedEventArgs)
    
        If MasterGrid.SelectedIndex <> -1 Then

            ' unset the selection, details view
            MasterGrid.SelectedIndex = -1
            BindDetailGrid()
        
        End If

        MasterGrid.CurrentPageIndex = e.NewPageIndex
        BindMasterGrid()
        
    End Sub
    
    Sub BindMasterGrid()

        ' TODO: Update the ConnectionString and CommandText values for your application
        Dim ConnectionString As String = "server=(local);database=pubs;trusted_connection=true"
        Dim CommandText As String = "select au_lname as [Last Name], au_fname as [First Name], Address, City, State from Authors order by [Last Name]"
        
        Dim myConnection As New SqlConnection(ConnectionString)
        Dim myCommand As New SqlDataAdapter(CommandText, myConnection)

        Dim ds As New DataSet()
        myCommand.Fill(ds)

        MasterGrid.DataSource = ds
        MasterGrid.DataBind()
        
    End Sub

    Sub BindDetailGrid()

        ' get the filter value from the master Grid's DataKeys collection
        If MasterGrid.SelectedIndex <> -1 Then
        
            ' TODO: update the ConnectionString value for your application
            Dim ConnectionString As String = "server=(local);database=pubs;trusted_connection=true"
    
            ' TODO: update the CommandText value for your application
            Dim filterValue As String = CStr(MasterGrid.DataKeys(MasterGrid.SelectedIndex)).Replace("'", "''")
            Dim CommandText As String = "select title as Title, price as Price, ytd_sales as [YTD Sales] from titleview where au_lname = '" & filterValue & "'"
    
            Dim myConnection As New SqlConnection(ConnectionString)
            Dim myCommand As New SqlCommand(CommandText, myConnection)
        
            myConnection.Open()
        
            DetailsGrid.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            
        End If
        
        DetailsGrid.DataBind()
        
    End Sub

</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Master - Detail Grids
        </h2>
        <hr size="1">
        <form runat="server">
            <p>
            <asp:datagrid id="MasterGrid" DataKeyField="Last Name" OnSelectedIndexChanged="MasterGrid_Select" AllowPaging="true" PageSize="6" OnPageIndexChanged="MasterGrid_Page" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1" width="80%" runat="server">
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


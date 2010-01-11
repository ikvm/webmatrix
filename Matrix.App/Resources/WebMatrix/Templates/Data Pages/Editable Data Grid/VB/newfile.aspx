<%@ Page language="VB"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    ' TODO: update the ConnectionString and Command values for your application
    
    Dim ConnectionString As String = "server=(local);database=pubs;trusted_connection=true"
    Dim SelectCommand As String = "SELECT au_id, au_lname, au_fname from Authors"
    
    Dim isEditing As Boolean = False

    Sub Page_Load(Sender As Object, E As EventArgs)

        If Not Page.IsPostBack Then

            ' Databind the data grid on the first request only
            ' (on postback, bind only in editing, paging and sorting commands)

            BindGrid()
        
        End If
        
    End Sub

    ' ---------------------------------------------------------------
    '
    ' DataGrid Commands: Page, Sort, Edit, Update, Cancel, Delete
    '

    Sub DataGrid_ItemCommand(Sender As Object, E As DataGridCommandEventArgs)

        ' this event fires prior to all of the other commands
        ' use it to provide a more graceful transition out of edit mode
        
        CheckIsEditing(e.CommandName)
        
    End Sub
    
    Sub CheckIsEditing(commandName As String)
    
        If DataGrid1.EditItemIndex <> -1 Then
        
            ' we are currently editing a row
            If commandName <> "Cancel" And commandName <> "Update" Then
            
                ' user's edit changes (If any) will not be committed
                Message.Text = "Your changes have not been saved yet.  Please press update to save your changes, or cancel to discard your changes, before selecting another item."
                isEditing = True
                
            End If
            
        End If
        
    End Sub

    Sub DataGrid_Edit(Sender As Object, E As DataGridCommandEventArgs)

        ' turn on editing for the selected row
        
        If Not isEditing Then
        
            DataGrid1.EditItemIndex = e.Item.ItemIndex
            BindGrid()
            
        End If
        
    End Sub

    Sub DataGrid_Update(Sender As Object, E As DataGridCommandEventArgs)

        ' update the database with the new values
        
        ' get the edit text boxes
        Dim id As String = CType(e.Item.Cells(2).Controls(0), TextBox).Text
        Dim lname As String = CType(e.Item.Cells(3).Controls(0), TextBox).Text
        Dim fname As String = CType(e.Item.Cells(4).Controls(0), TextBox).Text
        
        ' TODO: update the Command value for your application
        Dim myConnection As New SqlConnection(ConnectionString)
        Dim UpdateCommand As SqlCommand = new SqlCommand()
        UpdateCommand.Connection = myConnection
        
        If AddingNew = True Then
            UpdateCommand.CommandText = "INSERT INTO authors(au_id, au_lname, au_fname, contract) VALUES (@au_id, @au_lname, @au_fname, 0)"
        Else 
            UpdateCommand.CommandText = "UPDATE authors SET au_lname = @au_lname, au_fname = @au_fname WHERE au_id = @au_id"
        End If

        UpdateCommand.Parameters.Add("@au_id", SqlDbType.VarChar, 11).Value = id
        UpdateCommand.Parameters.Add("@au_lname", SqlDbType.VarChar, 40).Value = lname
        UpdateCommand.Parameters.Add("@au_fname", SqlDbType.VarChar, 20).Value = fname
        
        ' execute the command
        Try
            myConnection.Open()
            UpdateCommand.ExecuteNonQuery()
            
        Catch ex as Exception
            Message.Text = ex.ToString()
        
        Finally
            myConnection.Close()
            
        End Try
        
        ' Resort the grid for new records
        If AddingNew = True Then        
            DataGrid1.CurrentPageIndex = 0
            AddingNew = false
        End If
        
        ' rebind the grid
        DataGrid1.EditItemIndex = -1
        BindGrid()
        
    End Sub

    Sub DataGrid_Cancel(Sender As Object, E As DataGridCommandEventArgs)

        ' cancel editing
        
        DataGrid1.EditItemIndex = -1
        BindGrid()
        
        AddingNew = False
        
    End Sub

    Sub DataGrid_Delete(Sender As Object, E As DataGridCommandEventArgs)

        ' delete the selected row
        
        If Not isEditing Then
        
            ' the key value for this row is in the DataKeys collection 
            Dim keyValue As String = CStr(DataGrid1.DataKeys(e.Item.ItemIndex))
        
            ' TODO: update the Command value for your application
            Dim myConnection As New SqlConnection(ConnectionString)
            Dim DeleteCommand As New SqlCommand("DELETE from authors where au_id='" & keyValue & "'", myConnection)
            
            ' execute the command
            myConnection.Open()
            DeleteCommand.ExecuteNonQuery()
            myConnection.Close()
           
            ' rebind the grid
            DataGrid1.CurrentPageIndex = 0
            DataGrid1.EditItemIndex = -1
            BindGrid()
            
        End If
        
    End Sub

    Sub DataGrid_Page(Sender As Object, E As DataGridPageChangedEventArgs)

        ' display a new page of data
        
        If Not isEditing Then
        
            DataGrid1.EditItemIndex = -1            
            DataGrid1.CurrentPageIndex = e.NewPageIndex
            BindGrid()
            
        End If
        
    End Sub

    Sub AddNew_Click(Sender As Object, E As EventArgs)
    
        ' add a new row to the end of the data, and set editing mode 'on'
        
        CheckIsEditing("")
        
        If Not isEditing = True Then

            ' set the flag so we know to do an insert at Update time
            AddingNew = True

            ' add new row to the end of the dataset after binding

            ' first get the data
            Dim myConnection As New SqlConnection(ConnectionString)
            Dim myCommand As New SqlDataAdapter(SelectCommand, myConnection)

            Dim ds As New DataSet()
            myCommand.Fill(ds)

            ' add a new blank row to the end of the data
            Dim rowValues As Object() = {"", "", ""}
            ds.Tables(0).Rows.Add(rowValues)

            ' figure out the EditItemIndex, last record on last page
            Dim recordCount As Integer = ds.Tables(0).Rows.Count
            
            If recordCount > 1 Then
            
                recordCount -= 1
                DataGrid1.CurrentPageIndex = recordCount \ DataGrid1.PageSize
                DataGrid1.EditItemIndex = recordCount Mod DataGrid1.PageSize

            End If

            ' databind
            DataGrid1.DataSource = ds
            DataGrid1.DataBind()

        End If

        
    End Sub

    ' ---------------------------------------------------------------
    '
    ' Helpers Methods:
    '
    
    ' property to keep track of whether we are adding a new record,
    ' and save it in viewstate between postbacks
    
    Property AddingNew() As Boolean

        Get
            Dim o As Object = ViewState("AddingNew")
            If o Is Nothing Then
                Return False
            End If
            Return CBool(o)
        End Get

        Set(ByVal Value As Boolean)
            ViewState("AddingNew") = Value
        End Set

    End Property
    
    Sub BindGrid()

        Dim myConnection As New SqlConnection(ConnectionString)
        Dim myCommand As New SqlDataAdapter(SelectCommand, myConnection)

        Dim ds As New DataSet()
        myCommand.Fill(ds)

        DataGrid1.DataSource = ds
        DataGrid1.DataBind()
        
    End Sub
    
</script>
    
<html>
    <body style="font-family:arial">
        <h2>
            Editable Data Grid
        </h2>
        <hr size="1">
        <form runat="server">
            <asp:datagrid id="DataGrid1" DataKeyField="au_id" OnItemCommand="DataGrid_ItemCommand" OnEditCommand="DataGrid_Edit" OnUpdateCommand="DataGrid_Update" OnCancelCommand="DataGrid_Cancel" OnDeleteCommand="DataGrid_Delete" AllowPaging="true" PageSize="6" OnPageIndexChanged="DataGrid_Page" ForeColor="Black" BackColor="White" CellPadding="3" GridLines="None" CellSpacing="1" width="80%" runat="server">
                <HeaderStyle Font-Bold="True" ForeColor="white" BackColor="#4A3C8C"></HeaderStyle>
                <PagerStyle HorizontalAlign="Right" BackColor="#C6C3C6" mode="NumericPages" Font-Size="smaller"></PagerStyle>
                <ItemStyle BackColor="#DEDFDE"></ItemStyle>
                <FooterStyle BackColor="#C6C3C6"></FooterStyle>
                <Columns>
                    <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit" ItemStyle-Font-Size="smaller" ItemStyle-Width="10%"></asp:EditCommandColumn>
                    <asp:ButtonColumn Text="Delete" CommandName="Delete" ItemStyle-Font-Size="smaller" ItemStyle-Width="10%"></asp:ButtonColumn>
                </Columns>
            </asp:datagrid>
            <br>
            <asp:LinkButton id="LinkButton1" Text="Add new item" OnClick="AddNew_Click" Font-Size="smaller" runat="server"></asp:LinkButton>
            <br>
            <br>
            <asp:Label id="Message" ForeColor="red" EnableViewState="false" width="80%" runat="server"></asp:Label>  
        </form>
    </body>
</html>

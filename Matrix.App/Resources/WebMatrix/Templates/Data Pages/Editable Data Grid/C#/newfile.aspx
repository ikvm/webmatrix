<%@ Page language="C#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    // TODO: update the ConnectionString and Command values for your application
    
    string ConnectionString = "server=(local);database=pubs;trusted_connection=true";
    string SelectCommand = "SELECT au_id, au_lname, au_fname from Authors";
    
    bool isEditing = false;

    void Page_Load(object sender, EventArgs e) {

        if (!Page.IsPostBack) {

            // Databind the data grid on the first request only
            // (on postback, bind only in editing, paging and sorting commands)

            BindGrid();
        }
    }

    // ---------------------------------------------------------------
    //
    // DataGrid Commands: Page, Sort, Edit, Update, Cancel, Delete
    //

    void DataGrid_ItemCommand(object sender, DataGridCommandEventArgs e) {

        // this event fires prior to all of the other commands
        // use it to provide a more graceful transition out of edit mode
        
        CheckIsEditing(e.CommandName);
    }
    
    void CheckIsEditing(string commandName) {
    
        if (DataGrid1.EditItemIndex != -1) {
        
            // we are currently editing a row
            if (commandName != "Cancel" && commandName != "Update") {
            
                // user's edit changes (if any) will not be committed
                Message.Text = "Your changes have not been saved yet.  Please press update to save your changes, or cancel to discard your changes, before selecting another item.";
                isEditing = true;
            }
        }
    }

    void DataGrid_Edit(object sender, DataGridCommandEventArgs e) {

        // turn on editing for the selected row
        
        if (!isEditing) {
            DataGrid1.EditItemIndex = e.Item.ItemIndex;
            BindGrid();
        }
    }

    void DataGrid_Update(object sender, DataGridCommandEventArgs e) {

        // update the database with the new values
        
        // get the edit text boxes
        string id = ((TextBox)e.Item.Cells[2].Controls[0]).Text;
        string lname = ((TextBox)e.Item.Cells[3].Controls[0]).Text;
        string fname = ((TextBox)e.Item.Cells[4].Controls[0]).Text;
        
        // TODO: update the Command value for your application
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        SqlCommand UpdateCommand = new SqlCommand();
        UpdateCommand.Connection = myConnection;
        
        if (AddingNew) 
            UpdateCommand.CommandText = "INSERT INTO authors(au_id, au_lname, au_fname, contract) VALUES (@au_id, @au_lname, @au_fname, 0)";
        else 
            UpdateCommand.CommandText = "UPDATE authors SET au_lname = @au_lname, au_fname = @au_fname WHERE au_id = @au_id";

        UpdateCommand.Parameters.Add("@au_id", SqlDbType.VarChar, 11).Value = id;
        UpdateCommand.Parameters.Add("@au_lname", SqlDbType.VarChar, 40).Value = lname;
        UpdateCommand.Parameters.Add("@au_fname", SqlDbType.VarChar, 20).Value = fname;
        
        // execute the command
        try {
            myConnection.Open();
            UpdateCommand.ExecuteNonQuery();
        }
        catch (Exception ex) {
            Message.Text = ex.ToString();
        }
        finally {
            myConnection.Close();
        }
        
        // Resort the grid for new records
        if (AddingNew) {
        
            DataGrid1.CurrentPageIndex = 0;
            AddingNew = false;
        }
        
        // rebind the grid
        DataGrid1.EditItemIndex = -1;
        BindGrid();
    }

    void DataGrid_Cancel(object sender, DataGridCommandEventArgs e) {

        // cancel editing
        
        DataGrid1.EditItemIndex = -1;
        BindGrid();
        
        AddingNew = false;
    }

    void DataGrid_Delete(object sender, DataGridCommandEventArgs e) {

        // delete the selected row
        
        if (!isEditing) {
        
            // the key value for this row is in the DataKeys collection 
            string keyValue = (string)DataGrid1.DataKeys[e.Item.ItemIndex];
        
            // TODO: update the Command value for your application
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlCommand DeleteCommand = new SqlCommand("DELETE from authors where au_id='" + keyValue + "'", myConnection);
            
            // execute the command
            myConnection.Open();
            DeleteCommand.ExecuteNonQuery();
            myConnection.Close();
           
            // rebind the grid
            DataGrid1.CurrentPageIndex = 0;
            DataGrid1.EditItemIndex = -1;
            BindGrid();
        }
    }

    void DataGrid_Page(object sender, DataGridPageChangedEventArgs e) {

        // display a new page of data
        
        if (!isEditing) {
            DataGrid1.EditItemIndex = -1;
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }
    }

    void AddNew_Click(Object sender, EventArgs e) {
    
        // add a new row to the end of the data, and set editing mode 'on'
        
        CheckIsEditing("");
        
        if (!isEditing) {

            // set the flag so we know to do an insert at Update time
            AddingNew = true;
            
            // add new row to the end of the dataset after binding

            // first get the data
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter(SelectCommand, myConnection);

            DataSet ds = new DataSet();
            myCommand.Fill(ds);

            // add a new blank row to the end of the data
            object[] rowValues = { "", "", "" };
            ds.Tables[0].Rows.Add(rowValues);
            
            // figure out the EditItemIndex, last record on last page
            int recordCount = ds.Tables[0].Rows.Count;   
            if (recordCount > 1)
                recordCount--;    
            DataGrid1.CurrentPageIndex = recordCount/DataGrid1.PageSize;
            DataGrid1.EditItemIndex = recordCount%DataGrid1.PageSize;
            
            // databind
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();    
        }
    }

    // ---------------------------------------------------------------
    //
    // Helpers Methods:
    //
    
    // property to keep track of whether we are adding a new record,
    // and save it in viewstate between postbacks
    
    protected bool AddingNew {

        get {
            object o = ViewState["AddingNew"];
            return (o == null) ? false : (bool)o;
        }
        set {
            ViewState["AddingNew"] = value;
        }
    }
    
    void BindGrid() {

        SqlConnection myConnection = new SqlConnection(ConnectionString);
        SqlDataAdapter myCommand = new SqlDataAdapter(SelectCommand, myConnection);

        DataSet ds = new DataSet();
        myCommand.Fill(ds);

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    
    
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

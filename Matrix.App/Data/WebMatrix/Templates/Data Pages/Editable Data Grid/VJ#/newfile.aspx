<%@ Page language="VJ#"%%ClassName, ClassName="{0}"%% %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">

    // TODO: update the ConnectionString and Command values for your application
    
	String ConnectionString = "server=(local);database=pubs;trusted_connection=true";
	String SelectCommand = "SELECT au_id, au_lname, au_fname from Authors";
    
	boolean isEditing = false;

	void Page_Load(Object sender, System.EventArgs e) {

		if (!get_Page().get_IsPostBack()) 
		{

			// Databind the data grid on the first request only
			// (on postback, bind only in editing, paging and sorting commands)

			BindGrid();
		}
	}

	// ---------------------------------------------------------------
	//
	// DataGrid Commands: Page, Sort, Edit, Update, Cancel, Delete
	//

	void DataGrid_ItemCommand(Object sender, DataGridCommandEventArgs e) {

		// this event fires prior to all of the other commands
		// use it to provide a more graceful transition out of edit mode
        
		CheckIsEditing(e.get_CommandName());
	}
    
	void CheckIsEditing(String commandName) {
    
		if (DataGrid1.get_EditItemIndex()!= -1) 
		{
        
			// we are currently editing a row
			if (String.Compare(commandName,"Cancel") != 0 && String.Compare(commandName,"Update") != 0) 
			{
            
				// user's edit changes (if any) will not be committed
				Message.set_Text("Your changes have not been saved yet.  Please press update to save your changes, or cancel to discard your changes, before selecting another item.");
				isEditing = true;
			}
		}
	}

	void DataGrid_Edit(Object sender, DataGridCommandEventArgs e) {

		// turn on editing for the selected row
        
		if (!isEditing) 
		{
			DataGrid1.set_EditItemIndex(e.get_Item().get_ItemIndex());
			BindGrid();
		}
	}

	void DataGrid_Update(Object sender, DataGridCommandEventArgs e) {

		// update the database with the new values
        
		// get the edit text boxes
		String id = ((TextBox)e.get_Item().get_Cells().get_Item(2).get_Controls().get_Item(0)).get_Text();
		String lname = ((TextBox)e.get_Item().get_Cells().get_Item(3).get_Controls().get_Item(0)).get_Text();
		String fname = ((TextBox)e.get_Item().get_Cells().get_Item(4).get_Controls().get_Item(0)).get_Text();
        
		// TODO: update the Command value for your application
		SqlConnection myConnection = new SqlConnection(ConnectionString);
		SqlCommand UpdateCommand = new SqlCommand();
		UpdateCommand.set_Connection(myConnection);
        
		if (get_AddingNew()) 
			UpdateCommand.set_CommandText("INSERT INTO authors(au_id, au_lname, au_fname, contract) VALUES (@au_id, @au_lname, @au_fname, 0)");
		else 
			UpdateCommand.set_CommandText("UPDATE authors SET au_lname = @au_lname, au_fname = @au_fname WHERE au_id = @au_id");

		UpdateCommand.get_Parameters().Add("@au_id", SqlDbType.VarChar, 11).set_Value(id);
		UpdateCommand.get_Parameters().Add("@au_lname", SqlDbType.VarChar, 40).set_Value(lname);
		UpdateCommand.get_Parameters().Add("@au_fname", SqlDbType.VarChar, 20).set_Value(fname);
        
		// execute the command
		try 
		{
			myConnection.Open();
			UpdateCommand.ExecuteNonQuery();
		}
		catch (Exception ex) 
		{
			Message.set_Text(ex.ToString());
		}
		finally 
		{
			myConnection.Close();
		}
        
		// Resort the grid for new records
		if (get_AddingNew()) 
		{
        
			DataGrid1.set_CurrentPageIndex(0);
			this.set_AddingNew(false);
		}
        
		// rebind the grid
		DataGrid1.set_EditItemIndex(-1);
		BindGrid();
	}

	void DataGrid_Cancel(Object sender, DataGridCommandEventArgs e) {

		// cancel editing
        
		DataGrid1.set_EditItemIndex(-1);
		BindGrid();
        
		this.set_AddingNew(false);
	}

	void DataGrid_Delete(Object sender, DataGridCommandEventArgs e) {

		// delete the selected row
        
		if (!isEditing) 
		{
        
			// the key value for this row is in the DataKeys collection 
			String keyValue = (String)DataGrid1.get_DataKeys().get_Item(e.get_Item().get_ItemIndex());
        
			// TODO: update the Command value for your application
			SqlConnection myConnection = new SqlConnection(ConnectionString);
			SqlCommand DeleteCommand = new SqlCommand("DELETE from authors where au_id='" + keyValue + "'", myConnection);
            
			// execute the command
			myConnection.Open();
			DeleteCommand.ExecuteNonQuery();
			myConnection.Close();
           
			// rebind the grid
			DataGrid1.set_CurrentPageIndex(0);
			DataGrid1.set_EditItemIndex(-1);
			BindGrid();
		}
	}

	void DataGrid_Page(Object sender, DataGridPageChangedEventArgs e) {

		// display a new page of data
        
		if (!isEditing) 
		{
			DataGrid1.set_EditItemIndex(-1);
			DataGrid1.set_CurrentPageIndex(e.get_NewPageIndex());
			BindGrid();
		}
	}

	void AddNew_Click(Object sender, System.EventArgs e) {
    
		// add a new row to the end of the data, and set editing mode 'on'
        
		CheckIsEditing("");
        
		if (!isEditing) 
		{

			// set the flag so we know to do an insert at Update time
			this.set_AddingNew(true);
            
			// add new row to the end of the dataset after binding

			// first get the data
			SqlConnection myConnection = new SqlConnection(ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter(SelectCommand, myConnection);

			DataSet ds = new DataSet();
			myCommand.Fill(ds);

			// add a new blank row to the end of the data
			Object[] rowValues = { "", "", "" };
			ds.get_Tables().get_Item(0).get_Rows().Add(rowValues);
            
			// figure out the EditItemIndex, last record on last page
			int recordCount = ds.get_Tables().get_Item(0).get_Rows().get_Count();   
			if (recordCount > 1)
				recordCount--;    
			DataGrid1.set_CurrentPageIndex(recordCount/DataGrid1.get_PageSize());
			DataGrid1.set_EditItemIndex(recordCount%DataGrid1.get_PageSize());
            
			// databind
			DataGrid1.set_DataSource(ds);
			DataGrid1.DataBind();    
		}
	}

	// ---------------------------------------------------------------
	//
	// Helpers Methods:
	//
    
	// property to keep track of whether we are adding a new record,
	// and save it in viewstate between postbacks
	/** @property
	 */
	protected boolean get_AddingNew(){

		Object o = get_ViewState().get_Item("AddingNew");
		return (o == null) ? false : (boolean)(System.Boolean)o;
	}
    
	/** @property
	 */
	protected void set_AddingNew (boolean value){

		get_ViewState().set_Item("AddingNew",(Object)(System.Boolean)value);
	}
    
	void BindGrid() {

		SqlConnection myConnection = new SqlConnection(ConnectionString);
		SqlDataAdapter myCommand = new SqlDataAdapter(SelectCommand, myConnection);

		DataSet ds = new DataSet();
		myCommand.Fill(ds);

		DataGrid1.set_DataSource(ds);
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

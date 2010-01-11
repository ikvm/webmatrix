namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Documents;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class TableDataView : Panel, IDocumentView, ICommandHandler, IPropertyBrowserClient
    {
        private DbDataAdapter _adapter;
        private IDbConnection _connection;
        private DataTable _dataTable;
        private bool _dirty;
        private TableDocument _document;
        private EditDataGrid _grid;
        private int _savedRowIndex = 0;
        private IServiceProvider _serviceProvider;
        private Bitmap _viewImage;
        private static readonly object DocumentChangedEvent = new object();
        private const string MxAutoText = "(auto)";
        private const string MxRowStateColumn = "__mxStateIsFromDB";

        event EventHandler IDocumentView.DocumentChanged
        {
            add
            {
                base.Events.AddHandler(DocumentChangedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(DocumentChangedEvent, value);
            }
        }

        public TableDataView(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.InitializeUserInterface();
        }

        private bool CommitData(bool endCurrentEdit, bool deleting)
        {
            bool flag2;
            if (this._grid.ReadOnly)
            {
                return true;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                BindingManagerBase base2 = this.BindingContext[this._dataTable];
                try
                {
                    this._grid.EndEdit(null, this._grid.CurrentCell.RowNumber, false);
                    if (endCurrentEdit)
                    {
                        base2.EndCurrentEdit();
                    }
                    this._adapter.Update(this._dataTable);
                }
                catch (Exception exception)
                {
                    if (!deleting)
                    {
                        DataTable dataSource = (DataTable) this._grid.DataSource;
                        int count = dataSource.Columns.Count;
                        int num2 = base2.Count - 1;
                        bool flag = true;
                        for (int i = 0; i < count; i++)
                        {
                            DataColumn column = dataSource.Columns[i];
                            if (!column.AutoIncrement)
                            {
                                try
                                {
                                    if ((this._grid[num2, i] == null) || (this._grid[num2, i] == DBNull.Value))
                                    {
                                        goto Label_00ED;
                                    }
                                    flag = false;
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                }
                                break;
                            Label_00ED:;
                            }
                        }
                        if (flag)
                        {
                            try
                            {
                                base2.CancelCurrentEdit();
                                this._dataTable.RejectChanges();
                            }
                            catch (Exception)
                            {
                            }
                            return true;
                        }
                    }
                    IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                    if (!deleting)
                    {
                        DialogResult none = DialogResult.None;
                        if (service != null)
                        {
                            none = service.ShowMessage("A problem was encountered while committing the most recent changes to the database. Do you wish to correct the values?\n\nDetails:\n" + exception.Message, "Could not commit changes", MessageBoxIcon.Hand, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                        }
                        if (none == DialogResult.Yes)
                        {
                            return false;
                        }
                        try
                        {
                            base2.CancelCurrentEdit();
                            this._dataTable.RejectChanges();
                        }
                        catch (Exception)
                        {
                        }
                        return true;
                    }
                    if (service != null)
                    {
                        service.ReportError(exception, "A problem was encountered while committing the most recent changes to the database.", false);
                    }
                    foreach (DataRow row in this._dataTable.Rows)
                    {
                        row.RejectChanges();
                        row.ClearErrors();
                    }
                    base.BeginInvoke(new MethodInvoker(this.ResetDataSource));
                    return true;
                }
                flag2 = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return flag2;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void InitializeUserInterface()
        {
            this._grid = new EditDataGrid();
            this._grid.BeginInit();
            base.SuspendLayout();
            Color control = SystemColors.Control;
            Color window = SystemColors.Window;
            this._grid.AllowSorting = false;
            this._grid.BackgroundColor = Color.FromArgb((window.R + (2 * control.R)) / 3, (window.G + (2 * control.G)) / 3, (window.B + (2 * control.B)) / 3);
            this._grid.BorderStyle = BorderStyle.None;
            this._grid.CaptionVisible = false;
            this._grid.DataMember = "";
            this._grid.Dock = DockStyle.Fill;
            this._grid.HeaderForeColor = SystemColors.ControlText;
            this._grid.Location = new Point(1, 1);
            this._grid.Name = "_grid";
            this._grid.SelectionBackColor = SystemColors.Highlight;
            this._grid.SelectionForeColor = SystemColors.HighlightText;
            this._grid.Size = new Size(570, 0x142);
            this._grid.TabIndex = 0;
            this._grid.CurrentCellChanged += new EventHandler(this.OnGridCurrentCellChanged);
            base.Controls.AddRange(new Control[] { this._grid });
            base.Name = "TableDataEditor";
            base.DockPadding.All = 1;
            this.BackColor = SystemColors.ControlDark;
            this._grid.EndInit();
            base.ResumeLayout(false);
        }

        private void LoadDocumentItems()
        {
            this.LoadTable();
            this._dirty = false;
        }

        private void LoadTable()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this._connection == null)
                {
                    this._connection = ((IDataProviderDatabase) this._document.Table.Database).CreateConnection();
                }
                this._document.Table.Database.Connect();
                string name = this._document.Table.Name;
                bool readOnly = false;
                string explanation = string.Empty;
                TableEditState information = TableEditState.Information;
                bool flag2 = false;
                bool flag3 = true;
                foreach (Column column in this._document.Table.Columns)
                {
                    if (!column.IsIdentity)
                    {
                        flag3 = false;
                    }
                    if (column.InPrimaryKey)
                    {
                        flag2 = true;
                    }
                }
                if (!flag2)
                {
                    readOnly = true;
                    explanation = "The table's data cannot be edited because it does not have a primary key.";
                    information = TableEditState.Warning;
                }
                if (!readOnly && ((name.IndexOf("[") != -1) || (name.IndexOf("]") != -1)))
                {
                    readOnly = true;
                    explanation = "The table's data cannot be edited because its name contains the quote prefix '[' or ']'.";
                    information = TableEditState.Warning;
                }
                if (!readOnly && flag3)
                {
                    readOnly = true;
                    explanation = "The table's data cannot be edited because the table does not contain modifiable columns.";
                    information = TableEditState.Warning;
                }
                this._adapter = ((IDataProviderDatabase) this._document.Table.Database).CreateAdapter(this._connection, this._document.Table, readOnly);
                this._adapter.FillError += new FillErrorEventHandler(this.OnAdapterFillError);
                DataSet set = new DataSet("dataSet");
                this._dataTable = new DataTable(this._document.Table.Name);
                this._dataTable.RowDeleted += new DataRowChangeEventHandler(this.OnDataTableDataRowDeleted);
                this._adapter.FillSchema(this._dataTable, SchemaType.Source);
                this._grid.DataSource = null;
                this._grid.TableStyles.Clear();
                if (!readOnly)
                {
                    DataColumn column2 = new DataColumn("__mxStateIsFromDB", typeof(bool));
                    this._dataTable.Columns.Add(column2);
                    foreach (Column column3 in this._document.Table.Columns)
                    {
                        this._dataTable.Columns[column3.Name].AllowDBNull = column3.AllowNulls;
                    }
                    DataGridTableStyle table = new DataGridTableStyle();
                    table.MappingName = this._dataTable.TableName;
                    this._grid.TableStyles.Add(table);
                }
                set.EnforceConstraints = true;
                this._grid.DataSource = this._dataTable;
                if (!readOnly)
                {
                    ArrayList list = new ArrayList(this._grid.TableStyles[0].GridColumnStyles);
                    PropertyDescriptorCollection itemProperties = (this.BindingContext[this._dataTable] as CurrencyManager).GetItemProperties();
                    foreach (Column column4 in this._document.Table.Columns)
                    {
                        if (column4.IsIdentity)
                        {
                            DataGridColumnStyle element = this._grid.TableStyles[0].GridColumnStyles[column4.Name];
                            int index = this._grid.TableStyles[0].GridColumnStyles.IndexOf(element);
                            IdentityGridColumnStyle style3 = new IdentityGridColumnStyle(itemProperties[element.MappingName]);
                            list[index] = style3;
                        }
                    }
                    DataGridColumnStyle style4 = this._grid.TableStyles[0].GridColumnStyles["__mxStateIsFromDB"];
                    list.Remove(style4);
                    this._grid.TableStyles[0].GridColumnStyles.Clear();
                    this._grid.TableStyles[0].GridColumnStyles.AddRange((DataGridColumnStyle[]) list.ToArray(typeof(DataGridColumnStyle)));
                }
                this.SetReadOnly(readOnly, explanation, information);
                this._adapter.Fill(this._dataTable);
                if (!readOnly)
                {
                    foreach (DataRow row in this._dataTable.Rows)
                    {
                        row["__mxStateIsFromDB"] = true;
                    }
                }
            }
            catch (Exception exception)
            {
                this._grid.DataSource = null;
                if (this._dataTable != null)
                {
                    this._dataTable.RowDeleted -= new DataRowChangeEventHandler(this.OnDataTableDataRowDeleted);
                    this._dataTable.Dispose();
                    this._dataTable = null;
                }
                if (this._adapter != null)
                {
                    this._adapter.Dispose();
                    this._adapter = null;
                }
                this.SetReadOnly(true, "The table could not be opened for editing - " + exception.Message, TableEditState.Error);
            }
            finally
            {
                this._document.Table.Database.Disconnect();
                this.Cursor = Cursors.Default;
            }
        }

        void IDocumentView.Activate(bool viewSwitch)
        {
            ISelectionService service = (ISelectionService) this._serviceProvider.GetService(typeof(ISelectionService));
            if (service != null)
            {
                service.SetSelectedComponents(null);
            }
            base.Focus();
        }

        void IDocumentView.Deactivate(bool closing)
        {
            this._grid.SetDeactivating();
        }

        void IDocumentView.LoadFromDocument(Document document)
        {
            this._document = (TableDocument) document;
            this.LoadDocumentItems();
            this._grid.Focus();
        }

        bool IDocumentView.SaveToDocument()
        {
            return true;
        }

        bool ICommandHandler.HandleCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 3:
                        return true;

                    case 4:
                        return true;
                }
            }
            return false;
        }

        bool ICommandHandler.UpdateCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 3:
                        command.Enabled = false;
                        return true;

                    case 4:
                        command.Enabled = false;
                        return true;
                }
            }
            return false;
        }

        private void OnAdapterFillError(object sender, FillErrorEventArgs e)
        {
            e.Continue = true;
        }

        private void OnDataTableDataRowDeleted(object sender, DataRowChangeEventArgs e)
        {
            this.CommitData(false, true);
        }

        private void OnDocumentChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[DocumentChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnGridCurrentCellChanged(object sender, EventArgs e)
        {
            int rowNumber = this._grid.CurrentCell.RowNumber;
            if (this._savedRowIndex < this._dataTable.Rows.Count)
            {
                DataRow row = this._dataTable.Rows[this._savedRowIndex];
                if ((row.RowState == DataRowState.Modified) || (row.RowState == DataRowState.Added))
                {
                    this.CommitData(false, false);
                }
            }
            this._savedRowIndex = rowNumber;
        }

        private void ResetDataSource()
        {
            object dataSource = this._grid.DataSource;
            this._grid.DataSource = null;
            this._grid.DataSource = dataSource;
        }

        private void SetReadOnly(bool value, string explanation, TableEditState editState)
        {
            if (base.Controls[base.Controls.Count - 1] is MxInfoLabel)
            {
                base.Controls.RemoveAt(base.Controls.Count - 1);
            }
            this._grid.ReadOnly = value;
            if (explanation.Length > 0)
            {
                MxInfoLabel label = new MxInfoLabel();
                label.Dock = DockStyle.Top;
                switch (editState)
                {
                    case TableEditState.Error:
                        label.Image = MxInfoLabel.ErrorGlyph;
                        break;

                    case TableEditState.Information:
                        label.Image = MxInfoLabel.InfoGlyph;
                        break;

                    case TableEditState.Warning:
                        label.Image = MxInfoLabel.WarningGlyph;
                        break;
                }
                label.Padding = 2;
                label.Text = explanation;
                base.Controls.Add(label);
            }
        }

        bool IDocumentView.CanDeactivate
        {
            get
            {
                return this.CommitData(true, false);
            }
        }

        bool IDocumentView.IsDirty
        {
            get
            {
                return this._dirty;
            }
        }

        Image IDocumentView.ViewImage
        {
            get
            {
                if (this._viewImage == null)
                {
                    this._viewImage = new Bitmap(typeof(TableDataView), "TableDataView.bmp");
                    this._viewImage.MakeTransparent();
                }
                return this._viewImage;
            }
        }

        string IDocumentView.ViewName
        {
            get
            {
                return "Data";
            }
        }

        DocumentViewType IDocumentView.ViewType
        {
            get
            {
                return DocumentViewType.Source;
            }
        }

        bool IPropertyBrowserClient.SupportsPropertyBrowser
        {
            get
            {
                return true;
            }
        }

        private sealed class EditDataGrid : DataGrid
        {
            private bool _isDeactivating;

            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                try
                {
                    base.OnKeyPress(e);
                }
                catch (Exception)
                {
                }
            }

            protected override void OnLeave(EventArgs e)
            {
                if (!this._isDeactivating)
                {
                    base.OnLeave(e);
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                try
                {
                    base.OnPaint(e);
                }
                catch (Exception)
                {
                }
            }

            public override bool PreProcessMessage(ref Message msg)
            {
                try
                {
                    return base.PreProcessMessage(ref msg);
                }
                catch (Exception)
                {
                    return true;
                }
            }

            public void SetDeactivating()
            {
                this._isDeactivating = true;
            }
        }

        private class IdentityGridColumnStyle : DataGridTextBoxColumn
        {
            public IdentityGridColumnStyle(PropertyDescriptor propDesc) : base(propDesc)
            {
                base.MappingName = propDesc.Name;
                this.HeaderText = propDesc.Name;
                this.ReadOnly = true;
                this.TextBox.BackColor = SystemColors.Window;
            }

            protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
            {
                this.TextBox.Text = this.GetText(source, rowNum);
                Rectangle rc = bounds;
                this.TextBox.ReadOnly = true;
                if (cellIsVisible)
                {
                    bounds.Offset(2, 2);
                    bounds.Width -= 2;
                    bounds.Height -= 2;
                    this.TextBox.Bounds = bounds;
                    this.TextBox.Visible = true;
                    this.TextBox.TextAlign = this.Alignment;
                }
                else
                {
                    this.TextBox.Bounds = rc;
                    this.TextBox.Visible = false;
                }
                this.TextBox.RightToLeft = this.DataGridTableStyle.DataGrid.RightToLeft;
                this.TextBox.Focus();
                if (instantText == null)
                {
                    this.TextBox.SelectAll();
                }
                else
                {
                    int length = this.TextBox.Text.Length;
                    this.TextBox.Select(length, 0);
                }
                if (this.TextBox.Visible)
                {
                    this.DataGridTableStyle.DataGrid.Invalidate(rc);
                }
            }

            private string GetText(CurrencyManager currencyManager, int rowNum)
            {
                DataRowView view = currencyManager.List[rowNum] as DataRowView;
                if (((view["__mxStateIsFromDB"] == null) || (view["__mxStateIsFromDB"] == DBNull.Value)) || !((bool) view["__mxStateIsFromDB"]))
                {
                    return "(auto)";
                }
                object columnValueAtRow = this.GetColumnValueAtRow(currencyManager, rowNum);
                if (columnValueAtRow is DBNull)
                {
                    return this.NullText;
                }
                if (((base.Format != null) && (base.Format.Length != 0)) && (columnValueAtRow is IFormattable))
                {
                    try
                    {
                        return ((IFormattable) columnValueAtRow).ToString(base.Format, base.FormatInfo);
                    }
                    catch
                    {
                        goto Label_00D9;
                    }
                }
                TypeConverter converter = TypeDescriptor.GetConverter(this.PropertyDescriptor.PropertyType);
                if ((converter != null) && converter.CanConvertTo(typeof(string)))
                {
                    return (string) converter.ConvertTo(columnValueAtRow, typeof(string));
                }
            Label_00D9:
                if (columnValueAtRow == null)
                {
                    return "";
                }
                return columnValueAtRow.ToString();
            }

            protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
            {
                base.PaintText(g, bounds, this.GetText(source, rowNum), alignToRight);
            }

            protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
            {
                base.PaintText(g, bounds, this.GetText(source, rowNum), backBrush, foreBrush, alignToRight);
            }
        }

        private enum TableEditState
        {
            Error,
            Information,
            Warning
        }
    }
}


namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework;
    using Microsoft.Matrix.Framework.Web.UI.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Globalization;
    using System.Reflection;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    [Designer(typeof(MxDataGridDesigner))]
    public class MxDataGrid : Microsoft.Matrix.Framework.Web.UI.DataBoundControl, INamingContainer
    {
        private TableItemStyle alternatingItemStyle;
        private ArrayList autoGenFieldsArray;
        public const string CancelCommandName = "Cancel";
        private DataKeyCollection dataKeysCollection;
        public const string DeleteCommandName = "Delete";
        public const string EditCommandName = "Edit";
        private TableItemStyle editItemStyle;
        private static readonly object EventAfterDelete = new object();
        private static readonly object EventAfterUpdate = new object();
        private static readonly object EventBeforeDelete = new object();
        private static readonly object EventBeforeUpdate = new object();
        private static readonly object EventCancelCommand = new object();
        private static readonly object EventEditCommand = new object();
        private static readonly object EventItemCommand = new object();
        private static readonly object EventItemCreated = new object();
        private static readonly object EventItemDataBound = new object();
        private static readonly object EventPageIndexChanged = new object();
        private static readonly object EventSelectedIndexChanged = new object();
        private static readonly object EventSortCommand = new object();
        private MxDataGridFieldCollection fieldCollection;
        private ArrayList fields;
        protected object firstDataItem;
        private TableItemStyle footerStyle;
        private bool hasBeenDataBound;
        private TableItemStyle headerStyle;
        internal const string ItemCountViewStateKey = "_!ItemCount";
        protected ArrayList itemsArray;
        private MxDataGridItemCollection itemsCollection;
        private TableItemStyle itemStyle;
        public const string NextPageCommandArgument = "Next";
        public const string PageCommandName = "Page";
        protected PagedDataSource pagedDataSource;
        private MxDataGridPagerStyle pagerStyle;
        public const string PrevPageCommandArgument = "Prev";
        public const string SelectCommandName = "Select";
        private TableItemStyle selectedItemStyle;
        public const string SortCommandName = "Sort";
        protected IEnumerator storedData;
        protected bool storedDataValid;
        public const string UpdateCommandName = "Update";

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Action"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnAfterDeleteCommand")]
        public event MxDataGridStatusEventHandler AfterDelete
        {
            add
            {
                base.Events.AddHandler(EventAfterDelete, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventAfterDelete, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Action"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnAfterUpdateCommand")]
        public event MxDataGridStatusEventHandler AfterUpdate
        {
            add
            {
                base.Events.AddHandler(EventAfterUpdate, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventAfterUpdate, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Action"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnBeforeDeleteCommand")]
        public event MxDataGridCancelEventHandler BeforeDelete
        {
            add
            {
                base.Events.AddHandler(EventBeforeDelete, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventBeforeDelete, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnBeforeUpdateCommand"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Action")]
        public event MxDataGridUpdateEventHandler BeforeUpdate
        {
            add
            {
                base.Events.AddHandler(EventBeforeUpdate, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventBeforeUpdate, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnCancelCommand"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Action")]
        public event MxDataGridCommandEventHandler CancelCommand
        {
            add
            {
                base.Events.AddHandler(EventCancelCommand, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventCancelCommand, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Action"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnEditCommand")]
        public event MxDataGridCommandEventHandler EditCommand
        {
            add
            {
                base.Events.AddHandler(EventEditCommand, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventEditCommand, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnItemCommand"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Action")]
        public event MxDataGridCommandEventHandler ItemCommand
        {
            add
            {
                base.Events.AddHandler(EventItemCommand, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventItemCommand, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnItemCreated"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior")]
        public event MxDataGridItemEventHandler ItemCreated
        {
            add
            {
                base.Events.AddHandler(EventItemCreated, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventItemCreated, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnItemDataBound"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior")]
        public event MxDataGridItemEventHandler ItemDataBound
        {
            add
            {
                base.Events.AddHandler(EventItemDataBound, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventItemDataBound, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnPageIndexChanged"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Action")]
        public event MxDataGridPageChangedEventHandler PageIndexChanged
        {
            add
            {
                base.Events.AddHandler(EventPageIndexChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPageIndexChanged, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnSelectedIndexChanged"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Action")]
        public event EventHandler SelectedIndexChanged
        {
            add
            {
                base.Events.AddHandler(EventSelectedIndexChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedIndexChanged, value);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Action"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_OnSortCommand")]
        public event MxDataGridSortCommandEventHandler SortCommand
        {
            add
            {
                base.Events.AddHandler(EventSortCommand, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSortCommand, value);
            }
        }

        private ArrayList CreateAutoGeneratedFields(PagedDataSource dataSource)
        {
            if (dataSource == null)
            {
                return null;
            }
            ArrayList list = new ArrayList();
            PropertyDescriptorCollection itemProperties = null;
            bool flag = true;
            itemProperties = dataSource.GetItemProperties(new PropertyDescriptor[0]);
            if (itemProperties == null)
            {
                Type propertyType = null;
                object firstDataItem = null;
                PropertyInfo info = dataSource.DataSource.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, null, new Type[] { typeof(int) }, null);
                if (info != null)
                {
                    propertyType = info.PropertyType;
                }
                if ((propertyType == null) || (propertyType == typeof(object)))
                {
                    IEnumerator enumerator = dataSource.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        firstDataItem = enumerator.Current;
                    }
                    else
                    {
                        flag = false;
                    }
                    if (firstDataItem != null)
                    {
                        propertyType = firstDataItem.GetType();
                    }
                    this.StoreEnumerator(enumerator, firstDataItem);
                }
                if ((firstDataItem != null) && (firstDataItem is ICustomTypeDescriptor))
                {
                    itemProperties = TypeDescriptor.GetProperties(firstDataItem);
                }
                else if (propertyType != null)
                {
                    if (BaseDataList.IsBindableType(propertyType))
                    {
                        Microsoft.Matrix.Framework.Web.UI.BoundField field = new Microsoft.Matrix.Framework.Web.UI.BoundField();
                        ((IStateManager) field).TrackViewState();
                        field.HeaderText = "Item";
                        field.DataField = Microsoft.Matrix.Framework.Web.UI.BoundField.thisExpr;
                        field.SortExpression = "Item";
                        field.SetOwner(this);
                        list.Add(field);
                    }
                    else
                    {
                        itemProperties = TypeDescriptor.GetProperties(propertyType);
                    }
                }
            }
            if ((itemProperties != null) && (itemProperties.Count != 0))
            {
                foreach (PropertyDescriptor descriptor in itemProperties)
                {
                    if (BaseDataList.IsBindableType(descriptor.PropertyType) && ((this.AutoGenerateExcludeFields.Length == 0) || (Array.IndexOf(this.AutoGenerateExcludeFields, descriptor.Name) == -1)))
                    {
                        Microsoft.Matrix.Framework.Web.UI.BoundField field2 = new Microsoft.Matrix.Framework.Web.UI.BoundField();
                        ((IStateManager) field2).TrackViewState();
                        field2.HeaderText = descriptor.Name;
                        field2.DataField = descriptor.Name;
                        field2.SortExpression = descriptor.Name;
                        field2.ReadOnly = descriptor.IsReadOnly;
                        field2.SetOwner(this);
                        list.Add(field2);
                    }
                }
            }
            if ((list.Count == 0) && flag)
            {
                throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("MxDataGrid_NoAutoGenColumns"), this.ID));
            }
            return list;
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            this.pagedDataSource = this.CreatePagedDataSource();
            IEnumerator storedData = null;
            int num = 0;
            ArrayList dataKeysArray = this.DataKeysArray;
            ArrayList list2 = null;
            if (this.itemsArray != null)
            {
                this.itemsArray.Clear();
            }
            else
            {
                this.itemsArray = new ArrayList();
            }
            if (!dataBinding)
            {
                this.pagedDataSource.DataSource = dataSource;
                storedData = this.pagedDataSource.GetEnumerator();
                list2 = this.CreateFieldSet(null, false);
                ICollection is2 = dataSource as ICollection;
                if (is2 == null)
                {
                    throw new HttpException(Microsoft.Matrix.Framework.SR.GetString("MxDataGrid_DataSourceMustBeCollection"));
                }
                this.itemsArray.Capacity = is2.Count;
            }
            else
            {
                dataKeysArray.Clear();
                if (dataSource != null)
                {
                    ICollection is3 = dataSource as ICollection;
                    if (((is3 == null) && this.pagedDataSource.IsPagingEnabled) && !this.pagedDataSource.IsCustomPagingEnabled)
                    {
                        throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("MxDataGrid_Missing_VirtualItemCount"), this.ID));
                    }
                    this.pagedDataSource.DataSource = dataSource;
                    if (this.pagedDataSource.IsPagingEnabled && ((this.pagedDataSource.CurrentPageIndex < 0) || (this.pagedDataSource.CurrentPageIndex >= this.pagedDataSource.PageCount)))
                    {
                        throw new HttpException(Microsoft.Matrix.Framework.SR.GetString("Invalid_CurrentPageIndex"));
                    }
                    list2 = this.CreateFieldSet(this.pagedDataSource, dataBinding);
                    if (this.storedDataValid)
                    {
                        storedData = this.storedData;
                    }
                    else
                    {
                        storedData = this.pagedDataSource.GetEnumerator();
                    }
                    if (is3 != null)
                    {
                        int count = this.pagedDataSource.Count;
                        dataKeysArray.Capacity = count;
                        this.itemsArray.Capacity = count;
                    }
                }
            }
            int num3 = 0;
            if (list2 != null)
            {
                num3 = list2.Count;
            }
            if (num3 > 0)
            {
                MxDataGridItem item;
                ListItemType editItem;
                MxDataGridField[] array = new MxDataGridField[num3];
                list2.CopyTo(array, 0);
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].Initialize();
                }
                Table child = new MxDataGridTable();
                this.Controls.Add(child);
                TableRowCollection rows = child.Rows;
                int itemIndex = 0;
                int dataSourceIndex = 0;
                string dataKeyField = this.DataKeyField;
                bool flag = dataBinding && (dataKeyField.Length != 0);
                bool isPagingEnabled = this.pagedDataSource.IsPagingEnabled;
                int editItemIndex = this.EditItemIndex;
                int selectedIndex = this.SelectedIndex;
                if (this.pagedDataSource.IsPagingEnabled)
                {
                    dataSourceIndex = this.pagedDataSource.FirstIndexInPage;
                }
                if (isPagingEnabled)
                {
                    this.CreateItem(-1, -1, ListItemType.Pager, false, null, array, rows, this.pagedDataSource);
                }
                this.CreateItem(-1, -1, ListItemType.Header, dataBinding, null, array, rows, null);
                if (this.storedDataValid && (this.firstDataItem != null))
                {
                    if (flag)
                    {
                        object propertyValue = DataBinder.GetPropertyValue(this.firstDataItem, dataKeyField);
                        dataKeysArray.Add(propertyValue);
                    }
                    editItem = ListItemType.Item;
                    if (itemIndex == editItemIndex)
                    {
                        editItem = ListItemType.EditItem;
                    }
                    else if (itemIndex == selectedIndex)
                    {
                        editItem = ListItemType.SelectedItem;
                    }
                    item = this.CreateItem(0, dataSourceIndex, editItem, dataBinding, this.firstDataItem, array, rows, null);
                    this.itemsArray.Add(item);
                    num++;
                    itemIndex++;
                    dataSourceIndex++;
                    this.storedDataValid = false;
                    this.firstDataItem = null;
                }
                while (storedData.MoveNext())
                {
                    object current = storedData.Current;
                    if (flag)
                    {
                        object obj4 = DataBinder.GetPropertyValue(current, dataKeyField);
                        dataKeysArray.Add(obj4);
                    }
                    editItem = ListItemType.Item;
                    if (itemIndex == editItemIndex)
                    {
                        editItem = ListItemType.EditItem;
                    }
                    else if (itemIndex == selectedIndex)
                    {
                        editItem = ListItemType.SelectedItem;
                    }
                    else if ((itemIndex % 2) != 0)
                    {
                        editItem = ListItemType.AlternatingItem;
                    }
                    item = this.CreateItem(itemIndex, dataSourceIndex, editItem, dataBinding, current, array, rows, null);
                    this.itemsArray.Add(item);
                    num++;
                    dataSourceIndex++;
                    itemIndex++;
                }
                this.CreateItem(-1, -1, ListItemType.Footer, dataBinding, null, array, rows, null);
                if (isPagingEnabled)
                {
                    this.CreateItem(-1, -1, ListItemType.Pager, false, null, array, rows, this.pagedDataSource);
                }
            }
            int dataSourceCount = -1;
            if (dataBinding)
            {
                if (storedData != null)
                {
                    if (this.pagedDataSource.IsPagingEnabled)
                    {
                        this.ViewState["PageCount"] = this.pagedDataSource.PageCount;
                        if (this.pagedDataSource.IsCustomPagingEnabled)
                        {
                            dataSourceCount = num;
                        }
                        else
                        {
                            dataSourceCount = this.pagedDataSource.DataSourceCount;
                        }
                    }
                    else
                    {
                        this.ViewState["PageCount"] = 1;
                        dataSourceCount = this.pagedDataSource.DataSourceCount;
                    }
                }
                else
                {
                    this.ViewState["PageCount"] = 0;
                }
            }
            this.pagedDataSource = null;
            return dataSourceCount;
        }

        protected override Style CreateControlStyle()
        {
            TableStyle style = new TableStyle(this.ViewState);
            style.GridLines = System.Web.UI.WebControls.GridLines.Both;
            style.CellSpacing = 0;
            return style;
        }

        protected virtual ArrayList CreateFieldSet(PagedDataSource dataSource, bool useDataSource)
        {
            int num;
            ArrayList list = new ArrayList();
            MxDataGridField[] array = new MxDataGridField[this.Fields.Count];
            this.Fields.CopyTo(array, 0);
            for (num = 0; num < array.Length; num++)
            {
                list.Add(array[num]);
            }
            if (this.AutoGenerateFields)
            {
                ArrayList autoGenFieldsArray = null;
                if (useDataSource)
                {
                    autoGenFieldsArray = this.CreateAutoGeneratedFields(dataSource);
                    this.autoGenFieldsArray = autoGenFieldsArray;
                }
                else
                {
                    autoGenFieldsArray = this.autoGenFieldsArray;
                }
                if (autoGenFieldsArray == null)
                {
                    return list;
                }
                int count = autoGenFieldsArray.Count;
                for (num = 0; num < count; num++)
                {
                    list.Add(autoGenFieldsArray[num]);
                }
            }
            return list;
        }

        protected virtual MxDataGridItem CreateItem(int itemIndex, int dataSourceIndex, ListItemType itemType)
        {
            return new MxDataGridItem(itemIndex, dataSourceIndex, itemType);
        }

        protected MxDataGridItem CreateItem(int itemIndex, int dataSourceIndex, ListItemType itemType, bool dataBind, object dataItem, MxDataGridField[] fields, TableRowCollection rows, PagedDataSource pagedDataSource)
        {
            MxDataGridItem item = this.CreateItem(itemIndex, dataSourceIndex, itemType);
            MxDataGridItemEventArgs e = new MxDataGridItemEventArgs(item);
            if (itemType != ListItemType.Pager)
            {
                this.InitializeItem(item, fields);
                if (dataBind)
                {
                    item.DataItem = dataItem;
                }
                this.OnItemCreated(e);
                rows.Add(item);
                if (dataBind)
                {
                    item.DataBind();
                    this.OnItemDataBound(e);
                    item.DataItem = null;
                }
                return item;
            }
            this.InitializePager(item, fields.Length, pagedDataSource);
            this.OnItemCreated(e);
            rows.Add(item);
            return item;
        }

        protected PagedDataSource CreatePagedDataSource()
        {
            PagedDataSource source = new PagedDataSource();
            source.CurrentPageIndex = this.CurrentPageIndex;
            source.PageSize = this.PageSize;
            source.AllowPaging = this.AllowPaging;
            source.AllowCustomPaging = this.AllowCustomPaging;
            source.VirtualCount = this.VirtualItemCount;
            return source;
        }

        public override void DataBind()
        {
            base.DataBind();
            this.hasBeenDataBound = true;
        }

        protected virtual void InitializeItem(MxDataGridItem item, MxDataGridField[] fields)
        {
            TableCellCollection cells = item.Cells;
            for (int i = 0; i < fields.Length; i++)
            {
                TableCell cell = new TableCell();
                fields[i].InitializeCell(cell, i, item.ItemType);
                cells.Add(cell);
            }
        }

        protected virtual void InitializePager(MxDataGridItem item, int fieldSpan, PagedDataSource pagedDataSource)
        {
            TableCell cell = new TableCell();
            cell.ColumnSpan = fieldSpan;
            MxDataGridPagerStyle pagerStyle = this.PagerStyle;
            if (pagerStyle.Mode == PagerMode.NextPrev)
            {
                if (!pagedDataSource.IsFirstPage)
                {
                    LinkButton child = new MxDataGridLinkButton();
                    child.Text = pagerStyle.PrevPageText;
                    child.CommandName = "Page";
                    child.CommandArgument = "Prev";
                    child.CausesValidation = false;
                    cell.Controls.Add(child);
                }
                else
                {
                    Label label = new Label();
                    label.Text = pagerStyle.PrevPageText;
                    cell.Controls.Add(label);
                }
                cell.Controls.Add(new LiteralControl("&nbsp;"));
                if (!pagedDataSource.IsLastPage)
                {
                    LinkButton button2 = new MxDataGridLinkButton();
                    button2.Text = pagerStyle.NextPageText;
                    button2.CommandName = "Page";
                    button2.CommandArgument = "Next";
                    button2.CausesValidation = false;
                    cell.Controls.Add(button2);
                }
                else
                {
                    Label label2 = new Label();
                    label2.Text = pagerStyle.NextPageText;
                    cell.Controls.Add(label2);
                }
            }
            else
            {
                LinkButton button3;
                int pageCount = pagedDataSource.PageCount;
                int num2 = pagedDataSource.CurrentPageIndex + 1;
                int pageButtonCount = pagerStyle.PageButtonCount;
                int num4 = pageButtonCount;
                if (pageCount < num4)
                {
                    num4 = pageCount;
                }
                int num5 = 1;
                int num6 = num4;
                if (num2 > num6)
                {
                    int num7 = pagedDataSource.CurrentPageIndex / pageButtonCount;
                    num5 = (num7 * pageButtonCount) + 1;
                    num6 = (num5 + pageButtonCount) - 1;
                    if (num6 > pageCount)
                    {
                        num6 = pageCount;
                    }
                    if (((num6 - num5) + 1) < pageButtonCount)
                    {
                        num5 = Math.Max(1, (num6 - pageButtonCount) + 1);
                    }
                }
                if (num5 != 1)
                {
                    button3 = new MxDataGridLinkButton();
                    button3.Text = "...";
                    button3.CommandName = "Page";
                    int num9 = num5 - 1;
                    button3.CommandArgument = num9.ToString(NumberFormatInfo.InvariantInfo);
                    button3.CausesValidation = false;
                    cell.Controls.Add(button3);
                    cell.Controls.Add(new LiteralControl("&nbsp;"));
                }
                for (int i = num5; i <= num6; i++)
                {
                    string str = i.ToString(NumberFormatInfo.InvariantInfo);
                    if (i == num2)
                    {
                        Label label3 = new Label();
                        label3.Text = str;
                        cell.Controls.Add(label3);
                    }
                    else
                    {
                        button3 = new MxDataGridLinkButton();
                        button3.Text = str;
                        button3.CommandName = "Page";
                        button3.CommandArgument = str;
                        button3.CausesValidation = false;
                        cell.Controls.Add(button3);
                    }
                    if (i < num6)
                    {
                        cell.Controls.Add(new LiteralControl("&nbsp;"));
                    }
                }
                if (pageCount > num6)
                {
                    cell.Controls.Add(new LiteralControl("&nbsp;"));
                    button3 = new MxDataGridLinkButton();
                    button3.Text = "...";
                    button3.CommandName = "Page";
                    button3.CommandArgument = (num6 + 1).ToString(NumberFormatInfo.InvariantInfo);
                    button3.CausesValidation = false;
                    cell.Controls.Add(button3);
                }
            }
            item.Cells.Add(cell);
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[]) savedState;
                if (objArray[0] != null)
                {
                    base.LoadViewState(objArray[0]);
                }
                if (objArray[1] != null)
                {
                    ((IStateManager) this.Fields).LoadViewState(objArray[1]);
                }
                if (objArray[2] != null)
                {
                    ((IStateManager) this.PagerStyle).LoadViewState(objArray[2]);
                }
                if (objArray[3] != null)
                {
                    ((IStateManager) this.HeaderStyle).LoadViewState(objArray[3]);
                }
                if (objArray[4] != null)
                {
                    ((IStateManager) this.FooterStyle).LoadViewState(objArray[4]);
                }
                if (objArray[5] != null)
                {
                    ((IStateManager) this.ItemStyle).LoadViewState(objArray[5]);
                }
                if (objArray[6] != null)
                {
                    ((IStateManager) this.AlternatingItemStyle).LoadViewState(objArray[6]);
                }
                if (objArray[7] != null)
                {
                    ((IStateManager) this.SelectedItemStyle).LoadViewState(objArray[7]);
                }
                if (objArray[8] != null)
                {
                    ((IStateManager) this.EditItemStyle).LoadViewState(objArray[8]);
                }
                if (objArray[9] != null)
                {
                    object[] objArray2 = (object[]) objArray[9];
                    int length = objArray2.Length;
                    if (length != 0)
                    {
                        this.autoGenFieldsArray = new ArrayList();
                    }
                    else
                    {
                        this.autoGenFieldsArray = null;
                    }
                    for (int i = 0; i < length; i++)
                    {
                        Microsoft.Matrix.Framework.Web.UI.BoundField field = new Microsoft.Matrix.Framework.Web.UI.BoundField();
                        ((IStateManager) field).TrackViewState();
                        ((IStateManager) field).LoadViewState(objArray2[i]);
                        field.SetOwner(this);
                        this.autoGenFieldsArray.Add(field);
                    }
                }
            }
        }

        protected virtual void OnAfterDelete(MxDataGridStatusEventArgs e)
        {
            MxDataGridStatusEventHandler handler = (MxDataGridStatusEventHandler) base.Events[EventAfterDelete];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnAfterUpdate(MxDataGridStatusEventArgs e)
        {
            MxDataGridStatusEventHandler handler = (MxDataGridStatusEventHandler) base.Events[EventAfterUpdate];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnBeforeDelete(MxDataGridCancelEventArgs e)
        {
            MxDataGridCancelEventHandler handler = (MxDataGridCancelEventHandler) base.Events[EventBeforeDelete];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnBeforeUpdate(MxDataGridUpdateEventArgs e)
        {
            MxDataGridUpdateEventHandler handler = (MxDataGridUpdateEventHandler) base.Events[EventBeforeUpdate];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            bool flag = false;
            if (e is MxDataGridCommandEventArgs)
            {
                MxDataGridCommandEventArgs args = (MxDataGridCommandEventArgs) e;
                this.OnItemCommand(args);
                flag = true;
                string commandName = args.CommandName;
                if (string.Compare(commandName, "Select", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.SelectedIndex = args.Item.ItemIndex;
                    this.OnSelectedIndexChanged(EventArgs.Empty);
                    return flag;
                }
                if (string.Compare(commandName, "Page", true, CultureInfo.InvariantCulture) == 0)
                {
                    string commandArgument = (string) args.CommandArgument;
                    int currentPageIndex = this.CurrentPageIndex;
                    if (string.Compare(commandArgument, "Next", true, CultureInfo.InvariantCulture) == 0)
                    {
                        currentPageIndex++;
                    }
                    else if (string.Compare(commandArgument, "Prev", true, CultureInfo.InvariantCulture) == 0)
                    {
                        currentPageIndex--;
                    }
                    else
                    {
                        currentPageIndex = int.Parse(commandArgument) - 1;
                    }
                    MxDataGridPageChangedEventArgs args2 = new MxDataGridPageChangedEventArgs(source, currentPageIndex);
                    this.OnPageIndexChanged(args2);
                    return flag;
                }
                if (string.Compare(commandName, "Sort", true, CultureInfo.InvariantCulture) == 0)
                {
                    MxDataGridSortCommandEventArgs args3 = new MxDataGridSortCommandEventArgs(source, args);
                    this.OnSortCommand(args3);
                    return flag;
                }
                if (string.Compare(commandName, "Edit", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.OnEditCommand(args);
                    return flag;
                }
                if (string.Compare(commandName, "Update", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.OnUpdateCommand(new MxDataGridUpdateEventArgs(args, false));
                    return flag;
                }
                if (string.Compare(commandName, "Cancel", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.OnCancelCommand(args);
                    return flag;
                }
                if (string.Compare(commandName, "Delete", true, CultureInfo.InvariantCulture) == 0)
                {
                    this.OnDeleteCommand(new MxDataGridCancelEventArgs(args, false));
                }
            }
            return flag;
        }

        protected virtual void OnCancelCommand(MxDataGridCommandEventArgs e)
        {
            MxDataGridCommandEventHandler handler = (MxDataGridCommandEventHandler) base.Events[EventCancelCommand];
            if (handler != null)
            {
                handler(this, e);
            }
            this.EditItemIndex = -1;
            if (this.AutoDataBind)
            {
                this.DataBind();
            }
        }

        protected virtual void OnDeleteCommand(MxDataGridCancelEventArgs e)
        {
            if (!(this.DataSource is DataControl) || ((DataControl) this.DataSource).CanDelete)
            {
                this.OnBeforeDelete(e);
                if (!e.Cancel)
                {
                    int affectedRecords = 0;
                    this.EditItemIndex = -1;
                    if (this.DataSource is DataControl)
                    {
                        DataControl dataSource = (DataControl) this.DataSource;
                        if (dataSource.AutoGenerateDeleteCommand)
                        {
                            if (this.DataKeys.Count == 0)
                            {
                                throw new HttpException(Microsoft.Matrix.Framework.SR.GetString("MxDataGrid_CantGenerateDeleteCommand"));
                            }
                            object obj2 = this.DataKeys[e.Item.ItemIndex];
                            Hashtable selectionFilters = new Hashtable();
                            selectionFilters.Add(this.DataKeyField, obj2);
                            affectedRecords = dataSource.Delete(this.DataMember, selectionFilters);
                        }
                        else
                        {
                            affectedRecords = dataSource.Delete(this.DataMember, e.FieldValues);
                        }
                    }
                    if (this.AutoDataBind)
                    {
                        this.DataBind();
                    }
                    this.OnAfterDelete(new MxDataGridStatusEventArgs(e, affectedRecords));
                }
            }
        }

        protected virtual void OnEditCommand(MxDataGridCommandEventArgs e)
        {
            if (!(this.DataSource is DataControl) || ((DataControl) this.DataSource).CanUpdate)
            {
                MxDataGridCommandEventHandler handler = (MxDataGridCommandEventHandler) base.Events[EventEditCommand];
                if (handler != null)
                {
                    handler(this, e);
                }
                this.EditItemIndex = e.Item.ItemIndex;
                if (this.AutoDataBind)
                {
                    this.DataBind();
                }
            }
        }

        internal void OnFieldsChanged()
        {
        }

        protected virtual void OnItemCommand(MxDataGridCommandEventArgs e)
        {
            MxDataGridCommandEventHandler handler = (MxDataGridCommandEventHandler) base.Events[EventItemCommand];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemCreated(MxDataGridItemEventArgs e)
        {
            MxDataGridItemEventHandler handler = (MxDataGridItemEventHandler) base.Events[EventItemCreated];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemDataBound(MxDataGridItemEventArgs e)
        {
            MxDataGridItemEventHandler handler = (MxDataGridItemEventHandler) base.Events[EventItemDataBound];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPageIndexChanged(MxDataGridPageChangedEventArgs e)
        {
            MxDataGridPageChangedEventHandler handler = (MxDataGridPageChangedEventHandler) base.Events[EventPageIndexChanged];
            if (handler != null)
            {
                handler(this, e);
            }
            this.EditItemIndex = -1;
            this.CurrentPageIndex = e.NewPageIndex;
            if (this.AutoDataBind)
            {
                this.DataBind();
            }
        }

        internal void OnPagerChanged()
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            if ((this.AutoDataBind && !this.Page.IsPostBack) && !this.hasBeenDataBound)
            {
                this.DataBind();
            }
            base.OnPreRender(e);
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventSelectedIndexChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSortCommand(MxDataGridSortCommandEventArgs e)
        {
            object dataSource = this.DataSource;
            this.EditItemIndex = -1;
            if (dataSource is DataControl)
            {
                if (!((DataControl) dataSource).CanSort)
                {
                    return;
                }
                ((DataControl) dataSource).SetSortExpression(this.DataMember, e.SortExpression);
                this.CurrentPageIndex = 0;
            }
            MxDataGridSortCommandEventHandler handler = (MxDataGridSortCommandEventHandler) base.Events[EventSortCommand];
            if (handler != null)
            {
                handler(this, e);
            }
            if (this.AutoDataBind)
            {
                this.DataBind();
            }
        }

        protected virtual void OnUpdateCommand(MxDataGridUpdateEventArgs e)
        {
            if (!(this.DataSource is DataControl) || ((DataControl) this.DataSource).CanUpdate)
            {
                this.OnBeforeUpdate(e);
                if (!e.Cancel)
                {
                    int affectedRecords = 0;
                    DataControl dataSource = (DataControl) this.DataSource;
                    if (dataSource != null)
                    {
                        if (dataSource.AutoGenerateUpdateCommand)
                        {
                            if (((this.DataKeys.Count == 0) || !this.AutoGenerateFields) || (this.AutoGenerateExcludeFields.Length > 0))
                            {
                                throw new HttpException(Microsoft.Matrix.Framework.SR.GetString("MxDataGrid_CantGenerateUpdateCommand"));
                            }
                            object obj2 = this.DataKeys[e.Item.ItemIndex];
                            Hashtable selectionFilters = new Hashtable();
                            Hashtable newValues = new Hashtable();
                            for (int i = this.fieldCollection.Count; i < e.Item.Cells.Count; i++)
                            {
                                if (e.Item.Cells[i].Controls.Count != 0)
                                {
                                    TextBox box = (TextBox) e.Item.Cells[i].Controls[0];
                                    if (box != null)
                                    {
                                        newValues.Add(((Microsoft.Matrix.Framework.Web.UI.BoundField) this.autoGenFieldsArray[i - this.fieldCollection.Count]).DataField, box.Text);
                                    }
                                }
                            }
                            selectionFilters.Add(this.DataKeyField, obj2);
                            affectedRecords = dataSource.Update(this.DataMember, selectionFilters, newValues);
                        }
                        else
                        {
                            affectedRecords = dataSource.Update(this.DataMember, e.SelectionFilter, e.NewValues);
                        }
                        this.EditItemIndex = -1;
                    }
                    if (this.AutoDataBind)
                    {
                        this.DataBind();
                    }
                    this.OnAfterUpdate(new MxDataGridStatusEventArgs(e, affectedRecords));
                }
            }
        }

        protected virtual void PrepareControlHierarchy()
        {
            if (this.Controls.Count != 0)
            {
                Table table = (Table) this.Controls[0];
                table.CopyBaseAttributes(this);
                if (base.ControlStyleCreated)
                {
                    table.ApplyStyle(base.ControlStyle);
                }
                else
                {
                    table.GridLines = System.Web.UI.WebControls.GridLines.Both;
                    table.CellSpacing = 0;
                }
                TableRowCollection rows = table.Rows;
                int count = rows.Count;
                if (count != 0)
                {
                    int num2 = this.Fields.Count;
                    MxDataGridField[] array = new MxDataGridField[num2];
                    if (num2 > 0)
                    {
                        this.Fields.CopyTo(array, 0);
                    }
                    Style s = null;
                    if (this.alternatingItemStyle != null)
                    {
                        s = new TableItemStyle();
                        s.CopyFrom(this.itemStyle);
                        s.CopyFrom(this.alternatingItemStyle);
                    }
                    else
                    {
                        s = this.itemStyle;
                    }
                    for (int i = 0; i < count; i++)
                    {
                        Style style2;
                        Style style3;
                        TableCellCollection cells;
                        MxDataGridItem item = (MxDataGridItem) rows[i];
                        switch (item.ItemType)
                        {
                            case ListItemType.Header:
                                if (this.ShowHeader)
                                {
                                    break;
                                }
                                item.Visible = false;
                                goto Label_0336;

                            case ListItemType.Footer:
                                if (this.ShowFooter)
                                {
                                    goto Label_014D;
                                }
                                item.Visible = false;
                                goto Label_0336;

                            case ListItemType.Item:
                                item.MergeStyle(this.itemStyle);
                                goto Label_0281;

                            case ListItemType.AlternatingItem:
                                item.MergeStyle(s);
                                goto Label_0281;

                            case ListItemType.SelectedItem:
                                style2 = new TableItemStyle();
                                if ((item.ItemIndex % 2) == 0)
                                {
                                    goto Label_0200;
                                }
                                style2.CopyFrom(s);
                                goto Label_020D;

                            case ListItemType.EditItem:
                                style3 = new TableItemStyle();
                                if ((item.ItemIndex % 2) == 0)
                                {
                                    goto Label_0242;
                                }
                                style3.CopyFrom(s);
                                goto Label_024F;

                            case ListItemType.Pager:
                                if (this.pagerStyle.Visible)
                                {
                                    goto Label_0179;
                                }
                                item.Visible = false;
                                goto Label_0336;

                            default:
                                goto Label_0281;
                        }
                        if (this.headerStyle != null)
                        {
                            item.MergeStyle(this.headerStyle);
                        }
                        goto Label_0281;
                    Label_014D:
                        item.MergeStyle(this.footerStyle);
                        goto Label_0281;
                    Label_0179:
                        if (i == 0)
                        {
                            if (this.pagerStyle.IsPagerOnTop)
                            {
                                goto Label_01B1;
                            }
                            item.Visible = false;
                            goto Label_0336;
                        }
                        if (!this.pagerStyle.IsPagerOnBottom)
                        {
                            item.Visible = false;
                            goto Label_0336;
                        }
                    Label_01B1:
                        item.MergeStyle(this.pagerStyle);
                        goto Label_0281;
                    Label_0200:
                        style2.CopyFrom(this.itemStyle);
                    Label_020D:
                        style2.CopyFrom(this.selectedItemStyle);
                        item.MergeStyle(style2);
                        goto Label_0281;
                    Label_0242:
                        style3.CopyFrom(this.itemStyle);
                    Label_024F:
                        if (item.ItemIndex == this.SelectedIndex)
                        {
                            style3.CopyFrom(this.selectedItemStyle);
                        }
                        style3.CopyFrom(this.editItemStyle);
                        item.MergeStyle(style3);
                    Label_0281:
                        cells = item.Cells;
                        int num4 = cells.Count;
                        if ((num2 > 0) && (item.ItemType != ListItemType.Pager))
                        {
                            int num5 = num4;
                            if (num2 < num4)
                            {
                                num5 = num2;
                            }
                            for (int j = 0; j < num5; j++)
                            {
                                if (!array[j].Visible)
                                {
                                    cells[j].Visible = false;
                                }
                                else
                                {
                                    Style headerStyleInternal = null;
                                    switch (item.ItemType)
                                    {
                                        case ListItemType.Header:
                                            headerStyleInternal = array[j].HeaderStyleInternal;
                                            break;

                                        case ListItemType.Footer:
                                            headerStyleInternal = array[j].FooterStyleInternal;
                                            break;

                                        default:
                                            headerStyleInternal = array[j].ItemStyleInternal;
                                            break;
                                    }
                                    cells[j].MergeStyle(headerStyleInternal);
                                }
                            }
                        }
                    Label_0336:;
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.PrepareControlHierarchy();
            this.RenderContents(writer);
        }

        protected override object SaveViewState()
        {
            object obj2 = base.SaveViewState();
            object obj3 = (this.fieldCollection != null) ? ((IStateManager) this.fieldCollection).SaveViewState() : null;
            object obj4 = (this.pagerStyle != null) ? ((IStateManager) this.pagerStyle).SaveViewState() : null;
            object obj5 = (this.headerStyle != null) ? ((IStateManager) this.headerStyle).SaveViewState() : null;
            object obj6 = (this.footerStyle != null) ? ((IStateManager) this.footerStyle).SaveViewState() : null;
            object obj7 = (this.itemStyle != null) ? ((IStateManager) this.itemStyle).SaveViewState() : null;
            object obj8 = (this.alternatingItemStyle != null) ? ((IStateManager) this.alternatingItemStyle).SaveViewState() : null;
            object obj9 = (this.selectedItemStyle != null) ? ((IStateManager) this.selectedItemStyle).SaveViewState() : null;
            object obj10 = (this.editItemStyle != null) ? ((IStateManager) this.editItemStyle).SaveViewState() : null;
            object[] objArray = null;
            if ((this.autoGenFieldsArray != null) && (this.autoGenFieldsArray.Count != 0))
            {
                objArray = new object[this.autoGenFieldsArray.Count];
                for (int i = 0; i < objArray.Length; i++)
                {
                    objArray[i] = ((IStateManager) this.autoGenFieldsArray[i]).SaveViewState();
                }
            }
            return new object[] { obj2, obj3, obj4, obj5, obj6, obj7, obj8, obj9, obj10, objArray };
        }

        internal void StoreEnumerator(IEnumerator dataSource, object firstDataItem)
        {
            this.storedData = dataSource;
            this.firstDataItem = firstDataItem;
            this.storedDataValid = true;
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (this.fieldCollection != null)
            {
                ((IStateManager) this.fieldCollection).TrackViewState();
            }
            if (this.pagerStyle != null)
            {
                ((IStateManager) this.pagerStyle).TrackViewState();
            }
            if (this.headerStyle != null)
            {
                ((IStateManager) this.headerStyle).TrackViewState();
            }
            if (this.footerStyle != null)
            {
                ((IStateManager) this.footerStyle).TrackViewState();
            }
            if (this.itemStyle != null)
            {
                ((IStateManager) this.itemStyle).TrackViewState();
            }
            if (this.alternatingItemStyle != null)
            {
                ((IStateManager) this.alternatingItemStyle).TrackViewState();
            }
            if (this.selectedItemStyle != null)
            {
                ((IStateManager) this.selectedItemStyle).TrackViewState();
            }
            if (this.editItemStyle != null)
            {
                ((IStateManager) this.editItemStyle).TrackViewState();
            }
        }

        [DefaultValue(false), Microsoft.Matrix.Framework.Web.UI.WebCategory("Paging"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_AllowCustomPaging")]
        public virtual bool AllowCustomPaging
        {
            get
            {
                object obj2 = this.ViewState["AllowCustomPaging"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                this.ViewState["AllowCustomPaging"] = value;
            }
        }

        [DefaultValue(false), Microsoft.Matrix.Framework.Web.UI.WebCategory("Paging"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_AllowPaging")]
        public virtual bool AllowPaging
        {
            get
            {
                object obj2 = this.ViewState["AllowPaging"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                this.ViewState["AllowPaging"] = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_AllowSorting"), DefaultValue(false), Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior")]
        public virtual bool AllowSorting
        {
            get
            {
                object obj2 = this.ViewState["AllowSorting"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                this.ViewState["AllowSorting"] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_AlternatingItemStyle"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Style")]
        public virtual TableItemStyle AlternatingItemStyle
        {
            get
            {
                if (this.alternatingItemStyle == null)
                {
                    this.alternatingItemStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.alternatingItemStyle).TrackViewState();
                    }
                }
                return this.alternatingItemStyle;
            }
        }

        [Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design", typeof(UITypeEditor)), TypeConverter(typeof(Microsoft.Matrix.Framework.Web.UI.StringArrayConverter)), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_AutoGenerateExcludeFields"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior")]
        public string[] AutoGenerateExcludeFields
        {
            get
            {
                object obj2 = this.ViewState["autoGenerateExcludeFields"];
                if (obj2 != null)
                {
                    return (string[]) obj2;
                }
                return new string[0];
            }
            set
            {
                this.ViewState["autoGenerateExcludeFields"] = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior"), DefaultValue(true), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_AutoGenerateFields")]
        public virtual bool AutoGenerateFields
        {
            get
            {
                object obj2 = this.ViewState["AutoGenerateFields"];
                if (obj2 != null)
                {
                    return (bool) obj2;
                }
                return true;
            }
            set
            {
                this.ViewState["AutoGenerateFields"] = value;
            }
        }

        [Bindable(true), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_BackImageUrl"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Appearance"), DefaultValue(""), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public virtual string BackImageUrl
        {
            get
            {
                if (!base.ControlStyleCreated)
                {
                    return string.Empty;
                }
                return ((TableStyle) base.ControlStyle).BackImageUrl;
            }
            set
            {
                ((TableStyle) base.ControlStyle).BackImageUrl = value;
            }
        }

        [Bindable(true), DefaultValue(-1), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_CellPadding"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Layout")]
        public virtual int CellPadding
        {
            get
            {
                if (!base.ControlStyleCreated)
                {
                    return -1;
                }
                return ((TableStyle) base.ControlStyle).CellPadding;
            }
            set
            {
                ((TableStyle) base.ControlStyle).CellPadding = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_CellSpacing"), Bindable(true), Microsoft.Matrix.Framework.Web.UI.WebCategory("Layout"), DefaultValue(0)]
        public virtual int CellSpacing
        {
            get
            {
                if (!base.ControlStyleCreated)
                {
                    return 0;
                }
                return ((TableStyle) base.ControlStyle).CellSpacing;
            }
            set
            {
                ((TableStyle) base.ControlStyle).CellSpacing = value;
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        [Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_CurrentPageIndex")]
        public int CurrentPageIndex
        {
            get
            {
                object obj2 = this.ViewState["CurrentPageIndex"];
                if (obj2 != null)
                {
                    return (int) obj2;
                }
                return 0;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.ViewState["CurrentPageIndex"] = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_DataKeyField"), DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data")]
        public virtual string DataKeyField
        {
            get
            {
                object obj2 = this.ViewState["DataKeyField"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataKeyField"] = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_DataKeys"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataKeyCollection DataKeys
        {
            get
            {
                if (this.dataKeysCollection == null)
                {
                    this.dataKeysCollection = new DataKeyCollection(this.DataKeysArray);
                }
                return this.dataKeysCollection;
            }
        }

        protected ArrayList DataKeysArray
        {
            get
            {
                object obj2 = this.ViewState["DataKeys"];
                if (obj2 == null)
                {
                    obj2 = new ArrayList();
                    this.ViewState["DataKeys"] = obj2;
                }
                return (ArrayList) obj2;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_EditItemIndex"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Default"), DefaultValue(-1)]
        public virtual int EditItemIndex
        {
            get
            {
                object obj2 = this.ViewState["EditItemIndex"];
                if (obj2 != null)
                {
                    return (int) obj2;
                }
                return -1;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.ViewState["EditItemIndex"] = value;
            }
        }

        [NotifyParentProperty(true), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_EditItemStyle"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Style"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual TableItemStyle EditItemStyle
        {
            get
            {
                if (this.editItemStyle == null)
                {
                    this.editItemStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.editItemStyle).TrackViewState();
                    }
                }
                return this.editItemStyle;
            }
        }

        [DefaultValue((string) null), PersistenceMode(PersistenceMode.InnerProperty), Microsoft.Matrix.Framework.Web.UI.WebCategory("Default"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_Fields"), Editor(typeof(MxDataGridFieldCollectionEditor), typeof(UITypeEditor)), MergableProperty(false)]
        public virtual MxDataGridFieldCollection Fields
        {
            get
            {
                if (this.fieldCollection == null)
                {
                    this.fields = new ArrayList();
                    this.fieldCollection = new MxDataGridFieldCollection(this, this.fields);
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.fieldCollection).TrackViewState();
                    }
                }
                return this.fieldCollection;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_FooterStyle"), PersistenceMode(PersistenceMode.InnerProperty), Microsoft.Matrix.Framework.Web.UI.WebCategory("Style"), DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        public virtual TableItemStyle FooterStyle
        {
            get
            {
                if (this.footerStyle == null)
                {
                    this.footerStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.footerStyle).TrackViewState();
                    }
                }
                return this.footerStyle;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Appearance"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_GridLines"), DefaultValue(3), Bindable(true)]
        public virtual System.Web.UI.WebControls.GridLines GridLines
        {
            get
            {
                if (!base.ControlStyleCreated)
                {
                    return System.Web.UI.WebControls.GridLines.Both;
                }
                return ((TableStyle) base.ControlStyle).GridLines;
            }
            set
            {
                ((TableStyle) base.ControlStyle).GridLines = value;
            }
        }

        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_HeaderStyle"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Style"), DefaultValue((string) null)]
        public virtual TableItemStyle HeaderStyle
        {
            get
            {
                if (this.headerStyle == null)
                {
                    this.headerStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.headerStyle).TrackViewState();
                    }
                }
                return this.headerStyle;
            }
        }

        [DefaultValue(0), Category("Layout"), Bindable(true), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_HorizontalAlign")]
        public virtual System.Web.UI.WebControls.HorizontalAlign HorizontalAlign
        {
            get
            {
                if (!base.ControlStyleCreated)
                {
                    return System.Web.UI.WebControls.HorizontalAlign.NotSet;
                }
                return ((TableStyle) base.ControlStyle).HorizontalAlign;
            }
            set
            {
                ((TableStyle) base.ControlStyle).HorizontalAlign = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_Items"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual MxDataGridItemCollection Items
        {
            get
            {
                if (this.itemsCollection == null)
                {
                    if (this.itemsArray == null)
                    {
                        this.EnsureChildControls();
                    }
                    if (this.itemsArray == null)
                    {
                        this.itemsArray = new ArrayList();
                    }
                    this.itemsCollection = new MxDataGridItemCollection(this.itemsArray);
                }
                return this.itemsCollection;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Microsoft.Matrix.Framework.Web.UI.WebCategory("Style"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_ItemStyle"), NotifyParentProperty(true)]
        public virtual TableItemStyle ItemStyle
        {
            get
            {
                if (this.itemStyle == null)
                {
                    this.itemStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.itemStyle).TrackViewState();
                    }
                }
                return this.itemStyle;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_PageCount"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageCount
        {
            get
            {
                if (this.pagedDataSource != null)
                {
                    return this.pagedDataSource.PageCount;
                }
                object obj2 = this.ViewState["PageCount"];
                if (obj2 == null)
                {
                    return 0;
                }
                return (int) obj2;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_PagerStyle"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Style"), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual MxDataGridPagerStyle PagerStyle
        {
            get
            {
                if (this.pagerStyle == null)
                {
                    this.pagerStyle = new MxDataGridPagerStyle(this);
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.pagerStyle).TrackViewState();
                    }
                }
                return this.pagerStyle;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_PageSize"), DefaultValue(10), Microsoft.Matrix.Framework.Web.UI.WebCategory("Paging")]
        public virtual int PageSize
        {
            get
            {
                object obj2 = this.ViewState["PageSize"];
                if (obj2 != null)
                {
                    return (int) obj2;
                }
                return 10;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.ViewState["PageSize"] = value;
            }
        }

        [Bindable(true), DefaultValue(-1), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_SelectedIndex")]
        public virtual int SelectedIndex
        {
            get
            {
                object obj2 = this.ViewState["SelectedIndex"];
                if (obj2 != null)
                {
                    return (int) obj2;
                }
                return -1;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                int selectedIndex = this.SelectedIndex;
                this.ViewState["SelectedIndex"] = value;
                if (this.itemsArray != null)
                {
                    MxDataGridItem item;
                    if ((selectedIndex != -1) && (this.itemsArray.Count > selectedIndex))
                    {
                        item = (MxDataGridItem) this.itemsArray[selectedIndex];
                        if (item.ItemType != ListItemType.EditItem)
                        {
                            ListItemType itemType = ListItemType.Item;
                            if ((selectedIndex % 2) != 0)
                            {
                                itemType = ListItemType.AlternatingItem;
                            }
                            item.SetItemType(itemType);
                        }
                    }
                    if ((value != -1) && (this.itemsArray.Count > value))
                    {
                        item = (MxDataGridItem) this.itemsArray[value];
                        if (item.ItemType != ListItemType.EditItem)
                        {
                            item.SetItemType(ListItemType.SelectedItem);
                        }
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_SelectedItem")]
        public virtual MxDataGridItem SelectedItem
        {
            get
            {
                int selectedIndex = this.SelectedIndex;
                MxDataGridItem item = null;
                if (selectedIndex != -1)
                {
                    item = this.Items[selectedIndex];
                }
                return item;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Style"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_SelectedItemStyle")]
        public virtual TableItemStyle SelectedItemStyle
        {
            get
            {
                if (this.selectedItemStyle == null)
                {
                    this.selectedItemStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager) this.selectedItemStyle).TrackViewState();
                    }
                }
                return this.selectedItemStyle;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_ShowFooter"), Bindable(true), Microsoft.Matrix.Framework.Web.UI.WebCategory("Appearance"), DefaultValue(false)]
        public virtual bool ShowFooter
        {
            get
            {
                object obj2 = this.ViewState["ShowFooter"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                this.ViewState["ShowFooter"] = value;
            }
        }

        [DefaultValue(true), Bindable(true), Microsoft.Matrix.Framework.Web.UI.WebCategory("Appearance"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_ShowHeader")]
        public virtual bool ShowHeader
        {
            get
            {
                object obj2 = this.ViewState["ShowHeader"];
                if (obj2 != null)
                {
                    return (bool) obj2;
                }
                return true;
            }
            set
            {
                this.ViewState["ShowHeader"] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("MxDataGrid_VisibleItemCount")]
        public virtual int VirtualItemCount
        {
            get
            {
                object obj2 = this.ViewState["VirtualItemCount"];
                if (obj2 != null)
                {
                    return (int) obj2;
                }
                return 0;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.ViewState["VirtualItemCount"] = value;
            }
        }
    }
}


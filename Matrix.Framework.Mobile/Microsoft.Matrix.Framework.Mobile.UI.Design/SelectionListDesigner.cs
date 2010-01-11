namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Editors;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Drawing.Design;
    using System.Runtime.InteropServices;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.MobileControls;

    public class SelectionListDesigner : MobileControlDesigner, IDataSourceProvider, IListDesigner
    {
        private const string _dataMemberPropertyName = "DataMember";
        private const string _dataSourcePropertyName = "DataSource";
        private const string _dataTextFieldPropertyName = "DataTextField";
        private const string _dataValueFieldPropertyName = "DataValueField";
        private DesignerVerbCollection _designerVerbs;
        private DataTable _designTimeDataTable;
        private DataTable _dummyDataTable;
        private static readonly Attribute[] _emptyAttrs = new Attribute[0];
        private const string _itemsPropertyName = "Items";
        private SelectionList _selectionList;

        protected IEnumerable GetDesignTimeDataSource(int minimumRows, out bool dummyDataSource)
        {
            IEnumerable resolvedSelectedDataSource = this.GetResolvedSelectedDataSource();
            return this.GetDesignTimeDataSource(resolvedSelectedDataSource, minimumRows, out dummyDataSource);
        }

        protected IEnumerable GetDesignTimeDataSource(IEnumerable selectedDataSource, int minimumRows, out bool dummyDataSource)
        {
            DataTable dataTable = this._designTimeDataTable;
            dummyDataSource = false;
            if (dataTable == null)
            {
                if (selectedDataSource != null)
                {
                    this._designTimeDataTable = DesignTimeData.CreateSampleDataTable(selectedDataSource);
                    dataTable = this._designTimeDataTable;
                }
                if (dataTable == null)
                {
                    if (this._dummyDataTable == null)
                    {
                        this._dummyDataTable = DesignTimeData.CreateDummyDataTable();
                    }
                    dataTable = this._dummyDataTable;
                    dummyDataSource = true;
                }
            }
            return DesignTimeData.GetDesignTimeDataSource(dataTable, minimumRows);
        }

        protected override string GetDesignTimeNormalHtml()
        {
            IEnumerable selectedDataSource = null;
            string str = null;
            string str2 = null;
            bool dummyDataSource = false;
            HtmlTextWriter designerTextWriter = Utils.GetDesignerTextWriter(true);
            MobileListItemCollection items = this._selectionList.get_Items();
            if (items.get_Count() > 0)
            {
                this.Adapter.Render(designerTextWriter);
            }
            else
            {
                MobileListItem[] all = items.GetAll();
                int minimumRows = 5;
                selectedDataSource = this.GetResolvedSelectedDataSource();
                IEnumerable enumerable2 = this.GetDesignTimeDataSource(selectedDataSource, minimumRows, out dummyDataSource);
                if (dummyDataSource)
                {
                    str = this._selectionList.get_DataTextField();
                    str2 = this._selectionList.get_DataValueField();
                    this._selectionList.set_DataTextField("Column0");
                    this._selectionList.set_DataValueField("Column1");
                }
                try
                {
                    this._selectionList.set_DataSource(enumerable2);
                    this._selectionList.DataBind();
                    this.Adapter.Render(designerTextWriter);
                }
                finally
                {
                    this._selectionList.set_DataSource(null);
                    this._selectionList.get_Items().SetAll(all);
                    if (dummyDataSource)
                    {
                        this._selectionList.set_DataTextField(str);
                        this._selectionList.set_DataValueField(str2);
                    }
                }
            }
            return designerTextWriter.ToString();
        }

        public IEnumerable GetResolvedSelectedDataSource()
        {
            IEnumerable enumerable = null;
            DataBinding binding = base.DataBindings["DataSource"];
            if (binding != null)
            {
                enumerable = DesignTimeData.GetSelectedDataSource(this._selectionList, binding.Expression, this.DataMember);
            }
            return enumerable;
        }

        public object GetSelectedDataSource()
        {
            object selectedDataSource = null;
            DataBinding binding = base.DataBindings["DataSource"];
            if (binding != null)
            {
                selectedDataSource = DesignTimeData.GetSelectedDataSource(this._selectionList, binding.Expression);
            }
            return selectedDataSource;
        }

        public override void Initialize(IComponent component)
        {
            this._selectionList = (SelectionList) component;
            base.Initialize(component);
        }

        protected internal void InvokePropertyBuilder(int initialPage)
        {
            IComponentChangeService service = null;
            service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
            if (service != null)
            {
                try
                {
                    service.OnComponentChanging(this._selectionList, null);
                }
                catch (CheckoutException exception)
                {
                    if (exception != CheckoutException.Canceled)
                    {
                        throw;
                    }
                    return;
                }
            }
            try
            {
                new SelectionListComponentEditor(initialPage).EditComponent(this._selectionList);
            }
            finally
            {
                if (service != null)
                {
                    service.OnComponentChanged(this._selectionList, null, null, null);
                }
            }
        }

        public override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            if ((e.Member != null) && (e.Member.Name.Equals("DataSource") || e.Member.Name.Equals("DataMember")))
            {
                this.OnDataSourceChanged();
            }
            base.OnComponentChanged(sender, e);
        }

        public void OnDataSourceChanged()
        {
            this._designTimeDataTable = null;
        }

        protected void OnPropertyBuilder(object sender, EventArgs e)
        {
            this.InvokePropertyBuilder(0);
        }

        protected override void PreFilterAttributes(IDictionary attributes)
        {
            base.PreFilterAttributes(attributes);
            EditorAttribute attribute = new EditorAttribute(typeof(SelectionListComponentEditor), typeof(ComponentEditor));
            attributes[attribute.TypeId] = attribute;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            Type designerType = base.GetType();
            if (this.ActiveDeviceFilter == null)
            {
                Utils.AddAttributesToPropertiesOfDifferentType(designerType, typeof(string), properties, "DataSource", new TypeConverterAttribute(typeof(DataSourceConverter)));
                Utils.AddAttributesToProperty(this._selectionList.GetType(), properties, "Items", new Attribute[] { new EditorAttribute(typeof(ItemCollectionEditor), typeof(UITypeEditor)) });
            }
            Utils.AddAttributesToProperty(designerType, properties, "DataMember", _emptyAttrs);
            Utils.AddAttributesToProperty(designerType, properties, "DataTextField", _emptyAttrs);
            Utils.AddAttributesToProperty(designerType, properties, "DataValueField", _emptyAttrs);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerSelectionListAdapter(this._selectionList);
            }
        }

        public string DataMember
        {
            get
            {
                return this._selectionList.get_DataMember();
            }
            set
            {
                this._selectionList.set_DataMember(value);
                this.OnDataSourceChanged();
            }
        }

        public string DataSource
        {
            get
            {
                DataBinding binding = base.DataBindings["DataSource"];
                if (binding != null)
                {
                    return binding.Expression;
                }
                return string.Empty;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    base.DataBindings.Remove("DataSource");
                }
                else
                {
                    DataBinding binding = base.DataBindings["DataSource"];
                    if (binding == null)
                    {
                        binding = new DataBinding("DataSource", typeof(IEnumerable), value);
                    }
                    else
                    {
                        binding.Expression = value;
                    }
                    base.DataBindings.Add(binding);
                }
                this.OnDataSourceChanged();
                this.OnBindingsCollectionChanged("DataSource");
            }
        }

        public string DataTextField
        {
            get
            {
                return this._selectionList.get_DataTextField();
            }
            set
            {
                this._selectionList.set_DataTextField(value);
            }
        }

        public string DataValueField
        {
            get
            {
                return this._selectionList.get_DataValueField();
            }
            set
            {
                this._selectionList.set_DataValueField(value);
            }
        }

        public MobileListItemCollection Items
        {
            get
            {
                return this._selectionList.get_Items();
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (this._designerVerbs == null)
                {
                    this._designerVerbs = new DesignerVerbCollection();
                    this._designerVerbs.Add(new DesignerVerb(Constants.PropertyBuilderVerb, new EventHandler(this.OnPropertyBuilder)));
                }
                this._designerVerbs[0].Enabled = this.ActiveDeviceFilter == null;
                return this._designerVerbs;
            }
        }
    }
}


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

    public class ListDesigner : MobileTemplatedControlDesigner, IDataSourceProvider, IListDesigner
    {
        private const string _dataMemberPropertyName = "DataMember";
        private const string _dataSourcePropertyName = "DataSource";
        private const string _dataTextFieldPropertyName = "DataTextField";
        private const string _dataValueFieldPropertyName = "DataValueField";
        private DesignerVerbCollection _designerVerbs;
        private DataTable _designTimeDataTable;
        private DataTable _dummyDataTable;
        private static readonly Attribute[] _emptyAttrs = new Attribute[0];
        private const int _headerFooterTemplates = 0;
        private const string _itemsPropertyName = "Items";
        private const int _itemTemplates = 1;
        private List _list;
        private int _numberItems;
        private const int _numberOfTemplateFrames = 3;
        private const int _separatorTemplate = 2;
        private static readonly string[][] _templateFrameNames = new string[][] { new string[] { Constants.HeaderTemplateTag, Constants.FooterTemplateTag }, new string[] { Constants.ItemTemplateTag, Constants.AlternatingItemTemplateTag }, new string[] { Constants.SeparatorTemplateTag } };

        private IEnumerable GetDesignTimeDataSource(int minimumRows, out bool dummyDataSource)
        {
            IEnumerable resolvedSelectedDataSource = this.GetResolvedSelectedDataSource();
            return this.GetDesignTimeDataSource(resolvedSelectedDataSource, minimumRows, out dummyDataSource);
        }

        private IEnumerable GetDesignTimeDataSource(IEnumerable selectedDataSource, int minimumRows, out bool dummyDataSource)
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
            MobileListItemCollection items = this._list.get_Items();
            MobileListItem[] all = items.GetAll();
            if (this._list.get_IsTemplated() || (items.get_Count() == 0))
            {
                int minimumRows = items.get_Count();
                if (minimumRows == 0)
                {
                    minimumRows = 5;
                }
                selectedDataSource = this.GetResolvedSelectedDataSource();
                if (minimumRows != this._numberItems)
                {
                    this.OnDummyDataTableChanged();
                    this._numberItems = minimumRows;
                }
                IEnumerable enumerable2 = this.GetDesignTimeDataSource(selectedDataSource, minimumRows, out dummyDataSource);
                if (dummyDataSource)
                {
                    str = this._list.get_DataTextField();
                    str2 = this._list.get_DataValueField();
                    this._list.set_DataTextField("Column0");
                    this._list.set_DataValueField("Column1");
                }
                try
                {
                    this._list.set_DataSource(enumerable2);
                    this._list.DataBind();
                    this.Adapter.Render(designerTextWriter);
                }
                finally
                {
                    this._list.set_DataSource(null);
                    this._list.get_Items().SetAll(all);
                    this._list.Controls.Clear();
                    if (dummyDataSource)
                    {
                        this._list.set_DataTextField(str);
                        this._list.set_DataValueField(str2);
                    }
                }
            }
            else
            {
                this.Adapter.Render(designerTextWriter);
            }
            return designerTextWriter.ToString();
        }

        public IEnumerable GetResolvedSelectedDataSource()
        {
            IEnumerable enumerable = null;
            DataBinding binding = base.DataBindings["DataSource"];
            if (binding != null)
            {
                enumerable = DesignTimeData.GetSelectedDataSource(this._list, binding.Expression, this.DataMember);
            }
            return enumerable;
        }

        public object GetSelectedDataSource()
        {
            object selectedDataSource = null;
            DataBinding binding = base.DataBindings["DataSource"];
            if (binding != null)
            {
                selectedDataSource = DesignTimeData.GetSelectedDataSource(this._list, binding.Expression);
            }
            return selectedDataSource;
        }

        public override string GetTemplateContainerDataItemProperty(string templateName)
        {
            return "DataItem";
        }

        public override IEnumerable GetTemplateContainerDataSource(string templateName)
        {
            return this.GetResolvedSelectedDataSource();
        }

        protected override string[] GetTemplateFrameNames(int index)
        {
            return _templateFrameNames[index];
        }

        public override Type GetTemplatePropertyParentType(string templateName)
        {
            return typeof(MobileTemplatedControlDesigner.TemplateContainer);
        }

        protected override TemplateEditingVerb[] GetTemplateVerbs()
        {
            return new TemplateEditingVerb[] { new TemplateEditingVerb(Constants.TemplateFrameHeaderFooterTemplates, 0, this), new TemplateEditingVerb(Constants.TemplateFrameItemTemplates, 1, this), new TemplateEditingVerb(Constants.TemplateFrameSeparatorTemplate, 2, this) };
        }

        public override void Initialize(IComponent component)
        {
            this._list = (List) component;
            base.Initialize(component);
            this._numberItems = this._list.get_Items().get_Count();
        }

        protected internal void InvokePropertyBuilder(int initialPage)
        {
            IComponentChangeService service = null;
            service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
            if (service != null)
            {
                try
                {
                    service.OnComponentChanging(this._list, null);
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
                new ListComponentEditor(initialPage).EditComponent(this._list);
            }
            finally
            {
                if (service != null)
                {
                    service.OnComponentChanged(this._list, null, null, null);
                }
            }
        }

        public override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            if (e.Member != null)
            {
                string name = e.Member.Name;
                if (name.Equals("DataSource") || name.Equals("DataMember"))
                {
                    this.OnDataSourceChanged();
                }
            }
            base.OnComponentChanged(sender, e);
        }

        public void OnDataSourceChanged()
        {
            this._designTimeDataTable = null;
        }

        private void OnDummyDataTableChanged()
        {
            this._dummyDataTable = null;
        }

        private void OnPropertyBuilder(object sender, EventArgs e)
        {
            this.InvokePropertyBuilder(0);
        }

        protected override void PreFilterAttributes(IDictionary attributes)
        {
            base.PreFilterAttributes(attributes);
            EditorAttribute attribute = new EditorAttribute(typeof(ListComponentEditor), typeof(ComponentEditor));
            attributes[attribute.TypeId] = attribute;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            Type designerType = base.GetType();
            if (this.ActiveDeviceFilter == null)
            {
                Utils.AddAttributesToPropertiesOfDifferentType(designerType, typeof(string), properties, "DataSource", new TypeConverterAttribute(typeof(DataSourceConverter)));
                Utils.AddAttributesToProperty(this._list.GetType(), properties, "Items", new Attribute[] { new EditorAttribute(typeof(ItemCollectionEditor), typeof(UITypeEditor)) });
            }
            Utils.AddAttributesToProperty(designerType, properties, "DataMember", _emptyAttrs);
            Utils.AddAttributesToProperty(designerType, properties, "DataTextField", _emptyAttrs);
            Utils.AddAttributesToProperty(designerType, properties, "DataValueField", _emptyAttrs);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerListAdapter(this._list);
            }
        }

        public string DataMember
        {
            get
            {
                return this._list.get_DataMember();
            }
            set
            {
                this._list.set_DataMember(value);
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
                return this._list.get_DataTextField();
            }
            set
            {
                this._list.set_DataTextField(value);
            }
        }

        public string DataValueField
        {
            get
            {
                return this._list.get_DataValueField();
            }
            set
            {
                this._list.set_DataValueField(value);
            }
        }

        public MobileListItemCollection Items
        {
            get
            {
                return this._list.get_Items();
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


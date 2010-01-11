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
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;

    public class ObjectListDesigner : MobileTemplatedControlDesigner, IMobileDesigner, IDataSourceProvider
    {
        private const string _commandsMemberPropertyName = "Commands";
        private const string _dataMemberPropertyName = "DataMember";
        private const string _dataSourcePropertyName = "DataSource";
        private DesignerVerbCollection _designerVerbs;
        private DataTable _designTimeDataTable;
        private DataTable _dummyDataTable;
        private static readonly Attribute[] _emptyAttrs = new Attribute[0];
        private const string _fieldsMemberPropertyName = "Fields";
        private const int _headerFooterTemplates = 0;
        private const int _itemTemplates = 1;
        private const int _numberOfTemplateFrames = 3;
        private ObjectList _objectList;
        private const int _separatorTemplate = 2;
        private const string _tableFieldsMemberPropertyName = "TableFields";
        private static readonly string[][] _templateFrameNames = new string[][] { new string[] { Constants.HeaderTemplateTag, Constants.FooterTemplateTag }, new string[] { Constants.ItemTemplateTag, Constants.AlternatingItemTemplateTag, Constants.ItemDetailsTemplateTag }, new string[] { Constants.SeparatorTemplateTag } };

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
            int minimumRows = 5;
            bool dummyDataSource = false;
            string str = this._objectList.get_LabelField();
            string str2 = this._objectList.get_TableFields();
            HtmlTextWriter designerTextWriter = Utils.GetDesignerTextWriter(true);
            IEnumerable designTimeDataSource = this.GetDesignTimeDataSource(minimumRows, out dummyDataSource);
            bool flag2 = this._objectList.get_AutoGenerateFields();
            if (!flag2 && (this._objectList.get_Fields().get_Count() == 0))
            {
                this._objectList.set_AutoGenerateFields(true);
            }
            if (dummyDataSource)
            {
                this._objectList.set_LabelField(string.Empty);
                this._objectList.set_TableFields(string.Empty);
            }
            try
            {
                this._objectList.set_DataSource(designTimeDataSource);
                this._objectList.DataBind();
                this.Adapter.Render(designerTextWriter);
            }
            finally
            {
                this._objectList.set_DataSource(null);
                this._objectList.set_AutoGenerateFields(flag2);
                if (dummyDataSource)
                {
                    this._objectList.set_LabelField(str);
                    this._objectList.set_TableFields(str2);
                }
                this._objectList.Controls.Clear();
                Utils.InvalidateDisplayFieldIndices(this._objectList);
            }
            return designerTextWriter.ToString();
        }

        public IEnumerable GetResolvedSelectedDataSource()
        {
            IEnumerable enumerable = null;
            DataBinding binding = base.DataBindings["DataSource"];
            if (binding != null)
            {
                enumerable = DesignTimeData.GetSelectedDataSource(this._objectList, binding.Expression, this.DataMember);
            }
            return enumerable;
        }

        public object GetSelectedDataSource()
        {
            object selectedDataSource = null;
            DataBinding binding = base.DataBindings["DataSource"];
            if (binding != null)
            {
                selectedDataSource = DesignTimeData.GetSelectedDataSource(this._objectList, binding.Expression);
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
            this._objectList = (ObjectList) component;
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
                    service.OnComponentChanging(this._objectList, null);
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
                new ObjectListComponentEditor(initialPage).EditComponent(this._objectList);
            }
            finally
            {
                if (service != null)
                {
                    service.OnComponentChanged(this._objectList, null, null, null);
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

        protected internal virtual void OnDataSourceChanged()
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
            EditorAttribute attribute = new EditorAttribute(typeof(ObjectListComponentEditor), typeof(ComponentEditor));
            attributes[attribute.TypeId] = attribute;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            Type designerType = base.GetType();
            if (this.ActiveDeviceFilter == null)
            {
                Utils.AddAttributesToProperty(this._objectList.GetType(), properties, "Fields", new Attribute[] { new EditorAttribute(typeof(FieldCollectionEditor), typeof(UITypeEditor)) });
                Utils.AddAttributesToProperty(this._objectList.GetType(), properties, "Commands", new Attribute[] { new EditorAttribute(typeof(CommandCollectionEditor), typeof(UITypeEditor)) });
                Utils.AddAttributesToProperty(this._objectList.GetType(), properties, "TableFields", new Attribute[] { new EditorAttribute(typeof(TableFieldsEditor), typeof(UITypeEditor)) });
                Utils.AddAttributesToPropertiesOfDifferentType(designerType, typeof(string), properties, "DataSource", new TypeConverterAttribute(typeof(DataSourceConverter)));
            }
            else
            {
                Utils.AddAttributesToProperty(this._objectList.GetType(), properties, "TableFields", new Attribute[] { new BrowsableAttribute(false) });
            }
            Utils.AddAttributesToProperty(designerType, properties, "DataMember", _emptyAttrs);
        }

        public void UpdateRendering()
        {
            Utils.RefreshStyle(this._objectList.get_LabelStyle());
            Utils.RefreshStyle(this._objectList.get_CommandStyle());
            base.UpdateRendering();
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerObjectListAdapter(this._objectList);
            }
        }

        public string DataMember
        {
            get
            {
                return this._objectList.get_DataMember();
            }
            set
            {
                this._objectList.set_DataMember(value);
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


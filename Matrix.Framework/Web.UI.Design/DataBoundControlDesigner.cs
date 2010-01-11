namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using Microsoft.Matrix.Framework.Web.UI;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Web.UI;
    using System.Web.UI.Design;

    public abstract class DataBoundControlDesigner : ControlDesigner
    {
        protected DataBoundControlDesigner()
        {
        }

        public override string GetDesignTimeHtml()
        {
            Microsoft.Matrix.Framework.Web.UI.Design.DataControlDesigner dataControlDesigner = this.DataControlDesigner;
            IEnumerable designTimeDataSource = null;
            string designTimeHtml = null;
            string dataSourceControlID = string.Empty;
            bool flag = false;
            try
            {
                if (dataControlDesigner == null)
                {
                    designTimeDataSource = DesignTimeData.GetDesignTimeDataSource(DesignTimeData.CreateDummyDataTable(), 5);
                }
                else
                {
                    designTimeDataSource = dataControlDesigner.GetDataSource(this.DataMember);
                    if (designTimeDataSource == null)
                    {
                        return this.GetEmptyDesignTimeHtml();
                    }
                }
                this.DataBoundControl.DataSource = designTimeDataSource;
                dataSourceControlID = this.DataBoundControl.DataSourceControlID;
                this.DataBoundControl.DataSourceControlID = string.Empty;
                flag = true;
                this.DataBoundControl.DataBind();
                designTimeHtml = base.GetDesignTimeHtml();
            }
            catch (Exception exception)
            {
                designTimeHtml = this.GetErrorDesignTimeHtml(exception);
            }
            finally
            {
                this.DataBoundControl.DataSource = null;
                if (flag)
                {
                    this.DataBoundControl.DataSourceControlID = dataSourceControlID;
                }
            }
            return designTimeHtml;
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml(null);
        }

        protected virtual void OnDataSourceChanged()
        {
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor) properties["DataSource"];
            System.ComponentModel.AttributeCollection attributes = oldPropertyDescriptor.Attributes;
            Attribute[] array = new Attribute[attributes.Count + 1];
            attributes.CopyTo(array, 0);
            array[attributes.Count] = new TypeConverterAttribute(typeof(DataSourceConverter));
            oldPropertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), "DataSource", typeof(string), array);
            properties["DataSource"] = oldPropertyDescriptor;
            oldPropertyDescriptor = (PropertyDescriptor) properties["DataMember"];
            oldPropertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), oldPropertyDescriptor, new Attribute[] { new TypeConverterAttribute(typeof(DataMemberConverter)) });
            properties["DataMember"] = oldPropertyDescriptor;
            oldPropertyDescriptor = (PropertyDescriptor) properties["DataSourceControlID"];
            oldPropertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), oldPropertyDescriptor, new Attribute[] { new TypeConverterAttribute(typeof(DataSourceControlIDConverter)) });
            properties["DataSourceControlID"] = oldPropertyDescriptor;
        }

        private Microsoft.Matrix.Framework.Web.UI.DataBoundControl DataBoundControl
        {
            get
            {
                return (Microsoft.Matrix.Framework.Web.UI.DataBoundControl) base.Component;
            }
        }

        protected Microsoft.Matrix.Framework.Web.UI.Design.DataControlDesigner DataControlDesigner
        {
            get
            {
                try
                {
                    string dataSourceControlID = this.DataBoundControl.DataSourceControlID;
                    if (dataSourceControlID != string.Empty)
                    {
                        IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                        IComponent component = service.Container.Components[dataSourceControlID];
                        return (Microsoft.Matrix.Framework.Web.UI.Design.DataControlDesigner) service.GetDesigner(component);
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string DataMember
        {
            get
            {
                return this.DataBoundControl.DataMember;
            }
            set
            {
                this.DataBoundControl.DataMember = value;
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

        public string DataSourceControlID
        {
            get
            {
                return this.DataBoundControl.DataSourceControlID;
            }
            set
            {
                this.DataBoundControl.DataSourceControlID = value;
                this.OnDataSourceChanged();
            }
        }

        public override bool DesignTimeHtmlRequiresLoadComplete
        {
            get
            {
                return true;
            }
        }
    }
}


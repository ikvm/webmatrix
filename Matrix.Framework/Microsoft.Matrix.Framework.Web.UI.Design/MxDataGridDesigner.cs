namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using Microsoft.Matrix.Framework.Web.UI;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Web.UI.Design;

    public class MxDataGridDesigner : DataBoundControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            string designTimeHtml;
            Microsoft.Matrix.Framework.Web.UI.MxDataGrid mxDataGrid = this.MxDataGrid;
            string dataKeyField = string.Empty;
            bool autoGenerateFields = false;
            string[] autoGenerateExcludeFields = null;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            try
            {
                dataKeyField = mxDataGrid.DataKeyField;
                mxDataGrid.DataKeyField = string.Empty;
                flag2 = true;
                if (mxDataGrid.Fields.Count == 0)
                {
                    autoGenerateFields = mxDataGrid.AutoGenerateFields;
                    mxDataGrid.AutoGenerateFields = true;
                    flag3 = true;
                    autoGenerateExcludeFields = mxDataGrid.AutoGenerateExcludeFields;
                    mxDataGrid.AutoGenerateExcludeFields = null;
                    flag4 = true;
                }
                designTimeHtml = base.GetDesignTimeHtml();
            }
            finally
            {
                if (flag2)
                {
                    mxDataGrid.DataKeyField = dataKeyField;
                }
                if (flag3)
                {
                    mxDataGrid.AutoGenerateFields = autoGenerateFields;
                }
                if (flag4)
                {
                    mxDataGrid.AutoGenerateExcludeFields = autoGenerateExcludeFields;
                }
            }
            return designTimeHtml;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor) properties["DataKeyField"];
            oldPropertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), oldPropertyDescriptor, new Attribute[] { new TypeConverterAttribute(typeof(DataFieldConverter)) });
            properties["DataKeyField"] = oldPropertyDescriptor;
        }

        public string DataKeyField
        {
            get
            {
                return this.MxDataGrid.DataKeyField;
            }
            set
            {
                this.MxDataGrid.DataKeyField = value;
                this.OnDataSourceChanged();
            }
        }

        private Microsoft.Matrix.Framework.Web.UI.MxDataGrid MxDataGrid
        {
            get
            {
                return (Microsoft.Matrix.Framework.Web.UI.MxDataGrid) base.Component;
            }
        }
    }
}


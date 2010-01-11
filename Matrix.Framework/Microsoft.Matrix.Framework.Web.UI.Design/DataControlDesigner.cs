namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.UI.Design;

    public abstract class DataControlDesigner : ControlDesigner
    {
        public event EventHandler DataSourceChanged;

        protected DataControlDesigner()
        {
        }

        public abstract object GetDataSource();
        public abstract IEnumerable GetDataSource(string listName);
        public abstract object GetSchema();
        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            this.DataSourceChanged(this, e);
        }
    }
}


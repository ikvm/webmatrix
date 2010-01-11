namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework;
    using Microsoft.Matrix.Framework.Web.UI.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [Designer(typeof(DataBoundControlDesigner))]
    public abstract class DataBoundControl : WebControl, INamingContainer
    {
        private object _dataSource;
        private IEnumerable _resolvedDataSource;
        internal const string DataSourceItemCountViewStateKey = "_!DataSourceItemCount";

        protected DataBoundControl()
        {
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            object obj2 = this.ViewState["_!DataSourceItemCount"];
            if ((obj2 != null) && (((int) obj2) != -1))
            {
                Microsoft.Matrix.Framework.Web.UI.DummyDataSource dataSource = new Microsoft.Matrix.Framework.Web.UI.DummyDataSource((int) obj2);
                this.CreateChildControls(dataSource, false);
                base.ClearChildViewState();
            }
        }

        protected abstract int CreateChildControls(IEnumerable dataSource, bool dataBinding);
        public override void DataBind()
        {
            base.OnDataBinding(EventArgs.Empty);
            this.Controls.Clear();
            base.ClearChildViewState();
            this.ResolveDataSource();
            int num = this.CreateChildControls(this._resolvedDataSource, true);
            base.ChildControlsCreated = true;
            this.ViewState["_!DataSourceItemCount"] = num;
            this.TrackViewState();
        }

        private Control FindDataControl(string controlID)
        {
            Control namingContainer = this;
            Control control2 = null;
            while ((namingContainer != this.Page) && (control2 == null))
            {
                namingContainer = namingContainer.NamingContainer;
                if (namingContainer == null)
                {
                    throw new HttpException(string.Format("No naming container available", this.ID));
                }
                control2 = namingContainer.FindControl(controlID);
            }
            return control2;
        }

        private void ResolveDataSource()
        {
            if (this.DataSourceControlID.Length != 0)
            {
                DataControl control = this.FindDataControl(this.DataSourceControlID) as DataControl;
                if (control == null)
                {
                    throw new HttpException(Microsoft.Matrix.Framework.SR.GetString("DataBoundControl_DataSourceControlIDMustBeDataControl"));
                }
                this._dataSource = control;
                this._resolvedDataSource = control.GetDataSource(this.DataMember);
            }
            else
            {
                object dataSource = this._dataSource;
                if (dataSource != null)
                {
                    if (dataSource is DataControl)
                    {
                        this._resolvedDataSource = Microsoft.Matrix.Framework.Web.UI.DataSourceHelper.GetResolvedDataSource(((DataControl) dataSource).GetDataSource(this.DataMember), null);
                    }
                    else
                    {
                        this._resolvedDataSource = Microsoft.Matrix.Framework.Web.UI.DataSourceHelper.GetResolvedDataSource(dataSource, this.DataMember);
                    }
                }
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("DataBoundControl_AutoDataBind"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), DefaultValue(true)]
        public virtual bool AutoDataBind
        {
            get
            {
                object obj2 = this.ViewState["AutoDataBind"];
                if (obj2 != null)
                {
                    return (bool) obj2;
                }
                return true;
            }
            set
            {
                if (!value)
                {
                    this.ViewState["AutoDataBind"] = false;
                }
                else
                {
                    this.ViewState.Remove("AutoDataBind");
                }
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("DataBoundControl_DataMember"), DefaultValue("")]
        public virtual string DataMember
        {
            get
            {
                object obj2 = this.ViewState["DataMember"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataMember"] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("DataBoundControl_DataSource"), DefaultValue((string) null), Bindable(true)]
        public virtual object DataSource
        {
            get
            {
                if (this._dataSource == null)
                {
                    this.ResolveDataSource();
                }
                return this._dataSource;
            }
            set
            {
                if (((value != null) && !(value is IListSource)) && (!(value is IEnumerable) && !(value is DataControl)))
                {
                    throw new ArgumentException(Microsoft.Matrix.Framework.SR.GetString("DataBoundControl_InvalidDataSourceType"));
                }
                this._dataSource = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("DataBoundControl_DataSourceControlID"), DefaultValue("")]
        public virtual string DataSourceControlID
        {
            get
            {
                object obj2 = this.ViewState["DataSourceControlID"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["DataSourceControlID"] = value;
            }
        }
    }
}


namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework;
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI.WebControls;

    public class HyperLinkField : MxDataGridField
    {
        private PropertyDescriptor textFieldDesc;
        private PropertyDescriptor urlFieldDesc;

        protected virtual string FormatDataNavigateUrlValue(object dataUrlValue)
        {
            string str = string.Empty;
            if ((dataUrlValue == null) || (dataUrlValue == DBNull.Value))
            {
                return str;
            }
            string dataNavigateUrlFormatString = this.DataNavigateUrlFormatString;
            if (dataNavigateUrlFormatString.Length == 0)
            {
                return dataUrlValue.ToString();
            }
            return string.Format(dataNavigateUrlFormatString, dataUrlValue);
        }

        protected virtual string FormatDataTextValue(object dataTextValue)
        {
            string str = string.Empty;
            if ((dataTextValue == null) || (dataTextValue == DBNull.Value))
            {
                return str;
            }
            string dataTextFormatString = this.DataTextFormatString;
            if (dataTextFormatString.Length == 0)
            {
                return dataTextValue.ToString();
            }
            return string.Format(dataTextFormatString, dataTextValue);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.textFieldDesc = null;
            this.urlFieldDesc = null;
        }

        public override void InitializeCell(TableCell cell, int fieldIndex, ListItemType itemType)
        {
            base.InitializeCell(cell, fieldIndex, itemType);
            if ((itemType != ListItemType.Header) && (itemType != ListItemType.Footer))
            {
                HyperLink child = new HyperLink();
                child.Text = this.Text;
                child.NavigateUrl = this.NavigateUrl;
                child.Target = this.Target;
                if ((this.DataNavigateUrlField.Length != 0) || (this.DataTextField.Length != 0))
                {
                    child.DataBinding += new EventHandler(this.OnDataBindField);
                }
                cell.Controls.Add(child);
            }
        }

        private void OnDataBindField(object sender, EventArgs e)
        {
            HyperLink link = (HyperLink) sender;
            MxDataGridItem namingContainer = (MxDataGridItem) link.NamingContainer;
            object dataItem = namingContainer.DataItem;
            if ((this.textFieldDesc == null) && (this.urlFieldDesc == null))
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(dataItem);
                string dataTextField = this.DataTextField;
                if (dataTextField.Length != 0)
                {
                    this.textFieldDesc = properties.Find(dataTextField, true);
                    if ((this.textFieldDesc == null) && !base.DesignMode)
                    {
                        throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("Field_Not_Found"), dataTextField));
                    }
                }
                dataTextField = this.DataNavigateUrlField;
                if (dataTextField.Length != 0)
                {
                    this.urlFieldDesc = properties.Find(dataTextField, true);
                    if ((this.urlFieldDesc == null) && !base.DesignMode)
                    {
                        throw new HttpException(string.Format("Field_Not_Found", dataTextField));
                    }
                }
            }
            if (this.textFieldDesc != null)
            {
                object dataTextValue = this.textFieldDesc.GetValue(dataItem);
                string str2 = this.FormatDataTextValue(dataTextValue);
                link.Text = str2;
            }
            else if (base.DesignMode && (this.DataTextField.Length != 0))
            {
                link.Text = Microsoft.Matrix.Framework.SR.GetString("Sample_Databound_Text");
            }
            if (this.urlFieldDesc != null)
            {
                object dataUrlValue = this.urlFieldDesc.GetValue(dataItem);
                string str3 = this.FormatDataNavigateUrlValue(dataUrlValue);
                link.NavigateUrl = str3;
            }
            else if (base.DesignMode && (this.DataNavigateUrlField.Length != 0))
            {
                link.NavigateUrl = "url";
            }
        }

        [DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("HyperLinkField_DataNavigateUrlField"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data")]
        public virtual string DataNavigateUrlField
        {
            get
            {
                object obj2 = base.ViewState["DataNavigateUrlField"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["DataNavigateUrlField"] = value;
                this.OnFieldChanged();
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), DefaultValue(""), Description("The formatting applied to the value bound to the NavigateUrl property.")]
        public virtual string DataNavigateUrlFormatString
        {
            get
            {
                object obj2 = base.ViewState["DataNavigateUrlFormatString"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["DataNavigateUrlFormatString"] = value;
                this.OnFieldChanged();
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("HyperLinkField_DataTextField")]
        public virtual string DataTextField
        {
            get
            {
                object obj2 = base.ViewState["DataTextField"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["DataTextField"] = value;
                this.OnFieldChanged();
            }
        }

        [Description("The formatting applied to the value bound to the Text property."), DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data")]
        public virtual string DataTextFormatString
        {
            get
            {
                object obj2 = base.ViewState["DataTextFormatString"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["DataTextFormatString"] = value;
                this.OnFieldChanged();
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior"), DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("HyperLinkField_NavigateUrl")]
        public virtual string NavigateUrl
        {
            get
            {
                object obj2 = base.ViewState["NavigateUrl"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["NavigateUrl"] = value;
                this.OnFieldChanged();
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior"), DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("HyperLinkField_Target")]
        public virtual string Target
        {
            get
            {
                object obj2 = base.ViewState["Target"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["Target"] = value;
                this.OnFieldChanged();
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Appearance"), DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("HyperLinkField_Text")]
        public virtual string Text
        {
            get
            {
                object obj2 = base.ViewState["Text"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["Text"] = value;
                this.OnFieldChanged();
            }
        }
    }
}


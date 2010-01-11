namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework;
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ButtonField : MxDataGridField
    {
        private PropertyDescriptor textFieldDesc;

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
        }

        public override void InitializeCell(TableCell cell, int fieldIndex, ListItemType itemType)
        {
            base.InitializeCell(cell, fieldIndex, itemType);
            if ((itemType != ListItemType.Header) && (itemType != ListItemType.Footer))
            {
                WebControl child = null;
                if (this.ButtonType == ButtonColumnType.LinkButton)
                {
                    LinkButton button = new MxDataGridLinkButton();
                    button.Text = this.Text;
                    button.CommandName = this.CommandName;
                    button.CausesValidation = false;
                    child = button;
                }
                else
                {
                    Button button2 = new Button();
                    button2.Text = this.Text;
                    button2.CommandName = this.CommandName;
                    button2.CausesValidation = false;
                    child = button2;
                }
                if (this.DataTextField.Length != 0)
                {
                    child.DataBinding += new EventHandler(this.OnDataBindField);
                }
                cell.Controls.Add(child);
            }
        }

        private void OnDataBindField(object sender, EventArgs e)
        {
            string str2;
            Control control = (Control) sender;
            MxDataGridItem namingContainer = (MxDataGridItem) control.NamingContainer;
            object dataItem = namingContainer.DataItem;
            if (this.textFieldDesc == null)
            {
                string dataTextField = this.DataTextField;
                this.textFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(dataTextField, true);
                if ((this.textFieldDesc == null) && !base.DesignMode)
                {
                    throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("Field_Not_Found"), dataTextField));
                }
            }
            if (this.textFieldDesc != null)
            {
                object dataTextValue = this.textFieldDesc.GetValue(dataItem);
                str2 = this.FormatDataTextValue(dataTextValue);
            }
            else
            {
                str2 = Microsoft.Matrix.Framework.SR.GetString("Sample_Databound_Text");
            }
            if (control is LinkButton)
            {
                ((LinkButton) control).Text = str2;
            }
            else
            {
                ((Button) control).Text = str2;
            }
        }

        [DefaultValue(0), Microsoft.Matrix.Framework.Web.UI.WebCategory("Appearance"), Description("The type of button contained within the field.")]
        public virtual ButtonColumnType ButtonType
        {
            get
            {
                object obj2 = base.ViewState["ButtonType"];
                if (obj2 != null)
                {
                    return (ButtonColumnType) obj2;
                }
                return ButtonColumnType.LinkButton;
            }
            set
            {
                if ((value < ButtonColumnType.LinkButton) || (value > ButtonColumnType.PushButton))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                base.ViewState["ButtonType"] = value;
                this.OnFieldChanged();
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior"), Description("The command associated with the button."), DefaultValue("")]
        public virtual string CommandName
        {
            get
            {
                object obj2 = base.ViewState["CommandName"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["CommandName"] = value;
                this.OnFieldChanged();
            }
        }

        [Description("The field bound to the text property of the button."), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), DefaultValue("")]
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

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), Description("The formatting applied to the value bound to the Text property."), DefaultValue("")]
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

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Appearance"), DefaultValue(""), Description("The text used for the button.")]
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


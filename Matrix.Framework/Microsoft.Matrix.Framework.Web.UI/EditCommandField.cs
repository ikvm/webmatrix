namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class EditCommandField : MxDataGridField
    {
        public override void InitializeCell(TableCell cell, int fieldIndex, ListItemType itemType)
        {
            base.InitializeCell(cell, fieldIndex, itemType);
            if ((itemType != ListItemType.Header) && (itemType != ListItemType.Footer))
            {
                if (itemType == ListItemType.EditItem)
                {
                    ControlCollection controls = cell.Controls;
                    ButtonColumnType buttonType = this.ButtonType;
                    WebControl child = null;
                    if (buttonType == ButtonColumnType.LinkButton)
                    {
                        LinkButton button = new MxDataGridLinkButton();
                        child = button;
                        button.CommandName = "Update";
                        button.Text = this.UpdateText;
                    }
                    else
                    {
                        Button button2 = new Button();
                        child = button2;
                        button2.CommandName = "Update";
                        button2.Text = this.UpdateText;
                    }
                    controls.Add(child);
                    LiteralControl control2 = new LiteralControl("&nbsp;");
                    controls.Add(control2);
                    if (buttonType == ButtonColumnType.LinkButton)
                    {
                        LinkButton button3 = new MxDataGridLinkButton();
                        child = button3;
                        button3.CommandName = "Cancel";
                        button3.Text = this.CancelText;
                        button3.CausesValidation = false;
                    }
                    else
                    {
                        Button button4 = new Button();
                        child = button4;
                        button4.CommandName = "Cancel";
                        button4.Text = this.CancelText;
                        button4.CausesValidation = false;
                    }
                    controls.Add(child);
                }
                else
                {
                    ControlCollection controls2 = cell.Controls;
                    ButtonColumnType type2 = this.ButtonType;
                    WebControl control3 = null;
                    if (type2 == ButtonColumnType.LinkButton)
                    {
                        LinkButton button5 = new MxDataGridLinkButton();
                        control3 = button5;
                        button5.CommandName = "Edit";
                        button5.Text = this.EditText;
                        button5.CausesValidation = false;
                    }
                    else
                    {
                        Button button6 = new Button();
                        control3 = button6;
                        button6.CommandName = "Edit";
                        button6.Text = this.EditText;
                        button6.CausesValidation = false;
                    }
                    controls2.Add(control3);
                }
            }
        }

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

        public virtual string CancelText
        {
            get
            {
                object obj2 = base.ViewState["CancelText"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["CancelText"] = value;
                this.OnFieldChanged();
            }
        }

        public virtual string EditText
        {
            get
            {
                object obj2 = base.ViewState["EditText"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["EditText"] = value;
                this.OnFieldChanged();
            }
        }

        public virtual string UpdateText
        {
            get
            {
                object obj2 = base.ViewState["UpdateText"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["UpdateText"] = value;
                this.OnFieldChanged();
            }
        }
    }
}


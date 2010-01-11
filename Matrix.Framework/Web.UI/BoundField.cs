namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework;
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class BoundField : MxDataGridField
    {
        private string boundField;
        private PropertyDescriptor boundFieldDesc;
        private bool boundFieldDescValid;
        private string formatting;
        public static readonly string thisExpr = "!";

        protected virtual string FormatDataValue(object dataValue)
        {
            string str = string.Empty;
            if ((dataValue == null) || (dataValue == DBNull.Value))
            {
                return str;
            }
            if (this.formatting.Length == 0)
            {
                return dataValue.ToString();
            }
            return string.Format(this.formatting, dataValue);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.boundFieldDesc = null;
            this.boundFieldDescValid = false;
            this.boundField = this.DataField;
            this.formatting = this.DataFormatString;
        }

        public override void InitializeCell(TableCell cell, int fieldIndex, ListItemType itemType)
        {
            base.InitializeCell(cell, fieldIndex, itemType);
            Control child = null;
            Control control2 = null;
            switch (itemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                case ListItemType.SelectedItem:
                    break;

                case ListItemType.EditItem:
                {
                    if (this.ReadOnly)
                    {
                        break;
                    }
                    TextBox box = new TextBox();
                    child = box;
                    if (this.boundField.Length != 0)
                    {
                        control2 = box;
                    }
                    goto Label_005F;
                }
                default:
                    goto Label_005F;
            }
            if (this.DataField.Length != 0)
            {
                control2 = cell;
            }
        Label_005F:
            if (child != null)
            {
                cell.Controls.Add(child);
            }
            if (control2 != null)
            {
                control2.DataBinding += new EventHandler(this.OnDataBindField);
            }
        }

        private void OnDataBindField(object sender, EventArgs e)
        {
            string str;
            Control control = (Control) sender;
            MxDataGridItem namingContainer = (MxDataGridItem) control.NamingContainer;
            object dataItem = namingContainer.DataItem;
            if (!this.boundFieldDescValid)
            {
                if (!this.boundField.Equals(thisExpr))
                {
                    this.boundFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(this.boundField, true);
                    if ((this.boundFieldDesc == null) && !base.DesignMode)
                    {
                        throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("Field_Not_Found"), this.boundField));
                    }
                }
                this.boundFieldDescValid = true;
            }
            object dataValue = dataItem;
            if ((this.boundFieldDesc == null) && base.DesignMode)
            {
                str = Microsoft.Matrix.Framework.SR.GetString("Sample_Databound_Text");
            }
            else
            {
                if (this.boundFieldDesc != null)
                {
                    dataValue = this.boundFieldDesc.GetValue(dataItem);
                }
                str = this.FormatDataValue(dataValue);
            }
            if (control is TableCell)
            {
                if (str.Length == 0)
                {
                    str = "&nbsp;";
                }
                ((TableCell) control).Text = str;
            }
            else
            {
                ((TextBox) control).Text = str;
            }
        }

        [DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("BoundField_DataField"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data")]
        public virtual string DataField
        {
            get
            {
                object obj2 = base.ViewState["DataField"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["DataField"] = value;
                this.OnFieldChanged();
            }
        }

        [DefaultValue(""), Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("BoundField_DataFormatString")]
        public virtual string DataFormatString
        {
            get
            {
                object obj2 = base.ViewState["DataFormatString"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                base.ViewState["DataFormatString"] = value;
                this.OnFieldChanged();
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("BoundField_ReadOnly"), DefaultValue(false), Microsoft.Matrix.Framework.Web.UI.WebCategory("Behavior")]
        public virtual bool ReadOnly
        {
            get
            {
                object obj2 = base.ViewState["ReadOnly"];
                return ((obj2 != null) && ((bool) obj2));
            }
            set
            {
                base.ViewState["ReadOnly"] = value;
                this.OnFieldChanged();
            }
        }
    }
}


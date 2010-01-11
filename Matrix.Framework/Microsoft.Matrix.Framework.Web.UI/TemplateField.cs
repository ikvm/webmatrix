namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class TemplateField : MxDataGridField
    {
        private ITemplate editItemTemplate;
        private ITemplate footerTemplate;
        private ITemplate headerTemplate;
        private ITemplate itemTemplate;

        public override void InitializeCell(TableCell cell, int fieldIndex, ListItemType itemType)
        {
            base.InitializeCell(cell, fieldIndex, itemType);
            ITemplate itemTemplate = null;
            switch (itemType)
            {
                case ListItemType.Header:
                    itemTemplate = this.headerTemplate;
                    goto Label_0057;

                case ListItemType.Footer:
                    itemTemplate = this.footerTemplate;
                    goto Label_0057;

                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                case ListItemType.SelectedItem:
                    break;

                case ListItemType.EditItem:
                    if (this.editItemTemplate == null)
                    {
                        break;
                    }
                    itemTemplate = this.editItemTemplate;
                    goto Label_0057;

                default:
                    goto Label_0057;
            }
            itemTemplate = this.itemTemplate;
        Label_0057:
            if (itemTemplate != null)
            {
                cell.Text = string.Empty;
                itemTemplate.InstantiateIn(cell);
            }
        }

        [TemplateContainer(typeof(MxDataGridItem)), Browsable(false), DefaultValue((string) null), WebSysDescription("TemplateField_EditItemTemplate"), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate EditItemTemplate
        {
            get
            {
                return this.editItemTemplate;
            }
            set
            {
                this.editItemTemplate = value;
                this.OnFieldChanged();
            }
        }

        [DefaultValue((string) null), PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(MxDataGridItem)), WebSysDescription("TemplateField_FooterTemplate"), Browsable(false)]
        public virtual ITemplate FooterTemplate
        {
            get
            {
                return this.footerTemplate;
            }
            set
            {
                this.footerTemplate = value;
                this.OnFieldChanged();
            }
        }

        [TemplateContainer(typeof(MxDataGridItem)), Browsable(false), DefaultValue((string) null), WebSysDescription("TemplateField_HeaderTemplate"), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate HeaderTemplate
        {
            get
            {
                return this.headerTemplate;
            }
            set
            {
                this.headerTemplate = value;
                this.OnFieldChanged();
            }
        }

        [WebSysDescription("TemplateField_ItemTemplate"), TemplateContainer(typeof(MxDataGridItem)), DefaultValue((string) null), PersistenceMode(PersistenceMode.InnerProperty), Browsable(false)]
        public virtual ITemplate ItemTemplate
        {
            get
            {
                return this.itemTemplate;
            }
            set
            {
                this.itemTemplate = value;
                this.OnFieldChanged();
            }
        }
    }
}


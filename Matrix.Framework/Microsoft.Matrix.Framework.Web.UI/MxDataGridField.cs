namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class MxDataGridField : IStateManager
    {
        private bool designMode;
        private TableItemStyle footerStyle;
        private TableItemStyle headerStyle;
        private TableItemStyle itemStyle;
        private bool marked;
        private MxDataGrid owner;
        private StateBag statebag = new StateBag();

        public virtual void Initialize()
        {
            if ((this.owner != null) && (this.owner.Site != null))
            {
                this.designMode = this.owner.Site.DesignMode;
            }
        }

        public virtual void InitializeCell(TableCell cell, int fieldIndex, ListItemType itemType)
        {
            WebControl control;
            switch (itemType)
            {
                case ListItemType.Header:
                {
                    control = null;
                    bool flag = true;
                    string sortExpression = null;
                    if ((this.owner != null) && !this.owner.AllowSorting)
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        sortExpression = this.SortExpression;
                        if (sortExpression.Length == 0)
                        {
                            flag = false;
                        }
                    }
                    string headerImageUrl = this.HeaderImageUrl;
                    if (headerImageUrl.Length == 0)
                    {
                        string headerText = this.HeaderText;
                        if (flag)
                        {
                            LinkButton button2 = new MxDataGridLinkButton();
                            button2.Text = headerText;
                            button2.CommandName = "Sort";
                            button2.CommandArgument = sortExpression;
                            button2.CausesValidation = false;
                            control = button2;
                        }
                        else
                        {
                            if (headerText.Length == 0)
                            {
                                headerText = "&nbsp;";
                            }
                            cell.Text = headerText;
                        }
                        break;
                    }
                    if (flag)
                    {
                        ImageButton button = new ImageButton();
                        button.ImageUrl = this.HeaderImageUrl;
                        button.CommandName = "Sort";
                        button.CommandArgument = sortExpression;
                        button.CausesValidation = false;
                        control = button;
                    }
                    else
                    {
                        Image image = new Image();
                        image.ImageUrl = headerImageUrl;
                        control = image;
                    }
                    break;
                }
                case ListItemType.Footer:
                {
                    string footerText = this.FooterText;
                    if (footerText.Length == 0)
                    {
                        footerText = "&nbsp;";
                    }
                    cell.Text = footerText;
                    return;
                }
                default:
                    return;
            }
            if (control != null)
            {
                cell.Controls.Add(control);
            }
        }

        protected virtual void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[]) savedState;
                if (objArray[0] != null)
                {
                    ((IStateManager) this.ViewState).LoadViewState(objArray[0]);
                }
                if (objArray[1] != null)
                {
                    ((IStateManager) this.ItemStyle).LoadViewState(objArray[1]);
                }
                if (objArray[2] != null)
                {
                    ((IStateManager) this.HeaderStyle).LoadViewState(objArray[2]);
                }
                if (objArray[3] != null)
                {
                    ((IStateManager) this.FooterStyle).LoadViewState(objArray[3]);
                }
            }
        }

        protected virtual void OnFieldChanged()
        {
            if (this.owner != null)
            {
                this.owner.OnFieldsChanged();
            }
        }

        protected virtual object SaveViewState()
        {
            object obj2 = ((IStateManager) this.ViewState).SaveViewState();
            object obj3 = (this.itemStyle != null) ? ((IStateManager) this.itemStyle).SaveViewState() : null;
            object obj4 = (this.headerStyle != null) ? ((IStateManager) this.headerStyle).SaveViewState() : null;
            object obj5 = (this.footerStyle != null) ? ((IStateManager) this.footerStyle).SaveViewState() : null;
            if (((obj2 == null) && (obj3 == null)) && ((obj4 == null) && (obj5 == null)))
            {
                return null;
            }
            return new object[] { obj2, obj3, obj4, obj5 };
        }

        internal virtual void SetOwner(MxDataGrid owner)
        {
            this.owner = owner;
        }

        void IStateManager.LoadViewState(object state)
        {
            this.LoadViewState(state);
        }

        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }

        public override string ToString()
        {
            return string.Empty;
        }

        protected virtual void TrackViewState()
        {
            this.marked = true;
            ((IStateManager) this.ViewState).TrackViewState();
            if (this.itemStyle != null)
            {
                ((IStateManager) this.itemStyle).TrackViewState();
            }
            if (this.headerStyle != null)
            {
                ((IStateManager) this.headerStyle).TrackViewState();
            }
            if (this.footerStyle != null)
            {
                ((IStateManager) this.footerStyle).TrackViewState();
            }
            if (this.itemStyle != null)
            {
                ((IStateManager) this.itemStyle).TrackViewState();
            }
        }

        protected bool DesignMode
        {
            get
            {
                return this.designMode;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty), WebSysDescription("MxDataGridField_FooterStyle"), WebCategory("Style"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue((string) null)]
        public virtual TableItemStyle FooterStyle
        {
            get
            {
                if (this.footerStyle == null)
                {
                    this.footerStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager) this.footerStyle).TrackViewState();
                    }
                }
                return this.footerStyle;
            }
        }

        internal TableItemStyle FooterStyleInternal
        {
            get
            {
                return this.footerStyle;
            }
            set
            {
                this.footerStyle = value;
            }
        }

        [WebSysDescription("MxDataGridField_FooterText"), WebCategory("Appearance"), DefaultValue("")]
        public virtual string FooterText
        {
            get
            {
                object obj2 = this.ViewState["FooterText"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["FooterText"] = value;
                this.OnFieldChanged();
            }
        }

        [WebCategory("Appearance"), DefaultValue(""), WebSysDescription("MxDataGridField_HeaderImageUrl")]
        public virtual string HeaderImageUrl
        {
            get
            {
                object obj2 = this.ViewState["HeaderImageUrl"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["HeaderImageUrl"] = value;
                this.OnFieldChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), WebSysDescription("MxDataGridField_HeaderStyle"), WebCategory("Style"), PersistenceMode(PersistenceMode.InnerProperty), DefaultValue((string) null)]
        public virtual TableItemStyle HeaderStyle
        {
            get
            {
                if (this.headerStyle == null)
                {
                    this.headerStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager) this.headerStyle).TrackViewState();
                    }
                }
                return this.headerStyle;
            }
        }

        internal TableItemStyle HeaderStyleInternal
        {
            get
            {
                return this.headerStyle;
            }
            set
            {
                this.headerStyle = value;
            }
        }

        [WebSysDescription("MxDataGridField_HeaderText"), WebCategory("Appearance"), DefaultValue("")]
        public virtual string HeaderText
        {
            get
            {
                object obj2 = this.ViewState["HeaderText"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["HeaderText"] = value;
                this.OnFieldChanged();
            }
        }

        protected bool IsTrackingViewState
        {
            get
            {
                return this.marked;
            }
        }

        [DefaultValue((string) null), PersistenceMode(PersistenceMode.InnerProperty), WebSysDescription("MxDataGridField_ItemStyle"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), WebCategory("Style")]
        public virtual TableItemStyle ItemStyle
        {
            get
            {
                if (this.itemStyle == null)
                {
                    this.itemStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager) this.itemStyle).TrackViewState();
                    }
                }
                return this.itemStyle;
            }
        }

        internal TableItemStyle ItemStyleInternal
        {
            get
            {
                return this.itemStyle;
            }
            set
            {
                this.itemStyle = value;
            }
        }

        protected MxDataGrid Owner
        {
            get
            {
                return this.owner;
            }
        }

        [DefaultValue(""), WebCategory("Behavior"), WebSysDescription("MxDataGridField_SortExpression")]
        public virtual string SortExpression
        {
            get
            {
                object obj2 = this.ViewState["SortExpression"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["SortExpression"] = value;
                this.OnFieldChanged();
            }
        }

        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return this.IsTrackingViewState;
            }
        }

        protected StateBag ViewState
        {
            get
            {
                return this.statebag;
            }
        }

        [WebCategory("Behavior"), DefaultValue(true), WebSysDescription("MxDataGridField_Visible")]
        public bool Visible
        {
            get
            {
                object obj2 = this.ViewState["Visible"];
                if (obj2 != null)
                {
                    return (bool) obj2;
                }
                return true;
            }
            set
            {
                this.ViewState["Visible"] = value;
                this.OnFieldChanged();
            }
        }
    }
}


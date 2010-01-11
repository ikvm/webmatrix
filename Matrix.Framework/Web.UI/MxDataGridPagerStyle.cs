namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public sealed class MxDataGridPagerStyle : TableItemStyle
    {
        private MxDataGrid owner;
        private const int PROP_MODE = 0x80000;
        private const int PROP_NEXTPAGETEXT = 0x100000;
        private const int PROP_PAGEBUTTONCOUNT = 0x400000;
        private const int PROP_POSITION = 0x800000;
        private const int PROP_PREVPAGETEXT = 0x200000;
        private const int PROP_VISIBLE = 0x1000000;
        private int setPagerBits;

        internal MxDataGridPagerStyle(MxDataGrid owner)
        {
            this.owner = owner;
        }

        public override void CopyFrom(Style s)
        {
            if (s != null)
            {
                base.CopyFrom(s);
                if (s is MxDataGridPagerStyle)
                {
                    MxDataGridPagerStyle style = (MxDataGridPagerStyle) s;
                    if (style.IsPagerSet(0x80000))
                    {
                        this.Mode = style.Mode;
                    }
                    if (style.IsPagerSet(0x100000))
                    {
                        this.NextPageText = style.NextPageText;
                    }
                    if (style.IsPagerSet(0x200000))
                    {
                        this.PrevPageText = style.PrevPageText;
                    }
                    if (style.IsPagerSet(0x400000))
                    {
                        this.PageButtonCount = style.PageButtonCount;
                    }
                    if (style.IsPagerSet(0x800000))
                    {
                        this.Position = style.Position;
                    }
                    if (style.IsPagerSet(0x1000000))
                    {
                        this.Visible = style.Visible;
                    }
                }
            }
        }

        internal bool IsPagerSet(int propKey)
        {
            return ((propKey & this.setPagerBits) != 0);
        }

        public override void MergeWith(Style s)
        {
            if (s != null)
            {
                if (this.IsEmpty)
                {
                    this.CopyFrom(s);
                }
                else
                {
                    base.MergeWith(s);
                    if (s is MxDataGridPagerStyle)
                    {
                        MxDataGridPagerStyle style = (MxDataGridPagerStyle) s;
                        if (style.IsPagerSet(0x80000) && !this.IsPagerSet(0x80000))
                        {
                            this.Mode = style.Mode;
                        }
                        if (style.IsPagerSet(0x100000) && !this.IsPagerSet(0x100000))
                        {
                            this.NextPageText = style.NextPageText;
                        }
                        if (style.IsPagerSet(0x200000) && !this.IsPagerSet(0x200000))
                        {
                            this.PrevPageText = style.PrevPageText;
                        }
                        if (style.IsPagerSet(0x400000) && !this.IsPagerSet(0x400000))
                        {
                            this.PageButtonCount = style.PageButtonCount;
                        }
                        if (style.IsPagerSet(0x800000) && !this.IsPagerSet(0x800000))
                        {
                            this.Position = style.Position;
                        }
                        if (style.IsPagerSet(0x1000000) && !this.IsPagerSet(0x1000000))
                        {
                            this.Visible = style.Visible;
                        }
                    }
                }
            }
        }

        public override void Reset()
        {
            if (this.IsPagerSet(0x80000))
            {
                base.ViewState.Remove("Mode");
            }
            if (this.IsPagerSet(0x100000))
            {
                base.ViewState.Remove("NextPageText");
            }
            if (this.IsPagerSet(0x200000))
            {
                base.ViewState.Remove("PrevPageText");
            }
            if (this.IsPagerSet(0x400000))
            {
                base.ViewState.Remove("PageButtonCount");
            }
            if (this.IsPagerSet(0x800000))
            {
                base.ViewState.Remove("Position");
            }
            if (this.IsPagerSet(0x1000000))
            {
                base.ViewState.Remove("PagerVisible");
            }
            base.Reset();
        }

        internal void SetPagerBit(int bit)
        {
            this.setPagerBits |= bit;
        }

        internal bool IsPagerOnBottom
        {
            get
            {
                PagerPosition position = this.Position;
                if (position != PagerPosition.Bottom)
                {
                    return (position == PagerPosition.TopAndBottom);
                }
                return true;
            }
        }

        internal bool IsPagerOnTop
        {
            get
            {
                PagerPosition position = this.Position;
                if (position != PagerPosition.Top)
                {
                    return (position == PagerPosition.TopAndBottom);
                }
                return true;
            }
        }

        [WebCategory("Appearance"), Bindable(true), WebSysDescription("MxDataGridPagerStyle_Mode"), NotifyParentProperty(true), DefaultValue(0)]
        public PagerMode Mode
        {
            get
            {
                if (this.IsPagerSet(0x80000))
                {
                    return (PagerMode) base.ViewState["Mode"];
                }
                return PagerMode.NextPrev;
            }
            set
            {
                if ((value < PagerMode.NextPrev) || (value > PagerMode.NumericPages))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                base.ViewState["Mode"] = value;
                this.SetBit(0x80000);
                this.SetPagerBit(0x80000);
                this.owner.OnPagerChanged();
            }
        }

        [WebSysDescription("MxDataGridPagerStyle_NextPageText"), DefaultValue("&gt;"), Bindable(true), NotifyParentProperty(true), WebCategory("Appearance")]
        public string NextPageText
        {
            get
            {
                if (this.IsPagerSet(0x100000))
                {
                    return (string) base.ViewState["NextPageText"];
                }
                return "&gt;";
            }
            set
            {
                base.ViewState["NextPageText"] = value;
                this.SetBit(0x100000);
                this.SetPagerBit(0x100000);
                this.owner.OnPagerChanged();
            }
        }

        [DefaultValue(10), NotifyParentProperty(true), WebSysDescription("MxDataGridPagerStyle_PageButtonCount"), WebCategory("Behavior"), Bindable(true)]
        public int PageButtonCount
        {
            get
            {
                if (this.IsPagerSet(0x400000))
                {
                    return (int) base.ViewState["PageButtonCount"];
                }
                return 10;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                base.ViewState["PageButtonCount"] = value;
                this.SetBit(0x400000);
                this.SetPagerBit(0x400000);
                this.owner.OnPagerChanged();
            }
        }

        [WebSysDescription("MxDataGridPagerStyle_Position"), Bindable(true), WebCategory("Layout"), DefaultValue(0), NotifyParentProperty(true)]
        public PagerPosition Position
        {
            get
            {
                if (this.IsPagerSet(0x800000))
                {
                    return (PagerPosition) base.ViewState["Position"];
                }
                return PagerPosition.Bottom;
            }
            set
            {
                if ((value < PagerPosition.Bottom) || (value > PagerPosition.TopAndBottom))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                base.ViewState["Position"] = value;
                this.SetBit(0x800000);
                this.SetPagerBit(0x800000);
                this.owner.OnPagerChanged();
            }
        }

        [WebSysDescription("MxDataGridPagerStyle_PrevPageText"), DefaultValue("&lt;"), Bindable(true), WebCategory("Appearance"), NotifyParentProperty(true)]
        public string PrevPageText
        {
            get
            {
                if (this.IsPagerSet(0x200000))
                {
                    return (string) base.ViewState["PrevPageText"];
                }
                return "&lt;";
            }
            set
            {
                base.ViewState["PrevPageText"] = value;
                this.SetBit(0x200000);
                this.SetPagerBit(0x200000);
                this.owner.OnPagerChanged();
            }
        }

        [NotifyParentProperty(true), WebSysDescription("MxDataGridPagerStyle_Visible"), DefaultValue(true), Bindable(true), WebCategory("Appearance")]
        public bool Visible
        {
            get
            {
                if (this.IsPagerSet(0x1000000))
                {
                    return (bool) base.ViewState["PagerVisible"];
                }
                return true;
            }
            set
            {
                base.ViewState["PagerVisible"] = value;
                this.SetBit(0x1000000);
                this.SetPagerBit(0x1000000);
                this.owner.OnPagerChanged();
            }
        }
    }
}


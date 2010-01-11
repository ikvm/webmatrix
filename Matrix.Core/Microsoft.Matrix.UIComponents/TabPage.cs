namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class TabPage : Panel
    {
        private int _imageIndex;
        private System.Drawing.Rectangle _tabRectangle;
        private int _tabToolTipID;
        private string _toolTipText;

        //events
        public TabPage() : this(null)
        {
        }

        public TabPage(string text)
        {
            this.Text = text;
            this._imageIndex = -1;
            this._tabToolTipID = -1;
        }


        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new TabPageControlCollection(this);
        }

        public static TabPage GetTabPageOfControl(Control control)
        {
            Control parent = control;
            while ((parent != null) && !(parent is TabPage))
            {
                parent = parent.Parent;
            }
            return (TabPage) parent;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.UpdateParent(false);
        }


        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            // UNDONE: 修改边框使其可以支持TabPlacement.Left和TabPlacement.Left
            Control parent = base.Parent;
            if (parent is TabControl)
            {
                System.Drawing.Rectangle displayRectangle = parent.DisplayRectangle;
                base.SetBoundsCore(displayRectangle.X, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height, BoundsSpecified.All);
            }
            else
            {
                base.SetBoundsCore(x, y, width, height, specified);
            }
        }

        internal void UpdateParent(bool relayoutRequired)
        {
            Control parent = base.Parent;
            if ((parent != null) && (parent is TabControl))
            {
                ((TabControl) parent).OnTabChanged(this, relayoutRequired);
            }
        }

        public int ImageIndex
        {
            get
            {
                return this._imageIndex;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._imageIndex = value;
                this.UpdateParent(false);
            }
        }

        internal System.Drawing.Rectangle Rectangle
        {
            get
            {
                return this._tabRectangle;
            }
            set
            {
                this._tabRectangle = value;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (this.Text != value)
                {
                    base.Text = value;
                    this.UpdateParent(true);
                }
            }
        }

        internal int ToolTipID
        {
            get
            {
                return this._tabToolTipID;
            }
            set
            {
                this._tabToolTipID = value;
            }
        }

        public string ToolTipText
        {
            get
            {
                if (this._toolTipText == null)
                {
                    return string.Empty;
                }
                return this._toolTipText;
            }
            set
            {
                if (value != this._toolTipText)
                {
                    this._toolTipText = value;
                    this.UpdateParent(false);
                }
            }
        }

        public class TabPageControlCollection : Control.ControlCollection
        {
            public TabPageControlCollection(TabPage owner) : base(owner)
            {
            }

            public override void Add(Control value)
            {
                if (value is TabPage)
                {
                    throw new Exception("Cannot add another TabPage into a TabPage directly");
                }
                base.Add(value);
            }
        }


    }
}


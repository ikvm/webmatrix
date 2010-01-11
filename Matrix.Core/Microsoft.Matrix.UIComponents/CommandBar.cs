namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class CommandBar : Panel
    {
        private MainMenu _menuBar;
        private MxToolBar _topRightToolBar;
        private const int CommandBarPadding = 1;
        private const int GripperOffset = 8;
        private const int StripHeight = 2;
        private const int StripOffsetX = 2;
        private const int StripOffsetY = 1;
        private const int StripsPerGripper = 8;
        private const int StripWidth = 3;

        public CommandBar()
        {
            this.Dock = DockStyle.Top;
            base.Height = 0;
        }

        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new ControlCollection(this);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            int num2;
            int num = 2;
            bool flag = ((this._topRightToolBar != null) && this._topRightToolBar.Visible) && (base.Controls.Count > 1);
            for (num2 = 0; num2 < base.Controls.Count; num2++)
            {
                ToolBar bar = (ToolBar) base.Controls[num2];
                if (bar.Visible && (!flag || (bar != this._topRightToolBar)))
                {
                    num += bar.Height;
                }
            }
            num = Math.Max(num, 0x18);
            base.Height = num;
            Rectangle displayRectangle = this.DisplayRectangle;
            int top = displayRectangle.Top;
            int width = 0;
            bool flag2 = false;
            if (flag)
            {
                width = this._topRightToolBar.Width;
                flag2 = true;
            }
            for (num2 = 0; num2 < base.Controls.Count; num2++)
            {
                ToolBar bar2 = (ToolBar) base.Controls[num2];
                if (bar2.Visible)
                {
                    if (flag && (bar2 == this._topRightToolBar))
                    {
                        bar2.SetBounds((displayRectangle.Left + displayRectangle.Width) - width, displayRectangle.Top, width, bar2.Height, BoundsSpecified.All);
                    }
                    else
                    {
                        int num5 = displayRectangle.Width;
                        if (flag2)
                        {
                            num5 -= width;
                            flag2 = false;
                        }
                        bar2.SetBounds(displayRectangle.Left, top, num5, bar2.Height, BoundsSpecified.All);
                        top += bar2.Height;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            Rectangle displayRectangle = this.DisplayRectangle;
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(SystemBrushes.Control, 0, 0, clientRectangle.Width, clientRectangle.Height);
            int top = displayRectangle.Top;
            for (int i = 0; i < base.Controls.Count; i++)
            {
                ToolBar bar = (ToolBar) base.Controls[i];
                if (bar.Visible)
                {
                    int num5 = top + ((bar.Height - 0x10) / 2);
                    for (int j = 0; j < 8; j++)
                    {
                        int num3 = ((j * 2) + num5) + 1;
                        graphics.DrawLine(SystemPens.ControlDark, 2, num3, 4, num3);
                    }
                    top += bar.Height;
                }
            }
        }

        protected virtual void OnToolBarAdded(ToolBar toolBar)
        {
            toolBar.VisibleChanged += new EventHandler(this.OnToolBarVisibleChanged);
            base.PerformLayout();
        }

        protected virtual void OnToolBarRemoved(ToolBar toolBar)
        {
            toolBar.VisibleChanged -= new EventHandler(this.OnToolBarVisibleChanged);
            base.PerformLayout();
        }

        protected virtual void OnToolBarVisibleChanged(object sender, EventArgs e)
        {
            base.PerformLayout();
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                return new Rectangle(8, 1, clientRectangle.Width - 8, clientRectangle.Height - 2);
            }
        }

        public MainMenu MenuBar
        {
            get
            {
                return this._menuBar;
            }
            set
            {
                this._menuBar = value;
            }
        }

        public MxToolBar TopRightToolBar
        {
            get
            {
                return this._topRightToolBar;
            }
            set
            {
                if (this._topRightToolBar != value)
                {
                    if (value.Parent != this)
                    {
                        throw new ArgumentException();
                    }
                    this._topRightToolBar = value;
                    base.PerformLayout();
                }
            }
        }

        public class ControlCollection : Control.ControlCollection
        {
            private CommandBar _owner;

            public ControlCollection(CommandBar owner) : base(owner)
            {
                this._owner = owner;
            }

            public override void Add(Control value)
            {
                ToolBar bar = value as MxToolBar;
                if (bar == null)
                {
                    throw new ArgumentException();
                }
                base.Add(bar);
                this._owner.OnToolBarAdded(bar);
            }

            public override void Remove(Control value)
            {
                ToolBar bar = value as MxToolBar;
                if (bar == null)
                {
                    throw new ArgumentException();
                }
                base.Remove(bar);
                this._owner.OnToolBarRemoved(bar);
            }
        }
    }
}


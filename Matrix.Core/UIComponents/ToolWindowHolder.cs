namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class ToolWindowHolder : TabControl
    {
        private ImageList _toolWindowImages = new ImageList();

        public ToolWindowHolder()
        {
            this._toolWindowImages.ImageSize = new Size(0x10, 0x10);
            this._toolWindowImages.ColorDepth = ColorDepth.Depth32Bit;
            this.Dock = DockStyle.Fill;
            base.ImageList = this._toolWindowImages;
            base.TabPlacement = TabPlacement.Bottom;
            base.Mode = TabControlMode.TextAndImage;
        }

        public void AddToolWindow(ToolWindow toolWindow, bool makeActive)
        {
            TabPage page = new TabPage();
            Icon icon = toolWindow.Icon;
            if (icon == null)
            {
                icon = Icon.FromHandle(new Bitmap(0x10, 0x10).GetHicon());
            }
            this._toolWindowImages.Images.Add(icon);
            page.Text = toolWindow.ShortText;
            page.ToolTipText = toolWindow.Text;
            page.ImageIndex = this._toolWindowImages.Images.Count - 1;
            base.Tabs.Add(page);
            page.Controls.Add(toolWindow);
            toolWindow.EnsureActive += new EventHandler(this.OnToolWindowEnsureActive);
            if (makeActive || (base.Tabs.Count == 1))
            {
                this.SelectedIndex = base.Tabs.Count - 1;
                base.Parent.Text = toolWindow.Text;
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            string text = string.Empty;
            int selectedIndex = this.SelectedIndex;
            if (selectedIndex != -1)
            {
                TabPage page = base.Tabs[selectedIndex];
                text = page.Controls[0].Text;
            }
            base.Parent.Text = text;
        }

        private void OnToolWindowEnsureActive(object sender, EventArgs e)
        {
            ToolWindow window = (ToolWindow) sender;
            int index = base.Controls.IndexOf((TabPage) window.Parent);
            if (index != -1)
            {
                this.SelectedIndex = index;
                TabPage page = base.Tabs[this.SelectedIndex];
                base.Parent.Text = page.Controls[0].Text;
            }
        }
    }
}


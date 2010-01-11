namespace Microsoft.Matrix.Packages.Community.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class CommunityToolWindow : ToolWindow
    {
        private CommunityTab _activeTab;
        private ImageList _buttonImages;
        private PageViewer _communityView;
        private MxToolBarButton _refreshButton;
        private ArrayList _tabs;
        private Timer _timer;
        private MxToolBar _toolBar;

        public CommunityToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeUserInterface();
            Bitmap bitmap = new Bitmap(typeof(CommunityToolWindow), "Refresh.bmp");
            bitmap.MakeTransparent(Color.Fuchsia);
            this._buttonImages.Images.Add(bitmap);
            this._refreshButton = new MxToolBarButton();
            this._refreshButton.ToolTipText = "Refresh";
            this._refreshButton.Enabled = false;
            this._refreshButton.ImageIndex = 0;
            MxToolBarButton button = new MxToolBarButton();
            button.Style = ToolBarButtonStyle.Separator;
            this._toolBar.Buttons.Add(this._refreshButton);
            this._toolBar.Buttons.Add(button);
            this._tabs = new ArrayList();
            ArrayList config = (ArrayList) ConfigurationSettings.GetConfig("microsoft.matrix/community");
            if (config != null)
            {
                for (int i = 0; i < config.Count; i += 2)
                {
                    string typeName = (string) config[i];
                    string initializationData = (string) config[i + 1];
                    try
                    {
                        CommunityTab tab = (CommunityTab) Activator.CreateInstance(System.Type.GetType(typeName, true, false));
                        tab.Initialize(serviceProvider, initializationData);
                        this.AddTab(tab);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void AddTab(CommunityTab tab)
        {
            int count = this._tabs.Count;
            this._tabs.Add(tab);
            this._buttonImages.Images.Add(tab.Glyph);
            ToolBarButton button = new ToolBarButton();
            button.ImageIndex = count + 1;
            button.Tag = count;
            button.ToolTipText = tab.Name;
            this._toolBar.Buttons.Add(button);
            if (count == 0)
            {
                button.Pushed = true;
                this._refreshButton.Enabled = tab.SupportsRefresh;
                this._activeTab = tab;
                if (base.IsHandleCreated)
                {
                    this.UpdateDisplay(true);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._timer.Enabled = false;
                if (this._activeTab != null)
                {
                    this._activeTab.DisplayChanged -= new EventHandler(this.OnTabDisplayChanged);
                    this._activeTab = null;
                }
                foreach (CommunityTab tab in this._tabs)
                {
                    ((IDisposable) tab).Dispose();
                }
                this._tabs = null;
            }
            base.Dispose(disposing);
        }

        private void InitializeUserInterface()
        {
            Panel panel = new Panel();
            this._timer = new Timer();
            this._timer.Enabled = false;
            this._timer.Tick += new EventHandler(this.OnTimerTick);
            this._buttonImages = new ImageList();
            this._buttonImages.ColorDepth = ColorDepth.Depth32Bit;
            this._buttonImages.ImageSize = new Size(0x10, 0x10);
            this._toolBar = new MxToolBar();
            this._toolBar.Appearance = ToolBarAppearance.Flat;
            this._toolBar.Divider = false;
            this._toolBar.DropDownArrows = false;
            this._toolBar.ShowToolTips = true;
            this._toolBar.TabIndex = 0;
            this._toolBar.TextAlign = ToolBarTextAlign.Right;
            this._toolBar.Wrappable = false;
            this._toolBar.ImageList = this._buttonImages;
            this._toolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.OnToolBarButtonClick);
            this._communityView = new PageViewer();
            this._communityView.Dock = DockStyle.Fill;
            this._communityView.TabIndex = 0;
            panel.SuspendLayout();
            base.SuspendLayout();
            panel.BackColor = SystemColors.ControlDark;
            panel.Dock = DockStyle.Fill;
            panel.DockPadding.All = 1;
            panel.TabIndex = 1;
            panel.Controls.Add(this._communityView);
            base.Controls.AddRange(new Control[] { panel, this._toolBar });
            this.Text = "Community";
            base.Icon = new Icon(typeof(CommunityToolWindow), "CommunityToolWindow.ico");
            panel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.UpdateDisplay(true);
            if (this._activeTab != null)
            {
                this._activeTab.DisplayChanged += new EventHandler(this.OnTabDisplayChanged);
            }
        }

        public object OnLinkClick(object[] args)
        {
            return null;
        }

        private void OnTabDisplayChanged(object sender, EventArgs e)
        {
            this.UpdateDisplay(true);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            this.UpdateDisplay(true);
        }

        private void OnToolBarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == this._refreshButton)
            {
                this.UpdateDisplay(true);
            }
            else
            {
                int tag = (int) e.Button.Tag;
                CommunityTab tab = (CommunityTab) this._tabs[tag];
                if (tab != this._activeTab)
                {
                    for (int i = 0; i < this._toolBar.Buttons.Count; i++)
                    {
                        this._toolBar.Buttons[i].Pushed = false;
                    }
                    e.Button.Pushed = true;
                    if (this._activeTab != null)
                    {
                        this._activeTab.DisplayChanged -= new EventHandler(this.OnTabDisplayChanged);
                    }
                    this._activeTab = tab;
                    this.UpdateDisplay(false);
                    this._activeTab.DisplayChanged += new EventHandler(this.OnTabDisplayChanged);
                }
            }
        }

        private void UpdateDisplay(bool recreate)
        {
            Page page = null;
            if (this._activeTab != null)
            {
                page = this._activeTab.GetPage(recreate);
            }
            this._communityView.Page = page;
            this.UpdateRefresh();
        }

        private void UpdateRefresh()
        {
            bool flag = (this._activeTab != null) && this._activeTab.SupportsRefresh;
            if (flag && (this._activeTab.RefreshRate != 0))
            {
                this._timer.Interval = this._activeTab.RefreshRate;
                this._timer.Enabled = true;
            }
            else
            {
                this._timer.Enabled = false;
            }
            this._refreshButton.Enabled = flag;
        }
    }
}


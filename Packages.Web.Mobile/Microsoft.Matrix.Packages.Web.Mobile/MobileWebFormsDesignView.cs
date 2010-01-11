namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms;

    public class MobileWebFormsDesignView : WebFormsDesignView, IDeviceSpecificSelectionProvider
    {
        private const string _allDeviceEditingToolTip = "All Device Mode";
        private ToolBarButton _allDeviceModeToolBarButton;
        private object[] _baseFilterList;
        private string _currentFilter;
        private ToolBarButton _customizationModeToolBarButton;
        private const string _customizationToolTip = "Customization Mode";
        private const string _defaultFilterName = "(Default)";
        private static readonly int _docHeight = 30;
        private MxToolBar _editingModeToolBar;
        private Panel _editorContainer;
        private MxComboBox _filterComboBox;
        private ArrayList _filterList;
        private ImageList _imageList;
        private EventHandler _selectionChangedHandler;
        private Panel _selectionContainer;

        event EventHandler IDeviceSpecificSelectionProvider.SelectionChanged
        {
            add
            {
                this._selectionChangedHandler = (EventHandler) Delegate.Combine(this._selectionChangedHandler, value);
            }
            remove
            {
                if (this._selectionChangedHandler != null)
                {
                    this._selectionChangedHandler = (EventHandler) Delegate.Remove(this._selectionChangedHandler, value);
                }
            }
        }

        public MobileWebFormsDesignView(IServiceProvider serviceProvider) : base(serviceProvider, WebFormsEditorMode.UserControl)
        {
            this._editorContainer = null;
            this._selectionContainer = null;
        }

        private BooleanOption GetNegatedBooleanOption(BooleanOption oldValue)
        {
            switch (oldValue)
            {
                case -1:
                    return 1;

                case 0:
                    return 1;

                case 1:
                    return 0;
            }
            return -1;
        }

        protected override void InitializeUserInterface()
        {
            base.InitializeUserInterface();
            this._selectionContainer = new Panel();
            this._selectionContainer.Dock = DockStyle.Top;
            this._selectionContainer.Height = _docHeight;
            this._selectionContainer.TabIndex = 1;
            this._selectionContainer.BackColor = SystemColors.Control;
            this._selectionContainer.SuspendLayout();
            base.SuspendLayout();
            base.Editor.AbsolutePositioningEnabled = false;
            base.Editor.BordersVisible = false;
            base.Editor.GlyphsVisible = false;
            base.Editor.GridVisible = false;
            this._imageList = new ImageList();
            this._filterList = new ArrayList();
            this._baseFilterList = new object[] { 
                "isHTML32", "isWML11", "isCHTML10", "isGoAmerica", "isMME", "isMyPalm", "isPocketIE", "isUP3X", "isUP4X", "isEriccsonR380", "isNokia7110", "prefersGIF", "prefersWBMP", "supportsColor", "supportsCookies", "supportsJavaScript", 
                "supportsVoiceCalls", "(Default)"
             };
            this._filterList.AddRange(this._baseFilterList);
            Bitmap bitmap = new Bitmap(typeof(MobileWebFormsDesignView), "Customization.bmp");
            Bitmap bitmap2 = new Bitmap(typeof(MobileWebFormsDesignView), "AllDevice.bmp");
            bitmap.MakeTransparent();
            bitmap2.MakeTransparent();
            this._imageList.ImageSize = new Size(0x10, 0x10);
            this._imageList.Images.Add(bitmap2);
            this._imageList.Images.Add(bitmap);
            this._allDeviceModeToolBarButton = new ToolBarButton();
            this._allDeviceModeToolBarButton.Text = "All Device Mode";
            this._allDeviceModeToolBarButton.ImageIndex = 0;
            this._allDeviceModeToolBarButton.Pushed = true;
            this._allDeviceModeToolBarButton.Style = ToolBarButtonStyle.ToggleButton;
            this._allDeviceModeToolBarButton.ToolTipText = "All Device Mode";
            this._customizationModeToolBarButton = new ToolBarButton();
            this._customizationModeToolBarButton.Text = "Customization Mode";
            this._customizationModeToolBarButton.ImageIndex = 1;
            this._customizationModeToolBarButton.Pushed = false;
            this._customizationModeToolBarButton.Style = ToolBarButtonStyle.ToggleButton;
            this._customizationModeToolBarButton.ToolTipText = "Customization Mode";
            this._editingModeToolBar = new MxToolBar();
            this._editingModeToolBar.Appearance = ToolBarAppearance.Flat;
            this._editingModeToolBar.Divider = false;
            this._editingModeToolBar.Dock = DockStyle.None;
            this._editingModeToolBar.DropDownArrows = true;
            this._editingModeToolBar.ImageList = this._imageList;
            this._editingModeToolBar.Location = new Point(4, 4);
            this._editingModeToolBar.ShowToolTips = true;
            this._editingModeToolBar.Size = new Size(240, 0x19);
            this._editingModeToolBar.TabIndex = 2;
            this._editingModeToolBar.TextAlign = ToolBarTextAlign.Right;
            this._editingModeToolBar.Wrappable = false;
            this._editingModeToolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.OnToolBarButtonClick);
            this._editingModeToolBar.Buttons.AddRange(new ToolBarButton[] { this._allDeviceModeToolBarButton, this._customizationModeToolBarButton });
            this._filterComboBox = new MxComboBox();
            this._filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._filterComboBox.Enabled = false;
            this._filterComboBox.FlatAppearance = true;
            this._filterComboBox.Items.AddRange(this._filterList.ToArray());
            this._filterComboBox.Location = new Point(250, 5);
            this._filterComboBox.Size = new Size(160, 0x18);
            this._filterComboBox.SelectedIndex = 0;
            this._filterComboBox.SelectedIndexChanged += new EventHandler(this.OnModeChanged);
            this._filterComboBox.TabIndex = 3;
            this._editorContainer = new Panel();
            this._editorContainer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._editorContainer.Height = base.Height - _docHeight;
            this._editorContainer.Location = new Point(0, _docHeight);
            this._editorContainer.SuspendLayout();
            this._editorContainer.Width = base.Width;
            this._selectionContainer.Controls.AddRange(new Control[] { this._editingModeToolBar, this._filterComboBox });
            this._editorContainer.Controls.Add(base.Editor);
            base.Controls.Add(this._editorContainer);
            base.Controls.Add(this._selectionContainer);
            this._editorContainer.ResumeLayout(false);
            this._selectionContainer.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        void IDeviceSpecificSelectionProvider.AddFilter(string filterName)
        {
            if (!((IDeviceSpecificSelectionProvider) this).FilterExists(filterName))
            {
                this._filterList.Insert(0, filterName);
                this._filterComboBox.Items.Insert(0, filterName);
            }
        }

        bool IDeviceSpecificSelectionProvider.FilterExists(string filterName)
        {
            return this._filterList.Contains(filterName);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            IServiceContainer service = (IServiceContainer) base.ServiceProvider.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                service.AddService(typeof(IDeviceSpecificSelectionProvider), this);
            }
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            this._filterComboBox.Items.Clear();
            this._filterComboBox.Items.AddRange(this._baseFilterList);
            this._filterComboBox.SelectedIndex = 0;
            this._filterList.Clear();
            this._filterList.AddRange(this._baseFilterList);
            IServiceContainer service = (IServiceContainer) base.ServiceProvider.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                service.RemoveService(typeof(IDeviceSpecificSelectionProvider));
            }
        }

        private void OnModeChanged(object sender, EventArgs e)
        {
            this._filterComboBox.Enabled = this._customizationModeToolBarButton.Pushed;
            if (this._allDeviceModeToolBarButton.Pushed)
            {
                this._currentFilter = null;
            }
            else if (this._filterComboBox.SelectedIndex != -1)
            {
                this._currentFilter = (string) this._filterComboBox.Items[this._filterComboBox.SelectedIndex];
                if ("(Default)".Equals(this._currentFilter))
                {
                    this._currentFilter = string.Empty;
                }
            }
            else
            {
                this._currentFilter = null;
            }
            if (this._selectionChangedHandler != null)
            {
                bool isDirty = this.IsDirty;
                this._selectionChangedHandler(this, e);
                if (!isDirty)
                {
                    base.Editor.ClearDirtyState();
                }
            }
        }

        protected override void OnTemplateModeChanged(EventArgs e)
        {
            if (!base.InTemplateMode)
            {
                this.OnModeChanged(this, e);
            }
        }

        private void OnToolBarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            bool flag = e.Button == this._allDeviceModeToolBarButton;
            this._allDeviceModeToolBarButton.Pushed = flag;
            this._customizationModeToolBarButton.Pushed = !flag;
            this.OnModeChanged(sender, e);
        }

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            if (base.InTemplateMode)
            {
                if (!base.SupportsToolboxSection(section))
                {
                    return (section is MobileWebFormsToolboxSection);
                }
                return true;
            }
            Type type = section.GetType();
            if ((type != typeof(MobileWebFormsToolboxSection)) && (type != typeof(SnippetToolboxSection)))
            {
                return (type == typeof(CustomControlsToolboxSection));
            }
            return true;
        }

        public override ToolboxSection DefaultToolboxSection
        {
            get
            {
                return MobileWebFormsToolboxSection.MobileWebForms;
            }
        }

        string IDeviceSpecificSelectionProvider.CurrentFilter
        {
            get
            {
                return this._currentFilter;
            }
        }

        bool IDeviceSpecificSelectionProvider.DeviceSpecificSelectionProviderEnabled
        {
            get
            {
                return !base.InTemplateMode;
            }
        }
    }
}


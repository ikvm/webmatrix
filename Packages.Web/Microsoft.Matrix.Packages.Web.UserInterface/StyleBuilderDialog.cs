namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html;
    using Microsoft.Matrix.Packages.Web.Html.Css;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    internal sealed class StyleBuilderDialog : MxForm
    {
        private IList _attributeList;
        private IDictionary _attributes;
        private string _baseUrl;
        private MxButton _cancelButton;
        private string _cssText;
        private bool _cssTextMode;
        private StyleEditingGroup _currentGroup;
        private StyleEditingGroup[] _editingGroups;
        private GroupViewListView _groupList;
        private Panel _groupListHolder;
        private MxLabel _instructionLabel;
        private MxButton _okButton;
        private Interop.IHTMLStyle _parseStyle;
        private HtmlControl _preview;
        private Panel _previewHolder;
        private MxLabel _previewLabel;
        private PropertyGrid _propGrid;
        private IStyle _style;
        private const string PreviewHtml = "<html>\r\n              <body style=\"border: none; margin: 0; padding: 0; overflow: hidden\">\r\n                <div id=\"divPreview\" style=\"height: 100%; width: 100%; overflow: auto; padding: 1px\"></div>\r\n                <div style=\"display:none\"><span id=\"spanParse\"></span></div>\r\n              </body>\r\n              </html>";

        private StyleBuilderDialog(IServiceProvider serviceProvider, string baseUrl, string selection) : base(serviceProvider)
        {
            this.InitializeComponent();
            if ((selection != null) && (selection.Length != 0))
            {
                this.Text = this.Text + " - " + selection;
            }
            this._propGrid.Site = new PropertyGridSite(serviceProvider);
            ImageList list = new ImageList();
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.TransparentColor = Color.Red;
            list.Images.AddStrip(new Bitmap(typeof(StyleBuilderDialog), "StyleBuilderGroups.bmp"));
            this._editingGroups = new StyleEditingGroup[] { new FontStyleEditingGroup(this, new FontCssAttributes(), list.Images[0]), new BackgroundStyleEditingGroup(this, new BackgroundCssAttributes(), list.Images[1]) };
            this._attributes = new Hashtable();
            this._attributeList = new ArrayList();
            foreach (StyleEditingGroup group in this._editingGroups)
            {
                foreach (CssAttribute attribute in group.CssAttributes.Attributes)
                {
                    this._attributes[attribute.Name] = attribute;
                    this._attributeList.Add(attribute);
                }
            }
            this._baseUrl = baseUrl;
        }

        public StyleBuilderDialog(IServiceProvider serviceProvider, string baseUrl, string selection, IStyle style) : this(serviceProvider, baseUrl, selection)
        {
            this._style = style;
        }

        public StyleBuilderDialog(IServiceProvider serviceProvider, string baseUrl, string selection, string cssText) : this(serviceProvider, baseUrl, selection)
        {
            if (cssText == null)
            {
                this._cssText = string.Empty;
            }
            else
            {
                this._cssText = cssText;
            }
            this._cssTextMode = true;
        }

        private void AbortDialog()
        {
            base.DialogResult = DialogResult.Abort;
            base.Close();
        }

        private void InitializeComponent()
        {
            this._groupListHolder = new Panel();
            this._groupList = new GroupViewListView();
            this._propGrid = new PropertyGrid();
            this._previewHolder = new Panel();
            this._preview = new HtmlControl(base.ServiceProvider);
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._previewLabel = new MxLabel();
            this._instructionLabel = new MxLabel();
            this._groupListHolder.SuspendLayout();
            base.SuspendLayout();
            this._groupListHolder.BackColor = SystemColors.ControlDark;
            this._groupListHolder.Controls.AddRange(new Control[] { this._groupList });
            this._groupListHolder.DockPadding.All = 1;
            this._groupListHolder.Location = new Point(8, 30);
            this._groupListHolder.Name = "_groupListHolder";
            this._groupListHolder.Size = new Size(0x70, 0xf8);
            this._groupListHolder.TabIndex = 1;
            this._groupList.BackColor = SystemColors.Window;
            this._groupList.Dock = DockStyle.Fill;
            this._groupList.Location = new Point(1, 1);
            this._groupList.FullRowSelect = true;
            this._groupList.Name = "_groupList";
            this._groupList.Size = new Size(110, 0xf6);
            this._groupList.TabIndex = 0;
            this._groupList.SelectedIndexChanged += new EventHandler(this.OnGroupListSelectedIndexChanged);
            this._propGrid.CommandsVisibleIfAvailable = false;
            this._propGrid.LargeButtons = false;
            this._propGrid.LineColor = SystemColors.Control;
            this._propGrid.Location = new Point(0x7c, 30);
            this._propGrid.Name = "_propGrid";
            this._propGrid.PropertySort = PropertySort.Categorized;
            this._propGrid.Size = new Size(0x164, 0xf8);
            this._propGrid.TabIndex = 2;
            this._propGrid.ToolbarVisible = false;
            this._propGrid.BackColor = SystemColors.Control;
            this._propGrid.ViewBackColor = SystemColors.Window;
            this._propGrid.ViewForeColor = SystemColors.WindowText;
            this._propGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.OnPropGridPropertyValueChanged);
            this._previewHolder.BackColor = SystemColors.ControlDark;
            this._previewHolder.Location = new Point(8, 310);
            this._previewHolder.Controls.AddRange(new Control[] { this._preview });
            this._previewHolder.DockPadding.All = 1;
            this._previewHolder.Name = "_previewHolder";
            this._previewHolder.Size = new Size(0x1d8, 0x40);
            this._previewHolder.TabIndex = 4;
            this._previewHolder.TabStop = false;
            this._preview.Location = new Point(1, 1);
            this._preview.Name = "_preview";
            this._preview.Size = new Size(470, 0x3e);
            this._preview.ScrollBarsEnabled = false;
            this._preview.TabIndex = 0;
            this._preview.TabStop = false;
            this._okButton.Location = new Point(0x144, 0x180);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 5;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnOKButtonClicked);
            this._cancelButton.Location = new Point(0x194, 0x180);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 6;
            this._cancelButton.Text = "Cancel";
            this._previewLabel.Location = new Point(8, 0x124);
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.Size = new Size(100, 0x10);
            this._previewLabel.TabIndex = 3;
            this._previewLabel.Text = "Preview:";
            this._instructionLabel.Location = new Point(8, 12);
            this._instructionLabel.Name = "_instructionLabel";
            this._instructionLabel.Size = new Size(240, 0x10);
            this._instructionLabel.TabIndex = 0;
            this._instructionLabel.Text = "Edit CSS style attributes:";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(490, 0x19d);
            base.Controls.AddRange(new Control[] { this._instructionLabel, this._previewLabel, this._cancelButton, this._okButton, this._previewHolder, this._groupListHolder, this._propGrid });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "StyleBuilder";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "StyleBuilder";
            this._groupListHolder.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnGroupListSelectedIndexChanged(object sender, EventArgs e)
        {
            CssAttributes selectedObject = this._propGrid.SelectedObject as CssAttributes;
            GroupViewListViewItem item = null;
            if (this._groupList.SelectedItems.Count != 0)
            {
                item = (GroupViewListViewItem) this._groupList.SelectedItems[0];
            }
            else
            {
                item = (GroupViewListViewItem) this._groupList.Items[0];
            }
            StyleEditingGroup groupViewItem = (StyleEditingGroup) item.GroupViewItem;
            CssAttributes cssAttributes = groupViewItem.CssAttributes;
            if (selectedObject != cssAttributes)
            {
                this._currentGroup = groupViewItem;
                this._propGrid.SelectedObject = cssAttributes;
                this.UpdatePreview();
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this._preview.LoadHtml("<html>\r\n              <body style=\"border: none; margin: 0; padding: 0; overflow: hidden\">\r\n                <div id=\"divPreview\" style=\"height: 100%; width: 100%; overflow: auto; padding: 1px\"></div>\r\n                <div style=\"display:none\"><span id=\"spanParse\"></span></div>\r\n              </body>\r\n              </html>", this._baseUrl);
            Application.DoEvents();
            if (this._cssTextMode)
            {
                try
                {
                    Element elementByID = this._preview.GetElementByID("spanParse");
                    if (elementByID != null)
                    {
                        this._parseStyle = elementByID.Peer.GetStyle();
                    }
                    if (this._cssText.Length != 0)
                    {
                        this._parseStyle.SetCssText(this._cssText);
                    }
                    this._style = new InlineStyle(this._parseStyle);
                }
                catch (Exception)
                {
                }
                if (this._style == null)
                {
                    ((IMxUIService) this.GetService(typeof(IMxUIService))).ReportError("Unable to process the initial style string.", "StyleBuilder", false);
                    base.BeginInvoke(new MethodInvoker(this.AbortDialog));
                    return;
                }
            }
            IStyle[] styles = new IStyle[] { this._style };
            foreach (CssAttribute attribute in this._attributeList)
            {
                attribute.Load(styles);
            }
            this._groupList.Items.AddRange(new ListViewItem[] { new GroupViewListViewItem(this._editingGroups[0]), new GroupViewListViewItem(this._editingGroups[1]) });
            this._groupList.Items[0].Selected = true;
        }

        private void OnOKButtonClicked(object sender, EventArgs e)
        {
            IStyle[] styles = new IStyle[] { this._style };
            foreach (CssAttribute attribute in this._attributeList)
            {
                if (attribute.IsDirty)
                {
                    attribute.Save(styles);
                }
            }
            if (this._cssTextMode)
            {
                try
                {
                    this._cssText = this._parseStyle.GetCssText();
                }
                catch (Exception)
                {
                }
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void OnPropGridPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            this.UpdatePreview();
        }

        private void UpdatePreview()
        {
            try
            {
                string p = string.Empty;
                if (this._currentGroup != null)
                {
                    p = this._currentGroup.GetPreviewContent();
                }
                this._preview.GetElementByID("divPreview").Peer.SetInnerHTML(p);
            }
            catch (Exception)
            {
            }
        }

        public string CssText
        {
            get
            {
                return this._cssText;
            }
        }

        private sealed class BackgroundStyleEditingGroup : StyleBuilderDialog.StyleEditingGroup
        {
            private const string PreviewContentTemplate = "<table width=100% height=100% border=0 cellspacing=0 cellpadding=0>\r\n                  <tr><td align=center style=\"{0}\"><br>Sample Text<br><br><br><br><br><br></td></tr>\r\n                  </table>";

            public BackgroundStyleEditingGroup(StyleBuilderDialog owner, CssAttributes cssAttributes, Image image) : base(owner, cssAttributes, "Background", image)
            {
            }

            public override string GetPreviewContent()
            {
                string[] attributeNames = new string[] { "font-family", "font-size", "font-weight", "font-style", "color", "text-decoration", "font-variant", "background-color", "background-image", "background-repeat", "background-scroll", "background-position-x", "background-position-y" };
                string str = base.CreateStyleString(attributeNames);
                return string.Format("<table width=100% height=100% border=0 cellspacing=0 cellpadding=0>\r\n                  <tr><td align=center style=\"{0}\"><br>Sample Text<br><br><br><br><br><br></td></tr>\r\n                  </table>", str);
            }
        }

        private sealed class FontStyleEditingGroup : StyleBuilderDialog.StyleEditingGroup
        {
            private const string PreviewContentTemplate = "<table width=100% height=100% border=0 cellspacing=0 cellpadding=0>\r\n                  <tr><td align=center valign=middle style=\"{0}\">Sample Text</td></tr>\r\n                  </table>";

            public FontStyleEditingGroup(StyleBuilderDialog owner, CssAttributes cssAttributes, Image image) : base(owner, cssAttributes, "Font", image)
            {
            }

            public override string GetPreviewContent()
            {
                string[] attributeNames = new string[] { "font-family", "font-size", "font-weight", "font-style", "color", "text-decoration", "font-variant", "background-color", "background-image", "background-repeat", "background-scroll", "background-position-x", "background-position-y" };
                string str = base.CreateStyleString(attributeNames);
                return string.Format("<table width=100% height=100% border=0 cellspacing=0 cellpadding=0>\r\n                  <tr><td align=center valign=middle style=\"{0}\">Sample Text</td></tr>\r\n                  </table>", str);
            }
        }

        private sealed class PropertyGridSite : ISite, IServiceProvider
        {
            private IServiceProvider _serviceProvider;

            public PropertyGridSite(IServiceProvider serviceProvider)
            {
                this._serviceProvider = serviceProvider;
            }

            object IServiceProvider.GetService(Type type)
            {
                return this._serviceProvider.GetService(type);
            }

            IComponent ISite.Component
            {
                get
                {
                    return null;
                }
            }

            IContainer ISite.Container
            {
                get
                {
                    return null;
                }
            }

            bool ISite.DesignMode
            {
                get
                {
                    return true;
                }
            }

            string ISite.Name
            {
                get
                {
                    return "PropertyGrid";
                }
                set
                {
                }
            }
        }

        private abstract class StyleEditingGroup : GroupViewItem
        {
            private Microsoft.Matrix.Packages.Web.Html.Css.CssAttributes _cssAttributes;
            private StyleBuilderDialog _owner;

            protected StyleEditingGroup(StyleBuilderDialog owner, Microsoft.Matrix.Packages.Web.Html.Css.CssAttributes cssAttributes, string groupName, Image image) : base(groupName, image)
            {
                this._owner = owner;
                this._cssAttributes = cssAttributes;
            }

            protected string CreateStyleString(string[] attributeNames)
            {
                StringBuilder builder = new StringBuilder(0x100);
                IDictionary dictionary = this._owner._attributes;
                foreach (string str in attributeNames)
                {
                    string str2 = ((CssAttribute) dictionary[str]).Value;
                    if ((str2 != null) && (str2.Length != 0))
                    {
                        builder.Append(str);
                        builder.Append(": ");
                        builder.Append(str2);
                        builder.Append("; ");
                    }
                }
                return builder.ToString();
            }

            public virtual string GetPreviewContent()
            {
                return string.Empty;
            }

            public Microsoft.Matrix.Packages.Web.Html.Css.CssAttributes CssAttributes
            {
                get
                {
                    return this._cssAttributes;
                }
            }
        }
    }
}


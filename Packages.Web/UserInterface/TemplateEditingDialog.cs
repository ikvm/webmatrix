namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web.Html.WebForms;
    using Microsoft.Matrix.Packages.Web.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    internal sealed class TemplateEditingDialog : MxForm
    {
        private string _activeTemplateName;
        private MxButton _cancelButton;
        private TemplatedControlDesigner _designer;
        private TemplateDesignView _designView;
        private System.Windows.Forms.Panel _editPanel;
        private MxLabel _editTemplateLabel;
        private TemplateEditingFrame[] _frames;
        private bool _initialized;
        private bool _isDirty;
        private IDictionary _layers;
        private MxButton _okButton;
        private WebFormsEditor _parentEditor;
        private int _savedGroupIndex;
        private int _savedTemplateIndex;
        private MxComboBox _templateCombo;
        private TemplateEditingServiceProvider _templateEditingServiceProvider;
        private MxComboBox _templateGroupCombo;
        private MxLabel _templateGroupLabel;

        public TemplateEditingDialog(TemplatedControlDesigner designer, WebFormsEditor parentEditor, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._savedGroupIndex = -1;
            this._savedTemplateIndex = -1;
            if (designer == null)
            {
                throw new ArgumentNullException("designer");
            }
            this._designer = designer;
            this._parentEditor = parentEditor;
            this._templateEditingServiceProvider = new TemplateEditingServiceProvider(serviceProvider);
            this.InitializeComponent();
            this.Text = "Edit " + this._designer.Component.Site.Name + " Templates";
        }

        private TemplateDesignView CreateTemplateDesignView()
        {
            return new TemplateDesignView(this._templateEditingServiceProvider, (System.Web.UI.Control)this._designer.Component, this);
        }

        private string GenerateLayerKey(string templateGroup, string templateName)
        {
            return (templateGroup + "." + templateName);
        }

        private void InitializeComponent()
        {
            this._editPanel = new System.Windows.Forms.Panel();
            this._designView = this.CreateTemplateDesignView();
            this._templateGroupLabel = new MxLabel();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._templateGroupCombo = new MxComboBox();
            this._templateCombo = new MxComboBox();
            this._editTemplateLabel = new MxLabel();
            this._editPanel.SuspendLayout();
            base.SuspendLayout();
            this._designView.Dock = DockStyle.Fill;
            this._editPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._editPanel.BackColor = SystemColors.ControlDark;
            this._editPanel.Controls.AddRange(new System.Windows.Forms.Control[] { this._designView });
            this._editPanel.DockPadding.All = 1;
            this._editPanel.Location = new Point(12, 0x4c);
            this._editPanel.Name = "_editPanel";
            this._editPanel.Size = new Size(0x194, 0x80);
            this._editPanel.TabIndex = 5;
            this._templateGroupLabel.Location = new Point(12, 8);
            this._templateGroupLabel.Name = "_templateGroupLabel";
            this._templateGroupLabel.Size = new Size(0x98, 13);
            this._templateGroupLabel.TabIndex = 0;
            this._templateGroupLabel.Text = "Select a Template to edit:";
            this._okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._okButton.Location = new Point(260, 0xd8);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 6;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnClickedOKButton);
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(340, 0xd8);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 7;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._templateGroupCombo.AlwaysShowFocusCues = true;
            this._templateGroupCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._templateGroupCombo.DropDownWidth = 0x79;
            this._templateGroupCombo.FlatAppearance = true;
            this._templateGroupCombo.InitialText = null;
            this._templateGroupCombo.Location = new Point(12, 0x18);
            this._templateGroupCombo.Name = "_templateGroupCombo";
            this._templateGroupCombo.Size = new Size(0xc0, 0x15);
            this._templateGroupCombo.TabIndex = 1;
            this._templateGroupCombo.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedTemplateGroupCombo);
            this._templateCombo.AlwaysShowFocusCues = true;
            this._templateCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._templateCombo.DropDownWidth = 0x79;
            this._templateCombo.FlatAppearance = true;
            this._templateCombo.InitialText = null;
            this._templateCombo.Location = new Point(220, 0x18);
            this._templateCombo.Name = "_templateCombo";
            this._templateCombo.Size = new Size(0xc0, 0x15);
            this._templateCombo.TabIndex = 3;
            this._templateCombo.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedTemplateCombo);
            this._editTemplateLabel.Location = new Point(12, 0x38);
            this._editTemplateLabel.Name = "_editTemplateLabel";
            this._editTemplateLabel.Size = new Size(0x74, 13);
            this._editTemplateLabel.TabIndex = 4;
            this._editTemplateLabel.Text = "Template Design:";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.ClientSize = new Size(0x1a8, 0xf9);
            base.ControlBox = false;
            base.Controls.AddRange(new System.Windows.Forms.Control[] { this._editPanel, this._cancelButton, this._okButton, this._editTemplateLabel, this._templateGroupLabel, this._templateCombo, this._templateGroupCombo });
            base.Icon = null;
            base.MinimumSize = new Size(430, 250);
            base.Name = "TemplateEditingDialog";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this._editPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadCurrentTemplate()
        {
            int selectedIndex = this._templateCombo.SelectedIndex;
            int index = this._templateGroupCombo.SelectedIndex;
            if ((index != -1) && (selectedIndex != -1))
            {
                TemplateEditingFrame editingFrame = this._frames[index];
                string templateName = editingFrame.TemplateNames[selectedIndex];
                this._activeTemplateName = templateName;
                string content = (string) editingFrame.ChangeTable[templateName];
                if (content == null)
                {
                    bool allowEditing = false;
                    content = this._designer.GetTemplateContent(editingFrame, templateName, out allowEditing);
                }
                IDocumentDesignerHost service = (IDocumentDesignerHost) base.ServiceProvider.GetService(typeof(IDocumentDesignerHost));
                ILayeredDesignerHost host2 = (ILayeredDesignerHost) base.ServiceProvider.GetService(typeof(ILayeredDesignerHost));
                string str3 = this.GenerateLayerKey(this._templateGroupCombo.Text, this._templateCombo.Text);
                host2.ActiveLayer = (IDesignLayer) this.Layers[str3];
                service.BeginLoad();
                try
                {
                    Style style = new Style();
                    style.CopyFrom(editingFrame.ControlStyle);
                    if (editingFrame.TemplateStyles != null)
                    {
                        style.CopyFrom(editingFrame.TemplateStyles[selectedIndex]);
                    }
                    string str4 = string.Format("<style>body {{ {0} }}</style>", this.StyleToCss(style));
                    string url = this._parentEditor.Url;
                    if (content != null)
                    {
                        this._designView.Editor.LoadHtml(content, url, str4);
                    }
                    else
                    {
                        this._designView.Editor.LoadHtml(string.Empty, url, str4);
                    }
                }
                finally
                {
                    service.EndLoad();
                }
            }
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void OnClickedOKButton(object sender, EventArgs e)
        {
            if (this.SaveTemplates())
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                this.RemoveLayers();
                foreach (TemplateEditingFrame frame in this._frames)
                {
                    frame.ChangeTable.Clear();
                }
                ((IDocumentView) this._designView).Deactivate(true);
            }
        }

        private void OnSelectedIndexChangedTemplateCombo(object sender, EventArgs e)
        {
            this.UpdateChangeTable();
            this.LoadCurrentTemplate();
        }

        private void OnSelectedIndexChangedTemplateGroupCombo(object sender, EventArgs e)
        {
            this._templateCombo.Items.Clear();
            int selectedIndex = this._templateGroupCombo.SelectedIndex;
            if (selectedIndex != -1)
            {
                this._templateCombo.Items.AddRange(this._frames[selectedIndex].TemplateNames);
                this._templateCombo.SelectedIndex = 0;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible && !this._initialized)
            {
                ((IDocumentView) this._designView).Activate(false);
                foreach (string str in this._parentEditor.NamespaceManager.NamespaceList)
                {
                    this._designView.RegisterNamespace(str);
                }
                TemplateEditingVerb[] templateEditingVerbs = this._designer.GetTemplateEditingVerbs();
                int length = templateEditingVerbs.Length;
                this._frames = new TemplateEditingFrame[length];
                ILayeredDesignerHost service = (ILayeredDesignerHost) base.ServiceProvider.GetService(typeof(ILayeredDesignerHost));
                for (int i = 0; i < length; i++)
                {
                    TemplateEditingVerb verb = templateEditingVerbs[i];
                    TemplateEditingFrame frame = (TemplateEditingFrame) this._designer.GetType().GetMethod("CreateTemplateEditingFrame", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this._designer, new object[] { verb });
                    if (frame != null)
                    {
                        frame.Verb = verb;
                        this._frames[i] = frame;
                        string item = frame.Name.Replace("&", string.Empty);
                        this._templateGroupCombo.Items.Add(item);
                        for (int j = 0; j < frame.TemplateNames.Length; j++)
                        {
                            string key = this.GenerateLayerKey(item, frame.TemplateNames[j]);
                            this.Layers.Add(key, service.AddLayer(this._templateEditingServiceProvider, key, this._designer, true));
                        }
                    }
                }
                if (length > 0)
                {
                    this._templateGroupCombo.SelectedIndex = 0;
                }
                this._initialized = true;
            }
        }

        private void RemoveLayers()
        {
            if (this.Layers.Count > 0)
            {
                ILayeredDesignerHost service = (ILayeredDesignerHost) base.ServiceProvider.GetService(typeof(ILayeredDesignerHost));
                if (service != null)
                {
                    foreach (IDesignLayer layer in this.Layers.Values)
                    {
                        service.RemoveLayer(layer);
                    }
                }
                this.Layers.Clear();
            }
        }

        internal bool SaveTemplates()
        {
            this.UpdateChangeTable();
            foreach (string str in ((WebFormsEditor) this._designView.Editor).NamespaceManager.NamespaceList)
            {
                this._parentEditor.RegisterNamespace(str);
            }
            bool flag = false;
            IDesignerHost service = base.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            ILayeredDesignerHost host2 = (ILayeredDesignerHost) base.ServiceProvider.GetService(typeof(ILayeredDesignerHost));
            IDesignLayer activeLayer = host2.ActiveLayer;
            host2.ActiveLayer = null;
            try
            {
                DesignerTransaction transaction = service.CreateTransaction();
                try
                {
                    foreach (TemplateEditingFrame frame in this._frames)
                    {
                        IDictionary changeTable = frame.ChangeTable;
                        if (changeTable.Count > 0)
                        {
                            flag = true;
                            IDictionaryEnumerator enumerator = changeTable.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                                this._designer.SetTemplateContent(frame, (string) current.Key, (string) current.Value);
                            }
                        }
                    }
                }
                finally
                {
                    if (flag)
                    {
                        try
                        {
                            this._designer.IsDirty = true;
                            ((Behavior) this._designer.Behavior).Element.GetOuterHTML();
                            ((IComponentChangeService) base.ServiceProvider.GetService(typeof(IComponentChangeService))).OnComponentChanged(this._designer.Component, null, null, null);
                            goto Label_01AA;
                        }
                        finally
                        {
                            if (transaction != null)
                            {
                                transaction.Commit();
                            }
                        }
                    }
                    if (transaction != null)
                    {
                        transaction.Cancel();
                    }
                Label_01AA:;
                }
            }
            finally
            {
                host2.ActiveLayer = activeLayer;
                this.LoadCurrentTemplate();
            }
            this._designView.ClearDirty();
            this._isDirty = false;
            return flag;
        }

        private string StyleToCss(Style style)
        {
            if (style == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            Color foreColor = style.ForeColor;
            if (!foreColor.IsEmpty)
            {
                builder.Append("color:");
                builder.Append(ColorTranslator.ToHtml(foreColor));
                builder.Append(";");
            }
            foreColor = style.BackColor;
            if (!foreColor.IsEmpty)
            {
                builder.Append("background-color:");
                builder.Append(ColorTranslator.ToHtml(foreColor));
                builder.Append(";");
            }
            FontInfo font = style.Font;
            string name = font.Name;
            if (name.Length != 0)
            {
                builder.Append("font-family:'");
                builder.Append(name);
                builder.Append("';");
            }
            if (font.Bold)
            {
                builder.Append("font-weight:bold;");
            }
            if (font.Italic)
            {
                builder.Append("font-style:italic;");
            }
            name = string.Empty;
            if (font.Underline)
            {
                name = name + "underline";
            }
            if (font.Strikeout)
            {
                name = name + " line-through";
            }
            if (font.Overline)
            {
                name = name + " overline";
            }
            if (name.Length != 0)
            {
                builder.Append("text-decoration:");
                builder.Append(name);
                builder.Append(';');
            }
            FontUnit size = font.Size;
            if (!size.IsEmpty)
            {
                builder.Append("font-size:");
                builder.Append(size.ToString());
            }
            return builder.ToString();
        }

        private void UpdateChangeTable()
        {
            if ((this._savedGroupIndex != -1) && (this._savedTemplateIndex != -1))
            {
                TemplateEditingFrame frame = this._frames[this._savedGroupIndex];
                string str = frame.TemplateNames[this._savedTemplateIndex];
                frame.ChangeTable[str] = this._designView.Editor.SaveHtml();
                this._isDirty = true;
            }
            this._savedGroupIndex = this._templateGroupCombo.SelectedIndex;
            this._savedTemplateIndex = this._templateCombo.SelectedIndex;
        }

        internal string ActiveTemplateName
        {
            get
            {
                return this._activeTemplateName;
            }
        }

        internal TemplateDesignView DesignView
        {
            get
            {
                return this._designView;
            }
        }

        internal bool IsDirty
        {
            get
            {
                return (this._isDirty | this._designView.IsDirty);
            }
        }

        private IDictionary Layers
        {
            get
            {
                if (this._layers == null)
                {
                    this._layers = new Hashtable();
                }
                return this._layers;
            }
        }

        private class TemplateEditingServiceProvider : IServiceProvider
        {
            private ServiceContainer _serviceContainer;

            public TemplateEditingServiceProvider(IServiceProvider baseProvider)
            {
                this._serviceContainer = new ServiceContainer(baseProvider);
            }

            object IServiceProvider.GetService(Type type)
            {
                if (type == typeof(IEventBindingService))
                {
                    return null;
                }
                return this._serviceContainer.GetService(type);
            }
        }
    }
}


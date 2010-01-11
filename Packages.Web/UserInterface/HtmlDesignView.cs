namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Packages.Web.Html;
    using Microsoft.Matrix.Packages.Web.Html.Css;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class HtmlDesignView : Panel, IDocumentView, ICommandHandler, IToolboxClient, ISelectionContainer, ISelectionOutlineProvider, IDesignView, IPropertyBrowserClient, ISearchableDocumentView
    {
        private ICommandManager _commandManager;
        private Microsoft.Matrix.Packages.Web.Html.DataObjectConverter _dataObjectConverter;
        private Microsoft.Matrix.Packages.Web.Documents.HtmlDocument _document;
        private HtmlEditor _editor;
        private bool _isDirty;
        private bool _selectionTrackingEnabled;
        private IServiceProvider _serviceProvider;
        private Timer _timer;
        private static readonly object DocumentChangedEvent = new object();
        private static Bitmap viewImage;

        event EventHandler IDocumentView.DocumentChanged
        {
            add
            {
                base.Events.AddHandler(DocumentChangedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(DocumentChangedEvent, value);
            }
        }

        public HtmlDesignView(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this._serviceProvider = serviceProvider;
            this._commandManager = (ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager));
        }

        internal void ClearDirty()
        {
            this._isDirty = false;
        }

        protected virtual Microsoft.Matrix.Packages.Web.Html.DataObjectConverter CreateDataObjectConverter()
        {
            return new Microsoft.Matrix.Packages.Web.Html.DataObjectConverter();
        }

        protected virtual HtmlEditor CreateEditor()
        {
            return new HtmlEditor(this._serviceProvider);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._commandManager = null;
                this._serviceProvider = null;
            }
            base.Dispose(disposing);
        }

        protected void EnableSelectionTracking(bool enable)
        {
            if (this._timer != null)
            {
                if (enable)
                {
                    this._timer.Start();
                }
                else
                {
                    this._timer.Stop();
                }
            }
            this._selectionTrackingEnabled = enable;
        }

        public void EnsureEditor()
        {
            if (this._editor == null)
            {
                this.InitializeUserInterface();
            }
        }

        protected virtual ICollection GetOutline(object selectedObject)
        {
            return this.Editor.Selection.GetParentHierarchy(selectedObject);
        }

        protected virtual bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(WebCommands))
            {
                ComboBoxToolBarButton commandUI;
                switch (command.CommandID)
                {
                    case 1:
                        this._editor.GlyphsVisible = !this._editor.GlyphsVisible;
                        flag = true;
                        goto Label_09DA;

                    case 2:
                        this._editor.BordersVisible = !this._editor.BordersVisible;
                        flag = true;
                        goto Label_09DA;

                    case 3:
                        this._editor.GridVisible = !this._editor.GridVisible;
                        flag = true;
                        goto Label_09DA;

                    case 4:
                        this._editor.SnapEnabled = !this._editor.SnapEnabled;
                        flag = true;
                        goto Label_09DA;

                    case 100:
                        this._editor.TextFormatting.ToggleBold();
                        flag = true;
                        goto Label_09DA;

                    case 0x65:
                        this._editor.TextFormatting.ToggleItalics();
                        flag = true;
                        goto Label_09DA;

                    case 0x66:
                        this._editor.TextFormatting.ToggleUnderline();
                        flag = true;
                        goto Label_09DA;

                    case 0x67:
                        this._editor.TextFormatting.ToggleSuperscript();
                        flag = true;
                        goto Label_09DA;

                    case 0x68:
                        this._editor.TextFormatting.ToggleSubscript();
                        flag = true;
                        goto Label_09DA;

                    case 0x69:
                        this._editor.TextFormatting.ToggleStrikethrough();
                        flag = true;
                        goto Label_09DA;

                    case 0x6a:
                    {
                        ColorDialog dialog2 = new ColorDialog();
                        if (dialog2.ShowDialog() == DialogResult.OK)
                        {
                            this._editor.TextFormatting.ForeColor = dialog2.Color;
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0x6b:
                    {
                        ColorDialog dialog = new ColorDialog();
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            this._editor.TextFormatting.BackColor = dialog.Color;
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0x6c:
                        this._editor.TextFormatting.SetAlignment(Alignment.Left);
                        flag = true;
                        goto Label_09DA;

                    case 0x6d:
                        this._editor.TextFormatting.SetAlignment(Alignment.Right);
                        flag = true;
                        goto Label_09DA;

                    case 110:
                        this._editor.TextFormatting.SetAlignment(Alignment.Center);
                        flag = true;
                        goto Label_09DA;

                    case 0x6f:
                        this._editor.TextFormatting.SetTextFormat(TextFormat.OrderedList);
                        flag = true;
                        goto Label_09DA;

                    case 0x70:
                        this._editor.TextFormatting.SetTextFormat(TextFormat.UnorderedList);
                        flag = true;
                        goto Label_09DA;

                    case 0x71:
                        this._editor.TextFormatting.Indent();
                        flag = true;
                        goto Label_09DA;

                    case 0x72:
                        this._editor.TextFormatting.Unindent();
                        flag = true;
                        goto Label_09DA;

                    case 0x73:
                    case 0x74:
                    case 0x75:
                    case 0x76:
                    case 0x77:
                    case 0x7c:
                    case 0x7d:
                    case 0x7e:
                    case 0x7f:
                    case 0x80:
                    case 0x81:
                    case 0x8b:
                    case 230:
                        goto Label_09DA;

                    case 120:
                        commandUI = (ComboBoxToolBarButton) command.CommandUI;
                        this._editor.TextFormatting.SetTextFormat((TextFormat) commandUI.ComboBox.SelectedIndex);
                        flag = true;
                        goto Label_09DA;

                    case 0x79:
                        commandUI = (ComboBoxToolBarButton) command.CommandUI;
                        this._editor.TextFormatting.FontName = commandUI.ComboBox.Text;
                        flag = true;
                        goto Label_09DA;

                    case 0x7a:
                        commandUI = (ComboBoxToolBarButton) command.CommandUI;
                        this._editor.TextFormatting.FontSize = (FontSize) int.Parse(commandUI.ComboBox.Text);
                        flag = true;
                        goto Label_09DA;

                    case 0x7b:
                    {
                        IList elements = this._editor.Selection.Elements;
                        if ((elements != null) && (elements.Count != 0))
                        {
                            Element element = elements[0] as Element;
                            if (element != null)
                            {
                                IStyle style = new InlineStyle(element.Peer.GetStyle());
                                StyleBuilderDialog dialog3 = new StyleBuilderDialog(this._serviceProvider, this._editor.Url, element.ToString(), style);
                                ((IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService))).ShowDialog(dialog3);
                            }
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 130:
                    case 0x83:
                    case 0x84:
                    case 0x85:
                    case 0x86:
                    case 0x87:
                    case 0x88:
                    case 0x89:
                    case 0x8a:
                        this._editor.TextFormatting.SetTextFormat((TextFormat) (command.CommandID - 130));
                        flag = true;
                        goto Label_09DA;

                    case 140:
                        this._editor.Selection.ToggleAbsolutePosition();
                        flag = true;
                        goto Label_09DA;

                    case 0x8d:
                        this._editor.Selection.BringToFront();
                        flag = true;
                        goto Label_09DA;

                    case 0x8e:
                        this._editor.Selection.SendToBack();
                        flag = true;
                        goto Label_09DA;

                    case 0x8f:
                        this._editor.Selection.AlignLeft();
                        flag = true;
                        goto Label_09DA;

                    case 0x90:
                        this._editor.Selection.AlignHorizontalCenter();
                        flag = true;
                        goto Label_09DA;

                    case 0x91:
                        this._editor.Selection.AlignRight();
                        flag = true;
                        goto Label_09DA;

                    case 0x92:
                        this._editor.Selection.AlignTop();
                        flag = true;
                        goto Label_09DA;

                    case 0x93:
                        this._editor.Selection.AlignVerticalCenter();
                        flag = true;
                        goto Label_09DA;

                    case 0x94:
                        this._editor.Selection.AlignBottom();
                        flag = true;
                        goto Label_09DA;

                    case 0x95:
                        this._editor.Selection.MatchWidth();
                        flag = true;
                        goto Label_09DA;

                    case 150:
                        this._editor.Selection.MatchHeight();
                        flag = true;
                        goto Label_09DA;

                    case 0x97:
                        this._editor.Selection.MatchSize();
                        flag = true;
                        goto Label_09DA;

                    case 0x98:
                        this._editor.Selection.ToggleLock();
                        flag = true;
                        goto Label_09DA;

                    case 200:
                    {
                        string initialDescription = null;
                        if (this._editor.Selection.SelectionType == EditorSelectionType.TextSelection)
                        {
                            initialDescription = this._editor.Selection.Text;
                        }
                        InsertHyperlinkDialog form = new InsertHyperlinkDialog(initialDescription, this._serviceProvider);
                        IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                        if (service.ShowDialog(form) == DialogResult.OK)
                        {
                            this._editor.Document.InsertHyperlink(form.Url, form.Description);
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xc9:
                        this._editor.Selection.RemoveHyperlink();
                        flag = true;
                        goto Label_09DA;

                    case 0xca:
                        this._editor.Selection.WrapSelectionInSpan();
                        flag = true;
                        goto Label_09DA;

                    case 0xcb:
                        this._editor.Selection.WrapSelectionInDiv();
                        flag = true;
                        goto Label_09DA;

                    case 220:
                    {
                        InsertTableDialog dialog5 = new InsertTableDialog(this._serviceProvider);
                        IUIService service3 = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                        if (service3.ShowDialog(dialog5) == DialogResult.OK)
                        {
                            this._editor.Document.InsertHtml(dialog5.HtmlTableString);
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xdd:
                    {
                        EditorTable tableEditor = this._editor.Selection.TableEditor;
                        if (tableEditor != null)
                        {
                            tableEditor.InsertTableRow();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xde:
                    {
                        EditorTable table2 = this._editor.Selection.TableEditor;
                        if (table2 != null)
                        {
                            table2.InsertTableColumn();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xdf:
                    {
                        EditorTable table3 = this._editor.Selection.TableEditor;
                        if (table3 != null)
                        {
                            table3.DeleteTableRow();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xe0:
                    {
                        EditorTable table4 = this._editor.Selection.TableEditor;
                        if (table4 != null)
                        {
                            table4.DeleteTableColumn();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xe1:
                    {
                        EditorTable table5 = this._editor.Selection.TableEditor;
                        if (table5 != null)
                        {
                            table5.MergeLeft();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xe2:
                    {
                        EditorTable table6 = this._editor.Selection.TableEditor;
                        if (table6 != null)
                        {
                            table6.MergeRight();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xe3:
                    {
                        EditorTable table7 = this._editor.Selection.TableEditor;
                        if (table7 != null)
                        {
                            table7.MergeUp();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xe4:
                    {
                        EditorTable table8 = this._editor.Selection.TableEditor;
                        if (table8 != null)
                        {
                            table8.MergeDown();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xe5:
                    {
                        EditorTable table9 = this._editor.Selection.TableEditor;
                        if (table9 != null)
                        {
                            table9.SplitHorizontal();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 0xe7:
                    {
                        EditorTable table10 = this._editor.Selection.TableEditor;
                        if (table10 != null)
                        {
                            table10.SplitVertical();
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                    case 260:
                    {
                        EditorSelection selection = this._editor.Selection;
                        QuickTagEditDialog dialog6 = new QuickTagEditDialog(this._serviceProvider);
                        string outerHtml = selection.GetOuterHtml();
                        dialog6.TagText = outerHtml;
                        IUIService service4 = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                        if (service4.ShowDialog(dialog6) == DialogResult.OK)
                        {
                            string tagText = dialog6.TagText;
                            if (string.Compare(tagText, outerHtml) != 0)
                            {
                                selection.SetOuterHtml(tagText);
                            }
                        }
                        flag = true;
                        goto Label_09DA;
                    }
                }
            }
            else if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 100:
                        this._editor.Undo();
                        flag = true;
                        goto Label_09DA;

                    case 0x65:
                        this._editor.Redo();
                        flag = true;
                        goto Label_09DA;

                    case 0x66:
                        this._editor.Cut();
                        flag = true;
                        goto Label_09DA;

                    case 0x67:
                        this._editor.Copy();
                        flag = true;
                        goto Label_09DA;

                    case 0x68:
                        this._editor.Paste();
                        flag = true;
                        goto Label_09DA;

                    case 0x69:
                        goto Label_09DA;

                    case 0x6a:
                        this._editor.SelectAll();
                        flag = true;
                        goto Label_09DA;

                    case 3:
                    {
                        IDesignerHost host = (IDesignerHost) this._serviceProvider.GetService(typeof(IDesignerHost));
                        if (host.Loading)
                        {
                            flag = true;
                        }
                        goto Label_09DA;
                    }
                }
            }
        Label_09DA:
            if (flag)
            {
                this._commandManager.UpdateCommands(false);
            }
            return flag;
        }

        protected virtual void InitializeUserInterface()
        {
            this._editor = this.CreateEditor();
            this._editor.Dock = DockStyle.Fill;
            this._editor.DesignModeEnabled = true;
            this._editor.MultipleSelectionEnabled = true;
            this._editor.AbsolutePositioningEnabled = true;
            this._editor.BordersVisible = true;
            this._editor.ScrollBarsEnabled = true;
            this._editor.FlatScrollBars = true;
            this._editor.ScriptEnabled = false;
            this._editor.Border3d = false;
            this._editor.DataObjectConverter = this.DataObjectConverter;
            this._editor.Glyphs.AddStandardGlyphs();
            foreach (EditorGlyph glyph in this._editor.Glyphs.StandardGlyphs)
            {
                if (!glyph.Tag.Equals("form"))
                {
                    glyph.Visible = false;
                }
            }
            this._editor.Selection.SelectionChanged += new EventHandler(this.OnEditorSelectionChanged);
            this._editor.ReadyStateComplete += new EventHandler(this.OnEditorReadyStateComplete);
            this._editor.ShowContextMenu += new ShowContextMenuEventHandler(this.OnEditorShowContextMenu);
            base.Controls.Add(this._editor);
            this.OnEditorCreated();
            this._timer = new Timer();
            this._timer.Interval = 500;
            this._timer.Tick += new EventHandler(this.OnTimerTick);
        }

        protected virtual void LoadFromDocument(Microsoft.Matrix.Packages.Web.Documents.HtmlDocument document)
        {
            string text = document.Text;
            if (text != null)
            {
                this._editor.LoadHtml(text, document.ProjectItem.Url);
            }
        }

        void IDocumentView.Activate(bool viewSwitch)
        {
            this.EnsureEditor();
            this.EnableSelectionTracking(true);
            IServiceContainer service = (IServiceContainer) this._serviceProvider.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                service.AddService(typeof(ISelectionOutlineProvider), this);
                service.AddService(typeof(IDesignView), this);
            }
            IDocumentDesignerHost host = (IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost));
            host.DesignerActive = true;
            this._editor.Focus();
            this.OnActivated(EventArgs.Empty);
        }

        void IDocumentView.Deactivate(bool closing)
        {
            this.EnableSelectionTracking(false);
            IServiceContainer service = (IServiceContainer) this._serviceProvider.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                service.RemoveService(typeof(ISelectionOutlineProvider));
                service.RemoveService(typeof(IDesignView));
            }
            if (!closing)
            {
                IDocumentDesignerHost host = (IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost));
                host.DesignerActive = false;
            }
            this.OnDeactivated(EventArgs.Empty);
        }

        void IDocumentView.LoadFromDocument(Document document)
        {
            this._document = (Microsoft.Matrix.Packages.Web.Documents.HtmlDocument) document;
            this.EnsureEditor();
            ((IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost))).BeginLoad();
            this.OnBeforeLoadFromDocument(this._document);
            this.LoadFromDocument(this._document);
            this._commandManager.UpdateCommands(false);
            this._isDirty = true;
            this.OnDocumentChanged(EventArgs.Empty);
        }

        bool IDocumentView.SaveToDocument()
        {
            if (this.SaveToDocument())
            {
                this._isDirty = false;
                return true;
            }
            return false;
        }

        bool ISearchableDocumentView.PerformFind(string searchString, FindReplaceOptions options)
        {
            return this.PerformFind(searchString, options);
        }

        bool ISearchableDocumentView.PerformReplace(string searchString, string replaceString, FindReplaceOptions options)
        {
            return this.PerformReplace(searchString, replaceString, options);
        }

        void ISelectionContainer.SetSelectedObject(object o)
        {
            this.SetSelectedObject(o);
        }

        ICollection ISelectionOutlineProvider.GetOutline(object selectedObject)
        {
            return this.GetOutline(selectedObject);
        }

        void IToolboxClient.OnToolboxDataItemPicked(ToolboxDataItem dataItem)
        {
            this.OnToolboxDataItemPicked(dataItem);
        }

        bool IToolboxClient.SupportsToolboxSection(ToolboxSection section)
        {
            return this.SupportsToolboxSection(section);
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            return this.HandleCommand(command);
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            return this.UpdateCommand(command);
        }

        protected virtual void OnActivated(EventArgs e)
        {
        }

        protected virtual void OnBeforeLoadFromDocument(Microsoft.Matrix.Packages.Web.Documents.HtmlDocument document)
        {
        }

        protected virtual void OnDeactivated(EventArgs e)
        {
        }

        protected virtual void OnDocumentChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[DocumentChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
            this._editor.Focus();
        }

        protected virtual void OnEditorCreated()
        {
        }

        private void OnEditorReadyStateComplete(object sender, EventArgs e)
        {
            ((IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost))).EndLoad();
            this._commandManager.UpdateCommands(false);
        }

        private void OnEditorSelectionChanged(object sender, EventArgs args)
        {
            this.UpdateSelection();
        }

        private void OnEditorShowContextMenu(object sender, ShowContextMenuEventArgs e)
        {
            ICommandManager service = (ICommandManager) this.ServiceProvider.GetService(typeof(ICommandManager));
            ((DocumentWindow) this.ServiceProvider.GetService(typeof(DocumentWindow))).Activate();
            if (service != null)
            {
                if (EditorTable.IsTableSelection(this._editor.Selection))
                {
                    service.ShowContextMenu(typeof(WebCommands), 2, null, null, this, e.Location);
                }
                else
                {
                    service.ShowContextMenu(typeof(WebCommands), 1, null, null, this, e.Location);
                }
            }
        }

        protected void OnTimerTick(object sender, EventArgs args)
        {
            this.UpdateDesignState();
        }

        public virtual void OnToolboxDataItemPicked(ToolboxDataItem dataItem)
        {
            IDesignerHost service = (IDesignerHost) this.ServiceProvider.GetService(typeof(IDesignerHost));
            IDataObject dataObject = dataItem.GetDataObject(service);
            string html = string.Empty;
            if (dataObject != null)
            {
                if (dataObject.GetDataPresent(DataFormats.Html))
                {
                    html = dataObject.GetData(DataFormats.Html).ToString();
                }
                else if (dataObject.GetDataPresent(DataFormats.Text))
                {
                    html = dataObject.GetData(DataFormats.Text).ToString();
                }
                else if (this.DataObjectConverter.CanConvertToHtml(dataObject) == DataObjectConverterInfo.CanConvert)
                {
                    DataObject newDataObject = new DataObject();
                    if (this.DataObjectConverter.ConvertToHtml(dataObject, newDataObject))
                    {
                        html = (string) newDataObject.GetData(DataFormats.Html);
                    }
                }
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Toolbox");
                try
                {
                    this._editor.Document.InsertHtml(html);
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        protected virtual bool PerformFind(string searchString, FindReplaceOptions options)
        {
            return this._editor.Find(searchString, (options & FindReplaceOptions.MatchCase) != FindReplaceOptions.None, (options & FindReplaceOptions.WholeWord) != FindReplaceOptions.None, (options & FindReplaceOptions.SearchUp) != FindReplaceOptions.None);
        }

        protected virtual bool PerformReplace(string searchString, string replaceString, FindReplaceOptions options)
        {
            return this._editor.Replace(searchString, replaceString, (options & FindReplaceOptions.MatchCase) != FindReplaceOptions.None, (options & FindReplaceOptions.WholeWord) != FindReplaceOptions.None, (options & FindReplaceOptions.SearchUp) != FindReplaceOptions.None);
        }

        protected virtual bool SaveToDocument()
        {
            this._document.Text = this._editor.SaveHtml();
            return true;
        }

        protected virtual void SetSelectedObject(object o)
        {
            this._editor.Selection.SelectElement(o);
        }

        public virtual bool SupportsToolboxSection(ToolboxSection section)
        {
            Type type = section.GetType();
            if (type != typeof(HtmlElementToolboxSection))
            {
                return (type == typeof(SnippetToolboxSection));
            }
            return true;
        }

        protected virtual bool UpdateCommand(Command command)
        {
            bool flag = false;
            this.UpdateDesignState();
            if (command.CommandGroup != typeof(WebCommands))
            {
                if (command.CommandGroup == typeof(GlobalCommands))
                {
                    switch (command.CommandID)
                    {
                        case 100:
                            command.Enabled = this._editor.CanUndo;
                            return true;

                        case 0x65:
                            command.Enabled = this._editor.CanRedo;
                            return true;

                        case 0x66:
                            command.Enabled = this._editor.CanCut;
                            return true;

                        case 0x67:
                            command.Enabled = this._editor.CanCopy;
                            return true;

                        case 0x68:
                            command.Enabled = this._editor.CanPaste;
                            return true;

                        case 0x69:
                            return flag;

                        case 0x6a:
                            return true;

                        case 3:
                        {
                            IDesignerHost service = (IDesignerHost) this._serviceProvider.GetService(typeof(IDesignerHost));
                            if (service.Loading)
                            {
                                command.Enabled = false;
                                flag = true;
                            }
                            return flag;
                        }
                    }
                }
                return flag;
            }
            int commandID = command.CommandID;
            if (commandID <= 0x98)
            {
                ToolBarComboBoxCommand command2;
                switch (commandID)
                {
                    case 1:
                        command.Enabled = true;
                        command.Checked = this._editor.GlyphsVisible;
                        return true;

                    case 2:
                        command.Enabled = true;
                        command.Checked = this._editor.BordersVisible;
                        return true;

                    case 3:
                        command.Enabled = true;
                        command.Checked = this._editor.GridVisible;
                        return true;

                    case 4:
                        command.Enabled = true;
                        command.Checked = this._editor.SnapEnabled;
                        return true;

                    case 100:
                        command.Enabled = (this._editor.TextFormatting.GetBoldInfo() & CommandInfo.Enabled) != 0;
                        command.Checked = (this._editor.TextFormatting.GetBoldInfo() & CommandInfo.Checked) != 0;
                        return true;

                    case 0x65:
                        command.Enabled = (this._editor.TextFormatting.GetItalicsInfo() & CommandInfo.Enabled) != 0;
                        command.Checked = (this._editor.TextFormatting.GetItalicsInfo() & CommandInfo.Checked) != 0;
                        return true;

                    case 0x66:
                        command.Enabled = (this._editor.TextFormatting.GetUnderlineInfo() & CommandInfo.Enabled) != 0;
                        command.Checked = (this._editor.TextFormatting.GetUnderlineInfo() & CommandInfo.Checked) != 0;
                        return true;

                    case 0x67:
                        command.Enabled = (this._editor.TextFormatting.GetSuperscriptInfo() & CommandInfo.Enabled) != 0;
                        command.Checked = (this._editor.TextFormatting.GetSuperscriptInfo() & CommandInfo.Checked) != 0;
                        return true;

                    case 0x68:
                        command.Enabled = (this._editor.TextFormatting.GetSubscriptInfo() & CommandInfo.Enabled) != 0;
                        command.Checked = (this._editor.TextFormatting.GetSubscriptInfo() & CommandInfo.Checked) != 0;
                        return true;

                    case 0x69:
                        command.Enabled = (this._editor.TextFormatting.GetStrikethroughInfo() & CommandInfo.Enabled) != 0;
                        command.Checked = (this._editor.TextFormatting.GetStrikethroughInfo() & CommandInfo.Checked) != 0;
                        return true;

                    case 0x6a:
                        command.Enabled = this._editor.TextFormatting.CanSetForeColor;
                        return true;

                    case 0x6b:
                        command.Enabled = this._editor.TextFormatting.CanSetBackColor;
                        return true;

                    case 0x6c:
                        command.Enabled = this._editor.TextFormatting.CanAlign(Alignment.Left);
                        command.Checked = this._editor.TextFormatting.GetAlignment() == Alignment.Left;
                        return true;

                    case 0x6d:
                        command.Enabled = this._editor.TextFormatting.CanAlign(Alignment.Right);
                        command.Checked = this._editor.TextFormatting.GetAlignment() == Alignment.Right;
                        return true;

                    case 110:
                        command.Enabled = this._editor.TextFormatting.CanAlign(Alignment.Center);
                        command.Checked = this._editor.TextFormatting.GetAlignment() == Alignment.Center;
                        return true;

                    case 0x6f:
                        command.Enabled = this._editor.TextFormatting.CanSetHtmlFormat;
                        command.Checked = this._editor.TextFormatting.GetTextFormat() == TextFormat.OrderedList;
                        return true;

                    case 0x70:
                        command.Enabled = this._editor.TextFormatting.CanSetHtmlFormat;
                        command.Checked = this._editor.TextFormatting.GetTextFormat() == TextFormat.UnorderedList;
                        return true;

                    case 0x71:
                        command.Enabled = this._editor.TextFormatting.CanIndent;
                        return true;

                    case 0x72:
                        command.Enabled = this._editor.TextFormatting.CanUnindent;
                        return true;

                    case 0x73:
                    case 0x74:
                    case 0x75:
                    case 0x76:
                    case 0x77:
                    case 0x7c:
                    case 0x7d:
                    case 0x7e:
                    case 0x7f:
                    case 0x80:
                    case 0x81:
                    case 0x8b:
                        return flag;

                    case 120:
                    {
                        command2 = (ToolBarComboBoxCommand) command;
                        command.Enabled = this._editor.TextFormatting.CanSetHtmlFormat;
                        int textFormat = (int) this._editor.TextFormatting.GetTextFormat();
                        if (textFormat >= 9)
                        {
                            command2.SelectedIndex = -1;
                        }
                        else
                        {
                            command2.SelectedIndex = textFormat;
                        }
                        goto Label_04FC;
                    }
                    case 0x79:
                        command2 = (ToolBarComboBoxCommand) command;
                        command.Enabled = this._editor.TextFormatting.CanSetFontName;
                        command2.Text = this._editor.TextFormatting.FontName;
                        return true;

                    case 0x7a:
                        command2 = (ToolBarComboBoxCommand) command;
                        command.Enabled = this._editor.TextFormatting.CanSetFontSize;
                        command2.SelectedIndex = ((int) this._editor.TextFormatting.FontSize) - 1;
                        return true;

                    case 0x7b:
                        command.Enabled = false;
                        if ((this._editor.Selection.Length != 0) || (this._editor.Selection.SelectionType == EditorSelectionType.TextSelection))
                        {
                            IList elements = this._editor.Selection.Elements;
                            if ((elements != null) && (elements.Count != 0))
                            {
                                command.Enabled = elements[0] is Element;
                            }
                        }
                        return true;

                    case 130:
                    case 0x83:
                    case 0x84:
                    case 0x85:
                    case 0x86:
                    case 0x87:
                    case 0x88:
                    case 0x89:
                    case 0x8a:
                        command.Enabled = this._editor.TextFormatting.CanSetHtmlFormat;
                        command.Checked = this._editor.TextFormatting.GetTextFormat() == (TextFormat)(command.CommandID - 130);
                        return true;

                    case 140:
                    {
                        CommandInfo absolutePositionInfo = this._editor.Selection.GetAbsolutePositionInfo();
                        command.Enabled = (absolutePositionInfo & CommandInfo.Enabled) != 0;
                        command.Checked = (absolutePositionInfo & CommandInfo.Checked) != 0;
                        return true;
                    }
                    case 0x8d:
                    case 0x8e:
                        command.Enabled = this._editor.Selection.CanChangeZIndex;
                        return true;

                    case 0x8f:
                    case 0x90:
                    case 0x91:
                    case 0x92:
                    case 0x93:
                    case 0x94:
                        command.Enabled = this._editor.Selection.CanAlign;
                        return true;

                    case 0x95:
                    case 150:
                    case 0x97:
                        command.Enabled = this._editor.Selection.CanMatchSize;
                        return true;

                    case 0x98:
                    {
                        CommandInfo lockInfo = this._editor.Selection.GetLockInfo();
                        command.Enabled = (lockInfo & CommandInfo.Enabled) != 0;
                        command.Checked = (lockInfo & CommandInfo.Checked) != 0;
                        return true;
                    }
                }
                return flag;
            }
            switch (commandID)
            {
                case 200:
                    command.Enabled = this._editor.Document.CanInsertHyperlink;
                    return true;

                case 0xc9:
                    command.Enabled = this._editor.Selection.CanRemoveHyperlink;
                    return true;

                case 0xca:
                case 0xcb:
                    command.Enabled = this._editor.Selection.CanWrapSelection;
                    return true;

                case 220:
                    command.Enabled = this._editor.Document.CanInsertHtml;
                    return true;

                case 0xdd:
                {
                    command.Enabled = false;
                    EditorTable tableEditor = this._editor.Selection.TableEditor;
                    if (tableEditor != null)
                    {
                        command.Enabled = tableEditor.CanInsertTableRow;
                        Interop.IHTMLElement selectedElement = tableEditor.SelectedElement;
                        if ((string.Compare(selectedElement.GetTagName(), "td", true) != 0) && (string.Compare(selectedElement.GetTagName(), "tr", true) != 0))
                        {
                            if ((string.Compare(selectedElement.GetTagName(), "tbody", true) == 0) || (string.Compare(selectedElement.GetTagName(), "table", true) == 0))
                            {
                                command.Enabled = true;
                                command.Text = "Add Table Row";
                                command.HelpText = "Add a table row after at the end of the table";
                            }
                        }
                        else
                        {
                            command.Enabled = true;
                            command.Text = "Insert Table Row";
                            command.HelpText = "Insert a table row after the currently selected table cell";
                        }
                    }
                    return true;
                }
                case 0xde:
                {
                    command.Enabled = false;
                    EditorTable table2 = this._editor.Selection.TableEditor;
                    if (table2 != null)
                    {
                        command.Enabled = table2.CanInsertTableColumn;
                        Interop.IHTMLElement element3 = table2.SelectedElement;
                        if ((string.Compare(element3.GetTagName(), "td", true) != 0) && (string.Compare(element3.GetTagName(), "tr", true) != 0))
                        {
                            if ((string.Compare(element3.GetTagName(), "tbody", true) == 0) || (string.Compare(element3.GetTagName(), "table", true) == 0))
                            {
                                command.Enabled = true;
                                command.Text = "Add Table Column";
                                command.HelpText = "Add a table column after at the end of the table";
                            }
                        }
                        else
                        {
                            command.Enabled = true;
                            command.Text = "Insert Table Column";
                            command.HelpText = "Insert a table column after the currently selected table cell";
                        }
                    }
                    return true;
                }
                case 0xdf:
                {
                    command.Enabled = false;
                    EditorTable table3 = this._editor.Selection.TableEditor;
                    if (table3 != null)
                    {
                        command.Enabled = table3.CanDeleteTableRow;
                    }
                    return true;
                }
                case 0xe0:
                {
                    command.Enabled = false;
                    EditorTable table4 = this._editor.Selection.TableEditor;
                    if (table4 != null)
                    {
                        command.Enabled = table4.CanDeleteTableColumn;
                    }
                    return true;
                }
                case 0xe1:
                {
                    command.Enabled = false;
                    EditorTable table5 = this._editor.Selection.TableEditor;
                    if (table5 != null)
                    {
                        command.Enabled = table5.CanMergeLeft;
                    }
                    return true;
                }
                case 0xe2:
                {
                    command.Enabled = false;
                    EditorTable table6 = this._editor.Selection.TableEditor;
                    if (table6 != null)
                    {
                        command.Enabled = table6.CanMergeRight;
                    }
                    return true;
                }
                case 0xe3:
                {
                    command.Enabled = false;
                    EditorTable table7 = this._editor.Selection.TableEditor;
                    if (table7 != null)
                    {
                        command.Enabled = table7.CanMergeUp;
                    }
                    return true;
                }
                case 0xe4:
                {
                    command.Enabled = false;
                    EditorTable table8 = this._editor.Selection.TableEditor;
                    if (table8 != null)
                    {
                        command.Enabled = table8.CanMergeDown;
                    }
                    return true;
                }
                case 0xe5:
                {
                    command.Enabled = false;
                    EditorTable table9 = this._editor.Selection.TableEditor;
                    if (table9 != null)
                    {
                        command.Enabled = table9.CanSplitHorizontal;
                    }
                    return true;
                }
                case 230:
                    return flag;

                case 0xe7:
                {
                    command.Enabled = false;
                    EditorTable table10 = this._editor.Selection.TableEditor;
                    if (table10 != null)
                    {
                        command.Enabled = table10.CanSplitVertical;
                    }
                    return true;
                }
                case 260:
                    if ((this._editor.Selection.SelectionType == EditorSelectionType.TextSelection) || ((this._editor.Selection.SelectionType == EditorSelectionType.ElementSelection) && (this._editor.Selection.Elements.Count == 1)))
                    {
                        command.Enabled = true;
                        foreach (object obj2 in this._editor.Selection.Elements)
                        {
                            if (obj2 is Element)
                            {
                                Element element = (Element) obj2;
                                switch (element.TagName.ToLower())
                                {
                                    case "body":
                                    case "form":
                                    case "tr":
                                    case "td":
                                        command.Enabled = false;
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        command.Enabled = false;
                    }
                    return true;

                default:
                    return flag;
            }
        Label_04FC:
            return true;
        }

        public void UpdateDesignState()
        {
            if (!this._editor.Selection.SynchronizeSelection())
            {
                this._commandManager.UpdateCommands(false);
            }
            else if (this._editor.Selection.SelectionType == EditorSelectionType.ElementSelection)
            {
                this._editor.Focus();
            }
            if (!this._isDirty && this._editor.IsDirty)
            {
                this._isDirty = true;
                this.OnDocumentChanged(EventArgs.Empty);
            }
        }

        protected void UpdateSelection()
        {
            if (this._selectionTrackingEnabled)
            {
                ISelectionService service = (ISelectionService) this._serviceProvider.GetService(typeof(ISelectionService));
                if (service != null)
                {
                    ICollection elements = this._editor.Selection.Elements;
                    service.SetSelectedComponents(elements);
                }
            }
        }

        protected virtual bool CanDeactivate
        {
            get
            {
                IDesignerHost service = (IDesignerHost) this._serviceProvider.GetService(typeof(IDesignerHost));
                return !service.Loading;
            }
        }

        private Microsoft.Matrix.Packages.Web.Html.DataObjectConverter DataObjectConverter
        {
            get
            {
                if (this._dataObjectConverter == null)
                {
                    this._dataObjectConverter = this.CreateDataObjectConverter();
                }
                return this._dataObjectConverter;
            }
        }

        public virtual ToolboxSection DefaultToolboxSection
        {
            get
            {
                return HtmlElementToolboxSection.HtmlElements;
            }
        }

        public HtmlEditor Editor
        {
            get
            {
                return this._editor;
            }
        }

        protected virtual FindReplaceOptions FindSupport
        {
            get
            {
                return (FindReplaceOptions.SearchUp | FindReplaceOptions.WholeWord | FindReplaceOptions.MatchCase);
            }
        }

        //protected virtual bool IsDirty //NOTE: changed
        public virtual bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
        }

        bool IDocumentView.CanDeactivate
        {
            get
            {
                return this.CanDeactivate;
            }
        }

        bool IDocumentView.IsDirty
        {
            get
            {
                return this.IsDirty;
            }
        }

        Image IDocumentView.ViewImage
        {
            get
            {
                return this.ViewImage;
            }
        }

        string IDocumentView.ViewName
        {
            get
            {
                return this.ViewName;
            }
        }

        DocumentViewType IDocumentView.ViewType
        {
            get
            {
                return this.ViewType;
            }
        }

        bool IPropertyBrowserClient.SupportsPropertyBrowser
        {
            get
            {
                return this.SupportsPropertyBrowser;
            }
        }

        FindReplaceOptions ISearchableDocumentView.FindSupport
        {
            get
            {
                return this.FindSupport;
            }
        }

        string ISearchableDocumentView.InitialSearchString
        {
            get
            {
                EditorSelection selection = this.Editor.Selection;
                if (selection.SelectionType == EditorSelectionType.TextSelection)
                {
                    string text = selection.Text;
                    if (((text != null) && (text.Length != 0)) && ((text.IndexOf('\r') == -1) && (text.IndexOf('\n') == -1)))
                    {
                        return text;
                    }
                }
                return string.Empty;
            }
        }

        FindReplaceOptions ISearchableDocumentView.ReplaceSupport
        {
            get
            {
                return this.ReplaceSupport;
            }
        }

        string ISelectionOutlineProvider.OutlineTitle
        {
            get
            {
                return this.OutlineTitle;
            }
        }

        ToolboxSection IToolboxClient.DefaultToolboxSection
        {
            get
            {
                return this.DefaultToolboxSection;
            }
        }

        protected virtual string OutlineTitle
        {
            get
            {
                return "(Click to see parent HTML elements)";
            }
        }

        protected virtual FindReplaceOptions ReplaceSupport
        {
            get
            {
                return (FindReplaceOptions.SearchUp | FindReplaceOptions.WholeWord | FindReplaceOptions.MatchCase);
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }

        protected virtual bool SupportsPropertyBrowser
        {
            get
            {
                return true;
            }
        }

        protected virtual Image ViewImage
        {
            get
            {
                if (viewImage == null)
                {
                    viewImage = new Bitmap(typeof(HtmlDesignView), "HtmlDesignView.bmp");
                    viewImage.MakeTransparent();
                }
                return viewImage;
            }
        }

        protected virtual string ViewName
        {
            get
            {
                return "Design";
            }
        }

        protected virtual DocumentViewType ViewType
        {
            get
            {
                return DocumentViewType.Design;
            }
        }
    }
}


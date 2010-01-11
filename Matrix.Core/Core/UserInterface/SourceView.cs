namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class SourceView : TextControl, IDocumentView, ISearchableDocumentView, ICommandHandler, IToolboxClient, IPropertyBrowserClient
    {
        private ICommandManager _commandManager;
        private bool _dirty;
        private TextDocument _document;
        private bool _internalChange;
        private bool _overwriteMode;
        private IServiceProvider _serviceProvider;
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

        public SourceView(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._commandManager = (ICommandManager) serviceProvider.GetService(typeof(ICommandManager));
            this._internalChange = true;
            this.BeginInit();
            try
            {
                this.Site = this.CreateTextControlSite();
            }
            finally
            {
                this.EndInit();
            }
            this._internalChange = false;
            base.HelpEnabled = true;
        }

        protected virtual ISite CreateTextControlSite()
        {
            return new SourceViewTextControlSite(this);
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

        private void FocusWindow()
        {
            ((DocumentWindow) this.ServiceProvider.GetService(typeof(DocumentWindow))).Activate();
        }

        protected override bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 0x8d:
                        ((ICommandHandler) this).HandleCommand(new TextBufferCommand(0x5c, base.Selection.Start, base.Selection.End));
                        flag = true;
                        goto Label_01B0;

                    case 0x8e:
                        ((ICommandHandler) this).HandleCommand(new TextBufferCommand(0x5d, base.Selection.Start, base.Selection.End));
                        flag = true;
                        goto Label_01B0;

                    case 600:
                        ((ICommandHandler) this).HandleCommand(new TextBufferCommand(0x5e));
                        flag = true;
                        goto Label_01B0;

                    case 100:
                        base.Undo();
                        flag = true;
                        goto Label_01B0;

                    case 0x65:
                        base.Redo();
                        flag = true;
                        goto Label_01B0;

                    case 0x66:
                        base.Cut();
                        flag = true;
                        goto Label_01B0;

                    case 0x67:
                        base.Copy();
                        flag = true;
                        goto Label_01B0;

                    case 0x68:
                        base.Paste();
                        flag = true;
                        goto Label_01B0;

                    case 0x69:
                        goto Label_01B0;

                    case 0x6a:
                        base.SelectAll();
                        flag = true;
                        goto Label_01B0;

                    case 0x6b:
                    {
                        GoToLineDialog form = new GoToLineDialog(this._serviceProvider, base.CaretLineIndex + 1);
                        IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                        if (service.ShowDialog(form) == DialogResult.OK)
                        {
                            base.CaretLineIndex = form.LineNumber - 1;
                        }
                        flag = true;
                        goto Label_01B0;
                    }
                    case 120:
                        SnippetToolboxSection.Snippets.AddToolboxDataItem(SnippetToolboxSection.Snippets.CreateToolboxDataItem(base.SelectedText));
                        flag = true;
                        goto Label_01B0;
                }
            }
            else if (command.CommandGroup == typeof(TextBufferCommands))
            {
                flag = base.HandleCommand(command);
            }
        Label_01B0:
            if (flag)
            {
                this._commandManager.UpdateCommands(false);
            }
            return flag;
        }

        protected virtual void LoadDocument()
        {
            this.Text = this.Document.Text;
        }

        void IDocumentView.Activate(bool viewSwitch)
        {
            ISelectionService service = (ISelectionService) this._serviceProvider.GetService(typeof(ISelectionService));
            if (service != null)
            {
                service.SetSelectedComponents(null);
            }
            base.ClearSelection();
            base.Focus();
            this.OnActivated();
        }

        void IDocumentView.Deactivate(bool closing)
        {
            if (!closing)
            {
                this._dirty = false;
            }
            this.OnDeactivated();
        }

        void IDocumentView.LoadFromDocument(Microsoft.Matrix.Core.Documents.Document document)
        {
            this._document = (TextDocument) document;
            try
            {
                this._internalChange = true;
                this.LoadDocument();
                this.UpdateLanguageSettings();
            }
            finally
            {
                this._internalChange = false;
            }
        }

        bool IDocumentView.SaveToDocument()
        {
            if (this.SaveToDocument())
            {
                this._dirty = false;
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

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnDeactivated()
        {
        }

        protected virtual void OnDocumentChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[DocumentChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy;
            }
        }

        protected override void OnShowContextMenu(ShowContextMenuEventArgs e)
        {
            base.OnShowContextMenu(e);
            ICommandManager service = (ICommandManager) this.ServiceProvider.GetService(typeof(ICommandManager));
            this.FocusWindow();
            if (service != null)
            {
                service.ShowContextMenu(typeof(GlobalCommands), 3, null, null, this, e.Location);
            }
        }

        protected override void OnTextBufferChanged(object sender, TextBufferEventArgs e)
        {
            base.OnTextBufferChanged(sender, e);
            this._commandManager.UpdateCommands(false);
            if (!this._internalChange)
            {
                this._dirty = true;
                this.OnDocumentChanged(EventArgs.Empty);
            }
        }

        public virtual void OnToolboxDataItemPicked(ToolboxDataItem dataItem)
        {
            IDesignerHost service = (IDesignerHost) this.ServiceProvider.GetService(typeof(IDesignerHost));
            IDataObject dataObject = dataItem.GetDataObject(service);
            if (dataObject != null)
            {
                base.Paste(dataObject);
            }
        }

        protected internal override void OnViewSelectionChanged(TextView view)
        {
            base.OnViewSelectionChanged(view);
            this._commandManager.UpdateCommands(false);
        }

        protected virtual bool PerformFind(string searchString, FindReplaceOptions options)
        {
            bool searchUp = (options & FindReplaceOptions.SearchUp) != FindReplaceOptions.None;
            bool matchCase = (options & FindReplaceOptions.MatchCase) != FindReplaceOptions.None;
            bool wholeWord = (options & FindReplaceOptions.WholeWord) != FindReplaceOptions.None;
            bool inSelection = (options & FindReplaceOptions.InSelection) != FindReplaceOptions.None;
            using (TextBufferSpan span = base.Find(searchString, matchCase, wholeWord, searchUp, inSelection))
            {
                if (span != null)
                {
                    base.Select(span);
                    return true;
                }
            }
            return false;
        }

        protected virtual bool PerformReplace(string searchString, string replaceString, FindReplaceOptions options)
        {
            bool searchUp = (options & FindReplaceOptions.SearchUp) != FindReplaceOptions.None;
            bool matchCase = (options & FindReplaceOptions.MatchCase) != FindReplaceOptions.None;
            bool wholeWord = (options & FindReplaceOptions.WholeWord) != FindReplaceOptions.None;
            bool replaceAll = (options & FindReplaceOptions.All) != FindReplaceOptions.None;
            bool inSelection = (options & FindReplaceOptions.InSelection) != FindReplaceOptions.None;
            return base.Replace(searchString, replaceString, matchCase, wholeWord, searchUp, replaceAll, inSelection);
        }

        protected virtual bool SaveToDocument()
        {
            this._document.Text = this.Text;
            return true;
        }

        public virtual bool SupportsToolboxSection(ToolboxSection section)
        {
            return (section.GetType() == typeof(SnippetToolboxSection));
        }

        protected override bool UpdateCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup != typeof(GlobalCommands))
            {
                if (command.CommandGroup == typeof(TextBufferCommands))
                {
                    flag = base.UpdateCommand(command);
                }
                return flag;
            }
            int commandID = command.CommandID;
            if (commandID <= 120)
            {
                switch (commandID)
                {
                    case 100:
                        command.Enabled = base.CanUndo;
                        return true;

                    case 0x65:
                        command.Enabled = base.CanRedo;
                        return true;

                    case 0x66:
                    case 0x67:
                        command.Enabled = base.CanCopy;
                        return true;

                    case 0x68:
                        command.Enabled = base.CanPaste;
                        return true;

                    case 0x69:
                        return flag;

                    case 0x6a:
                        command.Enabled = true;
                        return true;

                    case 0x6b:
                        command.Enabled = true;
                        return true;

                    case 120:
                        command.Enabled = (SnippetToolboxSection.Snippets != null) && base.CanCopy;
                        return true;
                }
                return flag;
            }
            switch (commandID)
            {
                case 0x8d:
                {
                    TextBufferCommand command2 = new TextBufferCommand(0x5c);
                    ((ICommandHandler) this).UpdateCommand(command2);
                    command.Enabled = command2.Enabled && base.SelectionExists;
                    return true;
                }
                case 0x8e:
                {
                    TextBufferCommand command3 = new TextBufferCommand(0x5d);
                    ((ICommandHandler) this).UpdateCommand(command3);
                    command.Enabled = command3.Enabled && base.SelectionExists;
                    return true;
                }
                case 600:
                    command.Enabled = true;
                    return true;

                case 0x3e9:
                    ((EditorStatusBarPanelCommand) command).Line = base.CaretLineIndex + 1;
                    ((EditorStatusBarPanelCommand) command).Column = base.TabbedColumnIndex + 1;
                    return true;

                case 0x3ea:
                    if (base.InsertMode)
                    {
                        command.Text = "INS";
                        break;
                    }
                    command.Text = "OVR";
                    break;

                default:
                    return flag;
            }
            return true;
        }

        internal void UpdateLanguageSettings()
        {
            TextDocumentLanguage textLanguage = base.TextLanguage as TextDocumentLanguage;
            if (textLanguage != null)
            {
                TextOptions textOptions = textLanguage.GetTextOptions(this._serviceProvider);
                base.ConvertTabsToSpaces = textOptions.ConvertTabsToSpaces;
                this.LineNumbersVisible = textOptions.ShowLineNumbers;
                this.ShowWhitespace = textOptions.ShowWhitespace;
                this.TabSize = textOptions.TabSize;
                this.TrimTrailingWhitespace = textOptions.TrimTrailingWhitespace;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 0x100) && (((int) m.WParam) == 0x2d))
            {
                this._overwriteMode = !this._overwriteMode;
            }
            base.WndProc(ref m);
        }

        public virtual ToolboxSection DefaultToolboxSection
        {
            get
            {
                return SnippetToolboxSection.Snippets;
            }
        }

        protected TextDocument Document
        {
            get
            {
                return this._document;
            }
        }

        protected virtual FindReplaceOptions FindSupport
        {
            get
            {
                FindReplaceOptions options = FindReplaceOptions.SearchUp | FindReplaceOptions.WholeWord | FindReplaceOptions.MatchCase;
                if (base.SelectionExists)
                {
                    options |= FindReplaceOptions.InSelection;
                }
                return options;
            }
        }

        protected virtual string InitialSearchString
        {
            get
            {
                if ((base.Selection != null) && base.Selection.IsSingleLine)
                {
                    return base.Selection.Text;
                }
                return string.Empty;
            }
        }

        bool IDocumentView.CanDeactivate
        {
            get
            {
                return true;
            }
        }

        bool IDocumentView.IsDirty
        {
            get
            {
                return this._dirty;
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
                return this.InitialSearchString;
            }
        }

        FindReplaceOptions ISearchableDocumentView.ReplaceSupport
        {
            get
            {
                return this.ReplaceSupport;
            }
        }

        ToolboxSection IToolboxClient.DefaultToolboxSection
        {
            get
            {
                return this.DefaultToolboxSection;
            }
        }

        protected virtual FindReplaceOptions ReplaceSupport
        {
            get
            {
                FindReplaceOptions options = FindReplaceOptions.All | FindReplaceOptions.SearchUp | FindReplaceOptions.WholeWord | FindReplaceOptions.MatchCase;
                if (base.SelectionExists)
                {
                    options |= FindReplaceOptions.InSelection;
                }
                return options;
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
                    viewImage = new Bitmap(typeof(SourceView), "SourceView.bmp");
                    viewImage.MakeTransparent();
                }
                return viewImage;
            }
        }

        protected virtual string ViewName
        {
            get
            {
                return "Source";
            }
        }

        protected virtual DocumentViewType ViewType
        {
            get
            {
                return DocumentViewType.Source;
            }
        }

        private class SourceViewTextControlSite : ISite, IServiceProvider
        {
            private SourceView _owner;

            public SourceViewTextControlSite(SourceView owner)
            {
                this._owner = owner;
            }

            public object GetService(Type type)
            {
                if (type == typeof(ITextLanguage))
                {
                    Document document = this._owner.Document;
                    if (document != null)
                    {
                        return (document.Language as ITextLanguage);
                    }
                }
                return this._owner.ServiceProvider.GetService(type);
            }

            public IComponent Component
            {
                get
                {
                    return null;
                }
            }

            public IContainer Container
            {
                get
                {
                    return null;
                }
            }

            public bool DesignMode
            {
                get
                {
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return "SourceViewTextControlSite";
                }
                set
                {
                }
            }
        }
    }
}


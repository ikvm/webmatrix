namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Packages.Web.Html;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class HtmlPreviewView : Panel, IDocumentView, ICommandHandler, ISearchableDocumentView
    {
        private HtmlEditor _editor;
        private IServiceProvider _serviceProvider;
        private static Bitmap viewImage;

        event EventHandler IDocumentView.DocumentChanged
        {
            add
            {
            }
            remove
            {
            }
        }

        public HtmlPreviewView(IServiceProvider provider)
        {
            this._serviceProvider = provider;
        }

        protected virtual HtmlEditor CreateEditor()
        {
            return new HtmlEditor(this._serviceProvider);
        }

        public void EnsureEditor()
        {
            if (this._editor == null)
            {
                this.InitializeUserInterface();
            }
        }

        protected virtual bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup != typeof(WebCommands))
            {
                if (command.CommandGroup == typeof(GlobalCommands))
                {
                    switch (command.CommandID)
                    {
                        case 100:
                        case 0x65:
                        case 0x66:
                        case 0x68:
                            return true;

                        case 0x67:
                            this._editor.Copy();
                            return true;
                    }
                }
                return flag;
            }
            int commandID = command.CommandID;
            if (commandID <= 0x98)
            {
                switch (commandID)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 100:
                    case 0x65:
                    case 0x66:
                    case 0x67:
                    case 0x68:
                    case 0x69:
                    case 0x6a:
                    case 0x6b:
                    case 0x6c:
                    case 0x6d:
                    case 110:
                    case 0x6f:
                    case 0x70:
                    case 0x71:
                    case 0x72:
                    case 120:
                    case 0x79:
                    case 0x7a:
                    case 140:
                    case 0x8d:
                    case 0x8e:
                    case 0x8f:
                    case 0x90:
                    case 0x91:
                    case 0x92:
                    case 0x93:
                    case 0x94:
                    case 0x95:
                    case 150:
                    case 0x97:
                    case 0x98:
                        goto Label_0144;

                    case 0x73:
                    case 0x74:
                    case 0x75:
                    case 0x76:
                    case 0x77:
                    case 0x7b:
                    case 0x7c:
                    case 0x7d:
                    case 0x7e:
                    case 0x7f:
                    case 0x80:
                    case 0x81:
                    case 130:
                    case 0x83:
                    case 0x84:
                    case 0x85:
                    case 0x86:
                    case 0x87:
                    case 0x88:
                    case 0x89:
                    case 0x8a:
                    case 0x8b:
                        return flag;
                }
                return flag;
            }
            switch (commandID)
            {
                case 200:
                case 0xc9:
                case 0xca:
                case 0xcb:
                case 220:
                    goto Label_0144;

                default:
                    return flag;
            }
        Label_0144:
            return true;
        }

        protected virtual void InitializeUserInterface()
        {
            this._editor = this.CreateEditor();
            this._editor.Dock = DockStyle.Fill;
            this._editor.DesignModeEnabled = true;
            this._editor.BordersVisible = true;
            this._editor.ScrollBarsEnabled = true;
            this._editor.FlatScrollBars = true;
            this._editor.ScriptEnabled = false;
            this._editor.Border3d = false;
            this._editor.ReadyStateComplete += new EventHandler(this.OnEditorReadyStateComplete);
            base.Controls.Add(this._editor);
            this.OnEditorCreated();
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
            IDocumentDesignerHost service = (IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost));
            service.DesignerActive = true;
            this.EnsureEditor();
            this._editor.Focus();
        }

        void IDocumentView.Deactivate(bool closing)
        {
            if (!closing)
            {
                IDocumentDesignerHost service = (IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost));
                service.DesignerActive = false;
            }
        }

        void IDocumentView.LoadFromDocument(Document document)
        {
            Microsoft.Matrix.Packages.Web.Documents.HtmlDocument document2 = (Microsoft.Matrix.Packages.Web.Documents.HtmlDocument) document;
            this.EnsureEditor();
            ((IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost))).BeginLoad();
            this.OnBeforeLoadFromDocument(document2);
            this.LoadFromDocument(document2);
        }

        bool IDocumentView.SaveToDocument()
        {
            return true;
        }

        bool ISearchableDocumentView.PerformFind(string searchString, FindReplaceOptions options)
        {
            return this.PerformFind(searchString, options);
        }

        bool ISearchableDocumentView.PerformReplace(string searchString, string replaceString, FindReplaceOptions options)
        {
            throw new NotSupportedException();
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            return this.HandleCommand(command);
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            return this.UpdateCommand(command);
        }

        protected virtual void OnBeforeLoadFromDocument(Microsoft.Matrix.Packages.Web.Documents.HtmlDocument document)
        {
        }

        protected virtual void OnEditorCreated()
        {
        }

        private void OnEditorReadyStateComplete(object sender, EventArgs e)
        {
            ((IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost))).EndLoad();
        }

        protected virtual bool PerformFind(string searchString, FindReplaceOptions options)
        {
            return this._editor.Find(searchString, (options & FindReplaceOptions.MatchCase) != FindReplaceOptions.None, (options & FindReplaceOptions.WholeWord) != FindReplaceOptions.None, (options & FindReplaceOptions.SearchUp) != FindReplaceOptions.None);
        }

        protected virtual bool UpdateCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup != typeof(WebCommands))
            {
                if (command.CommandGroup == typeof(GlobalCommands))
                {
                    switch (command.CommandID)
                    {
                        case 100:
                        case 0x65:
                            command.Enabled = true;
                            return true;

                        case 0x66:
                        case 0x68:
                            command.Enabled = false;
                            return true;

                        case 0x67:
                            command.Enabled = this._editor.CanCopy;
                            return true;
                    }
                }
                return flag;
            }
            int commandID = command.CommandID;
            if (commandID <= 0x98)
            {
                switch (commandID)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 100:
                    case 0x65:
                    case 0x66:
                    case 0x67:
                    case 0x68:
                    case 0x69:
                    case 0x6a:
                    case 0x6b:
                    case 0x6c:
                    case 0x6d:
                    case 110:
                    case 0x6f:
                    case 0x70:
                    case 0x71:
                    case 0x72:
                    case 120:
                    case 0x79:
                    case 0x7a:
                    case 140:
                    case 0x8d:
                    case 0x8e:
                    case 0x8f:
                    case 0x90:
                    case 0x91:
                    case 0x92:
                    case 0x93:
                    case 0x94:
                    case 0x95:
                    case 150:
                    case 0x97:
                    case 0x98:
                        goto Label_0147;

                    case 0x73:
                    case 0x74:
                    case 0x75:
                    case 0x76:
                    case 0x77:
                    case 0x7b:
                    case 0x7c:
                    case 0x7d:
                    case 0x7e:
                    case 0x7f:
                    case 0x80:
                    case 0x81:
                    case 130:
                    case 0x83:
                    case 0x84:
                    case 0x85:
                    case 0x86:
                    case 0x87:
                    case 0x88:
                    case 0x89:
                    case 0x8a:
                    case 0x8b:
                        return flag;
                }
                return flag;
            }
            switch (commandID)
            {
                case 200:
                case 0xc9:
                case 0xca:
                case 0xcb:
                case 220:
                    goto Label_0147;

                default:
                    return flag;
            }
        Label_0147:
            command.Enabled = false;
            return true;
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
                return false;
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
                return "Preview";
            }
        }

        DocumentViewType IDocumentView.ViewType
        {
            get
            {
                return DocumentViewType.Preview;
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
                    return selection.Text;
                }
                return string.Empty;
            }
        }

        FindReplaceOptions ISearchableDocumentView.ReplaceSupport
        {
            get
            {
                return FindReplaceOptions.None;
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }

        protected virtual Image ViewImage
        {
            get
            {
                if (viewImage == null)
                {
                    viewImage = new Bitmap(typeof(HtmlPreviewView), "HtmlDesignView.bmp");
                    viewImage.MakeTransparent();
                }
                return viewImage;
            }
        }
    }
}


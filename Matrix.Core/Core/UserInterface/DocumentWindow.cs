namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    public abstract class DocumentWindow : MxForm, ICommandHandler, IToolboxClient, ISelectionContainer, IPropertyBrowserClient
    {
        private ICommandHandlerWithContext _contextCommandHandler;
        private ICommandHandler _designerCommandHandler;
        private bool _discardChanges;
        private Microsoft.Matrix.Core.Documents.Document _document;
        private IDocumentView _documentView;
        private ICommandHandler _viewCommandHandler;
        private Panel _viewContainer;
        private ISelectionContainer _viewSelectionContainer;
        private IToolboxClient _viewToolboxClient;

        public DocumentWindow(IServiceProvider serviceProvider, Microsoft.Matrix.Core.Documents.Document document) : base(serviceProvider)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            this._document = document;
            try
            {
                base.SuspendLayout();
                this.InitializeUserInterface();
                this._documentView = this.CreateDocumentView();
            }
            finally
            {
                base.ResumeLayout(true);
            }
            this._documentView.DocumentChanged += new EventHandler(this.OnDocumentViewDocumentChanged);
            this._viewCommandHandler = this._documentView as ICommandHandler;
            this._viewToolboxClient = this._documentView as IToolboxClient;
            this._viewSelectionContainer = this._documentView as ISelectionContainer;
            IDesigner designer = ((IDesignerHost) serviceProvider.GetService(typeof(IDesignerHost))).GetDesigner(document);
            this._designerCommandHandler = designer as ICommandHandler;
            this._contextCommandHandler = this._document.ProjectItem.Project;
            this.UpdateCaption();
        }

        internal bool ActivateWindow()
        {
            IWindowManager service = (IWindowManager) this.GetService(typeof(IWindowManager));
            if (service == null)
            {
                return false;
            }
            service.ActivateChildForm(this);
            return true;
        }

        internal void CloseWindow(bool discardChanges)
        {
            IWindowManager service = (IWindowManager) this.GetService(typeof(IWindowManager));
            if (service != null)
            {
                this._discardChanges = discardChanges;
                service.CloseChildForm(this);
            }
        }

        protected abstract IDocumentView CreateDocumentView();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._viewCommandHandler = null;
                this._documentView.DocumentChanged -= new EventHandler(this.OnDocumentViewDocumentChanged);
                this.DisposeDocumentView();
                this._documentView = null;
                this._designerCommandHandler = null;
                this._contextCommandHandler = null;
                this._document = null;
            }
            base.Dispose(disposing);
        }

        protected abstract void DisposeDocumentView();
        protected virtual string GetCaption(string displayName)
        {
            if ((displayName == null) || (displayName.Length == 0))
            {
                throw new ArgumentNullException("displayName");
            }
            string str = displayName;
            if (this._document.IsReadOnly)
            {
                str = str + " [Read-Only]";
            }
            if (this.IsDirty)
            {
                str = str + " *";
            }
            return str;
        }

        protected virtual bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 3:
                    {
                        bool flag2 = true;
                        if (this._documentView.IsDirty)
                        {
                            flag2 = this._documentView.SaveToDocument();
                        }
                        if (flag2)
                        {
                            IDocumentManager service = (IDocumentManager) this.GetService(typeof(IDocumentManager));
                            if ((service != null) && service.SaveDocument(this._document, false))
                            {
                                this.UpdateCaption();
                            }
                        }
                        return true;
                    }
                    case 4:
                    {
                        bool flag3 = true;
                        if (this._documentView.IsDirty)
                        {
                            flag3 = this._documentView.SaveToDocument();
                        }
                        if (flag3)
                        {
                            IDocumentManager manager2 = (IDocumentManager) this.GetService(typeof(IDocumentManager));
                            if ((manager2 != null) && manager2.SaveDocument(this._document, true))
                            {
                                this.UpdateCaption();
                            }
                        }
                        return true;
                    }
                    case 5:
                        return flag;

                    case 6:
                    {
                        bool flag5 = true;
                        if (this._documentView.IsDirty)
                        {
                            flag5 = this._documentView.SaveToDocument();
                        }
                        if (flag5)
                        {
                            IDocumentManager manager4 = (IDocumentManager) this.GetService(typeof(IDocumentManager));
                            if (manager4 != null)
                            {
                                manager4.PrintDocument((IPrintableDocument) this._document);
                            }
                        }
                        return true;
                    }
                    case 7:
                    {
                        bool flag4 = true;
                        if (this._documentView.IsDirty)
                        {
                            flag4 = this._documentView.SaveToDocument();
                        }
                        if (flag4)
                        {
                            IDocumentManager manager3 = (IDocumentManager) this.GetService(typeof(IDocumentManager));
                            if (manager3 != null)
                            {
                                manager3.PrintPreviewDocument((IPrintableDocument) this._document);
                            }
                        }
                        return true;
                    }
                    case 200:
                        return true;

                    case 210:
                    {
                        bool flag6 = true;
                        if (this._documentView.IsDirty)
                        {
                            flag6 = this._documentView.SaveToDocument();
                        }
                        if (flag6 && this._document.IsDirty)
                        {
                            IDocumentManager manager5 = (IDocumentManager) this.GetService(typeof(IDocumentManager));
                            if ((manager5 != null) && manager5.SaveDocument(this._document, false))
                            {
                                this.UpdateCaption();
                            }
                        }
                        ((IRunnableDocument) this._document).Run();
                        return true;
                    }
                }
            }
            return flag;
        }

        private void InitializeUserInterface()
        {
            this.Text = this._document.DocumentName;
            base.ClientSize = new Size(600, 400);
            base.MinimumSize = new Size(30, 30);
            base.DockPadding.All = 1;
            base.Icon = new Icon(typeof(DocumentWindow), "DocumentWindow.ico");
            this._viewContainer = new Panel();
            this._viewContainer.Dock = DockStyle.Fill;
            base.Controls.Add(this._viewContainer);
        }

        void ISelectionContainer.SetSelectedObject(object o)
        {
            if (this._viewSelectionContainer != null)
            {
                this._viewSelectionContainer.SetSelectedObject(o);
            }
        }

        void IToolboxClient.OnToolboxDataItemPicked(ToolboxDataItem dataItem)
        {
            if (this._viewToolboxClient != null)
            {
                this._viewToolboxClient.OnToolboxDataItemPicked(dataItem);
            }
        }

        bool IToolboxClient.SupportsToolboxSection(ToolboxSection section)
        {
            return ((this._viewToolboxClient != null) && this._viewToolboxClient.SupportsToolboxSection(section));
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            bool flag = false;
            if (this._designerCommandHandler != null)
            {
                flag = this._designerCommandHandler.HandleCommand(command);
            }
            if (!flag && (this._viewCommandHandler != null))
            {
                flag = this._viewCommandHandler.HandleCommand(command);
            }
            if (!flag)
            {
                flag = this.HandleCommand(command);
            }
            if (!flag)
            {
                flag = this._contextCommandHandler.HandleCommand(command, this._document.ProjectItem);
            }
            return flag;
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            bool flag = false;
            if (this._designerCommandHandler != null)
            {
                flag = this._designerCommandHandler.UpdateCommand(command);
            }
            if (!flag && (this._viewCommandHandler != null))
            {
                flag = this._viewCommandHandler.UpdateCommand(command);
            }
            if (!flag)
            {
                flag = this.UpdateCommand(command);
            }
            if (!flag)
            {
                flag = this._contextCommandHandler.UpdateCommand(command, this._document.ProjectItem);
            }
            return flag;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            bool flag = this._discardChanges;
            this._discardChanges = false;
            base.OnClosing(e);
            if (!e.Cancel)
            {
                if (!flag && !this._documentView.CanDeactivate)
                {
                    e.Cancel = true;
                }
                if ((!e.Cancel && !flag) && this.IsDirty)
                {
                    switch (MessageBox.Show(this, "The file contains changes that have not been saved.\n\nDo you want to save the changes?", this._document.DocumentName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;

                        case DialogResult.Yes:
                        {
                            bool flag2 = true;
                            if (this._documentView.IsDirty)
                            {
                                flag2 = this._documentView.SaveToDocument();
                            }
                            if (flag2)
                            {
                                IDocumentManager service = (IDocumentManager) this.GetService(typeof(IDocumentManager));
                                if ((service != null) && !service.SaveDocument(this._document, false))
                                {
                                    e.Cancel = true;
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                            break;
                        }
                    }
                }
                if (!e.Cancel)
                {
                    this._documentView.Deactivate(true);
                }
            }
        }

        private void OnDocumentViewDocumentChanged(object sender, EventArgs e)
        {
            this.UpdateCaption();
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this._documentView.LoadFromDocument(this._document);
            this._documentView.Activate(false);
        }

        internal bool ShowWindow()
        {
            IWindowManager service = (IWindowManager) this.GetService(typeof(IWindowManager));
            if (service == null)
            {
                return false;
            }
            service.ShowChildForm(this);
            return true;
        }

        protected void UpdateCaption()
        {
            this.Text = this.GetCaption(this._document.ProjectItem.DisplayName);
        }

        protected virtual bool UpdateCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup.Equals(typeof(GlobalCommands)))
            {
                switch (command.CommandID)
                {
                    case 3:
                    case 4:
                        return true;

                    case 5:
                        return flag;

                    case 6:
                    case 7:
                        command.Enabled = this._document is IPrintableDocument;
                        return true;

                    case 200:
                        command.Text = this._documentView.ViewName;
                        command.Glyph = this._documentView.ViewImage;
                        command.Checked = true;
                        return true;

                    case 210:
                        command.Enabled = (this._document is IRunnableDocument) && ((IRunnableDocument) this._document).CanRun;
                        return true;
                }
            }
            return flag;
        }

        public Microsoft.Matrix.Core.Documents.Document Document
        {
            get
            {
                return this._document;
            }
        }

        public IDocumentView DocumentView
        {
            get
            {
                return this._documentView;
            }
        }

        public bool IsDirty
        {
            get
            {
                if (!this._document.IsDirty)
                {
                    return this._documentView.IsDirty;
                }
                return true;
            }
        }

        bool IPropertyBrowserClient.SupportsPropertyBrowser
        {
            get
            {
                IPropertyBrowserClient client = this._documentView as IPropertyBrowserClient;
                return ((client != null) && client.SupportsPropertyBrowser);
            }
        }

        ToolboxSection IToolboxClient.DefaultToolboxSection
        {
            get
            {
                if (this._viewToolboxClient != null)
                {
                    return this._viewToolboxClient.DefaultToolboxSection;
                }
                return null;
            }
        }

        protected Panel ViewContainer
        {
            get
            {
                return this._viewContainer;
            }
        }
    }
}


namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class OpenDocumentsToolWindow : ToolWindow
    {
        private bool _internalChange;
        private TreeNode _openDocsTreeNode;
        private MxTreeView _openDocsTreeView;

        public OpenDocumentsToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            IDocumentManager service = (IDocumentManager) this.GetService(typeof(IDocumentManager));
            if (service != null)
            {
                service.ActiveDocumentChanged += new ActiveDocumentEventHandler(this.OnActiveDocumentChanged);
                service.DocumentOpened += new DocumentEventHandler(this.OnDocumentOpened);
                service.DocumentClosed += new DocumentEventHandler(this.OnDocumentClosed);
                service.DocumentRenamed += new DocumentEventHandler(this.OnDocumentRenamed);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDocumentManager service = (IDocumentManager) this.GetService(typeof(IDocumentManager));
                if (service != null)
                {
                    service.ActiveDocumentChanged -= new ActiveDocumentEventHandler(this.OnActiveDocumentChanged);
                    service.DocumentOpened -= new DocumentEventHandler(this.OnDocumentOpened);
                    service.DocumentClosed -= new DocumentEventHandler(this.OnDocumentClosed);
                    service.DocumentRenamed -= new DocumentEventHandler(this.OnDocumentRenamed);
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._openDocsTreeView = new MxTreeView();
            base.SuspendLayout();
            this._openDocsTreeView.BorderStyle = BorderStyle.None;
            this._openDocsTreeView.Dock = DockStyle.Fill;
            this._openDocsTreeView.HideSelection = false;
            this._openDocsTreeView.ShowRootLines = false;
            this._openDocsTreeView.ShowLines = false;
            this._openDocsTreeView.TabIndex = 0;
            this._openDocsTreeView.BeforeCollapse += new TreeViewCancelEventHandler(this.OnBeforeCollapseOpenDocsTreeView);
            this._openDocsTreeView.NodeDoubleClick += new TreeViewEventHandler(this.OnNodeDoubleClickOpenDocsTreeView);
            this.BackColor = SystemColors.ControlDark;
            base.DockPadding.All = 1;
            base.Controls.Add(this._openDocsTreeView);
            this.Text = "Open Windows";
            base.ShortText = "";
            base.Icon = new Icon(typeof(OpenDocumentsToolWindow), "OpenDocumentsToolWindow.ico");
            base.ResumeLayout(false);
        }

        private void OnActiveDocumentChanged(object sender, ActiveDocumentEventArgs e)
        {
            foreach (DocumentTreeNode node in this._openDocsTreeNode.Nodes)
            {
                if (node.Document == e.NewDocument)
                {
                    this._internalChange = true;
                    try
                    {
                        this._openDocsTreeView.SelectedNode = node;
                        node.EnsureVisible();
                        break;
                    }
                    finally
                    {
                        this._internalChange = false;
                    }
                }
            }
        }

        private void OnBeforeCollapseOpenDocsTreeView(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == this._openDocsTreeNode)
            {
                e.Cancel = true;
            }
        }

        private void OnDocumentClosed(object sender, DocumentEventArgs e)
        {
            foreach (DocumentTreeNode node in this._openDocsTreeNode.Nodes)
            {
                if (node.Document == e.Document)
                {
                    this._openDocsTreeView.Nodes.Remove(node);
                    break;
                }
            }
        }

        private void OnDocumentOpened(object sender, DocumentEventArgs e)
        {
            if (this._openDocsTreeNode == null)
            {
                IDocumentTypeManager service = (IDocumentTypeManager) this.GetService(typeof(IDocumentTypeManager));
                if (service != null)
                {
                    ImageList list = new ImageList();
                    list.ImageSize = new Size(0x10, 0x10);
                    list.ColorDepth = ColorDepth.Depth32Bit;
                    list.Images.Add(base.Icon);
                    foreach (Image image in service.DocumentIcons.Images)
                    {
                        list.Images.Add(image);
                    }
                    this._openDocsTreeView.ImageList = list;
                }
                this._openDocsTreeNode = new TreeNode("Open Document Windows", 0, 0);
                this._openDocsTreeView.Nodes.Add(this._openDocsTreeNode);
            }
            this._internalChange = true;
            try
            {
                DocumentTreeNode node = new DocumentTreeNode(e.Document);
                this._openDocsTreeNode.Nodes.Add(node);
                this._openDocsTreeView.SelectedNode = node;
                node.EnsureVisible();
            }
            finally
            {
                this._internalChange = false;
            }
        }

        private void OnDocumentRenamed(object sender, DocumentEventArgs e)
        {
            foreach (DocumentTreeNode node in this._openDocsTreeNode.Nodes)
            {
                if (node.Document == e.Document)
                {
                    node.UpdateText();
                    break;
                }
            }
        }

        private void OnNodeDoubleClickOpenDocsTreeView(object sender, TreeViewEventArgs e)
        {
            if (!this._internalChange)
            {
                DocumentTreeNode node = e.Node as DocumentTreeNode;
                if (node != null)
                {
                    DocumentWindow service = (DocumentWindow) node.Document.Site.GetService(typeof(DocumentWindow));
                    if (service != null)
                    {
                        service.Activate();
                    }
                }
            }
        }

        private class DocumentTreeNode : TreeNode
        {
            private Microsoft.Matrix.Core.Documents.Document _document;

            public DocumentTreeNode(Microsoft.Matrix.Core.Documents.Document document)
            {
                this._document = document;
                base.ImageIndex = document.ProjectItem.DocumentType.IconIndex + 1;
                base.SelectedImageIndex = base.ImageIndex;
                this.UpdateText();
            }

            public void UpdateText()
            {
                base.Text = this._document.ProjectItem.Caption;
            }

            public Microsoft.Matrix.Core.Documents.Document Document
            {
                get
                {
                    return this._document;
                }
            }
        }
    }
}


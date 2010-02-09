using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Matrix.Core.Documents;
using Microsoft.Matrix.Core.UserInterface;

namespace Microsoft.Matrix.UIComponents
{
    public class OpenDocumentsTabControl:TabControl
    {
        private IServiceProvider _serviceProvider;
        public OpenDocumentsTabControl(IServiceProvider serverProvider)
        {
            if (serverProvider == null) { throw new ArgumentNullException("serviceProvider"); }
            this._serviceProvider = serverProvider;
            IDocumentManager manager = this._serviceProvider.GetService(typeof(IDocumentManager)) as IDocumentManager;
            if (manager != null)
            {
                manager.DocumentOpened += new DocumentEventHandler(this.OnDocumentOpened);
                //manager.DocumentRenamed += new DocumentEventHandler(this.OnDocumentRenamed);
                manager.DocumentClosed += new DocumentEventHandler(this.OnDocumentClosed);
                manager.ActiveDocumentChanged += new ActiveDocumentEventHandler(this.OnActiveDocumentChanged);
            }

            this.TabPlacement = TabPlacement.Top;
            this.Height = this.TabWellHeight + 2;
            this.AwaysShowTab = true;
        }

        private DocumentTabPage GetAssociateTabPage(Document doc)
        {
            foreach (DocumentTabPage page in this.Tabs)
            {
                if (page.Document == doc)
                {
                    return  page;
                }
            }

            return null;
        }

        void OnActiveDocumentChanged(object sender, ActiveDocumentEventArgs e)
        {
            DocumentTabPage page = this.GetAssociateTabPage(e.NewDocument);
            if (page != null)
            {
                this.SelectedTabPage = page;
            }
        }

        void OnDocumentClosed(object sender, DocumentEventArgs e)
        {
            DocumentTabPage page = this.GetAssociateTabPage(e.Document);
            if (page != null)
            {
                this.Tabs.Remove(page);
            }
        }

        //void OnDocumentRenamed(object sender, DocumentEventArgs e)
        //{
        //    DocumentTabPage page = this.GetAssociateTabPage(e.NewDocument);
        //    if (page != null)
        //    {
        //        page.Text = e.Document.DocumentName;
        //    }
        //}

        void OnDocumentOpened(object sender, DocumentEventArgs e)
        {
            DocumentTabPage page = new DocumentTabPage(e.Document);
            page.ToolTipText = e.Document.DocumentPath;
            this.Tabs.Add(page);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            DocumentTabPage page = this.SelectedTabPage as DocumentTabPage;
            DocumentWindow window = page.Document.Site.GetService(typeof(DocumentWindow)) as DocumentWindow;
            if (window != null)
            {
                window.Activate();
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get { return this._serviceProvider; }
        }

        protected object GetService(Type type)
        {
            if (this._serviceProvider != null)
            {
                return ServiceProvider.GetService(type);
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                IDocumentManager manager = this._serviceProvider.GetService(typeof(IDocumentManager)) as IDocumentManager;
                if (manager != null)
                {
                    manager.DocumentOpened -= new DocumentEventHandler(this.OnDocumentOpened);
                    //manager.DocumentRenamed -= new DocumentEventHandler(this.OnDocumentRenamed);
                    manager.DocumentClosed -= new DocumentEventHandler(this.OnDocumentClosed);
                    manager.ActiveDocumentChanged -= new ActiveDocumentEventHandler(this.OnActiveDocumentChanged);
                }

                this._serviceProvider = null;
            }
            base.Dispose(disposing);
        }

    }

    public class DocumentTabPage : TabPage
    {
        private Document _document;
        public Document Document
        {
            get { return this._document; }
            set { this._document = value; }
        }

        public DocumentTabPage(Document document)
        {
            this._document = document;
        }

        public override string Text
        {
            get
            {
                return this._document == null ? "" : this._document.DocumentName;
            }
            set {}
        }
    }
}

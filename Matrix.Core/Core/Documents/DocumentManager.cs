namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    internal sealed class DocumentManager : IDocumentManager, IDesignerEventService, IDisposable
    {
        private ActiveDesignerEventHandler _activeDesignerChangedHandler;
        private DocumentEntry _activeDocument;
        private ActiveDocumentEventHandler _activeDocumentChangedHandler;
        private DesignerEventHandler _designerCreatedHandler;
        private DesignerEventHandler _designerDisposedHandler;
        private DocumentEventHandler _documentClosedHandler;
        private ArrayList _documentList;
        private DocumentMode _documentMode;
        private DocumentEventHandler _documentOpenedHandler;
        private DocumentEventHandler _documentRenamedHandler;
        private EventHandler _selectionChangedHandler;
        private IServiceProvider _serviceProvider;

        event ActiveDocumentEventHandler IDocumentManager.ActiveDocumentChanged
        {
            add
            {
                this._activeDocumentChangedHandler = (ActiveDocumentEventHandler) Delegate.Combine(this._activeDocumentChangedHandler, value);
            }
            remove
            {
                if (this._activeDocumentChangedHandler != null)
                {
                    this._activeDocumentChangedHandler = (ActiveDocumentEventHandler) Delegate.Remove(this._activeDocumentChangedHandler, value);
                }
            }
        }

        event DocumentEventHandler IDocumentManager.DocumentClosed
        {
            add
            {
                this._documentClosedHandler = (DocumentEventHandler) Delegate.Combine(this._documentClosedHandler, value);
            }
            remove
            {
                if (this._documentClosedHandler != null)
                {
                    this._documentClosedHandler = (DocumentEventHandler) Delegate.Remove(this._documentClosedHandler, value);
                }
            }
        }

        event DocumentEventHandler IDocumentManager.DocumentOpened
        {
            add
            {
                this._documentOpenedHandler = (DocumentEventHandler) Delegate.Combine(this._documentOpenedHandler, value);
            }
            remove
            {
                if (this._documentOpenedHandler != null)
                {
                    this._documentOpenedHandler = (DocumentEventHandler) Delegate.Remove(this._documentOpenedHandler, value);
                }
            }
        }

        event DocumentEventHandler IDocumentManager.DocumentRenamed
        {
            add
            {
                this._documentRenamedHandler = (DocumentEventHandler) Delegate.Combine(this._documentRenamedHandler, value);
            }
            remove
            {
                if (this._documentRenamedHandler != null)
                {
                    this._documentRenamedHandler = (DocumentEventHandler) Delegate.Remove(this._documentRenamedHandler, value);
                }
            }
        }

        event ActiveDesignerEventHandler IDesignerEventService.ActiveDesignerChanged
        {
            add
            {
                this._activeDesignerChangedHandler = (ActiveDesignerEventHandler) Delegate.Combine(this._activeDesignerChangedHandler, value);
            }
            remove
            {
                if (this._activeDesignerChangedHandler != null)
                {
                    this._activeDesignerChangedHandler = (ActiveDesignerEventHandler) Delegate.Remove(this._activeDesignerChangedHandler, value);
                }
            }
        }

        event DesignerEventHandler IDesignerEventService.DesignerCreated
        {
            add
            {
                this._designerCreatedHandler = (DesignerEventHandler) Delegate.Combine(this._designerCreatedHandler, value);
            }
            remove
            {
                if (this._designerCreatedHandler != null)
                {
                    this._designerCreatedHandler = (DesignerEventHandler) Delegate.Remove(this._designerCreatedHandler, value);
                }
            }
        }

        event DesignerEventHandler IDesignerEventService.DesignerDisposed
        {
            add
            {
                this._designerDisposedHandler = (DesignerEventHandler) Delegate.Combine(this._designerDisposedHandler, value);
            }
            remove
            {
                if (this._designerDisposedHandler != null)
                {
                    this._designerDisposedHandler = (DesignerEventHandler) Delegate.Remove(this._designerDisposedHandler, value);
                }
            }
        }

        event EventHandler IDesignerEventService.SelectionChanged
        {
            add
            {
                this._selectionChangedHandler = (EventHandler) Delegate.Combine(this._selectionChangedHandler, value);
            }
            remove
            {
                if (this._selectionChangedHandler != null)
                {
                    this._selectionChangedHandler = (EventHandler) Delegate.Combine(this._selectionChangedHandler, value);
                }
            }
        }

        public DocumentManager(IServiceProvider serviceProvider, bool isDebugger)
        {
            this._serviceProvider = serviceProvider;
            this._documentMode = !isDebugger ? DocumentMode.Editing : DocumentMode.Debugging;
        }

        void IDocumentManager.CloseDocument(Document document, bool discardChanges)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            foreach (DocumentEntry entry in this._documentList)
            {
                if (entry.document == document)
                {
                    entry.docWindow.CloseWindow(discardChanges);
                    break;
                }
            }
        }

        DocumentProjectItem IDocumentManager.CreateDocument(Project project, FolderProjectItem parentItem, bool fixedParentItem)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            if (fixedParentItem && (parentItem == null))
            {
                throw new ArgumentNullException("parentItem");
            }
            IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
            string location = null;
            if (parentItem != null)
            {
                location = parentItem.Path;
            }
            AddFileDialog dialog = new AddFileDialog(project, location, fixedParentItem);
            if (service.ShowDialog(dialog) == DialogResult.OK)
            {
                return dialog.NewProjectItem;
            }
            return null;
        }

        Document IDocumentManager.GetDocument(DocumentProjectItem projectItem)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            if (this._documentList != null)
            {
                foreach (DocumentEntry entry in this._documentList)
                {
                    if (projectItem.Equals(entry.document.ProjectItem))
                    {
                        return entry.document;
                    }
                }
            }
            return null;
        }

        Document IDocumentManager.OpenDocument(DocumentProjectItem projectItem, bool readOnly, DocumentViewType initialView)
        {
            if (this._documentList != null)
            {
                foreach (DocumentEntry entry in this._documentList)
                {
                    if (projectItem.Equals(entry.document.ProjectItem))
                    {
                        if (entry.docWindow.ActivateWindow())
                        {
                            return entry.document;
                        }
                        return null;
                    }
                }
            }
            DocumentType documentType = projectItem.DocumentType;
            if (documentType != null)
            {
                IDocumentFactory factory = documentType.Factory;
                if (factory != null)
                {
                    try
                    {
                        DocumentEntry entry2 = new DocumentEntry();
                        entry2.document = factory.CreateDocument(projectItem, readOnly, this._documentMode, initialView, out entry2.docWindow, out entry2.designerHost);
                        if (this._documentList == null)
                        {
                            this._documentList = new ArrayList();
                        }
                        this._documentList.Add(entry2);
                        IServiceContainer service = (IServiceContainer) ((IServiceProvider) entry2.designerHost).GetService(typeof(IServiceContainer));
                        service.AddService(typeof(Document), entry2.document);
                        service.AddService(typeof(DocumentWindow), entry2.docWindow);
                        entry2.docWindow.Activated += new EventHandler(this.OnDocumentWindowActivated);
                        entry2.docWindow.Closed += new EventHandler(this.OnDocumentWindowClosed);
                        try
                        {
                            this.OnDesignerCreated(new DesignerEventArgs(entry2.designerHost));
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            this.OnDocumentOpened(new DocumentEventArgs(entry2.document));
                        }
                        catch (Exception)
                        {
                        }
                        if (entry2.docWindow.ShowWindow())
                        {
                            return entry2.document;
                        }
                    }
                    catch (Exception exception)
                    {
                        ((IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService))).ReportError(exception, "Unable to open the document", false);
                    }
                }
            }
            return null;
        }

        void IDocumentManager.PrintDocument(IPrintableDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            IPrintService service = (IPrintService) this._serviceProvider.GetService(typeof(IPrintService));
            if (service != null)
            {
                service.PrintDocument(document);
            }
        }

        void IDocumentManager.PrintPreviewDocument(IPrintableDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            IPrintService service = (IPrintService) this._serviceProvider.GetService(typeof(IPrintService));
            if (service != null)
            {
                service.PreviewDocument(document);
            }
        }

        bool IDocumentManager.SaveDocument(Document document, bool saveAs)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            bool flag = false;
            if (document.IsReadOnly)
            {
                saveAs = true;
            }
            if (!saveAs)
            {
                try
                {
                    document.Save();
                    flag = true;
                }
                catch (Exception exception)
                {
                    IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                    if (service.ShowMessage(exception.Message + "\r\nDo you want to save the document to another location?", "Unable to save document '" + document.ProjectItem.Caption + "'", MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        saveAs = true;
                    }
                }
            }
            if (saveAs)
            {
                DocumentProjectItem projectItem = document.ProjectItem;
                DocumentProjectItem item = null;
                try
                {
                    item = projectItem.Project.GetSaveAsProjectItem(projectItem);
                    if (item == null)
                    {
                        return flag;
                    }
                    document.SaveAs(item);
                    flag = true;
                    try
                    {
                        this.OnDocumentRenamed(new DocumentEventArgs(document));
                        return flag;
                    }
                    catch (Exception)
                    {
                        return flag;
                    }
                }
                catch (Exception exception3)
                {
                    string str;
                    IMxUIService service2 = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                    if (item != null)
                    {
                        str = "Unable to save the document to '" + item.Caption + "'";
                    }
                    else
                    {
                        str = "Unable to save the document to another location.";
                    }
                    service2.ReportError(exception3, str, false);
                }
            }
            return flag;
        }

        private void OnActiveDesignerChanged(ActiveDesignerEventArgs e)
        {
            if (this._activeDesignerChangedHandler != null)
            {
                this._activeDesignerChangedHandler(this, e);
            }
        }

        private void OnActiveDocumentChanged(ActiveDocumentEventArgs e)
        {
            if (this._activeDocumentChangedHandler != null)
            {
                this._activeDocumentChangedHandler(this, e);
            }
        }

        private void OnDesignerCreated(DesignerEventArgs e)
        {
            if (this._designerCreatedHandler != null)
            {
                this._designerCreatedHandler(this, e);
            }
        }

        private void OnDesignerDisposed(DesignerEventArgs e)
        {
            if (this._designerDisposedHandler != null)
            {
                this._designerDisposedHandler(this, e);
            }
        }

        private void OnDocumentClosed(DocumentEventArgs e)
        {
            if (this._documentClosedHandler != null)
            {
                this._documentClosedHandler(this, e);
            }
        }

        private void OnDocumentOpened(DocumentEventArgs e)
        {
            if (this._documentOpenedHandler != null)
            {
                this._documentOpenedHandler(this, e);
            }
        }

        private void OnDocumentRenamed(DocumentEventArgs e)
        {
            if (this._documentRenamedHandler != null)
            {
                this._documentRenamedHandler(this, e);
            }
        }

        private void OnDocumentWindowActivated(object sender, EventArgs e)
        {
            DocumentEntry entry = this._activeDocument;
            DocumentEntry entry2 = null;
            foreach (DocumentEntry entry3 in this._documentList)
            {
                if (entry3.docWindow == sender)
                {
                    entry2 = entry3;
                    break;
                }
            }
            if (entry != entry2)
            {
                IDesignerHost oldDesigner = null;
                IDesignerHost newDesigner = null;
                Document oldDocument = null;
                Document newDocument = null;
                this._activeDocument = entry2;
                if (this._activeDocument != null)
                {
                    newDesigner = this._activeDocument.designerHost;
                    newDocument = this._activeDocument.document;
                    ISelectionService service = (ISelectionService) newDesigner.GetService(typeof(ISelectionService));
                    if (service != null)
                    {
                        service.SelectionChanged += new EventHandler(this.OnDocumentWindowSelectionChanged);
                    }
                    IMultiViewDocumentWindow docWindow = this._activeDocument.docWindow as IMultiViewDocumentWindow;
                    if (docWindow != null)
                    {
                        docWindow.CurrentViewChanged += new EventHandler(this.OnDocumentWindowViewChanged);
                    }
                }
                if (entry != null)
                {
                    oldDesigner = entry.designerHost;
                    oldDocument = entry.document;
                    ISelectionService service2 = (ISelectionService) oldDesigner.GetService(typeof(ISelectionService));
                    if (service2 != null)
                    {
                        service2.SelectionChanged -= new EventHandler(this.OnDocumentWindowSelectionChanged);
                    }
                    IMultiViewDocumentWindow window2 = entry.docWindow as IMultiViewDocumentWindow;
                    if (window2 != null)
                    {
                        window2.CurrentViewChanged -= new EventHandler(this.OnDocumentWindowViewChanged);
                    }
                }
                this.OnActiveDesignerChanged(new ActiveDesignerEventArgs(oldDesigner, newDesigner));
                this.OnActiveDocumentChanged(new ActiveDocumentEventArgs(oldDocument, newDocument));
                if (this._activeDocument != null)
                {
                    ICommandHandler activeUICommandHandler = (ICommandHandler) sender;
                    ((ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager))).UpdateActiveUICommandHandler(activeUICommandHandler, null);
                }
            }
        }

        private void OnDocumentWindowClosed(object sender, EventArgs e)
        {
            ((ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager))).UpdateActiveUICommandHandler(null, null);
            DocumentEntry entry = null;
            int index = 0;
            foreach (DocumentEntry entry2 in this._documentList)
            {
                if (entry2.docWindow == sender)
                {
                    entry = entry2;
                    break;
                }
                index++;
            }
            if (entry != null)
            {
                this._documentList.RemoveAt(index);
            }
            ISelectionService service = (ISelectionService) ((IServiceProvider) entry.designerHost).GetService(typeof(ISelectionService));
            if (service != null)
            {
                service.SelectionChanged -= new EventHandler(this.OnDocumentWindowSelectionChanged);
            }
            if ((this._activeDocument != null) && (this._activeDocument.docWindow == sender))
            {
                this._activeDocument = null;
                this.OnActiveDesignerChanged(new ActiveDesignerEventArgs(entry.designerHost, null));
                this.OnActiveDocumentChanged(new ActiveDocumentEventArgs(entry.document, null));
            }
            this.OnDocumentClosed(new DocumentEventArgs(entry.document));
            this.OnDesignerDisposed(new DesignerEventArgs(entry.designerHost));
        }

        private void OnDocumentWindowSelectionChanged(object sender, EventArgs e)
        {
            this.OnSelectionChanged(EventArgs.Empty);
        }

        private void OnDocumentWindowViewChanged(object sender, EventArgs e)
        {
            this.OnActiveDesignerChanged(new ActiveDesignerEventArgs(this._activeDocument.designerHost, this._activeDocument.designerHost));
            this.OnActiveDocumentChanged(new ActiveDocumentEventArgs(this._activeDocument.document, this._activeDocument.document));
            ICommandHandler handler1 = (ICommandHandler) sender;
            ((ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager))).UpdateCommands(false);
        }

        private void OnSelectionChanged(EventArgs e)
        {
            if (this._selectionChangedHandler != null)
            {
                this._selectionChangedHandler(this, EventArgs.Empty);
            }
            ((ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager))).UpdateCommands(false);
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }

        Document IDocumentManager.ActiveDocument
        {
            get
            {
                if (this._activeDocument == null)
                {
                    return null;
                }
                return this._activeDocument.document;
            }
        }

        ICollection IDocumentManager.OpenDocuments
        {
            get
            {
                int count = 0;
                if (this._documentList != null)
                {
                    count = this._documentList.Count;
                }
                Document[] documentArray = new Document[count];
                for (int i = 0; i < count; i++)
                {
                    documentArray[i] = ((DocumentEntry) this._documentList[i]).document;
                }
                return documentArray;
            }
        }

        IDesignerHost IDesignerEventService.ActiveDesigner
        {
            get
            {
                if (this._activeDocument == null)
                {
                    return null;
                }
                return this._activeDocument.designerHost;
            }
        }

        DesignerCollection IDesignerEventService.Designers
        {
            get
            {
                int count = 0;
                if (this._documentList != null)
                {
                    count = this._documentList.Count;
                }
                IDesignerHost[] designers = new IDesignerHost[count];
                for (int i = 0; i < count; i++)
                {
                    designers[i] = ((DocumentEntry) this._documentList[i]).designerHost;
                }
                return new DesignerCollection(designers);
            }
        }

        private class DocumentEntry
        {
            public DesignerHost designerHost;
            public Document document;
            public DocumentWindow docWindow;
        }
    }
}


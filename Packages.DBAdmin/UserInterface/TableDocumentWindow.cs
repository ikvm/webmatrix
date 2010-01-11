namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;
    using System.Windows.Forms;

    internal sealed class TableDocumentWindow : MultiViewDocumentWindow
    {
        public TableDocumentWindow(IServiceProvider serviceProvider, Document document, DocumentViewType initialView) : base(serviceProvider, document)
        {
            if (initialView == DocumentViewType.Default)
            {
                initialView = DocumentViewType.Design;
            }
            IDocumentView viewByType = ((IMultiViewDocumentWindow) this).GetViewByType(initialView);
            if (viewByType != null)
            {
                ((IMultiViewDocumentWindow) this).ActivateView(viewByType);
            }
        }

        protected override void CreateDocumentViews()
        {
            DatabaseProject project = (DatabaseProject) base.Document.ProjectItem.Project;
            Database database = project.Database;
            base.AddDocumentView(new TableView(base.ServiceProvider), true);
            if (database is IDataProviderDatabase)
            {
                base.AddDocumentView(new TableDataView(base.ServiceProvider), false);
            }
        }

        protected override BorderStyle ViewBorderStyle
        {
            get
            {
                return BorderStyle.None;
            }
        }
    }
}


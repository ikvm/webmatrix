namespace Microsoft.Matrix.Packages.DBAdmin.Projects
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.IO;
    using System.Windows.Forms;

    public abstract class DatabaseProject : Project
    {
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database _database;
        public static readonly string DataCategory = "Data";

        public DatabaseProject(IProjectFactory factory, IServiceProvider serviceProvider, Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database) : base(factory, serviceProvider)
        {
            this._database = database;
        }

        public override string CombinePath(string path1, string path2)
        {
            throw new NotSupportedException();
        }

        public override string CombineRelativePath(string path1, string path2)
        {
            throw new NotSupportedException();
        }

        internal Column CreateColumn()
        {
            return this._database.CreateColumn();
        }

        protected override DocumentProjectItem CreateDocumentInternal(FolderProjectItem parentItem)
        {
            if (!(parentItem is StoredProcedureGroupProjectItem))
            {
                if (!(parentItem is TableGroupProjectItem))
                {
                    throw new NotSupportedException();
                }
                TableGroupProjectItem item2 = (TableGroupProjectItem) parentItem;
                PromptDialog dialog2 = new PromptDialog(base.ServiceProvider);
                dialog2.Text = "Create New Table";
                dialog2.EntryText = "Enter the name of the new table:";
                IMxUIService service2 = (IMxUIService) base.GetService(typeof(IMxUIService));
            Label_015E:
                if (service2.ShowDialog(dialog2) == DialogResult.OK)
                {
                    string itemName = dialog2.EntryValue.Trim();
                    string identifierPart = this.GetIdentifierPart(itemName);
                    if (!this.ValidateProjectItemName(identifierPart))
                    {
                        service2.ReportError("The name '" + itemName + "' contains invalid characters or is too long.", "Create New Table", false);
                        goto Label_015E;
                    }
                    try
                    {
                        this.Database.Connect();
                        TableCollection tables = this.Database.Tables;
                        if (tables.Contains(identifierPart))
                        {
                            service2.ReportError("A table with the name '" + itemName + "' already exists.", "Create New Table", false);
                            goto Label_015E;
                        }
                        tables.AddNew(identifierPart);
                        return new TableProjectItem(identifierPart);
                    }
                    catch (Exception exception2)
                    {
                        service2.ReportError(exception2, "Error adding table.", false);
                        goto Label_015E;
                    }
                    finally
                    {
                        this.Database.Disconnect();
                    }
                }
                return null;
            }
            StoredProcedureGroupProjectItem item1 = (StoredProcedureGroupProjectItem) parentItem;
            PromptDialog dialog = new PromptDialog(base.ServiceProvider);
            dialog.Text = "Create New Stored Procedure";
            dialog.EntryText = "Enter the name of the new stored procedure:";
            IMxUIService service = (IMxUIService) base.GetService(typeof(IMxUIService));
        Label_004A:
            if (service.ShowDialog(dialog) == DialogResult.OK)
            {
                string str = dialog.EntryValue.Trim();
                string str2 = this.GetIdentifierPart(str);
                if (!this.ValidateProjectItemName(str2))
                {
                    service.ReportError("The name '" + str + "' contains invalid characters or is too long.", "Create New Stored Procedure", false);
                    goto Label_004A;
                }
                try
                {
                    this.Database.Connect();
                    StoredProcedureCollection storedProcedures = this.Database.StoredProcedures;
                    if (storedProcedures.Contains(str2))
                    {
                        service.ReportError("A stored procedure with the name '" + str + "' already exists.", "Create New Stored Procedure", false);
                        goto Label_004A;
                    }
                    storedProcedures.AddNew(str2);
                    return new StoredProcedureProjectItem(str2);
                }
                catch (Exception exception)
                {
                    service.ReportError(exception, "Error adding stored procedure.", false);
                    goto Label_004A;
                }
                finally
                {
                    this.Database.Disconnect();
                }
            }
            return null;
        }

        protected override FolderProjectItem CreateFolderInternal(FolderProjectItem parentItem)
        {
            throw new NotSupportedException();
        }

        protected override bool DeleteProjectItemInternal(ProjectItem item)
        {
            if (item is StoredProcedureProjectItem)
            {
                StoredProcedureProjectItem item2 = (StoredProcedureProjectItem) item;
                try
                {
                    this.Database.StoredProcedures.Delete(item2.Name);
                }
                catch (Exception exception)
                {
                    ((IMxUIService) base.GetService(typeof(IMxUIService))).ReportError(exception, "Error deleting stored procedure.", false);
                    return false;
                }
                try
                {
                    IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
                    Document document = service.GetDocument((DocumentProjectItem) item);
                    if (document != null)
                    {
                        service.CloseDocument(document, true);
                    }
                }
                catch
                {
                }
                return true;
            }
            if (!(item is TableProjectItem))
            {
                throw new NotSupportedException();
            }
            TableProjectItem item3 = (TableProjectItem) item;
            try
            {
                this.Database.Tables.Delete(item3.Name);
            }
            catch (Exception exception2)
            {
                ((IMxUIService) base.GetService(typeof(IMxUIService))).ReportError(exception2, "Error deleting table.", false);
                return false;
            }
            try
            {
                IDocumentManager manager2 = (IDocumentManager) base.GetService(typeof(IDocumentManager));
                Document document2 = manager2.GetDocument((DocumentProjectItem) item);
                if (document2 != null)
                {
                    manager2.CloseDocument(document2, true);
                }
            }
            catch
            {
            }
            return true;
        }

        protected virtual string GetIdentifierPart(string itemName)
        {
            return itemName;
        }

        protected override ProjectItemAttributes GetProjectItemAttributesInternal(ProjectItem projectItem)
        {
            return ProjectItemAttributes.Normal;
        }

        protected override DocumentType GetProjectItemDocumentTypeInternal(ProjectItem projectItem)
        {
            if (projectItem is TableProjectItem)
            {
                IDocumentTypeManager manager = (IDocumentTypeManager) base.GetService(typeof(IDocumentTypeManager));
                if (manager == null)
                {
                    return null;
                }
                return manager.GetDocumentType("@dbtable");
            }
            if (!(projectItem is StoredProcedureProjectItem))
            {
                return null;
            }
            IDocumentTypeManager service = (IDocumentTypeManager) base.GetService(typeof(IDocumentTypeManager));
            if (service == null)
            {
                return null;
            }
            return service.GetDocumentType("@dbsproc");
        }

        protected override string GetProjectItemPathInternal(ProjectItem projectItem)
        {
            if (projectItem is DatabaseProjectItem)
            {
                return ((DatabaseProjectItem) projectItem).Caption;
            }
            if (projectItem is TableProjectItem)
            {
                return ((TableProjectItem) projectItem).Caption;
            }
            if (projectItem is StoredProcedureProjectItem)
            {
                return ((StoredProcedureProjectItem) projectItem).Caption;
            }
            return string.Empty;
        }

        protected override Stream GetProjectItemStream(DocumentProjectItem projectItem, ProjectItemStreamMode mode)
        {
            return null;
        }

        protected override string GetProjectItemUrlInternal(ProjectItem projectItem)
        {
            throw new NotSupportedException();
        }

        public override DocumentProjectItem GetSaveAsProjectItem(DocumentProjectItem item)
        {
            return null;
        }

        public override Document OpenProjectItem(DocumentProjectItem projectItem, bool readOnly, DocumentViewType initialView)
        {
            if ((projectItem is TableProjectItem) || (projectItem is StoredProcedureProjectItem))
            {
                IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
                if (service == null)
                {
                    return null;
                }
                try
                {
                    return service.OpenDocument(projectItem, readOnly, DocumentViewType.Default);
                }
                catch (Exception exception)
                {
                    ((IMxUIService) base.GetService(typeof(IMxUIService))).ReportError(exception, "Error opening document.", false);
                    return null;
                }
            }
            return null;
        }

        protected override ProjectItem ParsePathInternal(string path, bool newFile)
        {
            throw new NotSupportedException();
        }

        public override bool ProjectItemExists(string itemPath)
        {
            throw new NotSupportedException();
        }

        public override bool ValidateProjectItemPath(string filePath, bool createIfNeeded)
        {
            throw new NotSupportedException();
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this._database;
            }
        }

        public override IComparer ProjectItemComparer
        {
            get
            {
                return new DatabaseProjectItemComparer();
            }
        }

        private class DatabaseProjectItemComparer : IComparer
        {
            int IComparer.Compare(object o1, object o2)
            {
                if ((o1 is StoredProcedureProjectItem) && (o2 is StoredProcedureProjectItem))
                {
                    ProjectItem item = (ProjectItem) o1;
                    ProjectItem item2 = (ProjectItem) o2;
                    return string.Compare(item.Caption, item2.Caption);
                }
                if (o1 is TableProjectItem)
                {
                    if (o2 is TableProjectItem)
                    {
                        ProjectItem item3 = (ProjectItem) o1;
                        ProjectItem item4 = (ProjectItem) o2;
                        return string.Compare(item3.Caption, item4.Caption);
                    }
                }
                else if (o1 is StoredProcedureGroupProjectItem)
                {
                    if (o2 is TableGroupProjectItem)
                    {
                        return 1;
                    }
                }
                else if ((o1 is TableGroupProjectItem) && (o2 is StoredProcedureGroupProjectItem))
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}


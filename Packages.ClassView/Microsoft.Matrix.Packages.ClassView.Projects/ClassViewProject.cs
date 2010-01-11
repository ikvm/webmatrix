namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Packages.ClassView.Core;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class ClassViewProject : Project, IClassViewService
    {
        private ClassViewProjectData _projectData;
        private ClassBrowserProjectItem _rootItem;
        private ArrayList _searchResults;
        private string _searchString;
        private TypeSearchTask _searchTask;

        public ClassViewProject(IProjectFactory factory, IServiceProvider serviceProvider) : base(factory, serviceProvider)
        {
            this._rootItem = new ClassBrowserProjectItem(this);
            this._projectData = new ClassViewProjectData();
            this._projectData.Load(serviceProvider);
        }

        public override string CombinePath(string path1, string path2)
        {
            throw new NotSupportedException();
        }

        public override string CombineRelativePath(string path1, string path2)
        {
            throw new NotSupportedException();
        }

        protected override DocumentProjectItem CreateDocumentInternal(FolderProjectItem parentItem)
        {
            throw new NotSupportedException();
        }

        protected override FolderProjectItem CreateFolderInternal(FolderProjectItem parentItem)
        {
            throw new NotSupportedException();
        }

        protected override bool DeleteProjectItemInternal(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._projectData.Save(base.ServiceProvider);
            }
            base.Dispose(disposing);
        }

        protected override ProjectItemAttributes GetProjectItemAttributesInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            return ProjectItemAttributes.Normal;
        }

        protected override DocumentType GetProjectItemDocumentTypeInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem is TypeProjectItem)
            {
                IDocumentTypeManager service = (IDocumentTypeManager) base.GetService(typeof(IDocumentTypeManager));
                if (service != null)
                {
                    return service.GetDocumentType("@type");
                }
            }
            return null;
        }

        protected override string GetProjectItemPathInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem is TypeProjectItem)
            {
                return ((TypeProjectItem) projectItem).Type.FullName;
            }
            return string.Empty;
        }

        protected override Stream GetProjectItemStream(DocumentProjectItem projectItem, ProjectItemStreamMode mode)
        {
            return null;
        }

        protected override string GetProjectItemUrlInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            throw new NotSupportedException();
        }

        public override DocumentProjectItem GetSaveAsProjectItem(DocumentProjectItem item)
        {
            return null;
        }

        void IClassViewService.ShowType(string searchString)
        {
            if ((searchString != null) && (searchString.Length != 0))
            {
                if (this._searchResults == null)
                {
                    this._searchResults = new ArrayList();
                }
                else
                {
                    this._searchResults.Clear();
                }
                this._searchString = searchString;
                if (this._searchTask != null)
                {
                    this._searchTask.Cancel();
                    this._searchTask = null;
                }
                this._searchTask = TypeSearchTask.CreateSearchTask(this.ProjectData, searchString);
                this._searchTask.Start(new AsyncTaskResultPostedEventHandler(this.OnSearchResultsPosted));
            }
        }

        void IClassViewService.ShowType(System.Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            while (!type.IsEnum && (type.GetElementType() != null))
            {
                type = type.GetElementType();
            }
            OpenTypeProjectItem projectItem = new OpenTypeProjectItem(type, this);
            this.OpenProjectItem(projectItem, true, DocumentViewType.Default);
        }

        private void OnSearchResultsPosted(object sender, AsyncTaskResultPostedEventArgs e)
        {
            if ((this._searchTask != null) && (this._searchTask == sender))
            {
                ICollection data = e.Data as ICollection;
                if ((data != null) && (data.Count != 0))
                {
                    foreach (System.Type type in data)
                    {
                        this._searchResults.Add(type);
                    }
                }
                if (e.IsComplete)
                {
                    this._searchTask = null;
                    System.Type type2 = null;
                    if (this._searchResults.Count > 0)
                    {
                        foreach (System.Type type3 in this._searchResults)
                        {
                            if (type3.Name.ToLower() == this._searchString.ToLower())
                            {
                                type2 = type3;
                                break;
                            }
                        }
                    }
                    if (type2 != null)
                    {
                        ((IClassViewService) this).ShowType(type2);
                    }
                    else
                    {
                        IUIService service = (IUIService) base.ServiceProvider.GetService(typeof(IUIService));
                        if (service != null)
                        {
                            service.ShowMessage("No help found for '" + this._searchString + "'", "Help", MessageBoxButtons.OK);
                        }
                    }
                    this._searchString = null;
                }
            }
        }

        public override Document OpenProjectItem(DocumentProjectItem projectItem, bool readOnly, DocumentViewType initialView)
        {
            IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
            if (service == null)
            {
                return null;
            }
            OpenTypeProjectItem item = projectItem as OpenTypeProjectItem;
            if (item == null)
            {
                item = new OpenTypeProjectItem(((TypeProjectItem) projectItem).Type, this);
            }
            return service.OpenDocument(item, true, DocumentViewType.Default);
        }

        protected override Microsoft.Matrix.Core.Projects.ProjectItem ParsePathInternal(string path, bool newFile)
        {
            throw new NotSupportedException();
        }

        public override bool ProjectItemExists(string itemPath)
        {
            throw new NotSupportedException();
        }

        public override bool ValidateProjectItemName(string fileName)
        {
            throw new NotSupportedException();
        }

        public override bool ValidateProjectItemPath(string filePath, bool createIfNeeded)
        {
            throw new NotSupportedException();
        }

        public ClassViewProjectData ProjectData
        {
            get
            {
                return this._projectData;
            }
        }

        public override RootProjectItem ProjectItem
        {
            get
            {
                return this._rootItem;
            }
        }
    }
}


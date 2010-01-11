namespace Microsoft.Matrix.Core.Projects
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects.FileSystem;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    internal sealed class ProjectManager : IProjectManager, ICommandHandler, IDisposable
    {
        private ArrayList _autoInstantiateFactoryList;
        private ArrayList _creatableFactoryList;
        private HybridDictionary _factories;
        private Type _myComputerFactoryType;
        private Project _myComputerProject;
        private ArrayList _openProjects;
        private ProjectEventHandler _projectAddedHandler;
        private ProjectEventHandler _projectClosedHandler;
        private IServiceProvider _serviceProvider;

        event ProjectEventHandler IProjectManager.ProjectAdded
        {
            add
            {
                this._projectAddedHandler = (ProjectEventHandler) Delegate.Combine(this._projectAddedHandler, value);
            }
            remove
            {
                if (this._projectAddedHandler != null)
                {
                    this._projectAddedHandler = (ProjectEventHandler) Delegate.Remove(this._projectAddedHandler, value);
                }
            }
        }

        event ProjectEventHandler IProjectManager.ProjectClosed
        {
            add
            {
                this._projectClosedHandler = (ProjectEventHandler) Delegate.Combine(this._projectClosedHandler, value);
            }
            remove
            {
                if (this._projectClosedHandler != null)
                {
                    this._projectClosedHandler = (ProjectEventHandler) Delegate.Remove(this._projectClosedHandler, value);
                }
            }
        }

        public ProjectManager(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._openProjects = new ArrayList();
            this._autoInstantiateFactoryList = new ArrayList();
            this._creatableFactoryList = new ArrayList();
            this._factories = new HybridDictionary();
        }

        public bool Close()
        {
            if (this._openProjects != null)
            {
                IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
                service.ResetPreferencesStore(typeof(ProjectManager));
                PreferencesStore preferencesStore = null;
                int num = 0;
                foreach (Project project in this._openProjects)
                {
                    if (project.ProjectFactory.SaveMode == ProjectFactorySaveMode.SaveToPreferences)
                    {
                        if (preferencesStore == null)
                        {
                            preferencesStore = service.GetPreferencesStore(typeof(ProjectManager));
                        }
                        preferencesStore.SetValue("Project" + num, new ProjectSaveData(project));
                        num++;
                    }
                }
                if (num != 0)
                {
                    preferencesStore.SetValue("ProjectCount", num, 0);
                }
            }
            return true;
        }

        private Project CreateProject(Type projectType)
        {
            ArrayList projectFactories = this.GetProjectFactories(projectType);
            if (projectFactories.Count != 0)
            {
                AddProjectDialog dialog = new AddProjectDialog(this._serviceProvider, projectFactories, null);
                IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                if (service.ShowDialog(dialog) == DialogResult.OK)
                {
                    return this.CreateProject(dialog.SelectedFactory, null);
                }
            }
            return null;
        }

        private Project CreateProject(IProjectFactory factory, object creationArgs)
        {
            Project project = null;
            if (factory.IsSingleInstance)
            {
                foreach (Project project2 in this._openProjects)
                {
                    if (project2.ProjectFactory == factory)
                    {
                        project = project2;
                        break;
                    }
                }
            }
            if (project == null)
            {
                try
                {
                    project = factory.CreateProject(creationArgs);
                }
                catch (Exception exception)
                {
                    IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                    if (service != null)
                    {
                        service.ReportError(exception.Message, "Unable to create a new " + factory.Name + " workspace.", false);
                    }
                }
                if (project == null)
                {
                    return project;
                }
                this._openProjects.Add(project);
                if ((this._myComputerProject == null) && (factory.GetType() == this._myComputerFactoryType))
                {
                    this._myComputerProject = project;
                }
                try
                {
                    ProjectEventArgs e = new ProjectEventArgs(project);
                    this.OnProjectAdded(e);
                }
                catch (Exception)
                {
                }
            }
            return project;
        }

        private ArrayList GetProjectFactories(Type projectType)
        {
            if (projectType == null)
            {
                return this._creatableFactoryList;
            }
            ArrayList list = new ArrayList();
            foreach (IProjectFactory factory in this._creatableFactoryList)
            {
                if (projectType.IsAssignableFrom(factory.ProjectType))
                {
                    list.Add(factory);
                }
            }
            return list;
        }

        public void Initialize()
        {
            PreferencesStore store;
            ICommandManager manager = (ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager));
            if (manager != null)
            {
                manager.AddGlobalCommandHandler(this);
            }
            if (this._autoInstantiateFactoryList.Count != 0)
            {
                foreach (IProjectFactory factory in this._autoInstantiateFactoryList)
                {
                    this.CreateProject(factory, null);
                }
            }
            IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
            if (service.GetPreferencesStore(typeof(ProjectManager), out store))
            {
                int num = store.GetValue("ProjectCount", 0);
                for (int i = 0; i < num; i++)
                {
                    try
                    {
                        ProjectSaveData data = (ProjectSaveData) store.GetValue("Project" + i);
                        ((IProjectManager) this).CreateProject(data.FactoryType, data.Data);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public void LoadProjectTypes()
        {
            ICollection config = (ICollection) ConfigurationSettings.GetConfig("microsoft.matrix/projects");
            if (config != null)
            {
                foreach (string str in config)
                {
                    try
                    {
                        Type type = Type.GetType(str, true);
                        if (type == typeof(MyComputerProjectFactory))
                        {
                            this._myComputerFactoryType = type;
                        }
                        IProjectFactory factory = (IProjectFactory) Activator.CreateInstance(type);
                        this._factories.Add(type, factory);
                        factory.Initialize(this._serviceProvider);
                        if (factory.AutoInstantiate)
                        {
                            this._autoInstantiateFactoryList.Add(factory);
                        }
                        if (factory.SaveMode != ProjectFactorySaveMode.None)
                        {
                            this._creatableFactoryList.Add(factory);
                        }
                        continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        bool IProjectManager.CloseProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            if (!this._openProjects.Contains(project))
            {
                throw new ArgumentException("Unknown project", "project");
            }
            bool flag = true;
            IDocumentManager service = (IDocumentManager) this._serviceProvider.GetService(typeof(IDocumentManager));
            if (service != null)
            {
                bool flag2 = false;
                foreach (Document document in service.OpenDocuments)
                {
                    if (document.ProjectItem.Project == project)
                    {
                        flag2 = true;
                        service.CloseDocument(document, false);
                    }
                }
                if (flag2)
                {
                    foreach (Document document2 in service.OpenDocuments)
                    {
                        if (document2.ProjectItem.Project == project)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            if (flag)
            {
                this.OnProjectClosed(new ProjectEventArgs(project));
                this._openProjects.Remove(project);
                try
                {
                    ((IDisposable) project).Dispose();
                }
                catch (Exception)
                {
                }
            }
            return flag;
        }

        Project IProjectManager.CreateProject(Type projectType)
        {
            return this.CreateProject(projectType);
        }

        Project IProjectManager.CreateProject(Type factoryType, object creationArgs)
        {
            IProjectFactory factory = (IProjectFactory) this._factories[factoryType];
            if (factory == null)
            {
                throw new ArgumentException("A project factory of the specified type has not been registered");
            }
            return this.CreateProject(factory, creationArgs);
        }

        ICollection IProjectManager.GetProjectFactories(Type projectType)
        {
            return this.GetProjectFactories(projectType);
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 40:
                    case 0x29:
                    case 0x2a:
                    case 0x2b:
                    case 0x2c:
                        return true;

                    case 10:
                        this.CreateProject(null);
                        return true;
                }
            }
            return false;
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 40:
                        command.Enabled = false;
                        command.Text = "(Empty)";
                        return true;

                    case 0x29:
                    case 0x2a:
                    case 0x2b:
                    case 0x2c:
                        command.Enabled = false;
                        command.Visible = false;
                        return true;

                    case 10:
                        command.Enabled = this._creatableFactoryList.Count != 0;
                        return true;
                }
            }
            return false;
        }

        private void OnProjectAdded(ProjectEventArgs e)
        {
            if (this._projectAddedHandler != null)
            {
                this._projectAddedHandler(this, e);
            }
        }

        private void OnProjectClosed(ProjectEventArgs e)
        {
            if (this._projectClosedHandler != null)
            {
                this._projectClosedHandler(this, e);
            }
        }

        void IDisposable.Dispose()
        {
            if (this._serviceProvider != null)
            {
                ICommandManager service = (ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager));
                if (service != null)
                {
                    service.RemoveGlobalCommandHandler(this);
                }
            }
            if (this._openProjects != null)
            {
                foreach (IDisposable disposable in this._openProjects)
                {
                    if (disposable != this._myComputerProject)
                    {
                        disposable.Dispose();
                    }
                }
                this._openProjects = null;
            }
            if (this._factories != null)
            {
                foreach (IDisposable disposable2 in this._factories.Values)
                {
                    if (disposable2.GetType() != this._myComputerFactoryType)
                    {
                        disposable2.Dispose();
                    }
                }
                this._creatableFactoryList = null;
                this._factories = null;
            }
            if (this._myComputerProject != null)
            {
                IProjectFactory projectFactory = this._myComputerProject.ProjectFactory;
                ((IDisposable) this._myComputerProject).Dispose();
                projectFactory.Dispose();
                this._myComputerProject = null;
            }
            this._myComputerFactoryType = null;
            this._serviceProvider = null;
        }

        Project IProjectManager.ActiveProject
        {
            get
            {
                return null;
            }
        }

        Project IProjectManager.LocalFileSystemProject
        {
            get
            {
                if ((this._myComputerProject == null) && (this._myComputerFactoryType != null))
                {
                    ((IProjectManager) this).CreateProject(this._myComputerFactoryType, null);
                }
                return this._myComputerProject;
            }
        }

        ICollection IProjectManager.OpenProjects
        {
            get
            {
                return ArrayList.ReadOnly(this._openProjects);
            }
        }

        [Serializable]
        private sealed class ProjectSaveData : ISerializable
        {
            private object _data;
            private string _factoryTypeName;

            public ProjectSaveData(Project project)
            {
                IProjectFactory projectFactory = project.ProjectFactory;
                Type type = projectFactory.GetType();
                this._data = projectFactory.SaveProject(project);
                this._factoryTypeName = type.FullName + ", " + type.Assembly.GetName().Name;
            }

            private ProjectSaveData(SerializationInfo info, StreamingContext context)
            {
                this._data = info.GetValue("Data", typeof(object));
                this._factoryTypeName = (string) info.GetValue("Factory", typeof(string));
            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Data", this._data);
                info.AddValue("Factory", this._factoryTypeName);
            }

            public object Data
            {
                get
                {
                    return this._data;
                }
            }

            public Type FactoryType
            {
                get
                {
                    return Type.GetType(this._factoryTypeName);
                }
            }
        }
    }
}


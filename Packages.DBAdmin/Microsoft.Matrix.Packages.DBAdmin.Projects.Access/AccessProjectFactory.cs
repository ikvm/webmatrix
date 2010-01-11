namespace Microsoft.Matrix.Packages.DBAdmin.Projects.Access
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.UserInterface.Access;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public sealed class AccessProjectFactory : IProjectFactory, IDisposable
    {
        private IServiceProvider _serviceProvider;

        private DatabaseProject CreateProject(Database database)
        {
            return new AccessDatabaseProject(this, this._serviceProvider, (AccessDatabase) database);
        }

        Project IProjectFactory.CreateProject(object creationArgs)
        {
            AccessDatabase database = null;
            if (creationArgs != null)
            {
                AccessConnectionSettings connectionSettings = creationArgs as AccessConnectionSettings;
                if (connectionSettings == null)
                {
                    throw new ArgumentException("Invalid creationArgs");
                }
                database = new AccessDatabase(connectionSettings);
                return this.CreateProject(database);
            }
            IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
            if (service != null)
            {
                if (!AccessHelper.IsAdoxPresent())
                {
                    AccessHelper.PromptInstallAdox(this._serviceProvider);
                    return null;
                }
                AccessDatabaseLoginForm form = new AccessDatabaseLoginForm(this._serviceProvider);
                if (service.ShowDialog(form) == DialogResult.OK)
                {
                    database = new AccessDatabase(form.ConnectionSettings);
                    return this.CreateProject(database);
                }
            }
            return null;
        }

        void IProjectFactory.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        object IProjectFactory.SaveProject(Project project)
        {
            AccessDatabaseProject project2 = project as AccessDatabaseProject;
            if (project2 == null)
            {
                throw new ArgumentException("Unexpected project type");
            }
            return ((AccessDatabase) project2.Database).ConnectionSettings;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }

        bool IProjectFactory.AutoInstantiate
        {
            get
            {
                return false;
            }
        }

        string IProjectFactory.Category
        {
            get
            {
                return DatabaseProject.DataCategory;
            }
        }

        string IProjectFactory.CreateNewDescription
        {
            get
            {
                return "Create a new Access database project.";
            }
        }

        bool IProjectFactory.IsSingleInstance
        {
            get
            {
                return false;
            }
        }

        Icon IProjectFactory.LargeIcon
        {
            get
            {
                return new Icon(typeof(AccessDatabaseProject), "AccessProjectLargeIcon.ico");
            }
        }

        string IProjectFactory.Name
        {
            get
            {
                return "Access Database";
            }
        }

        System.Type IProjectFactory.ProjectType
        {
            get
            {
                return typeof(AccessDatabaseProject);
            }
        }

        ProjectFactorySaveMode IProjectFactory.SaveMode
        {
            get
            {
                return ProjectFactorySaveMode.SaveToPreferences;
            }
        }

        Icon IProjectFactory.SmallIcon
        {
            get
            {
                return new Icon(typeof(AccessDatabaseProject), "AccessProjectSmallIcon.ico");
            }
        }
    }
}


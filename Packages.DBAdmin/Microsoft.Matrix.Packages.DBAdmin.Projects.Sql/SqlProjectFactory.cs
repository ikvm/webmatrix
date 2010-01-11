namespace Microsoft.Matrix.Packages.DBAdmin.Projects.Sql
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.UserInterface.Sql;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public sealed class SqlProjectFactory : IProjectFactory, IDisposable
    {
        private IServiceProvider _serviceProvider;

        private DatabaseProject CreateProject(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database)
        {
            return new SqlDatabaseProject(this, this._serviceProvider, (SqlDatabase) database);
        }

        Project IProjectFactory.CreateProject(object creationArgs)
        {
            SqlDatabase database = null;
            if (creationArgs != null)
            {
                SqlConnectionSettings connectionSettings = creationArgs as SqlConnectionSettings;
                if (connectionSettings == null)
                {
                    throw new ArgumentException("Invalid creationArgs");
                }
                database = new SqlDatabase(connectionSettings);
                return this.CreateProject(database);
            }
            IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
            if (service != null)
            {
                if (!SqlHelper.IsDmoPresent())
                {
                    SqlHelper.PromptInstallDmo(this._serviceProvider);
                    return null;
                }
                SqlDatabaseLoginForm form = new SqlDatabaseLoginForm(this._serviceProvider);
                if (service.ShowDialog(form) == DialogResult.OK)
                {
                    database = new SqlDatabase(form.ConnectionSettings);
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
            SqlDatabaseProject project2 = project as SqlDatabaseProject;
            if (project2 == null)
            {
                throw new ArgumentException("Unexpected project type");
            }
            return ((SqlDatabase) project2.Database).ConnectionSettings;
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
                return "Create a new SQL database project.";
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
                return new Icon(typeof(SqlDatabaseProject), "SqlProjectLargeIcon.ico");
            }
        }

        string IProjectFactory.Name
        {
            get
            {
                return "SQL Server/MSDE Database";
            }
        }

        System.Type IProjectFactory.ProjectType
        {
            get
            {
                return typeof(SqlDatabaseProject);
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
                return new Icon(typeof(SqlDatabaseProject), "SqlProjectSmallIcon.ico");
            }
        }
    }
}


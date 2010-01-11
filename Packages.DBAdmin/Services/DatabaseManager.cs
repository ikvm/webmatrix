namespace Microsoft.Matrix.Packages.DBAdmin.Services
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;
    using System.Collections;

    internal sealed class DatabaseManager : IDatabaseManager, IDisposable
    {
        private IServiceProvider _serviceProvider;

        public DatabaseManager(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        Database IDatabaseManager.CreateDatabaseConnection()
        {
            IProjectManager service = (IProjectManager) this._serviceProvider.GetService(typeof(IProjectManager));
            if (service != null)
            {
                DatabaseProject project = (DatabaseProject) service.CreateProject(typeof(DatabaseProject));
                if (project != null)
                {
                    return project.Database;
                }
            }
            return null;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }

        ICollection IDatabaseManager.DatabaseConnections
        {
            get
            {
                ArrayList list = new ArrayList();
                IProjectManager service = (IProjectManager) this._serviceProvider.GetService(typeof(IProjectManager));
                if (service != null)
                {
                    foreach (Project project in service.OpenProjects)
                    {
                        DatabaseProject project2 = project as DatabaseProject;
                        if (project2 != null)
                        {
                            list.Add(project2.Database);
                        }
                    }
                }
                return list;
            }
        }
    }
}


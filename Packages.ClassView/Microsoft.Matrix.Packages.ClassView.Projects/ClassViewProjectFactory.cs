namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Drawing;

    public sealed class ClassViewProjectFactory : IProjectFactory, IDisposable
    {
        private IServiceProvider _serviceProvider;

        Project IProjectFactory.CreateProject(object creationArgs)
        {
            if (creationArgs != null)
            {
                throw new ArgumentException("creationArgs must be null");
            }
            return new ClassViewProject(this, this._serviceProvider);
        }

        void IProjectFactory.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        object IProjectFactory.SaveProject(Project project)
        {
            throw new NotSupportedException();
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
                return null;
            }
        }

        string IProjectFactory.CreateNewDescription
        {
            get
            {
                return null;
            }
        }

        bool IProjectFactory.IsSingleInstance
        {
            get
            {
                return true;
            }
        }

        Icon IProjectFactory.LargeIcon
        {
            get
            {
                return null;
            }
        }

        string IProjectFactory.Name
        {
            get
            {
                return "Class Browser";
            }
        }

        Type IProjectFactory.ProjectType
        {
            get
            {
                return typeof(ClassViewProject);
            }
        }

        ProjectFactorySaveMode IProjectFactory.SaveMode
        {
            get
            {
                return ProjectFactorySaveMode.None;
            }
        }

        Icon IProjectFactory.SmallIcon
        {
            get
            {
                return null;
            }
        }
    }
}


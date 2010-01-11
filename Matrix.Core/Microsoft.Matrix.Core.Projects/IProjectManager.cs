namespace Microsoft.Matrix.Core.Projects
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public interface IProjectManager
    {
        event ProjectEventHandler ProjectAdded;

        event ProjectEventHandler ProjectClosed;

        bool CloseProject(Project project);
        Project CreateProject(Type projectType);
        Project CreateProject(Type factoryType, object creationArgs);
        ICollection GetProjectFactories(Type projectType);

        Project ActiveProject { get; }

        Project LocalFileSystemProject { get; }

        ICollection OpenProjects { get; }
    }
}


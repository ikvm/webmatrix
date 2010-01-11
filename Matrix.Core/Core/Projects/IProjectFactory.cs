namespace Microsoft.Matrix.Core.Projects
{
    using System;
    using System.Drawing;

    public interface IProjectFactory : IDisposable
    {
        Project CreateProject(object creationArgs);
        void Initialize(IServiceProvider serviceProvider);
        object SaveProject(Project project);

        bool AutoInstantiate { get; }

        string Category { get; }

        string CreateNewDescription { get; }

        bool IsSingleInstance { get; }

        Icon LargeIcon { get; }

        string Name { get; }

        Type ProjectType { get; }

        ProjectFactorySaveMode SaveMode { get; }

        Icon SmallIcon { get; }
    }
}


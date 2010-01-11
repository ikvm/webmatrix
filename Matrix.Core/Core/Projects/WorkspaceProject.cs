namespace Microsoft.Matrix.Core.Projects
{
    using System;

    public abstract class WorkspaceProject : Project
    {
        public WorkspaceProject(IProjectFactory factory, IServiceProvider serviceProvider) : base(factory, serviceProvider)
        {
        }
    }
}


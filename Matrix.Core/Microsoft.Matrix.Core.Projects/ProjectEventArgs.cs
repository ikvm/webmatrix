namespace Microsoft.Matrix.Core.Projects
{
    using System;

    public class ProjectEventArgs : EventArgs
    {
        private Microsoft.Matrix.Core.Projects.Project _project;

        public ProjectEventArgs(Microsoft.Matrix.Core.Projects.Project project)
        {
            this._project = project;
        }

        public Microsoft.Matrix.Core.Projects.Project Project
        {
            get
            {
                return this._project;
            }
        }
    }
}


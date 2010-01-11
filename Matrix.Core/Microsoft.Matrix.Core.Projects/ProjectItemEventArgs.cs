namespace Microsoft.Matrix.Core.Projects
{
    using System;

    public class ProjectItemEventArgs : EventArgs
    {
        private Microsoft.Matrix.Core.Projects.ProjectItem _projectItem;

        public ProjectItemEventArgs(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            this._projectItem = projectItem;
        }

        public Microsoft.Matrix.Core.Projects.ProjectItem ProjectItem
        {
            get
            {
                return this._projectItem;
            }
        }
    }
}


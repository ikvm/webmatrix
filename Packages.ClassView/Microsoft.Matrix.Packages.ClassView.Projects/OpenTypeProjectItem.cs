namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;

    internal sealed class OpenTypeProjectItem : TypeProjectItem
    {
        private ClassViewProject _project;

        public OpenTypeProjectItem(Type type, ClassViewProject project) : base(type)
        {
            this._project = project;
        }

        public override Microsoft.Matrix.Core.Projects.Project Project
        {
            get
            {
                return this._project;
            }
        }
    }
}


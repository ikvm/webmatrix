namespace Microsoft.Matrix.Core.Projects
{
    using System;

    public abstract class RootProjectItem : FolderProjectItem
    {
        private Microsoft.Matrix.Core.Projects.Project _owningProject;

        public RootProjectItem(Microsoft.Matrix.Core.Projects.Project owningProject, string caption) : base(caption)
        {
            if (owningProject == null)
            {
                throw new ArgumentNullException();
            }
            this._owningProject = owningProject;
        }

        internal override ProjectItem ParentInternal
        {
            get
            {
                return null;
            }
        }

        public override Microsoft.Matrix.Core.Projects.Project Project
        {
            get
            {
                return this._owningProject;
            }
        }

        public virtual bool ShowChildrenByDefault
        {
            get
            {
                return true;
            }
        }
    }
}


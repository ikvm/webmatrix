namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;

    internal sealed class GroupProjectItem : FolderProjectItem
    {
        private ClassViewProjectData.GroupEntry _group;

        public GroupProjectItem(ClassViewProjectData.GroupEntry group) : base(group.Name)
        {
            this._group = group;
            base.SetIconIndex(1);
        }

        protected override void CreateChildItems()
        {
            foreach (ClassViewProjectData.GroupItem item in this._group.Items)
            {
                string name = item.Name;
                Type type = Type.GetType(item.TypeName, false);
                if (type != null)
                {
                    TypeProjectItem item2 = new TypeProjectItem(name, type);
                    item2.EnsureTypeInformation();
                    base.AddChildItem(item2);
                }
            }
            base.CreateChildItems();
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }
    }
}


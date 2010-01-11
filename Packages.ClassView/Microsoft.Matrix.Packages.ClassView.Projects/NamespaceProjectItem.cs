namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;

    internal sealed class NamespaceProjectItem : FolderProjectItem
    {
        private ArrayList _typeItems;

        public NamespaceProjectItem(string nameSpace) : base(nameSpace)
        {
            base.SetIconIndex(3);
            this._typeItems = new ArrayList();
        }

        public TypeProjectItem AddType(Type type)
        {
            TypeProjectItem item = new TypeProjectItem(type);
            this._typeItems.Add(item);
            return item;
        }

        protected override void CreateChildItems()
        {
            if ((this._typeItems != null) && (this._typeItems.Count != 0))
            {
                foreach (TypeProjectItem item in this._typeItems)
                {
                    item.EnsureTypeInformation();
                }
                ClassViewProjectSortMode sortMode = ((ClassViewProject) this.Project).ProjectData.SortMode;
                this._typeItems.Sort(TypeProjectItem.GetComparer(sortMode));
                foreach (TypeProjectItem item2 in this._typeItems)
                {
                    base.AddChildItem(item2);
                }
                this._typeItems = null;
            }
            base.CreateChildItems();
        }

        public override string Description
        {
            get
            {
                return ("namespace\r\n" + this.Namespace);
            }
        }

        public string Namespace
        {
            get
            {
                return base.Caption;
            }
        }

        public sealed class NamespaceProjectItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.Compare(((NamespaceProjectItem) x).Namespace, ((NamespaceProjectItem) y).Namespace);
            }
        }
    }
}


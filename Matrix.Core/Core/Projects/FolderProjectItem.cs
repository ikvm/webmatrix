namespace Microsoft.Matrix.Core.Projects
{
    using System;

    public abstract class FolderProjectItem : ProjectItem
    {
        public FolderProjectItem(string caption) : base(caption)
        {
            base.SetIconIndex(1);
            base.SetFlags(ProjectItemFlags.SupportsAddItem, true);
        }

        public DocumentProjectItem AddDocument()
        {
            if (!base.SupportsAddItem)
            {
                throw new NotSupportedException("This folder does not support creating new documents.");
            }
            ProjectItemCollection childItems = base.ChildItems;
            DocumentProjectItem item = this.Project.CreateDocumentInternal(this);
            if (item != null)
            {
                base.AddChildItem(item);
            }
            return item;
        }

        public FolderProjectItem AddFolder()
        {
            if (!base.SupportsAddItem)
            {
                throw new NotSupportedException("This folder does not support creating new folders.");
            }
            ProjectItemCollection childItems = base.ChildItems;
            FolderProjectItem item = this.Project.CreateFolderInternal(this);
            if (item != null)
            {
                base.AddChildItem(item);
            }
            return item;
        }
    }
}


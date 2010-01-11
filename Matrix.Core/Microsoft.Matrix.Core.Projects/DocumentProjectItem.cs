namespace Microsoft.Matrix.Core.Projects
{
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.IO;

    public abstract class DocumentProjectItem : ProjectItem
    {
        public DocumentProjectItem(string caption) : base(caption)
        {
            base.SetFlags(ProjectItemFlags.NonExpandable, true);
            base.SetIconIndex(7);
        }

        public Stream GetStream(ProjectItemStreamMode mode)
        {
            if ((mode < ProjectItemStreamMode.Read) || (mode > ProjectItemStreamMode.Write))
            {
                throw new ArgumentOutOfRangeException("mode");
            }
            return this.Project.GetProjectItemStream(this, mode);
        }

        public Microsoft.Matrix.Core.Documents.DocumentType DocumentType
        {
            get
            {
                return this.Project.GetProjectItemDocumentTypeInternal(this);
            }
        }
    }
}


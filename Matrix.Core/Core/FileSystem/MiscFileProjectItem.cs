namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix.Core.Projects;
    using System;
using System.Text;

    internal sealed class MiscFileProjectItem : FileProjectItem, IMiscProjectItem
    {
        private string _path;
        private Microsoft.Matrix.Core.Projects.Project _project;
        private Encoding encoding = Encoding.UTF8;

        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public MiscFileProjectItem(string caption, string path, Microsoft.Matrix.Core.Projects.Project project) : base(caption)
        {
            this._project = project;
            this._path = path;
        }

        public override string Path
        {
            get
            {
                return this._path;
            }
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


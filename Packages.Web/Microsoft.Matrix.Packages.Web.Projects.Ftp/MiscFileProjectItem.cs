namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using Microsoft.Matrix.Core.Projects;
    using System;

    internal sealed class MiscFileProjectItem : FileProjectItem
    {
        private string _path;
        private FtpProject _project;

        public MiscFileProjectItem(string caption, string path, FtpProject project) : base(caption)
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


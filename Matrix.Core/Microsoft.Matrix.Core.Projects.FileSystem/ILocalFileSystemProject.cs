namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using System;

    public interface ILocalFileSystemProject
    {
        int MruDocumentsLength { get; set; }
    }
}


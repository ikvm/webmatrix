namespace Microsoft.Matrix.Packages.DBAdmin.Projects
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;

    public interface IDataProviderDatabaseProject
    {
        IDataProviderDatabase Database { get; }
    }
}


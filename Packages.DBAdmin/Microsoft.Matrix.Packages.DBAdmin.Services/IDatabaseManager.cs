namespace Microsoft.Matrix.Packages.DBAdmin.Services
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System.Collections;

    public interface IDatabaseManager
    {
        Database CreateDatabaseConnection();

        ICollection DatabaseConnections { get; }
    }
}


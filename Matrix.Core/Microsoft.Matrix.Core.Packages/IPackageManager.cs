namespace Microsoft.Matrix.Core.Packages
{
    using System;
    using System.Collections;

    public interface IPackageManager
    {
        IPackage GetPackage(Type packageType);

        ICollection Packages { get; }
    }
}


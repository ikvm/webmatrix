namespace Microsoft.Matrix.Core.Packages
{
    using Microsoft.Matrix.Core.UserInterface;
    using System;

    public interface IPackage : IDisposable
    {
        OptionsPage[] GetOptionsPages();
        void Initialize(IServiceProvider serviceProvider);
    }
}


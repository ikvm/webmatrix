namespace Microsoft.Matrix.Packages.Community
{
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.Core.UserInterface;
    using System;

    public sealed class CommunityPackage : IPackage, IDisposable
    {
        private IServiceProvider _serviceProvider;

        OptionsPage[] IPackage.GetOptionsPages()
        {
            return null;
        }

        void IPackage.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }
    }
}


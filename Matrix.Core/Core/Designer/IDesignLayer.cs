namespace Microsoft.Matrix.Core.Designer
{
    using System;

    public interface IDesignLayer : IServiceProvider, IDisposable
    {
        string Name { get; }
    }
}


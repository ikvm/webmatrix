namespace Microsoft.Matrix.Core.Services
{
    using System;

    public interface IClassViewService
    {
        void ShowType(string searchString);
        void ShowType(Type type);
    }
}


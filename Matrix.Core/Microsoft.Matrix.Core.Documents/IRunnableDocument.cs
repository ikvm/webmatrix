namespace Microsoft.Matrix.Core.Documents
{
    using System;

    public interface IRunnableDocument
    {
        void Run();

        bool CanRun { get; }
    }
}


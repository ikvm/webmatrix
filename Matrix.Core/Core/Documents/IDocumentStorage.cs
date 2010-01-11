namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.IO;

    public interface IDocumentStorage : IDisposable
    {
        void Load(Stream contentStream);
        void Save(Stream contentStream);
    }
}


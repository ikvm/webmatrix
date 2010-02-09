namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.IO;
using System.Text;

    public interface IDocumentStorage : IDisposable
    {
        void Load(Stream contentStream);
        void Save(Stream contentStream);
    }
}


namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface IDocumentView
    {
        event EventHandler DocumentChanged;

        void Activate(bool viewSwitch);
        void Deactivate(bool closing);
        void LoadFromDocument(Document document);
        bool SaveToDocument();

        bool CanDeactivate { get; }

        bool IsDirty { get; }

        Image ViewImage { get; }

        string ViewName { get; }

        DocumentViewType ViewType { get; }
    }
}


namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.Runtime.CompilerServices;

    public interface IMultiViewDocumentWindow
    {
        event EventHandler CurrentViewChanged;

        void ActivateView(IDocumentView documentView);
        IDocumentView GetViewByIndex(int index);
        IDocumentView GetViewByType(DocumentViewType viewType);

        IDocumentView CurrentView { get; }

        int ViewCount { get; }
    }
}


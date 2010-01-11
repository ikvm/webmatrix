namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;

    public interface ITextControlHost : IDisposable
    {
        void OnTextViewCreated(TextView view);
    }
}


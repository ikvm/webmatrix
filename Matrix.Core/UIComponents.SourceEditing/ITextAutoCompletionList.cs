namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Collections;

    public interface ITextAutoCompletionList
    {
        bool IsCompletionChar(char c);
        void OnDismiss();

        ICollection Items { get; }
    }
}


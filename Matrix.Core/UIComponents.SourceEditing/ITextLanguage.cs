namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Windows.Forms;

    public interface ITextLanguage
    {
        ITextColorizer GetColorizer(IServiceProvider provider);
        IDataObject GetDataObjectFromText(string text);
        ITextControlHost GetTextControlHost(TextControl control, IServiceProvider provider);
        string GetTextFromDataObject(IDataObject dataObject, IServiceProvider provider);
        TextBufferSpan GetWordSpan(TextBufferLocation location, WordType type);
        void ShowHelp(IServiceProvider provider, TextBufferLocation location);
        bool SupportsDataObject(IServiceProvider provider, IDataObject dataObject);
    }
}


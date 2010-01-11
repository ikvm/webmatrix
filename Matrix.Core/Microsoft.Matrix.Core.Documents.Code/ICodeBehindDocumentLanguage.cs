namespace Microsoft.Matrix.Core.Documents.Code
{
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public interface ICodeBehindDocumentLanguage
    {
        int GenerateEventHandler(TextBuffer buffer, string methodName, EventDescriptor eventDescriptor, out bool existing);
        string GenerateEventHandlerName(TextBuffer buffer, IComponent component, EventDescriptor eventDescriptor);
        ICollection GetEventHandlers(TextBuffer buffer, EventDescriptor eventDescriptor);
    }
}


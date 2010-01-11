namespace Microsoft.Matrix.Core.Documents
{
    using System;

    [Flags]
    public enum TemplateFlags
    {
        CodeRequiresClassName = 8,
        CodeRequiresNamespace = 4,
        HasCode = 2,
        HasMacros = 1,
        IsCode = 0x10,
        None = 0
    }
}


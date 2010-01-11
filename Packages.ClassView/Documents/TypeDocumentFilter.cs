namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using System;

    [Flags]
    public enum TypeDocumentFilter
    {
        Current = 0x100,
        Declared = 0x10000,
        Inherited = 0x20000,
        Obsolete = 0x200,
        Private = 4,
        Protected = 2,
        Public = 1
    }
}


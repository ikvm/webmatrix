namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using System;
    using System.Reflection;

    public sealed class FieldItem : TypeDocumentItem
    {
        public FieldItem(TypeDocument document, FieldInfo fi) : base(document, fi)
        {
        }

        protected override TypeDocumentFilter GetVisibilityFilter()
        {
            FieldInfo member = (FieldInfo) base.Member;
            if (member.IsFamily || member.IsFamilyAndAssembly)
            {
                return TypeDocumentFilter.Protected;
            }
            if (!member.IsAssembly && !member.IsPrivate)
            {
                return TypeDocumentFilter.Public;
            }
            return TypeDocumentFilter.Private;
        }
    }
}


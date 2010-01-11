namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using System;
    using System.Reflection;

    public sealed class EventItem : TypeDocumentItem
    {
        public EventItem(TypeDocument document, EventInfo ei, MethodInfo underlyingMethod) : base(document, ei, underlyingMethod)
        {
        }

        protected override TypeDocumentFilter GetVisibilityFilter()
        {
            return base.GetVisibilityFilter(base.MemberMethod);
        }

        public MethodInfo UnderlyingMethod
        {
            get
            {
                return base.MemberMethod;
            }
        }
    }
}


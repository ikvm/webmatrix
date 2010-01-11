namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;

    public sealed class EmptyElementCollection : ElementCollection
    {
        public EmptyElementCollection(Element owner) : base(owner)
        {
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
    }
}


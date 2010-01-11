namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Reflection;

    public sealed class VisualElementCollection : ElementCollection
    {
        public VisualElementCollection(Element owner) : base(owner)
        {
        }

        protected override void InsertInternal(int index, Element element)
        {
            if (!(element is VisualElement))
            {
                throw new ArgumentException();
            }
            base.InsertInternal(index, element);
        }

        public VisualElement this[int index]
        {
            get
            {
                return (VisualElement) base[index];
            }
        }
    }
}


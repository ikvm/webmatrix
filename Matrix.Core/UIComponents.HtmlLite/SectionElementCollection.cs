namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Reflection;

    public sealed class SectionElementCollection : ElementCollection
    {
        public SectionElementCollection(ContentElement owner) : base(owner)
        {
        }

        protected override void InsertInternal(int index, Element element)
        {
            if (!(element is SectionElement))
            {
                throw new ArgumentException();
            }
            base.InsertInternal(index, element);
        }

        public SectionElement this[int index]
        {
            get
            {
                return (SectionElement) base[index];
            }
        }
    }
}


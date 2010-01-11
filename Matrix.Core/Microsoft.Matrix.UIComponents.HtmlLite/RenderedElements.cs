namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class RenderedElements
    {
        private ArrayList _elements = new ArrayList();

        public void Add(VisualElement element)
        {
            this._elements.Add(element);
        }

        internal int Count
        {
            get
            {
                return this._elements.Count;
            }
        }

        internal VisualElement this[int index]
        {
            get
            {
                return (VisualElement) this._elements[index];
            }
        }
    }
}


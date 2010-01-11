namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Collections;
    using System.Drawing;

    public sealed class TextSpanElement : VisualElement
    {
        private string _text;
        private ArrayList _wordElements;

        public TextSpanElement() : this(null)
        {
        }

        public TextSpanElement(string text)
        {
            this.Text = text;
        }

        protected internal override void AddRenderedElementsToList(RenderedElements elements)
        {
            if (this._wordElements == null)
            {
                this.CreateWordElements();
            }
            if (this._wordElements != null)
            {
                for (int i = 0; i < this._wordElements.Count; i++)
                {
                    elements.Add((VisualElement) this._wordElements[i]);
                }
            }
        }

        private void CreateWordElements()
        {
            string[] strArray = this.Text.Split(new char[] { ' ' });
            if (strArray.Length != 0)
            {
                this._wordElements = new ArrayList();
                bool flag = true;
                foreach (string str in strArray)
                {
                    if (!flag)
                    {
                        TextElement element = new TextElement(" ");
                        element.SetLogicalElement(this);
                        this._wordElements.Add(element);
                    }
                    TextElement element2 = new TextElement(str);
                    element2.SetLogicalElement(this);
                    this._wordElements.Add(element2);
                    flag = false;
                }
            }
        }

        protected override Size Measure(ElementRenderData renderData)
        {
            return Size.Empty;
        }

        protected override void RenderBackground(ElementRenderData renderData)
        {
        }

        protected override void RenderForeground(ElementRenderData renderData)
        {
        }

        public string Text
        {
            get
            {
                if (this._text == null)
                {
                    return string.Empty;
                }
                return this._text;
            }
            set
            {
                if (!this.Text.Equals(value))
                {
                    this._text = value;
                    this._wordElements = null;
                    this.OnChanged(true);
                }
            }
        }
    }
}


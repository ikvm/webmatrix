namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;

    public class ElementEventArgs : EventArgs
    {
        private Microsoft.Matrix.UIComponents.HtmlLite.Element _element;

        internal ElementEventArgs(Microsoft.Matrix.UIComponents.HtmlLite.Element element)
        {
            this._element = element;
        }

        public Microsoft.Matrix.UIComponents.HtmlLite.Element Element
        {
            get
            {
                return this._element;
            }
        }
    }
}


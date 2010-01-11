namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Drawing;

    public class TextAutoCompletionItem
    {
        private string _description;
        private System.Drawing.Image _image;
        private string _text;

        public TextAutoCompletionItem(string text, System.Drawing.Image image, string description)
        {
            if ((text == null) || (text.Length == 0))
            {
                throw new ArgumentNullException("text");
            }
            this._text = text;
            this._image = image;
            this._description = description;
        }

        public override string ToString()
        {
            return this._text;
        }

        public string Description
        {
            get
            {
                if (this._description == null)
                {
                    return string.Empty;
                }
                return this._description;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
        }

        public string Text
        {
            get
            {
                return this._text;
            }
        }
    }
}


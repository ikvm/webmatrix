namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Drawing;

    internal sealed class ToggleImageElement : ImageElement
    {
        private bool _condition;
        private Image _falseImage;
        private Image _trueImage;

        public ToggleImageElement(Image trueImage, Image falseImage) : base(falseImage)
        {
            this._trueImage = trueImage;
            this._falseImage = falseImage;
        }

        public bool Condition
        {
            get
            {
                return this._condition;
            }
            set
            {
                if (this._condition != value)
                {
                    this._condition = value;
                    base.Image = this._condition ? this._trueImage : this._falseImage;
                }
            }
        }

        public Image FalseImage
        {
            get
            {
                return this._falseImage;
            }
            set
            {
                if (this._falseImage != value)
                {
                    this._falseImage = value;
                    if (!this._condition)
                    {
                        this.OnChanged(true);
                    }
                }
            }
        }

        public Image TrueImage
        {
            get
            {
                return this._trueImage;
            }
            set
            {
                if (this._trueImage != value)
                {
                    this._trueImage = value;
                    if (this._condition)
                    {
                        this.OnChanged(true);
                    }
                }
            }
        }
    }
}


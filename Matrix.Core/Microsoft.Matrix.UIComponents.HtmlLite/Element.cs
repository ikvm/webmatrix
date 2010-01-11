namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    public abstract class Element
    {
        private Color _backColor = Color.Transparent;
        private ElementCollection _elements;
        private string _fontFamily;
        private float _fontSize;
        private System.Drawing.FontStyle _fontStyle;
        private Color _foreColor;
        private VisualElement _logicalElement;
        private string _name;
        private Element _nameScope;
        private Element _parent;
        private string _toolTip;
        private object _userData;
        private bool _visible = true;

        protected Element()
        {
        }

        protected virtual ElementCollection CreateElementCollection()
        {
            return new EmptyElementCollection(this);
        }

        protected internal virtual Color GetBackColor()
        {
            if (this._logicalElement != null)
            {
                return this._logicalElement.GetBackColor();
            }
            return this.BackColor;
        }

        protected internal virtual string GetFontFamily()
        {
            if (this._logicalElement != null)
            {
                return this._logicalElement.GetFontFamily();
            }
            string fontFamily = this.FontFamily;
            if ((fontFamily.Length == 0) && (this._parent != null))
            {
                fontFamily = this._parent.GetFontFamily();
            }
            return fontFamily;
        }

        protected internal virtual float GetFontSize()
        {
            if (this._logicalElement != null)
            {
                return this._logicalElement.GetFontSize();
            }
            float fontSize = this.FontSize;
            if ((fontSize == 0f) && (this._parent != null))
            {
                fontSize = this._parent.GetFontSize();
            }
            return fontSize;
        }

        protected internal virtual System.Drawing.FontStyle GetFontStyle()
        {
            if (this._logicalElement != null)
            {
                return this._logicalElement.GetFontStyle();
            }
            System.Drawing.FontStyle fontStyle = this.FontStyle;
            if ((this._parent != null) && (fontStyle != (System.Drawing.FontStyle.Strikeout | System.Drawing.FontStyle.Underline | System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Bold)))
            {
                fontStyle |= this._parent.GetFontStyle();
            }
            return fontStyle;
        }

        protected internal virtual Color GetForeColor()
        {
            if (this._logicalElement != null)
            {
                return this._logicalElement.GetForeColor();
            }
            Color foreColor = this.ForeColor;
            if (foreColor.IsEmpty && (this._parent != null))
            {
                foreColor = this._parent.GetForeColor();
            }
            return foreColor;
        }

        protected internal virtual void OnChanged(bool requiresLayoutUpdate)
        {
            if (this._parent != null)
            {
                this._parent.OnChanged(requiresLayoutUpdate);
            }
        }

        internal void OnElementsChanged()
        {
            this.OnChanged(true);
        }

        internal void SetLogicalElement(VisualElement element)
        {
            this._logicalElement = element;
        }

        internal void SetName(string name)
        {
            this._name = name;
        }

        internal void SetParent(Element parent)
        {
            this._parent = parent;
            this._nameScope = null;
        }

        public virtual Color BackColor
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.BackColor;
                }
                return this._backColor;
            }
            set
            {
                if (this._backColor != value)
                {
                    this._backColor = value;
                    this.OnChanged(false);
                }
            }
        }

        public ElementCollection Elements
        {
            get
            {
                if (this._elements == null)
                {
                    this._elements = this.CreateElementCollection();
                }
                return this._elements;
            }
        }

        public virtual string FontFamily
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.FontFamily;
                }
                if (this._fontFamily == null)
                {
                    return string.Empty;
                }
                return this._fontFamily;
            }
            set
            {
                if (!this.FontFamily.Equals(value))
                {
                    this._fontFamily = value;
                    this.OnChanged(true);
                }
            }
        }

        public virtual float FontSize
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.FontSize;
                }
                return this._fontSize;
            }
            set
            {
                if (value < 0f)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                if (this._fontSize != value)
                {
                    this._fontSize = value;
                    this.OnChanged(true);
                }
            }
        }

        public virtual System.Drawing.FontStyle FontStyle
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.FontStyle;
                }
                return this._fontStyle;
            }
            set
            {
                if (this._fontStyle != value)
                {
                    this._fontStyle = value;
                    this.OnChanged(true);
                }
            }
        }

        public virtual Color ForeColor
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.ForeColor;
                }
                return this._foreColor;
            }
            set
            {
                if (this._foreColor != value)
                {
                    this._foreColor = value;
                    this.OnChanged(false);
                }
            }
        }

        internal VisualElement LogicalElement
        {
            get
            {
                return this._logicalElement;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                string str = this._name;
                this._name = value;
                if ((str != null) && (this.NameScope != null))
                {
                    this.NameScope.Elements.DirtyNamedElements();
                }
            }
        }

        public Element NameScope
        {
            get
            {
                if ((this._nameScope == null) && (this._parent != null))
                {
                    if (this._parent is INameScopeElement)
                    {
                        this._nameScope = this._parent;
                    }
                    else
                    {
                        this._nameScope = this._parent.NameScope;
                    }
                }
                return this._nameScope;
            }
        }

        public Element Parent
        {
            get
            {
                return this._parent;
            }
        }

        public string ToolTip
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.ToolTip;
                }
                if (this._toolTip == null)
                {
                    return string.Empty;
                }
                return this._toolTip;
            }
            set
            {
                this._toolTip = value;
            }
        }

        public object UserData
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.UserData;
                }
                return this._userData;
            }
            set
            {
                this._userData = value;
            }
        }

        public bool Visible
        {
            get
            {
                if (this._logicalElement != null)
                {
                    return this._logicalElement.Visible;
                }
                return this._visible;
            }
            set
            {
                if (this._visible != value)
                {
                    this._visible = value;
                    this.OnChanged(true);
                }
            }
        }
    }
}


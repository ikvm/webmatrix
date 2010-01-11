namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web.UI.WebControls;

    [StructLayout(LayoutKind.Sequential), TypeConverter(typeof(BackgroundPositionConverter))]
    public struct BackgroundPosition
    {
        public static readonly BackgroundPosition Empty;
        public static readonly BackgroundPosition TopOrLeft;
        public static readonly BackgroundPosition Center;
        public static readonly BackgroundPosition BottomOrRight;
        private readonly BackgroundPositionType _type;
        private readonly System.Web.UI.WebControls.Unit _value;
        public BackgroundPosition(BackgroundPositionType type)
        {
            if ((type < BackgroundPositionType.NotSet) || (type > BackgroundPositionType.BottomOrRight))
            {
                throw new ArgumentOutOfRangeException("type");
            }
            this._type = type;
            if (type == BackgroundPositionType.Unit)
            {
                this._value = System.Web.UI.WebControls.Unit.Pixel(0);
            }
            else
            {
                this._value = System.Web.UI.WebControls.Unit.Empty;
            }
        }

        public BackgroundPosition(System.Web.UI.WebControls.Unit value)
        {
            this._type = BackgroundPositionType.NotSet;
            if (!value.IsEmpty)
            {
                this._type = BackgroundPositionType.Unit;
                this._value = value;
            }
            else
            {
                this._value = System.Web.UI.WebControls.Unit.Empty;
            }
        }

        public BackgroundPosition(string value) : this(value, CultureInfo.CurrentCulture)
        {
        }

        public BackgroundPosition(string value, CultureInfo culture)
        {
            this._type = BackgroundPositionType.NotSet;
            this._value = System.Web.UI.WebControls.Unit.Empty;
            if ((value != null) && (value.Length != 0))
            {
                string str = value.ToLower();
                if ((str.Equals("top") || str.Equals("left")) || str.Equals("toporleft"))
                {
                    this._type = BackgroundPositionType.TopOrLeft;
                }
                else if (str.Equals("center"))
                {
                    this._type = BackgroundPositionType.Center;
                }
                else if ((str.Equals("bottom") || str.Equals("right")) || str.Equals("bottomorright"))
                {
                    this._type = BackgroundPositionType.BottomOrRight;
                }
                else
                {
                    this._value = System.Web.UI.WebControls.Unit.Parse(value, culture);
                    this._type = BackgroundPositionType.Unit;
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this._type == BackgroundPositionType.NotSet);
            }
        }
        public BackgroundPositionType Type
        {
            get
            {
                return this._type;
            }
        }
        public System.Web.UI.WebControls.Unit Unit
        {
            get
            {
                return this._value;
            }
        }
        public override int GetHashCode()
        {
            return ((((BackgroundPositionType) this._type).GetHashCode() << 2) ^ this._value.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is BackgroundPosition))
            {
                return false;
            }
            BackgroundPosition position = (BackgroundPosition) obj;
            if (position.Type != this._type)
            {
                return false;
            }
            if (position.Type == BackgroundPositionType.Unit)
            {
                return (position.Unit == this._value);
            }
            return true;
        }

        public static BackgroundPosition Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        public static BackgroundPosition Parse(string s, CultureInfo culture)
        {
            return new BackgroundPosition(s, culture);
        }

        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentCulture);
        }

        public string ToString(CultureInfo culture)
        {
            string str = string.Empty;
            if (!this.IsEmpty)
            {
                switch (this._type)
                {
                    case BackgroundPositionType.Unit:
                        return this._value.ToString(culture);

                    case BackgroundPositionType.TopOrLeft:
                        return "TopOrLeft";

                    case BackgroundPositionType.Center:
                        return "Center";

                    case BackgroundPositionType.BottomOrRight:
                        return "BottomOrRight";
                }
            }
            return str;
        }

        static BackgroundPosition()
        {
            Empty = new BackgroundPosition();
            TopOrLeft = new BackgroundPosition(BackgroundPositionType.TopOrLeft);
            Center = new BackgroundPosition(BackgroundPositionType.Center);
            BottomOrRight = new BackgroundPosition(BackgroundPositionType.BottomOrRight);
        }
    }
}


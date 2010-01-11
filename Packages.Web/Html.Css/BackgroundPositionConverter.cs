namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class BackgroundPositionConverter : TypeConverter
    {
        private TypeConverter.StandardValuesCollection _standardValues;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(string)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }
            string str = value as string;
            if (str == null)
            {
                return base.ConvertFrom(context, culture, value);
            }
            string s = str.Trim();
            if (s.Length == 0)
            {
                return BackgroundPosition.Empty;
            }
            return BackgroundPosition.Parse(s, culture);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            if (value != null)
            {
                BackgroundPosition position = (BackgroundPosition) value;
                if (position.Type != BackgroundPositionType.NotSet)
                {
                    position = (BackgroundPosition) value;
                    return position.ToString(culture);
                }
            }
            return string.Empty;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (this._standardValues == null)
            {
                object[] values = new object[] { BackgroundPosition.TopOrLeft, BackgroundPosition.Center, BackgroundPosition.BottomOrRight };
                this._standardValues = new TypeConverter.StandardValuesCollection(values);
            }
            return this._standardValues;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}


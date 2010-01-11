namespace Microsoft.JSharp
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Globalization;

    public sealed class JSharpCodeProvider : CodeDomProvider
    {
        internal const string COMPILERNAME = "vjc.exe";
        internal const string FILEEXTENSION = "jsl";
        private JSharpCodeGenerator generator = new JSharpCodeGenerator();
        internal const string NULLTOKEN = "null";

        public override ICodeCompiler CreateCompiler()
        {
            return this.generator;
        }

        public override ICodeGenerator CreateGenerator()
        {
            this.generator.fEnableDataformWizWorkaround = false;
            return this.generator;
        }

        public override TypeConverter GetConverter(Type type)
        {
            if (type == typeof(MemberAttributes))
            {
                return JSharpMemberAttributeConverter.Default;
            }
            return base.GetConverter(type);
        }

        public override string FileExtension
        {
            get
            {
                return "jsl";
            }
        }

        internal class JSharpMemberAttributeConverter : TypeConverter
        {
            private static JSharpCodeProvider.JSharpMemberAttributeConverter defaultConverter;
            private static string[] names;
            private static object[] values;

            private JSharpMemberAttributeConverter()
            {
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    string str = (string) value;
                    string[] names = this.Names;
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (names[i].Equals(str))
                        {
                            return this.Values[i];
                        }
                    }
                }
                return MemberAttributes.Private;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == null)
                {
                    throw new ArgumentNullException("destinationType");
                }
                if (destinationType != typeof(string))
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
                object[] values = this.Values;
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].Equals(value))
                    {
                        return this.Names[i];
                    }
                }
                throw new ArgumentException("value");
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new TypeConverter.StandardValuesCollection(this.Values);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public static JSharpCodeProvider.JSharpMemberAttributeConverter Default
            {
                get
                {
                    if (defaultConverter == null)
                    {
                        defaultConverter = new JSharpCodeProvider.JSharpMemberAttributeConverter();
                    }
                    return defaultConverter;
                }
            }

            private string[] Names
            {
                get
                {
                    if (names == null)
                    {
                        names = new string[] { "Public", "Protected", "Package", "Private" };
                    }
                    return names;
                }
            }

            private object[] Values
            {
                get
                {
                    if (values == null)
                    {
                        values = new object[] { MemberAttributes.Public, MemberAttributes.Family, MemberAttributes.Assembly, MemberAttributes.Private };
                    }
                    return values;
                }
            }
        }
    }
}


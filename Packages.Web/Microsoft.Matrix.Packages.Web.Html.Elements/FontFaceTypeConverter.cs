namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;

    public class FontFaceTypeConverter : StringConverter
    {
        private static object[] fontArray;
        private static TypeConverter.StandardValuesCollection values;

        static FontFaceTypeConverter()
        {
            InitializeFontList();
            SystemEvents.InstalledFontsChanged += new EventHandler(FontFaceTypeConverter.OnInstalledFontsChanged);
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (values == null)
            {
                values = new TypeConverter.StandardValuesCollection(fontArray);
            }
            return values;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        private static void InitializeFontList()
        {
            Hashtable hashtable = new Hashtable();
            FontFamily[] families = FontFamily.Families;
            for (int i = 0; i < families.Length; i++)
            {
                hashtable[families[i].Name.ToLower()] = families[i].Name;
            }
            fontArray = new object[hashtable.Count];
            hashtable.Values.CopyTo(fontArray, 0);
            Array.Sort(fontArray);
        }

        private static void OnInstalledFontsChanged(object sender, EventArgs e)
        {
            InitializeFontList();
        }
    }
}


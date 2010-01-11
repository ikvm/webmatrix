namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public sealed class DataTypeConverter : StringConverter
    {
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context == null)
            {
                return null;
            }
            Column instance = (Column) context.Instance;
            ArrayList list = new ArrayList(instance.DataTypeList);
            list.Sort();
            return new TypeConverter.StandardValuesCollection((string[]) list.ToArray(typeof(string)));
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}


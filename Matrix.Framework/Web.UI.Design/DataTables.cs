namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using System;
    using System.Collections;
    using System.Reflection;

    internal class DataTables : ArrayList
    {
        public override object this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                if (!(value is DataTableSchema))
                {
                    throw new ArgumentOutOfRangeException();
                }
                base[index] = value;
            }
        }
    }
}


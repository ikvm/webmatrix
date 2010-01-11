namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public sealed class FlagsEnumEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider serviceProvider, object value)
        {
            if (((context == null) || (context.Instance == null)) || (serviceProvider == null))
            {
                return value;
            }
            IWindowsFormsEditorService service = (IWindowsFormsEditorService) serviceProvider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
            {
                return value;
            }
            int num = (int) Convert.ChangeType(value, typeof(int));
            CheckedListBox control = new CheckedListBox();
            control.BorderStyle = BorderStyle.None;
            control.CheckOnClick = true;
            Type propertyType = context.PropertyDescriptor.PropertyType;
            string[] names = Enum.GetNames(propertyType);
            int num2 = 0;
            foreach (string str in names)
            {
                object obj2 = Enum.Parse(propertyType, str);
                int num3 = (int) Convert.ChangeType(obj2, typeof(int));
                if (num3 == 0)
                {
                    num2++;
                }
                else
                {
                    EnumFieldItem item = new EnumFieldItem(obj2.ToString(), num3);
                    bool isChecked = (num & num3) != 0;
                    control.Items.Add(item, isChecked);
                }
            }
            int num4 = Math.Min(names.Length - num2, 5);
            control.Height = num4 * 0x10;
            service.DropDownControl(control);
            int num5 = 0;
            foreach (EnumFieldItem item2 in control.CheckedItems)
            {
                num5 += item2.Value;
            }
            return Enum.ToObject(propertyType, num5);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        internal sealed class EnumFieldItem
        {
            private string _text;
            private int _value;

            public EnumFieldItem(string text, int value)
            {
                this._text = text;
                this._value = value;
            }

            public override string ToString()
            {
                return this._text;
            }

            public int Value
            {
                get
                {
                    return this._value;
                }
            }
        }
    }
}


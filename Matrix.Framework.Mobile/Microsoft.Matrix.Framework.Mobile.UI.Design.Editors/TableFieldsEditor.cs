namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Web.UI.MobileControls;

    internal class TableFieldsEditor : MobileUITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ObjectList instance = context.Instance as ObjectList;
            (base.GetDesigner(context) as ObjectListDesigner).InvokePropertyBuilder(0);
            return instance.get_TableFields();
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context.Instance is ObjectList)
            {
                return base.GetEditStyle(context);
            }
            return UITypeEditorEditStyle.None;
        }
    }
}


namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Web.UI.MobileControls;

    internal class FieldCollectionEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IDesignerHost service = (IDesignerHost) context.GetService(typeof(IDesignerHost));
            ObjectList instance = (ObjectList) context.Instance;
            ObjectListDesigner designer = (ObjectListDesigner) service.GetDesigner(instance);
            if (designer.ActiveDeviceFilter == null)
            {
                designer.InvokePropertyBuilder(2);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}


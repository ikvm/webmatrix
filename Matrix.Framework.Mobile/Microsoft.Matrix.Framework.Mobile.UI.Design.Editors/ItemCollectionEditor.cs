namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Web.UI.MobileControls;

    internal class ItemCollectionEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IDesignerHost service = (IDesignerHost) context.GetService(typeof(IDesignerHost));
            object instance = context.Instance;
            IDesigner designer = service.GetDesigner((IComponent) instance);
            if (instance is List)
            {
                if (((ListDesigner) designer).ActiveDeviceFilter == null)
                {
                    ((ListDesigner) designer).InvokePropertyBuilder(1);
                }
                return value;
            }
            if (((SelectionListDesigner) designer).ActiveDeviceFilter == null)
            {
                ((SelectionListDesigner) designer).InvokePropertyBuilder(1);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}


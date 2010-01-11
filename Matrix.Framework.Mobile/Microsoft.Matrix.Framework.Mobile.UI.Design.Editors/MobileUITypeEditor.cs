namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Web.UI;
    using System.Web.UI.Design;

    internal class MobileUITypeEditor : UITypeEditor
    {
        protected ControlDesigner GetDesigner(ITypeDescriptorContext context)
        {
            IDesignerHost service = (IDesignerHost) context.GetService(typeof(IDesignerHost));
            Control instance = (Control) context.Instance;
            return (ControlDesigner) service.GetDesigner(instance);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}


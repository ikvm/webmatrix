namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;

    public sealed class ElementStyleEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider serviceProvider, object value)
        {
            string cssText = null;
            if (value != null)
            {
                cssText = (string) value;
            }
            StyledElement instance = (StyledElement) context.Instance;
            StyleBuilderDialog dialog = new StyleBuilderDialog(serviceProvider, instance.Owner.Url, null, cssText);
            IMxUIService service = (IMxUIService) serviceProvider.GetService(typeof(IMxUIService));
            if (service.ShowDialog(dialog) == DialogResult.OK)
            {
                cssText = dialog.CssText;
            }
            return cssText;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;

    public sealed class FontEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider serviceProvider, object value)
        {
            string fontFamily = null;
            if (value != null)
            {
                fontFamily = (string) value;
            }
            FontPicker dialog = new FontPicker(serviceProvider, fontFamily);
            IMxUIService service = (IMxUIService) serviceProvider.GetService(typeof(IMxUIService));
            if (service.ShowDialog(dialog) == DialogResult.OK)
            {
                fontFamily = dialog.FontFamily;
            }
            return fontFamily;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}


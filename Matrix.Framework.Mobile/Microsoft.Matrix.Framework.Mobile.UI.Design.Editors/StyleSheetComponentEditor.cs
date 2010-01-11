namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms;

    internal class StyleSheetComponentEditor : ComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component)
        {
            StylesEditorDialog dialog;
            StyleSheet sheet = (StyleSheet) component;
            StyleSheetDesigner controlDesigner = (StyleSheetDesigner) Utils.GetControlDesigner(sheet);
            if (controlDesigner.ActiveDeviceFilter != null)
            {
                return false;
            }
            if (controlDesigner.InTemplateMode)
            {
                MessageBox.Show(MobileResource.GetString("BaseTemplatedMobileComponentEditor_TemplateModeErrorMessage"), MobileResource.GetString("BaseTemplatedMobileComponentEditor_TemplateModeErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }
            try
            {
                dialog = new StylesEditorDialog(sheet, controlDesigner, null);
            }
            catch
            {
                return false;
            }
            bool flag = dialog.ShowDialog() == DialogResult.OK;
            controlDesigner.RefreshMobilePage();
            return flag;
        }
    }
}


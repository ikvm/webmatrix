namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal abstract class BaseTemplatedMobileComponentEditor : WindowsFormsComponentEditor
    {
        private int _initialPage;

        internal BaseTemplatedMobileComponentEditor(int initialPage)
        {
            this._initialPage = initialPage;
        }

        public override bool EditComponent(ITypeDescriptorContext context, object obj, IWin32Window parent)
        {
            bool inTemplateMode = false;
            IComponent component = (IComponent) obj;
            ISite site = component.Site;
            if (site != null)
            {
                IDesignerHost service = (IDesignerHost) site.GetService(typeof(IDesignerHost));
                MobileTemplatedControlDesigner designer = (MobileTemplatedControlDesigner) service.GetDesigner(component);
                inTemplateMode = designer.InTemplateMode;
                if (designer.ActiveDeviceFilter != null)
                {
                    return false;
                }
            }
            if (!inTemplateMode)
            {
                return base.EditComponent(context, obj, parent);
            }
            MessageBox.Show(Constants.BaseTemplatedMobileComponentEditorTemplateModeErrorMessage, Constants.BaseTemplatedMobileComponentEditorTemplateModeErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
        }

        protected override int GetInitialComponentEditorPageIndex()
        {
            return this._initialPage;
        }
    }
}


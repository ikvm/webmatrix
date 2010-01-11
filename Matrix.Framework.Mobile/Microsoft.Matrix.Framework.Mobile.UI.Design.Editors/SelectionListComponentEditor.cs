namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal class SelectionListComponentEditor : WindowsFormsComponentEditor
    {
        private static Type[] _editorPages = new Type[] { typeof(ListGeneralPage), typeof(ListItemsPage) };
        private int _initialPage;
        internal const int IDX_GENERAL = 0;
        internal const int IDX_ITEMS = 1;

        public SelectionListComponentEditor()
        {
            this._initialPage = 0;
        }

        public SelectionListComponentEditor(int initialPage)
        {
            this._initialPage = initialPage;
        }

        public override bool EditComponent(ITypeDescriptorContext context, object obj, IWin32Window parent)
        {
            IComponent component = (IComponent) obj;
            ISite site = component.Site;
            if (site != null)
            {
                IDesignerHost service = (IDesignerHost) site.GetService(typeof(IDesignerHost));
                MobileControlDesigner designer = (MobileControlDesigner) service.GetDesigner(component);
                if (designer.ActiveDeviceFilter != null)
                {
                    return false;
                }
            }
            return base.EditComponent(context, obj, parent);
        }

        protected override Type[] GetComponentEditorPages()
        {
            return _editorPages;
        }

        protected override int GetInitialComponentEditorPageIndex()
        {
            return this._initialPage;
        }
    }
}


namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;

    internal class ListComponentEditor : BaseTemplatedMobileComponentEditor
    {
        private static Type[] _editorPages = new Type[] { typeof(ListGeneralPage), typeof(ListItemsPage) };
        internal const int IDX_GENERAL = 0;
        internal const int IDX_ITEMS = 1;

        public ListComponentEditor() : base(0)
        {
        }

        public ListComponentEditor(int initialPage) : base(initialPage)
        {
        }

        protected override Type[] GetComponentEditorPages()
        {
            return _editorPages;
        }
    }
}


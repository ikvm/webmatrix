namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;

    internal class ObjectListComponentEditor : BaseTemplatedMobileComponentEditor
    {
        private static Type[] _editorPages = new Type[] { typeof(ObjectListGeneralPage), typeof(ObjectListCommandsPage), typeof(ObjectListFieldsPage) };
        internal const int IDX_COMMANDS = 1;
        internal const int IDX_FIELDS = 2;
        internal const int IDX_GENERAL = 0;

        public ObjectListComponentEditor() : this(0)
        {
        }

        public ObjectListComponentEditor(int initialPage) : base(initialPage)
        {
        }

        protected override Type[] GetComponentEditorPages()
        {
            return _editorPages;
        }
    }
}


namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Packages.ClassView.Documents;
    using System;

    internal sealed class MemberPage : ClassViewPage
    {
        private TypeDocumentItem _item;

        public void SetItem(TypeDocumentItem item)
        {
            this._item = item;
        }

        public TypeDocumentItem Item
        {
            get
            {
                return this._item;
            }
        }
    }
}


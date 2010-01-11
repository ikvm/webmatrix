namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using Microsoft.Matrix.Framework.Web.UI;
    using System;
    using System.ComponentModel.Design;

    public class MxDataGridFieldCollectionEditor : CollectionEditor
    {
        public MxDataGridFieldCollectionEditor(Type type) : base(type)
        {
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(BoundField), typeof(ButtonField), typeof(TemplateField), typeof(EditCommandField), typeof(HyperLinkField) };
        }
    }
}


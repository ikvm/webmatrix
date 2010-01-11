namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class TextBoxDesigner : MobileTextControlDesigner
    {
        private TextBox _textBox;

        public override void Initialize(IComponent component)
        {
            this._textBox = (TextBox) component;
            base.Initialize(component);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerTextBoxAdapter(this._textBox);
            }
        }
    }
}


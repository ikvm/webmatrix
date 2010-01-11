namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class TextViewDesigner : MobileTextControlDesigner
    {
        private TextView _textView;

        public override void Initialize(IComponent component)
        {
            this._textView = (TextView) component;
            base.Initialize(component);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerTextViewAdapter(this._textView);
            }
        }
    }
}


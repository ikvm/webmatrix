namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class LabelDesigner : MobileTextControlDesigner
    {
        private Label _label;

        public override void Initialize(IComponent component)
        {
            this._label = (Label) component;
            base.Initialize(component);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerLabelAdapter(this._label);
            }
        }
    }
}


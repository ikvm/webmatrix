namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class AdRotatorDesigner : MobileControlDesigner
    {
        private AdRotator _adRotator;

        public override void Initialize(IComponent component)
        {
            this._adRotator = (AdRotator) component;
            base.Initialize(component);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerAdRotatorAdapter(this._adRotator);
            }
        }
    }
}


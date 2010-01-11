namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class ValidationSummaryDesigner : MobileControlDesigner
    {
        private ValidationSummary _validationSummary;

        public override void Initialize(IComponent component)
        {
            this._validationSummary = (ValidationSummary) component;
            base.Initialize(component);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerValidationSummaryAdapter(this._validationSummary);
            }
        }
    }
}


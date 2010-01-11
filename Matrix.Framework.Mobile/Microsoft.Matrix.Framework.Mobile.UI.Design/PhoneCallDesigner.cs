namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Converters;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class PhoneCallDesigner : MobileTextControlDesigner
    {
        private const string _alternateUrlPropertyName = "AlternateUrl";
        private PhoneCall _phoneCall;

        public override void Initialize(IComponent component)
        {
            this._phoneCall = (PhoneCall) component;
            base.Initialize(component);
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            Utils.AddAttributesToProperty(this._phoneCall.GetType(), properties, "AlternateUrl", new Attribute[] { new TypeConverterAttribute(typeof(NavigateUrlConverter)) });
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerPhoneCallAdapter(this._phoneCall);
            }
        }
    }
}


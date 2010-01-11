namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Converters;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class LinkDesigner : MobileTextControlDesigner
    {
        private Link _link;
        private const string _navigateUrlPropertyName = "NavigateUrl";

        public override void Initialize(IComponent component)
        {
            this._link = (Link) component;
            base.Initialize(component);
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            Utils.AddAttributesToProperty(this._link.GetType(), properties, "NavigateUrl", new Attribute[] { new TypeConverterAttribute(typeof(NavigateUrlConverter)) });
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerLinkAdapter(this._link);
            }
        }
    }
}


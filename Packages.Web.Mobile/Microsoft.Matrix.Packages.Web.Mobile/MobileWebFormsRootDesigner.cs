namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using System;
    using System.Collections;
    using System.Web.UI.Design;

    internal sealed class MobileWebFormsRootDesigner : ControlDesigner
    {
        protected override void PreFilterEvents(IDictionary events)
        {
            events.Clear();
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            properties.Clear();
        }
    }
}


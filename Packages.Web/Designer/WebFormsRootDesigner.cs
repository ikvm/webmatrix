namespace Microsoft.Matrix.Packages.Web.Designer
{
    using System;
    using System.Collections;
    using System.Web.UI.Design;

    internal sealed class WebFormsRootDesigner : ControlDesigner
    {
        protected override void PreFilterEvents(IDictionary events)
        {
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            properties.Clear();
        }
    }
}


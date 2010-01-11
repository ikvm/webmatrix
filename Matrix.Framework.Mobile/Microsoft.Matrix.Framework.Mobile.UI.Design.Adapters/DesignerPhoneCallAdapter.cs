namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.UI.MobileControls;

    public class DesignerPhoneCallAdapter : DesignerLabelAdapter
    {
        public DesignerPhoneCallAdapter(PhoneCall phoneCall) : base(phoneCall)
        {
            base.set_Control(phoneCall);
        }
    }
}


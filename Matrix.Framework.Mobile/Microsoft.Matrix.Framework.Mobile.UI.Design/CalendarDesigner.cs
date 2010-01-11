namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class CalendarDesigner : MobileControlDesigner
    {
        private Calendar _calendar;

        public override void Initialize(IComponent component)
        {
            this._calendar = (Calendar) component;
            base.Initialize(component);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerCalendarAdapter(this._calendar);
            }
        }
    }
}


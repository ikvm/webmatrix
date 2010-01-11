namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI.MobileControls;

    public class CommandDesigner : MobileTextControlDesigner
    {
        private Command _command;

        public override void Initialize(IComponent component)
        {
            this._command = (Command) component;
            base.Initialize(component);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerCommandAdapter(this._command);
            }
        }
    }
}


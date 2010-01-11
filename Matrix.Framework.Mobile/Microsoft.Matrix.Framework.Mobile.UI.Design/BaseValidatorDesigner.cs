namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters;
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;
    using System.Web.UI.WebControls;

    public class BaseValidatorDesigner : MobileTextControlDesigner
    {
        private BaseValidator _baseValidator;

        protected override string GetDesignTimeNormalHtml()
        {
            string str = this._baseValidator.get_ErrorMessage();
            bool flag = (this._baseValidator.get_Display() == ValidatorDisplay.None) || ((str.Trim().Length == 0) && (this._baseValidator.get_Text().Trim().Length == 0));
            if (flag)
            {
                this._baseValidator.set_ErrorMessage("[" + this._baseValidator.ID + "]");
            }
            HtmlMobileTextWriter designerTextWriter = Utils.GetDesignerTextWriter();
            this.Adapter.Render(designerTextWriter);
            if (flag)
            {
                this._baseValidator.set_ErrorMessage(str);
            }
            return designerTextWriter.ToString();
        }

        public override void Initialize(IComponent component)
        {
            this._baseValidator = (BaseValidator) component;
            base.Initialize(component);
            for (int i = this._baseValidator.Controls.Count - 1; i >= 0; i--)
            {
                Control control = this._baseValidator.Controls[i];
                if (control is BaseValidator)
                {
                    this._baseValidator.Controls.RemoveAt(i);
                }
            }
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return new DesignerValidatorAdapter(this._baseValidator);
            }
        }
    }
}


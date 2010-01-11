namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html.WebForms;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Web.UI;

    internal sealed class TemplateDesignView : WebFormsDesignView, ITemplateDesignView
    {
        private Control _defaultControlParent;
        private TemplateEditingDialog _owner;

        public TemplateDesignView(IServiceProvider serviceProvider, Control defaultControlParent, TemplateEditingDialog owner) : base(serviceProvider, WebFormsEditorMode.Template)
        {
            this._defaultControlParent = defaultControlParent;
            this._owner = owner;
        }

        protected override void OnEditorCreated()
        {
            base.OnEditorCreated();
            WebFormsEditor editor = base.Editor as WebFormsEditor;
            editor.DefaultControlParent = this._defaultControlParent;
        }

        protected override bool UpdateCommand(Command command)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(WebCommands)) && (command.CommandID == 240))
            {
                command.Enabled = false;
                flag = true;
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }

        string ITemplateDesignView.ActiveTemplateName
        {
            get
            {
                if (this._owner != null)
                {
                    return this._owner.ActiveTemplateName;
                }
                return null;
            }
        }
    }
}


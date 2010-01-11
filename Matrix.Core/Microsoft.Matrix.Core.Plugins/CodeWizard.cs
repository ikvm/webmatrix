namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.CodeDom.Compiler;
    using System.Windows.Forms;

    public abstract class CodeWizard : Plugin
    {
        private System.CodeDom.Compiler.CodeDomProvider _codeDomProvider;
        public static string CodeWizardDataFormat = "CodeWizard";

        protected CodeWizard()
        {
        }

        protected virtual CodeWizardForm CreateCodeWizardForm()
        {
            return null;
        }

        protected override void Dispose()
        {
            this._codeDomProvider = null;
            base.Dispose();
        }

        protected virtual string Run()
        {
            IMxUIService service = (IMxUIService) base.GetService(typeof(IMxUIService));
            CodeWizardForm dialog = this.CreateCodeWizardForm();
            if ((dialog != null) && (service.ShowDialog(dialog) == DialogResult.OK))
            {
                return dialog.GeneratedCode;
            }
            return string.Empty;
        }

        protected sealed override object RunPlugin(object initializationData)
        {
            this._codeDomProvider = (System.CodeDom.Compiler.CodeDomProvider) initializationData;
            return this.Run();
        }

        protected internal System.CodeDom.Compiler.CodeDomProvider CodeDomProvider
        {
            get
            {
                return this._codeDomProvider;
            }
        }
    }
}


namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.CodeDom.Compiler;
    using System.Drawing;
    using System.Windows.Forms;

    public abstract class CodeWizardForm : WizardForm
    {
        private CodeWizard _codeWizard;
        private static Image codeWizardFormGlyph;

        public CodeWizardForm(CodeWizard codeWizard) : base(codeWizard.ServiceProvider)
        {
            this._codeWizard = codeWizard;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.TaskGlyph = CodeWizardFormGlyph;
            base.TaskAbout = true;
        }

        protected sealed override void ShowTaskAboutInformation()
        {
            IMxUIService service = (IMxUIService) this.GetService(typeof(IMxUIService));
            if (service != null)
            {
                AboutPluginDialog dialog = new AboutPluginDialog(this._codeWizard, "Code Wizard", CodeWizardFormGlyph);
                service.ShowDialog(dialog);
            }
        }

        protected System.CodeDom.Compiler.CodeDomProvider CodeDomProvider
        {
            get
            {
                return this._codeWizard.CodeDomProvider;
            }
        }

        private static Image CodeWizardFormGlyph
        {
            get
            {
                if (codeWizardFormGlyph == null)
                {
                    codeWizardFormGlyph = new Bitmap(typeof(CodeWizardForm), "CodeWizardGlyph.bmp");
                }
                return codeWizardFormGlyph;
            }
        }

        public virtual string GeneratedCode
        {
            get
            {
                return string.Empty;
            }
        }
    }
}


namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public abstract class DocumentWizardForm : WizardForm
    {
        private byte[] _documentContent;
        private string _documentContentText;
        private DocumentWizard _documentWizard;
        private static Image documentWizardFormGlyph;

        public DocumentWizardForm(DocumentWizard documentWizard) : base(documentWizard.ServiceProvider)
        {
            this._documentWizard = documentWizard;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.TaskGlyph = DocumentWizardFormGlyph;
            base.TaskAbout = true;
        }

        protected void SetDocumentContent(string content)
        {
            this._documentContent = null;
            this._documentContentText = content;
        }

        protected sealed override void ShowTaskAboutInformation()
        {
            IMxUIService service = (IMxUIService) this.GetService(typeof(IMxUIService));
            if (service != null)
            {
                AboutPluginDialog dialog = new AboutPluginDialog(this._documentWizard, "Document Wizard", DocumentWizardFormGlyph);
                service.ShowDialog(dialog);
            }
        }

        public virtual byte[] DocumentContent
        {
            get
            {
                if (((this._documentContent == null) && (this._documentContentText != null)) && (this._documentContentText.Length != 0))
                {
                    this._documentContent = Encoding.UTF8.GetBytes(this._documentContentText);
                }
                return this._documentContent;
            }
        }

        private static Image DocumentWizardFormGlyph
        {
            get
            {
                if (documentWizardFormGlyph == null)
                {
                    documentWizardFormGlyph = new Bitmap(typeof(DocumentWizardForm), "DocumentWizardGlyph.bmp");
                }
                return documentWizardFormGlyph;
            }
        }

        protected DocumentInstanceArguments InstanceArguments
        {
            get
            {
                return this._documentWizard.InstanceArguments;
            }
        }
    }
}


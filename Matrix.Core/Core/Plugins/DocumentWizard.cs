namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Windows.Forms;

    public abstract class DocumentWizard : Plugin
    {
        private DocumentInstanceArguments _instanceArguments;

        protected DocumentWizard()
        {
        }

        protected virtual DocumentWizardForm CreateDocumentWizardForm()
        {
            return null;
        }

        protected override void Dispose()
        {
            this._instanceArguments = null;
            base.Dispose();
        }

        protected virtual byte[] Run()
        {
            IMxUIService service = (IMxUIService) base.GetService(typeof(IMxUIService));
            DocumentWizardForm dialog = this.CreateDocumentWizardForm();
            if ((dialog != null) && (service.ShowDialog(dialog) == DialogResult.OK))
            {
                return dialog.DocumentContent;
            }
            return null;
        }

        protected sealed override object RunPlugin(object initializationData)
        {
            this._instanceArguments = (DocumentInstanceArguments) initializationData;
            return this.Run();
        }

        protected internal DocumentInstanceArguments InstanceArguments
        {
            get
            {
                return this._instanceArguments;
            }
        }
    }
}


namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    internal sealed class DocumentWizardHost : PluginHost
    {
        public DocumentWizardHost(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public byte[] RunDocumentWizard(string documentWizardTypeName, DocumentInstanceArguments instanceArguments)
        {
            return (byte[]) base.RunPlugin(documentWizardTypeName, instanceArguments);
        }

        protected override string Description
        {
            get
            {
                return "Add New File";
            }
        }

        protected override Type PluginBaseType
        {
            get
            {
                return typeof(DocumentWizard);
            }
        }
    }
}


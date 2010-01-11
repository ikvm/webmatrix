namespace Microsoft.Matrix.Core.Documents.Code
{
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.CodeDom.Compiler;
    using System.Windows.Forms;

    public abstract class CodeDocumentLanguage : TextDocumentLanguage
    {
        private System.CodeDom.Compiler.CodeDomProvider _codeDomProvider;

        protected CodeDocumentLanguage(string name, System.CodeDom.Compiler.CodeDomProvider codeDomProvider) : base(name)
        {
            if (codeDomProvider == null)
            {
                throw new ArgumentNullException("codeDomProvider");
            }
            this._codeDomProvider = codeDomProvider;
        }

        protected override string GetTextFromDataObject(IDataObject dataObject, IServiceProvider serviceProvider)
        {
            if (dataObject.GetDataPresent(CodeWizard.CodeWizardDataFormat))
            {
                string codeWizardTypeName = dataObject.GetData(CodeWizard.CodeWizardDataFormat).ToString();
                CodeWizardHost host = new CodeWizardHost(serviceProvider);
                return host.RunCodeWizard(codeWizardTypeName, this.CodeDomProvider);
            }
            return base.GetTextFromDataObject(dataObject, serviceProvider);
        }

        protected override void ShowHelp(IServiceProvider serviceProvider, TextBufferLocation location)
        {
            IClassViewService service = (IClassViewService) serviceProvider.GetService(typeof(IClassViewService));
            if (service != null)
            {
                using (TextBufferSpan span = ((ITextLanguage) this).GetWordSpan(location, WordType.Current))
                {
                    if (span != null)
                    {
                        service.ShowType(span.Text);
                    }
                }
            }
        }

        protected override bool SupportsDataObject(IServiceProvider serviceProvider, IDataObject dataObject)
        {
            if (!base.SupportsDataObject(serviceProvider, dataObject))
            {
                return dataObject.GetDataPresent(CodeWizard.CodeWizardDataFormat);
            }
            return true;
        }

        public System.CodeDom.Compiler.CodeDomProvider CodeDomProvider
        {
            get
            {
                return this._codeDomProvider;
            }
        }
    }
}


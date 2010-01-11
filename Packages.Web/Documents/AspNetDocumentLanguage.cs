namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Windows.Forms;

    public class AspNetDocumentLanguage : TextDocumentLanguage
    {
        internal static AspNetDocumentLanguage Instance;

        public AspNetDocumentLanguage() : base("ASP.NET", null)
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private CodeDocumentLanguage GetCodeLanguage(IServiceProvider provider)
        {
            IDocumentDesignerHost service = provider.GetService(typeof(IDocumentDesignerHost)) as IDocumentDesignerHost;
            CodeDocument document = service.Document as CodeDocument;
            if ((document != null) && (document.Language != null))
            {
                return (CodeDocumentLanguage) document.Language;
            }
            IDocumentWithCode code = service.Document as IDocumentWithCode;
            if (((code != null) && (code.Code != null)) && (code.Code.Language != null))
            {
                return code.Code.Language;
            }
            return null;
        }

        protected override ITextColorizer GetColorizer(IServiceProvider serviceProvider)
        {
            return new AspNetColorizer(serviceProvider);
        }

        protected override ITextControlHost GetTextControlHost(TextControl control, IServiceProvider provider)
        {
            return new AspNetTextControlHost(control, provider);
        }

        protected override string GetTextFromDataObject(IDataObject dataObject, IServiceProvider provider)
        {
            if (dataObject.GetDataPresent(CodeWizard.CodeWizardDataFormat))
            {
                ITextLanguage codeLanguage = this.GetCodeLanguage(provider);
                if (codeLanguage != null)
                {
                    return codeLanguage.GetTextFromDataObject(dataObject, provider);
                }
            }
            return base.GetTextFromDataObject(dataObject, provider);
        }

        public override TextOptions GetTextOptions(IServiceProvider serviceProvider)
        {
            CodeDocumentLanguage codeLanguage = this.GetCodeLanguage(serviceProvider);
            if (codeLanguage != null)
            {
                return codeLanguage.GetTextOptions(serviceProvider);
            }
            return new TextOptions();
        }

        protected override void ShowHelp(IServiceProvider provider, TextBufferLocation location)
        {
            ITextLanguage codeLanguage = this.GetCodeLanguage(provider);
            if (codeLanguage != null)
            {
                codeLanguage.ShowHelp(provider, location);
            }
        }

        protected override bool SupportsDataObject(IServiceProvider provider, IDataObject dataObject)
        {
            if (!base.SupportsDataObject(provider, dataObject))
            {
                return dataObject.GetDataPresent(CodeWizard.CodeWizardDataFormat);
            }
            return true;
        }
    }
}


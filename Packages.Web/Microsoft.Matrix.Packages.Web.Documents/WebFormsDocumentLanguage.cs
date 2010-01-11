namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Windows.Forms;

    public sealed class WebFormsDocumentLanguage : HtmlDocumentLanguage
    {
        internal static WebFormsDocumentLanguage Instance;

        public WebFormsDocumentLanguage() : base("WebForms", null)
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private ITextLanguage GetCodeLanguage(IServiceProvider provider)
        {
            IDocumentDesignerHost service = provider.GetService(typeof(IDocumentDesignerHost)) as IDocumentDesignerHost;
            CodeDocument document = service.Document as CodeDocument;
            if ((document != null) && (document.Language != null))
            {
                return (ITextLanguage) document.Language;
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
            return new WebFormsColorizer(serviceProvider);
        }

        protected override ITextControlHost GetTextControlHost(TextControl control, IServiceProvider provider)
        {
            return new WebFormsTextControlHost(control, provider);
        }

        protected override string GetTextFromDataObject(IDataObject dataObject, IServiceProvider provider)
        {
            IDataObjectMapper dataObjectMapper = ((IDataObjectMappingService) provider.GetService(typeof(IDataObjectMappingService))).GetDataObjectMapper(dataObject, DataFormats.Text);
            if (dataObjectMapper != null)
            {
                DataObject mappedDataObject = new DataObject();
                dataObjectMapper.PerformMapping(provider, dataObject, mappedDataObject);
                if (mappedDataObject.GetDataPresent(DataFormats.Text))
                {
                    return (string) mappedDataObject.GetData(DataFormats.Text);
                }
            }
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
            return HtmlDocumentLanguage.Instance.GetTextOptions(serviceProvider);
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
            bool flag = false;
            IDataObjectMappingService service = (IDataObjectMappingService) provider.GetService(typeof(IDataObjectMappingService));
            if (service != null)
            {
                IDataObjectMapper dataObjectMapper = service.GetDataObjectMapper(dataObject, DataFormats.Text);
                if (dataObjectMapper != null)
                {
                    if (dataObjectMapper.CanMapDataObject(provider, dataObject))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            if (!flag && dataObject.GetDataPresent(CodeWizard.CodeWizardDataFormat))
            {
                ITextLanguage codeLanguage = this.GetCodeLanguage(provider);
                if (codeLanguage != null)
                {
                    flag = codeLanguage.SupportsDataObject(provider, dataObject);
                }
            }
            if (!base.SupportsDataObject(provider, dataObject))
            {
                return flag;
            }
            return true;
        }
    }
}


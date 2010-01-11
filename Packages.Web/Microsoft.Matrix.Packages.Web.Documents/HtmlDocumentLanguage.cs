namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Windows.Forms;

    public class HtmlDocumentLanguage : TextDocumentLanguage
    {
        private ITextColorizer _colorizer;
        internal static HtmlDocumentLanguage Instance;

        public HtmlDocumentLanguage() : this("HTML")
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        protected HtmlDocumentLanguage(string name) : base(name)
        {
        }

        protected HtmlDocumentLanguage(string name, TextOptions options) : base(name, options)
        {
        }

        protected override ITextColorizer GetColorizer(IServiceProvider serviceProvider)
        {
            if (this._colorizer == null)
            {
                this._colorizer = new HtmlColorizer();
            }
            return this._colorizer;
        }

        protected override IDataObject GetDataObjectFromText(string text)
        {
            DataObject obj2 = new DataObject();
            obj2.SetData(DataFormats.Text, text);
            obj2.SetData(DataFormats.Html, text);
            return obj2;
        }

        protected override ITextControlHost GetTextControlHost(TextControl control, IServiceProvider provider)
        {
            return new HtmlTextControlHost(control);
        }

        protected override string GetTextFromDataObject(IDataObject dataObject, IServiceProvider serviceProvider)
        {
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                return base.GetTextFromDataObject(dataObject, serviceProvider);
            }
            if (!dataObject.GetDataPresent(DataFormats.Html))
            {
                return string.Empty;
            }
            string str = dataObject.GetData(DataFormats.Html).ToString();
            int index = str.IndexOf("<!--StartFragment-->");
            int num2 = str.LastIndexOf("<!--EndFragment-->");
            if ((index != -1) && (num2 != -1))
            {
                index += 20;
                str = str.Substring(index, num2 - index);
            }
            return str;
        }

        protected override bool SupportsDataObject(IServiceProvider provider, IDataObject dataObject)
        {
            if (!base.SupportsDataObject(provider, dataObject))
            {
                return dataObject.GetDataPresent(DataFormats.Html);
            }
            return true;
        }
    }
}


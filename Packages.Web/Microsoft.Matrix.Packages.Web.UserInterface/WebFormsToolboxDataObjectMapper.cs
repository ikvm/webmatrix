namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Utility;
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Windows.Forms;

    public sealed class WebFormsToolboxDataObjectMapper : IDataObjectMapper
    {
        public bool CanMapDataObject(IServiceProvider serviceProvider, IDataObject dataObject)
        {
            return (dataObject.GetDataPresent(WebFormsToolboxDataItem.WebFormsToolboxDataFormat) && this.IsValidDataObject(serviceProvider, dataObject));
        }

        private bool IsValidDataObject(IServiceProvider serviceProvider, IDataObject dataObject)
        {
            IDocumentDesignerHost service = (IDocumentDesignerHost) serviceProvider.GetService(typeof(IDocumentDesignerHost));
            Type data = dataObject.GetData(WebFormsToolboxDataItem.WebFormsToolboxDataFormat) as Type;
            return (data != null);
        }

        public bool PerformMapping(IServiceProvider serviceProvider, IDataObject originalDataObject, DataObject mappedDataObject)
        {
            if (this.IsValidDataObject(serviceProvider, originalDataObject))
            {
                IDesignerHost service = (IDesignerHost) serviceProvider.GetService(typeof(IDesignerHost));
                if (service == null)
                {
                    return false;
                }
                string input = null;
                Type data = originalDataObject.GetData(WebFormsToolboxDataItem.WebFormsToolboxDataFormat) as Type;
                if (data != null)
                {
                    string format = string.Empty;
                    object[] customAttributes = data.GetCustomAttributes(typeof(ToolboxDataAttribute), false);
                    if (customAttributes.Length > 0)
                    {
                        ToolboxDataAttribute attribute = (ToolboxDataAttribute) customAttributes[0];
                        format = attribute.Data;
                    }
                    if ((format == null) || (format.Length == 0))
                    {
                        format = "<{0}:" + data.Name + " runat=\"server\"></{0}:" + data.Name + ">";
                    }
                    string tagPrefix = ((IWebFormReferenceManager) service.GetService(typeof(IWebFormReferenceManager))).GetTagPrefix(data);
                    WebFormsDesignView view = service.GetService(typeof(IDesignView)) as WebFormsDesignView;
                    if (view != null)
                    {
                        view.RegisterNamespace(tagPrefix);
                    }
                    input = string.Format(format, tagPrefix);
                    StringWriter output = new StringWriter();
                    new HtmlFormatter().Format(input, output, new HtmlFormatterOptions(' ', 4, 80, HtmlFormatterCase.LowerCase, HtmlFormatterCase.LowerCase, true));
                    input = output.ToString();
                }
                if (input != null)
                {
                    mappedDataObject.SetData(DataFormats.Html, input);
                    mappedDataObject.SetData(DataFormats.Text, input);
                    return true;
                }
            }
            return false;
        }
    }
}


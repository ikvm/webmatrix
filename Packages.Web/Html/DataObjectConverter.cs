namespace Microsoft.Matrix.Packages.Web.Html
{
    using System;
    using System.Windows.Forms;

    public class DataObjectConverter
    {
        public virtual DataObjectConverterInfo CanConvertToHtml(IDataObject dataObject)
        {
            return DataObjectConverterInfo.Unhandled;
        }

        public virtual bool ConvertToHtml(IDataObject originalDataObject, DataObject newDataObject)
        {
            return false;
        }
    }
}


namespace Microsoft.Matrix.Core.Services
{
    using System;
    using System.Windows.Forms;

    public interface IDataObjectMapper
    {
        bool CanMapDataObject(IServiceProvider serviceProvider, IDataObject dataObject);
        bool PerformMapping(IServiceProvider serviceProvider, IDataObject originalDataObject, DataObject mappedDataObject);
    }
}


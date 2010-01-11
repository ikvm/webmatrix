namespace Microsoft.Matrix.Core.Services
{
    using System;
    using System.Windows.Forms;

    public interface IDataObjectMappingService
    {
        IDataObjectMapper GetDataObjectMapper(string fromDataFormat, string toDataFormat);
        IDataObjectMapper GetDataObjectMapper(IDataObject dataObj, string toDataFormat);
    }
}


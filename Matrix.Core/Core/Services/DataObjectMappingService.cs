namespace Microsoft.Matrix.Core.Services
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Windows.Forms;

    internal sealed class DataObjectMappingService : IDataObjectMappingService
    {
        private IDictionary _dataObjectMap;
        private IServiceProvider _serviceProvider;

        public DataObjectMappingService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private string GenerateKey(string fromDataFormat, string toDataFormat)
        {
            return (fromDataFormat + toDataFormat);
        }

        IDataObjectMapper IDataObjectMappingService.GetDataObjectMapper(string fromDataFormat, string toDataFormat)
        {
            IDataObjectMapper mapper = null;
            string str = this.GenerateKey(fromDataFormat, toDataFormat);
            object obj2 = this.DataObjectMap[str];
            if (obj2 is IDataObjectMapper)
            {
                return (IDataObjectMapper) obj2;
            }
            if (obj2 is string)
            {
                try
                {
                    string typeName = (string) obj2;
                    Type type = Type.GetType(typeName, false, true);
                    if (type != null)
                    {
                        mapper = (IDataObjectMapper) Activator.CreateInstance(type);
                    }
                }
                catch (Exception)
                {
                }
                this.DataObjectMap[str] = mapper;
            }
            return mapper;
        }

        IDataObjectMapper IDataObjectMappingService.GetDataObjectMapper(IDataObject dataObj, string toDataFormat)
        {
            IDataObjectMapper dataObjectMapper = null;
            foreach (string str in dataObj.GetFormats())
            {
                dataObjectMapper = ((IDataObjectMappingService) this).GetDataObjectMapper(str, toDataFormat);
                if (dataObjectMapper != null)
                {
                    return dataObjectMapper;
                }
            }
            return null;
        }

        private IDictionary DataObjectMap
        {
            get
            {
                if (this._dataObjectMap == null)
                {
                    this._dataObjectMap = new HybridDictionary(true);
                    new DataObjectMappingSectionHandler();
                    ICollection config = (ICollection) ConfigurationSettings.GetConfig("microsoft.matrix/dataObjectMappings");
                    foreach (DataObjectMappingInfo info in config)
                    {
                        this._dataObjectMap[this.GenerateKey(info.FromDataFormat, info.ToDataFormat)] = info.TypeName;
                    }
                }
                return this._dataObjectMap;
            }
        }
    }
}


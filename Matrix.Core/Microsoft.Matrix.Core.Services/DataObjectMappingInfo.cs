namespace Microsoft.Matrix.Core.Services
{
    using System;

    public sealed class DataObjectMappingInfo
    {
        private string _fromDataFormat;
        private string _toDataFormat;
        private string _typeName;

        public DataObjectMappingInfo(string fromDataFormat, string toDataFormat, string typeName)
        {
            this._fromDataFormat = fromDataFormat;
            this._toDataFormat = toDataFormat;
            this._typeName = typeName;
        }

        public string FromDataFormat
        {
            get
            {
                return this._fromDataFormat;
            }
        }

        public string ToDataFormat
        {
            get
            {
                return this._toDataFormat;
            }
        }

        public string TypeName
        {
            get
            {
                return this._typeName;
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class AccessConnectionSettings : ISerializable
    {
        private string _filename;

        public AccessConnectionSettings(string filename)
        {
            this._filename = filename;
        }

        private AccessConnectionSettings(SerializationInfo info, StreamingContext context)
        {
            this._filename = info.GetString("Filename");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Filename", this._filename);
        }

        public string Filename
        {
            get
            {
                return this._filename;
            }
            set
            {
                this._filename = value;
            }
        }
    }
}


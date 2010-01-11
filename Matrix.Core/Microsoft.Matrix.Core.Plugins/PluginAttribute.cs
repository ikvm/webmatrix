namespace Microsoft.Matrix.Core.Plugins
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PluginAttribute : Attribute
    {
        private string _author;
        private string _contactAddress;
        private string _description;
        private string _name;

        public PluginAttribute(string name) : this(name, null, null, null)
        {
        }

        public PluginAttribute(string name, string description) : this(name, null, null, null)
        {
            this._name = name;
            this._description = description;
        }

        public PluginAttribute(string name, string description, string author, string contactAddress)
        {
            this._name = name;
            this._description = description;
            this._author = author;
            this._contactAddress = contactAddress;
        }

        public string Author
        {
            get
            {
                if (this._author == null)
                {
                    return string.Empty;
                }
                return this._author;
            }
        }

        public string ContactAddress
        {
            get
            {
                if (this._contactAddress == null)
                {
                    return string.Empty;
                }
                return this._contactAddress;
            }
        }

        public string Description
        {
            get
            {
                if (this._description == null)
                {
                    return string.Empty;
                }
                return this._description;
            }
        }

        public string Name
        {
            get
            {
                if (this._name == null)
                {
                    return "Plugin";
                }
                return this._name;
            }
        }
    }
}


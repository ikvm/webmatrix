namespace Microsoft.Matrix.Core.Plugins
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal sealed class AddInEntry : ISerializable
    {
        private string _description;
        private bool _includeInMenu;
        private string _name;
        private AddInScope _scope;
        private string _typeName;

        public AddInEntry(Type type)
        {
            this._includeInMenu = false;
            this._typeName = type.AssemblyQualifiedName;
            PluginAttribute attribute = Attribute.GetCustomAttribute(type, typeof(PluginAttribute), false) as PluginAttribute;
            if (attribute == null)
            {
                this._name = type.Name;
            }
            else
            {
                this._name = attribute.Name;
                this._description = attribute.Description;
            }
            if (typeof(DocumentAddIn).IsAssignableFrom(type))
            {
                this._scope = AddInScope.Document;
            }
            else
            {
                this._scope = AddInScope.Global;
            }
        }

        private AddInEntry(SerializationInfo info, StreamingContext context)
        {
            this._includeInMenu = false;
            this._name = info.GetString("Name");
            this._description = info.GetString("Description");
            this._typeName = info.GetString("TypeName");
            this._scope = (AddInScope) info.GetInt32("Scope");
            this._includeInMenu = info.GetBoolean("IncludeInMenu");
        }

        public AddInEntry(string typeName, string name, string description, AddInScope scope)
        {
            this._includeInMenu = false;
            this._name = name;
            this._description = description;
            this._typeName = typeName;
            this._scope = scope;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this._name);
            info.AddValue("Description", this._description);
            info.AddValue("TypeName", this._typeName);
            info.AddValue("Scope", (int) this._scope);
            info.AddValue("IncludeInMenu", this._includeInMenu);
        }

        public override string ToString()
        {
            return this.Name;
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

        public bool IncludeInMenu
        {
            get
            {
                return this._includeInMenu;
            }
            set
            {
                this._includeInMenu = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public AddInScope Scope
        {
            get
            {
                return this._scope;
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


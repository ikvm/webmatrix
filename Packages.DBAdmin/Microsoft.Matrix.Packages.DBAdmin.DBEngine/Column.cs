namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.ComponentModel;
    using System.Data;

    [DefaultProperty("Name")]
    public abstract class Column : ICloneable, ICustomTypeDescriptor
    {
        private bool _allowNulls;
        private string _dataType;
        private string[] _dataTypeList;
        private string _defaultValue;
        private string _description;
        private int _identityIncrement;
        private int _identitySeed;
        private bool _inPrimaryKey;
        private bool _isIdentity;
        private bool _isReadOnly;
        private bool _isRowGuid;
        private bool _isUniqueKey;
        private string _name;
        private int _numericPrecision;
        private int _numericScale;
        private PropertyDescriptorCollection _props;
        private int _size;
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table _table;

        protected Column()
        {
        }

        protected Column(Column original)
        {
            this.AllowNulls = original.AllowNulls;
            this.DataType = original.DataType;
            this.DefaultValue = original.DefaultValue;
            this.Description = original.Description;
            this.IsIdentity = original.IsIdentity;
            this.IdentityIncrement = original.IdentityIncrement;
            this.IdentitySeed = original.IdentitySeed;
            this.InPrimaryKey = original.InPrimaryKey;
            this.IsRowGuid = original.IsRowGuid;
            this.IsReadOnly = original.IsReadOnly;
            this.IsUniqueKey = original.IsUniqueKey;
            this.Name = original.Name;
            this.NumericPrecision = original.NumericPrecision;
            this.NumericScale = original.NumericScale;
            this.Size = original.Size;
        }

        protected Column(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table)
        {
            this._table = table;
        }

        protected abstract Column Clone();
        protected abstract string[] CreateDataTypeList();
        protected abstract PropertyDescriptorCollection CreateProperties(PropertyDescriptorCollection reflectedProperties);
        private void EnsureProperties()
        {
            if (this._props == null)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, true);
                this._props = this.CreateProperties(properties);
            }
        }

        public virtual bool IsEquivalentTo(Column other)
        {
            return (((((this.AllowNulls == other.AllowNulls) && (this.DataType == other.DataType)) && ((this.IsIdentity == other.IsIdentity) && (this.IsRowGuid == other.IsRowGuid))) && (((this.Name == other.Name) && (this.NumericPrecision == other.NumericPrecision)) && (this.NumericScale == other.NumericScale))) && (this.Size == other.Size));
        }

        protected void ResetProperties()
        {
            this._props = null;
            TypeDescriptor.Refresh(this);
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection(null);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return "Column";
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return "Column";
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            this.EnsureProperties();
            return this._props[0];
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            this.EnsureProperties();
            return this._props;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            this.EnsureProperties();
            return this._props;
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        [Category("Behavior"), Description("Whether the column allows null values to be entered.")]
        public virtual bool AllowNulls
        {
            get
            {
                return this._allowNulls;
            }
            set
            {
                this._allowNulls = value;
            }
        }

        [TypeConverter(typeof(DataTypeConverter)), Category("(Default)"), Description("The type of data that this column contains.")]
        public virtual string DataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }

        [Browsable(false)]
        public virtual string[] DataTypeList
        {
            get
            {
                if (this._dataTypeList == null)
                {
                    this._dataTypeList = this.CreateDataTypeList();
                }
                return this._dataTypeList;
            }
        }

        [Browsable(false)]
        public abstract System.Data.DbType DbType { get; }

        [Category("Behavior"), Description("The default value of this column when no value is specified.")]
        public virtual string DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
            }
        }

        [Category("(Default)"), Description("An optional description of this column.")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [Description("Indicates by how much the identity value will increase for each new row in this table."), Category("Behavior")]
        public virtual int IdentityIncrement
        {
            get
            {
                return this._identityIncrement;
            }
            set
            {
                this._identityIncrement = value;
            }
        }

        [Description("The initial value for column if it is an identity column."), Category("Behavior")]
        public virtual int IdentitySeed
        {
            get
            {
                return this._identitySeed;
            }
            set
            {
                this._identitySeed = value;
            }
        }

        [Description("Indicates that this column is in the primary key of the table. More than one column can be in the primary key."), Category("Behavior")]
        public virtual bool InPrimaryKey
        {
            get
            {
                return this._inPrimaryKey;
            }
            set
            {
                this._inPrimaryKey = value;
            }
        }

        [Category("Behavior"), Description("Indicates that this column is an identity column, and its value is determined automatically by the IdentitySeed and IdentityIncrement properties.")]
        public virtual bool IsIdentity
        {
            get
            {
                return this._isIdentity;
            }
            set
            {
                this._isIdentity = value;
            }
        }

        [Browsable(false)]
        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
            set
            {
                this._isReadOnly = value;
            }
        }

        [Description("Indicates that this row is used as the globally unique identifier (GUID) for a row."), Category("Behavior")]
        public virtual bool IsRowGuid
        {
            get
            {
                return this._isRowGuid;
            }
            set
            {
                this._isRowGuid = value;
            }
        }

        [Category("Behavior"), Description("Indicates that this column contains unique values for each row.")]
        public virtual bool IsUniqueKey
        {
            get
            {
                return this._isUniqueKey;
            }
            set
            {
                this._isUniqueKey = value;
            }
        }

        [Category("(Default)"), Description("The name of the column.")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [Description("The total number of digits that can be stored, both to the left and right of the decimal point."), Category("Behavior")]
        public virtual int NumericPrecision
        {
            get
            {
                return this._numericPrecision;
            }
            set
            {
                this._numericPrecision = value;
            }
        }

        [Description("The maximum number of digits that can be stored to the right of the decimal separator."), Category("Behavior")]
        public virtual int NumericScale
        {
            get
            {
                return this._numericScale;
            }
            set
            {
                this._numericScale = value;
            }
        }

        [Category("Behavior"), Description("The maximum number of characters that you can enter in the field.")]
        public virtual int Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        [Browsable(false)]
        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table Table
        {
            get
            {
                return this._table;
            }
        }
    }
}


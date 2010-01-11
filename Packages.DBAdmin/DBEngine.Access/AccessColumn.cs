namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;

    internal sealed class AccessColumn : Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column
    {
        private bool _autoGenerate;
        private static IList _propertyPositionList;

        private AccessColumn()
        {
        }

        private AccessColumn(AccessColumn original) : base(original)
        {
            this.AutoGenerate = original.AutoGenerate;
        }

        public AccessColumn(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table) : base(table)
        {
        }

        protected override Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column Clone()
        {
            return new AccessColumn(this);
        }

        public static Interop.AdoxDataType ConvertAccessToAdoxDataType(AccessDataType accessType)
        {
            switch (accessType)
            {
                case AccessDataType.ReplicationID:
                    return Interop.AdoxDataType.GUID;

                case AccessDataType.LongInteger:
                    return Interop.AdoxDataType.Integer;

                case AccessDataType.Byte:
                    return Interop.AdoxDataType.UnsignedTinyInt;

                case AccessDataType.Currency:
                    return Interop.AdoxDataType.Currency;

                case AccessDataType.DateTime:
                    return Interop.AdoxDataType.Date;

                case AccessDataType.Decimal:
                    return Interop.AdoxDataType.Numeric;

                case AccessDataType.Double:
                    return Interop.AdoxDataType.Double;

                case AccessDataType.Memo:
                    return Interop.AdoxDataType.LongVarWChar;

                case AccessDataType.Integer:
                    return Interop.AdoxDataType.SmallInt;

                case AccessDataType.OleObject:
                    return Interop.AdoxDataType.LongVarBinary;

                case AccessDataType.Single:
                    return Interop.AdoxDataType.Single;

                case AccessDataType.Text:
                    return Interop.AdoxDataType.VarWChar;

                case AccessDataType.YesNo:
                    return Interop.AdoxDataType.Boolean;
            }
            throw new ArgumentException();
        }

        public static AccessDataType ConvertAdoxToAccessDataType(Interop.AdoxDataType adoxType)
        {
            Interop.AdoxDataType type = adoxType;
            if (type <= Interop.AdoxDataType.UnsignedTinyInt)
            {
                switch (type)
                {
                    case Interop.AdoxDataType.SmallInt:
                        return AccessDataType.Integer;

                    case Interop.AdoxDataType.Integer:
                        return AccessDataType.LongInteger;

                    case Interop.AdoxDataType.Single:
                        return AccessDataType.Single;

                    case Interop.AdoxDataType.Double:
                        return AccessDataType.Double;

                    case Interop.AdoxDataType.Currency:
                        return AccessDataType.Currency;

                    case Interop.AdoxDataType.Date:
                        return AccessDataType.DateTime;

                    case Interop.AdoxDataType.BSTR:
                    case ((Interop.AdoxDataType) 9):
                    case (Interop.AdoxDataType.BSTR | Interop.AdoxDataType.SmallInt):
                        goto Label_0087;

                    case Interop.AdoxDataType.Boolean:
                        return AccessDataType.YesNo;

                    case Interop.AdoxDataType.UnsignedTinyInt:
                        return AccessDataType.Byte;
                }
            }
            else if (type != Interop.AdoxDataType.GUID)
            {
                switch (type)
                {
                    case Interop.AdoxDataType.VarWChar:
                        return AccessDataType.Text;

                    case Interop.AdoxDataType.LongVarWChar:
                        return AccessDataType.Memo;

                    case Interop.AdoxDataType.VarBinary:
                        goto Label_0087;

                    case Interop.AdoxDataType.LongVarBinary:
                        return AccessDataType.OleObject;

                    case Interop.AdoxDataType.Numeric:
                        return AccessDataType.Decimal;
                }
            }
            else
            {
                return AccessDataType.ReplicationID;
            }
        Label_0087:
            throw new ArgumentException();
        }

        protected override string[] CreateDataTypeList()
        {
            string[] names = Enum.GetNames(typeof(AccessDataType));
            string[] array = new string[names.Length + 1];
            names.CopyTo(array, 0);
            array[array.Length - 1] = "AutoNumber";
            return array;
        }

        protected override PropertyDescriptorCollection CreateProperties(PropertyDescriptorCollection reflectedProperties)
        {
            ArrayList list = new ArrayList();
            foreach (PropertyDescriptor descriptor in reflectedProperties)
            {
                bool isReadOnly = base.IsReadOnly;
                if ((descriptor.Name == "IdentitySeed") || (descriptor.Name == "IdentityIncrement"))
                {
                    isReadOnly = true;
                }
                AccessDataType accessDataType = GetAccessDataType(this.DataType);
                if ((descriptor.Name == "Size") && !SupportsSize(accessDataType))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "NumericPrecision") && !SupportsNumericPrecision(accessDataType))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "NumericScale") && !SupportsNumericScale(accessDataType))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "AutoGenerate") && !SupportsAutoGenerate(accessDataType))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "AllowNulls") && !SupportsAllowNulls(accessDataType))
                {
                    isReadOnly = true;
                }
                if (descriptor.Name == "IsIdentity")
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "AllowNulls") && this.InPrimaryKey)
                {
                    isReadOnly = true;
                }
                if (isReadOnly)
                {
                    list.Add(TypeDescriptor.CreateProperty(base.GetType(), descriptor, new Attribute[] { ReadOnlyAttribute.Yes }));
                }
                else
                {
                    list.Add(TypeDescriptor.CreateProperty(base.GetType(), descriptor, new Attribute[0]));
                }
            }
            list.Sort(new AccessColumnComparer());
            return new PropertyDescriptorCollection((PropertyDescriptor[]) list.ToArray(typeof(PropertyDescriptor)));
        }

        public static AccessDataType GetAccessDataType(string dataType)
        {
            if (dataType == "AutoNumber")
            {
                return AccessDataType.LongInteger;
            }
            return (AccessDataType) Enum.Parse(typeof(AccessDataType), dataType, true);
        }

        private static int GetDefaultNumericPrecision(AccessDataType dataType)
        {
            switch (dataType)
            {
                case AccessDataType.LongInteger:
                    return 10;

                case AccessDataType.Byte:
                    return 3;

                case AccessDataType.Currency:
                    return 0x13;

                case AccessDataType.Decimal:
                    return 0x12;

                case AccessDataType.Double:
                    return 15;

                case AccessDataType.Integer:
                    return 5;

                case AccessDataType.Single:
                    return 7;
            }
            return 0;
        }

        private static int GetDefaultNumericScale(AccessDataType dataType)
        {
            return 0;
        }

        internal static int GetDefaultSize(AccessDataType dataType)
        {
            switch (dataType)
            {
                case AccessDataType.Text:
                    return 50;

                case AccessDataType.YesNo:
                    return 2;
            }
            return 0;
        }

        internal void Initialize(Interop.IColumn adoxColumn)
        {
            this.Name = adoxColumn.GetName();
            Interop.IProperties properties = adoxColumn.GetProperties();
            Interop.IProperty item = properties.GetItem("Nullable");
            this.AllowNulls = (bool) item.GetValue();
            this.DataType = Enum.GetName(typeof(AccessDataType), ConvertAdoxToAccessDataType((Interop.AdoxDataType) adoxColumn.GetType()));
            item = properties.GetItem("Default");
            if (item.GetValue() != null)
            {
                this.DefaultValue = item.GetValue().ToString();
            }
            item = properties.GetItem("Description");
            this.Description = (string) item.GetValue();
            item = properties.GetItem("Increment");
            this.IdentityIncrement = (int) item.GetValue();
            item = properties.GetItem("Seed");
            this.IdentitySeed = (int) item.GetValue();
            item = properties.GetItem("Autoincrement");
            this.IsIdentity = (bool) item.GetValue();
            item = properties.GetItem("Jet OLEDB:AutoGenerate");
            this.AutoGenerate = (bool) item.GetValue();
            this.NumericPrecision = adoxColumn.GetPrecision();
            this.NumericScale = adoxColumn.GetNumericScale();
            this.Size = adoxColumn.GetDefinedSize();
            foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key key in base.Table.Keys)
            {
                if (key.KeyType == KeyType.Primary)
                {
                    if (key.ReferencedColumns.Get(this.Name) != null)
                    {
                        this.InPrimaryKey = true;
                    }
                }
                else if (((key.KeyType == KeyType.Unique) && (key.ReferencedColumns.Count == 1)) && (key.ReferencedColumns.Get(this.Name) != null))
                {
                    this.IsUniqueKey = true;
                }
            }
        }

        public override bool IsEquivalentTo(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column other)
        {
            return (base.IsEquivalentTo(other) && (this.AutoGenerate == ((AccessColumn) other).AutoGenerate));
        }

        private static bool IsValidNumericPrecision(AccessDataType dataType, int numericPrecision)
        {
            if (dataType != AccessDataType.Decimal)
            {
                return (numericPrecision == GetDefaultNumericPrecision(dataType));
            }
            return ((numericPrecision >= 1) && (numericPrecision <= 0x1c));
        }

        private static bool IsValidNumericScale(AccessDataType dataType, int numericScale, int numericPrecision)
        {
            if (dataType != AccessDataType.Decimal)
            {
                return (numericScale == GetDefaultNumericScale(dataType));
            }
            return ((numericScale >= 0) && (numericScale <= numericPrecision));
        }

        private static bool IsValidSize(AccessDataType dataType, int size)
        {
            if (dataType != AccessDataType.Text)
            {
                return (size == GetDefaultSize(dataType));
            }
            return ((size >= 1) && (size <= 0xff));
        }

        public static bool SupportsAllowNulls(AccessDataType dataType)
        {
            if (dataType == AccessDataType.YesNo)
            {
                return false;
            }
            return true;
        }

        public static bool SupportsAutoGenerate(AccessDataType dataType)
        {
            AccessDataType type = dataType;
            return (type == AccessDataType.ReplicationID);
        }

        public static bool SupportsIdentity(AccessDataType dataType)
        {
            AccessDataType type = dataType;
            return (type == AccessDataType.LongInteger);
        }

        public static bool SupportsNumericPrecision(AccessDataType dataType)
        {
            AccessDataType type = dataType;
            return (type == AccessDataType.Decimal);
        }

        public static bool SupportsNumericScale(AccessDataType dataType)
        {
            AccessDataType type = dataType;
            return (type == AccessDataType.Decimal);
        }

        public static bool SupportsSize(AccessDataType dataType)
        {
            AccessDataType type = dataType;
            return (type == AccessDataType.Text);
        }

        [Description("Whether the values of this column should be automatically generated when a new row is added."), Category("Behavior")]
        public bool AutoGenerate
        {
            get
            {
                return this._autoGenerate;
            }
            set
            {
                if (this._autoGenerate != value)
                {
                    this._autoGenerate = value;
                    AccessDataType accessDataType = GetAccessDataType(this.DataType);
                    if (SupportsAutoGenerate(accessDataType))
                    {
                        if (value)
                        {
                            if ((accessDataType == AccessDataType.ReplicationID) && ((this.DefaultValue == null) || (this.DefaultValue == "")))
                            {
                                this.DefaultValue = "GenGUID()";
                            }
                        }
                        else if ((accessDataType == AccessDataType.ReplicationID) && (this.DefaultValue == "GenGUID()"))
                        {
                            this.DefaultValue = null;
                        }
                    }
                    base.ResetProperties();
                }
            }
        }

        public override string DataType
        {
            get
            {
                if ((base.DataType == "LongInteger") && this.IsIdentity)
                {
                    return "AutoNumber";
                }
                return base.DataType;
            }
            set
            {
                if (this.DataType != value)
                {
                    if (value == "AutoNumber")
                    {
                        base.DataType = "LongInteger";
                        this.IsIdentity = true;
                        base.ResetProperties();
                    }
                    else
                    {
                        if (this.DataType == "AutoNumber")
                        {
                            this.IsIdentity = false;
                        }
                        base.DataType = value;
                        AccessDataType accessDataType = GetAccessDataType(this.DataType);
                        if (!SupportsIdentity(accessDataType))
                        {
                            this.IsIdentity = false;
                        }
                        if (!SupportsAutoGenerate(accessDataType))
                        {
                            this.AutoGenerate = false;
                        }
                        if (!SupportsAllowNulls(accessDataType))
                        {
                            this.AllowNulls = false;
                        }
                        this.NumericPrecision = GetDefaultNumericPrecision(accessDataType);
                        this.NumericScale = GetDefaultNumericScale(accessDataType);
                        this.Size = GetDefaultSize(accessDataType);
                        base.ResetProperties();
                    }
                }
            }
        }

        public override System.Data.DbType DbType
        {
            get
            {
                switch (GetAccessDataType(this.DataType))
                {
                    case AccessDataType.ReplicationID:
                        return System.Data.DbType.Guid;

                    case AccessDataType.LongInteger:
                        return System.Data.DbType.Int32;

                    case AccessDataType.Byte:
                        return System.Data.DbType.Byte;

                    case AccessDataType.Currency:
                        return System.Data.DbType.Currency;

                    case AccessDataType.DateTime:
                        return System.Data.DbType.DateTime;

                    case AccessDataType.Decimal:
                        return System.Data.DbType.Decimal;

                    case AccessDataType.Double:
                        return System.Data.DbType.Double;

                    case AccessDataType.Memo:
                        return System.Data.DbType.String;

                    case AccessDataType.Integer:
                        return System.Data.DbType.Int32;

                    case AccessDataType.OleObject:
                        return System.Data.DbType.Object;

                    case AccessDataType.Single:
                        return System.Data.DbType.Single;

                    case AccessDataType.Text:
                        return System.Data.DbType.String;

                    case AccessDataType.YesNo:
                        return System.Data.DbType.Boolean;
                }
                throw new ArgumentException();
            }
        }

        public static AccessColumn DefaultColumn
        {
            get
            {
                AccessColumn column = new AccessColumn();
                column.Name = "Column0";
                column.DataType = "Integer";
                return column;
            }
        }

        public override string Description
        {
            get
            {
                return base.Description;
            }
            set
            {
                base.Description = value;
            }
        }

        public override int IdentityIncrement
        {
            get
            {
                return base.IdentityIncrement;
            }
            set
            {
                if (base.IdentityIncrement != value)
                {
                    GetAccessDataType(this.DataType);
                    if (this.IsIdentity && (value == 0))
                    {
                        throw new ArgumentException("IdentityIncrement must not be 0");
                    }
                    base.IdentityIncrement = value;
                }
            }
        }

        public override int IdentitySeed
        {
            get
            {
                return base.IdentitySeed;
            }
            set
            {
                if (base.IdentitySeed != value)
                {
                    base.IdentitySeed = value;
                }
            }
        }

        public override bool InPrimaryKey
        {
            get
            {
                return base.InPrimaryKey;
            }
            set
            {
                if (base.InPrimaryKey != value)
                {
                    base.InPrimaryKey = value;
                    if (value)
                    {
                        this.AllowNulls = false;
                    }
                    base.ResetProperties();
                }
            }
        }

        public override bool IsIdentity
        {
            get
            {
                return base.IsIdentity;
            }
            set
            {
                if (base.IsIdentity != value)
                {
                    base.IsIdentity = value;
                    if (!value)
                    {
                        this.IdentityIncrement = 0;
                        this.IdentitySeed = 0;
                    }
                    else
                    {
                        this.IdentityIncrement = 1;
                        this.IdentitySeed = 1;
                    }
                    base.ResetProperties();
                }
            }
        }

        [Browsable(false)]
        public override bool IsRowGuid
        {
            get
            {
                return base.IsRowGuid;
            }
            set
            {
                base.IsRowGuid = value;
            }
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                if (base.Name != value)
                {
                    if (!AccessHelper.IsValidIdentifier(value))
                    {
                        throw new ArgumentException("The column name contains invalid characters.");
                    }
                    base.Name = value;
                }
            }
        }

        public override int NumericPrecision
        {
            get
            {
                return base.NumericPrecision;
            }
            set
            {
                if (base.NumericPrecision != value)
                {
                    if (!IsValidNumericPrecision(GetAccessDataType(this.DataType), value))
                    {
                        throw new ArgumentException("Invalid numeric precision.");
                    }
                    base.NumericPrecision = value;
                    base.ResetProperties();
                }
            }
        }

        public override int NumericScale
        {
            get
            {
                return base.NumericScale;
            }
            set
            {
                if (base.NumericScale != value)
                {
                    if (!IsValidNumericScale(GetAccessDataType(this.DataType), value, this.NumericPrecision))
                    {
                        throw new ArgumentException("Invalid numeric precision.");
                    }
                    base.NumericScale = value;
                    base.ResetProperties();
                }
            }
        }

        private static IList PropertyPositionList
        {
            get
            {
                if (_propertyPositionList == null)
                {
                    _propertyPositionList = new ArrayList();
                    _propertyPositionList.Add("Name");
                    _propertyPositionList.Add("Description");
                    _propertyPositionList.Add("DataType");
                    _propertyPositionList.Add("AutoGenerate");
                    _propertyPositionList.Add("Size");
                    _propertyPositionList.Add("AllowNulls");
                    _propertyPositionList.Add("DefaultValue");
                    _propertyPositionList.Add("NumericPrecision");
                    _propertyPositionList.Add("NumericScale");
                    _propertyPositionList.Add("IsIdentity");
                    _propertyPositionList.Add("IdentitySeed");
                    _propertyPositionList.Add("IdentityIncrement");
                    _propertyPositionList.Add("IsRowGuid");
                    _propertyPositionList.Add("InPrimaryKey");
                    _propertyPositionList.Add("IsUniqueKey");
                }
                return _propertyPositionList;
            }
        }

        public override int Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                if (base.Size != value)
                {
                    if (!IsValidSize(GetAccessDataType(this.DataType), value))
                    {
                        throw new ArgumentException("Invalid size.");
                    }
                    base.Size = value;
                }
            }
        }

        private class AccessColumnComparer : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                PropertyDescriptor descriptor = x as PropertyDescriptor;
                PropertyDescriptor descriptor2 = y as PropertyDescriptor;
                return (AccessColumn.PropertyPositionList.IndexOf(descriptor.Name) - AccessColumn.PropertyPositionList.IndexOf(descriptor2.Name));
            }
        }
    }
}


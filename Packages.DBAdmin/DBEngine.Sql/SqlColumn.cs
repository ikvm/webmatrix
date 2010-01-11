namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;

    internal sealed class SqlColumn : Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column
    {
        private static IList _propertyPositionList;

        private SqlColumn()
        {
        }

        private SqlColumn(SqlColumn original) : base(original)
        {
        }

        public SqlColumn(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table) : base(table)
        {
        }

        protected override Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column Clone()
        {
            return new SqlColumn(this);
        }

        protected override string[] CreateDataTypeList()
        {
            string[] names = Enum.GetNames(typeof(SqlDataType));
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = names[i].ToLower();
            }
            return names;
        }

        protected override PropertyDescriptorCollection CreateProperties(PropertyDescriptorCollection reflectedProperties)
        {
            ArrayList list = new ArrayList();
            foreach (PropertyDescriptor descriptor in reflectedProperties)
            {
                bool isReadOnly = base.IsReadOnly;
                if (!this.IsIdentity && ((descriptor.Name == "IdentitySeed") || (descriptor.Name == "IdentityIncrement")))
                {
                    isReadOnly = true;
                }
                SqlDataType sqlDataType = GetSqlDataType(this.DataType);
                if ((descriptor.Name == "Size") && !SupportsSize(sqlDataType))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "NumericPrecision") && !SupportsNumericPrecision(sqlDataType))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "NumericScale") && !SupportsNumericScale(sqlDataType))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "IsIdentity") && !SupportsIdentity(sqlDataType, this.NumericScale))
                {
                    isReadOnly = true;
                }
                if ((descriptor.Name == "IsRowGuid") && !SupportsRowGuid(sqlDataType))
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
            list.Sort(new SqlColumnComparer());
            return new PropertyDescriptorCollection((PropertyDescriptor[]) list.ToArray(typeof(PropertyDescriptor)));
        }

        private static int GetDefaultNumericPrecision(SqlDataType dataType)
        {
            switch (dataType)
            {
                case SqlDataType.Decimal:
                case SqlDataType.Numeric:
                    return 0x12;

                case SqlDataType.Float:
                    return 0x35;

                case SqlDataType.Int:
                case SqlDataType.SmallMoney:
                    return 10;

                case SqlDataType.Money:
                case SqlDataType.BigInt:
                    return 0x13;

                case SqlDataType.Real:
                    return 0x18;

                case SqlDataType.SmallInt:
                    return 5;

                case SqlDataType.TinyInt:
                    return 3;
            }
            return 0;
        }

        private static int GetDefaultNumericScale(SqlDataType dataType)
        {
            SqlDataType type = dataType;
            if ((type != SqlDataType.Money) && (type != SqlDataType.SmallMoney))
            {
                return 0;
            }
            return 4;
        }

        private static int GetDefaultSize(SqlDataType dataType, int numericPrecision)
        {
            switch (dataType)
            {
                case SqlDataType.BigInt:
                case SqlDataType.DateTime:
                case SqlDataType.Float:
                case SqlDataType.Money:
                case SqlDataType.Timestamp:
                    return 8;

                case SqlDataType.Binary:
                case SqlDataType.Char:
                case SqlDataType.NChar:
                case SqlDataType.NVarChar:
                case SqlDataType.VarBinary:
                case SqlDataType.VarChar:
                    return 200;

                case SqlDataType.Bit:
                case SqlDataType.TinyInt:
                    return 1;

                case SqlDataType.Decimal:
                case SqlDataType.Numeric:
                    if (numericPrecision > 9)
                    {
                        if (numericPrecision <= 0x13)
                        {
                            return 9;
                        }
                        if (numericPrecision <= 0x1c)
                        {
                            return 13;
                        }
                        if (numericPrecision > 0x26)
                        {
                            throw new ArgumentException("Invalid precision for this data type.");
                        }
                        return 0x11;
                    }
                    return 5;

                case SqlDataType.Image:
                case SqlDataType.NText:
                case SqlDataType.Text:
                case SqlDataType.UniqueIdentifier:
                    return 0x10;

                case SqlDataType.Int:
                case SqlDataType.Real:
                case SqlDataType.SmallDateTime:
                case SqlDataType.SmallMoney:
                    return 4;

                case SqlDataType.SmallInt:
                    return 2;
            }
            return 0;
        }

        public static SqlDataType GetSqlDataType(string dataType)
        {
            return (SqlDataType) Enum.Parse(typeof(SqlDataType), dataType, true);
        }

        internal void Initialize(Interop.IColumn dmoColumn)
        {
            this.Name = dmoColumn.GetName();
            this.DataType = dmoColumn.GetPhysicalDatatype();
            SqlDataType sqlDataType = GetSqlDataType(this.DataType);
            this.AllowNulls = dmoColumn.GetAllowNulls();
            string text = dmoColumn.GetDRIDefault().GetText();
            if (((text != null) && text.StartsWith("(")) && text.EndsWith(")"))
            {
                this.DefaultValue = text.Substring(1, text.Length - 2);
            }
            else
            {
                this.DefaultValue = text;
            }
            if (SupportsRowGuid(sqlDataType))
            {
                this.IsRowGuid = dmoColumn.GetIsRowGuidCol();
            }
            if (SupportsNumericPrecision(sqlDataType))
            {
                this.NumericPrecision = dmoColumn.GetNumericPrecision();
            }
            if (SupportsNumericScale(sqlDataType))
            {
                this.NumericScale = dmoColumn.GetNumericScale();
            }
            if (SupportsSize(sqlDataType))
            {
                this.Size = dmoColumn.GetLength();
            }
            if (SupportsIdentity(sqlDataType, this.NumericScale))
            {
                this.IsIdentity = dmoColumn.GetIdentity();
                if (this.IsIdentity)
                {
                    this.IdentityIncrement = dmoColumn.GetIdentityIncrement();
                    this.IdentitySeed = dmoColumn.GetIdentitySeed();
                }
            }
            this.InPrimaryKey = dmoColumn.GetInPrimaryKey();
            foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key key in base.Table.Keys)
            {
                if (((key.KeyType == KeyType.Unique) && (key.ReferencedColumns.Count == 1)) && (key.ReferencedColumns.GetValues(this.Name) != null))
                {
                    this.IsUniqueKey = true;
                }
            }
        }

        private static bool IsValidIdentityIncrement(SqlDataType dataType, int identityIncrement, int numericPrecision)
        {
            return ((identityIncrement != 0) && IsValidIdentityValue(dataType, identityIncrement, numericPrecision));
        }

        private static bool IsValidIdentitySeed(SqlDataType dataType, int identitySeed, int numericPrecision)
        {
            return IsValidIdentityValue(dataType, identitySeed, numericPrecision);
        }

        private static bool IsValidIdentityValue(SqlDataType dataType, int identityValue, int numericPrecision)
        {
            SqlDataType type = dataType;
            if (type <= SqlDataType.Int)
            {
                switch (type)
                {
                    case SqlDataType.BigInt:
                    case SqlDataType.Decimal:
                    case SqlDataType.Int:
                        goto Label_0023;
                }
                goto Label_0045;
            }
            if (((type != SqlDataType.Numeric) && (type != SqlDataType.SmallInt)) && (type != SqlDataType.TinyInt))
            {
                goto Label_0045;
            }
        Label_0023:
            if (identityValue == 0)
            {
                return true;
            }
            int num = ((int) Math.Floor(Math.Log10((double) Math.Abs(identityValue)))) + 1;
            return (num <= numericPrecision);
        Label_0045:
            return false;
        }

        private static bool IsValidNumericPrecision(SqlDataType dataType, int numericPrecision)
        {
            SqlDataType type = dataType;
            if ((type != SqlDataType.Decimal) && (type != SqlDataType.Numeric))
            {
                return (numericPrecision == GetDefaultNumericPrecision(dataType));
            }
            return ((numericPrecision >= 1) && (numericPrecision <= 0x26));
        }

        private static bool IsValidNumericScale(SqlDataType dataType, int numericScale, int numericPrecision)
        {
            SqlDataType type = dataType;
            if ((type != SqlDataType.Decimal) && (type != SqlDataType.Numeric))
            {
                return (numericScale == GetDefaultNumericScale(dataType));
            }
            return ((numericScale >= 0) && (numericScale <= numericPrecision));
        }

        private static bool IsValidSize(SqlDataType dataType, int size, int numericPrecision)
        {
            switch (dataType)
            {
                case SqlDataType.VarBinary:
                case SqlDataType.VarChar:
                case SqlDataType.Binary:
                case SqlDataType.Char:
                    return ((size >= 1) && (size <= 0x1f40));

                case SqlDataType.NVarChar:
                case SqlDataType.NChar:
                    return ((size >= 1) && (size <= 0xfa0));
            }
            return (size == GetDefaultSize(dataType, numericPrecision));
        }

        private static bool SupportsIdentity(SqlDataType dataType, int numericScale)
        {
            if (numericScale > 0)
            {
                return false;
            }
            SqlDataType type = dataType;
            if (type <= SqlDataType.Int)
            {
                switch (type)
                {
                    case SqlDataType.BigInt:
                    case SqlDataType.Decimal:
                    case SqlDataType.Int:
                        goto Label_0029;
                }
                goto Label_002B;
            }
            if (((type != SqlDataType.Numeric) && (type != SqlDataType.SmallInt)) && (type != SqlDataType.TinyInt))
            {
                goto Label_002B;
            }
        Label_0029:
            return true;
        Label_002B:
            return false;
        }

        private static bool SupportsNumericPrecision(SqlDataType dataType)
        {
            SqlDataType type = dataType;
            if ((type != SqlDataType.Decimal) && (type != SqlDataType.Numeric))
            {
                return false;
            }
            return true;
        }

        private static bool SupportsNumericScale(SqlDataType dataType)
        {
            SqlDataType type = dataType;
            if ((type != SqlDataType.Decimal) && (type != SqlDataType.Numeric))
            {
                return false;
            }
            return true;
        }

        private static bool SupportsRowGuid(SqlDataType dataType)
        {
            SqlDataType type = dataType;
            return (type == SqlDataType.UniqueIdentifier);
        }

        private static bool SupportsSize(SqlDataType dataType)
        {
            switch (dataType)
            {
                case SqlDataType.VarBinary:
                case SqlDataType.VarChar:
                case SqlDataType.NVarChar:
                case SqlDataType.Binary:
                case SqlDataType.Char:
                case SqlDataType.NChar:
                    return true;
            }
            return false;
        }

        public override string DataType
        {
            get
            {
                return base.DataType;
            }
            set
            {
                if (base.DataType != value)
                {
                    base.DataType = value;
                    SqlDataType sqlDataType = GetSqlDataType(this.DataType);
                    this.NumericPrecision = GetDefaultNumericPrecision(sqlDataType);
                    this.NumericScale = GetDefaultNumericScale(sqlDataType);
                    this.Size = GetDefaultSize(sqlDataType, this.NumericPrecision);
                    if (!SupportsIdentity(sqlDataType, this.NumericScale))
                    {
                        this.IsIdentity = false;
                    }
                    else if (this.IsIdentity)
                    {
                        if (!IsValidIdentitySeed(sqlDataType, this.IdentitySeed, this.NumericPrecision))
                        {
                            this.IdentitySeed = 1;
                        }
                        if (!IsValidIdentityIncrement(sqlDataType, this.IdentityIncrement, this.NumericPrecision))
                        {
                            this.IdentityIncrement = 1;
                        }
                    }
                    if (!SupportsRowGuid(sqlDataType))
                    {
                        this.IsRowGuid = false;
                    }
                    base.ResetProperties();
                }
            }
        }

        public override System.Data.DbType DbType
        {
            get
            {
                switch (GetSqlDataType(this.DataType))
                {
                    case SqlDataType.BigInt:
                    case SqlDataType.Int:
                    case SqlDataType.SmallInt:
                    case SqlDataType.TinyInt:
                        return System.Data.DbType.Int32;

                    case SqlDataType.Binary:
                        return System.Data.DbType.Binary;

                    case SqlDataType.Bit:
                        return System.Data.DbType.Boolean;

                    case SqlDataType.Char:
                        return System.Data.DbType.StringFixedLength;

                    case SqlDataType.DateTime:
                    case SqlDataType.SmallDateTime:
                        return System.Data.DbType.DateTime;

                    case SqlDataType.Decimal:
                    case SqlDataType.Numeric:
                        return System.Data.DbType.Decimal;

                    case SqlDataType.Float:
                        return System.Data.DbType.Single;

                    case SqlDataType.Image:
                        return System.Data.DbType.Binary;

                    case SqlDataType.Money:
                    case SqlDataType.SmallMoney:
                        return System.Data.DbType.Currency;

                    case SqlDataType.NChar:
                    case SqlDataType.NText:
                    case SqlDataType.NVarChar:
                    case SqlDataType.Text:
                    case SqlDataType.VarChar:
                        return System.Data.DbType.String;

                    case SqlDataType.Real:
                        return System.Data.DbType.Double;

                    case SqlDataType.Sql_Variant:
                        return System.Data.DbType.Object;

                    case SqlDataType.Timestamp:
                        return System.Data.DbType.Time;

                    case SqlDataType.UniqueIdentifier:
                        return System.Data.DbType.Guid;

                    case SqlDataType.VarBinary:
                        return System.Data.DbType.Binary;
                }
                throw new ArgumentException();
            }
        }

        public static SqlColumn DefaultColumn
        {
            get
            {
                SqlColumn column = new SqlColumn();
                column.Name = "Column0";
                column.DataType = "int";
                return column;
            }
        }

        [Browsable(false)]
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
                    SqlDataType sqlDataType = GetSqlDataType(this.DataType);
                    if (this.IsIdentity && !IsValidIdentityIncrement(sqlDataType, value, this.NumericPrecision))
                    {
                        throw new ArgumentException("Invalid identity increment.");
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
                    SqlDataType sqlDataType = GetSqlDataType(this.DataType);
                    if (this.IsIdentity && !IsValidIdentitySeed(sqlDataType, value, this.NumericPrecision))
                    {
                        throw new ArgumentException("Invalid identity seed.");
                    }
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
                    if (!SqlHelper.IsValidIdentifier(value))
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
                    SqlDataType sqlDataType = GetSqlDataType(this.DataType);
                    if (!IsValidNumericPrecision(sqlDataType, value))
                    {
                        throw new ArgumentException("Invalid numeric precision.");
                    }
                    base.NumericPrecision = value;
                    this.Size = GetDefaultSize(sqlDataType, value);
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
                    if (!IsValidNumericScale(GetSqlDataType(this.DataType), value, this.NumericPrecision))
                    {
                        throw new ArgumentException("Invalid numeric scale.");
                    }
                    base.NumericScale = value;
                    if (value > 0)
                    {
                        this.IsIdentity = false;
                    }
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
                    if (!IsValidSize(GetSqlDataType(this.DataType), value, this.NumericPrecision))
                    {
                        throw new ArgumentException("Invalid size.");
                    }
                    base.Size = value;
                }
            }
        }

        private class SqlColumnComparer : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                PropertyDescriptor descriptor = x as PropertyDescriptor;
                PropertyDescriptor descriptor2 = y as PropertyDescriptor;
                return (SqlColumn.PropertyPositionList.IndexOf(descriptor.Name) - SqlColumn.PropertyPositionList.IndexOf(descriptor2.Name));
            }
        }
    }
}


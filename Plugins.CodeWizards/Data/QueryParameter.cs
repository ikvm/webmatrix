namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using System;
    using System.Data;

    internal class QueryParameter
    {
        private string _name;
        private string _operandString;
        private DbType _type;

        public QueryParameter(string name, string operandString, DbType type)
        {
            this._name = name;
            this._type = type;
            this._operandString = operandString;
        }

        public static System.Type GetTypeFromDbType(DbType type)
        {
            switch (((int) type))
            {
                case 0:
                case 0x10:
                case 0x16:
                case 0x17:
                    return typeof(string);

                case 1:
                    return typeof(byte[]);

                case 2:
                    return typeof(byte);

                case 3:
                    return typeof(bool);

                case 4:
                case 7:
                    return typeof(decimal);

                case 5:
                case 6:
                case 0x11:
                    return typeof(DateTime);

                case 8:
                    return typeof(double);

                case 9:
                    return typeof(Guid);

                case 10:
                    return typeof(short);

                case 11:
                    return typeof(int);

                case 12:
                    return typeof(long);

                case 13:
                    return typeof(object);

                case 14:
                    return typeof(byte);

                case 15:
                    return typeof(float);

                case 0x12:
                    return typeof(ushort);

                case 0x13:
                    return typeof(uint);

                case 20:
                    return typeof(ulong);

                case 0x15:
                    return typeof(int);
            }
            return typeof(int);
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public string OperandString
        {
            get
            {
                return this._operandString;
            }
        }

        public DbType Type
        {
            get
            {
                return this._type;
            }
        }
    }
}


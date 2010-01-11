namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    internal sealed class Interop
    {
        public const int ActionEnum_adAccessDeny = 3;
        public const int ActionEnum_adAccessGrant = 1;
        public const int ActionEnum_adAccessRevoke = 4;
        public const int ActionEnum_adAccessSet = 2;
        public const int AllowNullsEnum_adIndexNullsAllow = 0;
        public const int AllowNullsEnum_adIndexNullsDisallow = 1;
        public const int AllowNullsEnum_adIndexNullsIgnore = 2;
        public const int AllowNullsEnum_adIndexNullsIgnoreAny = 4;
        public const int ColumnAttributesEnum_adColFixed = 1;
        public const int ColumnAttributesEnum_adColNullable = 2;
        public const int InheritTypeEnum_adInheritBoth = 3;
        public const int InheritTypeEnum_adInheritContainers = 2;
        public const int InheritTypeEnum_adInheritNone = 0;
        public const int InheritTypeEnum_adInheritNoPropogate = 4;
        public const int InheritTypeEnum_adInheritObjects = 1;
        public const int ObjectTypeEnum_adPermObjColumn = 2;
        public const int ObjectTypeEnum_adPermObjDatabase = 3;
        public const int ObjectTypeEnum_adPermObjProcedure = 4;
        public const int ObjectTypeEnum_adPermObjTable = 1;
        public const int ObjectTypeEnum_adPermObjView = 5;
        public const int RightsEnum_adRightCreate = 0x4000;
        public const int RightsEnum_adRightDelete = 0x10000;
        public const int RightsEnum_adRightDrop = 0x100;
        public const int RightsEnum_adRightExclusive = 0x200;
        public const int RightsEnum_adRightExecute = 0x20000000;
        public const int RightsEnum_adRightFull = 0x10000000;
        public const int RightsEnum_adRightInsert = 0x8000;
        public const int RightsEnum_adRightMaximumAllowed = 0x2000000;
        public const int RightsEnum_adRightNone = 0;
        public const int RightsEnum_adRightReadDesign = 0x400;
        public const int RightsEnum_adRightReadPermissions = 0x20000;
        public const int RightsEnum_adRightReference = 0x2000;
        public const int RightsEnum_adRightUpdate = 0x40000000;
        public const int RightsEnum_adRightWithGrant = 0x1000;
        public const int RightsEnum_adRightWriteDesign = 0x800;
        public const int RightsEnum_adRightWriteOwner = 0x80000;
        public const int RightsEnum_adRightWritePermissions = 0x40000;
        public const int SortOrderEnum_adSortAscending = 1;
        public const int SortOrderEnum_adSortDescending = 2;

        private Interop()
        {
        }

        [ComImport, Guid("00000514-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public class AdoConnection
        {
        }

        public enum AdoxDataType
        {
            BigInt = 20,
            Binary = 0x80,
            Boolean = 11,
            BSTR = 8,
            Chapter = 0x88,
            Char = 0x81,
            Currency = 6,
            Date = 7,
            DBDate = 0x85,
            DBTime = 0x86,
            DBTimeStamp = 0x87,
            Decimal = 14,
            Double = 5,
            FileTime = 0x40,
            GUID = 0x48,
            Integer = 3,
            LongVarBinary = 0xcd,
            LongVarChar = 0xc9,
            LongVarWChar = 0xcb,
            Numeric = 0x83,
            PropVariant = 0x8a,
            Single = 4,
            SmallInt = 2,
            TinyInt = 0x10,
            UnsignedBigInt = 0x15,
            UnsignedInt = 0x13,
            UnsignedSmallInt = 0x12,
            UnsignedTinyInt = 0x11,
            UserDefined = 0x84,
            VarBinary = 0xcc,
            VarChar = 200,
            Variant = 12,
            VarNumeric = 0x8b,
            VarWChar = 0xca,
            WChar = 130
        }

        public enum AdoxKeyType
        {
            Foreign = 2,
            Primary = 1,
            Unique = 3
        }

        public enum AdoxRule
        {
            None,
            Cascade,
            SetNull,
            SetDefault
        }

        [ComImport, Guid("00000602-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public class Catalog
        {
        }

        [ComImport, ComVisible(true), Guid("0000061B-0000-0010-8000-00AA006D2EA4")]
        public class Column
        {
        }

        [ComImport, Guid("00000615-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public class Group
        {
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000515-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public interface IAdoConnection
        {
            int Bogus_i01();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
            int Bogus_05();
            int Bogus_06();
            int Bogus_07();
            int Close();
            int Bogus_09();
            int Bogus_10();
            int Bogus_11();
            int Bogus_12();
            int Open(string ConnectionString, string UserID, string Password, int Options);
            int Bogus_14();
            int Bogus_15();
            int Bogus_16();
            int Bogus_17();
            int Bogus_18();
            int Bogus_19();
            int Bogus_20();
            int Bogus_21();
            int Bogus_22();
            int Bogus_23();
            int Bogus_24();
            string GetProvider();
            int SetProvider(string pbstr);
            int Bogus_27();
            int Bogus_28();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("00000603-0000-0010-8000-00AA006D2EA4")]
        public interface ICatalog
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.ITables GetTables();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetActiveConnection();
            [PreserveSig]
            int SetActiveConnection([MarshalAs(UnmanagedType.Struct)] object pVal);
            [PreserveSig]
            int LetActiveConnection([In, MarshalAs(UnmanagedType.Struct)] object pVal);
            Interop.IProcedures GetProcedures();
            Interop.IViews GetViews();
            Interop.IGroups GetGroups();
            Interop.IUsers GetUsers();
            object Create(string ConnectString);
            int Bogus_09();
            int Bogus_10();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("00000512-0000-0010-8000-00AA006D2EA4")]
        public interface ICollection
        {
            int GetCount();
            int Bogus_i01();
            int Bogus_i02();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("0000061C-0000-0010-8000-00AA006D2EA4")]
        public interface IColumn
        {
            string GetName();
            int SetName(string pVal);
            int GetAttributes();
            int SetAttributes(int pVal);
            int GetDefinedSize();
            int SetDefinedSize(int pVal);
            byte GetNumericScale();
            int SetNumericScale(byte pVal);
            int GetPrecision();
            int SetPrecision(int pVal);
            string GetRelatedColumn();
            int SetRelatedColumn(string pVal);
            int GetSortOrder();
            int SetSortOrder(int pVal);
            int GetType();
            int SetType(int pVal);
            Interop.IProperties GetProperties();
            int Bogus_17();
            int SetParentCatalog(Interop.ICatalog ppvObject);
            int Bogus_19();
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("0000061D-0000-0010-8000-00AA006D2EA4")]
        public interface IColumns
        {
            int GetCount();
            int Bogus_i01();
            int Refresh();
            Interop.IColumn GetItem(object Item);
            int Append(object Item, int Type, int DefinedSize);
            int Delete(object Item);
        }

        [ComImport, ComVisible(true), Guid("00000513-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IDynaCollection
        {
            int GetCount();
            int Bogus_i01();
            int Bogus_i02();
            int Bogus_00();
            int Bogus_01();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000628-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public interface IGroup
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
        }

        [ComImport, Guid("00000616-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface IGroup25
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
        }

        [ComImport, ComVisible(true), Guid("00000617-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IGroups
        {
            int GetCount();
            int Bogus_i01();
            int Bogus_i02();
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
        }

        [ComImport, Guid("0000061F-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface IIndex
        {
            string GetName();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
            int Bogus_05();
            bool GetPrimaryKey();
            int SetPrimaryKey(bool pVal);
            int Bogus_08();
            int Bogus_09();
            int Bogus_10();
            int Bogus_11();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("00000620-0000-0010-8000-00AA006D2EA4")]
        public interface IIndexes
        {
            int GetCount();
            int Bogus_i01();
            int Refresh();
            Interop.IIndex GetItem(object Item);
            int Bogus_01();
            int Delete(object Item);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000622-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public interface IKey
        {
            string GetName();
            int SetName(string pVal);
            int GetDeleteRule();
            int SetDeleteRule(int pVal);
            int GetType();
            int SetType(int pVal);
            string GetRelatedTable();
            int SetRelatedTable(string pVal);
            int GetUpdateRule();
            int SetUpdateRule(int pVal);
            Interop.IColumns GetColumns();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000623-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public interface IKeys
        {
            int GetCount();
            int Bogus_i01();
            int Refresh();
            Interop.IKey GetItem(object Item);
            void Append([In, MarshalAs(UnmanagedType.Struct)] object Item, [In, Optional] int Type, [In, Optional, MarshalAs(UnmanagedType.Struct)] object Column, [In, Optional, MarshalAs(UnmanagedType.BStr)] string RelatedTable, [In, Optional, MarshalAs(UnmanagedType.BStr)] string RelatedColumn);
            int Delete(object Item);
        }

        [ComImport, ComVisible(true), Guid("0000061E-0000-0010-8000-00AA006D2EA4")]
        public class Index
        {
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000625-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public interface IProcedure
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
            int Bogus_05();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000626-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public interface IProcedures
        {
            int GetCount();
            int Bogus_i01();
            int Bogus_i02();
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000504-0000-0010-8000-00AA006D2EA4")]
        public interface IProperties
        {
            int GetCount();
            int Bogus_i01();
            int Refresh();
            Interop.IProperty GetItem(object Item);
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000503-0000-0010-8000-00AA006D2EA4")]
        public interface IProperty
        {
            object GetValue();
            int SetValue(object pVal);
            string GetName();
            int GetType();
            int Bogus_04();
            int Bogus_05();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("00000610-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public interface ITable
        {
            Interop.IColumns GetColumns();
            string GetName();
            int SetName(string pVal);
            string GetType();
            Interop.IIndexes GetIndexes();
            Interop.IKeys GetKeys();
            Interop.IProperties GetProperties();
            object GetDateCreated();
            object GetDateModified();
            int Bogus_09();
            int SetParentCatalog(Interop.ICatalog ppvObject);
            int Bogus_11();
        }

        [Guid("00000611-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface ITables
        {
            int GetCount();
            int Bogus_i01();
            int Refresh();
            Interop.ITable GetItem(object Item);
            int Append(object Item);
            int Delete(object Item);
        }

        [ComImport, ComVisible(true), Guid("00000627-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IUser
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
        }

        [ComImport, ComVisible(true), Guid("00000619-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IUser25
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
            int Bogus_05();
        }

        [ComImport, ComVisible(true), Guid("0000061A-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IUsers
        {
            int GetCount();
            int Bogus_i01();
            int Bogus_i02();
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
        }

        [ComImport, Guid("00000613-0000-0010-8000-00AA006D2EA4"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface IView
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
            int Bogus_05();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("00000614-0000-0010-8000-00AA006D2EA4")]
        public interface IViews
        {
            int GetCount();
            int Bogus_i01();
            int Bogus_i02();
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
        }

        [ComImport, ComVisible(true), Guid("00000621-0000-0010-8000-00AA006D2EA4")]
        public class Key
        {
        }

        [ComImport, ComVisible(true), Guid("00000609-0000-0010-8000-00AA006D2EA4")]
        public class Table
        {
        }

        [ComImport, Guid("00000618-0000-0010-8000-00AA006D2EA4"), ComVisible(true)]
        public class User
        {
        }
    }
}


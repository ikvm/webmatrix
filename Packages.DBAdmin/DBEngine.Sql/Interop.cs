namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    internal sealed class Interop
    {
        public const int SQLDMODBStat_All = 0x87e0;
        public const int SQLDMODBStat_EmergencyMode = 0x8000;
        public const int SQLDMODBStat_Inaccessible = 0x3e0;
        public const int SQLDMODBStat_Loading = 0x20;
        public const int SQLDMODBStat_Normal = 0;
        public const int SQLDMODBStat_Offline = 0x200;
        public const int SQLDMODBStat_Recovering = 0xc0;
        public const int SQLDMODBStat_Standby = 0x400;
        public const int SQLDMODBStat_Suspect = 0x100;
        public const int SQLDMOGrowth_Invalid = 0x63;
        public const int SQLDMOGrowth_MB = 0;
        public const int SQLDMOGrowth_Percent = 1;
        public const int SQLDMOKey_Foreign = 3;
        public const int SQLDMOKey_Primary = 1;
        public const int SQLDMOKey_Unique = 2;
        public const int SQLDMOKey_Unknown = 0;
        public const int SQLDMOPriv_AllDatabasePrivs = 0x1ff80;
        public const int SQLDMOPriv_AllObjectPrivs = 0x3f;
        public const int SQLDMOPriv_CreateDatabase = 0x100;
        public const int SQLDMOPriv_CreateDefault = 0x1000;
        public const int SQLDMOPriv_CreateFunction = 0xff56;
        public const int SQLDMOPriv_CreateProcedure = 0x400;
        public const int SQLDMOPriv_CreateRule = 0x4000;
        public const int SQLDMOPriv_CreateTable = 0x80;
        public const int SQLDMOPriv_CreateView = 0x200;
        public const int SQLDMOPriv_Delete = 8;
        public const int SQLDMOPriv_DumpDatabase = 0x800;
        public const int SQLDMOPriv_DumpTable = 0x8000;
        public const int SQLDMOPriv_DumpTransaction = 0x2000;
        public const int SQLDMOPriv_Execute = 0x10;
        public const int SQLDMOPriv_Insert = 2;
        public const int SQLDMOPriv_References = 0x20;
        public const int SQLDMOPriv_Select = 1;
        public const int SQLDMOPriv_Unknown = 0;
        public const int SQLDMOPriv_Update = 4;
        public const int SQLDMOScript_Aliases = 0x4000;
        public const int SQLDMOScript_AppendToFile = 0x100;
        public const int SQLDMOScript_Bindings = 0x80;
        public const int SQLDMOScript_ClusteredIndexes = 8;
        public const int SQLDMOScript_DatabasePermissions = 0x20;
        public const int SQLDMOScript_Default = 4;
        public const int SQLDMOScript_DRI_All = 0x1fc00000;
        public const int SQLDMOScript_DRI_AllConstraints = 0x1f000000;
        public const int SQLDMOScript_DRI_AllKeys = 0x1c000000;
        public const int SQLDMOScript_DRI_Checks = 0x1000000;
        public const int SQLDMOScript_DRI_Clustered = 0x800000;
        public const int SQLDMOScript_DRI_Defaults = 0x2000000;
        public const int SQLDMOScript_DRI_ForeignKeys = 0x8000000;
        public const int SQLDMOScript_DRI_NonClustered = 0x400000;
        public const int SQLDMOScript_DRI_PrimaryKey = 0x10000000;
        public const int SQLDMOScript_DRI_UniqueKeys = 0x4000000;
        public const int SQLDMOScript_DRIIndexes = 0x10000;
        public const int SQLDMOScript_DRIWithNoCheck = 0x20000000;
        public const int SQLDMOScript_Drops = 1;
        public const int SQLDMOScript_IncludeHeaders = 0x20000;
        public const int SQLDMOScript_IncludeIfNotExists = 0x1000;
        public const int SQLDMOScript_Indexes = 0x12008;
        public const int SQLDMOScript_NoCommandTerm = 0x8000;
        public const int SQLDMOScript_NoDRI = 0x200;
        public const int SQLDMOScript_NoIdentity = 0x40000000;
        public const int SQLDMOScript_NonClusteredIndexes = 0x2000;
        public const int SQLDMOScript_None = 0;
        public const int SQLDMOScript_ObjectPermissions = 2;
        public const int SQLDMOScript_OwnerQualify = 0x40000;
        public const int SQLDMOScript_Permissions = 0x22;
        public const int SQLDMOScript_PrimaryObject = 4;
        public const int SQLDMOScript_SortedData = 0x100000;
        public const int SQLDMOScript_SortedDataReorg = 0x200000;
        public const int SQLDMOScript_TimestampToBinary = 0x80000;
        public const int SQLDMOScript_ToFileOnly = 0x40;
        public const int SQLDMOScript_TransferDefault = 0x670ff;
        public const int SQLDMOScript_Triggers = 0x10;
        public const int SQLDMOScript_UDDTsToBaseType = 0x400;

        private Interop()
        {
        }

        [ComImport, Guid("10020500-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public class Column
        {
        }

        [ComImport, Guid("10020300-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public class Database
        {
        }

        [ComImport, ComVisible(true), Guid("10022D00-E260-11CF-AE68-00AA004A34D5")]
        public class DBFile
        {
        }

        public enum DmoKeyType
        {
            Unknown,
            Primary,
            Unique,
            Foreign
        }

        [ComImport, ComVisible(true), Guid("10022C00-E260-11CF-AE68-00AA004A34D5")]
        public class FileGroup
        {
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("10020506-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public interface IColumn
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.IProperties GetProperties();
            string GetName();
            int SetName(string pRetVal);
            int GetIdentityIncrement();
            int SetIdentityIncrement(int pRetVal);
            int GetIdentitySeed();
            int SetIdentitySeed(int pRetVal);
            Interop.IDRIDefault GetDRIDefault();
            bool GetInPrimaryKey();
            string GetDatatype();
            int SetDatatype(string pRetVal);
            string GetPhysicalDatatype();
            int GetLength();
            int SetLength(int pRetVal);
            string GetDefault();
            int SetDefault(string pRetVal);
            int Bogus_9();
            int Bogus_10();
            bool GetAllowNulls();
            int SetAllowNulls(bool pRetVal);
            int Bogus_11();
            bool GetIdentity();
            int SetIdentity(bool pRetVal);
            int GetNumericPrecision();
            int SetNumericPrecision(int pRetVal);
            int GetNumericScale();
            int SetNumericScale(int pRetVal);
            int Remove();
            int Bogus_12();
            bool GetIsRowGuidCol();
            int SetIsRowGuidCol(bool pRetVal);
            int Bogus_13();
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
            int Bogus_25();
            int Bogus_26();
        }

        [ComImport, ComVisible(true), Guid("10020503-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IColumns
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.IColumn Item(object Index);
            int Bogus_6();
            int GetCount();
            int Bogus_7();
            int Add(Interop.IColumn Object);
            int Bogus_8();
            int Refresh(object ReleaseMemberObjects);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("10020306-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public interface IDatabase
        {
            int Bogus_Application();
            int Bogus_Parent();
            int Bogus_UserData1();
            int Bogus_UserData2();
            int Bogus_TypeOf();
            int Bogus_Properties();
            string GetName();
            int SetName(string pRetVal);
            Interop.ITables GetTables();
            int Bogus_SystemObject();
            int Bogus_ID();
            int Bogus_UserProfile();
            int Bogus_CreateForAttach1();
            int Bogus_CreateForAttach2();
            string GetOwner();
            int Bogus_Version();
            string GetCreateDate();
            int Bogus_DataSpaceUsage();
            int Bogus_GetUserName();
            int Bogus_UserName();
            int GetStatus();
            int GetSize();
            int GetSpaceAvailable();
            int Bogus_IndexSpaceUsage();
            int Bogus_SpaceAvailableInMB();
            int Bogus_Views();
            Interop.IStoredProcedures GetStoredProcedures();
            int Bogus_Defaults();
            int Bogus_Rules();
            int Bogus_UserDefinedDatatypes();
            Interop.IUsers GetUsers();
            int Bogus_Groups();
            int Bogus_SystemDatatypes();
            Interop.ITransactionLog GetTransactionLog();
            int Bogus_DBOption();
            int Bogus_DboLogin();
            int Bogus_Grant();
            int Bogus_Revoke();
            int Bogus_ExecuteImmediate();
            int Bogus_GetObjectByName();
            int Bogus_Checkpoint();
            int Bogus_CheckTables();
            int Bogus_CheckAllocations();
            int Bogus_CheckCatalog();
            int Bogus_GetMemoryUsage();
            int Bogus_ExecuteWithResults();
            int Bogus_ListObjectPermissions();
            int Bogus_EnumLocks();
            int Bogus_ListObjects();
            int Bogus_EnumDependencies();
            int Bogus_SetOwner();
            int Bogus_ListDatabasePermissions();
            int Remove();
            int Bogus_RecalcSpaceUsage();
            int Bogus_EnumCandidateKeys();
            int Bogus_IsValidKeyDatatype();
            int Bogus_GetDatatypeByName();
            int Bogus_Transfer();
            int Bogus_ScriptTransfer();
            int Bogus_CheckIdentityValues();
            int Bogus_ExecuteWithResultsAndMessages();
            string Script(int ScriptType, object ScriptFilePath, int Script2Type);
            int Bogus_CheckTablesDataOnly();
            int Bogus_CheckAllocationsDataOnly();
            int Bogus_UpdateIndexStatistics();
            int Bogus_EnumLoginMappings();
            int Bogus_PrimaryFilePath();
            Interop.IFileGroups GetFileGroups();
            int Bogus_DatabaseRoles();
            int GetPermissions();
            int Bogus_Isdb_accessadmin();
            int Bogus_Isdb_datareader();
            int Bogus_Isdb_ddladmin();
            int Bogus_Isdb_denydatareader();
            int Bogus_Isdb_denydatawriter();
            int Bogus_Isdb_backupoperator();
            int Bogus_Isdb_owner();
            int Bogus_Isdb_securityadmin();
            int Bogus_Isdb_datawriter();
            int Bogus_EnumFiles();
            int Bogus_EnumFileGroups();
            int Bogus_EnumUsers();
            int Bogus_EnumNTGroups();
            int Bogus_Deny();
            bool IsUser(string UserName);
            int Bogus_GenerateSQL();
            int Bogus_Shrink();
            int Bogus_CheckTextAllocsFast();
            int Bogus_CheckTextAllocsFull();
            int Bogus_EnumMatchingSPs();
            int Bogus_EnableFullTextCatalogs();
            int Bogus_RemoveFullTextCatalogs();
            int Bogus_FullTextIndexScript();
            int Bogus_IsFullTextEnabled();
            int Bogus_FullTextCatalogs();
            int Bogus_DisableFullTextCatalogs();
            int Bogus_CompatibilityLevel1();
            int Bogus_CompatibilityLevel2();
            int Bogus_UseServerName1();
            int Bogus_UseServerName2();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("10020303-E260-11CF-AE68-00AA004A34D5")]
        public interface IDatabases
        {
            int Bogus_Application();
            int Bogus_Parent();
            int Bogus_UserData1();
            int Bogus_UserData2();
            int Bogus_TypeOf();
            Interop.IDatabase Item(object Index, object Owner);
            int Bogus_NewEnum();
            int GetCount();
            int Bogus_ItemByID();
            int Add(Interop.IDatabase Object);
            int Bogus_Remove();
            int Refresh(object ReleaseMemberObjects);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("10022D06-E260-11CF-AE68-00AA004A34D5")]
        public interface IDBFile
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            int Bogus_11();
            int Bogus_12();
            int GetFileGrowth();
            int SetFileGrowth(int pRetVal);
            int GetMaximumSize();
            int SetMaximumSize(int pRetVal);
            int Bogus_13();
            int Bogus_14();
            int Bogus_15();
            int Bogus_16();
            int GetFileGrowthType();
            int SetFileGrowthType(int pRetVal);
            int Bogus_17();
            int Bogus_18();
            int Bogus_19();
            int Bogus_20();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("10022D03-E260-11CF-AE68-00AA004A34D5")]
        public interface IDBFiles
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.IDBFile Item(object ItemIndex);
            int Bogus_6();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            int Refresh(object ReleaseMemberObjects);
        }

        [ComImport, ComVisible(true), Guid("10022B06-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IDRIDefault
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            string GetName();
            int SetName(string pRetVal);
            string GetText();
            int SetText(string pRetVal);
            int Remove();
            int Bogus_7();
        }

        [ComImport, ComVisible(true), Guid("10022C06-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IFileGroup
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            int Bogus_7();
            int Bogus_8();
            Interop.IDBFiles GetDBFiles();
            int Bogus_9();
            int Bogus_10();
            int Bogus_11();
            int Bogus_12();
            int Bogus_13();
            int Bogus_14();
            int Bogus_15();
            int Bogus_16();
            int Bogus_17();
            int Bogus_18();
            int Bogus_19();
        }

        [ComImport, ComVisible(true), Guid("10022C03-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IFileGroups
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.IFileGroup Item(object ItemInd);
            int Bogus_6();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            int Bogus_11();
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("10020F06-E260-11CF-AE68-00AA004A34D5")]
        public interface IKey
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.IProperties GetProperties();
            string GetName();
            int SetName(string pRetVal);
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            Interop.INames GetKeyColumns();
            string GetReferencedTable();
            int SetReferencedTable(string pRetVal);
            Interop.INames GetReferencedColumns();
            int GetType();
            int SetType(int pRetVal);
            string GetReferencedKey();
            int Remove();
            string Script(int ScriptType, object ScriptFilePath, int Script2Type);
            int Bogus_15();
            int Bogus_16();
            int Bogus_17();
            int Bogus_18();
            int Bogus_19();
            int Bogus_20();
            int Bogus_21();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("10020F03-E260-11CF-AE68-00AA004A34D5")]
        public interface IKeys
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.IKey Item(object ItemIndex);
            int Bogus_6();
            int GetCount();
            int Add(Interop.IKey Object);
            int Remove(object Index);
            int Refresh(object ReleaseMemberObjects);
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("10022E06-E260-11CF-AE68-00AA004A34D5")]
        public interface ILogFile
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            int Bogus_11();
            int Bogus_12();
            int Bogus_13();
            int GetFileGrowth();
            int SetFileGrowth(int pRetVal);
            int GetFileGrowthType();
            int SetFileGrowthType(int pRetVal);
            int Bogus_14();
            int GetMaximumSize();
            int SetMaximumSize(int pRetVal);
            int Bogus_15();
            int Bogus_16();
        }

        [ComImport, Guid("10022E03-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface ILogFiles
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.ILogFile Item(object ItemIndex);
            int Bogus_6();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Refresh(object ReleaseMemberObjects);
        }

        [ComImport, Guid("10021D03-E260-11CF-AE68-00AA004A34D5"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface INames
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            string Item(object Index);
            int Bogus_6();
            int GetCount();
            int Add(string NewName);
            int Remove(object Index);
            int Refresh();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("10020001-E260-11CF-AE68-00AA004A34D5")]
        public interface IProperties
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
            Interop.IProperty Item(object index);
            int Bogus_06();
            int GetCount();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("10020002-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public interface IProperty
        {
            int Bogus_00();
            int Bogus_01();
            int Bogus_02();
            int Bogus_03();
            int Bogus_04();
            object GetValue();
            int SetValue(object pRetVal);
            int Bogus_07();
            string GetName();
            int GetType();
            bool GetGet();
            bool GetSet();
        }

        [ComImport, Guid("10020206-E260-11CF-AE68-00AA004A34D5"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface ISqlServer
        {
            int Bogus_Application();
            int Bogus_Parent();
            int Bogus_UserData1();
            int Bogus_UserData2();
            int Bogus_TypeOf();
            int Bogus_Properties();
            Interop.IDatabases GetDatabases();
            int Bogus_Password1();
            int Bogus_Password2();
            int Bogus_Name1();
            int Bogus_Name2();
            int Bogus_Login1();
            int Bogus_Login2();
            int Bogus_VersionString();
            int Bogus_BackupDevices();
            int Bogus_VersionMajor();
            int Bogus_VersionMinor();
            int Bogus_CommandTerminator1();
            int Bogus_CommandTerminator2();
            int Bogus_TrueName();
            int Bogus_ConnectionID();
            int Bogus_TrueLogin();
            int Bogus_IntegratedSecurity();
            int Bogus_Languages();
            int Bogus_RemoteServers();
            int Bogus_Logins();
            int Bogus_UserProfile();
            int Bogus_MaxNumericPrecision();
            int Bogus_NextDeviceNumber();
            int Bogus_QueryTimeout1();
            int Bogus_QueryTimeout2();
            int Bogus_LoginTimeout1();
            int Bogus_LoginTimeout2();
            int Bogus_NetPacketSize1();
            int Bogus_NetPacketSize2();
            int Bogus_HostName1();
            int Bogus_HostName2();
            int Bogus_ApplicationName1();
            int Bogus_ApplicationName2();
            bool GetLoginSecure();
            int SetLoginSecure(bool pRetVal);
            int Bogus_ProcessID();
            int Bogus_Status();
            int Bogus_Registry();
            int Bogus_Configuration();
            int Bogus_JobServer();
            int Bogus_ProcessOutputBuffer1();
            int Bogus_ProcessOutputBuffer2();
            int Bogus_Language1();
            int Bogus_Language2();
            int Bogus_AutoReConnect1();
            int Bogus_AutoReConnect2();
            int Bogus_StatusInfoRefetchInterval1();
            int Bogus_StatusInfoRefetchInterval2();
            int Bogus_SaLogin();
            int Bogus_AnsiNulls1();
            int Bogus_AnsiNull2();
            int Connect([In, MarshalAs(UnmanagedType.Struct)] object ServerName, [In, MarshalAs(UnmanagedType.Struct)] object Login, [In, MarshalAs(UnmanagedType.Struct)] object Password);
            int Bogus_Close();
            [PreserveSig]
            int DisConnect();
            int Bogus_KillProcess();
            int Bogus_ExecuteImmediate();
            int Bogus_ReConnect();
            int Bogus_Shutdown();
            int Bogus_Start();
            int Bogus_UnloadODSDLL();
            int Bogus_KillDatabase();
            int Bogus_ExecuteWithResults();
            int Bogus_ListStartupProcedures();
            int Bogus_BeginTransaction();
            int Bogus_CommitTransaction();
            int Bogus_SaveTransaction();
            int Bogus_RollbackTransaction();
            int Bogus_CommandShellImmediate();
            int Bogus_ReadErrorLog();
            int Bogus_EnumErrorLogs();
            int Bogus_EnumAvailableMedia();
            int Bogus_EnumDirectories();
            int Bogus_EnumServerAttributes();
            int Bogus_EnumVersionInfo();
            int Bogus_EnumLocks();
            int Bogus_CommandShellWithResults();
            int Bogus_ReadBackupHeader();
            int Bogus_EnumProcesses();
            int Bogus_Pause();
            int Bogus_Continue();
            int Bogus_VerifyConnection();
            int Bogus_IsOS();
            int Bogus_AddStartParameter();
            int Bogus_NetName();
            int Bogus_ExecuteWithResultsAndMessages();
            int Bogus_EnumLoginMappings();
            int Bogus_Replication();
            int Bogus_EnableBcp1();
            int Bogus_EnableBcp2();
            int Bogus_BlockingTimeout1();
            int Bogus_BlockingTimeout2();
            int Bogus_ServerRoles();
            int Bogus_Isdbcreator();
            int Bogus_Isdiskadmin();
            int Bogus_Isprocessadmin();
            int Bogus_Issecurityadmin();
            int Bogus_Isserveradmin();
            int Bogus_Issetupadmin();
            int Bogus_Issysadmin();
            int Bogus_EnumNTDomainGroups();
            int Bogus_EnumAccountInfo();
            int Bogus_ListMembers();
            int Bogus_IsLogin();
            int Bogus_Abort();
            int Bogus_DetachDB();
            int Bogus_AttachDB();
            int Bogus_QuotedIdentifier1();
            int Bogus_QuotedIdentifier2();
            int Bogus_LinkedServers();
            int Bogus_CodePageOverride();
            int Bogus_FullTextService();
            int Bogus_ODBCPrefix1();
            int Bogus_ODBCPrefix2();
            int Bogus_Stop();
            int Bogus_PingSQLServerVersion();
            int Bogus_IsPackage();
            int Bogus_RegionalSetting1();
            int Bogus_RegionalSetting2();
            int Bogus_CodePage();
            int Bogus_AttachDBWithSingleFile();
            int Bogus_IsNTGroupMember();
            int Bogus_ServerTime();
            int Bogus_TranslateChar1();
            int Bogus_TranslateChar2();
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("10020D06-E260-11CF-AE68-00AA004A34D5")]
        public interface IStoredProcedure
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            string GetName();
            int SetName(string pRetVal);
            bool GetSystemObject();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            string GetOwner();
            int SetOwner(string pRetVal);
            string GetCreateDate();
            int Bogus_11();
            string GetText();
            int SetText(string pRetVal);
            int Bogus_12();
            int Bogus_13();
            int Bogus_14();
            int Bogus_15();
            int Bogus_16();
            int Remove();
            string Script(int ScriptType, object ScriptFilePath, int Script2Type);
            int Bogus_17();
            int Bogus_18();
            int Alter(string NewText);
            int Bogus_19();
            int Bogus_20();
        }

        [ComImport, ComVisible(true), Guid("10020D03-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IStoredProcedures
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.IStoredProcedure Item(object Index, object Owner);
            int Bogus_6();
            int GetCount();
            int Bogus_7();
            int Add(Interop.IStoredProcedure Object);
            int Bogus_8();
            int Refresh(object ReleaseMemberObjects);
        }

        [ComImport, ComVisible(true), Guid("10020406-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface ITable
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            string GetName();
            int SetName(string pRetVal);
            Interop.IColumns GetColumns();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            Interop.IKeys GetKeys();
            string GetOwner();
            int SetOwner(string pRetVal);
            int Bogus_10();
            string GetCreateDate();
            int Bogus_11();
            int GetIndexes();
            int Bogus_13();
            int Bogus_14();
            int Bogus_15();
            bool GetSystemObject();
            int GetRows();
            int BeginAlter();
            int DoAlter();
            int CancelAlter();
            int Bogus_19();
            int Bogus_20();
            int Bogus_21();
            int Bogus_22();
            int Bogus_23();
            int Bogus_24();
            int Bogus_25();
            int Bogus_26();
            int Remove();
            int Bogus_27();
            int Bogus_28();
            int Bogus_29();
            int Bogus_30();
            int Bogus_31();
            int Bogus_32();
            int Bogus_33();
            int Bogus_34();
            string Script(int ScriptType, object ScriptFilePath, object NewName, int Script2Type);
            int Bogus_35();
            int Refresh();
            int Bogus_37();
            int Bogus_38();
            int Bogus_39();
            int Bogus_40();
            int Bogus_41();
            int Bogus_42();
            int Bogus_43();
            int Bogus_44();
            int Bogus_45();
            int Bogus_46();
            int Bogus_47();
            int Bogus_48();
            int Bogus_49();
            int Bogus_50();
            int Bogus_51();
            int Bogus_52();
            int Bogus_53();
            int Bogus_54();
            int Bogus_55();
            int Bogus_56();
            int Bogus_57();
            int Bogus_58();
            int Bogus_59();
            int Bogus_60();
            int Bogus_61();
            int Bogus_62();
            int Bogus_63();
            int Bogus_64();
            int Bogus_65();
        }

        [ComImport, ComVisible(true), Guid("10020403-E260-11CF-AE68-00AA004A34D5"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface ITables
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            Interop.ITable Item(object Index, object Owner);
            int Bogus_6();
            int GetCount();
            int Bogus_7();
            int Add(Interop.ITable Object);
            int Bogus_8();
            int Refresh(object ReleaseMemberObjects);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("10022606-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public interface ITransactionLog
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            int Bogus_7();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            int Bogus_11();
            int Bogus_12();
            int Bogus_13();
            Interop.ILogFiles GetLogFiles();
        }

        [ComImport, Guid("10020B03-E260-11CF-AE68-00AA004A34D5"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IUsers
        {
            int Bogus_1();
            int Bogus_2();
            int Bogus_3();
            int Bogus_4();
            int Bogus_5();
            int Bogus_6();
            int Bogus_7();
            int GetCount();
            int Bogus_8();
            int Bogus_9();
            int Bogus_10();
            int Bogus_11();
        }

        [ComImport, Guid("10020F00-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public class Key
        {
        }

        [ComImport, Guid("10022E00-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public class LogFile
        {
        }

        [ComImport, Guid("10020200-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public class SqlServer
        {
        }

        [ComImport, Guid("10020D00-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public class StoredProcedure
        {
        }

        [ComImport, Guid("10020400-E260-11CF-AE68-00AA004A34D5"), ComVisible(true)]
        public class Table
        {
        }
    }
}


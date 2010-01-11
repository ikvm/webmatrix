namespace Microsoft.Matrix.Packages.Community
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    internal sealed class Interop
    {
        public const int MISTATUS_AWAY = 0x22;
        public const int MISTATUS_BUSY = 10;
        public const int MISTATUS_IDLE = 0x12;
        public const int MISTATUS_INVISIBLE = 6;
        public const int MISTATUS_OFFLINE = 1;
        public const int MISTATUS_ONLINE = 2;
        public const int MISTATUS_UNKNOWN = 0;
        public const int MSGR_E_ALREADY_LOGGED_ON = -2130705660;
        public const int MSGR_E_NOT_LOGGED_ON = -2130705634;
        public const int S_FALSE = 1;
        public const int S_OK = 0;

        private Interop()
        {
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("D50C3286-0F89-48F8-B204-3604629DEE10"), ComVisible(true)]
        public interface IMessenger2
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetWindow();
            [PreserveSig]
            int ViewProfile([In, MarshalAs(UnmanagedType.Struct)] object vContact);
            string GeetReceiveFileDirectory();
            [return: MarshalAs(UnmanagedType.Interface)]
            object StartVoice([In, MarshalAs(UnmanagedType.Struct)] object vContact);
            [return: MarshalAs(UnmanagedType.Interface)]
            object InviteApp([In, MarshalAs(UnmanagedType.Struct)] object vContact, string bstrAppId);
            [PreserveSig]
            int SendMail([In, MarshalAs(UnmanagedType.Struct)] object vContact);
            [PreserveSig]
            int OpenInbox();
            [return: MarshalAs(UnmanagedType.Interface)]
            object SendFile([MarshalAs(UnmanagedType.Struct)] object vContact, string bstrFileName);
            [PreserveSig]
            int Signout();
            [PreserveSig]
            int SignIn(IntPtr hwndParent, string bstrSigninName, string bstrPassword);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetContact(string bstrSigninName, string bstrServiceId);
            [PreserveSig]
            int OptionsPages(IntPtr hwndParent, int optionPage);
            [PreserveSig]
            int AddContact(IntPtr hwndParent, string bstrEmail);
            [PreserveSig]
            int FindContact(IntPtr hwndParent, string bstrFirstName, string bstrLastName, [MarshalAs(UnmanagedType.Struct)] object vbstrCity, [MarshalAs(UnmanagedType.Struct)] object vbstrState, [MarshalAs(UnmanagedType.Struct)] object vbstrCountry);
            [return: MarshalAs(UnmanagedType.Interface)]
            object InstantMessage([In, MarshalAs(UnmanagedType.Struct)] object vContact);
            [return: MarshalAs(UnmanagedType.Interface)]
            object Phone([In, MarshalAs(UnmanagedType.Struct)] object vContact, int phoneNumberType, string bstrNumber);
            [PreserveSig]
            int MediaWizard(IntPtr hwndParent);
            [return: MarshalAs(UnmanagedType.Interface)]
            object Page([In, MarshalAs(UnmanagedType.Struct)] object vContact);
            [PreserveSig]
            int AutoSignin();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetMyContacts();
            string GetMySigninName();
            string GetMyFriendlyName();
            [PreserveSig]
            int SetMyStatus(int status);
            int GetMyStatus();
            int GetUnreadEmailCount(int folder);
            string GetMyServiceName();
            string GetMyPhoneNumber(int phoneNumberType);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMyProperty(int propType);
            [PreserveSig]
            int SetMyProperty(int propType, [MarshalAs(UnmanagedType.Struct)] object propVal);
            string GetMyServiceId();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetServices();
            int GetContactsSortOrder();
            [PreserveSig]
            int SetContactsSortOrder(int sort);
            [return: MarshalAs(UnmanagedType.Interface)]
            object StartVideo([In, MarshalAs(UnmanagedType.Struct)] object vContact);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetMyGroups();
            [return: MarshalAs(UnmanagedType.Interface)]
            object CreateGroup(string bstrName, [MarshalAs(UnmanagedType.Struct)] object vService);
        }

        [ComVisible(true), Guid("E7479A0F-BB19-44A5-968F-6F41D93EE0BC"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IMessengerContact
        {
            string GetFriendlyName();
            int GetStatus();
            string GetSigninName();
            string GetServiceName();
            bool GetBlocked();
            [PreserveSig]
            int SetBlocked(bool blocked);
            bool GetCanPage();
            string GetPhoneNumber(int phoneNumberType);
            bool GetIsSelf();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetProperty(int propType);
            [PreserveSig]
            int SetProperty(int propType, [MarshalAs(UnmanagedType.Struct)] object propValue);
            string GetServiceId();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("E7479A0D-BB19-44A5-968F-6F41D93EE0BC")]
        public interface IMessengerContacts
        {
            int GetCount();
            [return: MarshalAs(UnmanagedType.Interface)]
            object Item(int index);
            [PreserveSig]
            int Remove([MarshalAs(UnmanagedType.Interface)] object contact);
            [return: MarshalAs(UnmanagedType.Interface)]
            object Get_NewEnum();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("E1AF1038-B884-44CB-A535-1C3C11A3D1DB"), ComVisible(true)]
        public interface IMessengerGroup
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetContacts();
            string GetName();
            [PreserveSig]
            int SetName(string name);
            [PreserveSig]
            int AddContact([MarshalAs(UnmanagedType.Struct)] object vContact);
            [PreserveSig]
            int RemoveContact([MarshalAs(UnmanagedType.Struct)] object vContact);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetService();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("E1AF1028-B884-44CB-A535-1C3C11A3D1DB"), ComVisible(true)]
        public interface IMessengerGroups
        {
            [PreserveSig]
            int RemoveGroup([MarshalAs(UnmanagedType.Interface)] object group);
            int GetCount();
            [return: MarshalAs(UnmanagedType.Interface)]
            object Item(int index);
            [return: MarshalAs(UnmanagedType.Interface)]
            object Get_NewEnum();
        }

        [ComImport, ComVisible(true), Guid("B69003B3-C55E-4B48-836C-BC5946FC3B28")]
        public class Messenger
        {
        }
    }
}


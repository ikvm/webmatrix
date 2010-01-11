namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class ProtocolHandler : Interop.IClassFactory, Interop.IInternetProtocolInfo
    {
        private static Encoding _contentEncoding;
        private static bool _registered;
        internal const string ProtocolPrefix = "maxtrix";

        int Interop.IClassFactory.CreateInstance(object pUnkOuter, ref Guid riid, out object obj)
        {
            obj = new ProtocolRequest();
            IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(obj);
            IntPtr zero = IntPtr.Zero;
            Marshal.QueryInterface(iUnknownForObject, ref riid, out zero);
            Marshal.Release(iUnknownForObject);
            if (zero == IntPtr.Zero)
            {
                Marshal.Release(zero);
                obj = null;
                return -2147467262;
            }
            return 0;
        }

        int Interop.IClassFactory.LockServer(bool fLock)
        {
            return 0;
        }

        int Interop.IInternetProtocolInfo.CombineUrl(string pwzBaseUrl, string pwzRelativeUrl, int dwCombineFlags, IntPtr pwzResult, int cchResult, out int pcchResult, int dwReserved)
        {
            int index = pwzBaseUrl.IndexOf(':');
            return Interop.CoInternetCombineUrl(pwzBaseUrl.Substring(index + 1, (pwzBaseUrl.Length - index) - 1), pwzRelativeUrl, dwCombineFlags, pwzResult, cchResult, out pcchResult, dwReserved);
        }

        int Interop.IInternetProtocolInfo.CompareUrl(string pwzUrl1, string pwzUrl2, int dwCompareFlags)
        {
            return -2146697199;
        }

        int Interop.IInternetProtocolInfo.ParseUrl(string pwzUrl, int parseAction, int dwParseFlags, IntPtr pwzResult, int cchResult, IntPtr pcchResult, int dwReserved)
        {
            return -2146697199;
        }

        int Interop.IInternetProtocolInfo.QueryInfo(string pwzUrl, int queryOption, int dwQueryFlags, IntPtr pBuffer, int cbBuffer, IntPtr pcbBuf, int dwReserved)
        {
            return -2146697199;
        }

        public static void Register()
        {
            lock (typeof(ProtocolHandler))
            {
                if (!_registered)
                {
                    try
                    {
                        Interop.IInternetSession session;
                        Interop.CoInternetGetSession(0, out session, 0);
                        Guid gUID = typeof(ProtocolRequest).GUID;
                        ProtocolHandler classFactory = new ProtocolHandler();
                        session.RegisterNameSpace(classFactory, ref gUID, "maxtrix", 0, null, 0);
                    }
                    finally
                    {
                        _registered = true;
                    }
                }
            }
        }

        public static void SetContentEncoding(string encodingName)
        {
            if ((_contentEncoding == null) || (_contentEncoding.WebName.ToLower() != encodingName.ToLower()))
            {
                try
                {
                    _contentEncoding = Encoding.GetEncoding(encodingName);
                }
                catch
                {
                    _contentEncoding = Encoding.Default;
                }
            }
        }

        public static Encoding ContentEncoding
        {
            get
            {
                if (_contentEncoding != null)
                {
                    return _contentEncoding;
                }
                return Encoding.Default;
            }
        }
    }
}


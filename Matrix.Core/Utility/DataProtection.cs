namespace Microsoft.Matrix.Utility
{
    using Microsoft.Matrix;
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class DataProtection
    {
        private DataProtection()
        {
        }

        public static string DecryptData(string data)
        {
            if ((data == null) || (data.Length == 0))
            {
                throw new ArgumentNullException("data");
            }
            byte[] bytes = UnprotectData(Convert.FromBase64String(data), 4);
            string str = null;
            if (bytes != null)
            {
                str = Encoding.Unicode.GetString(bytes);
            }
            return str;
        }

        public static string DecryptUserData(string data)
        {
            if ((data == null) || (data.Length == 0))
            {
                throw new ArgumentNullException("data");
            }
            byte[] bytes = UnprotectData(Convert.FromBase64String(data), 0);
            string str = null;
            if (bytes != null)
            {
                str = Encoding.Unicode.GetString(bytes);
            }
            return str;
        }

        public static string EncryptData(string data)
        {
            if ((data == null) || (data.Length == 0))
            {
                throw new ArgumentNullException("data");
            }
            byte[] inArray = ProtectData(Encoding.Unicode.GetBytes(data), 4);
            string str = null;
            if (inArray != null)
            {
                str = Convert.ToBase64String(inArray);
            }
            return str;
        }

        public static string EncryptUserData(string data)
        {
            if ((data == null) || (data.Length == 0))
            {
                throw new ArgumentNullException("data");
            }
            byte[] inArray = ProtectData(Encoding.Unicode.GetBytes(data), 0);
            string str = null;
            if (inArray != null)
            {
                str = Convert.ToBase64String(inArray);
            }
            return str;
        }

        private static byte[] ProtectData(byte[] data, int flags)
        {
            byte[] destination = null;
            flags |= 1;
            Interop.DATA_BLOB dataIn = new Interop.DATA_BLOB();
            Interop.DATA_BLOB pDataOut = new Interop.DATA_BLOB();
            try
            {
                dataIn.cbData = data.Length;
                dataIn.pbData = Marshal.AllocHGlobal(dataIn.cbData);
                Marshal.Copy(data, 0, dataIn.pbData, dataIn.cbData);
                if (!Interop.CryptProtectData(ref dataIn, "data", IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, flags, ref pDataOut))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                destination = new byte[pDataOut.cbData];
                Marshal.Copy(pDataOut.pbData, destination, 0, pDataOut.cbData);
                Interop.LocalFree(pDataOut.pbData);
                return destination;
            }
            finally
            {
                if (dataIn.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(dataIn.pbData);
                }
            }
            return destination;
        }

        private static byte[] UnprotectData(byte[] data, int flags)
        {
            byte[] destination = null;
            flags |= 1;
            Interop.DATA_BLOB dataIn = new Interop.DATA_BLOB();
            Interop.DATA_BLOB pDataOut = new Interop.DATA_BLOB();
            try
            {
                dataIn.cbData = data.Length;
                dataIn.pbData = Marshal.AllocHGlobal(dataIn.cbData);
                Marshal.Copy(data, 0, dataIn.pbData, dataIn.cbData);
                if (!Interop.CryptUnprotectData(ref dataIn, null, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, flags, ref pDataOut))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                destination = new byte[pDataOut.cbData];
                Marshal.Copy(pDataOut.pbData, destination, 0, pDataOut.cbData);
                Interop.LocalFree(pDataOut.pbData);
                return destination;
            }
            finally
            {
                if (dataIn.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(dataIn.pbData);
                }
            }
            return destination;
        }
    }
}


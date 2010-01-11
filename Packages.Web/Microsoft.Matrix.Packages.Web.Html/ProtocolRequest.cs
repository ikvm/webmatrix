namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Runtime.InteropServices;

    internal sealed class ProtocolRequest : Interop.IInternetProtocol
    {
        private byte[] _data;
        private int _index;
        private int _length;
        private string _realUrl;

        int Interop.IInternetProtocol.Abort(int hrReason, int dwOptions)
        {
            return -2147467263;
        }

        int Interop.IInternetProtocol.Continue(IntPtr protocolData)
        {
            return -2147467263;
        }

        int Interop.IInternetProtocol.LockRequest(int dwOptions)
        {
            return 0;
        }

        int Interop.IInternetProtocol.Read(IntPtr buffer, int bufferSize, out int numRead)
        {
            int num = Math.Min(bufferSize, this._length - this._index);
            for (int i = 0; i < num; i++)
            {
                Marshal.WriteByte(buffer, i, this._data[this._index++]);
            }
            numRead = num;
            if (this._index >= this._length)
            {
                return 1;
            }
            return 0;
        }

        int Interop.IInternetProtocol.Resume()
        {
            return -2147467263;
        }

        int Interop.IInternetProtocol.Seek(long moveAmount, int dwOrigin, IntPtr newPosition)
        {
            return -2147467263;
        }

        int Interop.IInternetProtocol.Start(string szUrl, Interop.IInternetProtocolSink protocolSink, IntPtr bindInfo, int grfPI, int dwReserved)
        {
            string str = szUrl;
            int index = str.IndexOf(":");
            this._realUrl = str.Substring(index + 1, (str.Length - index) - 1);
            string s = (string) HtmlControl.UrlMap[this._realUrl];
            if (s.Length == 0)
            {
                s = "<html><head></head><body></body></html>";
            }
            this._data = ProtocolHandler.ContentEncoding.GetBytes(s);
            this._index = 0;
            this._length = this._data.Length;
            int grfBSCF = 13;
            protocolSink.ReportData(grfBSCF, (uint) this._length, (uint) this._length);
            return 0;
        }

        int Interop.IInternetProtocol.Suspend()
        {
            return -2147467263;
        }

        int Interop.IInternetProtocol.Terminate(int dwOptions)
        {
            return -2147467263;
        }

        int Interop.IInternetProtocol.UnlockRequest()
        {
            return 0;
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;

    internal sealed class FtpConnection : IDisposable
    {
        private Socket _clientSocket;
        private bool _isOpened;
        private string _password;
        private string _remoteHostName;
        private int _remotePort;
        private string _reply;
        private byte[] _replyBuffer;
        private FtpResponseCode _replyCode;
        private string _userName;
        private const string AsciiModeCommand = "TYPE A\r\n";
        private const string BinaryModeCommand = "TYPE I\r\n";
        private const string DeleteFileCommand = "DELE {0}\r\n";
        private const string ListCommand = "LIST\r\n";
        private const string ListMaskCommand = "LIST {0}\r\n";
        private const string MakeDirectoryCommand = "MKD {0}\r\n";
        private const string PassiveModeCommand = "PASV\r\n";
        private const string PasswordCommand = "PASS {0}\r\n";
        private const string QuitCommand = "QUIT\r\n";
        private const string RemoveDirectoryCommand = "RMD {0}\r\n";
        private const string RenameFromCommand = "RNFR {0}\r\n";
        private const string RenameToCommand = "RNTO {0}\r\n";
        private const int replyBufferSize = 0x200;
        private const string RetrieveCommand = "RETR {0}\r\n";
        private const string SizeCommand = "SIZE {0}\r\n";
        private const string StoreCommand = "STOR {0}\r\n";
        private const string UserCommand = "USER {0}\r\n";

        public FtpConnection(FtpConnectionInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            if (!info.IsValid)
            {
                throw new ArgumentException("info is not valid");
            }
            this._userName = info.UserName;
            this._password = info.Password;
            this._remoteHostName = info.RemoteHostName;
            this._remotePort = info.RemotePort;
        }

        public FtpConnection(string remoteHostName, int remotePort, string userName, string password)
        {
            if ((remoteHostName == null) || (remoteHostName.Length == 0))
            {
                throw new ArgumentNullException("remoteHostName");
            }
            if (remotePort <= 0)
            {
                throw new ArgumentOutOfRangeException("remotePort");
            }
            if ((userName == null) || (userName.Length == 0))
            {
                throw new ArgumentNullException("userName");
            }
            this._userName = userName;
            if (this.IsAnonymous && ((password == null) || (password.Length == 0)))
            {
                throw new FtpException("You must use your email address as your password for anonymous access");
            }
            this._remoteHostName = remoteHostName;
            this._remotePort = remotePort;
            this._password = password;
        }

        private void CheckConnection()
        {
            if (!this._isOpened)
            {
                throw new Exception("Must call Open before issuing FTP requests.");
            }
        }

        public void Close()
        {
            if (this.IsOpened)
            {
                try
                {
                    this.SendCommand("QUIT\r\n", false);
                }
                catch
                {
                }
                this.Disconnect();
                this._isOpened = false;
            }
        }

        private Socket CreateDataSocket()
        {
            this.SendCommand("PASV\r\n");
            if (this._replyCode != FtpResponseCode.EnteringPassiveMode)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
            IPEndPoint remoteEP = null;
            Socket socket = null;
            int index = this._reply.IndexOf('(');
            int num2 = this._reply.IndexOf(')');
            if ((index > 0) || (num2 > index))
            {
                string ipData = this._reply.Substring(index + 1, (num2 - index) - 1);
                remoteEP = this.CreateDataSocketEndPoint(ipData);
            }
            if (remoteEP != null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(remoteEP);
                }
                catch (Exception)
                {
                    socket = null;
                }
            }
            if (socket == null)
            {
                throw new FtpException("Could not open data connection to server.");
            }
            return socket;
        }

        private IPEndPoint CreateDataSocketEndPoint(string ipData)
        {
            bool flag = false;
            int length = ipData.Length;
            string s = string.Empty;
            int[] numArray = new int[6];
            int num2 = 0;
            for (int i = 0; (i < length) && (num2 <= 6); i++)
            {
                char c = ipData[i];
                if (char.IsDigit(c))
                {
                    s = s + c;
                }
                else if (c != ',')
                {
                    flag = true;
                    break;
                }
                if ((c == ',') || ((i + 1) == length))
                {
                    try
                    {
                        numArray[num2++] = int.Parse(s);
                        s = string.Empty;
                    }
                    catch
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                string hostName = string.Concat(new object[] { numArray[0], ".", numArray[1], ".", numArray[2], ".", numArray[3] });
                int port = (numArray[4] << 8) | numArray[5];
                IPHostEntry entry = Dns.Resolve(hostName);
                if (entry != null)
                {
                    return new IPEndPoint(entry.AddressList[0], port);
                }
            }
            return null;
        }

        public void CreateDirectory(string dirName)
        {
            this.CheckConnection();
            if ((dirName == null) || (dirName.Length == 0))
            {
                throw new ArgumentNullException("dirName");
            }
            this.SendCommand(string.Format("MKD {0}\r\n", dirName));
            if (this._replyCode != FtpResponseCode.PathNameCreated)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
        }

        public void DeleteDirectory(string dirName)
        {
            this.CheckConnection();
            if ((dirName == null) || (dirName.Length == 0))
            {
                throw new ArgumentNullException("dirName");
            }
            this.SendCommand(string.Format("RMD {0}\r\n", dirName));
            if (this._replyCode != FtpResponseCode.RequestedFileActionOK)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
        }

        public void DeleteFile(string fileName)
        {
            this.CheckConnection();
            if ((fileName == null) || (fileName.Length == 0))
            {
                throw new ArgumentNullException("fileName");
            }
            this.SendCommand(string.Format("DELE {0}\r\n", fileName));
            if (this._replyCode != FtpResponseCode.RequestedFileActionOK)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
        }

        private void Disconnect()
        {
            if (this._clientSocket != null)
            {
                try
                {
                    this._clientSocket.Close();
                }
                catch
                {
                }
                this._clientSocket = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            this.Close();
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        ~FtpConnection()
        {
            this.Dispose(false);
        }

        public ICollection GetDirectoryListing(string mask)
        {
            string str;
            this.CheckConnection();
            if ((mask != null) && (mask.Length != 0))
            {
                str = string.Format("LIST {0}\r\n", mask);
            }
            else
            {
                str = "LIST\r\n";
            }
            Socket socket = this.CreateDataSocket();
            this.SendCommand(str);
            if ((this._replyCode != FtpResponseCode.FileStatusOK) && (this._replyCode != FtpResponseCode.DataConnectionAlreadyOpen))
            {
                try
                {
                    socket.Close();
                }
                catch
                {
                }
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
            StringBuilder builder = new StringBuilder(0x400);
            int count = 1;
            while (count > 0)
            {
                count = socket.Receive(this._replyBuffer, 0x200, SocketFlags.None);
                if (count > 0)
                {
                    builder.Append(Encoding.ASCII.GetString(this._replyBuffer, 0, count));
                }
            }
            try
            {
                socket.Close();
            }
            catch
            {
            }
            string[] strArray = Regex.Split(builder.ToString(), "\\r*\n");
            int length = strArray.Length;
            if ((length > 0) && (strArray[length - 1].Length == 0))
            {
                length--;
            }
            ArrayList list = new ArrayList();
            for (int i = 0; i < length; i++)
            {
                string entryText = strArray[i];
                if (entryText.Length != 0)
                {
                    try
                    {
                        FtpEntry entry = FtpEntry.CreateEntry(entryText);
                        if ((entry != null) && (entry.FileName.Length != 0))
                        {
                            int num4 = entry.FileName.Length;
                            if (((num4 != 1) || (entry.FileName[0] != '.')) && ((num4 != 2) || !entry.FileName.StartsWith("..")))
                            {
                                list.Add(entry);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            this.ReadHostReply();
            if ((this._replyCode != FtpResponseCode.ClosingDataConnection) && (this._replyCode != FtpResponseCode.RequestedFileActionOK))
            {
                throw new IOException(this._reply.Substring(4));
            }
            return list;
        }

        private Stream GetDownloadStream(string fileName)
        {
            this.CheckConnection();
            this.SetBinaryMode();
            if (!this.GetFileExists(fileName))
            {
                throw new FileNotFoundException("The specified file was not found on the server.", fileName);
            }
            Socket socket = this.CreateDataSocket();
            this.SendCommand(string.Format("RETR {0}\r\n", fileName));
            if ((this._replyCode != FtpResponseCode.FileStatusOK) && (this._replyCode != FtpResponseCode.DataConnectionAlreadyOpen))
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
            return new FtpNetworkStream(this, socket, FileAccess.Read);
        }

        public bool GetFileExists(string fileName)
        {
            this.CheckConnection();
            if ((fileName == null) || (fileName.Length == 0))
            {
                throw new ArgumentNullException("fileName");
            }
            try
            {
                this.GetFileSizeInternal(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetFileSize(string fileName)
        {
            this.CheckConnection();
            if ((fileName == null) || (fileName.Length == 0))
            {
                throw new ArgumentNullException("fileName");
            }
            return this.GetFileSizeInternal(fileName);
        }

        private int GetFileSizeInternal(string fileName)
        {
            this.SendCommand(string.Format("SIZE {0}\r\n", fileName));
            if (this._replyCode != FtpResponseCode.FileStatus)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
            return int.Parse(this._reply.Substring(4));
        }

        public Stream GetFileStream(string fileName, FileAccess access)
        {
            this.CheckConnection();
            if ((fileName == null) || (fileName.Length == 0))
            {
                throw new ArgumentNullException("fileName");
            }
            if (access == FileAccess.Read)
            {
                return this.GetDownloadStream(fileName);
            }
            return this.GetUploadStream(fileName);
        }

        private Stream GetUploadStream(string fileName)
        {
            this.CheckConnection();
            this.SetBinaryMode();
            Socket socket = this.CreateDataSocket();
            this.SendCommand(string.Format("STOR {0}\r\n", fileName));
            if ((this._replyCode != FtpResponseCode.FileStatusOK) && (this._replyCode != FtpResponseCode.DataConnectionAlreadyOpen))
            {
                throw new FtpException(this._reply.Substring(4));
            }
            return new FtpNetworkStream(this, socket, FileAccess.Write);
        }

        private void OnFileTransferComplete()
        {
            this.ReadHostReply();
            if ((this._replyCode != FtpResponseCode.ClosingDataConnection) && (this._replyCode != FtpResponseCode.RequestedFileActionOK))
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
        }

        public void Open()
        {
            if (this.IsOpened)
            {
                throw new InvalidOperationException("Already connected. Must call Disconnect first.");
            }
            this.OpenInternal();
        }

        private void OpenInternal()
        {
            this._replyBuffer = new byte[0x200];
            IPHostEntry entry = Dns.Resolve(this._remoteHostName);
            if (entry == null)
            {
                throw new FtpException("Unable to resolve the host name '" + this._remoteHostName + "' using DNS");
            }
            IPEndPoint remoteEP = new IPEndPoint(entry.AddressList[0], this._remotePort);
            this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0x2710);
            try
            {
                this._clientSocket.Connect(remoteEP);
            }
            catch (Exception exception)
            {
                throw new FtpException("Could not connect to host '" + this._remoteHostName + "'", exception);
            }
            this.ReadHostReply();
            if (this._replyCode != FtpResponseCode.ServiceReady)
            {
                this.Disconnect();
                throw new FtpException("Could not connect to host '" + this._remoteHostName + "' using FTP.");
            }
            this.SendCommand(string.Format("USER {0}\r\n", this._userName), false);
            if ((this._replyCode != FtpResponseCode.UserOKPasswordNeeded) && (this._replyCode != FtpResponseCode.UserLoggedIn))
            {
                this.Disconnect();
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
            if (this._replyCode == FtpResponseCode.UserOKPasswordNeeded)
            {
                this.SendCommand(string.Format("PASS {0}\r\n", this._password), false);
                if (this._replyCode != FtpResponseCode.UserLoggedIn)
                {
                    this.Disconnect();
                    throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
                }
            }
            this._isOpened = true;
        }

        private void ReadHostReply()
        {
            this._reply = this.ReadHostReplyLine();
            try
            {
                this._replyCode = (FtpResponseCode) int.Parse(this._reply.Substring(0, 3));
            }
            catch
            {
                this._replyCode = FtpResponseCode.Unknown;
            }
        }

        private string ReadHostReplyLine()
        {
            int num;
            string str = string.Empty;
            do
            {
                num = this._clientSocket.Receive(this._replyBuffer, 0x200, SocketFlags.None);
                if (num != 0)
                {
                    str = str + Encoding.ASCII.GetString(this._replyBuffer, 0, num);
                }
            }
            while (num >= 0x200);
            if (str.Length != 0)
            {
                string[] strArray = str.Split(new char[] { '\n' });
                if (strArray.Length > 2)
                {
                    str = strArray[strArray.Length - 2];
                }
                else
                {
                    str = strArray[0];
                }
                if (str[3] != ' ')
                {
                    return this.ReadHostReplyLine();
                }
            }
            return str;
        }

        public void RenameFile(string oldFileName, string newFileName)
        {
            this.CheckConnection();
            if ((oldFileName == null) || (oldFileName.Length == 0))
            {
                throw new ArgumentNullException("oldFileName");
            }
            if ((newFileName == null) || (newFileName.Length == 0))
            {
                throw new ArgumentNullException("newFileName");
            }
            this.SendCommand(string.Format("RNFR {0}\r\n", oldFileName));
            if (this._replyCode != FtpResponseCode.FileActionPending)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
            this.SendCommand(string.Format("RNTO {0}\r\n", newFileName));
            if (this._replyCode != FtpResponseCode.RequestedFileActionOK)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
        }

        private void ResendCommand(string command)
        {
            this.Disconnect();
            this.OpenInternal();
            this.SendCommand(command, false);
        }

        private void SendCommand(string command)
        {
            this.SendCommand(command, true);
        }

        private void SendCommand(string command, bool retry)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(command);
            try
            {
                if (this._clientSocket == null)
                {
                    this.OpenInternal();
                }
                this._clientSocket.Send(bytes);
                this.ReadHostReply();
                if (this._replyCode == FtpResponseCode.ClosingControlConnection)
                {
                    retry = false;
                    this.ResendCommand(command);
                }
            }
            catch (Exception exception)
            {
                bool flag = true;
                if (retry)
                {
                    try
                    {
                        this.ResendCommand(command);
                        flag = false;
                    }
                    catch
                    {
                    }
                }
                if (flag)
                {
                    throw new FtpException("The FTP server closed the connection unexpectedly.", exception);
                }
            }
        }

        private void SetAsciiMode()
        {
            this.CheckConnection();
            this.SendCommand("TYPE A\r\n");
            if (this._replyCode != FtpResponseCode.CommandOK)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
        }

        private void SetBinaryMode()
        {
            this.CheckConnection();
            this.SendCommand("TYPE I\r\n");
            if (this._replyCode != FtpResponseCode.CommandOK)
            {
                throw new FtpException(this._reply.Substring(4), (int) this._replyCode);
            }
        }

        public bool IsAnonymous
        {
            get
            {
                return (string.Compare(this._userName, "anonymous", true) == 0);
            }
        }

        public bool IsOpened
        {
            get
            {
                return this._isOpened;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
        }

        public string RemoteHostName
        {
            get
            {
                return this._remoteHostName;
            }
        }

        public int RemotePort
        {
            get
            {
                return this._remotePort;
            }
        }

        public string UserName
        {
            get
            {
                return this._userName;
            }
        }

        private class FtpNetworkStream : NetworkStream
        {
            private FtpConnection _owner;

            public FtpNetworkStream(FtpConnection owner, Socket socket, FileAccess access) : base(socket, access, true)
            {
                this._owner = owner;
            }

            public override void Close()
            {
                base.Close();
                this._owner.OnFileTransferComplete();
            }
        }
    }
}


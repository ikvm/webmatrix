namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using System;

    internal enum FtpResponseCode
    {
        ClosingControlConnection = 0x1a5,
        ClosingDataConnection = 0xe2,
        CommandOK = 200,
        DataConnectionAlreadyOpen = 0x7d,
        DataConnectionOpen = 0xe1,
        EnteringPassiveMode = 0xe3,
        FileActionPending = 350,
        FileStatus = 0xd5,
        FileStatusOK = 150,
        PathNameCreated = 0x101,
        RequestedFileActionOK = 250,
        ServiceClosingControlConnection = 0xdd,
        ServiceReady = 220,
        Unknown = 0,
        UserLoggedIn = 230,
        UserOKPasswordNeeded = 0x14b
    }
}


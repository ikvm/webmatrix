namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;
    using Microsoft.Matrix.Core.Application;

    internal sealed class ApplicationInformation
    {
        private FileVersionInfo _version = FileVersionInfo.GetVersionInfo(MxApplication.Current.GetType().Module.FullyQualifiedName);

        [Category("Process"), Description("The command line and arguments used to start the application.")]
        public string CommandLine
        {
            get
            {
                return Environment.CommandLine;
            }
        }

        [Description("The name of the company who has developed this application."), Category("Application")]
        public string CompanyName
        {
            get
            {
                return this._version.CompanyName;
            }
        }

        [Description("A description of the application"), Category("Application")]
        public string Description
        {
            get
            {
                return this._version.Comments;
            }
        }

        [Category("Operating System"), Description("The version of Microsoft Internet Explorer installed on the machine.")]
        public string InternetExplorerVersion
        {
            get
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer", false);
                    if (key != null)
                    {
                        string str = key.GetValue("Version").ToString();
                        key.Close();
                        return str;
                    }
                }
                catch (Exception)
                {
                }
                return string.Empty;
            }
        }

        [Category("Operating System"), Description("The version of the operating system installed on the machine.")]
        public string OperatingSystemVersion
        {
            get
            {
                return Environment.OSVersion.ToString();
            }
        }

        [Category("Application"), Description("The name of the product which this application belongs to.")]
        public string ProductName
        {
            get
            {
                return this._version.ProductName;
            }
        }

        [Category("Application"), Description("The version of product.")]
        public string ProductVersion
        {
            get
            {
                return this._version.ProductVersion;
            }
        }

        [Category("Process"), Description("The path where this process was started from.")]
        public string StartupPath
        {
            get
            {
                return Application.StartupPath;
            }
        }

        [Description("The amount of physical memory mapped into this process."), Category("Process")]
        public long WorkingSet
        {
            get
            {
                return Environment.WorkingSet;
            }
        }
    }
}


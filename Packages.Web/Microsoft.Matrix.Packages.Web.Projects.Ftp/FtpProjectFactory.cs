namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public sealed class FtpProjectFactory : IProjectFactory, IDisposable
    {
        private Icon _largeIcon;
        private IServiceProvider _serviceProvider;
        private Icon _smallIcon;

        private FtpProject CreateProject(FtpConnection connection, string httpRoot, string httpUrl)
        {
            FtpProject project = new FtpProject(this, this._serviceProvider, connection);
            project.HttpRoot = httpRoot;
            project.HttpUrl = httpUrl;
            return project;
        }

        Project IProjectFactory.CreateProject(object creationArgs)
        {
            if (creationArgs != null)
            {
                FtpConnectionInfo info = creationArgs as FtpConnectionInfo;
                if (info == null)
                {
                    throw new ArgumentException("Invalid creationArgs");
                }
                return this.CreateProject(new FtpConnection(info), info.HttpRoot, info.HttpUrl);
            }
            IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
            if (service != null)
            {
                FtpConnectionDialog form = new FtpConnectionDialog(this._serviceProvider);
                if (service.ShowDialog(form) == DialogResult.OK)
                {
                    return this.CreateProject(form.Connection, form.HttpRoot, form.HttpUrl);
                }
            }
            return null;
        }

        void IProjectFactory.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        object IProjectFactory.SaveProject(Project project)
        {
            FtpProject project2 = project as FtpProject;
            if (project2 == null)
            {
                throw new ArgumentException("Unexpected project type");
            }
            FtpConnectionInfo info = new FtpConnectionInfo();
            info.RemoteHostName = project2.Connection.RemoteHostName;
            info.RemotePort = project2.Connection.RemotePort;
            info.UserName = project2.Connection.UserName;
            info.Password = project2.Connection.Password;
            info.HttpRoot = project2.HttpRoot;
            info.HttpUrl = project2.HttpUrl;
            return info;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }

        bool IProjectFactory.AutoInstantiate
        {
            get
            {
                return false;
            }
        }

        string IProjectFactory.Category
        {
            get
            {
                return null;
            }
        }

        string IProjectFactory.CreateNewDescription
        {
            get
            {
                return "Create a connection to an FTP site.";
            }
        }

        bool IProjectFactory.IsSingleInstance
        {
            get
            {
                return false;
            }
        }

        Icon IProjectFactory.LargeIcon
        {
            get
            {
                if (this._largeIcon == null)
                {
                    this._largeIcon = new Icon(typeof(FtpProjectFactory), "FtpLarge.ico");
                }
                return this._largeIcon;
            }
        }

        string IProjectFactory.Name
        {
            get
            {
                return "FTP Connection";
            }
        }

        Type IProjectFactory.ProjectType
        {
            get
            {
                return typeof(FtpProject);
            }
        }

        ProjectFactorySaveMode IProjectFactory.SaveMode
        {
            get
            {
                return ProjectFactorySaveMode.SaveToPreferences;
            }
        }

        Icon IProjectFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(FtpProjectFactory), "FtpSmall.ico");
                }
                return this._smallIcon;
            }
        }
    }
}


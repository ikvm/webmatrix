namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal sealed class FtpConnectionProjectItem : RootProjectItem
    {
        public FtpConnectionProjectItem(FtpProject owningProject) : base(owningProject, owningProject.Connection.RemoteHostName)
        {
            base.SetIconIndex(3);
            base.SetFlags(ProjectItemFlags.IsDropTarget, true);
        }

        protected override void CreateChildItems()
        {
            FtpConnection connection = ((FtpProject) this.Project).Connection;
            if (!connection.IsOpened)
            {
                Cursor current = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    connection.Open();
                }
                catch (Exception)
                {
                    Cursor.Current = current;
                    IMxUIService service = (IMxUIService) ((IServiceProvider) this.Project).GetService(typeof(IMxUIService));
                    if (service != null)
                    {
                        service.ReportError("The FTP server could not be located.\r\nPlease make sure you are connected to the Internet, and try again by recreating the connection.", "The connection could not be opened.", false);
                    }
                }
                finally
                {
                    Cursor.Current = current;
                }
            }
            if (connection.IsOpened)
            {
                ICollection directoryListing = connection.GetDirectoryListing(this.Path);
                if ((directoryListing != null) && (directoryListing.Count != 0))
                {
                    try
                    {
                        int num;
                        ArrayList list = new ArrayList();
                        ArrayList list2 = new ArrayList();
                        foreach (FtpEntry entry in directoryListing)
                        {
                            if (entry.EntryType == FtpEntryType.File)
                            {
                                ProjectItem item = new FileProjectItem(entry.FileName);
                                list2.Add(item);
                            }
                            else if ((entry.EntryType == FtpEntryType.Directory) && !entry.FileName.StartsWith("_vti"))
                            {
                                ProjectItem item2 = new DirectoryProjectItem(entry.FileName);
                                list.Add(item2);
                            }
                        }
                        int count = list.Count;
                        for (num = 0; num < count; num++)
                        {
                            base.AddChildItem((ProjectItem) list[num]);
                        }
                        int num3 = list2.Count;
                        for (num = 0; num < num3; num++)
                        {
                            base.AddChildItem((ProjectItem) list2[num]);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            base.CreateChildItems();
        }

        public override bool ShowChildrenByDefault
        {
            get
            {
                return false;
            }
        }
    }
}


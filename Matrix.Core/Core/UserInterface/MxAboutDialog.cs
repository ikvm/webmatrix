namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Application;
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public abstract class MxAboutDialog : Form
    {
        protected MxAboutDialog()
        {
        }

        protected void OnClickFeedbackButton(object sender, EventArgs e)
        {
            IUIService service = (IUIService) ((IServiceProvider) MxApplication.Current).GetService(typeof(IUIService));
            if (service != null)
            {
                ApplicationInfoDialog form = new ApplicationInfoDialog(MxApplication.Current.ServiceProvider, true);
                service.ShowDialog(form);
            }
        }

        protected string ApplicationVersion
        {
            get
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(base.GetType().Module.FullyQualifiedName);
                return string.Format("Version {0}.{1} (Build {2})", versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart);
            }
        }

        protected string FrameworkVersion
        {
            get
            {
                return (".NET Framework Version " + Environment.Version);
            }
        }
    }
}


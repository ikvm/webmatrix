namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Win32;
    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class AccessHelper
    {
        private const string MdacDownloadUrl = "http://msdn.microsoft.com/downloads/list/dataaccess.asp";
        private const string MdacRequiredMessage = "The Microsoft Access database administration feature requires Microsoft MDAC 2.7 components on your machine.\n\nWould you like to install MDAC? (Clicking 'Yes' will navigate to http://msdn.microsoft.com/downloads/list/dataaccess.asp)\n\n(Web Matrix must be restarted after installation is complete.)";

        private AccessHelper()
        {
        }

        public static bool IsAdoxPresent()
        {
            RegistryKey key = null;
            bool flag = false;
            try
            {
                System.Type type = typeof(Interop.Catalog);
                key = Registry.ClassesRoot.OpenSubKey(@"CLSID\{" + type.GUID.ToString() + "}");
                if (key != null)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
            }
            return flag;
        }

        public static bool IsValidIdentifier(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if ((s.Length == 0) || (s.Length > 0x40))
            {
                return false;
            }
            if (s.IndexOfAny(new char[] { '.', '~', '`', '[', ']', '"' }) != -1)
            {
                return false;
            }
            if (s.StartsWith(" "))
            {
                return false;
            }
            return true;
        }

        public static void PromptInstallAdox(IServiceProvider serviceProvider)
        {
            IUIService service = (IUIService) serviceProvider.GetService(typeof(IUIService));
            if (service.ShowMessage("The Microsoft Access database administration feature requires Microsoft MDAC 2.7 components on your machine.\n\nWould you like to install MDAC? (Clicking 'Yes' will navigate to http://msdn.microsoft.com/downloads/list/dataaccess.asp)\n\n(Web Matrix must be restarted after installation is complete.)", "Unable to create project.", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IWebBrowsingService service2 = (IWebBrowsingService) serviceProvider.GetService(typeof(IWebBrowsingService));
                if (service2 != null)
                {
                    service2.BrowseUrl("http://msdn.microsoft.com/downloads/list/dataaccess.asp");
                }
            }
        }
    }
}


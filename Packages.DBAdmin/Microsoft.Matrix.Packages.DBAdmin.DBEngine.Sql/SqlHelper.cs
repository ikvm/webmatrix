namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class SqlHelper
    {
        private const string MsdeDownloadUrl = "http://www.asp.net/msde/default.aspx";
        private const string MsdeRequiredMessage = "The SQL Server administration feature requires SQL client components on your machine. These client components are available as part of Microsoft SQL Server or MSDE.\n\nMSDE is a light-weight database for small web applications.\n\nWould you like to install MSDE? (Clicking 'Yes' will navigate to http://www.asp.net/msde/default.aspx)\n\n(Web Matrix must be restarted after installation is complete.)";

        private SqlHelper()
        {
        }

        public static string[] GetIdentifierParts(string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }
            bool flag = false;
            StringBuilder builder = new StringBuilder();
            ArrayList list = new ArrayList();
            for (int i = 0; i < identifier.Length; i++)
            {
                char c = identifier[i];
                char ch2 = c;
                switch (ch2)
                {
                    case '[':
                        if (flag)
                        {
                            throw new ArgumentException(string.Format("The identifier {1} contains invalid characters", identifier));
                        }
                        break;

                    case ']':
                        if (!flag || ((identifier.Length > (i + 2)) && (identifier[i + 1] != '.')))
                        {
                            throw new ArgumentException(string.Format("The identifier {1} contains invalid characters", identifier));
                        }
                        goto Label_0095;

                    case '.':
                    {
                        if (flag)
                        {
                            builder.Append('.');
                        }
                        else
                        {
                            list.Add(builder.ToString());
                            builder.Length = 0;
                        }
                        continue;
                    }
                    default:
                        goto Label_00BD;
                }
                flag = true;
                continue;
            Label_0095:
                flag = false;
                continue;
            Label_00BD:
                if (!flag)
                {
                    ch2 = c;
                    if ((((ch2 != '#') && (ch2 != '@')) && ((ch2 != '_') && !char.IsLetter(c))) && ((builder.Length <= 0) || ((c != '$') && !char.IsDigit(c))))
                    {
                        throw new ArgumentException(string.Format("The identifier {1} contains invalid characters", identifier));
                    }
                }
                builder.Append(c);
            }
            list.Add(builder.ToString());
            return (string[]) list.ToArray(typeof(string));
        }

        public static bool IsDmoPresent()
        {
            RegistryKey key = null;
            bool flag = false;
            try
            {
                System.Type type = typeof(Interop.SqlServer);
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
            if ((s.Length == 0) || (s.Length > 0x80))
            {
                return false;
            }
            if (s[0] == '#')
            {
                return false;
            }
            return true;
        }

        public static void PromptInstallDmo(IServiceProvider serviceProvider)
        {
            IUIService service = (IUIService) serviceProvider.GetService(typeof(IUIService));
            if (service.ShowMessage("The SQL Server administration feature requires SQL client components on your machine. These client components are available as part of Microsoft SQL Server or MSDE.\n\nMSDE is a light-weight database for small web applications.\n\nWould you like to install MSDE? (Clicking 'Yes' will navigate to http://www.asp.net/msde/default.aspx)\n\n(Web Matrix must be restarted after installation is complete.)", "Unable to create project.", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IWebBrowsingService service2 = (IWebBrowsingService) serviceProvider.GetService(typeof(IWebBrowsingService));
                if (service2 != null)
                {
                    service2.BrowseUrl("http://www.asp.net/msde/default.aspx");
                }
            }
        }

        internal static string RemoveBracketedStrings(string s)
        {
            while (true)
            {
                int index = s.IndexOf('[');
                if (index == -1)
                {
                    return s;
                }
                int num2 = s.IndexOf(']', index);
                if (num2 == -1)
                {
                    return s;
                }
                s = s.Remove(index, (num2 - index) + 1);
            }
        }

        internal static string RemoveDelimiters(string s)
        {
            if (s == null)
            {
                return string.Empty;
            }
            if (s.Length < 3)
            {
                return s;
            }
            char ch = s[0];
            char ch2 = s[s.Length - 1];
            bool flag = false;
            switch (ch)
            {
                case '"':
                case '\'':
                    if (ch2 == ch)
                    {
                        flag = true;
                    }
                    break;

                case '[':
                    if (ch2 == ']')
                    {
                        flag = true;
                    }
                    break;
            }
            if (!flag)
            {
                return s;
            }
            return s.Substring(1, s.Length - 2);
        }

        internal static bool TableNamesEqual(string simpleName, string delimitedName)
        {
            string[] identifierParts = GetIdentifierParts(delimitedName);
            if (identifierParts.Length == 0)
            {
                return false;
            }
            return (identifierParts[identifierParts.Length - 1] == simpleName);
        }

        private class OrdinalStringComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.CompareOrdinal((string) x, (string) y);
            }
        }
    }
}


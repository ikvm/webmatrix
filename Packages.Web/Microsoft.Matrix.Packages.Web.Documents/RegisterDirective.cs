namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;

    public sealed class RegisterDirective : Directive
    {
        public RegisterDirective()
        {
        }

        public RegisterDirective(string tagPrefix, string namespaceName, string assemblyName)
        {
            base.Dictionary.Add("tagprefix", tagPrefix);
            base.Dictionary.Add("namespace", namespaceName);
            base.Dictionary.Add("assembly", assemblyName);
        }

        public RegisterDirective(string tagPrefix, string tagName, string sourceFile, bool userControl)
        {
            if (!userControl)
            {
                throw new ArgumentException();
            }
            base.Dictionary.Add("tagprefix", tagPrefix);
            base.Dictionary.Add("tagname", tagName);
            base.Dictionary.Add("src", sourceFile);
        }

        public string AssemblyName
        {
            get
            {
                object obj2 = base.Dictionary["assembly"];
                if (obj2 == null)
                {
                    return "";
                }
                return (string) obj2;
            }
        }

        public override string DirectiveName
        {
            get
            {
                return "Register";
            }
        }

        public bool IsUserControl
        {
            get
            {
                return (base.Dictionary["tagname"] != null);
            }
        }

        public string NamespaceName
        {
            get
            {
                object obj2 = base.Dictionary["namespace"];
                if (obj2 == null)
                {
                    return "";
                }
                return (string) obj2;
            }
        }

        public string SourceFile
        {
            get
            {
                object obj2 = base.Dictionary["src"];
                if (obj2 == null)
                {
                    return "";
                }
                return (string) obj2;
            }
        }

        public string TagName
        {
            get
            {
                object obj2 = base.Dictionary["tagname"];
                if (obj2 == null)
                {
                    return "";
                }
                return (string) obj2;
            }
        }

        public string TagPrefix
        {
            get
            {
                object obj2 = base.Dictionary["tagprefix"];
                if (obj2 == null)
                {
                    return "";
                }
                return (string) obj2;
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;

    public abstract class DocumentDirective : Directive
    {
        public string ClassName
        {
            get
            {
                string str = (string) base.Dictionary["classname"];
                if (str == null)
                {
                    return "";
                }
                return str;
            }
            set
            {
                base.Dictionary["classname"] = value;
            }
        }

        public bool Debug
        {
            get
            {
                object obj2 = base.Dictionary["debug"];
                if (obj2 == null)
                {
                    return false;
                }
                string str = (string) obj2;
                return str.ToLower().Equals("true");
            }
            set
            {
                base.Dictionary["debug"] = value.ToString();
            }
        }

        public bool EnableViewState
        {
            get
            {
                object obj2 = base.Dictionary["enableviewstate"];
                if (obj2 == null)
                {
                    return true;
                }
                string str = (string) obj2;
                return str.ToLower().Equals("true");
            }
            set
            {
                base.Dictionary["enableviewstate"] = value.ToString();
            }
        }

        public bool Explicit
        {
            get
            {
                object obj2 = base.Dictionary["explicit"];
                if (obj2 == null)
                {
                    return false;
                }
                string str = (string) obj2;
                return str.ToLower().Equals("true");
            }
            set
            {
                base.Dictionary["explicit"] = value.ToString();
            }
        }

        public string Inherits
        {
            get
            {
                string str = (string) base.Dictionary["inherits"];
                if (str == null)
                {
                    return "";
                }
                return str;
            }
            set
            {
                base.Dictionary["inherits"] = value;
            }
        }

        public string Language
        {
            get
            {
                return (base.Dictionary["language"] as string);
            }
            set
            {
                base.Dictionary["language"] = value;
            }
        }

        public string Src
        {
            get
            {
                string str = (string) base.Dictionary["src"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                base.Dictionary["src"] = value;
            }
        }

        public bool Strict
        {
            get
            {
                object obj2 = base.Dictionary["strict"];
                if (obj2 == null)
                {
                    return false;
                }
                string str = (string) obj2;
                return str.ToLower().Equals("true");
            }
            set
            {
                base.Dictionary["strict"] = value.ToString();
            }
        }

        public int WarningLevel
        {
            get
            {
                object obj2 = base.Dictionary["warninglevel"];
                if (obj2 == null)
                {
                    return 0;
                }
                try
                {
                    return int.Parse((string) obj2);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                if ((value < 0) || (value > 4))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                base.Dictionary["warninglevel"] = value.ToString();
            }
        }
    }
}


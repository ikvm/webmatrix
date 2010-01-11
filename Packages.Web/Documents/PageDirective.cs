namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;
    using System.Web;

    public sealed class PageDirective : DocumentDirective
    {
        public string ClientTarget
        {
            get
            {
                string str = (string) base.Dictionary["clienttarget"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                base.Dictionary["clienttarget"] = value;
            }
        }

        public string Culture
        {
            get
            {
                string str = (string) base.Dictionary["culture"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                base.Dictionary["culture"] = value;
            }
        }

        public override string DirectiveName
        {
            get
            {
                return "Page";
            }
        }

        public bool EnableViewStateMac
        {
            get
            {
                object obj2 = base.Dictionary["enableviewstatemac"];
                if (obj2 == null)
                {
                    return false;
                }
                string str = (string) obj2;
                return str.ToLower().Equals("true");
            }
            set
            {
                base.Dictionary["enableviewstatemac"] = value.ToString();
            }
        }

        public string ErrorPage
        {
            get
            {
                string str = (string) base.Dictionary["errorpage"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                base.Dictionary["errorpage"] = value;
            }
        }

        public bool Trace
        {
            get
            {
                object obj2 = base.Dictionary["trace"];
                if (obj2 == null)
                {
                    return false;
                }
                string str = (string) obj2;
                return str.ToLower().Equals("true");
            }
            set
            {
                base.Dictionary["trace"] = value.ToString();
            }
        }

        public System.Web.TraceMode TraceMode
        {
            get
            {
                object obj2 = base.Dictionary["tracemode"];
                if (obj2 == null)
                {
                    return System.Web.TraceMode.Default;
                }
                return (System.Web.TraceMode) Enum.Parse(typeof(System.Web.TraceMode), obj2.ToString(), true);
            }
            set
            {
                if (value == System.Web.TraceMode.Default)
                {
                    base.Dictionary.Remove("tracemode");
                }
                else
                {
                    string name = Enum.GetName(typeof(System.Web.TraceMode), value);
                    base.Dictionary["tracemode"] = name;
                }
            }
        }
    }
}


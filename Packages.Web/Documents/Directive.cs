namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Text;

    public abstract class Directive
    {
        private static IDictionary _casedNameTable = new HybridDictionary(true);
        private HybridDictionary _dictionary;

        static Directive()
        {
            _casedNameTable["aspcompat"] = "AspCompat";
            _casedNameTable["assembly"] = "Assembly";
            _casedNameTable["buffer"] = "Buffer";
            _casedNameTable["class"] = "Class";
            _casedNameTable["classname"] = "ClassName";
            _casedNameTable["clienttarget"] = "ClientTarget";
            _casedNameTable["codepage"] = "CodePage";
            _casedNameTable["compileroptions"] = "CompilerOptions";
            _casedNameTable["contenttype"] = "ContentType";
            _casedNameTable["culture"] = "Culture";
            _casedNameTable["debug"] = "Debug";
            _casedNameTable["enablesessionstate"] = "EnableSessionState";
            _casedNameTable["enableviewstate"] = "EnableViewState";
            _casedNameTable["enableviewstatemac"] = "EnableViewStateMac";
            _casedNameTable["errorpage"] = "ErrorPage";
            _casedNameTable["explicit"] = "Explicit";
            _casedNameTable["inherits"] = "Inherits";
            _casedNameTable["language"] = "Language";
            _casedNameTable["lcid"] = "LCID";
            _casedNameTable["namespace"] = "Namespace";
            _casedNameTable["responseencoding"] = "ResponseEncoding";
            _casedNameTable["src"] = "Src";
            _casedNameTable["strict"] = "Strict";
            _casedNameTable["tagprefix"] = "TagPrefix";
            _casedNameTable["tagname"] = "TagName";
            _casedNameTable["trace"] = "Trace";
            _casedNameTable["tracemode"] = "TraceMode";
            _casedNameTable["transaction"] = "Transaction";
            _casedNameTable["warninglevel"] = "WarningLevel";
        }

        protected Directive()
        {
        }

        public void AddAttribute(string attributeName, string attributeValue)
        {
            if ((attributeValue == null) || (attributeValue.Length == 0))
            {
                this.Dictionary.Remove(attributeName.ToLower());
            }
            else
            {
                this.Dictionary[attributeName.ToLower()] = attributeValue;
            }
        }

        protected string GetCasedName(string name)
        {
            string str = (string) _casedNameTable[name.ToLower()];
            if (str != null)
            {
                return str;
            }
            return name;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<%@ ");
            builder.Append(this.DirectiveName);
            IDictionaryEnumerator enumerator = this.Dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                string casedName = this.GetCasedName((string) current.Key);
                string str2 = string.Empty;
                object obj2 = current.Value;
                if (obj2 != null)
                {
                    str2 = obj2.ToString();
                }
                if ((str2 != null) && (str2.Length > 0))
                {
                    builder.Append(" ");
                    builder.Append(casedName);
                    if (str2.IndexOf("\"") != -1)
                    {
                        builder.Append("='");
                        builder.Append(str2);
                        builder.Append('\'');
                    }
                    else
                    {
                        builder.Append("=\"");
                        builder.Append(str2);
                        builder.Append('"');
                    }
                }
            }
            builder.Append(" %>");
            return builder.ToString();
        }

        protected IDictionary Dictionary
        {
            get
            {
                if (this._dictionary == null)
                {
                    this._dictionary = new HybridDictionary();
                }
                return this._dictionary;
            }
        }

        public abstract string DirectiveName { get; }
    }
}


namespace Microsoft.Matrix.Packages.Web.Utility
{
    using Microsoft.Matrix.Packages.Web.Documents;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    internal sealed class DirectiveParser
    {
        private string _defaultDirectiveName;
        private int _directiveEndIndex;
        private ArrayList _directiveIndexes;
        private ArrayList _directives;
        private static readonly Regex directiveRegex = new Regex("<%\\s*@(\\s*(?<attrname>\\w+(?=\\W))(\\s*(?<equal>=)\\s*\"(?<attrval>[^\"]*)\"|\\s*(?<equal>=)\\s*'(?<attrval>[^']*)'|\\s*(?<equal>=)\\s*(?<attrval>[^\\s%>]*)|(?<equal>)(?<attrval>\\s*?)))*\\s*?%>\\s*", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.Multiline);

        public DirectiveParser(string text, string defaultDirectiveName)
        {
            this._defaultDirectiveName = defaultDirectiveName.ToLower();
            this._directives = new ArrayList();
            this._directiveIndexes = new ArrayList();
            directiveRegex.Replace(text, new MatchEvaluator(this.DirectiveMatchEvaluator), this.CountDirectives(text, out this._directiveEndIndex));
        }

        private int CountDirectives(string text, out int endIndex)
        {
            int num = 0;
            int num2 = 0;
            endIndex = 0;
            this._directiveIndexes.Add((int) endIndex);
            while ((num < text.Length) && char.IsWhiteSpace(text[num]))
            {
                num++;
            }
            if ((((num + 1) < text.Length) && (text[num] == '<')) && (text[num + 1] == '%'))
            {
                while ((num + 1) < text.Length)
                {
                    if ((text[num] == '%') && (text[num + 1] == '>'))
                    {
                        num2++;
                        num += 2;
                        endIndex = num;
                        this._directiveIndexes.Add((int) endIndex);
                        while ((num < text.Length) && char.IsWhiteSpace(text[num]))
                        {
                            num++;
                        }
                        if (((num >= text.Length) || (text[num] != '<')) || (text[num + 1] != '%'))
                        {
                            return num2;
                        }
                    }
                    num++;
                }
            }
            return num2;
        }

        private string DirectiveMatchEvaluator(Match match)
        {
            GroupCollection groups = match.Groups;
            CaptureCollection captures = match.Groups["attrname"].Captures;
            if (captures.Count > 0)
            {
                CaptureCollection captures2 = match.Groups["attrval"].Captures;
                CaptureCollection captures3 = match.Groups["equal"].Captures;
                Directive directive = null;
                int num = 0;
                string name = this._defaultDirectiveName;
                if ((captures2[0].ToString().Length == 0) && (captures3[0].ToString().Length == 0))
                {
                    name = captures[0].ToString().ToLower();
                    num = 1;
                }
                if (name.Equals("page"))
                {
                    directive = new PageDirective();
                }
                else if (name.Equals("control"))
                {
                    directive = new ControlDirective();
                }
                else if (name.Equals("register"))
                {
                    directive = new RegisterDirective();
                }
                else if (name.Equals("webservice"))
                {
                    directive = new WebServiceDirective();
                }
                else if (name.Equals("webhandler"))
                {
                    directive = new WebHandlerDirective();
                }
                else
                {
                    directive = new UnknownDirective(name);
                }
                for (int i = num; i < captures.Count; i++)
                {
                    directive.AddAttribute(captures[i].ToString(), captures2[i].ToString());
                }
                this._directives.Add(directive);
            }
            return match.ToString();
        }

        public int DirectiveEndIndex
        {
            get
            {
                return (int) this._directiveIndexes[this._directives.Count];
            }
        }

        public ICollection Directives
        {
            get
            {
                return this._directives;
            }
        }
    }
}


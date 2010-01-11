namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.Web.Documents.Design;
    using Microsoft.Matrix.Packages.Web.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    [Designer(typeof(WebFormsDocumentDesigner), typeof(IDesigner)), DesignTimeVisible(false)]
    public abstract class WebFormsDocument : HtmlDocument, IDocumentWithCode
    {
        private CodeDocumentStorage _codeStorage;
        private Microsoft.Matrix.Packages.Web.Documents.DocumentDirective _documentDirective;
        private ArrayList _miscDirectives;
        private RegisterDirectiveCollection _registerDirectives;
        private ArrayList _scriptIncludes;
        private string _text;
        private static readonly Regex scriptRegex = new Regex("((</script\\s*>)|(<script(\\s+(?<attrname>[-\\w]+)(\\s*=\\s*\"(?<attrval>[^\"]*)\"|\\s*=\\s*'(?<attrval>[^']*)'|\\s*=\\s*(?<attrval>[^\\s=/>]*)|\\s*=\\s*(?<attrval><%#.*?%>)|(?<attrval>\\s*?)))*\\s*(?<empty>/)?>(\\n|\\r)*))", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public WebFormsDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        private int CountDirectives(string text)
        {
            int num = 0;
            int num2 = 0;
            while ((num + 1) < text.Length)
            {
                if ((text[num] == '%') && (text[num + 1] == '>'))
                {
                    num2++;
                    num += 2;
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
            return num2;
        }

        protected abstract Microsoft.Matrix.Packages.Web.Documents.DocumentDirective CreateDocumentDirective();
        protected override IDocumentStorage CreateStorage()
        {
            return new WebFormsDocumentStorage(this);
        }

        private void EnsureTextProcessed()
        {
            if (this._text != null)
            {
                string text = this._text;
                this._text = null;
                this.ProcessCompleteDocument(text);
            }
        }

        protected abstract bool IsValidDocumentDirective(Microsoft.Matrix.Packages.Web.Documents.DocumentDirective directive);
        protected override void OnAfterLoad(EventArgs e)
        {
            this._text = base.Text;
            this.EnsureTextProcessed();
            base.OnAfterLoad(e);
        }

        internal void ProcessCompleteDocument(string text)
        {
            this._documentDirective = null;
            RegisterDirectiveCollection registerDirectives = this.RegisterDirectives;
            ArrayList miscDirectives = this.MiscDirectives;
            ArrayList scriptIncludes = this.ScriptIncludes;
            miscDirectives.Clear();
            scriptIncludes.Clear();
            registerDirectives.Clear();
            DirectiveParser parser = new DirectiveParser(text, this.DocumentDirectiveName);
            foreach (Directive directive in parser.Directives)
            {
                if (directive is Microsoft.Matrix.Packages.Web.Documents.DocumentDirective)
                {
                    this._documentDirective = (Microsoft.Matrix.Packages.Web.Documents.DocumentDirective) directive;
                }
                else
                {
                    if (directive is RegisterDirective)
                    {
                        registerDirectives.AddParsedRegisterDirective((RegisterDirective) directive);
                        continue;
                    }
                    miscDirectives.Add(directive);
                }
            }
            text = text.Substring(parser.DirectiveEndIndex);
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            bool flag = false;
            int startIndex = 0;
            int num2 = 0;
            foreach (Match match in scriptRegex.Matches(text))
            {
                if (match.ToString().Substring(0, 2).ToLower(CultureInfo.InvariantCulture).Equals("<s"))
                {
                    if (!flag)
                    {
                        GroupCollection groups = match.Groups;
                        CaptureCollection captures = groups["attrname"].Captures;
                        CaptureCollection captures2 = groups["attrval"].Captures;
                        bool flag2 = false;
                        for (int i = 0; i < captures.Count; i++)
                        {
                            string str2 = captures[i].ToString().ToLower();
                            string str3 = captures2[i].ToString().ToLower();
                            if (str2.Equals("runat") && str3.Equals("server"))
                            {
                                flag2 = true;
                            }
                            if (str2.Equals("language") && (this.DocumentDirective.Language == null))
                            {
                                this.DocumentDirective.Language = str3;
                            }
                        }
                        if (flag2)
                        {
                            if (groups["empty"].Captures.Count > 0)
                            {
                                string str4 = match.ToString().Trim();
                                scriptIncludes.Add(str4);
                                num2 = match.Index + match.Length;
                            }
                            else
                            {
                                flag = true;
                                int index = match.Index;
                                startIndex = index + match.Length;
                                string str5 = text.Substring(num2, index - num2);
                                builder2.Append(str5);
                            }
                        }
                    }
                    continue;
                }
                if (flag)
                {
                    flag = false;
                    int num5 = match.Index;
                    string script = text.Substring(startIndex, num5 - startIndex);
                    ScriptFormatter formatter = new ScriptFormatter();
                    formatter.AddScript(script);
                    builder.Append(formatter.ToString());
                    builder.Append("\r\n");
                    num2 = num5 + match.Length;
                }
            }
            builder2.Append(text.Substring(num2).Trim());
            CodeDocumentLanguage documentLanguage = null;
            ILanguageManager service = (ILanguageManager) this.GetService(typeof(ILanguageManager));
            if (service != null)
            {
                string name = this.DocumentDirective.Language;
                if ((name != null) && (name.Length != 0))
                {
                    documentLanguage = service.GetDocumentLanguage(name) as CodeDocumentLanguage;
                }
                if (documentLanguage == null)
                {
                    documentLanguage = (CodeDocumentLanguage) service.DefaultCodeLanguage;
                    this.DocumentDirective.Language = documentLanguage.Name;
                }
            }
            this._codeStorage = new CodeDocumentStorage(documentLanguage);
            ((IEmbeddedDocumentStorage) this._codeStorage).SetContainerDocument(this);
            this._codeStorage.Text = builder.ToString().Trim();
            ((WebFormsDocumentStorage) base.Storage).Text = builder2.ToString().Trim();
        }

        internal void SaveCompleteDocumentToStream(Stream stream, bool writeUTF8Preamble)
        {
            writeUTF8Preamble = writeUTF8Preamble && (this._codeStorage.ContainsUnicodeCharacters || ((TextDocumentStorage) base.Storage).ContainsUnicodeCharacters);
            StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(writeUTF8Preamble));
            this.SaveDirectivesToWriter(writer);
            writer.Flush();
            if (this.ScriptIncludes.Count > 0)
            {
                foreach (string str in this.ScriptIncludes)
                {
                    writer.WriteLine(str);
                }
            }
            string s = this._codeStorage.Text.Trim();
            if ((this._codeStorage != null) && (s.Length > 0))
            {
                writer.WriteLine("<script runat=\"server\">");
                StringReader reader = new StringReader(s);
                string str3 = reader.ReadLine();
                if ((str3 != null) && (str3.Trim().Length > 0))
                {
                    writer.WriteLine();
                }
                string str4 = str3;
                while (str3 != null)
                {
                    writer.Write("    ");
                    writer.WriteLine(str3);
                    str4 = str3;
                    str3 = reader.ReadLine();
                }
                if ((str4 != null) && (str4.Trim().Length > 0))
                {
                    writer.WriteLine();
                }
                writer.WriteLine("</script>");
            }
            writer.Flush();
            base.SaveStorageToStream(stream);
        }

        private void SaveDirectivesToWriter(StreamWriter writer)
        {
            if (this._documentDirective != null)
            {
                writer.WriteLine(this._documentDirective.ToString());
            }
            foreach (RegisterDirective directive in this.RegisterDirectives)
            {
                writer.WriteLine(directive.ToString());
            }
            foreach (Directive directive2 in this.MiscDirectives)
            {
                writer.WriteLine(directive2.ToString());
            }
        }

        protected override void SaveStorageToStream(Stream stream)
        {
            if (this._text == null)
            {
                this.EnsureTextProcessed();
                this.SaveCompleteDocumentToStream(stream, true);
            }
            else
            {
                bool flag = false;
                for (int i = 0; i < this._text.Length; i++)
                {
                    if (this._text[i] > '\x007f')
                    {
                        flag = true;
                        break;
                    }
                }
                StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(flag));
                writer.Write(this._text);
                writer.Flush();
            }
        }

        [Category("Behavior"), DefaultValue(""), Description("The name of the generated code-behind class")]
        public string ClassName
        {
            get
            {
                return this.DocumentDirective.ClassName;
            }
            set
            {
                if (!this.DocumentDirective.ClassName.Equals(value))
                {
                    this.DocumentDirective.ClassName = value;
                    base.SetDirty();
                }
            }
        }

        [Browsable(false)]
        public string CodeLanguage
        {
            get
            {
                return this.DocumentDirective.Language;
            }
        }

        [Description("Whether debug mode should be enabled"), DefaultValue(false), Category("Compilation")]
        public bool Debug
        {
            get
            {
                return this.DocumentDirective.Debug;
            }
            set
            {
                if (this.DocumentDirective.Debug != value)
                {
                    this.DocumentDirective.Debug = value;
                    base.SetDirty();
                }
            }
        }

        [Browsable(false)]
        public Microsoft.Matrix.Packages.Web.Documents.DocumentDirective DocumentDirective
        {
            get
            {
                if (this._documentDirective == null)
                {
                    this._documentDirective = this.CreateDocumentDirective();
                }
                this.EnsureTextProcessed();
                return this._documentDirective;
            }
        }

        protected abstract string DocumentDirectiveName { get; }

        [Category("Behavior"), DefaultValue(true), Description("Whether view state should be enabled")]
        public bool EnableViewState
        {
            get
            {
                return this.DocumentDirective.EnableViewState;
            }
            set
            {
                if (this.DocumentDirective.EnableViewState != value)
                {
                    this.DocumentDirective.EnableViewState = value;
                    base.SetDirty();
                }
            }
        }

        [Category("Compilation"), Description("Whether to compile in Visual Basic Option Explicit mode"), DefaultValue(false)]
        public bool Explicit
        {
            get
            {
                return this.DocumentDirective.Explicit;
            }
            set
            {
                if (this.DocumentDirective.Explicit != value)
                {
                    this.DocumentDirective.Explicit = value;
                    base.SetDirty();
                }
            }
        }

        [Browsable(false)]
        public string Html
        {
            get
            {
                this.EnsureTextProcessed();
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [Category("Compilation"), DefaultValue(""), Description("The code-behind class from which to inherit")]
        public string Inherits
        {
            get
            {
                return this.DocumentDirective.Inherits;
            }
            set
            {
                if (!this.DocumentDirective.Inherits.Equals(value))
                {
                    this.DocumentDirective.Inherits = value;
                    base.SetDirty();
                }
            }
        }

        public override DocumentLanguage Language
        {
            get
            {
                return WebFormsDocumentLanguage.Instance;
            }
        }

        CodeDocumentStorage IDocumentWithCode.Code
        {
            get
            {
                this.EnsureTextProcessed();
                return this._codeStorage;
            }
        }

        private ArrayList MiscDirectives
        {
            get
            {
                if (this._miscDirectives == null)
                {
                    this._miscDirectives = new ArrayList();
                }
                this.EnsureTextProcessed();
                return this._miscDirectives;
            }
        }

        [Browsable(false)]
        public RegisterDirectiveCollection RegisterDirectives
        {
            get
            {
                if (this._registerDirectives == null)
                {
                    this._registerDirectives = new RegisterDirectiveCollection();
                }
                this.EnsureTextProcessed();
                return this._registerDirectives;
            }
        }

        [Browsable(false)]
        private ArrayList ScriptIncludes
        {
            get
            {
                if (this._scriptIncludes == null)
                {
                    this._scriptIncludes = new ArrayList();
                }
                this.EnsureTextProcessed();
                return this._scriptIncludes;
            }
        }

        [Category("Compilation"), Description("The source file name for the code-behind class"), DefaultValue("")]
        public string Src
        {
            get
            {
                return this.DocumentDirective.Src;
            }
            set
            {
                if (!this.DocumentDirective.Src.Equals(value))
                {
                    this.DocumentDirective.Src = value;
                    base.SetDirty();
                }
            }
        }

        [Category("Compilation"), DefaultValue(false), Description("Whether to compile in Visual Basic Option Strict mode")]
        public bool Strict
        {
            get
            {
                return this.DocumentDirective.Strict;
            }
            set
            {
                if (this.DocumentDirective.Strict != value)
                {
                    this.DocumentDirective.Strict = value;
                    base.SetDirty();
                }
            }
        }

        public override string Text
        {
            get
            {
                if (this._text == null)
                {
                    MemoryStream stream = new MemoryStream();
                    this.SaveCompleteDocumentToStream(stream, false);
                    this._text = Encoding.UTF8.GetString(stream.ToArray());
                }
                return this._text;
            }
            set
            {
                this._text = value;
                base.SetDirty();
            }
        }

        [Category("Compilation"), DefaultValue(0), Description("The warning level to use for the compilation")]
        public int WarningLevel
        {
            get
            {
                return this.DocumentDirective.WarningLevel;
            }
            set
            {
                if (this.DocumentDirective.WarningLevel != value)
                {
                    this.DocumentDirective.WarningLevel = value;
                    base.SetDirty();
                }
            }
        }
    }
}


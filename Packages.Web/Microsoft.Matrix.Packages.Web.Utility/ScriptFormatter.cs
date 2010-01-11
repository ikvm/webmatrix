namespace Microsoft.Matrix.Packages.Web.Utility
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    internal sealed class ScriptFormatter
    {
        private int _length;
        private ArrayList _lines;
        private int _minPadding = 0x7fffffff;
        public static int TabSize = 4;

        public void AddScript(string script)
        {
            StringReader reader = new StringReader(script);
            for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
            {
                ScriptLine line = new ScriptLine(str);
                int padding = line.Padding;
                if (padding < this._minPadding)
                {
                    this._minPadding = padding;
                }
                this._length += line.Length + 2;
                if (this._lines == null)
                {
                    this._lines = new ArrayList();
                }
                this._lines.Add(line);
            }
        }

        public override string ToString()
        {
            if (this._lines == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder(this._length);
            foreach (ScriptLine line in this._lines)
            {
                line.Padding -= this._minPadding;
                builder.Append(line.ToString());
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }

        private class ScriptLine
        {
            private bool _isBlank;
            private int _padding;
            private string _script;

            public ScriptLine(string script)
            {
                int num = 0;
                int num2 = 0;
                if (script.Length > 0)
                {
                    char c = script[num];
                    while (char.IsWhiteSpace(c) && (num < script.Length))
                    {
                        if (c == '\t')
                        {
                            num2 += ScriptFormatter.TabSize;
                        }
                        else
                        {
                            num2++;
                        }
                        c = script[num];
                        num++;
                    }
                }
                this._padding = num2;
                this._script = script.Trim();
                if (this._script.Length == 0)
                {
                    this._isBlank = true;
                    this._padding = 0;
                }
            }

            public override string ToString()
            {
                if (this.IsBlank)
                {
                    return "";
                }
                StringBuilder builder = new StringBuilder(this.Padding + this.Script.Length);
                for (int i = 0; i < this.Padding; i++)
                {
                    builder.Append(' ');
                }
                builder.Append(this.Script);
                return builder.ToString();
            }

            public bool IsBlank
            {
                get
                {
                    return this._isBlank;
                }
            }

            public int Length
            {
                get
                {
                    if (this.IsBlank)
                    {
                        return 0;
                    }
                    return (this.Padding + this.Script.Length);
                }
            }

            public int Padding
            {
                get
                {
                    if (this.IsBlank)
                    {
                        return 0x7fffffff;
                    }
                    return this._padding;
                }
                set
                {
                    if (!this.IsBlank)
                    {
                        this._padding = value;
                    }
                }
            }

            public string Script
            {
                get
                {
                    return this._script;
                }
            }
        }
    }
}


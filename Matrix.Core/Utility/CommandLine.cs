namespace Microsoft.Matrix.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;

    public sealed class CommandLine
    {
        private string[] _arguments;
        private IDictionary _options;
        private bool _showHelp;

        public CommandLine() : this(Environment.GetCommandLineArgs())
        {
        }

        public CommandLine(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            ArrayList list = new ArrayList();
            for (int i = 0; i < args.Length; i++)
            {
                char ch = args[i][0];
                if ((ch != '/') && (ch != '-'))
                {
                    list.Add(args[i]);
                }
                else
                {
                    int index = args[i].IndexOf(':');
                    if (index == -1)
                    {
                        string strA = args[i].Substring(1);
                        if ((string.Compare(strA, "help", true, CultureInfo.InvariantCulture) == 0) || strA.Equals("?"))
                        {
                            this._showHelp = true;
                        }
                        else
                        {
                            this.Options[strA] = string.Empty;
                        }
                    }
                    else
                    {
                        this.Options[args[i].Substring(1, index - 1)] = args[i].Substring(index + 1);
                    }
                }
            }
            this._arguments = (string[]) list.ToArray(typeof(string));
        }

        public string[] Arguments
        {
            get
            {
                return this._arguments;
            }
        }

        public IDictionary Options
        {
            get
            {
                if (this._options == null)
                {
                    this._options = new HybridDictionary(true);
                }
                return this._options;
            }
        }

        public bool ShowHelp
        {
            get
            {
                return this._showHelp;
            }
        }
    }
}


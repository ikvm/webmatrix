namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.IO;

    public class WebFormsAllView : SourceView
    {
        private bool _initialLoad;
        private bool _previewModeEnabled;
        private RegisterDirectiveCollection _registerDirectives;
        private string _viewName;

        public WebFormsAllView(IServiceProvider provider, string viewName) : base(provider)
        {
            this._viewName = viewName;
            this._previewModeEnabled = !WebPackage.Instance.DesignModeEnabled;
            this._initialLoad = true;
        }

        protected override bool HandleCommand(Command command)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(WebCommands)) && (command.CommandID == 250))
            {
                StringWriter output = new StringWriter();
                HtmlFormatter formatter = new HtmlFormatter();
                base.ClearSelection();
                formatter.Format(this.Text, output, new HtmlFormatterOptions(' ', 4, 80, true));
                output.Flush();
                base.SetText(output.ToString(), true);
                flag = true;
            }
            if (!flag)
            {
                flag = base.HandleCommand(command);
            }
            return flag;
        }

        protected override void LoadDocument()
        {
            if (this._initialLoad || !this._previewModeEnabled)
            {
                this._initialLoad = false;
                base.LoadDocument();
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (this._registerDirectives == null)
            {
                this._registerDirectives = ((WebFormsDocument) base.Document).RegisterDirectives;
                this._registerDirectives.DirectiveAdded += new DirectiveEventHandler(this.OnDirectiveAdded);
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (this._registerDirectives != null)
            {
                this._registerDirectives.DirectiveAdded -= new DirectiveEventHandler(this.OnDirectiveAdded);
                this._registerDirectives = null;
            }
        }

        private void OnDirectiveAdded(object sender, DirectiveEventArgs e)
        {
            using (TextBufferLocation location = base.Buffer.CreateFirstCharacterLocation())
            {
                Token nextToken = HtmlTokenizer.GetFirstToken(location.Line.ToCharArray(), location.Line.Length, 0);
                while (nextToken != null)
                {
                    string str = nextToken.Text.Trim();
                    if (nextToken.Type == 0x16)
                    {
                        int num = 2;
                        while ((num < str.Length) && char.IsWhiteSpace(str[num]))
                        {
                            num++;
                        }
                        if (str[num] != '@')
                        {
                            break;
                        }
                    }
                    else if ((nextToken.Type != 4) || (str != string.Empty))
                    {
                        break;
                    }
                    int endState = nextToken.EndState;
                    nextToken = HtmlTokenizer.GetNextToken(nextToken);
                    if (nextToken == null)
                    {
                        if (location.MoveDown(1) == 0)
                        {
                            break;
                        }
                        nextToken = HtmlTokenizer.GetFirstToken(location.Line.ToCharArray(), location.Line.Length, endState);
                    }
                }
                location.ColumnIndex = nextToken.StartIndex;
                base.Buffer.InsertText(location, e.Directive.ToString() + Environment.NewLine + Environment.NewLine);
            }
        }

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            Type type = section.GetType();
            if ((!base.SupportsToolboxSection(section) && (type != typeof(HtmlElementToolboxSection))) && ((type != typeof(WebFormsToolboxSection)) && (type != typeof(CustomControlsToolboxSection))))
            {
                return (type == typeof(CodeWizardToolboxSection));
            }
            return true;
        }

        protected override bool UpdateCommand(Command command)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(WebCommands)) && (command.CommandID == 250))
            {
                command.Enabled = true;
                flag = true;
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }

        protected override bool SupportsPropertyBrowser
        {
            get
            {
                return false;
            }
        }

        protected override string ViewName
        {
            get
            {
                return this._viewName;
            }
        }

        protected override DocumentViewType ViewType
        {
            get
            {
                return DocumentViewType.Composite;
            }
        }
    }
}


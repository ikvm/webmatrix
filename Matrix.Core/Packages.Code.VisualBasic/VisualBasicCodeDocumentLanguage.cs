namespace Microsoft.Matrix.Packages.Code.VisualBasic
{
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using Microsoft.VisualBasic;

    public sealed class VisualBasicCodeDocumentLanguage : CodeDocumentLanguage, ICodeBehindDocumentLanguage
    {
        private ITextColorizer _colorizer;
        internal static VisualBasicCodeDocumentLanguage Instance;

        public VisualBasicCodeDocumentLanguage() : base("VB", new VBCodeProvider())
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        protected override ITextColorizer GetColorizer(IServiceProvider serviceProvider)
        {
            if (this._colorizer == null)
            {
                this._colorizer = new VisualBasicColorizer();
            }
            return this._colorizer;
        }

        protected override ITextControlHost GetTextControlHost(TextControl control, IServiceProvider provider)
        {
            return new VisualBasicTextControlHost(control);
        }

        int ICodeBehindDocumentLanguage.GenerateEventHandler(TextBuffer buffer, string methodName, EventDescriptor eventDescriptor, out bool existing)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((methodName == null) || (methodName.Length == 0))
            {
                throw new ArgumentNullException("methodName");
            }
            if (eventDescriptor == null)
            {
                throw new ArgumentNullException("eventDescriptor");
            }
            existing = false;
            int num = -1;
            string searchString = "Sub " + methodName;
            using (TextBufferSpan span = buffer.Find(searchString, true, false, false))
            {
                if (span != null)
                {
                    num = span.Start.LineIndex + 1;
                    existing = true;
                    return num;
                }
                string str2 = eventDescriptor.EventType.Name.Replace("EventHandler", "EventArgs");
                string s = searchString + "(sender As Object, e As " + str2 + ")\n\nEnd Sub";
                TextBufferLocation location = buffer.CreateLastCharacterLocation();
                if (location.Line.Length != 0)
                {
                    s = "\n\n" + s;
                }
                buffer.InsertText(location, s);
                num = location.LineIndex - 1;
                location.Dispose();
            }
            return num;
        }

        string ICodeBehindDocumentLanguage.GenerateEventHandlerName(TextBuffer buffer, IComponent component, EventDescriptor eventDescriptor)
        {
            bool flag = false;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (component.Site == null)
            {
                throw new ArgumentException("Invalid component", "component");
            }
            if (eventDescriptor == null)
            {
                throw new ArgumentNullException("eventDescriptor");
            }
            string str = component.Site.Name + "_" + eventDescriptor.Name;
            ArrayList eventHandlers = (ArrayList) ((ICodeBehindDocumentLanguage) this).GetEventHandlers(buffer, eventDescriptor);
            if (eventHandlers.Count == 0)
            {
                return str;
            }
            string item = str;
            int num = 0;
            do
            {
                if (eventHandlers.Contains(item))
                {
                    num++;
                    item = str + "_" + num;
                }
            }
            while (!flag);
            return item;
        }

        ICollection ICodeBehindDocumentLanguage.GetEventHandlers(TextBuffer buffer, EventDescriptor eventDescriptor)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (eventDescriptor == null)
            {
                throw new ArgumentNullException("eventDescriptor");
            }
            ArrayList list = new ArrayList();
            string str = eventDescriptor.EventType.Name.Replace("EventHandler", "EventArgs");
            Regex regex = new Regex(@"Sub (?<handlerMethodName>[a-z_]\w*)\s*\(\s*[a-z_]\w*\s*as\s*object\s*,\s*[a-z_]\w*\s*as\s*" + str + @"\s*\)", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(buffer.Text))
            {
                list.Add(match.Groups["handlerMethodName"].ToString());
            }
            return list;
        }

        public override string DisplayName
        {
            get
            {
                return "Visual Basic .NET";
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.ComponentModel;
    using System.Web;

    public class AspxDocument : WebFormsDocument
    {
        public AspxDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override DocumentDirective CreateDocumentDirective()
        {
            return new PageDirective();
        }

        protected override bool IsValidDocumentDirective(DocumentDirective directive)
        {
            return (directive is PageDirective);
        }

        [Description("The target user agent for which the page should be rendered"), DefaultValue(""), Category("Behavior")]
        public string ClientTarget
        {
            get
            {
                return ((PageDirective) base.DocumentDirective).ClientTarget;
            }
            set
            {
                if (!((PageDirective) base.DocumentDirective).ClientTarget.Equals(value))
                {
                    ((PageDirective) base.DocumentDirective).ClientTarget = value;
                    base.SetDirty();
                }
            }
        }

        [Category("Behavior"), DefaultValue(""), Description("The culture setting of the page")]
        public string Culture
        {
            get
            {
                return ((PageDirective) base.DocumentDirective).Culture;
            }
            set
            {
                if (!((PageDirective) base.DocumentDirective).Culture.Equals(value))
                {
                    ((PageDirective) base.DocumentDirective).Culture = value;
                    base.SetDirty();
                }
            }
        }

        protected override string DocumentDirectiveName
        {
            get
            {
                return "Page";
            }
        }

        [Description("Whether view state should be MAC encryted"), DefaultValue(false), Category("Behavior")]
        public bool EnableViewStateMac
        {
            get
            {
                return ((PageDirective) base.DocumentDirective).EnableViewStateMac;
            }
            set
            {
                if (((PageDirective) base.DocumentDirective).EnableViewStateMac != value)
                {
                    ((PageDirective) base.DocumentDirective).EnableViewStateMac = value;
                    base.SetDirty();
                }
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Whether tracing should be enabled")]
        public bool Trace
        {
            get
            {
                return ((PageDirective) base.DocumentDirective).Trace;
            }
            set
            {
                if (((PageDirective) base.DocumentDirective).Trace != value)
                {
                    ((PageDirective) base.DocumentDirective).Trace = value;
                    base.SetDirty();
                }
            }
        }

        [Category("Behavior"), DefaultValue(2), Description("How the trace messages should be sorted")]
        public System.Web.TraceMode TraceMode
        {
            get
            {
                return ((PageDirective) base.DocumentDirective).TraceMode;
            }
            set
            {
                if (((PageDirective) base.DocumentDirective).TraceMode != value)
                {
                    ((PageDirective) base.DocumentDirective).TraceMode = value;
                    base.SetDirty();
                }
            }
        }
    }
}


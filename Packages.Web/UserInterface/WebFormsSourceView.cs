namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Documents;
    using System;

    public class WebFormsSourceView : HtmlSourceView
    {
        public WebFormsSourceView(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void LoadDocument()
        {
            WebFormsDocument document = base.Document as WebFormsDocument;
            this.Text = document.Html;
        }

        protected override bool SaveToDocument()
        {
            WebFormsDocument document = base.Document as WebFormsDocument;
            document.Html = this.Text;
            return true;
        }

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            Type type = section.GetType();
            if (!base.SupportsToolboxSection(section) && (type != typeof(WebFormsToolboxSection)))
            {
                return (type == typeof(CustomControlsToolboxSection));
            }
            return true;
        }

        public override bool LineNumbersVisible
        {
            get
            {
                return false;
            }
            set
            {
                base.LineNumbersVisible = value;
            }
        }
    }
}


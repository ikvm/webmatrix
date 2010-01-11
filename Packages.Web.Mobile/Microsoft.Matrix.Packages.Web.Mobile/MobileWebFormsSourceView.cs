namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;

    public class MobileWebFormsSourceView : WebFormsSourceView
    {
        public MobileWebFormsSourceView(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            Type type = section.GetType();
            if ((type != typeof(MobileWebFormsToolboxSection)) && (type != typeof(SnippetToolboxSection)))
            {
                return (type == typeof(CustomControlsToolboxSection));
            }
            return true;
        }

        protected override string ViewName
        {
            get
            {
                return "Markup";
            }
        }
    }
}


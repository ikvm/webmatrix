namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;

    public class MobileWebFormsAllView : WebFormsAllView
    {
        public MobileWebFormsAllView(IServiceProvider provider, string viewName) : base(provider, viewName)
        {
        }

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            Type type = section.GetType();
            if (((type != typeof(MobileWebFormsToolboxSection)) && (type != typeof(SnippetToolboxSection))) && (type != typeof(CustomControlsToolboxSection)))
            {
                return (type == typeof(CodeWizardToolboxSection));
            }
            return true;
        }
    }
}


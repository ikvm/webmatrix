namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Core.UserInterface;
    using System;

    public class AspNetSourceView : CodeView
    {
        public AspNetSourceView(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            if (!base.SupportsToolboxSection(section))
            {
                return (section.GetType() == typeof(CodeWizardToolboxSection));
            }
            return true;
        }
    }
}


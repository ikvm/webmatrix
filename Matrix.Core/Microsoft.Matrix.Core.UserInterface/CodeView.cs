namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Toolbox;
    using System;
    using System.Drawing;

    public class CodeView : SourceView
    {
        private static Bitmap viewImage;

        public CodeView(IServiceProvider serviceProvider) : base(serviceProvider)
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

        public override ToolboxSection DefaultToolboxSection
        {
            get
            {
                return CodeWizardToolboxSection.CodeWizards;
            }
        }

        protected override Image ViewImage
        {
            get
            {
                if (viewImage == null)
                {
                    viewImage = new Bitmap(typeof(CodeView), "CodeView.bmp");
                    viewImage.MakeTransparent();
                }
                return viewImage;
            }
        }

        protected override string ViewName
        {
            get
            {
                return "Code";
            }
        }

        protected override DocumentViewType ViewType
        {
            get
            {
                return DocumentViewType.Code;
            }
        }
    }
}


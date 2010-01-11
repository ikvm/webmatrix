namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.IO;

    public class HtmlSourceView : SourceView
    {
        private static Bitmap viewImage;

        public HtmlSourceView(IServiceProvider serviceProvider) : base(serviceProvider)
        {
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

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            if (!base.SupportsToolboxSection(section))
            {
                return (section.GetType() == typeof(HtmlElementToolboxSection));
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

        protected override Image ViewImage
        {
            get
            {
                if (viewImage == null)
                {
                    viewImage = new Bitmap(typeof(HtmlSourceView), "HtmlSourceView.bmp");
                    viewImage.MakeTransparent();
                }
                return viewImage;
            }
        }

        protected override string ViewName
        {
            get
            {
                return "HTML";
            }
        }
    }
}


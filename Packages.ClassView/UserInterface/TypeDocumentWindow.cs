namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Windows.Forms;

    internal sealed class TypeDocumentWindow : DocumentWindow
    {
        public TypeDocumentWindow(IServiceProvider serviceProvider, Document document) : base(serviceProvider, document)
        {
        }

        protected override IDocumentView CreateDocumentView()
        {
            TypeView view = new TypeView(base.ServiceProvider);
            Panel viewContainer = base.ViewContainer;
            viewContainer.DockPadding.All = 1;
            view.Dock = DockStyle.Fill;
            viewContainer.Controls.Add(view);
            return view;
        }

        protected override void DisposeDocumentView()
        {
            ((TypeView) base.DocumentView).Dispose();
        }

        protected override string GetCaption(string displayName)
        {
            return displayName;
        }

        protected override bool UpdateCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 3:
                    case 4:
                        command.Enabled = false;
                        return true;
                }
            }
            return base.UpdateCommand(command);
        }
    }
}


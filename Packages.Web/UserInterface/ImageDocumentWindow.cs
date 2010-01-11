namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ImageDocumentWindow : DocumentWindow
    {
        public ImageDocumentWindow(IServiceProvider serviceProvider, Document document) : base(serviceProvider, document)
        {
        }

        protected override IDocumentView CreateDocumentView()
        {
            ImageView view = this.CreateImageView();
            Panel viewContainer = base.ViewContainer;
            viewContainer.BackColor = SystemColors.ControlDark;
            viewContainer.DockPadding.All = 1;
            view.Dock = DockStyle.Fill;
            view.BorderStyle = BorderStyle.None;
            viewContainer.Controls.Add(view);
            return view;
        }

        protected virtual ImageView CreateImageView()
        {
            return new ImageView(base.ServiceProvider);
        }

        protected override void DisposeDocumentView()
        {
        }
    }
}


namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextDocumentWindow : DocumentWindow
    {
        public TextDocumentWindow(IServiceProvider serviceProvider, Document document) : base(serviceProvider, document)
        {
        }

        protected override IDocumentView CreateDocumentView()
        {
            SourceView view = this.CreateSourceView();
            Panel viewContainer = base.ViewContainer;
            viewContainer.BackColor = SystemColors.ControlDark;
            viewContainer.DockPadding.All = 1;
            view.Dock = DockStyle.Fill;
            viewContainer.Controls.Add(view);
            return view;
        }

        protected virtual SourceView CreateSourceView()
        {
            return new SourceView(base.ServiceProvider);
        }

        protected override void DisposeDocumentView()
        {
            ((SourceView) base.DocumentView).Dispose();
        }
    }
}


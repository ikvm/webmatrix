namespace Microsoft.Matrix.Core.Services
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing.Printing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class PrintService : IPrintService, IDisposable
    {
        private System.Drawing.Printing.PageSettings _pageSettings;
        private PrinterSettings _printerSettings;
        private IServiceProvider _serviceProvider;

        public PrintService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        void IPrintService.ConfigurePrintSettings()
        {
            PageSetupDialog dialog = new PageSetupDialog();
            dialog.PageSettings = this.PageSettings;
            dialog.PrinterSettings = this.PageSettings.PrinterSettings;
            dialog.ShowHelp = false;
            try
            {
                dialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to open print settings.\n\nDetails:\n" + exception.Message, "Print Setttings");
            }
        }

        void IPrintService.PreviewDocument(IPrintableDocument document)
        {
            IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
            PrintDocument document2 = document.CreatePrintDocument();
            document2.DefaultPageSettings = this.PageSettings;
            Microsoft.Matrix.Core.UserInterface.PrintPreviewDialog form = new Microsoft.Matrix.Core.UserInterface.PrintPreviewDialog(this._serviceProvider, document2);
            if (service != null)
            {
                service.ShowDialog(form);
            }
            else
            {
                form.ShowDialog();
            }
        }

        void IPrintService.PrintDocument(IPrintableDocument document)
        {
            PrintDocument document2 = document.CreatePrintDocument();
            document2.DefaultPageSettings = this.PageSettings;
            PrintDialog dialog = new PrintDialog();
            dialog.Document = document2;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                document2.Print();
            }
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }

        private System.Drawing.Printing.PageSettings PageSettings
        {
            get
            {
                if (this._pageSettings == null)
                {
                    this._pageSettings = new System.Drawing.Printing.PageSettings();
                    this._printerSettings = new PrinterSettings();
                    this._pageSettings.PrinterSettings = this._printerSettings;
                }
                return this._pageSettings;
            }
        }
    }
}


namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal sealed class ImportSnippetsWizard : WizardForm
    {
        private ICollection _importedSnippets;
        private ListSnippetsWizardPanel _snippetList;

        public ImportSnippetsWizard(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            base.WizardPanels.Add(new OpenSnippetsWizardPanel(serviceProvider));
            this._snippetList = new ListSnippetsWizardPanel(serviceProvider, "Import Snippets", "Select the snippets to import.");
            base.WizardPanels.Add(this._snippetList);
            this.Text = "Import Snippets";
        }

        protected override void OnCompleted()
        {
            this._importedSnippets = this.SnippetList.CheckedToolboxDataItems;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public ICollection ImportedSnippets
        {
            get
            {
                return this._importedSnippets;
            }
        }

        public ListSnippetsWizardPanel SnippetList
        {
            get
            {
                return this._snippetList;
            }
        }
    }
}


namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using System;

    internal sealed class ExportSnippetsWizard : WizardForm
    {
        private ListSnippetsWizardPanel _snippetList;

        public ExportSnippetsWizard(IServiceProvider serviceProvider, SnippetToolboxSection section) : base(serviceProvider)
        {
            this._snippetList = new ListSnippetsWizardPanel(serviceProvider, "Export Snippets", "Select the snippets to export.");
            foreach (SnippetToolboxDataItem item in section.ToolboxDataItems)
            {
                this._snippetList.AddSnippet(item);
            }
            base.WizardPanels.Add(this._snippetList);
            base.WizardPanels.Add(new SaveSnippetsWizardPanel(serviceProvider));
            this.Text = "Export Snippets";
        }

        protected override void OnCompleted()
        {
            base.Close();
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


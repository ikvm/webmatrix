namespace Microsoft.Matrix.Core.Toolbox
{
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [Serializable]
    public sealed class SnippetToolboxSection : ToolboxSection
    {
        private const int CustomizeExport = 1;
        private const int CustomizeImport = 0;
        public static SnippetToolboxSection Snippets;

        public SnippetToolboxSection() : this("My Snippets")
        {
        }

        public SnippetToolboxSection(string name) : base(name)
        {
            base.Icon = new Icon(typeof(SnippetToolboxSection), "SnippetToolboxSection.ico");
            if (Snippets == null)
            {
                Snippets = this;
            }
        }

        private SnippetToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
            if (Snippets == null)
            {
                Snippets = this;
            }
        }

        public override bool CanCreateToolboxDataItem(IDataObject dataObject)
        {
            return dataObject.GetDataPresent(DataFormats.Text);
        }

        public override ToolboxDataItem CreateToolboxDataItem(string toolboxData)
        {
            return new SnippetToolboxDataItem(toolboxData);
        }

        public override ToolboxDataItem CreateToolboxDataItem(IDataObject dataObject)
        {
            string data = dataObject.GetData(DataFormats.Text) as string;
            if ((data != null) && (data.Length != 0))
            {
                return this.CreateToolboxDataItem(data);
            }
            return null;
        }

        public override bool Customize(int command, IServiceProvider provider)
        {
            switch (command)
            {
                case 0:
                {
                    IUIService service = (IUIService) provider.GetService(typeof(IUIService));
                    ImportSnippetsWizard form = new ImportSnippetsWizard(provider);
                    if (service.ShowDialog(form) == DialogResult.OK)
                    {
                        ICollection importedSnippets = form.ImportedSnippets;
                        if (importedSnippets.Count > 0)
                        {
                            foreach (SnippetToolboxDataItem item in importedSnippets)
                            {
                                base.AddToolboxDataItem(item);
                            }
                        }
                        else if (service != null)
                        {
                            service.ShowMessage("No snippets were imported!");
                        }
                    }
                    return true;
                }
                case 1:
                {
                    ExportSnippetsWizard wizard2 = new ExportSnippetsWizard(provider, this);
                    IUIService service2 = (IUIService) provider.GetService(typeof(IUIService));
                    if (service2 != null)
                    {
                        service2.ShowDialog(wizard2);
                    }
                    return true;
                }
            }
            return false;
        }

        public override string GetCustomizationText(int command)
        {
            switch (command)
            {
                case 0:
                    return "Import Snippets from a file";

                case 1:
                    if (base.ToolboxDataItems.Count > 0)
                    {
                        return "Export Snippets to a file";
                    }
                    return null;
            }
            return null;
        }

        public override bool AllowDrop
        {
            get
            {
                return true;
            }
        }

        public override bool CanRemove
        {
            get
            {
                return true;
            }
        }

        public override bool CanRename
        {
            get
            {
                return true;
            }
        }

        public override bool CanReorder
        {
            get
            {
                return true;
            }
        }

        public override string CustomizationHint
        {
            get
            {
                return "Drag selected text onto the toolbox to add snippets.\n\nRight-click to customize...";
            }
        }

        public override bool IsCustomizable
        {
            get
            {
                return true;
            }
        }
    }
}


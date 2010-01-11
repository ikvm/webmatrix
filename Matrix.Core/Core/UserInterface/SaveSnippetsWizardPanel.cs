namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    internal class SaveSnippetsWizardPanel : WizardPanel
    {
        private MxButton _browseButton;
        private MxTextBox _fileNameTextBox;
        private MxLabel _infoLabel;

        public SaveSnippetsWizardPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private void _browseButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Snippet files (*.snippets)|*.snippets|All files (*.*)|*.*";
            dialog.OverwritePrompt = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._fileNameTextBox.Text = dialog.FileName;
                base.UpdateWizardState();
            }
        }

        private void InitializeComponent()
        {
            this._infoLabel = new MxLabel();
            this._fileNameTextBox = new MxTextBox();
            this._browseButton = new MxButton();
            base.SuspendLayout();
            this._infoLabel.Location = new Point(4, 8);
            this._infoLabel.Name = "_infoLabel";
            this._infoLabel.Size = new Size(0x110, 0x10);
            this._infoLabel.TabIndex = 0;
            this._infoLabel.Text = "Select a file:";
            this._fileNameTextBox.AlwaysShowFocusCues = true;
            this._fileNameTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._fileNameTextBox.FlatAppearance = true;
            this._fileNameTextBox.Location = new Point(4, 0x1c);
            this._fileNameTextBox.Name = "_fileNameTextBox";
            this._fileNameTextBox.Size = new Size(0x150, 20);
            this._fileNameTextBox.TabIndex = 1;
            this._fileNameTextBox.Text = "";
            this._fileNameTextBox.TextChanged += new EventHandler(this.OnFileNameTextBoxTextChanged);
            this._browseButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._browseButton.Location = new Point(0x158, 0x1c);
            this._browseButton.Name = "_browseButton";
            this._browseButton.Size = new Size(0x19, 0x17);
            this._browseButton.TabIndex = 2;
            this._browseButton.Text = "...";
            this._browseButton.Click += new EventHandler(this._browseButton_Click);
            base.Controls.AddRange(new Control[] { this._browseButton, this._fileNameTextBox, this._infoLabel });
            base.Caption = "Export snippets";
            base.Description = "Select a snippets file to export.";
            base.Name = "SaveLibraryPanel";
            base.Size = new Size(0x178, 0x48);
            base.ResumeLayout(false);
        }

        private void OnFileNameTextBoxTextChanged(object sender, EventArgs e)
        {
            base.UpdateWizardState();
        }

        protected internal override bool OnNext()
        {
            string text = this._fileNameTextBox.Text;
            if (text.Length == 0)
            {
                MessageBox.Show(base.WizardForm, "You must specify a filename", "Export Snippets", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            if (text.IndexOfAny(Path.InvalidPathChars) != -1)
            {
                MessageBox.Show(base.WizardForm, "The specified filename contains invalid characters.", "Export Snippets", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            ListSnippetsWizardPanel snippetList = ((ExportSnippetsWizard) base.WizardForm).SnippetList;
            FileStream stream = null;
            bool flag = false;
            try
            {
                if (Path.GetExtension(text).Length == 0)
                {
                    text = text + ".snippets";
                }
                stream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.Write);
                XmlTextWriter writer = new XmlTextWriter(new StreamWriter(stream, Encoding.UTF8));
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                writer.IndentChar = ' ';
                writer.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                writer.WriteStartElement("Snippets");
                foreach (SnippetToolboxDataItem item in snippetList.CheckedToolboxDataItems)
                {
                    writer.WriteStartElement("Snippet");
                    if (item.InternalDisplayName.Length > 0)
                    {
                        writer.WriteAttributeString("name", item.InternalDisplayName);
                    }
                    writer.WriteString(item.ToolboxData);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            }
            catch
            {
                MessageBox.Show(base.WizardForm, "The specified file could not be written.", "Export Snippets", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                flag = true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return !flag;
        }

        public override bool FinishEnabled
        {
            get
            {
                return (this._fileNameTextBox.Text.Trim().Length != 0);
            }
        }
    }
}


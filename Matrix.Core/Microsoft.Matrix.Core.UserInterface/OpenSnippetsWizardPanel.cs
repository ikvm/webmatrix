namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    internal sealed class OpenSnippetsWizardPanel : WizardPanel
    {
        private MxButton _browseButton;
        private string _filename;
        private MxTextBox _fileNameTextBox;
        private MxLabel _infoLabel;

        public OpenSnippetsWizardPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private void _browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Snippet files (*.snippets)|*.snippets|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._fileNameTextBox.Text = dialog.FileName;
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
            base.Caption = "Import snippets";
            base.Description = "Select a snippets file to import.";
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
            if (text.IndexOfAny(Path.InvalidPathChars) != -1)
            {
                MessageBox.Show(base.WizardForm, "The specified filename contains invalid characters.", "Import Snippets", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            if (!File.Exists(text))
            {
                MessageBox.Show(base.WizardForm, "The file '" + text + "' does not exist", "Import Snippets", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            bool flag = false;
            if (string.Compare(this._filename, text, true, CultureInfo.InvariantCulture) != 0)
            {
                ListSnippetsWizardPanel snippetList = ((ImportSnippetsWizard) base.WizardForm).SnippetList;
                snippetList.ClearSnippets();
                FileStream input = null;
                try
                {
                    input = new FileStream(this._fileNameTextBox.Text, FileMode.Open, FileAccess.Read, FileShare.Read);
                    XmlTextReader reader = new XmlTextReader(input);
                    reader.MoveToContent();
                    if (reader.Name == "Snippets")
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "Snippet")
                            {
                                string displayName = string.Empty;
                                if (reader.HasAttributes)
                                {
                                    displayName = reader.GetAttribute("name");
                                }
                                SnippetToolboxDataItem item = new SnippetToolboxDataItem(reader.ReadString(), displayName);
                                snippetList.AddSnippet(item);
                            }
                        }
                        this._filename = text;
                    }
                    else
                    {
                        MessageBox.Show(base.WizardForm, "The specified file is not in a valid format.", "Import Snippets", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        flag = true;
                    }
                }
                catch
                {
                    MessageBox.Show(base.WizardForm, "The specified file could not be read.", "Import Snippets", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    flag = true;
                }
                finally
                {
                    if (input != null)
                    {
                        input.Close();
                    }
                }
            }
            return !flag;
        }

        public override bool NextEnabled
        {
            get
            {
                return (this._fileNameTextBox.Text.Trim().Length != 0);
            }
        }
    }
}


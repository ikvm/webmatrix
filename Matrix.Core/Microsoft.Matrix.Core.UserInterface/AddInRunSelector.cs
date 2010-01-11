namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class AddInRunSelector : MxForm
    {
        private ListBox _addInList;
        private MxButton _cancelButton;
        private MxLabel _captionLabel;
        private Label _descriptionLabel;
        private Panel _descriptionPanel;
        private Panel _listPanel;
        private MxButton _okButton;
        private AddInEntry _selectedEntry;

        public AddInRunSelector(IServiceProvider serviceProvider, IList addIns) : base(serviceProvider)
        {
            this.InitializeComponent();
            if (addIns.Count > 0)
            {
                foreach (AddInEntry entry in addIns)
                {
                    this._addInList.Items.Add(entry);
                }
                this._addInList.SelectedIndex = 0;
            }
            else
            {
                this._okButton.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            this._listPanel = new Panel();
            this._addInList = new ListBox();
            this._captionLabel = new MxLabel();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._descriptionPanel = new Panel();
            this._descriptionLabel = new Label();
            this._listPanel.SuspendLayout();
            this._descriptionPanel.SuspendLayout();
            base.SuspendLayout();
            this._listPanel.BackColor = SystemColors.ControlDark;
            this._listPanel.Controls.Add(this._addInList);
            this._listPanel.DockPadding.All = 1;
            this._listPanel.Location = new Point(8, 0x19);
            this._listPanel.Name = "_listPanel";
            this._listPanel.Size = new Size(0x12a, 0xaf);
            this._listPanel.TabIndex = 1;
            this._addInList.BorderStyle = BorderStyle.None;
            this._addInList.DisplayMember = "Name";
            this._addInList.Dock = DockStyle.Fill;
            this._addInList.IntegralHeight = false;
            this._addInList.Location = new Point(1, 1);
            this._addInList.Name = "_addInList";
            this._addInList.Size = new Size(0x128, 0xad);
            this._addInList.TabIndex = 0;
            this._addInList.DoubleClick += new EventHandler(this.OnAddInListDoubleClicked);
            this._addInList.SelectedIndexChanged += new EventHandler(this.OnAddInListSelectedIndexChanged);
            this._captionLabel.FlatStyle = FlatStyle.System;
            this._captionLabel.Location = new Point(8, 7);
            this._captionLabel.Name = "_captionLabel";
            this._captionLabel.Size = new Size(0xab, 13);
            this._captionLabel.TabIndex = 0;
            this._captionLabel.Text = "Select the add-in you wish to run:";
            this._okButton.FlatStyle = FlatStyle.System;
            this._okButton.Location = new Point(0x97, 0x101);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new Size(0x4b, 0x19);
            this._okButton.TabIndex = 3;
            this._okButton.Text = "&Run";
            this._okButton.Click += new EventHandler(this.OnOKButtonClicked);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.FlatStyle = FlatStyle.System;
            this._cancelButton.Location = new Point(0xe7, 0x101);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new Size(0x4b, 0x19);
            this._cancelButton.TabIndex = 4;
            this._cancelButton.Text = "&Cancel";
            this._descriptionPanel.BackColor = SystemColors.ControlDark;
            this._descriptionPanel.Controls.Add(this._descriptionLabel);
            this._descriptionPanel.DockPadding.All = 1;
            this._descriptionPanel.Location = new Point(8, 0xcc);
            this._descriptionPanel.Name = "_descriptionPanel";
            this._descriptionPanel.Size = new Size(0x12a, 0x2c);
            this._descriptionPanel.TabIndex = 2;
            this._descriptionLabel.BackColor = SystemColors.Control;
            this._descriptionLabel.Dock = DockStyle.Fill;
            this._descriptionLabel.Location = new Point(1, 1);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new Size(0x128, 0x2a);
            this._descriptionLabel.TabIndex = 0;
            this._descriptionLabel.Text = "label1";
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(320, 0x125);
            base.Controls.AddRange(new Control[] { this._descriptionPanel, this._listPanel, this._captionLabel, this._cancelButton, this._okButton });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AddInRunSelector";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Run Add-in";
            base.DoubleClick += new EventHandler(this.OnAddInListDoubleClicked);
            this._listPanel.ResumeLayout(false);
            this._descriptionPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnAddInListDoubleClicked(object sender, EventArgs e)
        {
            this._selectedEntry = (AddInEntry) this._addInList.SelectedItem;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void OnAddInListSelectedIndexChanged(object sender, EventArgs e)
        {
            this._okButton.Enabled = this._addInList.SelectedItem != null;
            string description = string.Empty;
            if (this._addInList.SelectedIndex != -1)
            {
                AddInEntry selectedItem = (AddInEntry) this._addInList.SelectedItem;
                description = selectedItem.Description;
            }
            this._descriptionLabel.Text = description;
        }

        private void OnOKButtonClicked(object sender, EventArgs e)
        {
            this._selectedEntry = this._addInList.SelectedItem as AddInEntry;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public AddInEntry SelectedEntry
        {
            get
            {
                return this._selectedEntry;
            }
        }
    }
}


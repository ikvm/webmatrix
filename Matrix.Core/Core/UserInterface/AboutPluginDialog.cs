namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class AboutPluginDialog : MxForm
    {
        private LinkLabel _authorLabel;
        private GroupBox _descriptionGroup;
        private Label _descriptionLabel;
        private PictureBox _imagePictureBox;
        private Label _nameLabel;
        private AboutDialogButton _okButton;
        private Label _versionLabel;

        public AboutPluginDialog(Plugin plugin, string pluginTypeText, Image pluginTypeImage) : base(plugin.ServiceProvider)
        {
            this.InitializeComponent();
            this.Text = "About " + pluginTypeText;
            this._imagePictureBox.Image = pluginTypeImage;
            PluginAttribute attribute = (PluginAttribute) TypeDescriptor.GetAttributes(plugin.GetType())[typeof(PluginAttribute)];
            if (attribute != null)
            {
                this._nameLabel.Text = attribute.Name;
                this._descriptionLabel.Text = attribute.Description;
                string author = attribute.Author;
                if (author.Length != 0)
                {
                    this._authorLabel.Text = "By " + author;
                    string contactAddress = attribute.ContactAddress;
                    if (contactAddress.Length != 0)
                    {
                        this._authorLabel.LinkArea = new LinkArea(3, author.Length);
                        this._authorLabel.Tag = contactAddress;
                        this._authorLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnAuthorLabelLinkClicked);
                    }
                    else
                    {
                        this._authorLabel.LinkArea = new LinkArea(0, 0);
                    }
                }
            }
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(plugin.GetType().Module.FullyQualifiedName);
            if (versionInfo != null)
            {
                this._versionLabel.Text = string.Format("Version {0}.{1}.{2}.{3}", new object[] { versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, versionInfo.ProductPrivatePart });
            }
        }

        private void InitializeComponent()
        {
            this._okButton = new AboutDialogButton();
            this._imagePictureBox = new PictureBox();
            this._nameLabel = new Label();
            this._versionLabel = new Label();
            this._authorLabel = new LinkLabel();
            this._descriptionGroup = new GroupBox();
            this._descriptionLabel = new Label();
            this._descriptionGroup.SuspendLayout();
            base.SuspendLayout();
            this._okButton.DialogResult = DialogResult.OK;
            this._okButton.Location = new Point(0x114, 0xc4);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 6;
            this._okButton.Text = "OK";
            this._imagePictureBox.Location = new Point(12, 12);
            this._imagePictureBox.Name = "_imagePictureBox";
            this._imagePictureBox.Size = new Size(0x30, 0x30);
            this._imagePictureBox.TabIndex = 1;
            this._imagePictureBox.TabStop = false;
            this._nameLabel.FlatStyle = FlatStyle.System;
            this._nameLabel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this._nameLabel.Location = new Point(0x48, 0x10);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new Size(280, 0x12);
            this._nameLabel.TabIndex = 1;
            this._versionLabel.FlatStyle = FlatStyle.System;
            this._versionLabel.Location = new Point(0x48, 0x24);
            this._versionLabel.Name = "_versionLabel";
            this._versionLabel.Size = new Size(280, 0x10);
            this._versionLabel.TabIndex = 2;
            this._authorLabel.Location = new Point(0x48, 0x34);
            this._authorLabel.Name = "_authorLabel";
            this._authorLabel.Size = new Size(280, 0x10);
            this._authorLabel.TabIndex = 3;
            this._descriptionGroup.Controls.AddRange(new Control[] { this._descriptionLabel });
            this._descriptionGroup.FlatStyle = FlatStyle.System;
            this._descriptionGroup.Location = new Point(0x44, 0x48);
            this._descriptionGroup.Name = "_descriptionGroup";
            this._descriptionGroup.Size = new Size(0x11c, 0x70);
            this._descriptionGroup.TabIndex = 4;
            this._descriptionGroup.TabStop = false;
            this._descriptionGroup.Text = "Description";
            this._descriptionLabel.Location = new Point(6, 0x10);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new Size(0x110, 0x58);
            this._descriptionLabel.TabIndex = 0;
            this._descriptionLabel.Text = "Description";
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.BackColor = Color.White;
            base.CancelButton = this._okButton;
            base.ClientSize = new Size(360, 0xe3);
            base.Controls.AddRange(new Control[] { this._descriptionGroup, this._authorLabel, this._versionLabel, this._nameLabel, this._imagePictureBox, this._okButton });
            this.Font = new Font("Tahoma", 8f);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AboutPluginDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About Plugin";
            this._descriptionGroup.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnAuthorLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string fileName = string.Format("mailto:{0}&subject={1}", this._authorLabel.Tag, this._nameLabel.Text);
            try
            {
                Process.Start(fileName);
            }
            catch
            {
            }
        }
    }
}


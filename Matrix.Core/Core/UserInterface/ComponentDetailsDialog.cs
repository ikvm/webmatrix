namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    internal sealed class ComponentDetailsDialog : MxForm
    {
        private MxLabel _authorLabel;
        private MxLabel _averageRatingLabelLabel;
        private MxButton _cancelButton;
        private Microsoft.Matrix.UIComponents.TabPage _communityTabPage;
        private ComponentDescription _component;
        private MxLabel _dateLabelLabel;
        private MxLabel _dateReleasedLabel;
        private MxLabel _descriptionLabelLabel;
        private MxTextBox _descriptionTextBox;
        private LinkLabel _descriptionUrlLinkLabel;
        private Microsoft.Matrix.UIComponents.TabPage _detailsTabPage;
        private Microsoft.Matrix.UIComponents.TabControl _detailTabControl;
        private MxLabel _discussionUrlLabel;
        private LinkLabel _discussionUrlLinkLabel;
        private MxButton _downloadButton;
        private MxLabel _downloadCountLabel;
        private MxLabel _downloadCountLabelLabel;
        private MxLabel _fullDescriptionUrlLabelLabel;
        private Icon _icon;
        private PictureBox _imagePictureBox;
        private MxLabel _nameLabel;
        private MxLabel _packageContentsLabel;
        private MxTextBox _packageContentsTextBox;
        private bool _previewDownloaded;
        private PictureBox _previewPictureBox;
        private Microsoft.Matrix.UIComponents.TabPage _previewTabPage;
        private MxLabel _ratingLabel;
        private bool _reviewsDownloaded;
        private MxLabel _reviewsLabel;
        private MxTextBox _reviewTextBox;
        private ComponentGalleryServiceProxy _service;
        private MxLabel _sizeLabel;
        private MxLabel _sizeLabelLabel;
        private Microsoft.Matrix.UIComponents.TabPage _summaryTabPage;
        private MxLabel _versionLabel;

        public ComponentDetailsDialog(IServiceProvider provider, ComponentDescription comp, ComponentGalleryServiceProxy service) : base(provider)
        {
            this._component = comp;
            this._service = service;
            this.InitializeComponent();
            if (this._component.Glyph != null)
            {
                this._icon = new Icon(new MemoryStream(this._component.Glyph));
            }
            this._nameLabel.Text = this._component.Name;
            this._authorLabel.Text = "Author : " + this._component.Author;
            this._versionLabel.Text = "Version " + this._component.Version;
            this._descriptionTextBox.Text = this._component.ShortDescription;
            this._dateReleasedLabel.Text = this._component.DateReleased.ToString();
            this._downloadCountLabel.Text = this._component.DownloadCount.ToString();
            this._ratingLabel.Text = string.Format("{0:f1} based on " + this._component.RatingCount + " reviews", this._component.AverageRating);
            this._packageContentsTextBox.Text = this._component.PackageContents;
            this._discussionUrlLinkLabel.Text = this._component.DiscussionForumUrl;
            this._descriptionUrlLinkLabel.Text = this._component.FullDescriptionUrl;
            this._sizeLabel.Text = string.Format("{0:N0} bytes", this._component.Size);
            this._detailTabControl.SelectedIndexChanged += new EventHandler(this.OnDetailTabControlSelectedIndexChanged);
            this._reviewsDownloaded = false;
            this._nameLabel.Font = new Font(this.Font, FontStyle.Bold);
            if (this._icon != null)
            {
                this._imagePictureBox.Image = this._icon.ToBitmap();
            }
        }

        private void _downloadButton_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void InitializeComponent()
        {
            this._imagePictureBox = new PictureBox();
            this._nameLabel = new MxLabel();
            this._authorLabel = new MxLabel();
            this._versionLabel = new MxLabel();
            this._detailTabControl = new Microsoft.Matrix.UIComponents.TabControl();
            this._summaryTabPage = new Microsoft.Matrix.UIComponents.TabPage();
            this._descriptionUrlLinkLabel = new LinkLabel();
            this._ratingLabel = new MxLabel();
            this._downloadCountLabel = new MxLabel();
            this._dateReleasedLabel = new MxLabel();
            this._sizeLabel = new MxLabel();
            this._descriptionTextBox = new MxTextBox();
            this._averageRatingLabelLabel = new MxLabel();
            this._sizeLabelLabel = new MxLabel();
            this._downloadCountLabelLabel = new MxLabel();
            this._dateLabelLabel = new MxLabel();
            this._fullDescriptionUrlLabelLabel = new MxLabel();
            this._descriptionLabelLabel = new MxLabel();
            this._detailsTabPage = new Microsoft.Matrix.UIComponents.TabPage();
            this._packageContentsTextBox = new MxTextBox();
            this._packageContentsLabel = new MxLabel();
            this._communityTabPage = new Microsoft.Matrix.UIComponents.TabPage();
            this._discussionUrlLinkLabel = new LinkLabel();
            this._discussionUrlLabel = new MxLabel();
            this._reviewsLabel = new MxLabel();
            this._reviewTextBox = new MxTextBox();
            this._downloadButton = new MxButton();
            this._cancelButton = new MxButton();
            this._previewTabPage = new Microsoft.Matrix.UIComponents.TabPage();
            this._previewPictureBox = new PictureBox();
            this._detailTabControl.SuspendLayout();
            this._summaryTabPage.SuspendLayout();
            this._detailsTabPage.SuspendLayout();
            this._communityTabPage.SuspendLayout();
            this._previewTabPage.SuspendLayout();
            base.SuspendLayout();
            this._imagePictureBox.Location = new Point(8, 8);
            this._imagePictureBox.Name = "_imagePictureBox";
            this._imagePictureBox.Size = new Size(0x20, 0x20);
            this._imagePictureBox.TabIndex = 0;
            this._imagePictureBox.TabStop = false;
            this._nameLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._nameLabel.Location = new Point(0x38, 8);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new Size(0x130, 0x10);
            this._nameLabel.TabIndex = 0;
            this._authorLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._authorLabel.Location = new Point(0x38, 40);
            this._authorLabel.Name = "_authorLabel";
            this._authorLabel.Size = new Size(0x130, 0x10);
            this._authorLabel.TabIndex = 0;
            this._versionLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._versionLabel.Location = new Point(0x38, 0x18);
            this._versionLabel.Name = "_versionLabel";
            this._versionLabel.Size = new Size(0x130, 0x10);
            this._versionLabel.TabIndex = 0;
            this._detailTabControl.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._detailTabControl.Controls.AddRange(new Control[] { this._summaryTabPage, this._detailsTabPage, this._previewTabPage, this._communityTabPage });
            this._detailTabControl.Location = new Point(4, 0x40);
            this._detailTabControl.Name = "_detailTabControl";
            this._detailTabControl.SelectedIndex = 0;
            this._detailTabControl.Size = new Size(0x170, 0x144);
            this._detailTabControl.TabIndex = 1;
            this._detailTabControl.Mode = TabControlMode.TextOnly;
            this._summaryTabPage.Controls.AddRange(new Control[] { this._descriptionUrlLinkLabel, this._ratingLabel, this._downloadCountLabel, this._dateReleasedLabel, this._sizeLabel, this._descriptionTextBox, this._averageRatingLabelLabel, this._sizeLabelLabel, this._downloadCountLabelLabel, this._dateLabelLabel, this._fullDescriptionUrlLabelLabel, this._descriptionLabelLabel });
            this._summaryTabPage.Dock = DockStyle.Fill;
            this._summaryTabPage.Location = new Point(4, 0x16);
            this._summaryTabPage.Name = "_summaryTabPage";
            this._summaryTabPage.Size = new Size(360, 0x12a);
            this._summaryTabPage.TabIndex = 0;
            this._summaryTabPage.TabStop = true;
            this._summaryTabPage.Text = "Summary";
            this._descriptionUrlLinkLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._descriptionUrlLinkLabel.Location = new Point(8, 0xf8);
            this._descriptionUrlLinkLabel.Name = "_descriptionUrlLinkLabel";
            this._descriptionUrlLinkLabel.Size = new Size(0x158, 0x2c);
            this._descriptionUrlLinkLabel.TabIndex = 2;
            this._descriptionUrlLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.UrlLinkLabel_LinkClicked);
            this._ratingLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._ratingLabel.Location = new Point(100, 0xd0);
            this._ratingLabel.Name = "_ratingLabel";
            this._ratingLabel.Size = new Size(0xfc, 0x10);
            this._ratingLabel.TabIndex = 0;
            this._downloadCountLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._downloadCountLabel.Location = new Point(100, 0xbc);
            this._downloadCountLabel.Name = "_downloadCountLabel";
            this._downloadCountLabel.Size = new Size(0xfc, 0x10);
            this._downloadCountLabel.TabIndex = 0;
            this._dateReleasedLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._dateReleasedLabel.Location = new Point(100, 0xa8);
            this._dateReleasedLabel.Name = "_dateReleasedLabel";
            this._dateReleasedLabel.Size = new Size(0xfc, 0x10);
            this._dateReleasedLabel.TabIndex = 0;
            this._sizeLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._sizeLabel.Location = new Point(100, 0x94);
            this._sizeLabel.Name = "_sizeLabel";
            this._sizeLabel.Size = new Size(0xfc, 0x10);
            this._sizeLabel.TabIndex = 0;
            this._descriptionTextBox.AlwaysShowFocusCues = true;
            this._descriptionTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._descriptionTextBox.BorderStyle = BorderStyle.None;
            this._descriptionTextBox.FlatAppearance = true;
            this._descriptionTextBox.Location = new Point(8, 0x20);
            this._descriptionTextBox.Multiline = true;
            this._descriptionTextBox.Name = "_descriptionTextBox";
            this._descriptionTextBox.ReadOnly = true;
            this._descriptionTextBox.ScrollBars = ScrollBars.Vertical;
            this._descriptionTextBox.Size = new Size(0x158, 0x68);
            this._descriptionTextBox.TabIndex = 1;
            this._descriptionTextBox.Text = "";
            this._descriptionTextBox.WordWrap = true;
            this._averageRatingLabelLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._averageRatingLabelLabel.Location = new Point(8, 0xd0);
            this._averageRatingLabelLabel.Name = "_averageRatingLabelLabel";
            this._averageRatingLabelLabel.Size = new Size(0x58, 0x10);
            this._averageRatingLabelLabel.TabIndex = 0;
            this._averageRatingLabelLabel.Text = "Rating:";
            this._sizeLabelLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._sizeLabelLabel.Location = new Point(8, 0x94);
            this._sizeLabelLabel.Name = "_sizeLabelLabel";
            this._sizeLabelLabel.Size = new Size(0x58, 0x10);
            this._sizeLabelLabel.TabIndex = 0;
            this._sizeLabelLabel.Text = "Size:";
            this._downloadCountLabelLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._downloadCountLabelLabel.Location = new Point(8, 0xbc);
            this._downloadCountLabelLabel.Name = "_downloadCountLabelLabel";
            this._downloadCountLabelLabel.Size = new Size(0x58, 0x10);
            this._downloadCountLabelLabel.TabIndex = 0;
            this._downloadCountLabelLabel.Text = "Downloads:";
            this._dateLabelLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._dateLabelLabel.Location = new Point(8, 0xa8);
            this._dateLabelLabel.Name = "_dateLabelLabel";
            this._dateLabelLabel.Size = new Size(0x58, 0x10);
            this._dateLabelLabel.TabIndex = 0;
            this._dateLabelLabel.Text = "Date Submitted:";
            this._fullDescriptionUrlLabelLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._fullDescriptionUrlLabelLabel.Location = new Point(8, 0xe4);
            this._fullDescriptionUrlLabelLabel.Name = "_fullDescriptionUrlLabelLabel";
            this._fullDescriptionUrlLabelLabel.Size = new Size(40, 0x10);
            this._fullDescriptionUrlLabelLabel.TabIndex = 0;
            this._fullDescriptionUrlLabelLabel.Text = "URL:";
            this._descriptionLabelLabel.Location = new Point(8, 12);
            this._descriptionLabelLabel.Name = "_descriptionLabelLabel";
            this._descriptionLabelLabel.Size = new Size(0x44, 0x10);
            this._descriptionLabelLabel.TabIndex = 0;
            this._descriptionLabelLabel.Text = "Description:";
            this._detailsTabPage.Controls.AddRange(new Control[] { this._packageContentsTextBox, this._packageContentsLabel });
            this._detailsTabPage.Dock = DockStyle.Fill;
            this._detailsTabPage.Location = new Point(4, 0x16);
            this._detailsTabPage.Name = "_detailsTabPage";
            this._detailsTabPage.Size = new Size(360, 0x12a);
            this._detailsTabPage.TabIndex = 1;
            this._detailsTabPage.TabStop = true;
            this._detailsTabPage.Text = "Details";
            this._packageContentsTextBox.AlwaysShowFocusCues = true;
            this._packageContentsTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._packageContentsTextBox.BorderStyle = BorderStyle.None;
            this._packageContentsTextBox.FlatAppearance = true;
            this._packageContentsTextBox.Location = new Point(8, 0x20);
            this._packageContentsTextBox.Multiline = true;
            this._packageContentsTextBox.Name = "_packageContentsTextBox";
            this._packageContentsTextBox.ReadOnly = true;
            this._packageContentsTextBox.ScrollBars = ScrollBars.Vertical;
            this._packageContentsTextBox.Size = new Size(0x158, 0xf8);
            this._packageContentsTextBox.TabIndex = 1;
            this._packageContentsTextBox.Text = "";
            this._packageContentsLabel.Location = new Point(8, 12);
            this._packageContentsLabel.Name = "_packageContentsLabel";
            this._packageContentsLabel.Size = new Size(0x68, 0x10);
            this._packageContentsLabel.TabIndex = 0;
            this._packageContentsLabel.Text = "Package Contents:";
            this._communityTabPage.Controls.AddRange(new Control[] { this._discussionUrlLinkLabel, this._discussionUrlLabel, this._reviewsLabel, this._reviewTextBox });
            this._communityTabPage.Dock = DockStyle.Fill;
            this._communityTabPage.Location = new Point(4, 0x16);
            this._communityTabPage.Name = "_communityTabPage";
            this._communityTabPage.Size = new Size(360, 0x12a);
            this._communityTabPage.TabIndex = 1;
            this._communityTabPage.TabStop = true;
            this._communityTabPage.Text = "Community";
            this._discussionUrlLinkLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._discussionUrlLinkLabel.Location = new Point(8, 0xf8);
            this._discussionUrlLinkLabel.Name = "_discussionUrlLinkLabel";
            this._discussionUrlLinkLabel.Size = new Size(0x158, 40);
            this._discussionUrlLinkLabel.TabIndex = 2;
            this._discussionUrlLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.UrlLinkLabel_LinkClicked);
            this._discussionUrlLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._discussionUrlLabel.Location = new Point(8, 0xe4);
            this._discussionUrlLabel.Name = "_discussionUrlLabel";
            this._discussionUrlLabel.Size = new Size(0x60, 0x10);
            this._discussionUrlLabel.TabIndex = 0;
            this._discussionUrlLabel.Text = "Dicussion Forum:";
            this._reviewsLabel.Location = new Point(8, 12);
            this._reviewsLabel.Name = "_reviewsLabel";
            this._reviewsLabel.Size = new Size(80, 0x10);
            this._reviewsLabel.TabIndex = 0;
            this._reviewsLabel.Text = "User Reviews:";
            this._reviewTextBox.AlwaysShowFocusCues = true;
            this._reviewTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._reviewTextBox.BorderStyle = BorderStyle.None;
            this._reviewTextBox.FlatAppearance = true;
            this._reviewTextBox.Location = new Point(8, 0x20);
            this._reviewTextBox.Multiline = true;
            this._reviewTextBox.Name = "_reviewTextBox";
            this._reviewTextBox.ReadOnly = true;
            this._reviewTextBox.ScrollBars = ScrollBars.Vertical;
            this._reviewTextBox.Size = new Size(0x158, 0xb8);
            this._reviewTextBox.TabIndex = 1;
            this._reviewTextBox.Text = "";
            this._downloadButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._downloadButton.Location = new Point(0xd0, 0x18c);
            this._downloadButton.Name = "_downloadButton";
            this._downloadButton.TabIndex = 2;
            this._downloadButton.Text = "Install";
            this._downloadButton.Click += new EventHandler(this._downloadButton_Click);
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x124, 0x18c);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Close";
            this._previewTabPage.Controls.AddRange(new Control[] { this._previewPictureBox });
            this._previewTabPage.Location = new Point(4, 0x16);
            this._previewTabPage.Name = "_previewTabPage";
            this._previewTabPage.Size = new Size(360, 0x12a);
            this._previewTabPage.TabIndex = 2;
            this._previewTabPage.Text = "Preview";
            this._previewPictureBox.Dock = DockStyle.Fill;
            this._previewPictureBox.Name = "_previewPictureBox";
            this._previewPictureBox.Size = new Size(360, 0x12a);
            this._previewPictureBox.TabIndex = 0;
            this._previewPictureBox.TabStop = false;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x178, 430);
            base.Controls.AddRange(new Control[] { this._cancelButton, this._downloadButton, this._detailTabControl, this._versionLabel, this._authorLabel, this._nameLabel, this._imagePictureBox });
            base.Icon = new Icon(typeof(ComponentDetailsDialog), "ComponentGallery.ico");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ComponentDetailsDialog";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Component Details";
            this._detailTabControl.ResumeLayout(false);
            this._summaryTabPage.ResumeLayout(false);
            this._detailsTabPage.ResumeLayout(false);
            this._communityTabPage.ResumeLayout(false);
            this._previewTabPage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnDetailTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            string text = string.Empty;
            if ((this._detailTabControl.SelectedIndex == 2) && !this._previewDownloaded)
            {
                try
                {
                    this._previewDownloaded = true;
                    byte[] preview = this._service.GetPreview(this._component.Id);
                    if ((preview != null) && (preview.Length > 0))
                    {
                        Image image = Image.FromStream(new MemoryStream(preview));
                        this._previewPictureBox.Image = image;
                    }
                }
                catch
                {
                    text = "Couldn't download the preview from the component gallery service.";
                }
            }
            else if ((this._detailTabControl.SelectedIndex == 3) && !this._reviewsDownloaded)
            {
                try
                {
                    this._reviewsDownloaded = true;
                    Review[] reviewArray = this._service.GetReviews(this._component.Id, 0, 10);
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < reviewArray.Length; i++)
                    {
                        builder.Append(reviewArray[i].Title);
                        builder.Append(" by ");
                        builder.Append(reviewArray[i].Email);
                        builder.Append(" [");
                        builder.Append(reviewArray[i].Date.ToString());
                        builder.Append("]");
                        builder.Append(Environment.NewLine);
                        builder.Append("Rating : ");
                        builder.Append(reviewArray[i].Rating);
                        builder.Append(Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        builder.Append(reviewArray[i].Contents);
                        builder.Append(Environment.NewLine);
                        builder.Append(Environment.NewLine);
                    }
                    this._reviewTextBox.Text = builder.ToString();
                }
                catch
                {
                    text = "Couldn't download the reviews from the component gallery service.";
                }
            }
            if (text.Length > 0)
            {
                MessageBox.Show(this, text, "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void UrlLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender.ToString().Length > 0)
            {
                Process.Start(((LinkLabel) sender).Text);
            }
        }
    }
}


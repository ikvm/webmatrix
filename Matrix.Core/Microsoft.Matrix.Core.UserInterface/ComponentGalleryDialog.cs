namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public sealed class ComponentGalleryDialog : TaskForm
    {
        private bool _aborted;
        private MxButton _cancelButton;
        private ColumnHeader _categoryHeader;
        private MxListView _categoryListView;
        private Panel _categoryPanel;
        private MxRadioButton _categoryRadioButton;
        private MxButton _connectButton;
        private bool _connecting;
        private MruList _connectMruList;
        private Panel _containerPanel;
        private MxButton _detailsButton;
        private MxButton _downloadButton;
        private bool _downloading;
        private bool _downloadingDetails;
        private DownloadAsyncTask _downloadTask;
        private MxComboBox _galleryComboBox;
        private MxLabel _galleryLabel;
        private GradientBand _gradientBand;
        private MxComboBox _keywordComboBox;
        private MxRadioButton _keywordRadioButton;
        private Library _library;
        private MxLabel _lookForLabel;
        private ColumnHeader _nameHeader;
        private ComponentGalleryServiceProxy _newService;
        private int _oldResultsWidth;
        private Splitter _panelSplitter;
        private ColumnHeader _ratingHeader;
        private MxListView _resultsListView;
        private Panel _resultsListViewPanel;
        private Panel _resultsPanel;
        private Splitter _resultsSplitter;
        private MxLabel _resultsSummaryLabel;
        private Panel _resultsSummaryPanel;
        private MxButton _searchButton;
        private MxLabel _searchByLabel;
        private GroupLabel _searchGroupLabel;
        private bool _searching;
        private ComponentGalleryServiceProxy _service;
        private string _typeFilter;
        private ColumnHeader _versionHeader;
        private ColumnHeader Categories;

        public ComponentGalleryDialog(IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
            this._searchButton.Click += new EventHandler(this.OnSearchButtonClick);
            this._connectButton.Click += new EventHandler(this.OnConnectButtonClick);
            this._nameHeader = new ColumnHeader();
            this._nameHeader.Text = "Name";
            this._resultsListView.Columns.Add(this._nameHeader);
            this._versionHeader = new ColumnHeader();
            this._versionHeader.Text = "Version";
            this._resultsListView.Columns.Add(this._versionHeader);
            this._ratingHeader = new ColumnHeader();
            this._ratingHeader.Text = "Rating";
            this._resultsListView.Columns.Add(this._ratingHeader);
            this._resultsListView.View = View.Details;
            this._resultsListView.MultiSelect = false;
            this._categoryHeader = new ColumnHeader();
            this._categoryHeader.Text = "Categories";
            this._categoryListView.Columns.Add(this._categoryHeader);
            this._categoryListView.View = View.Details;
            this._categoryListView.MultiSelect = false;
            this.UpdateListViewHeaders();
            this._categoryListView.Enabled = false;
            this._searchButton.Enabled = false;
            this._keywordComboBox.Enabled = false;
            this._downloadButton.Enabled = false;
            this._detailsButton.Enabled = false;
            string str = ConfigurationSettings.AppSettings["IDE.ComponentGalleryServiceUrl"];
            if (str == null)
            {
                str = string.Empty;
            }
            this._galleryComboBox.Items.AddRange(this.ConnectMruList.Save());
            if (this._galleryComboBox.Items.Count > 0)
            {
                this._galleryComboBox.SelectedIndex = 0;
            }
            else
            {
                this._galleryComboBox.Text = str;
            }
            this._galleryComboBox.Enabled = false;
        }

        private void _panelSplitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.UpdateListViewHeaders();
        }

        private void BeginDownloadLibrary(string url)
        {
            this._downloadTask = new DownloadAsyncTask(this, url);
            this.Downloading = true;
            this._downloadTask.Start(new AsyncTaskResultPostedEventHandler(this.OnDownloadLibraryComplete));
        }

        private void ConnectToGallery()
        {
            string url = this._galleryComboBox.Text.Trim();
            if ((this._service == null) || ((this._service.Url != url) && (url.Length > 0)))
            {
                this.Connecting = true;
                try
                {
                    this._newService = new ComponentGalleryServiceProxy(url);
                }
                catch (UriFormatException)
                {
                    MessageBox.Show(this, "Invalid URL.  Please check your URL and try again.", "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                this._categoryListView.Items.Clear();
                this._newService.BeginGetCategories(this.TypeFilter, new AsyncCallback(this.OnGetCategoriesDone), this._service);
            }
        }

        private void InitializeComponent()
        {
            this._galleryLabel = new MxLabel();
            this._lookForLabel = new MxLabel();
            this._searchByLabel = new MxLabel();
            this._searchGroupLabel = new GroupLabel();
            this._resultsListView = new MxListView();
            this._searchButton = new MxButton();
            this._connectButton = new MxButton();
            this._galleryComboBox = new MxComboBox();
            this._keywordComboBox = new MxComboBox();
            this._gradientBand = new GradientBand();
            this._downloadButton = new MxButton();
            this._cancelButton = new MxButton();
            this._containerPanel = new Panel();
            this._resultsPanel = new Panel();
            this._resultsListViewPanel = new Panel();
            this._resultsSplitter = new Splitter();
            this._resultsSummaryPanel = new Panel();
            this._resultsSummaryLabel = new MxLabel();
            this._panelSplitter = new Splitter();
            this._categoryPanel = new Panel();
            this._categoryListView = new MxListView();
            this.Categories = new ColumnHeader();
            this._categoryRadioButton = new MxRadioButton();
            this._keywordRadioButton = new MxRadioButton();
            this._detailsButton = new MxButton();
            this._containerPanel.SuspendLayout();
            this._resultsPanel.SuspendLayout();
            this._resultsListViewPanel.SuspendLayout();
            this._resultsSummaryPanel.SuspendLayout();
            this._categoryPanel.SuspendLayout();
            base.SuspendLayout();
            this._galleryLabel.Location = new Point(0x10, 80);
            this._galleryLabel.Name = "_galleryLabel";
            this._galleryLabel.Size = new Size(0x30, 0x10);
            this._galleryLabel.TabIndex = 0;
            this._galleryLabel.Text = "&Gallery:";
            this._lookForLabel.Location = new Point(0x10, 160);
            this._lookForLabel.Name = "_lookForLabel";
            this._lookForLabel.Size = new Size(0x40, 0x10);
            this._lookForLabel.TabIndex = 7;
            this._lookForLabel.Text = "&Look For:";
            this._lookForLabel.Visible = false;
            this._searchByLabel.Location = new Point(0x10, 0x84);
            this._searchByLabel.Name = "_searchByLabel";
            this._searchByLabel.Size = new Size(0x48, 0x10);
            this._searchByLabel.TabIndex = 4;
            this._searchByLabel.Text = "Search By:";
            this._searchGroupLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._searchGroupLabel.Location = new Point(4, 0x6c);
            this._searchGroupLabel.Name = "_searchGroupLabel";
            this._searchGroupLabel.Size = new Size(0x204, 0x10);
            this._searchGroupLabel.TabIndex = 3;
            this._searchGroupLabel.Text = "Search";
            this._resultsListView.BorderStyle = BorderStyle.None;
            this._resultsListView.Dock = DockStyle.Fill;
            this._resultsListView.FlatScrollBars = false;
            this._resultsListView.FullRowSelect = true;
            this._resultsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this._resultsListView.HideSelection = false;
            this._resultsListView.Location = new Point(1, 1);
            this._resultsListView.MultiSelect = false;
            this._resultsListView.Name = "_resultsListView";
            this._resultsListView.ShowToolTips = false;
            this._resultsListView.Size = new Size(0x185, 0x80);
            this._resultsListView.TabIndex = 1;
            this._resultsListView.WatermarkText = "No components found.\r\nSelect a category to begin browsing.";
            this._resultsListView.SelectedIndexChanged += new EventHandler(this.OnResultsListViewSelectedIndexChanged);
            this._searchButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._searchButton.Location = new Point(440, 0x9d);
            this._searchButton.Name = "_searchButton";
            this._searchButton.TabIndex = 6;
            this._searchButton.Text = "&Search";
            this._searchButton.Visible = false;
            this._connectButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._connectButton.Location = new Point(440, 0x4d);
            this._connectButton.Name = "_connectButton";
            this._connectButton.TabIndex = 2;
            this._connectButton.Text = "&Connect";
            this._galleryComboBox.AlwaysShowFocusCues = true;
            this._galleryComboBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._galleryComboBox.FlatAppearance = true;
            this._galleryComboBox.InitialText = null;
            this._galleryComboBox.Location = new Point(0x60, 0x4e);
            this._galleryComboBox.Name = "_galleryComboBox";
            this._galleryComboBox.Size = new Size(0x150, 0x15);
            this._galleryComboBox.TabIndex = 1;
            this._galleryComboBox.KeyDown += new KeyEventHandler(this.OnGalleryComboBoxKeyDown);
            this._keywordComboBox.AlwaysShowFocusCues = true;
            this._keywordComboBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._keywordComboBox.FlatAppearance = true;
            this._keywordComboBox.InitialText = null;
            this._keywordComboBox.Location = new Point(0x60, 0x9e);
            this._keywordComboBox.Name = "_keywordComboBox";
            this._keywordComboBox.Size = new Size(0x150, 0x15);
            this._keywordComboBox.TabIndex = 5;
            this._keywordComboBox.KeyDown += new KeyEventHandler(this.OnKeywordComboBoxKeyDown);
            this._gradientBand.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._gradientBand.EndColor = Color.FromArgb(0x80, 0x80, 0xff);
            this._gradientBand.GradientSpeed = 1;
            this._gradientBand.Location = new Point(0, 0x38);
            this._gradientBand.Name = "_gradientBand";
            this._gradientBand.ScrollSpeed = 5;
            this._gradientBand.Size = new Size(0x20c, 6);
            this._gradientBand.StartColor = Color.White;
            this._gradientBand.TabIndex = 0;
            this._gradientBand.Text = "gradientBand1";
            this._downloadButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._downloadButton.Location = new Point(0x11c, 0x160);
            this._downloadButton.Name = "_downloadButton";
            this._downloadButton.TabIndex = 9;
            this._downloadButton.Text = "&Install";
            this._downloadButton.Click += new EventHandler(this.OnDownloadButtonClick);
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.Location = new Point(0x1bc, 0x160);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 11;
            this._cancelButton.Text = "Cl&ose";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._containerPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._containerPanel.BackColor = SystemColors.ControlDark;
            this._containerPanel.Controls.AddRange(new Control[] { this._resultsPanel, this._panelSplitter, this._categoryPanel });
            this._containerPanel.Location = new Point(4, 0x9c);
            this._containerPanel.Name = "_containerPanel";
            this._containerPanel.Size = new Size(0x204, 0xb8);
            this._containerPanel.TabIndex = 7;
            this._resultsPanel.Controls.AddRange(new Control[] { this._resultsListViewPanel, this._resultsSplitter, this._resultsSummaryPanel });
            this._resultsPanel.Dock = DockStyle.Fill;
            this._resultsPanel.Location = new Point(0x7d, 0);
            this._resultsPanel.Name = "_resultsPanel";
            this._resultsPanel.Size = new Size(0x187, 0xb8);
            this._resultsPanel.TabIndex = 5;
            this._resultsListViewPanel.BackColor = SystemColors.ControlDark;
            this._resultsListViewPanel.Controls.AddRange(new Control[] { this._resultsListView });
            this._resultsListViewPanel.Dock = DockStyle.Fill;
            this._resultsListViewPanel.DockPadding.All = 1;
            this._resultsListViewPanel.Name = "_resultsListViewPanel";
            this._resultsListViewPanel.Size = new Size(0x187, 130);
            this._resultsListViewPanel.TabIndex = 0;
            this._resultsSplitter.BackColor = SystemColors.Control;
            this._resultsSplitter.Dock = DockStyle.Bottom;
            this._resultsSplitter.Location = new Point(0, 130);
            this._resultsSplitter.Name = "_resultsSplitter";
            this._resultsSplitter.Size = new Size(0x187, 4);
            this._resultsSplitter.TabIndex = 2;
            this._resultsSplitter.TabStop = false;
            this._resultsSummaryPanel.BackColor = SystemColors.ControlDark;
            this._resultsSummaryPanel.Controls.AddRange(new Control[] { this._resultsSummaryLabel });
            this._resultsSummaryPanel.Dock = DockStyle.Bottom;
            this._resultsSummaryPanel.DockPadding.All = 1;
            this._resultsSummaryPanel.Location = new Point(0, 0x86);
            this._resultsSummaryPanel.Name = "_resultsSummaryPanel";
            this._resultsSummaryPanel.Size = new Size(0x187, 50);
            this._resultsSummaryPanel.TabIndex = 3;
            this._resultsSummaryLabel.BackColor = SystemColors.Control;
            this._resultsSummaryLabel.Dock = DockStyle.Fill;
            this._resultsSummaryLabel.Location = new Point(1, 1);
            this._resultsSummaryLabel.Name = "_resultsSummaryLabel";
            this._resultsSummaryLabel.Size = new Size(0x185, 0x30);
            this._resultsSummaryLabel.TabIndex = 0;
            this._panelSplitter.BackColor = SystemColors.Control;
            this._panelSplitter.Location = new Point(0x79, 0);
            this._panelSplitter.Name = "_panelSplitter";
            this._panelSplitter.Size = new Size(4, 0xb8);
            this._panelSplitter.TabIndex = 3;
            this._panelSplitter.TabStop = false;
            this._panelSplitter.SplitterMoved += new SplitterEventHandler(this._panelSplitter_SplitterMoved);
            this._categoryPanel.BackColor = SystemColors.ControlDark;
            this._categoryPanel.Controls.AddRange(new Control[] { this._categoryListView });
            this._categoryPanel.Dock = DockStyle.Left;
            this._categoryPanel.DockPadding.All = 1;
            this._categoryPanel.Name = "_categoryPanel";
            this._categoryPanel.Size = new Size(0x79, 0xb8);
            this._categoryPanel.TabIndex = 4;
            this._categoryListView.BorderStyle = BorderStyle.None;
            this._categoryListView.Dock = DockStyle.Fill;
            this._categoryListView.FlatScrollBars = false;
            this._categoryListView.FullRowSelect = true;
            this._categoryListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this._categoryListView.HideSelection = false;
            this._categoryListView.Location = new Point(1, 1);
            this._categoryListView.MultiSelect = false;
            this._categoryListView.Name = "_categoryListView";
            this._categoryListView.ShowToolTips = false;
            this._categoryListView.Size = new Size(0x77, 0xb6);
            this._categoryListView.TabIndex = 1;
            this._categoryListView.WatermarkText = "";
            this._categoryListView.SelectedIndexChanged += new EventHandler(this.OnCategoryListViewSelectedIndexChanged);
            this._categoryRadioButton.Checked = true;
            this._categoryRadioButton.Location = new Point(100, 0x84);
            this._categoryRadioButton.Name = "_categoryRadioButton";
            this._categoryRadioButton.Size = new Size(0x44, 0x10);
            this._categoryRadioButton.TabIndex = 3;
            this._categoryRadioButton.TabStop = true;
            this._categoryRadioButton.Text = "C&ategory";
            this._categoryRadioButton.CheckedChanged += new EventHandler(this.OnCategoryRadioButtonCheckedChanged);
            this._keywordRadioButton.Location = new Point(0xb8, 0x84);
            this._keywordRadioButton.Name = "_keywordRadioButton";
            this._keywordRadioButton.Size = new Size(0x44, 0x10);
            this._keywordRadioButton.TabIndex = 4;
            this._keywordRadioButton.TabStop = true;
            this._keywordRadioButton.Text = "&Keyword";
            this._detailsButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._detailsButton.Location = new Point(0x16c, 0x160);
            this._detailsButton.Name = "_detailsButton";
            this._detailsButton.TabIndex = 10;
            this._detailsButton.Text = "&Details";
            this._detailsButton.Click += new EventHandler(this.OnDetailsButtonClick);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x20c, 0x181);
            base.Controls.AddRange(new Control[] { this._detailsButton, this._keywordRadioButton, this._categoryRadioButton, this._cancelButton, this._downloadButton, this._gradientBand, this._galleryComboBox, this._connectButton, this._searchGroupLabel, this._searchByLabel, this._galleryLabel, this._containerPanel, this._searchButton, this._keywordComboBox, this._lookForLabel });
            base.Icon = new Icon(typeof(ComponentGalleryDialog), "ComponentGallery.ico");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.MinimumSize = new Size(0x214, 0x19c);
            base.Name = "ComponentGalleryDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            base.TaskCaption = "Online Component Gallery";
            base.TaskDescription = "Search the online component gallery";
            base.TaskGlyph = new Bitmap(typeof(ComponentGalleryDialog), "ComponentGalleryGlyph.bmp");
            this.Text = "Component Gallery";
            this._containerPanel.ResumeLayout(false);
            this._resultsPanel.ResumeLayout(false);
            this._resultsListViewPanel.ResumeLayout(false);
            this._resultsSummaryPanel.ResumeLayout(false);
            this._categoryPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            if (this.Connecting)
            {
                this._aborted = true;
                this._newService.Abort();
                this._newService = null;
                this.Connecting = false;
            }
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void OnCategoryListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            this._resultsListView.SelectedItems.Clear();
            if (this._categoryListView.SelectedItems.Count > 0)
            {
                CategoryInfo info = ((CategoryInfoListViewItem) this._categoryListView.SelectedItems[0]).info;
                this.Searching = true;
                this._service.BeginGetComponents(info.Id, this.TypeFilter, new AsyncCallback(this.OnGetComponentsDone), this._service);
            }
        }

        private void OnCategoryRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            this._resultsListView.Items.Clear();
            this._detailsButton.Enabled = false;
            this._downloadButton.Enabled = false;
            if (this._categoryRadioButton.Checked)
            {
                this._lookForLabel.Visible = false;
                this._keywordComboBox.Visible = false;
                this._searchButton.Visible = false;
                this._containerPanel.Location = new Point(this._containerPanel.Location.X, this._containerPanel.Location.Y - 0x20);
                this._containerPanel.Size = new Size(this._containerPanel.Size.Width, this._containerPanel.Size.Height + 0x20);
                this._categoryPanel.Visible = true;
                this._resultsListView.WatermarkText = "No components found.\r\nSelect a category to begin browsing.";
                this._resultsSummaryLabel.Text = string.Empty;
                this._panelSplitter.Enabled = true;
                this._panelSplitter.Visible = true;
            }
            else
            {
                this._containerPanel.Location = new Point(this._containerPanel.Location.X, this._containerPanel.Location.Y + 0x20);
                this._containerPanel.Size = new Size(this._containerPanel.Size.Width, this._containerPanel.Size.Height - 0x20);
                this._categoryPanel.Visible = false;
                this._lookForLabel.Visible = true;
                this._keywordComboBox.Visible = true;
                this._searchButton.Visible = true;
                this._categoryListView.SelectedItems.Clear();
                this._resultsListView.WatermarkText = "No components found.\r\nEnter a keyword and press the 'Search' button to begin searching.";
                this._resultsSummaryLabel.Text = string.Empty;
                this._panelSplitter.Enabled = false;
                this._panelSplitter.Visible = false;
            }
            this.UpdateListViewHeaders();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                IPreferencesService service = (IPreferencesService) base.ServiceProvider.GetService(typeof(IPreferencesService));
                if (service != null)
                {
                    PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(ComponentGalleryDialog));
                    if (this._connectMruList != null)
                    {
                        preferencesStore.SetValue("ConnectMruCount", this._connectMruList.Count, 0);
                        if (this._connectMruList.Count != 0)
                        {
                            string[] strArray = this._connectMruList.Save();
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                preferencesStore.SetValue("ConnectMru" + i, strArray[i], string.Empty);
                            }
                        }
                    }
                }
            }
        }

        private void OnConnectButtonClick(object sender, EventArgs e)
        {
            if (this.Connecting)
            {
                this._aborted = true;
                this._newService.Abort();
                this._newService = null;
                this.Connecting = false;
            }
            else
            {
                this.ConnectToGallery();
            }
        }

        private void OnDetailsButtonClick(object sender, EventArgs e)
        {
            if (this.DownloadingDetails)
            {
                this._aborted = true;
                this._service.Abort();
                this.DownloadingDetails = false;
            }
            else
            {
                ComponentInfoListViewItem item = (ComponentInfoListViewItem) this._resultsListView.SelectedItems[0];
                ComponentInfo info = item.info;
                ComponentDescription component = this._service.GetComponent(info.Id);
                ComponentDetailsDialog form = new ComponentDetailsDialog(base.ServiceProvider, component, this._service);
                IUIService service = base.ServiceProvider.GetService(typeof(IUIService)) as IUIService;
                if (service.ShowDialog(form) == DialogResult.OK)
                {
                    this.BeginDownloadLibrary(item.info.PackageUrl);
                }
            }
        }

        private void OnDownloadButtonClick(object sender, EventArgs e)
        {
            if (this.Downloading)
            {
                this._aborted = true;
                this._downloadTask.Cancel();
                this.Downloading = false;
            }
            else
            {
                ComponentInfoListViewItem item = this._resultsListView.SelectedItems[0] as ComponentInfoListViewItem;
                this.BeginDownloadLibrary(item.info.PackageUrl);
            }
        }

        private void OnDownloadLibraryComplete(object sender, AsyncTaskResultPostedEventArgs args)
        {
            byte[] data = (byte[]) args.Data;
            if (data == null)
            {
                MessageBox.Show(this, "Error downloading library.", "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            string tempFileName = Path.GetTempFileName();
            try
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(tempFileName, FileMode.Create);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                Interop.WINTRUST_FILE_INFO structure = new Interop.WINTRUST_FILE_INFO();
                structure.cbStruct = Marshal.SizeOf(typeof(Interop.WINTRUST_FILE_INFO));
                structure.pcwszFilePath = Marshal.StringToHGlobalUni(tempFileName);
                structure.hFile = IntPtr.Zero;
                Interop.WINTRUST_DATA pWinTrustData = new Interop.WINTRUST_DATA();
                pWinTrustData.cbStruct = Marshal.SizeOf(typeof(Interop.WINTRUST_DATA));
                pWinTrustData.pPolicyCallbackData = IntPtr.Zero;
                pWinTrustData.pSIPClientData = IntPtr.Zero;
                pWinTrustData.dwUIChoice = 1;
                pWinTrustData.fdwRevocationChecks = 1;
                pWinTrustData.dwUnionChoice = 1;
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
                Marshal.StructureToPtr(structure, ptr, false);
                pWinTrustData.pSomething = ptr;
                try
                {
                    if (Interop.WinVerifyTrust(base.Handle, ref Interop.WINTRUST_ACTION_GENERIC_VERIFY_V2, ref pWinTrustData) != 0)
                    {
                        this._library = null;
                    }
                    else
                    {
                        try
                        {
                            this._library = Library.CreateLibrary(data);
                            base.DialogResult = DialogResult.OK;
                            base.Close();
                        }
                        catch
                        {
                            MessageBox.Show(this, "The downloaded library is invalid.", "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
                finally
                {
                    if (structure.pcwszFilePath != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(structure.pcwszFilePath);
                    }
                    if (ptr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ptr);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
            }
        }

        private void OnFindComponentsDone(IAsyncResult result)
        {
            try
            {
                this.ProcessComponentResults(this._service.EndFindComponents(result));
            }
            catch (Exception exception)
            {
                if (!this._aborted)
                {
                    MessageBox.Show(this, exception.Message, "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this._aborted = false;
                }
            }
            finally
            {
                this.Searching = false;
            }
        }

        private void OnGalleryComboBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.ConnectToGallery();
            }
        }

        private void OnGetCategoriesDone(IAsyncResult result)
        {
            try
            {
                CategoryInfo[] infoArray = this._newService.EndGetCategories(result);
                this._service = this._newService;
                this._resultsListView.Items.Clear();
                this._categoryListView.Items.Clear();
                for (int i = 0; i < infoArray.Length; i++)
                {
                    this._categoryListView.Items.Add(new CategoryInfoListViewItem(infoArray[i]));
                }
                this._categoryListView.Enabled = true;
                this._searchButton.Enabled = true;
                this._keywordComboBox.Enabled = true;
                string text = this._galleryComboBox.Text;
                this.ConnectMruList.AddEntry(text);
                this._galleryComboBox.Items.Clear();
                this._galleryComboBox.Items.AddRange(this.ConnectMruList.Save());
                this._galleryComboBox.SelectedIndex = 0;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(this, "The URL did not contain a valid WebService.  Please check your URL and try again", "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (Exception exception)
            {
                if (!this._aborted)
                {
                    MessageBox.Show(this, exception.Message, "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this._aborted = false;
                }
            }
            finally
            {
                this._newService = null;
                this.Connecting = false;
            }
        }

        private void OnGetComponentsDone(IAsyncResult result)
        {
            try
            {
                this.ProcessComponentResults(this._service.EndGetComponents(result));
            }
            catch (Exception exception)
            {
                if (!this._aborted)
                {
                    MessageBox.Show(this, exception.Message, "Component Gallery", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this._aborted = false;
                }
            }
            finally
            {
                this.Searching = false;
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            Application.DoEvents();
            this.ConnectToGallery();
        }

        private void OnKeywordComboBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.OnSearchButtonClick(sender, EventArgs.Empty);
            }
        }

        private void OnResultsListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this._resultsListView.SelectedItems.Count > 0) && (this._resultsListView.SelectedItems[0] is ComponentInfoListViewItem))
            {
                this._downloadButton.Enabled = true;
                this._detailsButton.Enabled = true;
                ComponentInfoListViewItem item = this._resultsListView.SelectedItems[0] as ComponentInfoListViewItem;
                if (item == null)
                {
                    this._resultsSummaryLabel.Text = string.Empty;
                }
                else
                {
                    this._resultsSummaryLabel.Text = item.info.Summary;
                }
            }
            else
            {
                this._downloadButton.Enabled = false;
                this._detailsButton.Enabled = false;
                this._resultsSummaryLabel.Text = string.Empty;
            }
        }

        private void OnSearchButtonClick(object sender, EventArgs e)
        {
            if (this.Searching)
            {
                this._aborted = true;
                this._service.Abort();
                this.Searching = false;
            }
            else if (this._keywordComboBox.Text.Length != 0)
            {
                this.Searching = true;
                this._service.BeginFindComponents(this._keywordComboBox.Text, this.TypeFilter, new AsyncCallback(this.OnFindComponentsDone), this._service);
                if (!this._keywordComboBox.Items.Contains(this._keywordComboBox.Text))
                {
                    this._keywordComboBox.Items.Add(this._keywordComboBox.Text);
                }
            }
        }

        private void ProcessComponentResults(ComponentInfo[] components)
        {
            this._resultsListView.Items.Clear();
            if ((components != null) && (components.Length > 0))
            {
                for (int i = 0; i < components.Length; i++)
                {
                    ListViewItem item = new ComponentInfoListViewItem(components[i]);
                    this._resultsListView.Items.Add(item);
                }
                this._resultsListView.Refresh();
            }
            this.UpdateListViewHeaders();
        }

        private void UpdateListViewHeaders()
        {
            int num = this._resultsListView.Width - SystemInformation.VerticalScrollBarWidth;
            if (this._oldResultsWidth != 0)
            {
                this._nameHeader.Width = (num * this._nameHeader.Width) / this._oldResultsWidth;
                this._versionHeader.Width = (num * this._versionHeader.Width) / this._oldResultsWidth;
                this._ratingHeader.Width = (num * this._ratingHeader.Width) / this._oldResultsWidth;
            }
            else
            {
                this._nameHeader.Width = (int) (num * 0.5);
                this._versionHeader.Width = (int) (num * 0.25);
                this._ratingHeader.Width = (int) (num * 0.25);
            }
            this._oldResultsWidth = num;
            num = this._categoryListView.Width - SystemInformation.VerticalScrollBarWidth;
            this._categoryHeader.Width = num;
        }

        private bool Connecting
        {
            get
            {
                return this._connecting;
            }
            set
            {
                if (this._connecting != value)
                {
                    this._connecting = value;
                    if (this._connecting)
                    {
                        this._connectButton.Text = "&Stop";
                        this._gradientBand.Start();
                    }
                    else
                    {
                        this._connectButton.Text = "&Connect";
                        this._gradientBand.Stop();
                    }
                }
            }
        }

        private MruList ConnectMruList
        {
            get
            {
                if (this._connectMruList == null)
                {
                    this._connectMruList = new MruList(10);
                    IPreferencesService service = (IPreferencesService) base.ServiceProvider.GetService(typeof(IPreferencesService));
                    if (service != null)
                    {
                        PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(ComponentGalleryDialog));
                        int num = preferencesStore.GetValue("ConnectMruCount", 0);
                        if (num != 0)
                        {
                            string[] entries = new string[num];
                            for (int i = 0; i < num; i++)
                            {
                                entries[i] = preferencesStore.GetValue("ConnectMru" + i, string.Empty);
                            }
                            this._connectMruList.Load(entries);
                        }
                    }
                }
                return this._connectMruList;
            }
        }

        public Library DownloadedLibrary
        {
            get
            {
                return this._library;
            }
        }

        private bool Downloading
        {
            get
            {
                return this._downloading;
            }
            set
            {
                if (this._downloading != value)
                {
                    this._downloading = value;
                    if (this._downloading)
                    {
                        this._downloadButton.Text = "&Cancel";
                        this._detailsButton.Enabled = false;
                        this._gradientBand.Start();
                    }
                    else
                    {
                        this._downloadButton.Text = "&Install";
                        this._detailsButton.Enabled = true;
                        this._gradientBand.Stop();
                    }
                }
            }
        }

        private bool DownloadingDetails
        {
            get
            {
                return this._downloadingDetails;
            }
            set
            {
                if (this._downloadingDetails != value)
                {
                    this._downloadingDetails = value;
                    if (this._downloadingDetails)
                    {
                        this._detailsButton.Text = "&Stop";
                        this._downloadButton.Enabled = false;
                        this._gradientBand.Start();
                    }
                    else
                    {
                        this._detailsButton.Text = "&Details";
                        this._downloadButton.Enabled = true;
                        this._gradientBand.Stop();
                    }
                }
            }
        }

        private bool Searching
        {
            get
            {
                return this._searching;
            }
            set
            {
                if (this._searching != value)
                {
                    this._searching = value;
                    if (this._searching)
                    {
                        this._searchButton.Text = "&Stop";
                        this._gradientBand.Start();
                    }
                    else
                    {
                        this._searchButton.Text = "&Search";
                        this._gradientBand.Stop();
                    }
                }
            }
        }

        public string TypeFilter
        {
            get
            {
                if (this._typeFilter == null)
                {
                    return string.Empty;
                }
                return this._typeFilter;
            }
            set
            {
                this._typeFilter = value;
            }
        }

        private class CategoryInfoListViewItem : ListViewItem
        {
            public CategoryInfo info;

            public CategoryInfoListViewItem(CategoryInfo info) : base(info.Name)
            {
                this.info = info;
            }
        }

        private class ComponentInfoListViewItem : ListViewItem
        {
            public ComponentInfo info;

            public ComponentInfoListViewItem(ComponentInfo info) : base(info.Name)
            {
                base.SubItems.Add(info.Version.Trim());
                base.SubItems.Add(string.Format("{0:f1}", info.Rating));
                this.info = info;
            }
        }

        private class DownloadAsyncTask : AsyncTask
        {
            private ComponentGalleryDialog _owner;
            private string _url;

            public DownloadAsyncTask(ComponentGalleryDialog owner, string url)
            {
                this._owner = owner;
                this._url = url;
            }

            protected override void PerformTask()
            {
                WebRequest request = WebRequest.Create(this._url);
                WebResponse response = null;
                Stream responseStream = null;
                MemoryStream stream2 = null;
                bool flag = false;
                try
                {
                    response = request.GetResponse();
                    responseStream = response.GetResponseStream();
                    stream2 = new MemoryStream();
                    int count = 0x400;
                    byte[] buffer = new byte[count];
                    int num2 = 1;
                    while (num2 > 0)
                    {
                        num2 = responseStream.Read(buffer, 0, count);
                        stream2.Write(buffer, 0, num2);
                    }
                    stream2.Flush();
                }
                catch
                {
                    flag = true;
                }
                finally
                {
                    this._owner.Downloading = false;
                    if (stream2 != null)
                    {
                        stream2.Close();
                    }
                    if (responseStream != null)
                    {
                        responseStream.Close();
                    }
                    if (response != null)
                    {
                        response.Close();
                    }
                }
                if (!flag)
                {
                    base.PostResults(stream2.ToArray(), 100, true);
                }
                else
                {
                    base.PostResults(null, 100, true);
                }
            }
        }
    }
}


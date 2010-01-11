namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public sealed class AssemblySelectionDialog : MxForm
    {
        private MxButton _addButton;
        private ColumnHeader _assemblyHeader;
        private MxButton _browseButton;
        private MxButton _cancelButton;
        private ColumnHeader _gacAssemblyHeader;
        private MxLabel _gacLabel;
        private ListView _gacList;
        private Panel _gacPanel;
        private ColumnHeader _gacVersionHeader;
        private bool _hasChanges;
        private MxButton _OKButton;
        private MxButton _removeButton;
        private AssemblyName[] _selectedAssemblies;
        private MxLabel _selectedLabel;
        private Library[] _selectedLibraries;
        private ListView _selectedList;
        private Panel _selectedPanel;
        private ColumnHeader _versionHeader;

        public AssemblySelectionDialog(IServiceProvider serviceProvider) : this(serviceProvider, null)
        {
        }

        public AssemblySelectionDialog(IServiceProvider serviceProvider, AssemblyName[] initialList) : base(serviceProvider)
        {
            this._hasChanges = false;
            this.InitializeComponent();
            if ((initialList != null) && (initialList.Length != 0))
            {
                foreach (AssemblyName name in initialList)
                {
                    this._selectedList.Items.Add(new AssemblyItem(name));
                }
            }
        }

        private void AddSelectedAssemblies()
        {
            foreach (AssemblyItem item in this._gacList.SelectedItems)
            {
                bool flag = false;
                foreach (AssemblyItem item2 in this._selectedList.Items)
                {
                    AssemblyName assemblyName = item2.AssemblyName;
                    AssemblyName name2 = item.AssemblyName;
                    if (assemblyName.FullName.Equals(name2.FullName))
                    {
                        item2.Selected = true;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    AssemblyItem item3 = new AssemblyItem(item);
                    this._selectedList.Items.Add(item3);
                    item3.Selected = true;
                    this._hasChanges = true;
                }
            }
        }

        private void InitializeComponent()
        {
            this._addButton = new MxButton();
            this._browseButton = new MxButton();
            this._removeButton = new MxButton();
            this._OKButton = new MxButton();
            this._cancelButton = new MxButton();
            this._gacLabel = new MxLabel();
            this._selectedLabel = new MxLabel();
            this._gacList = new ListView();
            this._assemblyHeader = new ColumnHeader();
            this._versionHeader = new ColumnHeader();
            this._selectedList = new ListView();
            this._gacAssemblyHeader = new ColumnHeader();
            this._gacVersionHeader = new ColumnHeader();
            this._gacPanel = new Panel();
            this._selectedPanel = new Panel();
            this._gacPanel.SuspendLayout();
            this._selectedPanel.SuspendLayout();
            base.SuspendLayout();
            this._addButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._addButton.Enabled = false;
            this._addButton.Location = new Point(0x198, 0x1a);
            this._addButton.Name = "_addButton";
            this._addButton.TabIndex = 3;
            this._addButton.Text = "&Add";
            this._addButton.Click += new EventHandler(this.OnClickAddButton);
            this._browseButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._browseButton.Location = new Point(0x198, 0x38);
            this._browseButton.Name = "_browseButton";
            this._browseButton.TabIndex = 4;
            this._browseButton.Text = "&Browse...";
            this._browseButton.Click += new EventHandler(this.OnClickBrowseButton);
            this._removeButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._removeButton.Location = new Point(0x198, 0xe2);
            this._removeButton.Name = "_removeButton";
            this._removeButton.TabIndex = 8;
            this._removeButton.Text = "&Remove";
            this._removeButton.Click += new EventHandler(this.OnClickRemoveButton);
            this._removeButton.Enabled = false;
            this._OKButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._OKButton.Location = new Point(0x146, 360);
            this._OKButton.Name = "_OKButton";
            this._OKButton.TabIndex = 9;
            this._OKButton.Text = "OK";
            this._OKButton.Click += new EventHandler(this.OnClickOKButton);
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x198, 360);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 10;
            this._cancelButton.Text = "Cancel";
            this._gacLabel.Location = new Point(8, 8);
            this._gacLabel.Name = "_gacLabel";
            this._gacLabel.Size = new Size(0x10b, 13);
            this._gacLabel.TabIndex = 0;
            this._gacLabel.Text = "Select assemblies from the Global Assembly Cache:";
            this._selectedLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this._selectedLabel.Location = new Point(8, 0xce);
            this._selectedLabel.Name = "_selectedLabel";
            this._selectedLabel.Size = new Size(250, 13);
            this._selectedLabel.TabIndex = 5;
            this._selectedLabel.Text = "Selected components:";
            this._gacList.BorderStyle = BorderStyle.None;
            this._gacList.Columns.AddRange(new ColumnHeader[] { this._gacAssemblyHeader, this._gacVersionHeader });
            this._gacList.Dock = DockStyle.Fill;
            this._gacList.FullRowSelect = true;
            this._gacList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this._gacList.HideSelection = false;
            this._gacList.Location = new Point(1, 1);
            this._gacList.Name = "_gacList";
            this._gacList.Size = new Size(390, 0xa6);
            this._gacList.Sorting = SortOrder.Ascending;
            this._gacList.TabIndex = 2;
            this._gacList.View = View.Details;
            this._gacList.DoubleClick += new EventHandler(this.OnDoubleClickGacList);
            this._gacList.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChanged);
            this._assemblyHeader.Text = "Component";
            this._assemblyHeader.Width = 260;
            this._versionHeader.Text = "Version";
            this._versionHeader.Width = 110;
            this._selectedList.BorderStyle = BorderStyle.None;
            this._selectedList.Columns.AddRange(new ColumnHeader[] { this._assemblyHeader, this._versionHeader });
            this._selectedList.Dock = DockStyle.Fill;
            this._selectedList.FullRowSelect = true;
            this._selectedList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this._selectedList.HideSelection = false;
            this._selectedList.Location = new Point(1, 1);
            this._selectedList.MultiSelect = false;
            this._selectedList.Name = "_selectedList";
            this._selectedList.Size = new Size(390, 0x7c);
            this._selectedList.Sorting = SortOrder.Ascending;
            this._selectedList.TabIndex = 7;
            this._selectedList.View = View.Details;
            this._selectedList.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChanged);
            this._gacAssemblyHeader.Text = "Assembly";
            this._gacAssemblyHeader.Width = 260;
            this._gacVersionHeader.Text = "Version";
            this._gacVersionHeader.Width = 110;
            this._gacPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._gacPanel.BackColor = SystemColors.ControlDark;
            this._gacPanel.Controls.AddRange(new Control[] { this._gacList });
            this._gacPanel.DockPadding.All = 1;
            this._gacPanel.Location = new Point(8, 0x1a);
            this._gacPanel.Name = "_gacPanel";
            this._gacPanel.Size = new Size(0x188, 0xa8);
            this._gacPanel.TabIndex = 1;
            this._selectedPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._selectedPanel.BackColor = SystemColors.ControlDark;
            this._selectedPanel.Controls.AddRange(new Control[] { this._selectedList });
            this._selectedPanel.DockPadding.All = 1;
            this._selectedPanel.Location = new Point(8, 0xe2);
            this._selectedPanel.Name = "_selectedPanel";
            this._selectedPanel.Size = new Size(0x188, 0x7e);
            this._selectedPanel.TabIndex = 6;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x1ec, 0x18a);
            base.Controls.AddRange(new Control[] { this._selectedPanel, this._gacPanel, this._selectedLabel, this._gacLabel, this._cancelButton, this._OKButton, this._removeButton, this._browseButton, this._addButton });
            base.Icon = new Icon(typeof(AssemblySelectionDialog), "AssemblySelectionDialog.ico");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AssemblySelectionDialog";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Select Components";
            this._gacPanel.ResumeLayout(false);
            this._selectedPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnClickAddButton(object sender, EventArgs e)
        {
            this.AddSelectedAssemblies();
        }

        private void OnClickBrowseButton(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select Component";
            dialog.CheckFileExists = true;
            dialog.DereferenceLinks = true;
            dialog.Multiselect = false;
            dialog.Filter = "Components (*.dll,*.exe,*.mcl)|*.dll;*.exe;*.mcl|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = dialog.FileName;
                    if (Path.GetExtension(fileName).ToLower() == ".mcl")
                    {
                        FileStream stream = null;
                        try
                        {
                            stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                            int length = (int) stream.Length;
                            byte[] buffer = new byte[length];
                            stream.Read(buffer, 0, length);
                            Library library = Library.CreateLibrary(buffer);
                            if (library != null)
                            {
                                this._selectedList.Items.Add(new LibraryItem(Path.GetFileNameWithoutExtension(fileName), library));
                                this._hasChanges = true;
                            }
                            return;
                        }
                        finally
                        {
                            if (stream != null)
                            {
                                stream.Close();
                            }
                        }
                    }
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(fileName);
                    this._selectedList.Items.Add(new AssemblyItem(assemblyName));
                    this._hasChanges = true;
                }
                catch (Exception exception)
                {
                    ((IUIService) this.GetService(typeof(IUIService))).ShowError(exception, "Unable to load file.");
                }
            }
        }

        private void OnClickOKButton(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            foreach (ListViewItem item in this._selectedList.Items)
            {
                if (item is AssemblyItem)
                {
                    AssemblyName assemblyName = ((AssemblyItem) item).AssemblyName;
                    if (assemblyName != null)
                    {
                        list.Add(assemblyName);
                    }
                }
                else if (item is LibraryItem)
                {
                    list2.Add(((LibraryItem) item).Library);
                }
            }
            this._selectedAssemblies = (AssemblyName[]) list.ToArray(typeof(AssemblyName));
            this._selectedLibraries = (Library[]) list2.ToArray(typeof(Library));
            if (this._hasChanges)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
        }

        private void OnClickRemoveButton(object sender, EventArgs e)
        {
            foreach (AssemblyItem item in this._selectedList.SelectedItems)
            {
                item.Remove();
            }
            this._hasChanges = true;
        }

        private void OnDoubleClickGacList(object sender, EventArgs e)
        {
            this.AddSelectedAssemblies();
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            foreach (AssemblyName name in Gac.GetAssemblies())
            {
                this._gacList.Items.Add(new AssemblyItem(name));
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this._addButton.Enabled = this._gacList.SelectedItems.Count > 0;
            this._removeButton.Enabled = this._selectedList.SelectedItems.Count > 0;
        }

        public AssemblyName[] SelectedAssemblies
        {
            get
            {
                return this._selectedAssemblies;
            }
        }

        public Library[] SelectedLibraries
        {
            get
            {
                return this._selectedLibraries;
            }
        }

        private class AssemblyItem : ListViewItem
        {
            private System.Reflection.AssemblyName _assemblyName;

            public AssemblyItem(AssemblySelectionDialog.AssemblyItem item) : this(item.AssemblyName)
            {
            }

            public AssemblyItem(System.Reflection.AssemblyName assemblyName)
            {
                this._assemblyName = assemblyName;
                base.Text = this.ShortName;
                base.SubItems.Add(this.Version);
            }

            public System.Reflection.AssemblyName AssemblyName
            {
                get
                {
                    return this._assemblyName;
                }
            }

            public string FullName
            {
                get
                {
                    return this._assemblyName.FullName;
                }
            }

            public string ShortName
            {
                get
                {
                    return this._assemblyName.Name;
                }
            }

            public string Version
            {
                get
                {
                    return this._assemblyName.Version.ToString();
                }
            }
        }

        private class LibraryItem : ListViewItem
        {
            private Microsoft.Matrix.Utility.Library _library;

            public LibraryItem(string filename, Microsoft.Matrix.Utility.Library library)
            {
                base.Text = filename;
                this._library = library;
                base.SubItems.Add(library.Version);
            }

            public Microsoft.Matrix.Utility.Library Library
            {
                get
                {
                    return this._library;
                }
            }
        }
    }
}


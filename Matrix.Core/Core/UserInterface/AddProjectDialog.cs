namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class AddProjectDialog : MxForm
    {
        private MxButton _cancelButton;
        private ListBox _categoryListBox;
        private ICollection _creatableProjectFactories;
        private FactoryListViewItem _currentFactoryItem;
        private MxLabel _descriptionLabel;
        private HybridDictionary _factoryCategories;
        private ListView _factoryListView;
        private TrackButton _iconViewButton;
        private string _initialCategory;
        private TrackButton _listViewButton;
        private MxButton _okButton;
        private IProjectFactory _selectedFactory;

        public AddProjectDialog(IServiceProvider serviceProvider, IList creatableProjectFactories, string initialCategory) : base(serviceProvider)
        {
            this._creatableProjectFactories = creatableProjectFactories;
            this.InitializeComponent();
            Bitmap bitmap = new Bitmap(typeof(AddProjectDialog), "IconView.bmp");
            bitmap.MakeTransparent(Color.Fuchsia);
            this._iconViewButton.EnabledImage = bitmap;
            bitmap = new Bitmap(typeof(AddProjectDialog), "ListView.bmp");
            bitmap.MakeTransparent(Color.Fuchsia);
            this._listViewButton.EnabledImage = bitmap;
            IPreferencesService service = (IPreferencesService) this.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(AddProjectDialog));
                if ((preferencesStore != null) && (preferencesStore.GetValue("View", 0) != 0))
                {
                    this._listViewButton.Checked = true;
                    this._iconViewButton.Checked = false;
                    this._factoryListView.View = View.List;
                }
            }
            this._initialCategory = initialCategory;
        }

        private void InitializeComponent()
        {
            Panel panel = new Panel();
            Panel panel2 = new Panel();
            Panel panel3 = new Panel();
            MxLabel label = new MxLabel();
            this._factoryListView = new ListView();
            this._descriptionLabel = new MxLabel();
            this._categoryListBox = new ListBox();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._iconViewButton = new TrackButton();
            this._listViewButton = new TrackButton();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            base.SuspendLayout();
            label.Location = new Point(12, 8);
            label.Size = new Size(0x40, 0x10);
            label.TabIndex = 0;
            label.Text = "&Projects:";
            this._categoryListBox.IntegralHeight = false;
            this._categoryListBox.Dock = DockStyle.Fill;
            this._categoryListBox.BorderStyle = BorderStyle.None;
            this._categoryListBox.ScrollAlwaysVisible = true;
            this._categoryListBox.TabIndex = 0;
            this._categoryListBox.Sorted = true;
            this._categoryListBox.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedCategoryListBox);
            panel.BackColor = SystemColors.ControlDark;
            panel.DockPadding.All = 1;
            panel.Location = new Point(12, 0x1a);
            panel.Size = new Size(0x9b, 0xa8);
            panel.TabIndex = 1;
            panel.Controls.Add(this._categoryListBox);
            this._iconViewButton.Size = new Size(0x12, 0x12);
            this._iconViewButton.Location = new Point(0x163, 6);
            this._iconViewButton.TabIndex = 2;
            this._iconViewButton.TabStop = false;
            this._iconViewButton.Checked = true;
            this._iconViewButton.Click += new EventHandler(this.OnClickIconViewButton);
            this._listViewButton.Size = new Size(0x12, 0x12);
            this._listViewButton.Location = new Point(0x176, 6);
            this._listViewButton.TabIndex = 3;
            this._listViewButton.TabStop = false;
            this._listViewButton.Click += new EventHandler(this.OnClickListViewButton);
            this._factoryListView.BorderStyle = BorderStyle.None;
            this._factoryListView.Dock = DockStyle.Fill;
            this._factoryListView.HideSelection = false;
            this._factoryListView.MultiSelect = false;
            this._factoryListView.TabIndex = 0;
            this._factoryListView.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedFactoryListView);
            panel2.BackColor = SystemColors.ControlDark;
            panel2.Controls.Add(this._factoryListView);
            panel2.DockPadding.All = 1;
            panel2.Location = new Point(170, 0x1a);
            panel2.Size = new Size(0xde, 0xa8);
            panel2.TabIndex = 4;
            this._descriptionLabel.BackColor = SystemColors.Control;
            this._descriptionLabel.Dock = DockStyle.Fill;
            this._descriptionLabel.TabIndex = 0;
            panel3.BackColor = SystemColors.ControlDark;
            panel3.Controls.Add(this._descriptionLabel);
            panel3.DockPadding.All = 1;
            panel3.Location = new Point(12, 0xc6);
            panel3.Size = new Size(380, 0x12);
            panel3.TabIndex = 5;
            this._okButton.Location = new Point(0xee, 0xe0);
            this._okButton.TabIndex = 6;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnClickOKButton);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x13e, 0xe0);
            this._cancelButton.TabIndex = 7;
            this._cancelButton.Text = "Cancel";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x196, 0x100);
            base.Controls.AddRange(new Control[] { this._cancelButton, this._okButton, panel3, panel2, this._listViewButton, this._iconViewButton, panel, label });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.AcceptButton = this._okButton;
            base.CancelButton = this._cancelButton;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Add New Project";
            base.Icon = new Icon(typeof(AddProjectDialog), "AddProjectDialog.ico");
            panel.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadFactories()
        {
            FactoryCategory category = new FactoryCategory("(General)");
            this._factoryCategories = new HybridDictionary(true);
            this._factoryCategories.Add("General", category);
            FactoryCategory category2 = category;
            ImageList list = new ImageList();
            ImageList list2 = new ImageList();
            list.ImageSize = new Size(0x20, 0x20);
            list.ColorDepth = ColorDepth.Depth32Bit;
            this._factoryListView.LargeImageList = list;
            list2.ImageSize = new Size(0x10, 0x10);
            list2.ColorDepth = ColorDepth.Depth32Bit;
            this._factoryListView.SmallImageList = list2;
            ImageList.ImageCollection images = list.Images;
            ImageList.ImageCollection images2 = list2.Images;
            foreach (IProjectFactory factory in this._creatableProjectFactories)
            {
                images.Add(factory.LargeIcon);
                images2.Add(factory.SmallIcon);
                FactoryListViewItem item = new FactoryListViewItem(factory, images.Count - 1);
                string str = factory.Category;
                if ((str == null) || (str.Length == 0))
                {
                    category.Items.Add(item);
                    continue;
                }
                FactoryCategory category3 = (FactoryCategory) this._factoryCategories[str];
                if (category3 != null)
                {
                    category3.Items.Add(item);
                }
                else
                {
                    category3 = new FactoryCategory(str);
                    category3.Items.Add(item);
                    this._factoryCategories.Add(str, category3);
                }
                if (string.Compare(str, this._initialCategory, true) == 0)
                {
                    category2 = category3;
                }
            }
            FactoryCategory category4 = null;
            foreach (FactoryCategory category5 in this._factoryCategories.Values)
            {
                if (category5.Items.Count != 0)
                {
                    this._categoryListBox.Items.Add(category5);
                    if (category4 == null)
                    {
                        category4 = category5;
                    }
                }
            }
            if (category2.Items.Count == 0)
            {
                category2 = category4;
            }
            this._categoryListBox.SelectedItem = category2;
            this._categoryListBox.Focus();
        }

        private void OnClickIconViewButton(object sender, EventArgs e)
        {
            this._factoryListView.View = View.LargeIcon;
            this._iconViewButton.Checked = true;
            this._listViewButton.Checked = false;
        }

        private void OnClickListViewButton(object sender, EventArgs e)
        {
            this._factoryListView.View = View.List;
            this._iconViewButton.Checked = false;
            this._listViewButton.Checked = true;
        }

        private void OnClickOKButton(object sender, EventArgs e)
        {
            IPreferencesService service = (IPreferencesService) this.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(AddProjectDialog));
                if (preferencesStore != null)
                {
                    preferencesStore.SetValue("View", (this._factoryListView.View == View.List) ? 1 : 0, 0);
                }
            }
            this._selectedFactory = this._currentFactoryItem.Factory;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this.LoadFactories();
        }

        private void OnSelectedIndexChangedCategoryListBox(object sender, EventArgs e)
        {
            this._currentFactoryItem = null;
            this._factoryListView.Items.Clear();
            FactoryCategory selectedItem = this._categoryListBox.SelectedItem as FactoryCategory;
            if (selectedItem != null)
            {
                foreach (ListViewItem item in selectedItem.Items)
                {
                    this._factoryListView.Items.Add(item);
                }
                if (selectedItem.Items.Count != 0)
                {
                    this._factoryListView.Items[0].Selected = true;
                }
            }
        }

        private void OnSelectedIndexChangedFactoryListView(object sender, EventArgs e)
        {
            this._currentFactoryItem = null;
            if (this._factoryListView.SelectedItems.Count != 0)
            {
                this._currentFactoryItem = (FactoryListViewItem) this._factoryListView.SelectedItems[0];
            }
            if (this._currentFactoryItem != null)
            {
                this._descriptionLabel.Text = this._currentFactoryItem.Factory.CreateNewDescription;
                this._okButton.Enabled = true;
            }
            else
            {
                this._descriptionLabel.Text = string.Empty;
                this._okButton.Enabled = false;
            }
        }

        public IProjectFactory SelectedFactory
        {
            get
            {
                return this._selectedFactory;
            }
        }

        private class FactoryCategory
        {
            private string _category;
            private ArrayList _items;

            public FactoryCategory(string category)
            {
                this._category = category;
                this._items = new ArrayList();
            }

            public override string ToString()
            {
                return this.Category;
            }

            public string Category
            {
                get
                {
                    return this._category;
                }
            }

            public ArrayList Items
            {
                get
                {
                    return this._items;
                }
            }
        }

        private class FactoryListViewItem : ListViewItem
        {
            private IProjectFactory _factory;

            public FactoryListViewItem(IProjectFactory factory, int imageIndex) : base(factory.Name, imageIndex)
            {
                this._factory = factory;
            }

            public IProjectFactory Factory
            {
                get
                {
                    return this._factory;
                }
            }
        }
    }
}


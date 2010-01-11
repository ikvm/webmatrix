namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    internal sealed class AddInOrganizer : MxForm
    {
        private MxButton _addButton;
        private CheckedListBox _addInList;
        private MxButton _addLocalButton;
        private MxButton _closeButton;
        private Label _descriptionLabel;
        private Panel _descriptionPanel;
        private ImageButton _downButton;
        private MxLabel _instructionsLabel;
        private Panel _listPanel;
        private MxButton _removeButton;
        private AddInEntry[] _resultList;
        private ImageButton _upButton;

        public AddInOrganizer(IServiceProvider serviceProvider, ArrayList addIns) : base(serviceProvider)
        {
            this.InitializeComponent();
            foreach (AddInEntry entry in addIns)
            {
                this._addInList.Items.Add(entry, entry.IncludeInMenu);
            }
            this._upButton.Image = new Icon(typeof(AddInOrganizer), "SortUp.ico").ToBitmap();
            this._downButton.Image = new Icon(typeof(AddInOrganizer), "SortDown.ico").ToBitmap();
            if (this._addInList.Items.Count != 0)
            {
                this._addInList.SelectedIndex = 0;
            }
            this.UpdateButtonStates();
        }

        private void InitializeComponent()
        {
            this._addInList = new CheckedListBox();
            this._instructionsLabel = new MxLabel();
            this._upButton = new ImageButton();
            this._removeButton = new MxButton();
            this._downButton = new ImageButton();
            this._addButton = new MxButton();
            this._listPanel = new Panel();
            this._closeButton = new MxButton();
            this._addLocalButton = new MxButton();
            this._descriptionPanel = new Panel();
            this._descriptionLabel = new Label();
            this._listPanel.SuspendLayout();
            this._descriptionPanel.SuspendLayout();
            base.SuspendLayout();
            this._addInList.BorderStyle = BorderStyle.None;
            this._addInList.CheckOnClick = true;
            this._addInList.Dock = DockStyle.Fill;
            this._addInList.IntegralHeight = false;
            this._addInList.Location = new Point(1, 1);
            this._addInList.Name = "_addInList";
            this._addInList.Size = new Size(0x11c, 0x94);
            this._addInList.TabIndex = 0;
            this._addInList.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChanged);
            this._addInList.ItemCheck += new ItemCheckEventHandler(this.OnAddInListItemChecked);
            this._instructionsLabel.FlatStyle = FlatStyle.System;
            this._instructionsLabel.Location = new Point(0x10, 6);
            this._instructionsLabel.Name = "_instructionsLabel";
            this._instructionsLabel.Size = new Size(0x13c, 0x2a);
            this._instructionsLabel.TabIndex = 0;
            this._instructionsLabel.Text = "Organize your collection of add-ins using the Add and Remove buttons below. The checked add-ins will be displayed in the Tools menu.";
            this._upButton.Enabled = false;
            this._upButton.FlatStyle = FlatStyle.System;
            this._upButton.Location = new Point(0x134, 0x36);
            this._upButton.Name = "_upButton";
            this._upButton.Size = new Size(0x18, 0x17);
            this._upButton.TabIndex = 3;
            this._upButton.Click += new EventHandler(this.OnUpButtonClicked);
            this._removeButton.FlatStyle = FlatStyle.System;
            this._removeButton.Location = new Point(0xb0, 0x103);
            this._removeButton.Name = "_removeButton";
            this._removeButton.TabIndex = 7;
            this._removeButton.Text = "&Remove";
            this._removeButton.Click += new EventHandler(this.OnRemoveButtonClicked);
            this._downButton.Enabled = false;
            this._downButton.FlatStyle = FlatStyle.System;
            this._downButton.Location = new Point(0x134, 0x52);
            this._downButton.Name = "_downButton";
            this._downButton.Size = new Size(0x18, 0x17);
            this._downButton.TabIndex = 4;
            this._downButton.Click += new EventHandler(this.OnDownButtonClicked);
            this._addButton.FlatStyle = FlatStyle.System;
            this._addButton.Location = new Point(0x10, 0x103);
            this._addButton.Name = "_addButton";
            this._addButton.TabIndex = 5;
            this._addButton.Text = "&Add Online";
            this._addButton.Click += new EventHandler(this.OnAddButtonClicked);
            this._listPanel.BackColor = SystemColors.ControlDark;
            this._listPanel.Controls.Add(this._addInList);
            this._listPanel.DockPadding.All = 1;
            this._listPanel.Location = new Point(0x10, 0x36);
            this._listPanel.Name = "_listPanel";
            this._listPanel.Size = new Size(0x11e, 150);
            this._listPanel.TabIndex = 1;
            this._closeButton.FlatStyle = FlatStyle.System;
            this._closeButton.Location = new Point(0x100, 0x103);
            this._closeButton.Name = "_closeButton";
            this._closeButton.TabIndex = 8;
            this._closeButton.Text = "Close";
            this._closeButton.Click += new EventHandler(this.OnCloseButtonClick);
            this._addLocalButton.FlatStyle = FlatStyle.System;
            this._addLocalButton.Location = new Point(0x60, 0x103);
            this._addLocalButton.Name = "_addLocalButton";
            this._addLocalButton.TabIndex = 6;
            this._addLocalButton.Text = "Add &Local";
            this._addLocalButton.Click += new EventHandler(this.OnAddLocalButtonClicked);
            this._descriptionPanel.BackColor = SystemColors.ControlDark;
            this._descriptionPanel.Controls.Add(this._descriptionLabel);
            this._descriptionPanel.DockPadding.All = 1;
            this._descriptionPanel.Location = new Point(0x10, 0xd0);
            this._descriptionPanel.Name = "_descriptionPanel";
            this._descriptionPanel.Size = new Size(0x11e, 0x2c);
            this._descriptionPanel.TabIndex = 2;
            this._descriptionLabel.BackColor = SystemColors.Control;
            this._descriptionLabel.Dock = DockStyle.Fill;
            this._descriptionLabel.Location = new Point(1, 1);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new Size(0x11c, 0x2a);
            this._descriptionLabel.TabIndex = 0;
            base.AcceptButton = this._closeButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._closeButton;
            base.ClientSize = new Size(340, 0x125);
            base.Controls.AddRange(new Control[] { this._descriptionPanel, this._addLocalButton, this._listPanel, this._downButton, this._upButton, this._closeButton, this._addButton, this._instructionsLabel, this._removeButton });
            base.Icon = new Icon(typeof(AddInOrganizer), "AddInOrganizer.ico");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AddInOrganizer";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Organize Add-ins";
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this._listPanel.ResumeLayout(false);
            this._descriptionPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            Library library = ((IComponentGalleryService) base.ServiceProvider.GetService(typeof(IComponentGalleryService))).BrowseGallery(typeof(AddIn).FullName);
            bool flag = false;
            if (library != null)
            {
                IApplicationIdentity service = base.ServiceProvider.GetService(typeof(IApplicationIdentity)) as IApplicationIdentity;
                ICollection is2 = library.UnpackFiles(Library.RuntimeFilesSection, service.PluginsPath);
                if ((is2 != null) && (is2.Count > 0))
                {
                    foreach (string str in is2)
                    {
                        Assembly assembly = Assembly.LoadFrom(str);
                        flag |= this.ProcessAssembly(assembly);
                    }
                }
                if (flag)
                {
                    this._addInList.SelectedIndex = this._addInList.Items.Count - 1;
                }
                else
                {
                    this.ReportError("The assembly does not contain any add-ins", true);
                }
            }
        }

        private void OnAddInListItemChecked(object sender, ItemCheckEventArgs e)
        {
            AddInEntry entry = (AddInEntry) this._addInList.Items[e.Index];
            entry.IncludeInMenu = e.NewValue == CheckState.Checked;
        }

        private void OnAddLocalButtonClicked(object sender, EventArgs e)
        {
            AssemblySelectionDialog dialog = new AssemblySelectionDialog(base.ServiceProvider);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                IApplicationIdentity identity = base.ServiceProvider.GetService(typeof(IApplicationIdentity)) as IApplicationIdentity;
                bool flag = false;
                AssemblyName[] selectedAssemblies = dialog.SelectedAssemblies;
                try
                {
                    for (int i = 0; i < selectedAssemblies.Length; i++)
                    {
                        Assembly assembly = Assembly.Load(selectedAssemblies[i]);
                        if (!assembly.GlobalAssemblyCache)
                        {
                            Uri uri = new Uri(assembly.CodeBase);
                            string localPath = uri.LocalPath;
                            if (!Directory.Exists(identity.PluginsPath))
                            {
                                Directory.CreateDirectory(identity.PluginsPath);
                            }
                            string path = Path.Combine(identity.PluginsPath, Path.GetFileName(localPath));
                            if (localPath.ToLower() != path.ToLower())
                            {
                                if (File.Exists(path))
                                {
                                    IMxUIService service = (IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService));
                                    if ((service != null) && (service.ShowMessage("The assembly " + Path.GetFileName(path) + "' has previously been added. Do you wish to replace it?", "Add-in Organizer", MessageBoxIcon.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1) == DialogResult.No))
                                    {
                                        continue;
                                    }
                                }
                                File.Copy(localPath, path, true);
                            }
                        }
                        flag |= this.ProcessAssembly(assembly);
                    }
                }
                catch (Exception exception)
                {
                    this.ReportError("A problem was encountered.\n\nDetails:\n" + exception.Message, false);
                }
                if (flag)
                {
                    this._addInList.SelectedIndex = this._addInList.Items.Count - 1;
                }
                else
                {
                    this.ReportError("No new add-ins were found.", true);
                }
            }
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ICollection items = this._addInList.Items;
            this._resultList = new AddInEntry[items.Count];
            items.CopyTo(this._resultList, 0);
        }

        private void OnDownButtonClicked(object sender, EventArgs e)
        {
            int selectedIndex = this._addInList.SelectedIndex;
            if (selectedIndex < (this._addInList.Items.Count - 1))
            {
                this.SwapListItems(selectedIndex, selectedIndex + 1);
            }
        }

        private void OnRemoveButtonClicked(object sender, EventArgs e)
        {
            int selectedIndex = this._addInList.SelectedIndex;
            if (selectedIndex != -1)
            {
                this._addInList.Items.RemoveAt(selectedIndex);
                if (selectedIndex < this._addInList.Items.Count)
                {
                    this._addInList.SelectedIndex = selectedIndex;
                }
                else if (this._addInList.Items.Count > 0)
                {
                    this._addInList.SelectedIndex = this._addInList.Items.Count - 1;
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateButtonStates();
            string description = string.Empty;
            if (this._addInList.SelectedIndex != -1)
            {
                AddInEntry selectedItem = (AddInEntry) this._addInList.SelectedItem;
                description = selectedItem.Description;
            }
            this._descriptionLabel.Text = description;
        }

        private void OnUpButtonClicked(object sender, EventArgs e)
        {
            int selectedIndex = this._addInList.SelectedIndex;
            if (selectedIndex > 0)
            {
                this.SwapListItems(selectedIndex, selectedIndex - 1);
            }
        }

        private bool ProcessAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            Type type = typeof(AddIn);
            bool flag = false;
            foreach (Type type2 in types)
            {
                if ((type2.IsPublic && !type2.IsAbstract) && type.IsAssignableFrom(type2))
                {
                    AddInEntry item = new AddInEntry(type2);
                    this._addInList.Items.Add(item);
                    flag = true;
                }
            }
            return flag;
        }

        private void ReportError(string message, bool isWarning)
        {
            IMxUIService service = (IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService));
            if (service != null)
            {
                service.ReportError(message, "Add-in Organizer", isWarning);
            }
        }

        private void SwapListItems(int index1, int index2)
        {
            object obj2 = this._addInList.Items[index1];
            this._addInList.Items[index1] = this._addInList.Items[index2];
            this._addInList.Items[index2] = obj2;
            bool itemChecked = this._addInList.GetItemChecked(index1);
            this._addInList.SetItemChecked(index1, this._addInList.GetItemChecked(index2));
            this._addInList.SetItemChecked(index2, itemChecked);
            if (this._addInList.SelectedIndex == index1)
            {
                this._addInList.SelectedIndex = index2;
            }
            else if (this._addInList.SelectedIndex == index2)
            {
                this._addInList.SelectedIndex = index1;
            }
        }

        private void UpdateButtonStates()
        {
            int selectedIndex = this._addInList.SelectedIndex;
            if (selectedIndex == -1)
            {
                this._upButton.Enabled = false;
                this._downButton.Enabled = false;
                this._removeButton.Enabled = false;
            }
            else
            {
                this._upButton.Enabled = selectedIndex != 0;
                this._downButton.Enabled = selectedIndex != (this._addInList.Items.Count - 1);
                this._removeButton.Enabled = true;
            }
        }

        public AddInEntry[] AddIns
        {
            get
            {
                return this._resultList;
            }
        }
    }
}


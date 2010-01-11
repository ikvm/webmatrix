namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class OptionsDialog : MxForm
    {
        private MxButton _cancelButton;
        private TreeView _categoryTreeView;
        private Panel _containerPanel;
        private OptionsPage _currentPage;
        private MxButton _okButton;
        private IDictionary _optionsTable;
        private Panel _topAccentPanel;
        private Panel _treeViewPanel;

        public OptionsDialog(IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._categoryTreeView = new TreeView();
            this._containerPanel = new Panel();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._treeViewPanel = new Panel();
            this._topAccentPanel = new Panel();
            this._treeViewPanel.SuspendLayout();
            base.SuspendLayout();
            this._categoryTreeView.BorderStyle = BorderStyle.None;
            this._categoryTreeView.Dock = DockStyle.Fill;
            this._categoryTreeView.HideSelection = false;
            this._categoryTreeView.ImageIndex = -1;
            this._categoryTreeView.Location = new Point(1, 1);
            this._categoryTreeView.Name = "_categoryTreeView";
            this._categoryTreeView.SelectedImageIndex = -1;
            this._categoryTreeView.Size = new Size(0x8a, 0x11a);
            this._categoryTreeView.TabIndex = 0;
            this._categoryTreeView.AfterSelect += new TreeViewEventHandler(this.OnCategoryTreeViewAfterSelectopNode);
            this._containerPanel.Location = new Point(0x98, 12);
            this._containerPanel.Name = "_containerPanel";
            this._containerPanel.Size = new Size(0x1c4, 280);
            this._containerPanel.TabIndex = 1;
            this._okButton.Location = new Point(0x1c0, 300);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnOKButtonClick);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x210, 300);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._treeViewPanel.BackColor = SystemColors.ControlDark;
            this._treeViewPanel.Controls.AddRange(new Control[] { this._categoryTreeView });
            this._treeViewPanel.DockPadding.All = 1;
            this._treeViewPanel.Location = new Point(8, 8);
            this._treeViewPanel.Name = "_treeViewPanel";
            this._treeViewPanel.Size = new Size(140, 0x11c);
            this._treeViewPanel.TabIndex = 4;
            this._topAccentPanel.BackColor = SystemColors.ControlDark;
            this._topAccentPanel.Location = new Point(0x98, 8);
            this._topAccentPanel.Name = "_topAccentPanel";
            this._topAccentPanel.Size = new Size(0x1c4, 4);
            this._topAccentPanel.TabIndex = 5;
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(610, 0x14c);
            base.Controls.AddRange(new Control[] { this._topAccentPanel, this._treeViewPanel, this._cancelButton, this._okButton, this._containerPanel });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = new Icon(typeof(OptionsDialog), "OptionsDialog.ico");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "OptionsDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this._treeViewPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            base.Close();
        }

        private void OnCategoryTreeViewAfterSelectopNode(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = this._categoryTreeView.SelectedNode;
            while (!this._optionsTable.Contains(selectedNode.FullPath))
            {
                if (selectedNode.FirstNode == null)
                {
                    return;
                }
                selectedNode = selectedNode.FirstNode;
            }
            OptionsPage page = (OptionsPage) this._optionsTable[selectedNode.FullPath];
            if (page != null)
            {
                if (this._currentPage != null)
                {
                    this._currentPage.Visible = false;
                }
                page.Visible = true;
                this._currentPage = page;
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            IPackageManager service = (IPackageManager) base.ServiceProvider.GetService(typeof(IPackageManager));
            this._optionsTable = new HybridDictionary(true);
            foreach (IPackage package in service.Packages)
            {
                OptionsPage[] optionsPages = package.GetOptionsPages();
                if (optionsPages != null)
                {
                    for (int i = 0; i < optionsPages.Length; i++)
                    {
                        this._optionsTable[optionsPages[i].OptionsPath] = optionsPages[i];
                    }
                }
            }
            this._categoryTreeView.Sorted = true;
            IDictionaryEnumerator enumerator = this._optionsTable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                string[] strArray = ((string) current.Key).Split(new char[] { '\\' });
                TreeNodeCollection nodes = this._categoryTreeView.Nodes;
                for (int j = 0; j < strArray.Length; j++)
                {
                    TreeNode node = new TreeNode(strArray[j]);
                    int num3 = -1;
                    for (int k = 0; k < nodes.Count; k++)
                    {
                        if (nodes[k].Text.ToLower() == node.Text.ToLower())
                        {
                            num3 = k;
                            break;
                        }
                    }
                    if (num3 == -1)
                    {
                        nodes.Add(node);
                    }
                    else
                    {
                        node = nodes[num3];
                    }
                    nodes = node.Nodes;
                }
                OptionsPage page = (OptionsPage) current.Value;
                page.Dock = DockStyle.Fill;
                page.Visible = false;
                this._containerPanel.Controls.Add(page);
            }
            TreeNode firstNode = this._categoryTreeView.Nodes[0];
            while (!this._optionsTable.Contains(firstNode.FullPath))
            {
                firstNode = firstNode.FirstNode;
            }
            this._categoryTreeView.SelectedNode = firstNode;
        }

        private void OnOKButtonClick(object sender, EventArgs e)
        {
            foreach (OptionsPage page in this._optionsTable.Values)
            {
                if (page.IsDirty)
                {
                    try
                    {
                        page.CommitChanges();
                        continue;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            base.Close();
        }
    }
}


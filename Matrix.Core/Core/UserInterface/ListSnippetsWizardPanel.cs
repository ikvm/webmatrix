namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class ListSnippetsWizardPanel : WizardPanel
    {
        private MxButton _checkAllButton;
        private int _checkedCountChange;
        private MxButton _checkNoneButton;
        private Panel _containerPanel;
        private bool _internalChange;
        private ListView _snippetListView;

        public ListSnippetsWizardPanel(IServiceProvider serviceProvider, string caption, string description) : base(serviceProvider)
        {
            this.InitializeComponent();
            base.Caption = caption;
            base.Description = description;
        }

        public void AddSnippet(SnippetToolboxDataItem item)
        {
            ListViewItem item2 = new SnippetListViewItem(item);
            item2.Checked = true;
            this._snippetListView.Items.Add(item2);
        }

        public void ClearSnippets()
        {
            this._snippetListView.Clear();
        }

        private void InitializeComponent()
        {
            this._snippetListView = new ListView();
            this._containerPanel = new Panel();
            this._checkAllButton = new MxButton();
            this._checkNoneButton = new MxButton();
            this._containerPanel.SuspendLayout();
            base.SuspendLayout();
            this._snippetListView.BorderStyle = BorderStyle.None;
            this._snippetListView.CheckBoxes = true;
            this._snippetListView.Dock = DockStyle.Fill;
            this._snippetListView.FullRowSelect = true;
            this._snippetListView.HeaderStyle = ColumnHeaderStyle.None;
            this._snippetListView.HideSelection = false;
            this._snippetListView.Location = new Point(1, 1);
            this._snippetListView.Name = "_snippetListView";
            this._snippetListView.Size = new Size(0x132, 0xf2);
            this._snippetListView.Sorting = SortOrder.Ascending;
            this._snippetListView.TabIndex = 0;
            this._snippetListView.View = View.List;
            this._snippetListView.ItemCheck += new ItemCheckEventHandler(this.OnSnippetListViewItemCheck);
            this._containerPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._containerPanel.BackColor = SystemColors.ControlDark;
            this._containerPanel.Controls.AddRange(new Control[] { this._snippetListView });
            this._containerPanel.DockPadding.All = 1;
            this._containerPanel.Location = new Point(8, 8);
            this._containerPanel.Name = "_containerPanel";
            this._containerPanel.Size = new Size(0x134, 0xf4);
            this._containerPanel.TabIndex = 1;
            this._checkAllButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._checkAllButton.Location = new Point(0x144, 8);
            this._checkAllButton.Name = "_checkAllButton";
            this._checkAllButton.TabIndex = 2;
            this._checkAllButton.Text = "Check All";
            this._checkAllButton.Click += new EventHandler(this.OnCheckAllButtonClick);
            this._checkNoneButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._checkNoneButton.Location = new Point(0x144, 0x24);
            this._checkNoneButton.Name = "_checkNoneButton";
            this._checkNoneButton.TabIndex = 3;
            this._checkNoneButton.Text = "Check None";
            this._checkNoneButton.Click += new EventHandler(this.OnCheckNoneButtonClick);
            base.Controls.AddRange(new Control[] { this._checkNoneButton, this._checkAllButton, this._containerPanel });
            base.Name = "SnippetList";
            base.Size = new Size(0x194, 0x108);
            this._containerPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnCheckAllButtonClick(object sender, EventArgs e)
        {
            bool flag = false;
            try
            {
                this._internalChange = true;
                foreach (ListViewItem item in this._snippetListView.Items)
                {
                    if (!item.Checked)
                    {
                        item.Checked = true;
                        flag = true;
                    }
                }
            }
            finally
            {
                this._internalChange = false;
                if (flag)
                {
                    base.UpdateWizardState();
                }
            }
        }

        private void OnCheckNoneButtonClick(object sender, EventArgs e)
        {
            bool flag = false;
            try
            {
                this._internalChange = true;
                foreach (ListViewItem item in this._snippetListView.Items)
                {
                    if (item.Checked)
                    {
                        item.Checked = false;
                        flag = true;
                    }
                }
            }
            finally
            {
                this._internalChange = false;
                if (flag)
                {
                    base.UpdateWizardState();
                }
            }
        }

        private void OnSnippetListViewItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this._internalChange)
            {
                if (e.NewValue == CheckState.Unchecked)
                {
                    this._checkedCountChange = -1;
                }
                else if (e.NewValue == CheckState.Checked)
                {
                    this._checkedCountChange = 1;
                }
                base.UpdateWizardState();
            }
        }

        public ICollection CheckedToolboxDataItems
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (SnippetListViewItem item in this._snippetListView.CheckedItems)
                {
                    list.Add(item.Item);
                }
                return list;
            }
        }

        public override bool FinishEnabled
        {
            get
            {
                if (base.IsLast)
                {
                    bool flag = (this._snippetListView.CheckedItems.Count + this._checkedCountChange) != 0;
                    this._checkedCountChange = 0;
                    return flag;
                }
                return false;
            }
        }

        public override bool NextEnabled
        {
            get
            {
                bool flag = (this._snippetListView.CheckedItems.Count + this._checkedCountChange) != 0;
                this._checkedCountChange = 0;
                return flag;
            }
        }

        private sealed class SnippetListViewItem : ListViewItem
        {
            private SnippetToolboxDataItem _item;

            public SnippetListViewItem(SnippetToolboxDataItem item) : base(item.InternalDisplayName)
            {
                if (base.Text.Length == 0)
                {
                    base.Text = "(Unnamed Snippet) " + item.DisplayName;
                }
                this._item = item;
            }

            public SnippetToolboxDataItem Item
            {
                get
                {
                    return this._item;
                }
            }
        }
    }
}


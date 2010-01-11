namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AutoCompleteForm : Form, ICommandHandler, IDisposable
    {
        private ListBox _itemList;
        private int _lastSelectedIndex;
        private ITextAutoCompletionList _list;
        private ICommandHandler _nextHandler;
        private TextControl _owner;
        private string _pickedItem;
        private int _prefixLength;
        private TextBufferLocation _startLocation;
        private Timer _timer;
        private Label _toolTipLabel;
        private TextView _view;

        public AutoCompleteForm(TextView view, TextControl owner, TextBufferLocation location, ITextAutoCompletionList list)
        {
            this.InitializeComponent();
            this._pickedItem = string.Empty;
            this._list = list;
            foreach (TextAutoCompletionItem item in list.Items)
            {
                this._itemList.Items.Add(item);
            }
            this._view = view;
            this._nextHandler = this._view.AddCommandHandler(this);
            this._owner = owner;
            if (location.ColumnIndex == location.Line.Length)
            {
                this._startLocation = location.Clone();
            }
            else
            {
                using (TextBufferSpan span = this._view.GetWordSpan(location))
                {
                    this._startLocation = span.Start.Clone();
                }
            }
            this._timer = new Timer();
            this._timer.Interval = 500;
            this._timer.Tick += new EventHandler(this.OnTimerTick);
        }

        internal void Dismiss()
        {
            this._list.OnDismiss();
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void FilterItems(string prefix, bool ignoreCase)
        {
            this._prefixLength = prefix.Length;
            this._itemList.Items.Clear();
            if (ignoreCase)
            {
                prefix = prefix.ToLower();
            }
            foreach (TextAutoCompletionItem item in this._list.Items)
            {
                string text = item.Text;
                if (ignoreCase)
                {
                    text = text.ToLower();
                }
                if (text.StartsWith(prefix))
                {
                    this._itemList.Items.Add(item);
                }
            }
            if (this._itemList.Items.Count > 0)
            {
                this._itemList.SelectedIndex = 0;
            }
        }

        public bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(TextBufferCommands))
            {
                TextBufferCommand command2 = (TextBufferCommand) command;
                TextBufferLocation startLocation = command2.StartLocation;
                switch (command.CommandID)
                {
                    case 0x2a:
                    {
                        Point commandPosition = ((TextBufferCommand) command).CommandPosition;
                        if ((commandPosition.X != this._startLocation.LineIndex) || (commandPosition.Y < this._startLocation.ColumnIndex))
                        {
                            this.Dismiss();
                        }
                        goto Label_0201;
                    }
                    case 70:
                        this.SelectCurrentItem();
                        flag = true;
                        goto Label_0201;

                    case 80:
                        switch (command2.CommandValue)
                        {
                            case 0x1b:
                            case 0x23:
                            case 0x24:
                            case 0x25:
                            case 0x27:
                                this.Dismiss();
                                goto Label_0201;

                            case 0x1c:
                            case 0x1d:
                            case 30:
                            case 0x1f:
                            case 0x20:
                                goto Label_0201;

                            case 0x21:
                                this.PreviousPage();
                                flag = true;
                                goto Label_0201;

                            case 0x22:
                                this.NextPage();
                                flag = true;
                                goto Label_0201;

                            case 0x26:
                                this.PreviousItem();
                                flag = true;
                                goto Label_0201;

                            case 40:
                                this.NextItem();
                                flag = true;
                                goto Label_0201;
                        }
                        break;

                    case 1:
                    {
                        char data = (char) command2.Data;
                        if (!this._list.IsCompletionChar(data))
                        {
                            flag = this._nextHandler.HandleCommand(command);
                            this.FilterItems(new TextBufferSpan(this._startLocation, startLocation).Text, true);
                        }
                        else
                        {
                            this.SelectCurrentItem();
                        }
                        goto Label_0201;
                    }
                    case 2:
                        if ((startLocation.LineIndex != this._startLocation.LineIndex) || (startLocation.ColumnIndex == Math.Max(this._startLocation.ColumnIndex - 1, 0)))
                        {
                            this.Dismiss();
                        }
                        else
                        {
                            flag = this._nextHandler.HandleCommand(command);
                            this.FilterItems(new TextBufferSpan(this._startLocation, startLocation).Text, true);
                        }
                        goto Label_0201;

                    case 20:
                        this.SelectCurrentItem();
                        flag = true;
                        goto Label_0201;
                }
            }
        Label_0201:
            if (!flag)
            {
                flag = this._nextHandler.HandleCommand(command);
            }
            return flag;
        }

        private void HideToolTip()
        {
            if (this._toolTipLabel != null)
            {
                this._view.Controls.Remove(this._toolTipLabel);
                this._toolTipLabel.Visible = false;
                this._toolTipLabel.Dispose();
                this._toolTipLabel = null;
            }
        }

        private void InitializeComponent()
        {
            this._itemList = new ListBox();
            base.SuspendLayout();
            this._itemList.BorderStyle = BorderStyle.Fixed3D;
            this._itemList.DrawMode = DrawMode.OwnerDrawFixed;
            this._itemList.Font = new Font("Tahoma", 8f);
            this._itemList.Dock = DockStyle.Fill;
            this._itemList.IntegralHeight = false;
            this._itemList.ItemHeight = 0x10;
            this._itemList.Name = "_itemList";
            this._itemList.TabIndex = 0;
            this._itemList.DoubleClick += new EventHandler(this.OnItemListDoubleClick);
            this._itemList.MeasureItem += new MeasureItemEventHandler(this.OnItemListMeasureItem);
            this._itemList.DrawItem += new DrawItemEventHandler(this.OnItemListDrawItem);
            this._itemList.SelectedIndexChanged += new EventHandler(this.OnItemListSelectedIndexChanged);
            this._itemList.HandleCreated += new EventHandler(this.OnItemListHandleCreated);
            base.ClientSize = new Size(0x58, 100);
            base.Controls.AddRange(new Control[] { this._itemList });
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "AutoCompleteForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "AutoCompleteForm";
            this.Cursor = Cursors.Arrow;
            base.ResumeLayout(false);
        }

        private void NextItem()
        {
            if (this._itemList.SelectedIndex < (this._itemList.Items.Count - 1))
            {
                this._itemList.SelectedIndex++;
            }
        }

        private void NextPage()
        {
            int selectedIndex = this._itemList.SelectedIndex;
            this._itemList.SelectedIndex = Math.Min((int) (selectedIndex + 8), (int) (this._itemList.Items.Count - 1));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.HideToolTip();
            this._timer.Stop();
            this._timer.Dispose();
        }

        private void OnItemListDoubleClick(object sender, EventArgs e)
        {
            base.BeginInvoke(new MethodInvoker(this.SelectCurrentItem));
        }

        private void OnItemListDrawItem(object sender, DrawItemEventArgs e)
        {
            TextAutoCompletionItem item = (TextAutoCompletionItem) this._itemList.Items[e.Index];
            Brush white = Brushes.White;
            Brush black = Brushes.Black;
            if ((e.State & DrawItemState.Selected) != DrawItemState.None)
            {
                white = new SolidBrush(SystemColors.Highlight);
                black = new SolidBrush(SystemColors.HighlightText);
            }
            else
            {
                white = new SolidBrush(SystemColors.Window);
                black = new SolidBrush(SystemColors.WindowText);
            }
            e.Graphics.FillRectangle(white, e.Bounds);
            int left = e.Bounds.Left;
            if (item.Image != null)
            {
                e.Graphics.DrawImage(item.Image, left, e.Bounds.Top);
                left += item.Image.Width + 4;
            }
            e.Graphics.DrawString(item.Text, e.Font, black, (float) left, (float) e.Bounds.Top);
        }

        private void OnItemListHandleCreated(object sender, EventArgs e)
        {
            if (this._itemList.Items.Count > 0)
            {
                this._itemList.SelectedIndex = 0;
            }
            this._timer.Start();
        }

        private void OnItemListMeasureItem(object sender, MeasureItemEventArgs e)
        {
            TextAutoCompletionItem item = (TextAutoCompletionItem) this._itemList.Items[e.Index];
            SizeF ef = e.Graphics.MeasureString(item.Text, this._itemList.Font);
            if (item.Image != null)
            {
                e.ItemHeight = Math.Max(item.Image.Height, (int) ef.Height);
                e.ItemWidth = (item.Image.Width + 4) + ((int) ef.Width);
            }
            else
            {
                e.ItemHeight = (int) ef.Height;
                e.ItemWidth = (int) ef.Width;
            }
        }

        private void OnItemListSelectedIndexChanged(object sender, EventArgs e)
        {
            this.HideToolTip();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            int selectedIndex = this._itemList.SelectedIndex;
            if ((selectedIndex != -1) && (this._lastSelectedIndex == selectedIndex))
            {
                this.UpdateToolTip();
            }
            this._lastSelectedIndex = selectedIndex;
        }

        private void PreviousItem()
        {
            if ((this._itemList.SelectedIndex != -1) && (this._itemList.SelectedIndex > 0))
            {
                this._itemList.SelectedIndex--;
            }
        }

        private void PreviousPage()
        {
            if (this._itemList.SelectedIndex != -1)
            {
                int selectedIndex = this._itemList.SelectedIndex;
                this._itemList.SelectedIndex = Math.Max(0, selectedIndex - 8);
            }
        }

        private void SelectCurrentItem()
        {
            if (this._itemList.SelectedItems.Count > 0)
            {
                base.DialogResult = DialogResult.OK;
                this._pickedItem = this._itemList.SelectedItem.ToString();
            }
            base.Close();
        }

        void IDisposable.Dispose()
        {
            this.HideToolTip();
            this._view.RemoveCommandHandler(this._nextHandler);
            this._view = null;
            this._startLocation.Dispose();
        }

        public bool UpdateCommand(Command command)
        {
            return false;
        }

        private void UpdateToolTip()
        {
            this.HideToolTip();
            TextAutoCompletionItem selectedItem = (TextAutoCompletionItem) this._itemList.SelectedItem;
            if (selectedItem.Text.Length > 0)
            {
                this._toolTipLabel = new Label();
                this._toolTipLabel.BorderStyle = BorderStyle.FixedSingle;
                this._toolTipLabel.BackColor = SystemColors.Info;
                this._toolTipLabel.ForeColor = SystemColors.InfoText;
                this._toolTipLabel.Font = new Font("Tahoma", 8f);
                this._toolTipLabel.AutoSize = true;
                this._toolTipLabel.Text = selectedItem.Description;
                this._toolTipLabel.Cursor = Cursors.Arrow;
                Rectangle itemRectangle = this._itemList.GetItemRectangle(this._itemList.SelectedIndex);
                Point location = base.Location;
                this._toolTipLabel.Location = new Point((location.X + base.Width) + 2, (location.Y + itemRectangle.Top) + 2);
                this._view.Controls.Add(this._toolTipLabel);
                this._toolTipLabel.Visible = true;
            }
        }

        public string PickedItem
        {
            get
            {
                return this._pickedItem;
            }
        }

        public string PickedItemSuffix
        {
            get
            {
                return this._pickedItem.Substring(this._prefixLength);
            }
        }
    }
}


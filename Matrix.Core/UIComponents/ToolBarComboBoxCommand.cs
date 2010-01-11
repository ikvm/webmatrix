namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public class ToolBarComboBoxCommand : ToolBarButtonCommand
    {
        private bool _enterKeyPressed;
        private ToolBarComboBoxCommand _fillCommand;
        private string[] _items;
        private bool _itemsChanged;
        private int _selectedIndex;
        private bool _selectedIndexChanged;

        public ToolBarComboBoxCommand(Type commandGroup, int commandID, int fillCommandID, ComboBoxToolBarButton button) : base(commandGroup, commandID, button)
        {
            this._selectedIndex = -1;
            if (fillCommandID != 0)
            {
                this._fillCommand = new ToolBarComboBoxCommand(commandGroup, fillCommandID, 0, button);
            }
        }

        public override void UpdateCommandUI()
        {
            ComboBoxToolBarButton commandUI = (ComboBoxToolBarButton) base.CommandUI;
            ToolBarComboBox comboBox = commandUI.ComboBox;
            if (commandUI.ComboBox != null)
            {
                base.InternalChange = true;
                if (this._itemsChanged)
                {
                    comboBox.Items.Clear();
                    if ((this._items != null) && (this._items.Length != 0))
                    {
                        comboBox.Items.AddRange(this._items);
                    }
                    this._itemsChanged = false;
                }
                else
                {
                    comboBox.Enabled = this.Enabled;
                    if (!this.Enabled)
                    {
                        this.SelectedIndex = -1;
                        this.Text = string.Empty;
                    }
                    if (this._selectedIndexChanged)
                    {
                        if (comboBox.SelectedIndex != this._selectedIndex)
                        {
                            comboBox.SelectedIndex = this._selectedIndex;
                        }
                        this._selectedIndexChanged = false;
                    }
                    else if (base.TextChanged)
                    {
                        string text = this.Text;
                        if (string.Compare(text, comboBox.Text, true) != 0)
                        {
                            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                            {
                                this._selectedIndex = comboBox.FindString(text);
                                comboBox.SelectedIndex = this._selectedIndex;
                            }
                            else
                            {
                                this._selectedIndex = -1;
                                comboBox.Text = text;
                            }
                        }
                    }
                    base.TextChanged = false;
                }
                base.InternalChange = false;
            }
        }

        public bool EnterKeyPressed
        {
            get
            {
                return this._enterKeyPressed;
            }
            set
            {
                this._enterKeyPressed = value;
            }
        }

        public ToolBarComboBoxCommand FillCommand
        {
            get
            {
                return this._fillCommand;
            }
        }

        public string[] Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
                this._itemsChanged = true;
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (this._selectedIndexChanged)
                {
                    return this._selectedIndex;
                }
                ComboBoxToolBarButton commandUI = (ComboBoxToolBarButton) base.CommandUI;
                if (commandUI.ComboBox != null)
                {
                    return commandUI.ComboBox.SelectedIndex;
                }
                return -1;
            }
            set
            {
                ComboBoxToolBarButton commandUI = (ComboBoxToolBarButton) base.CommandUI;
                if (((commandUI.ComboBox != null) && (value != this._selectedIndex)) && ((value >= -1) && (value < commandUI.ComboBox.Items.Count)))
                {
                    this._selectedIndex = value;
                    this._selectedIndexChanged = true;
                }
            }
        }
    }
}


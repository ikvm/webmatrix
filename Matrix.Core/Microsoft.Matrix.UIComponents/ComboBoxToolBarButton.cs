namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public class ComboBoxToolBarButton : MxToolBarButton
    {
        private ToolBarComboBox _comboBox;
        private Panel _comboBoxHolder;
        private int _dropDownWidth;
        private string _initialText;
        private object[] _items;
        private bool _parentRecreating;
        private ComboBoxStyle _style;

        public ComboBoxToolBarButton() : this(null)
        {
        }

        public ComboBoxToolBarButton(string text) : base(text)
        {
            this._style = ComboBoxStyle.DropDownList;
            this._dropDownWidth = -1;
        }

        public MxComboBox CreateComboBox()
        {
            if (!this._parentRecreating)
            {
                if (this._comboBox != null)
                {
                    throw new InvalidOperationException();
                }
                this._comboBox = this.CreateComboBoxControl();
                this._comboBox.DropDownStyle = this._style;
                this._comboBox.FlatAppearance = true;
                this._comboBox.TabStop = false;
                if (this._dropDownWidth != -1)
                {
                    this._comboBox.DropDownWidth = this._dropDownWidth;
                }
                if (this._items != null)
                {
                    this._comboBox.Items.AddRange(this._items);
                }
                if (this._initialText != null)
                {
                    this._comboBox.InitialText = this._initialText;
                }
            }
            return this._comboBox;
        }

        protected virtual ToolBarComboBox CreateComboBoxControl()
        {
            return new ToolBarComboBox();
        }

        internal void SetParentRecreating(bool recreating)
        {
            this._parentRecreating = recreating;
            if (recreating)
            {
                this._comboBoxHolder.Parent = null;
            }
        }

        public ToolBarComboBox ComboBox
        {
            get
            {
                return this._comboBox;
            }
        }

        internal Panel ComboBoxHolder
        {
            get
            {
                return this._comboBoxHolder;
            }
            set
            {
                this._comboBoxHolder = value;
            }
        }

        public object[] ComboBoxItems
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        public ComboBoxStyle DropDownStyle
        {
            get
            {
                return this._style;
            }
            set
            {
                if ((value != ComboBoxStyle.DropDownList) && (value != ComboBoxStyle.DropDown))
                {
                    throw new ArgumentOutOfRangeException();
                }
                if ((base.Parent != null) && base.Parent.IsHandleCreated)
                {
                    throw new InvalidOperationException();
                }
                this._style = value;
            }
        }

        public int DropDownWidth
        {
            get
            {
                return this._dropDownWidth;
            }
            set
            {
                if ((value != -1) && (value < 1))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._dropDownWidth = value;
            }
        }

        public string InitialText
        {
            get
            {
                if (this._initialText == null)
                {
                    return string.Empty;
                }
                return this._initialText;
            }
            set
            {
                this._initialText = value;
            }
        }
    }
}


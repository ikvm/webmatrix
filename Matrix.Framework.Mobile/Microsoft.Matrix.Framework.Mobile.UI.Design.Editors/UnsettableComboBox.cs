namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal class UnsettableComboBox : ComboBox
    {
        private string notSetCompactText = MobileResource.GetString("UnsettableComboBox_NotSetCompactText");
        private string notSetText = MobileResource.GetString("UnsettableComboBox_NotSetText");

        internal UnsettableComboBox()
        {
        }

        internal void AddItem(object item)
        {
            this.EnsureNotSetItem();
            base.Items.Add(item);
        }

        internal void EnsureNotSetItem()
        {
            if (base.Items.Count == 0)
            {
                base.Items.Add(this.notSetText);
            }
        }

        internal bool IsSet()
        {
            return (this.SelectedIndex > 0);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (this.SelectedIndex == 0)
            {
                this.SelectedIndex = -1;
            }
        }

        protected override void SetItemsCore(IList values)
        {
            base.Items.Clear();
            if (!base.DesignMode)
            {
                base.Items.Add(this.notSetText);
            }
            ArrayList list = new ArrayList();
            foreach (object obj2 in values)
            {
                list.Add(obj2);
            }
            base.AddItemsCore(list.ToArray());
        }

        internal string NotSetCompactText
        {
            get
            {
                return this.notSetCompactText;
            }
            set
            {
                this.notSetCompactText = value;
            }
        }

        internal string NotSetText
        {
            get
            {
                return this.notSetText;
            }
            set
            {
                this.notSetText = value;
            }
        }

        public override string Text
        {
            get
            {
                if (this.SelectedIndex == 0)
                {
                    return string.Empty;
                }
                return base.Text;
            }
            set
            {
                if (value == this.notSetCompactText)
                {
                    base.Text = string.Empty;
                }
                else
                {
                    base.Text = value;
                }
            }
        }
    }
}


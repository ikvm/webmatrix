namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class WizardForm : TaskForm
    {
        private MxButton _cancelButton;
        private MxButton _finishButton;
        private MxButton _nextButton;
        private int _panelIndex;
        private MxButton _previousButton;
        private Panel _wizardPanelContainer;
        private WizardPanelCollection _wizardPanels;

        public WizardForm(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._panelIndex = -1;
        }

        private void InitializeComponent()
        {
            this._nextButton = new MxButton();
            this._previousButton = new MxButton();
            this._wizardPanelContainer = new Panel();
            this._cancelButton = new MxButton();
            this._finishButton = new MxButton();
            base.SuspendLayout();
            this._nextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._nextButton.Enabled = false;
            this._nextButton.Location = new Point(260, 0x14c);
            this._nextButton.Name = "_nextButton";
            this._nextButton.TabIndex = 100;
            this._nextButton.Text = "&Next";
            this._nextButton.Click += new EventHandler(this.OnNextButtonClick);
            this._previousButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._previousButton.Enabled = false;
            this._previousButton.Location = new Point(180, 0x14c);
            this._previousButton.Name = "_previousButton";
            this._previousButton.TabIndex = 0x65;
            this._previousButton.Text = "&Previous";
            this._previousButton.Click += new EventHandler(this.OnPreviousButtonClick);
            this._wizardPanelContainer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._wizardPanelContainer.Location = new Point(4, 0x40);
            this._wizardPanelContainer.Name = "_wizardPanelContainer";
            this._wizardPanelContainer.Size = new Size(0x1ec, 260);
            this._wizardPanelContainer.TabIndex = 0x67;
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.Location = new Point(420, 0x14c);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 0x67;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._finishButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._finishButton.Enabled = false;
            this._finishButton.Location = new Point(340, 0x14c);
            this._finishButton.Name = "_finishButton";
            this._finishButton.TabIndex = 0x66;
            this._finishButton.Text = "&Finish";
            this._finishButton.Click += new EventHandler(this.OnFinishButtonClick);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(500, 0x16a);
            base.Controls.AddRange(new Control[] { this._finishButton, this._cancelButton, this._wizardPanelContainer, this._nextButton, this._previousButton });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = null;
            base.Name = "WizardForm";
            base.MinimizeBox = false;
            base.MaximizeBox = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.ResumeLayout(false);
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        protected virtual void OnCompleted()
        {
        }

        private void OnFinishButtonClick(object sender, EventArgs e)
        {
            WizardPanel panel = this.WizardPanels[this._panelIndex];
            if (panel.OnNext())
            {
                this.OnCompleted();
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            if (this.WizardPanels.Count > 0)
            {
                this.ShowPanel(0);
            }
        }

        private void OnNextButtonClick(object sender, EventArgs e)
        {
            this.ShowNextPanel();
        }

        private void OnPreviousButtonClick(object sender, EventArgs e)
        {
            this.ShowPreviousPanel();
        }

        public void ShowNextPanel()
        {
            WizardPanel panel = this.WizardPanels[this._panelIndex];
            if (panel.OnNext())
            {
                this.ShowPanel(this._panelIndex + 1);
            }
        }

        private void ShowPanel(int index)
        {
            if (this._panelIndex != index)
            {
                if (this._panelIndex != -1)
                {
                    WizardPanel panel = this.WizardPanels[this._panelIndex];
                    panel.OnDeactivating();
                    panel.Visible = false;
                }
                this._panelIndex = index;
                WizardPanel panel2 = this.WizardPanels[this._panelIndex];
                panel2.Visible = true;
                panel2.OnActivated();
                panel2.Focus();
                this.UpdateWizardState();
                this.UpdateWizardText();
            }
        }

        public void ShowPreviousPanel()
        {
            if (this._panelIndex != -1)
            {
                this.WizardPanels[this._panelIndex].OnPrevious();
                this.ShowPanel(this._panelIndex - 1);
            }
        }

        internal void UpdateWizardState()
        {
            WizardPanel panel = this.WizardPanels[this._panelIndex];
            this._previousButton.Enabled = this._panelIndex != 0;
            this._nextButton.Enabled = (this._panelIndex != (this.WizardPanels.Count - 1)) && panel.NextEnabled;
            this._finishButton.Enabled = panel.FinishEnabled;
        }

        internal void UpdateWizardText()
        {
            WizardPanel panel = this.WizardPanels[this._panelIndex];
            base.TaskDescription = panel.Description;
            base.TaskCaption = panel.Caption;
        }

        public WizardPanelCollection WizardPanels
        {
            get
            {
                if (this._wizardPanels == null)
                {
                    this._wizardPanels = new WizardPanelCollection(this);
                }
                return this._wizardPanels;
            }
        }

        public sealed class WizardPanelCollection : IList, ICollection, IEnumerable
        {
            private WizardForm _owner;
            private ArrayList _wizardPanels;

            internal WizardPanelCollection(WizardForm owner)
            {
                this._owner = owner;
                this._wizardPanels = new ArrayList();
            }

            public void Add(WizardPanel panel)
            {
                if (!this._wizardPanels.Contains(panel))
                {
                    panel.Dock = DockStyle.Fill;
                    panel.SetWizardForm(this._owner);
                    panel.Visible = false;
                    this._wizardPanels.Add(panel);
                    this._owner._wizardPanelContainer.Controls.Add(panel);
                }
            }

            public void AddRange(WizardPanel[] panels)
            {
                foreach (WizardPanel panel in panels)
                {
                    this.Add(panel);
                }
            }

            public void Clear()
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    this.RemoveAt(0);
                }
            }

            public bool Contains(WizardPanel panel)
            {
                return (this.IndexOf(panel) != -1);
            }

            public void CopyTo(Array dest, int index)
            {
                if (this.Count > 0)
                {
                    this._wizardPanels.CopyTo(dest, index);
                }
            }

            public IEnumerator GetEnumerator()
            {
                if (this.Count != 0)
                {
                    return this._wizardPanels.GetEnumerator();
                }
                return new WizardPanel[0].GetEnumerator();
            }

            public int IndexOf(WizardPanel panel)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i] == panel)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Remove(WizardPanel panel)
            {
                if (this._wizardPanels.Contains(panel))
                {
                    this._owner._wizardPanelContainer.Controls.Remove(panel);
                    this._wizardPanels.Remove(panel);
                }
            }

            public void RemoveAt(int index)
            {
                WizardPanel panel = (WizardPanel) this._wizardPanels[index];
                this._wizardPanels.RemoveAt(index);
                this._owner._wizardPanelContainer.Controls.Remove(panel);
            }

            int IList.Add(object value)
            {
                WizardPanel panel = value as WizardPanel;
                if (panel == null)
                {
                    throw new ArgumentException();
                }
                this.Add(panel);
                return this.IndexOf(panel);
            }

            bool IList.Contains(object value)
            {
                WizardPanel panel = value as WizardPanel;
                return ((panel != null) && this.Contains(panel));
            }

            int IList.IndexOf(object value)
            {
                WizardPanel panel = value as WizardPanel;
                if (panel != null)
                {
                    return this.IndexOf(panel);
                }
                return -1;
            }

            void IList.Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            void IList.Remove(object value)
            {
                WizardPanel panel = value as WizardPanel;
                if (panel != null)
                {
                    this.Remove(panel);
                }
            }

            public int Count
            {
                get
                {
                    return this._wizardPanels.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            public WizardPanel this[int index]
            {
                get
                {
                    return (WizardPanel) this._wizardPanels[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            public object SyncRoot
            {
                get
                {
                    return this;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}


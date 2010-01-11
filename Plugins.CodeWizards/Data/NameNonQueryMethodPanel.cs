namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class NameNonQueryMethodPanel : WizardPanel
    {
        private string _defaultMethodName;
        private MxTextBox _methodNameTextBox;
        private MxLabel _nameLabel;

        public NameNonQueryMethodPanel(IServiceProvider serviceProvider, string defaultMethodName) : base(serviceProvider)
        {
            this._defaultMethodName = defaultMethodName;
            this.InitializeComponent();
        }

        private void _methodNameTextBox_TextChanged(object sender, EventArgs e)
        {
            base.UpdateWizardState();
        }

        private void InitializeComponent()
        {
            this._nameLabel = new MxLabel();
            this._methodNameTextBox = new MxTextBox();
            base.SuspendLayout();
            this._nameLabel.Location = new Point(8, 12);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new Size(460, 0x1c);
            this._nameLabel.TabIndex = 0;
            this._nameLabel.Text = "This CodeBuilder will generate a public method with strongly-typed parameters.  What should the method be called?";
            this._methodNameTextBox.AlwaysShowFocusCues = true;
            this._methodNameTextBox.Location = new Point(8, 0x2c);
            this._methodNameTextBox.Name = "_methodNameTextBox";
            this._methodNameTextBox.Size = new Size(0x10c, 20);
            this._methodNameTextBox.TabIndex = 4;
            this._methodNameTextBox.Text = this.DefaultMethodName;
            this._methodNameTextBox.TextChanged += new EventHandler(this._methodNameTextBox_TextChanged);
            base.Controls.AddRange(new Control[] { this._methodNameTextBox, this._nameLabel });
            base.Caption = "Name Method";
            base.Description = "Enter a name for the method to be generated.";
            base.Name = "NameDeleteMethodPanel";
            base.Size = new Size(0x1ec, 0x10c);
            base.ResumeLayout(false);
        }

        protected override bool OnNext()
        {
            string id = this._methodNameTextBox.Text.Trim();
            if (!this.QueryBuilder.ValidateIdentifier(id))
            {
                MessageBox.Show(base.WizardForm, "Invalid method name!");
                return false;
            }
            this.QueryBuilder.MethodName = id;
            return true;
        }

        public string DefaultMethodName
        {
            get
            {
                if (this._defaultMethodName == null)
                {
                    return "MyNonQueryMethod";
                }
                return this._defaultMethodName;
            }
            set
            {
                this._defaultMethodName = value;
            }
        }

        public override bool FinishEnabled
        {
            get
            {
                return (this._methodNameTextBox.Text.Trim().Length > 0);
            }
        }

        private Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder QueryBuilder
        {
            get
            {
                return (Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder) base.WizardForm;
            }
        }
    }
}


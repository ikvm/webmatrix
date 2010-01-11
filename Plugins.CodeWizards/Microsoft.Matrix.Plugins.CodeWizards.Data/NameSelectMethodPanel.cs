namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class NameSelectMethodPanel : WizardPanel
    {
        private MxRadioButton _dataReaderRadioButton;
        private MxRadioButton _dataSetRadioButton;
        private MxTextBox _methodNameTextBox;
        private MxLabel _nameLabel;
        private MxLabel _typeLabel;

        public NameSelectMethodPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private void _methodNameTextBox_TextChanged(object sender, EventArgs e)
        {
            base.UpdateWizardState();
        }

        private void InitializeComponent()
        {
            this._nameLabel = new MxLabel();
            this._typeLabel = new MxLabel();
            this._dataReaderRadioButton = new MxRadioButton();
            this._dataSetRadioButton = new MxRadioButton();
            this._methodNameTextBox = new MxTextBox();
            base.SuspendLayout();
            this._nameLabel.Location = new Point(8, 12);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new Size(460, 0x1c);
            this._nameLabel.TabIndex = 0;
            this._nameLabel.Text = "This CodeBuilder will generate a public method with strongly-typed parameters.  What should the method be called?";
            this._typeLabel.Location = new Point(8, 0x54);
            this._typeLabel.Name = "_typeLabel";
            this._typeLabel.Size = new Size(460, 0x24);
            this._typeLabel.TabIndex = 1;
            this._typeLabel.Text = "The method can return either an System.Data.SqlClient.SqlDataReader or a System.Data.DataSet.  Which do you want?";
            this._dataReaderRadioButton.Location = new Point(40, 0x90);
            this._dataReaderRadioButton.Name = "_dataReaderRadioButton";
            this._dataReaderRadioButton.Size = new Size(0x68, 0x10);
            this._dataReaderRadioButton.TabIndex = 2;
            this._dataReaderRadioButton.Text = "DataReader";
            this._dataSetRadioButton.Checked = true;
            this._dataSetRadioButton.Location = new Point(40, 0x7c);
            this._dataSetRadioButton.Name = "_dataSetRadioButton";
            this._dataSetRadioButton.Size = new Size(0x68, 0x10);
            this._dataSetRadioButton.TabIndex = 3;
            this._dataSetRadioButton.TabStop = true;
            this._dataSetRadioButton.Text = "DataSet";
            this._methodNameTextBox.AlwaysShowFocusCues = true;
            this._methodNameTextBox.Location = new Point(8, 0x2c);
            this._methodNameTextBox.Name = "_methodNameTextBox";
            this._methodNameTextBox.Size = new Size(0x10c, 20);
            this._methodNameTextBox.TabIndex = 4;
            this._methodNameTextBox.Text = "MyQueryMethod";
            this._methodNameTextBox.TextChanged += new EventHandler(this._methodNameTextBox_TextChanged);
            base.Controls.AddRange(new Control[] { this._methodNameTextBox, this._dataSetRadioButton, this._dataReaderRadioButton, this._typeLabel, this._nameLabel });
            base.Caption = "Name Method";
            base.Description = "Enter a name for the method to be generated.";
            base.Name = "NameMethodPanel";
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
            if (this._dataReaderRadioButton.Checked)
            {
                this.QueryBuilder.ReturnType = typeof(IDataReader);
            }
            else
            {
                this.QueryBuilder.ReturnType = typeof(DataSet);
            }
            return true;
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


namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Windows.Forms;

    public class QueryPreviewPanel : WizardPanel
    {
        private MxButton _previewButton;
        private DataGrid _previewGrid;
        private IDictionary _previewTextBoxes;

        public QueryPreviewPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._previewGrid = new DataGrid();
            this._previewButton = new MxButton();
            this._previewGrid.BeginInit();
            base.SuspendLayout();
            this._previewGrid.AllowNavigation = false;
            this._previewGrid.AllowSorting = false;
            this._previewGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._previewGrid.CaptionVisible = false;
            this._previewGrid.DataMember = "";
            this._previewGrid.HeaderForeColor = SystemColors.ControlText;
            this._previewGrid.Location = new Point(8, 0x10);
            this._previewGrid.Name = "_previewGrid";
            this._previewGrid.ReadOnly = true;
            this._previewGrid.Size = new Size(0x1dc, 0x120);
            this._previewGrid.TabIndex = 0;
            this._previewButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._previewButton.FlatStyle = FlatStyle.System;
            this._previewButton.Location = new Point(0x198, 0x134);
            this._previewButton.Name = "_previewButton";
            this._previewButton.TabIndex = 1;
            this._previewButton.Text = "Test Query";
            this._previewButton.Click += new EventHandler(this.OnPreviewButtonClick);
            base.Controls.AddRange(new Control[] { this._previewButton, this._previewGrid });
            base.Caption = "Query Preview";
            base.Description = "Test your query by entering some test data.";
            base.Name = "QueryPreviewPanel";
            base.Size = new Size(0x1ec, 340);
            this._previewGrid.EndInit();
            base.ResumeLayout(false);
        }

        protected override void OnActivated()
        {
            this._previewGrid.DataSource = new DataTable();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            Form form = ((Button) sender).FindForm();
            form.DialogResult = DialogResult.Cancel;
            form.Close();
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            Form owner = ((Button) sender).FindForm();
            foreach (MxTextBox box in this._previewTextBoxes.Values)
            {
                if (box.Text.Length == 0)
                {
                    MessageBox.Show(owner, "All fields are required.", "Query Preview", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            owner.DialogResult = DialogResult.OK;
            owner.Close();
        }

        private void OnPreviewButtonClick(object sender, EventArgs e)
        {
            bool flag = false;
            string previewQuery = ((Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder) base.WizardForm).PreviewQuery;
            if (previewQuery != null)
            {
                IDbConnection connection = ((IDataProviderDatabase) this.Database).CreateConnection();
                try
                {
                    IDbCommand selectCommand = ((IDataProviderDatabase) this.Database).CreateCommand(previewQuery);
                    selectCommand.Connection = connection;
                    if (this.Parameters.Count > 0)
                    {
                        MxForm form = new MxForm(base.ServiceProvider);
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowInTaskbar = false;
                        form.MinimizeBox = false;
                        form.MaximizeBox = false;
                        form.FormBorderStyle = FormBorderStyle.FixedDialog;
                        form.Text = "Preview";
                        int height = (((this.Parameters.Count * 0x18) + 30) + 40) + 0x7c;
                        form.Size = new Size(0x176, height);
                        int y = 4;
                        Label label = new Label();
                        label.Text = "Enter test values for the following query:";
                        label.Location = new Point(12, y);
                        label.Size = new Size(350, 0x10);
                        form.Controls.Add(label);
                        y += 20;
                        MxTextBox box = new MxTextBox();
                        box.ReadOnly = true;
                        box.Text = previewQuery;
                        box.Multiline = true;
                        box.Location = new Point(12, y);
                        box.Size = new Size(350, 100);
                        form.Controls.Add(box);
                        y += 100;
                        this._previewTextBoxes = new HybridDictionary(true);
                        foreach (QueryParameter parameter in this.Parameters)
                        {
                            Label label2 = new Label();
                            label2.Text = parameter.OperandString;
                            label2.Location = new Point(12, y + 2);
                            label2.Size = new Size(150, 0x10);
                            MxTextBox box2 = new MxTextBox();
                            box2.Size = new Size(200, 20);
                            box2.Location = new Point(0xa2, y);
                            box2.FlatAppearance = true;
                            box2.AlwaysShowFocusCues = true;
                            y += 0x18;
                            this._previewTextBoxes[parameter.Name] = box2;
                            form.Controls.Add(label2);
                            form.Controls.Add(box2);
                        }
                        Button button = new Button();
                        button.Text = "OK";
                        button.Click += new EventHandler(this.OnOkButtonClick);
                        button.FlatStyle = FlatStyle.System;
                        button.Location = new Point(0xd0, y);
                        form.Controls.Add(button);
                        Button button2 = new Button();
                        button2.Text = "Cancel";
                        button2.Click += new EventHandler(this.OnCancelButtonClick);
                        button2.FlatStyle = FlatStyle.System;
                        button2.Location = new Point(0x120, y);
                        form.Controls.Add(button2);
                        form.AcceptButton = button;
                        form.CancelButton = button2;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            flag = true;
                            foreach (QueryParameter parameter2 in this.Parameters)
                            {
                                object text = ((MxTextBox) this._previewTextBoxes[parameter2.Name]).Text;
                                IDataParameter parameter3 = ((IDataProviderDatabase) this.Database).CreateParameter(parameter2.Name, parameter2.Type);
                                parameter3.Value = text;
                                selectCommand.Parameters.Add(parameter3);
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        connection.Open();
                        DbDataAdapter adapter = ((IDataProviderDatabase) this.Database).CreateAdapter(selectCommand);
                        DataSet dataSet = new DataSet();
                        bool flag2 = false;
                        if (adapter.Fill(dataSet) > 0)
                        {
                            this._previewGrid.DataSource = dataSet.Tables[0];
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                flag2 = true;
                            }
                        }
                        if (!flag2)
                        {
                            MessageBox.Show(base.WizardForm, "The query returned no matches.", "Query Builder", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    return;
                }
                catch (FormatException)
                {
                    MessageBox.Show(base.WizardForm, "One of your parameters was formatted incorrectly, please try again.", "Query Builder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(base.WizardForm, "There was a problem executing your query.\r\nPlease check your query and database and try again. Details:\r\n" + exception.Message, "Query Builder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                finally
                {
                    connection.Close();
                }
            }
            MessageBox.Show(base.WizardForm, "Preview is not supported on this type of query", "Query Builder", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this.QueryBuilder.Database;
            }
        }

        private ArrayList Parameters
        {
            get
            {
                return this.QueryBuilder.PreviewParameters;
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


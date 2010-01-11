namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Collections;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class QueryBuilder : CodeWizardForm
    {
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database _database;
        private string _methodName;
        private ArrayList _parameters;
        private ArrayList _previewParameters;
        private string _previewQuery;
        private string _query;
        private System.Type _returnType;
        private QueryBuilderType _type;

        public QueryBuilder(CodeWizard codeWizard, QueryBuilderType type) : base(codeWizard)
        {
            this._type = type;
            base.WizardPanels.Add(new SelectDatabasePanel(base.ServiceProvider));
            if (type == QueryBuilderType.Select)
            {
                base.WizardPanels.Add(new BuildSelectPanel(base.ServiceProvider));
                base.WizardPanels.Add(new QueryPreviewPanel(base.ServiceProvider));
                base.WizardPanels.Add(new NameSelectMethodPanel(base.ServiceProvider));
                this.Text = "SELECT Data Code Wizard";
            }
            else if (type == QueryBuilderType.Delete)
            {
                base.WizardPanels.Add(new BuildDeletePanel(base.ServiceProvider));
                base.WizardPanels.Add(new QueryPreviewPanel(base.ServiceProvider));
                base.WizardPanels.Add(new NameNonQueryMethodPanel(base.ServiceProvider, "MyDeleteMethod"));
                this.Text = "DELETE Data Code Wizard";
            }
            else if (type == QueryBuilderType.Update)
            {
                base.WizardPanels.Add(new BuildUpdatePanel(base.ServiceProvider));
                base.WizardPanels.Add(new QueryPreviewPanel(base.ServiceProvider));
                base.WizardPanels.Add(new NameNonQueryMethodPanel(base.ServiceProvider, "MyUpdateMethod"));
                this.Text = "UPDATE Data Code Wizard";
            }
            else if (type == QueryBuilderType.Insert)
            {
                base.WizardPanels.Add(new BuildInsertPanel(base.ServiceProvider));
                base.WizardPanels.Add(new NameNonQueryMethodPanel(base.ServiceProvider, "MyInsertMethod"));
                this.Text = "INSERT Data Code Wizard";
            }
            base.Size = new Size(510, 0x198);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(500, 0x16a);
            base.Name = "QueryBuilder";
            base.ResumeLayout(false);
        }

        protected override void OnCompleted()
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        internal bool ValidateIdentifier(string id)
        {
            try
            {
                base.CodeDomProvider.CreateGenerator().ValidateIdentifier(id);
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this._database;
            }
            set
            {
                this._database = value;
            }
        }

        internal string MethodName
        {
            get
            {
                if (this._methodName == null)
                {
                    return "UnnamedMethod";
                }
                return this._methodName;
            }
            set
            {
                this._methodName = value;
            }
        }

        public ArrayList Parameters
        {
            get
            {
                if (this._parameters == null)
                {
                    this._parameters = new ArrayList();
                }
                return this._parameters;
            }
        }

        public ArrayList PreviewParameters
        {
            get
            {
                if (this._previewParameters == null)
                {
                    this._previewParameters = new ArrayList();
                }
                return this._previewParameters;
            }
        }

        public string PreviewQuery
        {
            get
            {
                return this._previewQuery;
            }
            set
            {
                this._previewQuery = value;
            }
        }

        public string Query
        {
            get
            {
                if (this._query == null)
                {
                    return string.Empty;
                }
                return this._query;
            }
            set
            {
                this._query = value;
            }
        }

        internal System.Type ReturnType
        {
            get
            {
                if (this._returnType == null)
                {
                    return typeof(IDataReader);
                }
                return this._returnType;
            }
            set
            {
                this._returnType = value;
            }
        }
    }
}


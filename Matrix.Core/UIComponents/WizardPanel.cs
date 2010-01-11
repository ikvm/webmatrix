namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.ComponentModel;

    public class WizardPanel : MxUserControl
    {
        private string _caption;
        private string _description;
        private Microsoft.Matrix.UIComponents.WizardForm _wizardForm;

        public WizardPanel()
        {
        }

        public WizardPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected internal virtual void OnActivated()
        {
        }

        protected internal virtual void OnDeactivating()
        {
        }

        protected internal virtual bool OnNext()
        {
            return true;
        }

        protected internal virtual void OnPrevious()
        {
        }

        internal void SetWizardForm(Microsoft.Matrix.UIComponents.WizardForm wizardForm)
        {
            this._wizardForm = wizardForm;
        }

        protected void UpdateWizardState()
        {
            if (this._wizardForm != null)
            {
                this._wizardForm.UpdateWizardState();
            }
        }

        [DefaultValue("")]
        public string Caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                this._caption = value;
                if (this._wizardForm != null)
                {
                    this._wizardForm.UpdateWizardText();
                }
            }
        }

        [DefaultValue("")]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
                if (this._wizardForm != null)
                {
                    this._wizardForm.UpdateWizardText();
                }
            }
        }

        public virtual bool FinishEnabled
        {
            get
            {
                return false;
            }
        }

        protected bool IsFirst
        {
            get
            {
                return ((this._wizardForm != null) && (this._wizardForm.WizardPanels.IndexOf(this) == 0));
            }
        }

        protected bool IsLast
        {
            get
            {
                return ((this._wizardForm != null) && (this._wizardForm.WizardPanels.IndexOf(this) == (this._wizardForm.WizardPanels.Count - 1)));
            }
        }

        public virtual bool NextEnabled
        {
            get
            {
                return true;
            }
        }

        protected Microsoft.Matrix.UIComponents.WizardForm WizardForm
        {
            get
            {
                return this._wizardForm;
            }
        }
    }
}


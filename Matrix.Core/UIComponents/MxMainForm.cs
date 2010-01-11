namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix.Utility;
    using System;
    using System.ComponentModel.Design;

    public abstract class MxMainForm : MxForm, ICommandHandler
    {
        private Microsoft.Matrix.UIComponents.CommandBar _commandBar;
        private Microsoft.Matrix.UIComponents.CommandManager _commandManager;
        private MxStatusBar _statusBar;

        public MxMainForm(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._commandManager = new Microsoft.Matrix.UIComponents.CommandManager(serviceProvider, this);
            this._commandManager.SetCommandHelpTextChangedHandler(new EventHandler(this.OnCommandHelpTextChanged));
            IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                service.AddService(typeof(ICommandManager), this._commandManager);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    service.RemoveService(typeof(ICommandManager));
                    service.RemoveService(typeof(IStatusBar));
                }
                this._commandManager.Dispose();
            }
            base.Dispose(disposing);
        }

        protected virtual bool HandleCommand(Command command)
        {
            return false;
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            return this.HandleCommand(command);
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            return this.UpdateCommand(command);
        }

        private void OnCommandHelpTextChanged(object sender, EventArgs e)
        {
            ((IStatusBar) this._statusBar).SetText(this._commandManager.CommandHelpText);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AsyncTaskManager.RegisterUIThread(this);
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            ((ICommandManager) this._commandManager).ResumeCommandUpdate();
        }

        protected virtual bool UpdateCommand(Command command)
        {
            return false;
        }

        public Microsoft.Matrix.UIComponents.CommandBar CommandBar
        {
            get
            {
                return this._commandBar;
            }
            set
            {
                this._commandBar = value;
            }
        }

        public Microsoft.Matrix.UIComponents.CommandManager CommandManager
        {
            get
            {
                return this._commandManager;
            }
        }

        public MxStatusBar StatusBar
        {
            get
            {
                return this._statusBar;
            }
            set
            {
                this._statusBar = value;
                IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    if (this._statusBar != null)
                    {
                        service.AddService(typeof(IStatusBar), this._statusBar);
                    }
                    else
                    {
                        service.RemoveService(typeof(IStatusBar));
                    }
                }
            }
        }
    }
}


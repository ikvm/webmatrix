namespace Microsoft.Matrix.WebIDE
{
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Windows.Forms.Design;

    internal sealed class ApplicationPackage : IPackage, IDisposable, IApplicationPackage, ICommandHandler
    {
        private IServiceProvider _serviceProvider;

        OptionsPage[] IPackage.GetOptionsPages()
        {
            return new OptionsPage[] { new ApplicationOptionsPage(this._serviceProvider) };
        }

        void IPackage.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            ICommandManager service = (ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager));
            if (service != null)
            {
                service.AddGlobalCommandHandler(this);
            }
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            if ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 0x259))
            {
                this.OnCommandHelpAbout();
                return true;
            }
            return false;
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            return ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 0x259));
        }

        private void OnCommandHelpAbout()
        {
            IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
            if (service != null)
            {
                service.ShowDialog(new AboutDialog());
            }
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }
    }
}


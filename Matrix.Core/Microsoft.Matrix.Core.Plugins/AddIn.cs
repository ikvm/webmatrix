namespace Microsoft.Matrix.Core.Plugins
{
    using System;
    using System.Windows.Forms.Design;

    public abstract class AddIn : Plugin
    {
        protected AddIn()
        {
        }

        protected virtual AddInForm CreateAddInForm()
        {
            return null;
        }

        protected virtual void Run()
        {
            IUIService service = (IUIService) base.GetService(typeof(IUIService));
            AddInForm form = this.CreateAddInForm();
            if (form != null)
            {
                service.ShowDialog(form);
            }
        }

        protected sealed override object RunPlugin(object initializationData)
        {
            this.Run();
            return null;
        }
    }
}


namespace Microsoft.Matrix.Core.Plugins
{
    using System;

    internal sealed class AddInHost : PluginHost
    {
        public AddInHost(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public void RunAddIn(string addInTypeName)
        {
            base.RunPlugin(addInTypeName);
        }

        protected override string Description
        {
            get
            {
                return "Run Add-in";
            }
        }

        protected override Type PluginBaseType
        {
            get
            {
                return typeof(AddIn);
            }
        }
    }
}


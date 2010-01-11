namespace Microsoft.Matrix.Core.Plugins
{
    using System;
    using System.CodeDom.Compiler;

    internal sealed class CodeWizardHost : PluginHost
    {
        public CodeWizardHost(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public string RunCodeWizard(string codeWizardTypeName, CodeDomProvider codeDomProvider)
        {
            return (string) base.RunPlugin(codeWizardTypeName, codeDomProvider);
        }

        protected override string Description
        {
            get
            {
                return "Code Wizard";
            }
        }

        protected override Type PluginBaseType
        {
            get
            {
                return typeof(CodeWizard);
            }
        }
    }
}


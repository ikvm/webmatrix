namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Core.Plugins;
    using System;

    [Plugin("UPDATE Data Method", "Constructs a UPDATE statement to update data from a database.")]
    public sealed class UpdateCodeWizard : CodeWizard
    {
        protected override string Run()
        {
            return SqlCodeWizard.Run(this, base.ServiceProvider, base.CodeDomProvider, QueryBuilderType.Update);
        }
    }
}


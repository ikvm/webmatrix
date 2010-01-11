namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Core.Plugins;
    using System;

    [Plugin("SELECT Data Method", "Constructs a SELECT statement to query a database.")]
    public sealed class SelectCodeWizard : CodeWizard
    {
        protected override string Run()
        {
            return SqlCodeWizard.Run(this, base.ServiceProvider, base.CodeDomProvider, QueryBuilderType.Select);
        }
    }
}


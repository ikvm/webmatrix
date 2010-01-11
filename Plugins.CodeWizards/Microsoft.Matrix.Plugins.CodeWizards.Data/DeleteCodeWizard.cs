namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Core.Plugins;
    using System;

    [Plugin("DELETE Data Method", "Constructs a DELETE statement to delete data from a database.")]
    public sealed class DeleteCodeWizard : CodeWizard
    {
        protected override string Run()
        {
            return SqlCodeWizard.Run(this, base.ServiceProvider, base.CodeDomProvider, QueryBuilderType.Delete);
        }
    }
}


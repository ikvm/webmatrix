namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Core.Plugins;
    using System;

    [Plugin("INSERT Data Method", "Constructs an INSERT statement to add data to a database.")]
    public sealed class InsertCodeWizard : CodeWizard
    {
        protected override string Run()
        {
            return SqlCodeWizard.Run(this, base.ServiceProvider, base.CodeDomProvider, QueryBuilderType.Insert);
        }
    }
}


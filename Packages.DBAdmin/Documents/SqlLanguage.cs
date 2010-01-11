namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    public sealed class SqlLanguage : DocumentLanguage
    {
        public static readonly SqlLanguage Instance = new SqlLanguage();

        private SqlLanguage() : base("sql")
        {
        }
    }
}


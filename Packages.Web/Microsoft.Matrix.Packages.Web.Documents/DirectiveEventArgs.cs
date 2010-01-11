namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;

    public class DirectiveEventArgs
    {
        private Microsoft.Matrix.Packages.Web.Documents.Directive _directive;

        public DirectiveEventArgs(Microsoft.Matrix.Packages.Web.Documents.Directive directive)
        {
            this._directive = directive;
        }

        public Microsoft.Matrix.Packages.Web.Documents.Directive Directive
        {
            get
            {
                return this._directive;
            }
        }
    }
}


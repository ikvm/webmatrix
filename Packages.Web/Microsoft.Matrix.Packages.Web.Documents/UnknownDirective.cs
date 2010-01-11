namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;

    public class UnknownDirective : Directive
    {
        private string _name;

        public UnknownDirective(string name)
        {
            this._name = name;
        }

        public override string DirectiveName
        {
            get
            {
                return this._name;
            }
        }
    }
}


namespace Microsoft.Matrix.Core.Packages
{
    using System;
    using System.Configuration;

    public class PackageListSectionHandler : DictionarySectionHandler
    {
        protected override string KeyAttributeName
        {
            get
            {
                return "package";
            }
        }

        protected override string ValueAttributeName
        {
            get
            {
                return "type";
            }
        }
    }
}


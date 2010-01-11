namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;

    internal sealed class TDTagInfo : TagInfo
    {
        public TDTagInfo() : base("td", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Other)
        {
        }

        public override bool CanContainTag(TagInfo info)
        {
            return ((info.Type == ElementType.Any) || ((info.Type == ElementType.Inline) | (info.Type == ElementType.Block)));
        }
    }
}


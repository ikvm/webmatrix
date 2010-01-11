namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;

    internal sealed class TRTagInfo : TagInfo
    {
        public TRTagInfo() : base("tr", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Other)
        {
        }

        public override bool CanContainTag(TagInfo info)
        {
            return ((info.Type == ElementType.Any) || (info.TagName.ToLower().Equals("th") | info.TagName.ToLower().Equals("td")));
        }
    }
}


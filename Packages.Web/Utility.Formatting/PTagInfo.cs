namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;

    internal sealed class PTagInfo : TagInfo
    {
        public PTagInfo() : base("p", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block)
        {
        }

        public override bool CanContainTag(TagInfo info)
        {
            return ((info.Type == ElementType.Any) || (((info.Type == ElementType.Inline) | info.TagName.ToLower().Equals("table")) | info.TagName.ToLower().Equals("hr")));
        }
    }
}


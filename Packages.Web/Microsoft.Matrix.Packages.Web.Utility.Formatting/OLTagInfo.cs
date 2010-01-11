namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;

    internal sealed class OLTagInfo : TagInfo
    {
        public OLTagInfo() : base("ol", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block)
        {
        }

        public override bool CanContainTag(TagInfo info)
        {
            return ((info.Type == ElementType.Any) || info.TagName.ToLower().Equals("li"));
        }
    }
}


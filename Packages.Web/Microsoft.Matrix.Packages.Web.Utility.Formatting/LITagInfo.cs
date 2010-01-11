namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;

    internal sealed class LITagInfo : TagInfo
    {
        public LITagInfo() : base("li", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.CarryThrough)
        {
        }

        public override bool CanContainTag(TagInfo info)
        {
            return ((info.Type == ElementType.Any) || ((info.Type == ElementType.Inline) | (info.Type == ElementType.Block)));
        }
    }
}


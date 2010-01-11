namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Flags, Editor(typeof(FlagsEnumEditor), typeof(UITypeEditor))]
    public enum TextDecoration
    {
        None = 1,
        NotSet = 0,
        Overline = 8,
        Strikethrough = 4,
        Underline = 2
    }
}


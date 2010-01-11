namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    public sealed class ListItemElement : StyledElement
    {
        internal ListItemElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("The type of bullets to use in an unordered list or numbering scheme in an ordered list"), DefaultValue(0), Category("Appearance")]
        public ListType type
        {
            get
            {
                switch (base.GetStringAttribute("type"))
                {
                    case "1":
                        return ListType.Numeric;

                    case "a":
                        return ListType.LowerCaseAlphabetic;

                    case "A":
                        return ListType.UpperCaseAlphabetic;

                    case "i":
                        return ListType.LowerCaseRoman;

                    case "I":
                        return ListType.UpperCaseRoman;

                    case "Disk":
                        return ListType.Disk;

                    case "Circle":
                        return ListType.Circle;

                    case "Square":
                        return ListType.Square;
                }
                return ListType.Default;
            }
            set
            {
                string str;
                switch (value)
                {
                    case ListType.Numeric:
                        str = "1";
                        break;

                    case ListType.LowerCaseAlphabetic:
                        str = "a";
                        break;

                    case ListType.UpperCaseAlphabetic:
                        str = "A";
                        break;

                    case ListType.LowerCaseRoman:
                        str = "i";
                        break;

                    case ListType.UpperCaseRoman:
                        str = "I";
                        break;

                    case ListType.Disk:
                        str = "Disk";
                        break;

                    case ListType.Circle:
                        str = "Circle";
                        break;

                    case ListType.Square:
                        str = "Square";
                        break;

                    default:
                        str = string.Empty;
                        break;
                }
                if (str.Length == 0)
                {
                    base.RemoveAttribute("type");
                }
                else
                {
                    base.SetAttribute("type", str);
                }
            }
        }
    }
}


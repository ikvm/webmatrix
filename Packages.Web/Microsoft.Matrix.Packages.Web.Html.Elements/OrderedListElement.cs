namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    public sealed class OrderedListElement : StyledElement
    {
        internal OrderedListElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("The type of numerals to display in an ordered list"), Category("Appearance"), DefaultValue(0)]
        public OrderedListType type
        {
            get
            {
                switch (base.GetStringAttribute("type"))
                {
                    case "1":
                        return OrderedListType.Numeric;

                    case "a":
                        return OrderedListType.LowerCaseAlphabetic;

                    case "A":
                        return OrderedListType.UpperCaseAlphabetic;

                    case "i":
                        return OrderedListType.LowerCaseRoman;

                    case "I":
                        return OrderedListType.UpperCaseRoman;
                }
                return OrderedListType.Default;
            }
            set
            {
                string str = string.Empty;
                switch (value)
                {
                    case OrderedListType.Numeric:
                        str = "1";
                        break;

                    case OrderedListType.LowerCaseAlphabetic:
                        str = "a";
                        break;

                    case OrderedListType.UpperCaseAlphabetic:
                        str = "A";
                        break;

                    case OrderedListType.LowerCaseRoman:
                        str = "i";
                        break;

                    case OrderedListType.UpperCaseRoman:
                        str = "I";
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


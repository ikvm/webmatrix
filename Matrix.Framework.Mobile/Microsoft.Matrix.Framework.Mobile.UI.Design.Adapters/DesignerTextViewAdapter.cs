namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerTextViewAdapter : HtmlControlAdapter
    {
        public DesignerTextViewAdapter(TextView textView)
        {
            base.set_Control(textView);
        }

        private string FilterTags(string text)
        {
            StringBuilder builder = new StringBuilder();
            int length = text.Length;
            int num3 = 0;
            bool flag = false;
            CursorStatus outsideTag = CursorStatus.OutsideTag;
            string str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                switch (text[i])
                {
                    case '<':
                    {
                        CursorStatus status2 = outsideTag;
                        if (status2 == CursorStatus.OutsideTag)
                        {
                            outsideTag = CursorStatus.InsideTagName;
                            num3 = i;
                        }
                        continue;
                    }
                    case '=':
                    {
                        switch (outsideTag)
                        {
                            case CursorStatus.OutsideTag:
                                goto Label_016E;

                            case CursorStatus.InsideTagName:
                            {
                                continue;
                            }
                        }
                        continue;
                    }
                    case '>':
                    {
                        switch (outsideTag)
                        {
                            case CursorStatus.OutsideTag:
                                goto Label_0372;

                            case CursorStatus.InsideTagName:
                            case CursorStatus.InsideAttributeName:
                            case CursorStatus.ExpectingAttributeValue:
                                goto Label_02CB;

                            case CursorStatus.InsideAttributeValue:
                            {
                                continue;
                            }
                        }
                        continue;
                    }
                    case '/':
                    {
                        switch (outsideTag)
                        {
                            case CursorStatus.OutsideTag:
                            {
                                builder.Append(text[i]);
                                continue;
                            }
                            case CursorStatus.InsideTagName:
                            {
                                str = text.Substring(num3 + 1, (i - num3) - 1).Trim().ToUpper(CultureInfo.InvariantCulture);
                                if (str.Trim().Length > 0)
                                {
                                    outsideTag = CursorStatus.InsideAttributeName;
                                }
                                continue;
                            }
                        }
                        continue;
                    }
                    case '"':
                    {
                        switch (outsideTag)
                        {
                            case CursorStatus.OutsideTag:
                            {
                                builder.Append(text[i]);
                                continue;
                            }
                            case CursorStatus.InsideTagName:
                            case CursorStatus.InsideAttributeName:
                            {
                                continue;
                            }
                            case CursorStatus.InsideAttributeValue:
                            {
                                if ((text[i - 1] != '\\') && flag)
                                {
                                    outsideTag = CursorStatus.InsideAttributeName;
                                }
                                continue;
                            }
                            case CursorStatus.ExpectingAttributeValue:
                            {
                                outsideTag = CursorStatus.InsideAttributeValue;
                                flag = true;
                                continue;
                            }
                        }
                        continue;
                    }
                    case '\'':
                    {
                        switch (outsideTag)
                        {
                            case CursorStatus.OutsideTag:
                            {
                                builder.Append(text[i]);
                                continue;
                            }
                            case CursorStatus.InsideTagName:
                            case CursorStatus.InsideAttributeName:
                            {
                                continue;
                            }
                            case CursorStatus.InsideAttributeValue:
                            {
                                if ((text[i - 1] != '\\') && !flag)
                                {
                                    outsideTag = CursorStatus.InsideAttributeName;
                                }
                                continue;
                            }
                            case CursorStatus.ExpectingAttributeValue:
                            {
                                outsideTag = CursorStatus.InsideAttributeValue;
                                flag = false;
                                continue;
                            }
                        }
                        continue;
                    }
                    default:
                        goto Label_0382;
                }
                outsideTag = CursorStatus.ExpectingAttributeValue;
                continue;
            Label_016E:
                builder.Append(text[i]);
                continue;
            Label_02CB:
                if (outsideTag == CursorStatus.InsideTagName)
                {
                    str = text.Substring(num3 + 1, (i - num3) - 1).Trim().ToUpper(CultureInfo.InvariantCulture);
                }
                outsideTag = CursorStatus.OutsideTag;
                switch (str)
                {
                    case "A":
                        builder.Append("<A HREF=\"\">");
                        break;

                    case "/A":
                    case "B":
                    case "/B":
                    case "BR":
                    case "/BR":
                    case "I":
                    case "/I":
                    case "P":
                    case "/P":
                        builder.Append("<" + str + ">");
                        break;
                }
                str = string.Empty;
                continue;
            Label_0372:
                builder.Append(text[i]);
                continue;
            Label_0382:
                if (char.IsWhiteSpace(text[i]))
                {
                    switch (outsideTag)
                    {
                        case CursorStatus.OutsideTag:
                        {
                            builder.Append(text[i]);
                            continue;
                        }
                        case CursorStatus.InsideTagName:
                        {
                            outsideTag = CursorStatus.InsideAttributeName;
                            str = text.Substring(num3 + 1, (i - num3) - 1).Trim().ToUpper(CultureInfo.InvariantCulture);
                            continue;
                        }
                    }
                }
                else
                {
                    switch (outsideTag)
                    {
                        case CursorStatus.OutsideTag:
                        {
                            builder.Append(text[i]);
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public override unsafe void Render(HtmlMobileTextWriter writer)
        {
            Alignment alignment = *((Alignment*) base.get_Style().get_Item(Style.AlignmentKey, true));
            Wrapping wrapping = *((Wrapping*) base.get_Style().get_Item(Style.WrappingKey, true));
            bool flag = (wrapping == 1) || (wrapping == 0);
            string str = "100%";
            Utils.EnterZeroFontSizeTag(writer);
            writer.WriteBeginTag("div");
            if (!flag)
            {
                writer.WriteAttribute("style", "overflow-x:hidden;width:" + str);
            }
            else
            {
                writer.WriteAttribute("style", "word-wrap:break-word;width:" + str);
            }
            if (alignment != null)
            {
                writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
            }
            writer.Write(">");
            Utils.WriteCssStyleText(writer, base.get_Style(), null, this.FilterTags(((TextView) base.get_Control()).get_Text().Trim()), false);
            writer.WriteEndTag("div");
            Utils.ExitZeroFontSizeTag(writer);
        }

        public override MobileCapabilities Device
        {
            get
            {
                return Utils.DesignerCapabilities;
            }
        }

        private enum CursorStatus
        {
            OutsideTag,
            InsideTagName,
            InsideAttributeName,
            InsideAttributeValue,
            ExpectingAttributeValue
        }
    }
}


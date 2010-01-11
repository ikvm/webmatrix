namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class ElementWrapperTable
    {
        private static IDictionary wrapperTable = new Hashtable();

        static ElementWrapperTable()
        {
            wrapperTable["font"] = typeof(FontElement);
            wrapperTable["p"] = typeof(ParagraphElement);
            wrapperTable["button"] = typeof(ButtonElement);
            wrapperTable["inputbutton"] = typeof(InputButtonElement);
            wrapperTable["text"] = typeof(InputTextElement);
            wrapperTable["textarea"] = typeof(TextAreaElement);
            wrapperTable["radio"] = typeof(InputRadioElement);
            wrapperTable["file"] = typeof(InputFileElement);
            wrapperTable["password"] = typeof(InputPasswordElement);
            wrapperTable["checkbox"] = typeof(InputCheckboxElement);
            wrapperTable["select"] = typeof(SelectElement);
            wrapperTable["hidden"] = typeof(InputHiddenElement);
            wrapperTable["table"] = typeof(TableElement);
            wrapperTable["body"] = typeof(BodyElement);
            wrapperTable["label"] = typeof(LabelElement);
            wrapperTable["submit"] = typeof(InputSubmitElement);
            wrapperTable["reset"] = typeof(InputResetElement);
            wrapperTable["image"] = typeof(InputImageElement);
            wrapperTable["tr"] = typeof(TableRowElement);
            wrapperTable["td"] = typeof(TableCellElement);
            wrapperTable["div"] = typeof(DivElement);
            wrapperTable["span"] = typeof(SpanElement);
            wrapperTable["a"] = typeof(AnchorElement);
            wrapperTable["fieldset"] = typeof(FieldSetElement);
            wrapperTable["hr"] = typeof(HorizontalRuleElement);
            wrapperTable["img"] = typeof(ImageElement);
            wrapperTable["iframe"] = typeof(IFrameElement);
            wrapperTable["form"] = typeof(FormElement);
            wrapperTable["ul"] = typeof(UnorderedListElement);
            wrapperTable["ol"] = typeof(OrderedListElement);
            wrapperTable["li"] = typeof(ListItemElement);
            wrapperTable["legend"] = typeof(LegendElement);
        }

        private ElementWrapperTable()
        {
        }

        public static Element GetWrapper(Interop.IHTMLElement element, HtmlControl owner)
        {
            Type type;
            Element element2;
            string str = element.GetTagName().ToLower();
            if (!str.Equals("input"))
            {
                type = (Type) wrapperTable[str];
            }
            else
            {
                object[] pvars = new object[1];
                element.GetAttribute("type", 1, pvars);
                string str2 = pvars[0].ToString().ToLower();
                if (str2.Equals("button"))
                {
                    type = (Type) wrapperTable["inputbutton"];
                }
                else
                {
                    type = (Type) wrapperTable[str2];
                }
            }
            if (type != null)
            {
                element2 = (Element) type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Interop.IHTMLElement) }, null).Invoke(new object[] { element });
            }
            else
            {
                element2 = new Element(element);
            }
            element2.Owner = owner;
            return element2;
        }
    }
}


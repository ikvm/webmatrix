namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;
    using System.Web.UI.WebControls;

    internal sealed class Utils
    {
        private static Type _designerCapabilitiesType = Type.GetType("System.Web.UI.Design.MobileControls.DesignerCapabilities," + MobileAssemblyFullName);
        private static Type _designerTextWriterType = Type.GetType("System.Web.UI.Design.MobileControls.Adapters.DesignerTextWriter," + MobileAssemblyFullName);
        private static readonly string MobileAssemblyFullName = typeof(MobileControl).Assembly.FullName;

        private Utils()
        {
        }

        internal static void ApplyStyleToWebControl(Style style, WebControl webControl)
        {
            typeof(Style).InvokeMember("ApplyTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, style, new object[] { webControl });
        }

        internal static void EnterZeroFontSizeTag(HtmlMobileTextWriter writer)
        {
            _designerTextWriterType.InvokeMember("EnterZeroFontSizeTag", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, writer, null);
        }

        internal static void ExitZeroFontSizeTag(HtmlMobileTextWriter writer)
        {
            _designerTextWriterType.InvokeMember("ExitZeroFontSizeTag", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, writer, null);
        }

        internal static void WriteCssStyleText(HtmlMobileTextWriter writer, Style style, string additionalStyle, string text, bool encodeText)
        {
            _designerTextWriterType.InvokeMember("WriteCssStyleText", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, writer, new object[] { style, additionalStyle, text, encodeText });
        }

        internal static unsafe void WriteDesignerStyleAttributes(HtmlMobileTextWriter writer, MobileControl control, Style style)
        {
            Alignment alignment = *((Alignment*) style.get_Item(Style.AlignmentKey, true));
            Wrapping wrapping = *((Wrapping*) style.get_Item(Style.WrappingKey, true));
            Color c = (Color) style.get_Item(Style.BackColorKey, true);
            bool flag = alignment != 0;
            bool flag2 = (wrapping == 1) || (wrapping == 0);
            string str = "100%";
            if (!flag2)
            {
                writer.Write(" style=\"overflow-x:hidden;width:" + str);
            }
            else
            {
                writer.Write(" style=\"word-wrap:break-word;overflow-x:hidden;width:" + str);
            }
            if (c != Color.Empty)
            {
                writer.Write(";background-color:" + ColorTranslator.ToHtml(c));
            }
            if (flag)
            {
                writer.Write(";text-align:" + Enum.GetName(typeof(Alignment), alignment));
            }
        }

        internal static void WriteStyleAttribute(HtmlMobileTextWriter writer, Style style, string additionalStyle)
        {
            _designerTextWriterType.InvokeMember("WriteStyleAttribute", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, writer, new object[] { style, additionalStyle });
        }

        internal static MobileCapabilities DesignerCapabilities
        {
            get
            {
                return (MobileCapabilities) _designerCapabilitiesType.InvokeMember("Instance", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, null, null);
            }
        }
    }
}


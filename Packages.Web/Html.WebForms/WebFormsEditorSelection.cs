namespace Microsoft.Matrix.Packages.Web.Html.WebForms
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using System;
    using System.ComponentModel.Design;
    using System.Web.UI;
    using System.Web.UI.Design;

    internal sealed class WebFormsEditorSelection : EditorSelection
    {
        public WebFormsEditorSelection(WebFormsEditor editor) : base(editor)
        {
        }

        protected override object CreateElementWrapper(Interop.IHTMLElement element)
        {
            object[] pvars = new object[1];
            element.GetAttribute("RuntimeComponent", 1, pvars);
            object obj2 = pvars[0];
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return obj2;
            }
            return base.CreateElementWrapper(element);
        }

        protected override Interop.IHTMLElement GetIHtmlElement(object o)
        {
            if (!(o is Control))
            {
                return base.GetIHtmlElement(o);
            }
            IDesignerHost service = (IDesignerHost) base.Editor.ServiceProvider.GetService(typeof(IDesignerHost));
            ControlDesigner designer = (ControlDesigner) service.GetDesigner((Control) o);
            if (designer != null)
            {
                Behavior behavior = (Behavior) designer.Behavior;
                if (behavior != null)
                {
                    return behavior.Element;
                }
            }
            return null;
        }

        protected override bool IsSelectableElement(Element element)
        {
            if (element == null)
            {
                return false;
            }
            if (((WebFormsEditor) base.Editor).EditorMode == WebFormsEditorMode.Template)
            {
                if (element is DivElement)
                {
                    element = element.GetParent();
                }
                if ((element is SpanElement) && (element.GetParent() is BodyElement))
                {
                    return false;
                }
            }
            return base.IsSelectableElement(element);
        }
    }
}


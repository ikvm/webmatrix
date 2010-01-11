namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Drawing;

    internal class ClassViewPage : Page
    {
        private Type _type;

        protected virtual void GenerateDocumentation(Element documentationContainer)
        {
            TextElement element = new TextElement("[Doc-comment support is not yet implemented.]");
            element.ForeColor = Color.Gray;
            documentationContainer.Elements.Add(element);
        }

        protected override bool OnElementClick(ElementEventArgs e)
        {
            ToggleImageElement element = e.Element as ToggleImageElement;
            if (element == null)
            {
                return base.OnElementClick(e);
            }
            string userData = (string) element.UserData;
            Element element2 = base.Elements[userData];
            if (element2 != null)
            {
                base.BeginBatchUpdate();
                try
                {
                    element2.Visible = !element2.Visible;
                    element.Condition = element2.Visible;
                    if (userData.Equals("Documentation"))
                    {
                        Element documentationContainer = element2;
                        if (documentationContainer.Elements.Count == 0)
                        {
                            this.GenerateDocumentation(documentationContainer);
                        }
                    }
                }
                finally
                {
                    base.EndBatchUpdate();
                }
            }
            return false;
        }

        public Type CurrentType
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}


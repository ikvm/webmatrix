namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Web.UI;

    public class WebFormsDesignerHost : DesignerHost
    {
        public WebFormsDesignerHost(Document document) : base(document)
        {
        }

        protected override IDesigner CreateComponentDesigner(IComponent component, Type designerType)
        {
            if (component is Page)
            {
                return new WebFormsRootDesigner();
            }
            if (!(component is UserControl))
            {
                return base.CreateComponentDesigner(component, designerType);
            }
            if (designerType == typeof(IRootDesigner))
            {
                return new WebFormsRootDesigner();
            }
            return new MxUserControlDesigner();
        }
    }
}


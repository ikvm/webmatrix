namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Packages.Web.Mobile.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Web.UI.MobileControls;

    public class MobileDesignerHost : WebFormsDesignerHost
    {
        private static Hashtable _designerTable = new Hashtable(50);

        static MobileDesignerHost()
        {
            _designerTable[typeof(AdRotator)] = typeof(AdRotatorDesigner);
            _designerTable[typeof(AspxDocument)] = typeof(MobileWebFormsDocumentDesigner);
            _designerTable[typeof(Calendar)] = typeof(CalendarDesigner);
            _designerTable[typeof(Command)] = typeof(CommandDesigner);
            _designerTable[typeof(CompareValidator)] = typeof(BaseValidatorDesigner);
            _designerTable[typeof(CustomValidator)] = typeof(BaseValidatorDesigner);
            _designerTable[typeof(DeviceSpecific)] = typeof(DeviceSpecificDesigner);
            _designerTable[typeof(Form)] = typeof(FormDesigner);
            _designerTable[typeof(Image)] = typeof(ImageDesigner);
            _designerTable[typeof(Label)] = typeof(LabelDesigner);
            _designerTable[typeof(Link)] = typeof(LinkDesigner);
            _designerTable[typeof(List)] = typeof(ListDesigner);
            _designerTable[typeof(Panel)] = typeof(PanelDesigner);
            _designerTable[typeof(PhoneCall)] = typeof(PhoneCallDesigner);
            _designerTable[typeof(ObjectList)] = typeof(ObjectListDesigner);
            _designerTable[typeof(RangeValidator)] = typeof(BaseValidatorDesigner);
            _designerTable[typeof(RegularExpressionValidator)] = typeof(BaseValidatorDesigner);
            _designerTable[typeof(RequiredFieldValidator)] = typeof(BaseValidatorDesigner);
            _designerTable[typeof(SelectionList)] = typeof(SelectionListDesigner);
            _designerTable[typeof(StyleSheet)] = typeof(StyleSheetDesigner);
            _designerTable[typeof(TextBox)] = typeof(TextBoxDesigner);
            _designerTable[typeof(TextView)] = typeof(TextViewDesigner);
            _designerTable[typeof(ValidationSummary)] = typeof(ValidationSummaryDesigner);
        }

        public MobileDesignerHost(Document document) : base(document)
        {
        }

        protected override IDesigner CreateComponentDesigner(IComponent component, Type designerType)
        {
            Type type = _designerTable[component.GetType()] as Type;
            if (type != null)
            {
                return (IDesigner) Activator.CreateInstance(type);
            }
            return base.CreateComponentDesigner(component, designerType);
        }
    }
}


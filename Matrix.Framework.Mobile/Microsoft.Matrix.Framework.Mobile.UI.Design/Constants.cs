namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Drawing;

    internal sealed class Constants
    {
        internal static readonly string BaseTemplatedMobileComponentEditorTemplateModeErrorMessage = MobileResource.GetString("BaseTemplatedMobileComponentEditor_TemplateModeErrorMessage");
        internal static readonly string BaseTemplatedMobileComponentEditorTemplateModeErrorTitle = MobileResource.GetString("BaseTemplatedMobileComponentEditor_TemplateModeErrorTitle");
        internal static readonly string ComponentModelAssemblyFullName = typeof(PropertyDescriptor).Assembly.FullName;
        internal static readonly int ControlMaxsizeAtToplevel = 0x129;
        internal static readonly string ControlSizeAtToplevelInErrormode = "300px";
        internal static readonly string ControlSizeAtToplevelInNonErrorMode = (ControlMaxsizeAtToplevel.ToString() + "px");
        internal static readonly string ControlSizeInContainer = "100%";
        internal static readonly string DefaultErrorDesignTimeHTML = "\r\n                <table cellpadding=2 cellspacing=0 width='{4}' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                    <tr><td><span style='font-weight:bold'>&nbsp;{0}</span> - {1}</td></tr>\r\n                    <tr><td>\r\n                        <table style='font-family:tahoma;font-size:8pt;color:window;background-color:ButtonShadow'>\r\n                            <tr>\r\n                                <td valign='top'><img src={3} /></td>\r\n                                <td width='100%'>{2}</td>\r\n                            </tr>\r\n                        </table>\r\n                    </td></tr>\r\n                </table>\r\n             ";
        internal static readonly string DeviceSpecificDuplicateWarningMessage = MobileResource.GetString("DeviceSpecific_DuplicateWarningMessage");
        internal static readonly Bitmap ErrorIcon = new Icon(typeof(IMobileDesigner), "Error.ico").ToBitmap();
        internal static readonly string ErrorIconUrl = (ResourceDllUrl + "//ERROR_GIF");
        internal static readonly string FormPanelContainmentErrorMessage = MobileResource.GetString("MobileControl_FormPanelContainmentErrorMessage");
        internal static readonly Bitmap InfoIcon = new Icon(typeof(IMobileDesigner), "Info.ico").ToBitmap();
        internal static readonly string InfoIconUrl = (ResourceDllUrl + "//INFO_GIF");
        internal static readonly string MobileAssemblyFullName = typeof(MobileControl).Assembly.FullName;
        internal static readonly string MobilePageErrorMessage = MobileResource.GetString("MobileControl_MobilePageErrorMessage");
        internal static readonly string PropertyBuilderVerb = MobileResource.GetString("PropertyBuilderVerb");
        internal static readonly string ReflectPropertyDescriptorTypeFullName = "System.ComponentModel.ReflectPropertyDescriptor";
        internal static readonly string ResourceDllUrl = ("res://" + typeof(MobileControl).Module.FullyQualifiedName);
        internal static readonly string StrictlyFormPanelContainmentErrorMessage = MobileResource.GetString("MobileControl_StrictlyFormPanelContainmentErrorMessage");
        internal static readonly string StyleSheetDefaultMessage = MobileResource.GetString("StyleSheet_DefaultMessage");
        internal static readonly string StyleSheetPropNotSet = MobileResource.GetString("StyleSheet_PropNotSet");
        internal static readonly string StyleSheetStylesEditorVerb = MobileResource.GetString("StyleSheet_StylesEditorVerb");
        internal static readonly string StyleSheetTemplateStyleDescription = MobileResource.GetString("StyleSheet_TemplateStyleDescription");
        internal static readonly string TemplateFrameContentTemplate = MobileResource.GetString("TemplateFrame_ContentTemplate");
        internal static readonly string TemplateFrameHeaderFooterTemplates = MobileResource.GetString("TemplateFrame_HeaderFooterTemplates");
        internal static readonly string TemplateFrameItemTemplates = MobileResource.GetString("TemplateFrame_ItemTemplates");
        internal static readonly string TemplateFrameSeparatorTemplate = MobileResource.GetString("TemplateFrame_SeparatorTemplate");
        internal static readonly int TemplateWidth = 0x113;
        internal static readonly string TopPageContainmentErrorMessage = MobileResource.GetString("MobileControl_TopPageContainmentErrorMessage");
        internal static readonly string UserControlWarningMessage = MobileResource.GetString("MobileControl_UserControlWarningMessage");
        internal static readonly string ValidationSummaryErrorMessage = MobileResource.GetString("ValidationSummary_ErrorMessage");

        private Constants()
        {
        }
    }
}


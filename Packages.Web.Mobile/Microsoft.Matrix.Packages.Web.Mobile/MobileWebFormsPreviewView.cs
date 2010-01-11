namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;

    public class MobileWebFormsPreviewView : WebFormsPreviewView
    {
        public MobileWebFormsPreviewView(IServiceProvider serviceProvider) : base(serviceProvider, WebFormsEditorMode.UserControl)
        {
        }
    }
}


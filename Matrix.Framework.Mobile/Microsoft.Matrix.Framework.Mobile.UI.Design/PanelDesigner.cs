namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Web.UI.Design;
    using System.Web.UI.MobileControls;

    public class PanelDesigner : MobileContainerDesigner
    {
        private TemporaryBitmapFile _backgroundBmpFile = null;
        private const int _contentTemplate = 0;
        private Size _defaultSize = new Size(300, 0x2d);
        private const int _numberOfTemplateFrames = 1;
        private Panel _panel;
        private static readonly string[][] _templateFrameNames = new string[][] { new string[] { Constants.ContentTemplateTag } };

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(this.OnUserPreferenceChanged);
                if (this._backgroundBmpFile != null)
                {
                    this._backgroundBmpFile.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override Size GetDefaultSize()
        {
            return this._defaultSize;
        }

        protected override string GetErrorMessage(out bool infoMode)
        {
            infoMode = false;
            if (!Utils.InMobileUserControl(this._panel))
            {
                if (Utils.InUserControl(this._panel))
                {
                    infoMode = true;
                    return Constants.UserControlWarningMessage;
                }
                if (!Utils.InMobilePage(this._panel))
                {
                    return Constants.MobilePageErrorMessage;
                }
                if (!this.ValidContainment)
                {
                    return Constants.FormPanelContainmentErrorMessage;
                }
            }
            return null;
        }

        protected override string[] GetTemplateFrameNames(int index)
        {
            return _templateFrameNames[index];
        }

        protected override TemplateEditingVerb[] GetTemplateVerbs()
        {
            return new TemplateEditingVerb[] { new TemplateEditingVerb(Constants.TemplateFrameContentTemplate, 0, this) };
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this._panel = (Panel) component;
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.OnUserPreferenceChanged);
        }

        protected override void OnBackgroundImageChange(string message, bool infoMode)
        {
            if (message == null)
            {
                base.RemoveBehaviorStyle("backgroundImage");
                base.SetBehaviorStyle("paddingTop", 8);
            }
            else
            {
                ImageCreator.CreateBackgroundImage(ref this._backgroundBmpFile, string.Empty, string.Empty, message, infoMode, this.GetDefaultSize().Width);
                base.SetBehaviorStyle("backgroundImage", "url(" + this._backgroundBmpFile.Url + ")");
                base.SetBehaviorStyle("paddingTop", this._backgroundBmpFile.UnderlyingBitmap.Height + 8);
            }
        }

        protected override void OnContainmentChanged()
        {
            base.OnContainmentChanged();
            base.SetBehaviorStyle("marginRight", (base.ContainmentStatus == ContainmentStatus.AtTopLevel) ? "30%" : "5px");
            base.SetBehaviorStyle("marginTop", this.ValidContainment ? "3px" : "5px");
            base.SetBehaviorStyle("marginBottom", this.ValidContainment ? "3px" : "5px");
            base.SetBehaviorStyle("width", this.ValidContainment ? "100%" : (this.GetDefaultSize().Width.ToString() + "px"));
        }

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Color)
            {
                bool flag;
                string errorMessage = this.GetErrorMessage(out flag);
                this.OnBackgroundImageChange(errorMessage, flag);
            }
        }

        protected override void SetControlDefaultAppearance()
        {
            base.SetControlDefaultAppearance();
            base.SetBehaviorStyle("borderStyle", "dotted");
        }

        private bool ValidContainment
        {
            get
            {
                if ((base.ContainmentStatus != ContainmentStatus.InForm) && (base.ContainmentStatus != ContainmentStatus.InPanel))
                {
                    return (base.ContainmentStatus == ContainmentStatus.InTemplateFrame);
                }
                return true;
            }
        }
    }
}


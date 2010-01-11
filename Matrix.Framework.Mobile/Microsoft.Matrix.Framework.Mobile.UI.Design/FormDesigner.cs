namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Web.UI.Design;
    using System.Web.UI.MobileControls;

    public class FormDesigner : MobileContainerDesigner
    {
        private TemporaryBitmapFile _backgroundBmpFile = null;
        private static readonly Attribute[] _emptyAttrs = new Attribute[0];
        private Form _form;
        private const int _headerFooterTemplates = 0;
        private const int _numberOfTemplateFrames = 1;
        private static readonly string[][] _templateFrameNames = new string[][] { new string[] { Constants.HeaderTemplateTag, Constants.FooterTemplateTag } };
        private const string _titlePropertyName = "Title";

        private void ChangeBackgroundImage()
        {
            bool infoMode = false;
            string errorMessage = this.GetErrorMessage(out infoMode);
            this.OnBackgroundImageChange(errorMessage, infoMode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(this.OnUserPreferenceChanged);
            }
            base.Dispose(disposing);
        }

        protected override string GetErrorMessage(out bool infoMode)
        {
            infoMode = false;
            if (!Utils.InMobileUserControl(this._form))
            {
                if (Utils.InUserControl(this._form))
                {
                    infoMode = true;
                    return Constants.UserControlWarningMessage;
                }
                if (!Utils.InMobilePage(this._form))
                {
                    return Constants.MobilePageErrorMessage;
                }
            }
            if (!this.ValidContainment)
            {
                return Constants.TopPageContainmentErrorMessage;
            }
            return null;
        }

        protected override string[] GetTemplateFrameNames(int index)
        {
            return _templateFrameNames[index];
        }

        protected override TemplateEditingVerb[] GetTemplateVerbs()
        {
            return new TemplateEditingVerb[] { new TemplateEditingVerb(Constants.TemplateFrameHeaderFooterTemplates, 0, this) };
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this._form = (Form) component;
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.OnUserPreferenceChanged);
        }

        protected override void OnBackgroundImageChange(string message, bool infoMode)
        {
            ImageCreator.CreateBackgroundImage(ref this._backgroundBmpFile, this._form.ID, this._form.get_Title(), message, infoMode, this.GetDefaultSize().Width);
            base.SetBehaviorStyle("backgroundImage", "url(" + this._backgroundBmpFile.Url + ")");
            base.SetBehaviorStyle("paddingTop", this._backgroundBmpFile.UnderlyingBitmap.Height + 8);
        }

        protected override void OnContainmentChanged()
        {
            base.OnContainmentChanged();
            base.SetBehaviorStyle("marginTop", this.ValidContainment ? "5px" : "3px");
            base.SetBehaviorStyle("marginBottom", this.ValidContainment ? "5px" : "3px");
            base.SetBehaviorStyle("marginRight", this.ValidContainment ? "30%" : "5px");
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

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor) properties["Title"];
            properties["Title"] = TypeDescriptor.CreateProperty(base.GetType(), oldPropertyDescriptor, _emptyAttrs);
        }

        protected override void SetControlDefaultAppearance()
        {
            base.SetControlDefaultAppearance();
            base.SetBehaviorStyle("borderStyle", "solid");
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                this.ChangeBackgroundImage();
            }
        }

        public virtual string Title
        {
            get
            {
                return this._form.get_Title();
            }
            set
            {
                this._form.set_Title(value);
                this.ChangeBackgroundImage();
            }
        }

        private bool ValidContainment
        {
            get
            {
                return (base.ContainmentStatus == ContainmentStatus.AtTopLevel);
            }
        }
    }
}


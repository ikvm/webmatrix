namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Runtime.InteropServices;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;

    public class DeviceSpecificDesigner : ControlDesigner, IMobileDesigner
    {
        private Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus _containmentStatus;
        private bool _containmentStatusDirty = true;
        private const string _designTimeHTML = "\r\n                <table cellpadding=4 cellspacing=0 width='100%' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td><span style='font-weight:bold'>DeviceSpecific</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:4px'>{1}</td></tr>\r\n                </table>\r\n             ";
        private DeviceSpecific _deviceSpecific;
        private IDeviceSpecificSelectionProvider _deviceSpecificSelectionProvider;
        private const string _duplicateDesignTimeHTML = "\r\n                <table cellpadding=4 cellspacing=0 width='100%' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td><span style='font-weight:bold'>DeviceSpecific</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:4px'>{1}</td></tr>\r\n                  <tr><td>\r\n                    <table style='font-size:8pt;color:window;background-color:ButtonShadow'>\r\n                      <tr><td valign='top'><img src='{2}'/></td><td>{3}</td></tr>\r\n                    </table>\r\n                  </td></tr>\r\n                </table>\r\n             ";
        private bool _isDuplicate = false;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Panel component = this._deviceSpecific.get_Owner() as Panel;
                if (component != null)
                {
                    component.set_DeviceSpecific(null);
                    Utils.SetDeviceSpecificOwner(this._deviceSpecific, null);
                    MobileContainerDesigner designer = (MobileContainerDesigner) this.Host.GetDesigner(component);
                    if (designer != null)
                    {
                        designer.OnDeviceSpecificRemoved();
                    }
                    bool flag1 = this._isDuplicate;
                }
                IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                if (service != null)
                {
                    service.LoadComplete -= new EventHandler(this.OnLoadComplete);
                }
            }
            base.Dispose(disposing);
        }

        protected virtual string GetDesignTimeErrorHtml(string errorMessage, bool infoMode)
        {
            return Utils.GetDesignTimeErrorHtml(errorMessage, infoMode, this._deviceSpecific, base.Behavior, this.ContainmentStatus);
        }

        public override string GetDesignTimeHtml()
        {
            if (this.Host.Loading)
            {
                return string.Empty;
            }
            bool infoMode = false;
            string errorMessage = this.GetErrorMessage(out infoMode);
            this.SetStyleAttributes();
            if (errorMessage != null)
            {
                return this.GetDesignTimeErrorHtml(errorMessage, infoMode);
            }
            try
            {
                return this.GetDesignTimeNormalHtml();
            }
            catch (Exception exception)
            {
                return this.GetDesignTimeErrorHtml(exception.Message, false);
            }
        }

        protected virtual string GetDesignTimeNormalHtml()
        {
            if (this._isDuplicate)
            {
                return string.Format("\r\n                <table cellpadding=4 cellspacing=0 width='100%' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td><span style='font-weight:bold'>DeviceSpecific</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:4px'>{1}</td></tr>\r\n                  <tr><td>\r\n                    <table style='font-size:8pt;color:window;background-color:ButtonShadow'>\r\n                      <tr><td valign='top'><img src='{2}'/></td><td>{3}</td></tr>\r\n                    </table>\r\n                  </td></tr>\r\n                </table>\r\n             ", new object[] { this._deviceSpecific.ID, string.Empty, Constants.ErrorIconUrl, Constants.DeviceSpecificDuplicateWarningMessage });
            }
            return string.Format("\r\n                <table cellpadding=4 cellspacing=0 width='100%' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td><span style='font-weight:bold'>DeviceSpecific</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:4px'>{1}</td></tr>\r\n                </table>\r\n             ", this._deviceSpecific.ID, string.Empty);
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return ("<div style='width:100%'>" + base.GetEmptyDesignTimeHtml() + "</div>");
        }

        protected virtual string GetErrorMessage(out bool infoMode)
        {
            infoMode = false;
            if (!Utils.InMobileUserControl(this._deviceSpecific))
            {
                if (Utils.InUserControl(this._deviceSpecific))
                {
                    infoMode = true;
                    return Constants.UserControlWarningMessage;
                }
                if (!Utils.InMobilePage(this._deviceSpecific))
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

        public override string GetPersistInnerHtml()
        {
            string str = Utils.InvokePersistInnerProperties(this._deviceSpecific);
            base.IsDirty = false;
            return str;
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this._deviceSpecific = (DeviceSpecific) component;
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (service != null)
            {
                service.LoadComplete += new EventHandler(this.OnLoadComplete);
            }
            this._deviceSpecificSelectionProvider = (IDeviceSpecificSelectionProvider) this.GetService(typeof(IDeviceSpecificSelectionProvider));
            if (this._deviceSpecificSelectionProvider != null)
            {
                foreach (DeviceSpecificChoice choice in this._deviceSpecific.get_Choices())
                {
                    string filterName = Utils.CreateUniqueChoiceName(choice);
                    if (!this._deviceSpecificSelectionProvider.FilterExists(filterName))
                    {
                        this._deviceSpecificSelectionProvider.AddFilter(filterName);
                    }
                }
            }
        }

        private void OnLoadComplete(object sender, EventArgs e)
        {
            this.UpdateRendering();
        }

        public override void OnSetParent()
        {
            base.OnSetParent();
            this._isDuplicate = false;
            this._containmentStatusDirty = true;
            Panel component = this._deviceSpecific.get_Owner() as Panel;
            if (component != null)
            {
                component.set_DeviceSpecific(null);
                Utils.SetDeviceSpecificOwner(this._deviceSpecific, null);
                MobileContainerDesigner designer = (MobileContainerDesigner) this.Host.GetDesigner(component);
                if (designer != null)
                {
                    designer.OnDeviceSpecificRemoved();
                }
            }
            Panel parent = this._deviceSpecific.Parent as Panel;
            if (parent != null)
            {
                if ((parent.get_DeviceSpecific() != null) && (parent.get_DeviceSpecific().ID != this._deviceSpecific.ID))
                {
                    DeviceSpecificDesigner designer2 = (DeviceSpecificDesigner) this.Host.GetDesigner(parent.get_DeviceSpecific());
                    this._isDuplicate = true;
                }
                else
                {
                    parent.set_DeviceSpecific(this._deviceSpecific);
                    MobileContainerDesigner designer3 = (MobileContainerDesigner) this.Host.GetDesigner(parent);
                    if (designer3 != null)
                    {
                        designer3.OnDeviceSpecificApplied();
                    }
                }
            }
            this.UpdateRendering();
        }

        protected override void PreFilterEvents(IDictionary events)
        {
            events.Clear();
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            Type designerType = base.GetType();
            Utils.AddAttributesToProperty(designerType, properties, "ID", new Attribute[] { new BrowsableAttribute(false) });
            Utils.AddAttributesToProperty(designerType, properties, "DataBindings", new Attribute[] { new BrowsableAttribute(false) });
        }

        protected virtual void SetStyleAttributes()
        {
            Utils.SetStandardStyleAttributes(base.Behavior, this.ContainmentStatus);
        }

        public void UpdateRendering()
        {
            this.UpdateDesignTimeHtml();
        }

        protected Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus ContainmentStatus
        {
            get
            {
                if (this._containmentStatusDirty)
                {
                    this._containmentStatus = Utils.GetContainmentStatus(this._deviceSpecific);
                    this._containmentStatusDirty = false;
                }
                return this._containmentStatus;
            }
        }

        private IDesignerHost Host
        {
            get
            {
                return (IDesignerHost) this.GetService(typeof(IDesignerHost));
            }
        }

        private bool ValidContainment
        {
            get
            {
                if ((this.ContainmentStatus != Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus.InTemplateFrame) && (this.ContainmentStatus != Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus.InForm))
                {
                    return (this.ContainmentStatus == Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus.InPanel);
                }
                return true;
            }
        }
    }
}


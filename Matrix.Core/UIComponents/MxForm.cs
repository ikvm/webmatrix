namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;
    using System.Diagnostics;

    public class MxForm : Form
    {
        private bool _initialActivatedRaised;
        private IServiceProvider _serviceProvider;
        private static readonly object InitialActivatedEvent = new object();

        public event EventHandler InitialActivated
        {
            add
            {
                base.Events.AddHandler(InitialActivatedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(InitialActivatedEvent, value);
            }
        }

        public MxForm()
        {
            if (LicenseManager.CurrentContext.UsageMode != LicenseUsageMode.Designtime)
            {
                throw new InvalidOperationException("Parameter-less constructor is meant for design-time use only.");
            }
        }

        public MxForm(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this._serviceProvider = serviceProvider;
            IUIService service = (IUIService) this.GetService(typeof(IUIService));
            if (service != null)
            {
                Font font = (Font) service.Styles["DialogFont"];
                this.Font = font;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._serviceProvider = null;
            }
            base.Dispose(disposing);
        }

        protected override object GetService(Type serviceType)
        {
            if (this._serviceProvider != null)
            {
                return this._serviceProvider.GetService(serviceType);
            }
            return null;
        }

        private bool HandleWmDrawItem(ref Message m)
        {
            if (((int) m.WParam) == 0)
            {
                Interop.DRAWITEMSTRUCT dis = (Interop.DRAWITEMSTRUCT) Marshal.PtrToStructure(m.LParam, typeof(Interop.DRAWITEMSTRUCT));
                if ((dis.itemID & 0x2000) != 0)
                {
                    switch ((dis.itemID & 0xffff))
                    {
                        case 0xf030:
                            MxMenuItem.MaximizeMenuItem.OnDrawItem(dis);
                            return true;

                        case 0xf060:
                            MxMenuItem.CloseMenuItem.OnDrawItem(dis);
                            return true;

                        case 0xf120:
                            MxMenuItem.RestoreMenuItem.OnDrawItem(dis);
                            return true;

                        case 0xf000:
                            MxMenuItem.SizeMenuItem.OnDrawItem(dis);
                            return true;

                        case 0xf010:
                            MxMenuItem.MoveMenuItem.OnDrawItem(dis);
                            return true;

                        case 0xf020:
                            MxMenuItem.MinimizeMenuItem.OnDrawItem(dis);
                            return true;
                    }
                }
            }
            return false;
        }

        private bool HandleWmMeasureItem(ref Message m)
        {
            if (((((int) m.WParam) >> 0x10) & 0x2000) != 0)
            {
                int num = ((int) m.WParam) & 0xffff;
                Interop.MEASUREITEMSTRUCT mis = (Interop.MEASUREITEMSTRUCT) Marshal.PtrToStructure(m.LParam, typeof(Interop.MEASUREITEMSTRUCT));
                switch (num)
                {
                    case 0xf030:
                        MxMenuItem.MaximizeMenuItem.OnMeasureItem(mis);
                        return true;

                    case 0xf060:
                        MxMenuItem.CloseMenuItem.OnMeasureItem(mis);
                        return true;

                    case 0xf120:
                        MxMenuItem.RestoreMenuItem.OnMeasureItem(mis);
                        return true;

                    case 0xf000:
                        MxMenuItem.SizeMenuItem.OnMeasureItem(mis);
                        return true;

                    case 0xf010:
                        MxMenuItem.MoveMenuItem.OnMeasureItem(mis);
                        return true;

                    case 0xf020:
                        MxMenuItem.MinimizeMenuItem.OnMeasureItem(mis);
                        return true;
                }
            }
            return false;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (!this._initialActivatedRaised)
            {
                this._initialActivatedRaised = true;
                this.OnInitialActivated(e);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        protected virtual void OnInitialActivated(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[InitialActivatedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x2b:
                    if (!this.HandleWmDrawItem(ref m))
                    {
                        base.WndProc(ref m);
                    }
                    return;

                case 0x2c:
                    if (!this.HandleWmMeasureItem(ref m))
                    {
                        base.WndProc(ref m);
                    }
                    return;

                case 0x222:
                    if (m.LParam == base.Handle)
                    {
                        this.OnActivated(EventArgs.Empty);
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }
    }
}


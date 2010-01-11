namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class MxToolBarSubclasser : IWindowTarget
    {
        private IWindowTarget _oldWindowTarget;
        private MxToolBarPainter _painter;
        private System.Windows.Forms.ToolBar _toolBar;

        public MxToolBarSubclasser(System.Windows.Forms.ToolBar toolBar)
        {
            if (toolBar == null)
            {
                throw new ArgumentNullException();
            }
            this._toolBar = toolBar;
            this._painter = new MxToolBarPainter(this._toolBar);
        }

        protected virtual void OnHandleChange(IntPtr newHandle)
        {
            this._oldWindowTarget.OnHandleChange(newHandle);
        }

        protected virtual void OnMessage(ref Message m)
        {
            if (m.Msg == 0x204e)
            {
                Interop.NMHDR nmhdr = (Interop.NMHDR) Marshal.PtrToStructure(m.LParam, typeof(Interop.NMHDR));
                if (nmhdr.code == -12)
                {
                    this._painter.OnCustomDraw(ref m);
                    return;
                }
            }
            else if (m.Msg == 0x201)
            {
                this._painter.MouseDown = true;
            }
            else if (this._painter.MouseDown && ((m.Msg == 0x202) || (m.Msg == 0x200)))
            {
                this._painter.MouseDown = false;
            }
            this._oldWindowTarget.OnMessage(ref m);
        }

        public void Subclass()
        {
            if (this._toolBar.WindowTarget != this)
            {
                this._oldWindowTarget = this._toolBar.WindowTarget;
                this._toolBar.WindowTarget = this;
            }
        }

        void IWindowTarget.OnHandleChange(IntPtr newHandle)
        {
            this.OnHandleChange(newHandle);
        }

        void IWindowTarget.OnMessage(ref Message m)
        {
            this.OnMessage(ref m);
        }

        public void Unsubclass()
        {
            if (this._oldWindowTarget != null)
            {
                this._toolBar.WindowTarget = this._oldWindowTarget;
                this._oldWindowTarget = null;
            }
        }

        public System.Windows.Forms.ToolBar ToolBar
        {
            get
            {
                return this._toolBar;
            }
        }
    }
}


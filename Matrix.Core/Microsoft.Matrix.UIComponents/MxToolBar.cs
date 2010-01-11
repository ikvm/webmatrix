namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class MxToolBar : ToolBar
    {
        private ImageList _disabledImageList;
        private MxToolBarPainter _painter;
        private static readonly object ComboBoxCreatedEvent = new object();

        public event ToolBarComboBoxButtonEventHandler ComboBoxCreated
        {
            add
            {
                base.Events.AddHandler(ComboBoxCreatedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(ComboBoxCreatedEvent, value);
            }
        }

        public MxToolBar()
        {
            this._painter = new MxToolBarPainter(this);
        }

        private void InitializeComboBoxes()
        {
            int wParam = -1;
            foreach (ToolBarButton button in base.Buttons)
            {
                wParam++;
                ComboBoxToolBarButton button2 = button as ComboBoxToolBarButton;
                if ((button2 != null) && button2.Visible)
                {
                    MxComboBox comboBox;
                    Panel comboBoxHolder;
                    Interop.RECT lParam = new Interop.RECT();
                    Interop.SendMessage(base.Handle, 0x41d, wParam, ref lParam);
                    if (base.RecreatingHandle)
                    {
                        comboBoxHolder = button2.ComboBoxHolder;
                        comboBox = button2.ComboBox;
                        button2.SetParentRecreating(false);
                    }
                    else
                    {
                        comboBoxHolder = new Panel();
                        button2.ComboBoxHolder = comboBoxHolder;
                        comboBox = button2.CreateComboBox();
                    }
                    int height = comboBox.Height;
                    int y = (lParam.Height - height) / 2;
                    comboBox.SetBounds(1, y, lParam.Width - 2, height);
                    comboBoxHolder.Controls.Add(comboBox);
                    base.Controls.Add(comboBoxHolder);
                    Interop.SetWindowPos(comboBoxHolder.Handle, IntPtr.Zero, lParam.left, lParam.top, lParam.Width, lParam.Height, 0x54);
                    if ((button2.ToolTipText != null) && (button2.ToolTipText.Length != 0))
                    {
                        IntPtr hWnd = Interop.SendMessage(base.Handle, 0x423, 0, 0);
                        if (hWnd != IntPtr.Zero)
                        {
                            Interop.TOOLINFO toolinfo = new Interop.TOOLINFO();
                            toolinfo.uFlags = 0x11;
                            toolinfo.hwnd = toolinfo.uId = comboBox.Handle;
                            toolinfo.lpszText = button2.ToolTipText;
                            Interop.SendMessage(hWnd, Interop.TTM_ADDTOOL, 0, toolinfo);
                        }
                    }
                    this.OnComboBoxCreated(new ToolBarComboBoxButtonEventArgs(button2));
                }
            }
        }

        protected virtual void OnComboBoxCreated(ToolBarComboBoxButtonEventArgs e)
        {
            ToolBarComboBoxButtonEventHandler handler = (ToolBarComboBoxButtonEventHandler) base.Events[ComboBoxCreatedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.InitializeComboBoxes();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            if (base.RecreatingHandle)
            {
                foreach (ToolBarButton button in base.Buttons)
                {
                    ComboBoxToolBarButton button2 = button as ComboBoxToolBarButton;
                    if (button2 != null)
                    {
                        button2.SetParentRecreating(true);
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
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
            base.WndProc(ref m);
        }

        public ImageList DisabledImageList
        {
            get
            {
                if (this._disabledImageList == null)
                {
                    ImageList imageList = base.ImageList;
                    if (imageList != null)
                    {
                        this._disabledImageList = new ImageList();
                        foreach (Bitmap bitmap in imageList.Images)
                        {
                            Image image = ImageUtility.CreateDisabledImage(bitmap);
                            this._disabledImageList.Images.Add(image);
                        }
                    }
                }
                return this._disabledImageList;
            }
        }
    }
}


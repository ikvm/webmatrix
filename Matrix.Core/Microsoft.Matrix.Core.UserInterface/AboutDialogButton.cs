namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    public sealed class AboutDialogButton : Button
    {
        private Color _highlightColor;
        private bool _mouseDown;
        private bool _mouseOver;
        private bool _mousePressed;

        public AboutDialogButton()
        {
            base.FlatStyle = FlatStyle.Flat;
            this._highlightColor = Color.FromArgb(0xff, 0x6c, 0x7c, 0xa6);
            this.ForeColor = this._highlightColor;
            this.BackColor = Color.FromArgb(100, this._highlightColor);
        }

        private void DrawText(Graphics g, Rectangle r, Color c)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            if (base.ShowKeyboardCues)
            {
                format.HotkeyPrefix = HotkeyPrefix.Show;
            }
            else
            {
                format.HotkeyPrefix = HotkeyPrefix.Hide;
            }
            SolidBrush brush = new SolidBrush(c);
            g.DrawString(this.Text, this.Font, brush, r, format);
            brush.Dispose();
            format.Dispose();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (!base.Enabled)
            {
                this._mouseDown = false;
                this._mouseOver = false;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Space) && !this._mouseDown)
            {
                this._mouseDown = true;
            }
            base.OnKeyDown(e);
            base.Update();
        }

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if ((kevent.KeyData == Keys.Space) && this._mouseDown)
            {
                this._mouseDown = false;
            }
            base.OnKeyUp(kevent);
            base.Update();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this._mouseDown = false;
            base.OnLostFocus(e);
            base.Update();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((Control.MouseButtons != MouseButtons.None) && (e.Button == MouseButtons.Left))
            {
                this._mouseDown = true;
                this._mousePressed = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this._mouseOver = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this._mouseOver = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button != MouseButtons.None) && this._mousePressed)
            {
                if (!base.ClientRectangle.Contains(e.X, e.Y))
                {
                    if (this._mouseDown)
                    {
                        this._mouseDown = false;
                    }
                }
                else if (!this._mouseDown)
                {
                    this._mouseDown = true;
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this._mousePressed)
            {
                this._mousePressed = false;
                this._mouseDown = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this._mouseDown)
            {
                this.PaintFlatDown(e);
            }
            else if (this._mouseOver)
            {
                this.PaintFlatOver(e);
            }
            else
            {
                this.PaintFlatUp(e);
            }
        }

        private void PaintFlatDown(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle clientRectangle = base.ClientRectangle;
            Rectangle rect = new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
            g.FillRectangle(Brushes.White, rect);
            Brush brush = new SolidBrush(Color.FromArgb(150, this._highlightColor));
            g.FillRectangle(brush, rect);
            brush.Dispose();
            this.DrawText(g, clientRectangle, Color.White);
            Pen pen = new Pen(this._highlightColor, 1f);
            g.DrawRectangle(pen, rect);
            pen.Dispose();
        }

        private void PaintFlatOver(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle clientRectangle = base.ClientRectangle;
            Rectangle rect = new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
            g.FillRectangle(Brushes.White, rect);
            Brush brush = new SolidBrush(this.BackColor);
            g.FillRectangle(brush, rect);
            brush.Dispose();
            this.DrawText(g, clientRectangle, Color.Black);
            Pen pen = new Pen(this._highlightColor, 1f);
            g.DrawRectangle(pen, rect);
            pen.Dispose();
        }

        private void PaintFlatUp(PaintEventArgs e)
        {
            if (this.Focused)
            {
                this.PaintFlatOver(e);
            }
            else
            {
                Graphics g = e.Graphics;
                Rectangle clientRectangle = base.ClientRectangle;
                Rectangle rect = new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
                clientRectangle.Inflate(-1, -1);
                g.FillRectangle(Brushes.White, clientRectangle);
                Brush brush = new SolidBrush(Color.FromArgb(this.BackColor.A / 2, this.BackColor));
                g.FillRectangle(brush, clientRectangle);
                brush.Dispose();
                this.DrawText(g, clientRectangle, Color.Black);
                Pen pen = new Pen(this._highlightColor, 1f);
                g.DrawRectangle(pen, rect);
                pen.Dispose();
            }
        }
    }
}


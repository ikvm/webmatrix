namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class GroupLabel : Label
    {
        internal GroupLabel()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle clientRectangle = base.ClientRectangle;
            string text = this.Text;
            Brush brush = new SolidBrush(this.ForeColor);
            graphics.DrawString(text, this.Font, brush, (float) 0f, (float) 0f);
            brush.Dispose();
            int x = clientRectangle.X;
            if (text.Length != 0)
            {
                Size size = Size.Ceiling(graphics.MeasureString(text, this.Font));
                x += 6 + size.Width;
            }
            int num2 = clientRectangle.Height / 2;
            graphics.DrawLine(SystemPens.ControlDark, x, num2, clientRectangle.Width, num2);
            num2++;
            graphics.DrawLine(SystemPens.ControlLightLight, x, num2, clientRectangle.Width, num2);
        }
    }
}


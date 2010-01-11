namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class GroupLabel : Label
    {
        public GroupLabel()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            Graphics graphics = e.Graphics;
            Font font = this.Font;
            string text = this.Text;
            Brush brush = new SolidBrush(this.ForeColor);
            graphics.DrawString(text, font, brush, (float) 0f, (float) 0f);
            brush.Dispose();
            int x = clientRectangle.X;
            if (text.Length != 0)
            {
                Size size = Size.Ceiling(graphics.MeasureString(text, font));
                x += 4 + size.Width;
            }
            int num2 = clientRectangle.Height / 2;
            graphics.DrawLine(SystemPens.ControlDark, x, num2, clientRectangle.Width, num2);
            num2++;
            graphics.DrawLine(SystemPens.ControlLightLight, x, num2, clientRectangle.Width, num2);
        }
    }
}


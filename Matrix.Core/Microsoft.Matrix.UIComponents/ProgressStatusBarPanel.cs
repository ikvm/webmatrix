namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ProgressStatusBarPanel : MxStatusBarPanel
    {
        private int _percentComplete = 0;
        private const int HighlightOpacity = 120;
        private const int HighlightOpacity2 = 0x4b;
        private const int Padding = 3;

        public override void DrawPanel(DrawItemEventArgs e)
        {
            if (this._percentComplete != 0)
            {
                Graphics graphics = e.Graphics;
                int width = (Math.Max(5, this._percentComplete) * ((e.Bounds.Width - 6) - 1)) / 100;
                Rectangle rect = new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, (e.Bounds.Height - 6) - 1);
                Brush brush = new SolidBrush(Color.FromArgb(120, SystemColors.Highlight));
                Brush brush2 = new SolidBrush(Color.FromArgb(0x4b, Color.White));
                graphics.FillRectangle(brush, rect);
                graphics.FillRectangle(brush2, rect);
                graphics.DrawRectangle(SystemPens.Highlight, rect);
                brush.Dispose();
                brush2.Dispose();
            }
            base.DrawPanelBorder(e);
        }

        public int PercentComplete
        {
            get
            {
                return this._percentComplete;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                if (this._percentComplete != value)
                {
                    this._percentComplete = value;
                    base.Invalidate();
                }
            }
        }
    }
}


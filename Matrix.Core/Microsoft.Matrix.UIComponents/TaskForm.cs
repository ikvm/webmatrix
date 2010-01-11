namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TaskForm : MxForm
    {
        private BorderStyle _taskBorderStyle;
        private string _taskCaption;
        private string _taskDescription;
        private Image _taskGlyph;

        public TaskForm()
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        public TaskForm(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle clientRectangle = base.ClientRectangle;
            bool flag = this.TaskCaption.Length != 0;
            if (flag)
            {
                Rectangle rect = new Rectangle(0, 0, clientRectangle.Width, 0x38);
                graphics.FillRectangle(Brushes.White, rect);
                if (this._taskBorderStyle != BorderStyle.None)
                {
                    graphics.DrawLine(SystemPens.ControlDark, 0, 0x39, rect.Width, 0x39);
                    graphics.DrawLine(SystemPens.ControlLightLight, 0, 0x3a, rect.Width, 0x3a);
                }
                if (flag)
                {
                    Font font2;
                    Font font = this.Font;
                    try
                    {
                        font2 = new Font(font.FontFamily, font.SizeInPoints + 1f, FontStyle.Bold);
                    }
                    catch (Exception)
                    {
                        font2 = font;
                    }
                    graphics.DrawString(this._taskCaption, font2, Brushes.Black, (float) 8f, (float) 10f);
                    if ((this._taskDescription != null) && (this._taskDescription.Length != 0))
                    {
                        graphics.DrawString(this._taskDescription, font, Brushes.Black, (float) 16f, (float) (14f + font2.GetHeight(graphics)));
                    }
                }
                if (this._taskGlyph != null)
                {
                    graphics.DrawImage(this._taskGlyph, (rect.Width - 0x30) - 6, 4, 0x30, 0x30);
                }
                else
                {
                    graphics.FillRectangle(Brushes.Beige, (rect.Width - 0x30) - 6, 4, 0x30, 0x30);
                    graphics.DrawRectangle(Pens.Tan, (rect.Width - 0x30) - 6, 4, 0x30, 0x30);
                }
            }
        }

        protected virtual void ShowTaskAboutInformation()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xa1)
            {
                if (((int) m.WParam) == 0x15)
                {
                    return;
                }
            }
            else if ((m.Msg == 0xa2) && (((int) m.WParam) == 0x15))
            {
                this.ShowTaskAboutInformation();
                return;
            }
            base.WndProc(ref m);
        }

        [DefaultValue(false)]
        public bool TaskAbout
        {
            get
            {
                return base.HelpButton;
            }
            set
            {
                base.HelpButton = value;
            }
        }

        [Category("Appearance"), DefaultValue(0)]
        public BorderStyle TaskBorderStyle
        {
            get
            {
                return this._taskBorderStyle;
            }
            set
            {
                if ((value != BorderStyle.None) && (value != BorderStyle.FixedSingle))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._taskBorderStyle = value;
                if (base.IsHandleCreated)
                {
                    base.Invalidate(false);
                }
            }
        }

        [Category("Appearance"), DefaultValue("")]
        public string TaskCaption
        {
            get
            {
                if (this._taskCaption == null)
                {
                    return string.Empty;
                }
                return this._taskCaption;
            }
            set
            {
                this._taskCaption = value;
                if (base.IsHandleCreated)
                {
                    base.Invalidate(false);
                }
            }
        }

        [Category("Appearance"), DefaultValue("")]
        public string TaskDescription
        {
            get
            {
                if (this._taskDescription == null)
                {
                    return string.Empty;
                }
                return this._taskDescription;
            }
            set
            {
                this._taskDescription = value;
                if (base.IsHandleCreated)
                {
                    base.Invalidate(false);
                }
            }
        }

        [Category("Appearance"), DefaultValue((string) null)]
        public Image TaskGlyph
        {
            get
            {
                return this._taskGlyph;
            }
            set
            {
                this._taskGlyph = value;
                if (base.IsHandleCreated)
                {
                    base.Invalidate(false);
                }
            }
        }
    }
}


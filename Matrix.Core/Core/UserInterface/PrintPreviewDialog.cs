namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Text;
    using System.Windows.Forms;

    internal sealed class PrintPreviewDialog : MxForm
    {
        private MxToolBarButton _closeButton;
        private CommandBar _commandBar;
        private PrintDocument _document;
        private MxToolBarButton _firstButton;
        private MxToolBarButton _fourPagesButton;
        private MxToolBarButton _lastButton;
        private MxToolBarButton _nextButton;
        private MxToolBarButton _onePageButton;
        private int _pageCount;
        private MxStatusBarPanel _pageIndexPanel;
        private MxToolBarButton _prevButton;
        private PrintPreviewControl _preview;
        private MxToolBarButton _printButton;
        private ProgressStatusBarPanel _progressPanel;
        private MxToolBarButton _settingsButton;
        private MxStatusBar _statusBar;
        private MxStatusBarPanel _textPanel;
        private MxToolBarButton _twoPagesButton;
        private ComboBoxToolBarButton _zoomButton;
        private const int CloseCommand = 2;
        private const int FirstPageCommand = 5;
        private const int FourPagesCommand = 10;
        private const int LastPageCommand = 6;
        private const int NextPageCommand = 3;
        private const int OnePageCommand = 8;
        private const int PrevPageCommand = 4;
        private const int PrintCommand = 1;
        private const int SettingsCommand = 7;
        private const int TwoPagesCommand = 9;
        private const int Zoom100 = 3;
        private const int Zoom150 = 4;
        private const int Zoom200 = 5;
        private const int Zoom25 = 1;
        private const int Zoom50 = 2;
        private const int ZoomAuto = 0;

        public PrintPreviewDialog(IServiceProvider serviceProvider, PrintDocument document) : base(serviceProvider)
        {
            this._document = document;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            MxToolBar bar = new MxToolBar();
            ToolBarButton button = new ToolBarButton();
            ToolBarButton button2 = new ToolBarButton();
            ImageList list = new ImageList();
            this._commandBar = new CommandBar();
            this._printButton = new MxToolBarButton();
            this._closeButton = new MxToolBarButton();
            this._firstButton = new MxToolBarButton();
            this._lastButton = new MxToolBarButton();
            this._nextButton = new MxToolBarButton();
            this._prevButton = new MxToolBarButton();
            this._zoomButton = new ComboBoxToolBarButton();
            this._settingsButton = new MxToolBarButton();
            this._onePageButton = new MxToolBarButton();
            this._twoPagesButton = new MxToolBarButton();
            this._fourPagesButton = new MxToolBarButton();
            this._preview = new PrintPreviewControl();
            this._statusBar = new MxStatusBar();
            this._textPanel = new MxStatusBarPanel();
            this._progressPanel = new ProgressStatusBarPanel();
            this._pageIndexPanel = new MxStatusBarPanel();
            list.ImageSize = new Size(0x10, 0x10);
            list.TransparentColor = Color.Fuchsia;
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.AddStrip(new Bitmap(typeof(PrintPreviewDialog), "PrintPreviewCommands.bmp"));
            this._printButton.Text = "Print";
            this._printButton.ToolTipText = "Print this document";
            this._printButton.ImageIndex = 0;
            this._printButton.Tag = 1;
            this._printButton.Enabled = false;
            this._closeButton.Text = "Close";
            this._closeButton.ToolTipText = "Close and return to document";
            this._closeButton.ImageIndex = 1;
            this._closeButton.Tag = 2;
            this._firstButton.ToolTipText = "Go to the first page";
            this._firstButton.ImageIndex = 2;
            this._firstButton.Tag = 5;
            this._firstButton.Enabled = false;
            this._prevButton.ToolTipText = "Go to the previous page";
            this._prevButton.ImageIndex = 3;
            this._prevButton.Tag = 4;
            this._prevButton.Enabled = false;
            this._nextButton.ToolTipText = "Go to the next page";
            this._nextButton.ImageIndex = 4;
            this._nextButton.Tag = 3;
            this._nextButton.Enabled = false;
            this._lastButton.ToolTipText = "Go to the last page";
            this._lastButton.ImageIndex = 5;
            this._lastButton.Tag = 6;
            this._lastButton.Enabled = false;
            this._zoomButton.Text = "_____________";
            this._zoomButton.ComboBoxItems = new string[] { "Fit to Window", "25%", "50%", "100%", "150%", "200%" };
            this._settingsButton.ToolTipText = "Printer Settings";
            this._settingsButton.ImageIndex = 6;
            this._settingsButton.Tag = 7;
            this._onePageButton.ToolTipText = "One Page at a time";
            this._onePageButton.ImageIndex = 7;
            this._onePageButton.Tag = 8;
            this._onePageButton.Pushed = true;
            this._onePageButton.Enabled = false;
            this._twoPagesButton.ToolTipText = "Two pages at a time";
            this._twoPagesButton.ImageIndex = 8;
            this._twoPagesButton.Tag = 9;
            this._twoPagesButton.Enabled = false;
            this._fourPagesButton.ToolTipText = "Four pages at a time";
            this._fourPagesButton.ImageIndex = 9;
            this._fourPagesButton.Tag = 10;
            this._fourPagesButton.Enabled = false;
            button.Style = ToolBarButtonStyle.Separator;
            button2.Style = ToolBarButtonStyle.Separator;
            bar.Divider = false;
            bar.Appearance = ToolBarAppearance.Flat;
            bar.TextAlign = ToolBarTextAlign.Right;
            bar.Wrappable = false;
            bar.ShowToolTips = true;
            bar.DropDownArrows = false;
            bar.TabIndex = 0;
            bar.ImageList = list;
            bar.ButtonClick += new ToolBarButtonClickEventHandler(this.OnToolBarButtonClicked);
            bar.ComboBoxCreated += new ToolBarComboBoxButtonEventHandler(this.OnToolBarComboBoxCreated);
            bar.Buttons.AddRange(new ToolBarButton[] { this._printButton, this._settingsButton, this._closeButton, button, this._zoomButton, this._onePageButton, this._twoPagesButton, this._fourPagesButton, button2, this._firstButton, this._prevButton, this._nextButton, this._lastButton });
            this._commandBar.Dock = DockStyle.Top;
            this._commandBar.TabIndex = 0;
            this._commandBar.Controls.Add(bar);
            this._preview.Dock = DockStyle.Fill;
            this._preview.TabIndex = 1;
            this._preview.TabStop = false;
            this._preview.StartPageChanged += new EventHandler(this.OnPreviewStartPageChanged);
            this._textPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            this._progressPanel.MinWidth = 0x80;
            this._progressPanel.Width = 0x80;
            this._pageIndexPanel.Alignment = HorizontalAlignment.Center;
            this._pageIndexPanel.AutoSize = StatusBarPanelAutoSize.Contents;
            this._pageIndexPanel.MinWidth = 0x80;
            this._statusBar.Dock = DockStyle.Bottom;
            this._statusBar.TabStop = false;
            this._statusBar.ShowPanels = true;
            this._statusBar.Panels.AddRange(new StatusBarPanel[] { this._textPanel, this._progressPanel, this._pageIndexPanel });
            this.Text = "Print Preview";
            base.Size = new Size(800, 600);
            base.MinimumSize = new Size(200, 150);
            base.ShowInTaskbar = false;
            base.MinimizeBox = false;
            base.SizeGripStyle = SizeGripStyle.Show;
            base.StartPosition = FormStartPosition.CenterParent;
            base.Icon = new Icon(typeof(PrintPreviewDialog), "PrintPreviewDialog.ico");
            base.Controls.AddRange(new Control[] { this._preview, this._commandBar, this._statusBar });
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this._preview.Document = this._document;
        }

        private void OnPreviewEndPage()
        {
            this._pageIndexPanel.Text = this._pageCount.ToString() + " Page(s)";
        }

        private void OnPreviewEndPrint()
        {
            this._textPanel.Text = string.Empty;
            this._progressPanel.PercentComplete = 0;
            this.OnVisiblePagesChanged();
            this._onePageButton.Enabled = true;
            this._twoPagesButton.Enabled = true;
            this._fourPagesButton.Enabled = true;
            this._printButton.Enabled = true;
            this._zoomButton.ComboBox.Enabled = true;
        }

        private void OnPreviewStartPage()
        {
            this._pageCount++;
        }

        private void OnPreviewStartPageChanged(object sender, EventArgs e)
        {
            this.OnVisiblePagesChanged();
        }

        private void OnPreviewStartPrint()
        {
            this._pageCount = 0;
            this._textPanel.Text = "Generating Preview...";
            this._progressPanel.PercentComplete = 50;
            this._pageIndexPanel.Text = string.Empty;
        }

        private void OnSelectedIndexChangedZoom(object sender, EventArgs e)
        {
            switch (this._zoomButton.ComboBox.SelectedIndex)
            {
                case 0:
                    this._preview.AutoZoom = true;
                    return;

                case 1:
                    this._preview.Zoom = 0.25;
                    return;

                case 2:
                    this._preview.Zoom = 0.5;
                    return;

                case 3:
                    this._preview.Zoom = 1.0;
                    return;

                case 4:
                    this._preview.Zoom = 1.5;
                    return;

                case 5:
                    this._preview.Zoom = 2.0;
                    return;
            }
        }

        private void OnToolBarButtonClicked(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button.Tag != null)
            {
                switch (((int) e.Button.Tag))
                {
                    case 1:
                    {
                        ICommandManager service = (ICommandManager) this.GetService(typeof(ICommandManager));
                        if (service != null)
                        {
                            service.InvokeCommand(typeof(GlobalCommands), 6);
                        }
                        return;
                    }
                    case 2:
                        base.Close();
                        return;

                    case 3:
                        this._preview.StartPage += this._preview.PagesPerView;
                        return;

                    case 4:
                        this._preview.StartPage -= this._preview.PagesPerView;
                        return;

                    case 5:
                        this._preview.StartPage = 0;
                        return;

                    case 6:
                        this._preview.StartPage = this._pageCount - this._preview.PagesPerView;
                        return;

                    case 7:
                    {
                        ICommandManager manager2 = (ICommandManager) this.GetService(typeof(ICommandManager));
                        if (manager2 != null)
                        {
                            manager2.InvokeCommand(typeof(GlobalCommands), 8);
                            this._preview.InvalidatePreview();
                        }
                        return;
                    }
                    case 8:
                        this._preview.Rows = 1;
                        this._preview.Columns = 1;
                        this._onePageButton.Pushed = true;
                        this._twoPagesButton.Pushed = false;
                        this._fourPagesButton.Pushed = false;
                        this.OnVisiblePagesChanged();
                        return;

                    case 9:
                        this._preview.Rows = 1;
                        this._preview.Columns = 2;
                        this._onePageButton.Pushed = false;
                        this._twoPagesButton.Pushed = true;
                        this._fourPagesButton.Pushed = false;
                        this.OnVisiblePagesChanged();
                        return;

                    case 10:
                        this._preview.Rows = 2;
                        this._preview.Columns = 2;
                        this._onePageButton.Pushed = false;
                        this._twoPagesButton.Pushed = false;
                        this._fourPagesButton.Pushed = true;
                        this.OnVisiblePagesChanged();
                        return;
                }
            }
        }

        private void OnToolBarComboBoxCreated(object sender, ToolBarComboBoxButtonEventArgs e)
        {
            ComboBox comboBox = e.Button.ComboBox;
            comboBox.SelectedIndex = 0;
            comboBox.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedZoom);
            comboBox.Enabled = false;
        }

        private void OnVisiblePagesChanged()
        {
            int startPage = this._preview.StartPage;
            int num2 = Math.Min(this._preview.PagesPerView, this._pageCount - startPage);
            switch (num2)
            {
                case 1:
                    this._pageIndexPanel.Text = string.Format("Page {0} of {1}", startPage + 1, this._pageCount);
                    break;

                case 2:
                    this._pageIndexPanel.Text = string.Format("Pages {0} and {1} of {2}", startPage + 1, startPage + 2, this._pageCount);
                    break;

                default:
                    this._pageIndexPanel.Text = string.Format("Pages {0} to {1} of {2}", startPage + 1, startPage + num2, this._pageCount);
                    break;
            }
            bool flag = this._preview.StartPage == 0;
            bool flag2 = startPage < (this._pageCount - num2);
            this._firstButton.Enabled = !flag;
            this._prevButton.Enabled = !flag;
            this._nextButton.Enabled = flag2;
            this._lastButton.Enabled = flag2;
        }

        private sealed class PrintPreviewControl : Control
        {
            private bool _antiAlias;
            private bool _autoZoom = true;
            private int _columns = 1;
            private PrintDocument _document;
            private Exception _exceptionPrinting;
            private Size _imageSize;
            private Point _lastOffset;
            private bool _layoutComputed;
            private PreviewPageInfo[] _pageInfo;
            private bool _pageInfoCalcPending;
            private Point _position = new Point(0, 0);
            private int _rows = 1;
            private Point _screenDpi;
            private int _startPage;
            private Size _virtualSize = new Size(1, 1);
            private double _zoom = 0.3;
            private const ContentAlignment anyBottom = (ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft);
            private const ContentAlignment anyCenter = (ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter);
            private const ContentAlignment anyMiddle = (ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft);
            private const ContentAlignment anyRight = (ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight);
            private static readonly object EventStartPageChanged = new object();
            private const int LineScrollDelta = 5;
            private const int PageScrollDelta = 100;
            private const int PageSpacing = 10;

            public event EventHandler StartPageChanged
            {
                add
                {
                    base.Events.AddHandler(EventStartPageChanged, value);
                }
                remove
                {
                    base.Events.RemoveHandler(EventStartPageChanged, value);
                }
            }

            public PrintPreviewControl()
            {
                this.ResetBackColor();
                this.ResetForeColor();
                base.Size = new Size(100, 100);
                base.SetStyle(ControlStyles.ResizeRedraw, false);
                base.SetStyle(ControlStyles.Opaque, true);
            }

            private int AdjustScroll(Message m, int pos, int maxPos)
            {
                switch ((((int) m.WParam) & 0xffff))
                {
                    case 0:
                        if (pos <= 5)
                        {
                            pos = 0;
                            return pos;
                        }
                        pos -= 5;
                        return pos;

                    case 1:
                        if (pos >= (maxPos - 5))
                        {
                            pos = maxPos;
                            return pos;
                        }
                        pos += 5;
                        return pos;

                    case 2:
                        if (pos <= 100)
                        {
                            pos = 0;
                            return pos;
                        }
                        pos -= 100;
                        return pos;

                    case 3:
                        if (pos >= (maxPos - 100))
                        {
                            pos = maxPos;
                            return pos;
                        }
                        pos += 100;
                        return pos;

                    case 4:
                    case 5:
                        pos = ((int) m.WParam) >> 0x10;
                        return pos;
                }
                return pos;
            }

            private void CalculatePageInfo()
            {
                if (!this._pageInfoCalcPending)
                {
                    this._pageInfoCalcPending = true;
                    try
                    {
                        if (this._pageInfo == null)
                        {
                            try
                            {
                                this.ComputePreview();
                            }
                            catch (Exception exception)
                            {
                                this._exceptionPrinting = exception;
                            }
                            finally
                            {
                                base.Invalidate();
                            }
                        }
                    }
                    finally
                    {
                        this._pageInfoCalcPending = false;
                    }
                }
            }

            private void ComputeLayout()
            {
                this._layoutComputed = true;
                if (this._pageInfo.Length == 0)
                {
                    base.ClientSize = base.Size;
                }
                else
                {
                    Graphics graphics = base.CreateGraphics();
                    IntPtr hdc = graphics.GetHdc();
                    this._screenDpi = new Point(Interop.GetDeviceCaps(hdc, 0x58), Interop.GetDeviceCaps(hdc, 90));
                    graphics.ReleaseHdcInternal(hdc);
                    graphics.Dispose();
                    Size physicalSize = this._pageInfo[this.StartPage].PhysicalSize;
                    Size size2 = new Size(PixelsToPhysical(new Point(base.Size), this._screenDpi));
                    if (this._autoZoom)
                    {
                        double num = (size2.Width - (10 * (this._columns + 1))) / ((double) (this._columns * physicalSize.Width));
                        double num2 = (size2.Height - (10 * (this._rows + 1))) / ((double) (this._rows * physicalSize.Height));
                        this._zoom = Math.Min(num, num2);
                    }
                    this._imageSize = new Size((int) (this._zoom * physicalSize.Width), (int) (this._zoom * physicalSize.Height));
                    int x = (this._imageSize.Width * this._columns) + (10 * (this._columns + 1));
                    int y = (this._imageSize.Height * this._rows) + (10 * (this._rows + 1));
                    this.SetVirtualSizeNoInvalidate(new Size(PhysicalToPixels(new Point(x, y), this._screenDpi)));
                }
            }

            private void ComputePreview()
            {
                int startPage = this.StartPage;
                if (this._document == null)
                {
                    this._pageInfo = new PreviewPageInfo[0];
                }
                else
                {
                    PrintController printController = this._document.PrintController;
                    PreviewPrintController underlyingController = new PreviewPrintController();
                    underlyingController.UseAntiAlias = this.UseAntiAlias;
                    PrintController controller3 = new PrintPreviewDialog.PrintPreviewUIPrintController(underlyingController, (PrintPreviewDialog) base.Parent);
                    try
                    {
                        this._document.PrintController = controller3;
                        this._document.Print();
                        this._pageInfo = underlyingController.GetPreviewPageInfo();
                    }
                    finally
                    {
                        this._document.PrintController = printController;
                    }
                }
                if (startPage != this.StartPage)
                {
                    this.OnStartPageChanged(EventArgs.Empty);
                }
            }

            private void InvalidateLayout()
            {
                this._layoutComputed = false;
                base.Invalidate();
            }

            public void InvalidatePreview()
            {
                this._pageInfo = null;
                this.InvalidateLayout();
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                using (Brush brush = new SolidBrush(this.BackColor))
                {
                    if ((this._pageInfo == null) || (this._pageInfo.Length == 0))
                    {
                        pevent.Graphics.FillRectangle(brush, base.ClientRectangle);
                        if ((this._pageInfo != null) || (this._exceptionPrinting != null))
                        {
                            StringFormat format = new StringFormat();
                            format.Alignment = TranslateAlignment(ContentAlignment.MiddleCenter);
                            format.LineAlignment = TranslateLineAlignment(ContentAlignment.MiddleCenter);
                            SolidBrush brush2 = new SolidBrush(this.ForeColor);
                            try
                            {
                                StringBuilder builder = new StringBuilder();
                                builder.Append("Unable to display the print preview");
                                if (this._exceptionPrinting != null)
                                {
                                    builder.Append("\n\nDetails:\n");
                                    builder.Append(this._exceptionPrinting.Message);
                                }
                                pevent.Graphics.DrawString(builder.ToString(), this.Font, brush2, base.ClientRectangle, format);
                                return;
                            }
                            finally
                            {
                                brush2.Dispose();
                                format.Dispose();
                            }
                        }
                        base.BeginInvoke(new MethodInvoker(this.CalculatePageInfo));
                    }
                    else
                    {
                        if (!this._layoutComputed)
                        {
                            this.ComputeLayout();
                        }
                        Point point = PhysicalToPixels(new Point(this._imageSize), this._screenDpi);
                        Point point2 = new Point(this.VirtualSize);
                        Point point3 = new Point(Math.Max(0, (base.Size.Width - point2.X) / 2), Math.Max(0, (base.Size.Height - point2.Y) / 2));
                        point3.X -= this.Position.X;
                        point3.Y -= this.Position.Y;
                        this._lastOffset = point3;
                        int num = PhysicalToPixels(10, this._screenDpi.X);
                        int num2 = PhysicalToPixels(10, this._screenDpi.Y);
                        Region clip = pevent.Graphics.Clip;
                        Rectangle[] rectangleArray = new Rectangle[this._rows * this._columns];
                        try
                        {
                            for (int j = 0; j < this._rows; j++)
                            {
                                for (int k = 0; k < this._columns; k++)
                                {
                                    int num5 = (this.StartPage + k) + (j * this._columns);
                                    if (num5 < this._pageInfo.Length)
                                    {
                                        int x = (point3.X + (num * (k + 1))) + (point.X * k);
                                        int y = (point3.Y + (num2 * (j + 1))) + (point.Y * j);
                                        rectangleArray[num5 - this.StartPage] = new Rectangle(x, y, point.X, point.Y);
                                        pevent.Graphics.ExcludeClip(rectangleArray[num5 - this.StartPage]);
                                    }
                                }
                            }
                            pevent.Graphics.FillRectangle(brush, base.ClientRectangle);
                        }
                        finally
                        {
                            pevent.Graphics.Clip = clip;
                        }
                        for (int i = 0; i < rectangleArray.Length; i++)
                        {
                            if ((i + this.StartPage) < this._pageInfo.Length)
                            {
                                Rectangle rect = rectangleArray[i];
                                pevent.Graphics.DrawRectangle(Pens.Black, rect);
                                rect.Inflate(-1, -1);
                                pevent.Graphics.FillRectangle(new SolidBrush(this.ForeColor), rect);
                                if (this._pageInfo[i + this.StartPage].Image != null)
                                {
                                    pevent.Graphics.DrawImage(this._pageInfo[i + this.StartPage].Image, rect);
                                }
                                pevent.Graphics.DrawRectangle(Pens.Black, rect);
                            }
                        }
                    }
                }
            }

            protected override void OnResize(EventArgs eventargs)
            {
                if (this._autoZoom)
                {
                    this.InvalidateLayout();
                }
                else
                {
                    PhysicalToPixels(new Point(this._imageSize), this._screenDpi);
                    Point point = new Point(this.VirtualSize);
                    Point point2 = new Point(Math.Max(0, (base.Size.Width - point.X) / 2), Math.Max(0, (base.Size.Height - point.Y) / 2));
                    point2.X -= this.Position.X;
                    point2.Y -= this.Position.Y;
                    if ((this._lastOffset.X != point2.X) || (this._lastOffset.Y != point2.Y))
                    {
                        base.Invalidate();
                    }
                }
                base.OnResize(eventargs);
            }

            private void OnStartPageChanged(EventArgs e)
            {
                EventHandler handler = base.Events[EventStartPageChanged] as EventHandler;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            private static Point PhysicalToPixels(Point physical, Point dpi)
            {
                return new Point(PhysicalToPixels(physical.X, dpi.X), PhysicalToPixels(physical.Y, dpi.Y));
            }

            private static Size PhysicalToPixels(Size physicalSize, Point dpi)
            {
                return new Size(PhysicalToPixels(physicalSize.Width, dpi.X), PhysicalToPixels(physicalSize.Height, dpi.Y));
            }

            private static int PhysicalToPixels(int physicalSize, int dpi)
            {
                return (int) (((double) (physicalSize * dpi)) / 100.0);
            }

            private static Point PixelsToPhysical(Point pixels, Point dpi)
            {
                return new Point(PixelsToPhysical(pixels.X, dpi.X), PixelsToPhysical(pixels.Y, dpi.Y));
            }

            private static Size PixelsToPhysical(Size pixels, Point dpi)
            {
                return new Size(PixelsToPhysical(pixels.Width, dpi.X), PixelsToPhysical(pixels.Height, dpi.Y));
            }

            private static int PixelsToPhysical(int pixels, int dpi)
            {
                return (int) ((pixels * 100.0) / ((double) dpi));
            }

            public override void ResetBackColor()
            {
                this.BackColor = SystemColors.AppWorkspace;
            }

            public override void ResetForeColor()
            {
                this.ForeColor = Color.White;
            }

            private void SetPositionNoInvalidate(Point value)
            {
                Point point = this._position;
                this._position = value;
                this._position.X = Math.Min(this._position.X, this._virtualSize.Width - base.Width);
                this._position.Y = Math.Min(this._position.Y, this._virtualSize.Height - base.Height);
                if (this._position.X < 0)
                {
                    this._position.X = 0;
                }
                if (this._position.Y < 0)
                {
                    this._position.Y = 0;
                }
                Rectangle clientRectangle = base.ClientRectangle;
                Interop.RECT rectScrollRegion = Interop.RECT.FromXYWH(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
                Interop.ScrollWindow(base.Handle, point.X - this._position.X, point.Y - this._position.Y, ref rectScrollRegion, ref rectScrollRegion);
                Interop.SetScrollPos(base.Handle, 0, this._position.X, true);
                Interop.SetScrollPos(base.Handle, 1, this._position.Y, true);
            }

            private void SetVirtualSizeNoInvalidate(Size value)
            {
                this._virtualSize = value;
                this.SetPositionNoInvalidate(this._position);
                Interop.SCROLLINFO si = new Interop.SCROLLINFO();
                si.fMask = 3;
                si.nMin = 0;
                si.nMax = Math.Max(base.Height, this._virtualSize.Height) - 1;
                si.nPage = base.Height;
                Interop.SetScrollInfo(base.Handle, 1, si, true);
                si.fMask = 3;
                si.nMin = 0;
                si.nMax = Math.Max(base.Width, this._virtualSize.Width) - 1;
                si.nPage = base.Width;
                Interop.SetScrollInfo(base.Handle, 0, si, true);
            }

            private static StringAlignment TranslateAlignment(ContentAlignment align)
            {
                if ((align & (ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight)) != ((ContentAlignment) 0))
                {
                    return StringAlignment.Far;
                }
                if ((align & (ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter)) != ((ContentAlignment) 0))
                {
                    return StringAlignment.Center;
                }
                return StringAlignment.Near;
            }

            private static StringAlignment TranslateLineAlignment(ContentAlignment align)
            {
                if ((align & (ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft)) != ((ContentAlignment) 0))
                {
                    return StringAlignment.Far;
                }
                if ((align & (ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft)) != ((ContentAlignment) 0))
                {
                    return StringAlignment.Center;
                }
                return StringAlignment.Near;
            }

            private void WmHScroll(ref Message m)
            {
                if (m.LParam != IntPtr.Zero)
                {
                    base.WndProc(ref m);
                }
                else
                {
                    Point point = this._position;
                    int x = point.X;
                    int maxPos = Math.Max(base.Width, this._virtualSize.Width);
                    point.X = this.AdjustScroll(m, x, maxPos);
                    this.Position = point;
                }
            }

            private void WmKeyDown(ref Message msg)
            {
                Keys keys = ((Keys) ((int) msg.WParam)) | Control.ModifierKeys;
                switch ((keys & Keys.KeyCode))
                {
                    case Keys.Prior:
                        this.StartPage--;
                        return;

                    case Keys.Next:
                        this.StartPage++;
                        return;

                    case Keys.End:
                        if ((keys & ~Keys.KeyCode) != Keys.Control)
                        {
                            break;
                        }
                        this.StartPage = this._pageInfo.Length;
                        return;

                    case Keys.Home:
                        if ((keys & ~Keys.KeyCode) != Keys.Control)
                        {
                            break;
                        }
                        this.StartPage = 0;
                        return;

                    default:
                        return;
                }
            }

            private void WmVScroll(ref Message m)
            {
                if (m.LParam != IntPtr.Zero)
                {
                    base.WndProc(ref m);
                }
                else
                {
                    Point position = this.Position;
                    int y = position.Y;
                    int maxPos = Math.Max(base.Height, this._virtualSize.Height);
                    position.Y = this.AdjustScroll(m, y, maxPos);
                    this.Position = position;
                }
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case 0x114:
                        this.WmHScroll(ref m);
                        return;

                    case 0x115:
                        this.WmVScroll(ref m);
                        return;

                    case 0x100:
                        this.WmKeyDown(ref m);
                        return;
                }
                base.WndProc(ref m);
            }

            public bool AutoZoom
            {
                get
                {
                    return this._autoZoom;
                }
                set
                {
                    this._autoZoom = value;
                    this.InvalidateLayout();
                }
            }

            public int Columns
            {
                get
                {
                    return this._columns;
                }
                set
                {
                    this._columns = value;
                    this.InvalidateLayout();
                }
            }

            protected override System.Windows.Forms.CreateParams CreateParams
            {
                get
                {
                    System.Windows.Forms.CreateParams createParams = base.CreateParams;
                    createParams.Style |= 0x100000;
                    createParams.Style |= 0x200000;
                    return createParams;
                }
            }

            public PrintDocument Document
            {
                get
                {
                    return this._document;
                }
                set
                {
                    this._document = value;
                    this.InvalidatePreview();
                }
            }

            public int PagesPerView
            {
                get
                {
                    return (this._rows * this._columns);
                }
            }

            private Point Position
            {
                get
                {
                    return this._position;
                }
                set
                {
                    this.SetPositionNoInvalidate(value);
                }
            }

            public int Rows
            {
                get
                {
                    return this._rows;
                }
                set
                {
                    this._rows = value;
                    this.InvalidateLayout();
                }
            }

            public int StartPage
            {
                get
                {
                    int num = this._startPage;
                    if (this._pageInfo != null)
                    {
                        num = Math.Min(num, this._pageInfo.Length - (this._rows * this._columns));
                    }
                    return Math.Max(num, 0);
                }
                set
                {
                    int startPage = this.StartPage;
                    this._startPage = value;
                    if (startPage != this._startPage)
                    {
                        this.InvalidateLayout();
                        this.OnStartPageChanged(EventArgs.Empty);
                    }
                }
            }

            public bool UseAntiAlias
            {
                get
                {
                    return this._antiAlias;
                }
                set
                {
                    this._antiAlias = value;
                }
            }

            private Size VirtualSize
            {
                get
                {
                    return this._virtualSize;
                }
                set
                {
                    this.SetVirtualSizeNoInvalidate(value);
                    base.Invalidate();
                }
            }

            public double Zoom
            {
                get
                {
                    return this._zoom;
                }
                set
                {
                    if (value <= 0.0)
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }
                    this._autoZoom = false;
                    this._zoom = value;
                    this.InvalidateLayout();
                }
            }
        }

        private class PrintPreviewUIPrintController : PrintController
        {
            private PrintPreviewDialog _owner;
            private PrintController _underlyingController;

            public PrintPreviewUIPrintController(PrintController underlyingController, PrintPreviewDialog owner)
            {
                this._underlyingController = underlyingController;
                this._owner = owner;
            }

            public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
            {
                this._underlyingController.OnEndPage(document, e);
                this._owner.OnPreviewEndPage();
                base.OnEndPage(document, e);
            }

            public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
            {
                this._underlyingController.OnEndPrint(document, e);
                this._owner.OnPreviewEndPrint();
                base.OnEndPrint(document, e);
            }

            public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
            {
                base.OnStartPage(document, e);
                Graphics graphics = this._underlyingController.OnStartPage(document, e);
                this._owner.OnPreviewStartPage();
                return graphics;
            }

            public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
            {
                base.OnStartPrint(document, e);
                this._owner.OnPreviewStartPrint();
                this._underlyingController.OnStartPrint(document, e);
            }
        }
    }
}


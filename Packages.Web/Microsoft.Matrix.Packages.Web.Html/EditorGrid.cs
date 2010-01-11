namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Runtime.InteropServices;

    internal sealed class EditorGrid : Interop.IHTMLEditHost, Interop.IHTMLPainter, Interop.IElementBehavior
    {
        private int _gridSize;
        private bool _gridVisible;
        private int _halfGridSize;
        private bool _snapEnabled;
        private Interop.IElementBehaviorSite behaviorSite;
        public static readonly string Name = "RenderBehavior";
        private Interop.IHTMLPaintSite paintSite;
        public static readonly string URL = "Microsoft.Matrix";

        public EditorGrid() : this(8)
        {
        }

        public EditorGrid(int gridSize)
        {
            this.GridSize = gridSize;
        }

        void Interop.IElementBehavior.Detach()
        {
            this.paintSite = null;
            this.behaviorSite = null;
        }

        void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite pBehaviorSite)
        {
            this.behaviorSite = pBehaviorSite;
            this.paintSite = (Interop.IHTMLPaintSite) this.behaviorSite;
        }

        void Interop.IElementBehavior.Notify(int dwEvent, IntPtr pVar)
        {
        }

        void Interop.IHTMLEditHost.SnapRect(Interop.IHTMLElement pElement, Interop.COMRECT rcNew, int nHandle)
        {
            if (this._snapEnabled)
            {
                switch (nHandle)
                {
                    case 0:
                    {
                        int num = rcNew.right - rcNew.left;
                        int num2 = rcNew.bottom - rcNew.top;
                        rcNew.top = ((rcNew.top + this._halfGridSize) / this._gridSize) * this._gridSize;
                        rcNew.left = ((rcNew.left + this._halfGridSize) / this._gridSize) * this._gridSize;
                        rcNew.bottom = rcNew.top + num2;
                        rcNew.right = rcNew.left + num;
                        return;
                    }
                    case 1:
                        rcNew.top = ((rcNew.top + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;

                    case 2:
                        rcNew.left = ((rcNew.left + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;

                    case 3:
                        rcNew.bottom = ((rcNew.bottom + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;

                    case 4:
                        rcNew.right = ((rcNew.right + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;

                    case 5:
                        rcNew.top = ((rcNew.top + this._halfGridSize) / this._gridSize) * this._gridSize;
                        rcNew.left = ((rcNew.left + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;

                    case 6:
                        rcNew.top = ((rcNew.top + this._halfGridSize) / this._gridSize) * this._gridSize;
                        rcNew.right = ((rcNew.right + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;

                    case 7:
                        rcNew.bottom = ((rcNew.bottom + this._halfGridSize) / this._gridSize) * this._gridSize;
                        rcNew.left = ((rcNew.left + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;

                    case 8:
                        rcNew.bottom = ((rcNew.bottom + this._halfGridSize) / this._gridSize) * this._gridSize;
                        rcNew.right = ((rcNew.right + this._halfGridSize) / this._gridSize) * this._gridSize;
                        return;
                }
            }
        }

        void Interop.IHTMLPainter.Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            if (this._gridVisible)
            {
                for (int i = leftBounds; i < rightBounds; i += this._gridSize)
                {
                    for (int j = topBounds; j < bottomBounds; j += this._gridSize)
                    {
                        Interop.SetPixel(hdc, i, j, 0);
                    }
                }
            }
        }

        void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO htmlPainterInfo)
        {
            htmlPainterInfo.lFlags = 2;
            htmlPainterInfo.lZOrder = 4;
            htmlPainterInfo.iidDrawObject = Guid.Empty;
            htmlPainterInfo.rcBounds.left = 0;
            htmlPainterInfo.rcBounds.top = 0;
            htmlPainterInfo.rcBounds.right = 0;
            htmlPainterInfo.rcBounds.bottom = 0;
        }

        bool Interop.IHTMLPainter.HitTestPoint(int ptx, int pty, int[] pbHit, int[] plPartID)
        {
            return false;
        }

        void Interop.IHTMLPainter.OnResize(int cx, int cy)
        {
        }

        public int GridSize
        {
            get
            {
                return this._gridSize;
            }
            set
            {
                this._gridSize = value;
                this._halfGridSize = this._gridSize / 2;
                if (this.paintSite != null)
                {
                    this.paintSite.InvalidateRect(Interop.NullIntPtr);
                }
            }
        }

        public bool GridVisible
        {
            get
            {
                return this._gridVisible;
            }
            set
            {
                this._gridVisible = value;
                if (this.paintSite != null)
                {
                    this.paintSite.InvalidateRect(Interop.NullIntPtr);
                }
            }
        }

        public bool SnapEnabled
        {
            get
            {
                return this._snapEnabled;
            }
            set
            {
                this._snapEnabled = value;
            }
        }
    }
}


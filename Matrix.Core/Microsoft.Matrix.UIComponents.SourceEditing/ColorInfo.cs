namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;

    public class ColorInfo
    {
        private Color _background;
        private bool _bold;
        private Color _foreground;
        private bool _italic;
        private int _win32Background;
        private int _win32Foreground;

        public ColorInfo(Color foreground, Color background, bool bold, bool italic)
        {
            this._foreground = foreground;
            this._win32Foreground = ColorTranslator.ToWin32(foreground);
            this._background = background;
            this._win32Background = ColorTranslator.ToWin32(background);
            this._bold = bold;
            this._italic = italic;
        }

        internal void RefreshWin32Colors()
        {
            this._win32Foreground = ColorTranslator.ToWin32(this._foreground);
            this._win32Background = ColorTranslator.ToWin32(this._background);
        }

        internal void SetupHdc(IntPtr hdc)
        {
            Interop.SetBkColor(hdc, this._win32Background);
            Interop.SetTextColor(hdc, this._win32Foreground);
        }

        internal IntPtr SetupHdc(IntPtr hdc, IntPtr fontHandle)
        {
            Interop.SetBkColor(hdc, this._win32Background);
            Interop.SetTextColor(hdc, this._win32Foreground);
            return Interop.SelectObject(hdc, fontHandle);
        }

        public Color Background
        {
            get
            {
                return this._background;
            }
        }

        public bool Bold
        {
            get
            {
                return this._bold;
            }
        }

        public Color Foreground
        {
            get
            {
                return this._foreground;
            }
        }

        public bool IsStyledFont
        {
            get
            {
                if (!this._bold)
                {
                    return this._italic;
                }
                return true;
            }
        }

        public bool Italic
        {
            get
            {
                return this._italic;
            }
        }
    }
}


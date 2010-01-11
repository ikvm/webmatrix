namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BoxEdges
    {
        private int _left;
        private int _top;
        private int _right;
        private int _bottom;
        public BoxEdges(int left, int top, int right, int bottom)
        {
            if (left < 0)
            {
                throw new ArgumentOutOfRangeException("left");
            }
            if (top < 0)
            {
                throw new ArgumentOutOfRangeException("top");
            }
            if (right < 0)
            {
                throw new ArgumentOutOfRangeException("right");
            }
            if (bottom < 0)
            {
                throw new ArgumentOutOfRangeException("bottom");
            }
            this._left = left;
            this._top = top;
            this._right = right;
            this._bottom = bottom;
        }

        public int Bottom
        {
            get
            {
                return this._bottom;
            }
        }
        public int Left
        {
            get
            {
                return this._left;
            }
        }
        public int Right
        {
            get
            {
                return this._right;
            }
        }
        public int Top
        {
            get
            {
                return this._top;
            }
        }
        public override bool Equals(object o)
        {
            if ((o == null) || !(o is BoxEdges))
            {
                return false;
            }
            BoxEdges edges = (BoxEdges) o;
            return (this == edges);
        }

        public override int GetHashCode()
        {
            return (((this._top ^ this._left) ^ this._bottom) ^ this._right);
        }

        public static bool operator ==(BoxEdges b1, BoxEdges b2)
        {
            return ((((b1._top == b2._top) && (b1._left == b2._left)) && (b1._bottom == b2._bottom)) && (b1._right == b2._right));
        }

        public static bool operator !=(BoxEdges b1, BoxEdges b2)
        {
            if (((b1._top == b2._top) && (b1._left == b2._left)) && (b1._bottom == b2._bottom))
            {
                return (b1._right != b2._right);
            }
            return true;
        }
    }
}


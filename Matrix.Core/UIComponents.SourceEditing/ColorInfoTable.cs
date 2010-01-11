namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Reflection;

    public class ColorInfoTable
    {
        private ColorInfo[] _infos;
        public const int Comment = 3;
        public const int Control = 7;
        public const int ControlDark = 8;
        public const int FirstUserColorIndex = 0x40;
        public const int InactiveSelectedText = 6;
        public const int Keyword = 1;
        public const int LineNumbers = 4;
        public const int PlainText = 0;
        public const int SelectedText = 5;
        public const int String = 2;

        internal ColorInfoTable(int size)
        {
            this._infos = new ColorInfo[size];
        }

        internal ColorInfo this[int index]
        {
            get
            {
                return this._infos[index];
            }
            set
            {
                this._infos[index] = value;
            }
        }
    }
}


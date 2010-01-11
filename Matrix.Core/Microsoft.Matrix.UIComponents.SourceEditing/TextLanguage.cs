namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Windows.Forms;

    public class TextLanguage : ITextLanguage
    {
        private static ITextColorizer defaultColorizer;
        public static ITextLanguage Instance = new TextLanguage();

        public virtual ITextColorizer GetColorizer(IServiceProvider provider)
        {
            if (defaultColorizer == null)
            {
                defaultColorizer = new PlainTextColorizer();
            }
            return defaultColorizer;
        }

        public IDataObject GetDataObjectFromText(string text)
        {
            return new DataObject(DataFormats.Text, text);
        }

        public virtual ITextControlHost GetTextControlHost(TextControl control, IServiceProvider provider)
        {
            return null;
        }

        public string GetTextFromDataObject(IDataObject dataObject, IServiceProvider provider)
        {
            return (dataObject.GetData(DataFormats.Text) as string);
        }

        public virtual TextBufferSpan GetWordSpan(TextBufferLocation location, WordType type)
        {
            TextBufferLocation location4;
            TextLine line2;
            int columnIndex;
            if (type != WordType.Previous)
            {
                if (type != WordType.Next)
                {
                    if (location.Line.Length == 0)
                    {
                        return null;
                    }
                    TextBufferLocation start = location.Clone();
                    if (start.ColumnIndex >= start.Line.Length)
                    {
                        start.ColumnIndex = start.Line.Length - 1;
                    }
                    TextBufferLocation location7 = location.Clone();
                    if (!this.IsSeperatorChar(start.Line[start.ColumnIndex]))
                    {
                        while ((start.ColumnIndex > 0) && !this.IsSeperatorChar(start.Line[start.ColumnIndex - 1]))
                        {
                            start.ColumnIndex--;
                        }
                        while ((location7.ColumnIndex < location7.Line.Length) && !this.IsSeperatorChar(location7.Line[location7.ColumnIndex]))
                        {
                            location7.ColumnIndex++;
                        }
                    }
                    else
                    {
                        while (((start.ColumnIndex > 0) && this.IsSeperatorChar(start.Line[start.ColumnIndex - 1])) && !char.IsWhiteSpace(start.Line[start.ColumnIndex - 1]))
                        {
                            start.ColumnIndex--;
                        }
                        while (((location7.ColumnIndex < location7.Line.Length) && this.IsSeperatorChar(location7.Line[location7.ColumnIndex])) && !char.IsWhiteSpace(start.Line[location7.ColumnIndex]))
                        {
                            location7.ColumnIndex++;
                        }
                    }
                    return new TextBufferSpan(start, location7);
                }
                location4 = location.Clone();
                line2 = location4.Line;
                columnIndex = location4.ColumnIndex;
                bool flag = false;
                if (columnIndex == line2.Length)
                {
                    if (location4.MoveDown(1) > 0)
                    {
                        line2 = location4.Line;
                        columnIndex = 0;
                        flag = true;
                    }
                    else
                    {
                        columnIndex = line2.Length;
                    }
                }
                if (!flag)
                {
                    if ((columnIndex >= line2.Length) || !this.IsSeperatorChar(line2[columnIndex]))
                    {
                        while ((columnIndex < line2.Length) && !this.IsSeperatorChar(line2[columnIndex]))
                        {
                            columnIndex++;
                        }
                    }
                    else
                    {
                        while ((columnIndex < line2.Length) && this.IsSeperatorChar(line2[columnIndex]))
                        {
                            columnIndex++;
                        }
                    }
                }
            }
            else
            {
                TextBufferLocation location2 = location.Clone();
                TextLine line = location2.Line;
                int num = location2.ColumnIndex;
                TextBufferLocation location3 = location2.Clone();
                if (num != 0)
                {
                    while ((num > 0) && char.IsWhiteSpace(line[num - 1]))
                    {
                        num--;
                    }
                    if ((num <= 0) || !this.IsSeperatorChar(line[num - 1]))
                    {
                        while ((num > 0) && !this.IsSeperatorChar(line[num - 1]))
                        {
                            num--;
                        }
                    }
                    else
                    {
                        while ((num > 0) && this.IsSeperatorChar(line[num - 1]))
                        {
                            num--;
                        }
                    }
                }
                else
                {
                    if (location2.MoveUp(1) > 0)
                    {
                        location2.ColumnIndex = location2.Line.Length;
                        return new TextBufferSpan(location2, location3);
                    }
                    location2.ColumnIndex = 0;
                    return new TextBufferSpan(location2, location3);
                }
                location2.ColumnIndex = num;
                return new TextBufferSpan(location2, location3);
            }
            while ((columnIndex < line2.Length) && char.IsWhiteSpace(line2[columnIndex]))
            {
                columnIndex++;
            }
            location4.ColumnIndex = columnIndex;
            TextBufferLocation end = location4.Clone();
            if ((end.ColumnIndex >= end.Line.Length) || !this.IsSeperatorChar(end.Line[end.ColumnIndex]))
            {
                while ((end.ColumnIndex < end.Line.Length) && !this.IsSeperatorChar(end.Line[end.ColumnIndex]))
                {
                    end.ColumnIndex++;
                }
            }
            else
            {
                while ((end.ColumnIndex < end.Line.Length) && this.IsSeperatorChar(end.Line[end.ColumnIndex]))
                {
                    end.ColumnIndex++;
                }
            }
            return new TextBufferSpan(location4, end);
        }

        protected virtual bool IsSeperatorChar(char c)
        {
            return (!char.IsLetterOrDigit(c) && (c != '_'));
        }

        public void ShowHelp(IServiceProvider provider, TextBufferLocation location)
        {
        }

        public bool SupportsDataObject(IServiceProvider provider, IDataObject dataObject)
        {
            return dataObject.GetDataPresent(DataFormats.Text);
        }
    }
}


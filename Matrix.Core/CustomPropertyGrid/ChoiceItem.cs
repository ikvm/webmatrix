using System;
using System.Collections.Generic;
using System.Text;

namespace Kingsoft.Blaze.WorldEditor.CustomPropertyGrid
{
    public class ChoiceItem
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public ChoiceItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}

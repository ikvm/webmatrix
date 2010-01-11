namespace Microsoft.Matrix.Packages.Code.VisualBasic
{
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Collections;

    public class VisualBasicColorizer : ITextColorizer, IDisposable
    {
        private static IDictionary delimiters = new Hashtable();
        private const string delimiterString = " \n,()\r\t:+-/*^[]\"&<>=.\\#";
        private static IDictionary keywords;

        static VisualBasicColorizer()
        {
            for (int i = 0; i < " \n,()\r\t:+-/*^[]\"&<>=.\\#".Length; i++)
            {
                delimiters.Add(" \n,()\r\t:+-/*^[]\"&<>=.\\#"[i], string.Empty);
            }
            keywords = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            keywords.Add("AddHandler", string.Empty);
            keywords.Add("AddressOf", string.Empty);
            keywords.Add("Alias", string.Empty);
            keywords.Add("And", string.Empty);
            keywords.Add("AndAlso", string.Empty);
            keywords.Add("Ansi", string.Empty);
            keywords.Add("As", string.Empty);
            keywords.Add("Assembly", string.Empty);
            keywords.Add("Auto", string.Empty);
            keywords.Add("Boolean", string.Empty);
            keywords.Add("ByRef", string.Empty);
            keywords.Add("Byte", string.Empty);
            keywords.Add("ByVal", string.Empty);
            keywords.Add("Call", string.Empty);
            keywords.Add("Case", string.Empty);
            keywords.Add("Catch", string.Empty);
            keywords.Add("CBool", string.Empty);
            keywords.Add("CByte", string.Empty);
            keywords.Add("CChar", string.Empty);
            keywords.Add("CDate", string.Empty);
            keywords.Add("CDec", string.Empty);
            keywords.Add("CDbl", string.Empty);
            keywords.Add("Char", string.Empty);
            keywords.Add("CInt", string.Empty);
            keywords.Add("Class", string.Empty);
            keywords.Add("CLng", string.Empty);
            keywords.Add("CObj", string.Empty);
            keywords.Add("Const", string.Empty);
            keywords.Add("CShort", string.Empty);
            keywords.Add("CSng", string.Empty);
            keywords.Add("CStr", string.Empty);
            keywords.Add("CType", string.Empty);
            keywords.Add("Date", string.Empty);
            keywords.Add("Decimal", string.Empty);
            keywords.Add("Declare", string.Empty);
            keywords.Add("Default", string.Empty);
            keywords.Add("Delegate", string.Empty);
            keywords.Add("Dim", string.Empty);
            keywords.Add("DirectCast", string.Empty);
            keywords.Add("Do", string.Empty);
            keywords.Add("Double", string.Empty);
            keywords.Add("Each", string.Empty);
            keywords.Add("Else", string.Empty);
            keywords.Add("ElseIf", string.Empty);
            keywords.Add("End", string.Empty);
            keywords.Add("Enum", string.Empty);
            keywords.Add("Erase", string.Empty);
            keywords.Add("Error", string.Empty);
            keywords.Add("Event", string.Empty);
            keywords.Add("Exit", string.Empty);
            keywords.Add("False", string.Empty);
            keywords.Add("Finally", string.Empty);
            keywords.Add("For", string.Empty);
            keywords.Add("Friend", string.Empty);
            keywords.Add("Function", string.Empty);
            keywords.Add("Get", string.Empty);
            keywords.Add("GetType", string.Empty);
            keywords.Add("GoSub", string.Empty);
            keywords.Add("Goto", string.Empty);
            keywords.Add("Handles", string.Empty);
            keywords.Add("If", string.Empty);
            keywords.Add("Implements", string.Empty);
            keywords.Add("Imports", string.Empty);
            keywords.Add("In", string.Empty);
            keywords.Add("Inherits", string.Empty);
            keywords.Add("Integer", string.Empty);
            keywords.Add("Interface", string.Empty);
            keywords.Add("Is", string.Empty);
            keywords.Add("Let", string.Empty);
            keywords.Add("Lib", string.Empty);
            keywords.Add("Like", string.Empty);
            keywords.Add("Long", string.Empty);
            keywords.Add("Loop", string.Empty);
            keywords.Add("Me", string.Empty);
            keywords.Add("Mod", string.Empty);
            keywords.Add("Module", string.Empty);
            keywords.Add("MustInherit", string.Empty);
            keywords.Add("MustOverride", string.Empty);
            keywords.Add("MyBase", string.Empty);
            keywords.Add("MyClass", string.Empty);
            keywords.Add("Namespace", string.Empty);
            keywords.Add("New", string.Empty);
            keywords.Add("Next", string.Empty);
            keywords.Add("Not", string.Empty);
            keywords.Add("Nothing", string.Empty);
            keywords.Add("NotInheritable", string.Empty);
            keywords.Add("NotOverridable", string.Empty);
            keywords.Add("Object", string.Empty);
            keywords.Add("On", string.Empty);
            keywords.Add("Option", string.Empty);
            keywords.Add("Optional", string.Empty);
            keywords.Add("Or", string.Empty);
            keywords.Add("OrElse", string.Empty);
            keywords.Add("Overloads", string.Empty);
            keywords.Add("Overridable", string.Empty);
            keywords.Add("ParamArray", string.Empty);
            keywords.Add("Preserve", string.Empty);
            keywords.Add("Private", string.Empty);
            keywords.Add("Property", string.Empty);
            keywords.Add("Protected", string.Empty);
            keywords.Add("Public", string.Empty);
            keywords.Add("RaiseEvent", string.Empty);
            keywords.Add("ReadOnly", string.Empty);
            keywords.Add("ReDim", string.Empty);
            keywords.Add("REM", string.Empty);
            keywords.Add("RemoveHandler", string.Empty);
            keywords.Add("Resume", string.Empty);
            keywords.Add("Return", string.Empty);
            keywords.Add("Select", string.Empty);
            keywords.Add("Set", string.Empty);
            keywords.Add("Shadows", string.Empty);
            keywords.Add("Shared", string.Empty);
            keywords.Add("Short", string.Empty);
            keywords.Add("Single", string.Empty);
            keywords.Add("Static", string.Empty);
            keywords.Add("Step", string.Empty);
            keywords.Add("Stop", string.Empty);
            keywords.Add("String", string.Empty);
            keywords.Add("Structure", string.Empty);
            keywords.Add("Sub", string.Empty);
            keywords.Add("SyncLock", string.Empty);
            keywords.Add("Then", string.Empty);
            keywords.Add("Throw", string.Empty);
            keywords.Add("To", string.Empty);
            keywords.Add("True", string.Empty);
            keywords.Add("Try", string.Empty);
            keywords.Add("TypeOf", string.Empty);
            keywords.Add("Unicode", string.Empty);
            keywords.Add("Until", string.Empty);
            keywords.Add("Variant", string.Empty);
            keywords.Add("Wend", string.Empty);
            keywords.Add("When", string.Empty);
            keywords.Add("While", string.Empty);
            keywords.Add("With", string.Empty);
            keywords.Add("WithEvents", string.Empty);
            keywords.Add("WriteOnly", string.Empty);
            keywords.Add("Xor", string.Empty);
            keywords.Add("#Const", string.Empty);
            keywords.Add("#If", string.Empty);
            keywords.Add("#Else", string.Empty);
            keywords.Add("#End", string.Empty);
            keywords.Add("#ElseIf", string.Empty);
            keywords.Add("#ExternalSource", string.Empty);
            keywords.Add("#Region", string.Empty);
            keywords.Add("Region", string.Empty);
        }

        public int Colorize(char[] text, byte[] colors, int startIndex, int endIndex, int intialColorState)
        {
            if ((text != null) && (text.Length != 0))
            {
                while ((startIndex < endIndex) && char.IsWhiteSpace(text[startIndex]))
                {
                    colors[startIndex] = 0;
                    startIndex++;
                }
                if (startIndex < endIndex)
                {
                    int index = startIndex;
                    while (index < endIndex)
                    {
                        if (text[startIndex] == '\'')
                        {
                            this.Fill(colors, startIndex, endIndex - startIndex, 3);
                            return 0;
                        }
                        char key = text[index];
                        bool flag = index == (endIndex - 1);
                        bool flag2 = delimiters.Contains(key);
                        if (flag2 || flag)
                        {
                            int length = index - startIndex;
                            if (length == 0)
                            {
                                this.Fill(colors, startIndex, 1, 0);
                            }
                            else
                            {
                                if (!flag2 && flag)
                                {
                                    length++;
                                }
                                string str = new string(text, startIndex, length);
                                if (keywords.Contains(str))
                                {
                                    this.Fill(colors, startIndex, length, 1);
                                }
                                else
                                {
                                    this.Fill(colors, startIndex, length, 0);
                                }
                                if (flag2)
                                {
                                    colors[index] = 0;
                                }
                                startIndex = index;
                            }
                            switch (key)
                            {
                                case '\\':
                                    index += 2;
                                    break;

                                case '"':
                                {
                                    bool flag3 = false;
                                    index++;
                                    while (!flag3)
                                    {
                                        if (index < endIndex)
                                        {
                                            if (text[index] == '"')
                                            {
                                                if (((index + 1) < endIndex) && (text[index + 1] == '"'))
                                                {
                                                    index++;
                                                }
                                                else
                                                {
                                                    flag3 = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            flag3 = true;
                                            if (text[endIndex - 1] != '"')
                                            {
                                                index--;
                                            }
                                        }
                                        if (flag3)
                                        {
                                            this.Fill(colors, startIndex, (index - startIndex) + 1, 2);
                                        }
                                        else
                                        {
                                            index++;
                                        }
                                    }
                                    break;
                                }
                            }
                            index++;
                            startIndex = index;
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
                return 0;
            }
            return intialColorState;
        }

        public void Dispose()
        {
        }

        private void Fill(byte[] data, int startIndex, int length, byte value)
        {
            for (int i = startIndex; i < (startIndex + length); i++)
            {
                data[i] = value;
            }
        }

        public ColorInfo[] ColorTable
        {
            get
            {
                return new ColorInfo[0];
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Utility
{
    using System;

    internal sealed class HtmlTokenizer
    {
        public static Token GetFirstToken(char[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            return GetNextToken(chars, chars.Length, 0, 0);
        }

        public static Token GetFirstToken(char[] chars, int length, int initialState)
        {
            return GetNextToken(chars, length, 0, initialState);
        }

        public static Token GetNextToken(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            return GetNextToken(token.Chars, token.CharsLength, token.EndIndex, token.EndState);
        }

        public static Token GetNextToken(char[] chars, int length, int startIndex, int startState)
        {
            int num12;
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            if (startIndex >= length)
            {
                return null;
            }
            int endState = startState;
            bool flag = (startState & 0x100) != 0;
            int num2 = flag ? 0x100 : 0;
            bool flag2 = (startState & 0x200) != 0;
            int num3 = flag2 ? 0x200 : 0;
            bool flag3 = (startState & 0x400) != 0;
            int num4 = flag3 ? 0x400 : 0;
            bool flag4 = (startState & 0x800) != 0;
            int num5 = flag4 ? 0x800 : 0;
            int index = startIndex;
            int num7 = startIndex;
            int endIndex = startIndex;
            Token token = null;
            while ((token == null) && (index < length))
            {
                string str;
                char c = chars[index];
                switch ((endState & 0xff))
                {
                    case 100:
                        if (c != '-')
                        {
                            goto Label_0476;
                        }
                        endState = 0x65;
                        goto Label_0C11;

                    case 0x65:
                        if (c != '-')
                        {
                            goto Label_049D;
                        }
                        endState = 0x66;
                        goto Label_0C11;

                    case 0x66:
                        if (c == '-')
                        {
                            endState = 0x67;
                        }
                        goto Label_0C11;

                    case 0x67:
                        if (c != '-')
                        {
                            goto Label_04C4;
                        }
                        endState = 0x68;
                        goto Label_0C11;

                    case 0x68:
                        if (!char.IsWhiteSpace(c))
                        {
                            if (c != '>')
                            {
                                goto Label_04F9;
                            }
                            endState = 0x11;
                            endIndex = index;
                            token = new Token(7, endState, num7, endIndex, chars, length);
                        }
                        goto Label_0C11;

                    case 60:
                        if (c == '>')
                        {
                            endState = 0x11;
                            endIndex = index;
                            token = new Token(0x18, endState, num7, endIndex, chars, length);
                        }
                        goto Label_0C11;

                    case 0:
                        if (c == '<')
                        {
                            endState = 1;
                            endIndex = index;
                            token = new Token(4, endState, num7, endIndex, chars, length);
                        }
                        goto Label_0C11;

                    case 1:
                        if (c != '<')
                        {
                            goto Label_0200;
                        }
                        if (((index + 1) >= length) || (chars[index + 1] != '%'))
                        {
                            break;
                        }
                        endState = (30 | num2) | num3;
                        num7 = index;
                        goto Label_0C11;

                    case 2:
                        if (c != '/')
                        {
                            goto Label_022D;
                        }
                        endState = (3 | num2) | num3;
                        endIndex = index;
                        token = new Token(6, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 3:
                        if (c != '/')
                        {
                            goto Label_02DF;
                        }
                        endState = (4 | num2) | num3;
                        endIndex = index + 1;
                        token = new Token(12, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 4:
                        if (!IsWordChar(c))
                        {
                            goto Label_0300;
                        }
                        endState = (5 | num2) | num3;
                        num7 = index;
                        goto Label_0C11;

                    case 5:
                        if (!IsWhitespace(c))
                        {
                            goto Label_0378;
                        }
                        endState = 6;
                        endIndex = index;
                        str = new string(chars, num7, endIndex - num7);
                        if (!str.ToLower().Equals("script"))
                        {
                            goto Label_0346;
                        }
                        if (!flag)
                        {
                            endState |= 0x100;
                        }
                        goto Label_0364;

                    case 6:
                        if (!IsWordChar(c))
                        {
                            goto Label_0551;
                        }
                        endState = ((7 | num2) | num3) | num5;
                        endIndex = index;
                        token = new Token(0, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 7:
                        if (IsWhitespace(c))
                        {
                            endState = ((8 | num2) | num3) | num5;
                            endIndex = index;
                            if (flag && (new string(chars, num7, endIndex - num7).ToLower() == "runat"))
                            {
                                endState |= 0x400;
                            }
                            token = new Token(2, endState, num7, endIndex, chars, length);
                        }
                        else
                        {
                            switch (c)
                            {
                                case '>':
                                    endState = ((0x11 | num2) | num3) | num5;
                                    endIndex = index;
                                    token = new Token(2, endState, num7, endIndex, chars, length);
                                    goto Label_0C11;

                                case '/':
                                    endState = (15 | num2) | num3;
                                    endIndex = index;
                                    token = new Token(2, endState, num7, endIndex, chars, length);
                                    goto Label_0C11;

                                case '=':
                                    endState = ((8 | num2) | num3) | num5;
                                    endIndex = index;
                                    if (flag && (new string(chars, num7, endIndex - num7).ToLower() == "runat"))
                                    {
                                        endState |= 0x400;
                                    }
                                    token = new Token(2, endState, num7, endIndex, chars, length);
                                    goto Label_0C11;
                            }
                            if (!IsWordChar(c))
                            {
                                endState = 0x10;
                            }
                        }
                        goto Label_0C11;

                    case 8:
                        if (c != '=')
                        {
                            goto Label_06EF;
                        }
                        endState = (((9 | num2) | num3) | num4) | num5;
                        num7 = index;
                        endIndex = index + 1;
                        token = new Token(15, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 9:
                        if (c != '\'')
                        {
                            goto Label_07DB;
                        }
                        endState = (((20 | num2) | num3) | num4) | num5;
                        endIndex = index;
                        token = new Token(0, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 10:
                        if (c == '"')
                        {
                            endState = ((11 | num2) | num3) | num5;
                            endIndex = index;
                            if (flag3 && (new string(chars, num7, endIndex - num7).ToLower() == "server"))
                            {
                                endState |= 0x800;
                            }
                            token = new Token(3, endState, num7, endIndex, chars, length);
                        }
                        goto Label_0C11;

                    case 11:
                        if (c != '"')
                        {
                            goto Label_0902;
                        }
                        endState = ((6 | num2) | num3) | num5;
                        endIndex = index + 1;
                        token = new Token(13, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 12:
                        if (c == '\'')
                        {
                            endState = ((13 | num2) | num3) | num5;
                            endIndex = index;
                            if (flag3 && (new string(chars, num7, endIndex - num7).ToLower() == "server"))
                            {
                                endState |= 0x800;
                            }
                            token = new Token(3, endState, num7, endIndex, chars, length);
                        }
                        goto Label_0C11;

                    case 13:
                        if (c != '\'')
                        {
                            goto Label_09C2;
                        }
                        endState = ((6 | num2) | num3) | num5;
                        endIndex = index + 1;
                        token = new Token(14, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 14:
                        if (IsWhitespace(c))
                        {
                            endState = ((6 | num2) | num3) | num5;
                            endIndex = index;
                            if (flag3 && (new string(chars, num7, endIndex - num7).ToLower() == "server"))
                            {
                                endState |= 0x800;
                            }
                            token = new Token(3, endState, num7, endIndex, chars, length);
                        }
                        else if (c == '>')
                        {
                            endState = ((0x11 | num2) | num3) | num5;
                            endIndex = index;
                            if (flag3 && (new string(chars, num7, endIndex - num7).ToLower() == "server"))
                            {
                                endState |= 0x800;
                            }
                            token = new Token(3, endState, num7, endIndex, chars, length);
                        }
                        else if (((c == '/') && ((index + 1) < length)) && (chars[index + 1] == '>'))
                        {
                            endState = ((15 | num2) | num3) | num5;
                            endIndex = index;
                            if (flag3 && (new string(chars, num7, endIndex - num7).ToLower() == "server"))
                            {
                                endState |= 0x800;
                            }
                            token = new Token(3, endState, num7, endIndex, chars, length);
                        }
                        goto Label_0C11;

                    case 15:
                        if (((c != '/') || ((index + 1) >= length)) || (chars[index + 1] != '>'))
                        {
                            goto Label_0B12;
                        }
                        endState = 0;
                        endIndex = index + 2;
                        token = new Token(5, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 0x10:
                        if (c == '>')
                        {
                            endState = 0x11;
                            endIndex = index;
                            token = new Token(8, endState, num7, endIndex, chars, length);
                        }
                        goto Label_0C11;

                    case 0x11:
                        if (c != '>')
                        {
                            goto Label_0B5A;
                        }
                        if (!flag)
                        {
                            goto Label_0B30;
                        }
                        endState = ((40 | num2) | num3) | num5;
                        goto Label_0B3F;

                    case 0x12:
                        if (c != '=')
                        {
                            goto Label_07A7;
                        }
                        endState = (((9 | num2) | num3) | num4) | num5;
                        endIndex = index + 1;
                        token = new Token(15, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 0x13:
                        if (c != '"')
                        {
                            goto Label_0879;
                        }
                        endState = (((10 | num2) | num3) | num4) | num5;
                        endIndex = index + 1;
                        token = new Token(13, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 20:
                        if (c != '\'')
                        {
                            goto Label_0939;
                        }
                        endState = (((12 | num2) | num3) | num4) | num5;
                        endIndex = index + 1;
                        token = new Token(14, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case 30:
                    {
                        int num9 = IndexOf(chars, index, length, "%>");
                        if (num9 <= -1)
                        {
                            goto Label_02A9;
                        }
                        endState = 0;
                        endIndex = num9 + 2;
                        token = new Token(0x16, endState, num7, endIndex, chars, length);
                        goto Label_0C11;
                    }
                    case 40:
                    {
                        int num10 = IndexOf(chars, index, length, "</script>");
                        if (num10 <= -1)
                        {
                            goto Label_0BAD;
                        }
                        endState = ((1 | num2) | num3) | num5;
                        endIndex = num10;
                        if (!flag4)
                        {
                            goto Label_0B9B;
                        }
                        token = new Token(0x17, endState, num7, endIndex, chars, length);
                        goto Label_0C11;
                    }
                    case 50:
                    {
                        int num11 = IndexOf(chars, index, length, "</style>");
                        if (num11 > -1)
                        {
                            endState = (1 | num2) | num3;
                            endIndex = num11;
                            token = new Token(0x15, endState, num7, endIndex, chars, length);
                        }
                        else
                        {
                            index = length - 1;
                            endIndex = index;
                        }
                        goto Label_0C11;
                    }
                    default:
                        goto Label_0C11;
                }
                endState = (2 | num2) | num3;
                endIndex = index + 1;
                token = new Token(10, endState, num7, endIndex, chars, length);
                goto Label_0C11;
            Label_0200:
                endState = 0x10;
                goto Label_0C11;
            Label_022D:
                switch (c)
                {
                    case '!':
                        endState = (100 | num2) | num3;
                        num7 = index;
                        goto Label_0C11;

                    case '%':
                        endState = 30;
                        num7 = index;
                        goto Label_0C11;

                    default:
                        if (IsWordChar(c))
                        {
                            endState = (5 | num2) | num3;
                            num7 = index;
                        }
                        else
                        {
                            endState = 0x10;
                        }
                        goto Label_0C11;
                }
            Label_02A9:
                index = length - 1;
                endIndex = index;
                goto Label_0C11;
            Label_02DF:
                endState = 0x10;
                goto Label_0C11;
            Label_0300:
                endState = 0x10;
                goto Label_0C11;
            Label_0346:
                if (str.ToLower().Equals("style") && !flag2)
                {
                    endState |= 0x200;
                }
            Label_0364:
                token = new Token(1, endState, num7, endIndex, chars, length);
                goto Label_0C11;
            Label_0378:
                if (c == '>')
                {
                    endState = 0x11;
                    endIndex = index;
                    string str2 = new string(chars, num7, endIndex - num7);
                    if (str2.ToLower().Equals("script"))
                    {
                        if (!flag)
                        {
                            endState |= 0x100;
                        }
                    }
                    else if (str2.ToLower().Equals("style") && !flag2)
                    {
                        endState |= 0x200;
                    }
                    token = new Token(1, endState, num7, endIndex, chars, length);
                }
                else if (!IsWordChar(c))
                {
                    if (c == '/')
                    {
                        endState = 15;
                        endIndex = index;
                        string str3 = new string(chars, num7, endIndex - num7);
                        if (str3.ToLower().Equals("script"))
                        {
                            if (!flag)
                            {
                                endState |= 0x100;
                            }
                        }
                        else if (str3.ToLower().Equals("style") && !flag2)
                        {
                            endState |= 0x200;
                        }
                        token = new Token(1, endState, num7, endIndex, chars, length);
                    }
                    else
                    {
                        endState = 0x10;
                    }
                }
                goto Label_0C11;
            Label_0476:
                if (IsWordChar(c))
                {
                    endState = 60;
                }
                else
                {
                    endState = 0x10;
                }
                goto Label_0C11;
            Label_049D:
                endState = 0x10;
                goto Label_0C11;
            Label_04C4:
                endState = 0x66;
                goto Label_0C11;
            Label_04F9:
                endState = 0x66;
                goto Label_0C11;
            Label_0551:
                switch (c)
                {
                    case '>':
                        endState = ((0x11 | num2) | num3) | num5;
                        endIndex = index;
                        token = new Token(0, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case '/':
                        endState = (15 | num2) | num3;
                        endIndex = index;
                        token = new Token(0, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    default:
                        if (!IsWhitespace(c))
                        {
                            endState = 0x10;
                        }
                        goto Label_0C11;
                }
            Label_06EF:
                switch (c)
                {
                    case '>':
                        endState = ((0x11 | num2) | num3) | num5;
                        endIndex = index;
                        token = new Token(0, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    case '/':
                        endState = 15;
                        endIndex = index;
                        token = new Token(0, endState, num7, endIndex, chars, length);
                        goto Label_0C11;

                    default:
                        if (IsWordChar(c))
                        {
                            endState = ((7 | num2) | num3) | num5;
                            endIndex = index;
                            token = new Token(0, endState, num7, endIndex, chars, length);
                        }
                        else if (!IsWhitespace(c))
                        {
                            endState = 0x10;
                        }
                        goto Label_0C11;
                }
            Label_07A7:
                endState = 0x10;
                goto Label_0C11;
            Label_07DB:
                if (c == '"')
                {
                    endState = (((0x13 | num2) | num3) | num4) | num5;
                    endIndex = index;
                    token = new Token(0, endState, num7, endIndex, chars, length);
                }
                else if (IsWordChar(c))
                {
                    endState = (((14 | num2) | num3) | num4) | num5;
                    endIndex = index;
                    token = new Token(0, endState, num7, endIndex, chars, length);
                }
                else if (!IsWhitespace(c))
                {
                    endState = 0x10;
                }
                goto Label_0C11;
            Label_0879:
                endState = 0x10;
                goto Label_0C11;
            Label_0902:
                endState = 0x10;
                goto Label_0C11;
            Label_0939:
                endState = 0x10;
                goto Label_0C11;
            Label_09C2:
                endState = 0x10;
                goto Label_0C11;
            Label_0B12:
                endState = 0x10;
                goto Label_0C11;
            Label_0B30:
                if (flag2)
                {
                    endState = (50 | num2) | num3;
                }
                else
                {
                    endState = 0;
                }
            Label_0B3F:
                endIndex = index + 1;
                token = new Token(11, endState, num7, endIndex, chars, length);
                goto Label_0C11;
            Label_0B5A:
                endState = 0x10;
                goto Label_0C11;
            Label_0B9B:
                token = new Token(20, endState, num7, endIndex, chars, length);
                goto Label_0C11;
            Label_0BAD:
                index = length - 1;
                endIndex = index;
            Label_0C11:
                index++;
            }
            if ((index < length) || (token != null))
            {
                return token;
            }
            switch ((endState & 0xff))
            {
                case 6:
                    num12 = 0;
                    break;

                case 7:
                    num12 = 2;
                    endState = 6;
                    break;

                case 14:
                    num12 = 3;
                    endState = 6;
                    break;

                case 0:
                    num12 = 4;
                    break;

                case 100:
                case 0x65:
                case 0x66:
                case 0x67:
                case 0x68:
                    num12 = 7;
                    break;

                case 50:
                    num12 = 0x15;
                    break;

                case 30:
                    num12 = 0x16;
                    break;

                case 40:
                    if (flag4)
                    {
                        num12 = 0x17;
                    }
                    else
                    {
                        num12 = 20;
                    }
                    break;

                default:
                    num12 = 8;
                    endState = 0x10;
                    break;
            }
            return new Token(num12, endState, num7, index, chars, length);
        }

        private static int IndexOf(char[] chars, int startIndex, int endColumnNumber, string s)
        {
            int length = s.Length;
            int num2 = (endColumnNumber - length) + 1;
            for (int i = startIndex; i < num2; i++)
            {
                bool flag = true;
                for (int j = 0; j < length; j++)
                {
                    if (char.ToUpper(chars[i + j]) != char.ToUpper(s[j]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return i;
                }
            }
            return -1;
        }

        private static bool IsWhitespace(char c)
        {
            return char.IsWhiteSpace(c);
        }

        private static bool IsWordChar(char c)
        {
            if (((!char.IsLetterOrDigit(c) && (c != '_')) && ((c != ':') && (c != '#'))) && (c != '-'))
            {
                return (c == '.');
            }
            return true;
        }

        private class HtmlTokenizerStates
        {
            public const int BeginCommentTag1 = 100;
            public const int BeginCommentTag2 = 0x65;
            public const int BeginDoubleQuote = 0x13;
            public const int BeginSingleQuote = 20;
            public const int EndCommentTag1 = 0x67;
            public const int EndCommentTag2 = 0x68;
            public const int EndDoubleQuote = 11;
            public const int EndSingleQuote = 13;
            public const int EndTag = 0x11;
            public const int EqualsChar = 0x12;
            public const int Error = 0x10;
            public const int ExpAttr = 6;
            public const int ExpAttrVal = 9;
            public const int ExpEquals = 8;
            public const int ExpTag = 2;
            public const int ExpTagAfterSlash = 4;
            public const int ForwardSlash = 3;
            public const int InAttr = 7;
            public const int InAttrVal = 14;
            public const int InCommentTag = 0x66;
            public const int InDoubleQuoteAttrVal = 10;
            public const int InSingleQuoteAttrVal = 12;
            public const int InTagName = 5;
            public const int RunAtServerState = 0x800;
            public const int RunAtState = 0x400;
            public const int Script = 40;
            public const int ScriptState = 0x100;
            public const int SelfTerminating = 15;
            public const int ServerSideScript = 30;
            public const int StartTag = 1;
            public const int Style = 50;
            public const int StyleState = 0x200;
            public const int Text = 0;
            public const int XmlDirective = 60;
        }
    }
}


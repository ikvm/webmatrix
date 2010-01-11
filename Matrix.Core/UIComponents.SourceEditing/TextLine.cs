namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public sealed class TextLine
    {
        private byte[] _attributes;
        private bool _attributesDirty;
        private string _cachedString;
        private int _colorState;
        private char[] _data;
        private int _length;
        private TextLine _next;
        private TextLine _previous;
        private int _userData;
        private static readonly int charSize = Marshal.SizeOf(typeof(char));
        public static readonly TextLine Empty = new TextLine();
        private const int PaddingSize = 0x10;

        public TextLine()
        {
            this._attributesDirty = true;
        }

        public TextLine(int initialCapacity)
        {
            if (initialCapacity < 0x10)
            {
                initialCapacity = 0x10;
            }
            this.EnsureCapacityInternal(initialCapacity);
            this._attributesDirty = true;
        }

        public TextLine(string s) : this(s.ToCharArray(), 0, s.Length)
        {
        }

        public TextLine(char[] chars) : this(chars, 0, chars.Length)
        {
        }

        public TextLine(char[] chars, int startIndex, int length)
        {
            if (length != 0)
            {
                this.EnsureCapacityInternal(length);
                UnsafeCopyChars(this._data, 0, chars, startIndex, length);
                this._length = length;
            }
            this._attributesDirty = true;
        }

        public int Append(char ch)
        {
            this.EnsureCapacityInternal(this._length + 1);
            this._data[this._length++] = ch;
            this.MakeDirty();
            return this._length;
        }

        public int Append(char[] chars)
        {
            return this.Append(chars, 0, chars.Length);
        }

        public int Append(string s)
        {
            return this.Append(s.ToCharArray(), 0, s.Length);
        }

        public int Append(char[] chars, int length)
        {
            return this.Append(chars, 0, length);
        }

        public int Append(string s, int length)
        {
            return this.Append(s.ToCharArray(0, length), 0, length);
        }

        public int Append(string s, int startIndex, int length)
        {
            return this.Append(s.ToCharArray(startIndex, length), 0, length);
        }

        public int Append(char[] chars, int startIndex, int length)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            if ((startIndex < 0) || (chars.Length < (startIndex + length)))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.EnsureCapacityInternal(this._length + length);
            UnsafeCopyChars(this._data, this._length, chars, startIndex, length);
            this._length += length;
            this.MakeDirty();
            return this._length;
        }

        public void Clear()
        {
            this._length = 0;
            this.MakeDirty();
        }

        public void Copy(char ch)
        {
            this.EnsureCapacityInternal(1);
            this._data[0] = ch;
            this._length = 1;
            this.MakeDirty();
        }

        public void Copy(string s)
        {
            this.Copy(s.ToCharArray(), 0, s.Length);
        }

        public void Copy(char[] chars)
        {
            this.Copy(chars, 0, chars.Length);
        }

        public void Copy(string s, int length)
        {
            this.Copy(s.ToCharArray(0, length), 0, length);
        }

        public void Copy(char[] chars, int length)
        {
            this.Copy(chars, 0, length);
        }

        public void Copy(string s, int startIndex, int length)
        {
            this.Copy(s.ToCharArray(startIndex, length), 0, length);
        }

        public void Copy(char[] chars, int startIndex, int length)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            if ((startIndex < 0) || (chars.Length < (startIndex + length)))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.EnsureCapacityInternal(length);
            UnsafeCopyChars(this._data, 0, chars, startIndex, length);
            this.MakeDirty();
            this._length = length;
        }

        public bool Delete(int startIndex)
        {
            if ((startIndex < 0) || (this._data == null))
            {
                return false;
            }
            this._length = startIndex;
            this.MakeDirty();
            return true;
        }

        public bool Delete(int startIndex, int length)
        {
            if (((startIndex < 0) || (this._data == null)) || (startIndex > this.Length))
            {
                return false;
            }
            this.ShiftCharsLeft(this._data, startIndex, this.Length, length);
            this._length -= length;
            this.MakeDirty();
            return true;
        }

        public void EnsureCapacity(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.EnsureCapacityInternal(capacity);
        }

        private void EnsureCapacityInternal(int capacity)
        {
            if (this._length < capacity)
            {
                if (capacity > 0x10)
                {
                    int num = 0x10 - (capacity % 0x10);
                    capacity += num;
                }
                else
                {
                    capacity = 0x10;
                }
                char[] destChars = new char[capacity];
                byte[] destBytes = new byte[capacity + 1];
                if (this._length != 0)
                {
                    UnsafeCopyChars(destChars, 0, this._data, 0, this._length);
                    UnsafeCopyBytes(destBytes, 0, this._attributes, 0, this._length + 1);
                }
                this._data = destChars;
                this._attributes = destBytes;
            }
        }

        public int IndexOf(char ch)
        {
            if (this._length != 0)
            {
                return UnsafeIndexOf(this._data, 0, this._length, ch);
            }
            return -1;
        }

        public int IndexOf(string s, bool matchCase)
        {
            return this.IndexOf(s, 0, matchCase, false);
        }

        public int IndexOf(string s, bool matchCase, bool wholeWord)
        {
            return this.IndexOf(s, 0, matchCase, wholeWord);
        }

        public int IndexOf(string s, int startIndex, bool matchCase, bool wholeWord)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            int length = s.Length;
            if (length == 0)
            {
                return 0;
            }
            if (this._length >= length)
            {
                char[] chArray = s.ToCharArray();
                int num2 = this._length - length;
                if (wholeWord)
                {
                    if (matchCase)
                    {
                        for (int i = startIndex; i <= num2; i++)
                        {
                            if ((UnsafeCompareChars(this._data, i, this._length - i, chArray, 0, length) == 0) && (((i == 0) || (this.IsWordChar(s[0]) && !this.IsWordChar(this._data[i - 1]))) || (!this.IsWordChar(s[0]) && this.IsWordChar(this._data[i - 1]))))
                            {
                                if (((i + length) == this._length) || (this.IsWordChar(s[length - 1]) && !this.IsWordChar(this._data[i + length])))
                                {
                                    return i;
                                }
                                if (!this.IsWordChar(s[length - 1]) && this.IsWordChar(this._data[i + length]))
                                {
                                    return i;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = startIndex; j <= num2; j++)
                        {
                            if ((UnsafeCompareCharsIgnoreCase(this._data, j, this._length - j, chArray, 0, length) == 0) && (((j == 0) || (this.IsWordChar(s[0]) && !this.IsWordChar(this._data[j - 1]))) || (!this.IsWordChar(s[0]) && this.IsWordChar(this._data[j - 1]))))
                            {
                                if (((j + length) == this._length) || (this.IsWordChar(s[length - 1]) && !this.IsWordChar(this._data[j + length])))
                                {
                                    return j;
                                }
                                if (!this.IsWordChar(s[length - 1]) && this.IsWordChar(this._data[j + length]))
                                {
                                    return j;
                                }
                            }
                        }
                    }
                }
                else if (matchCase)
                {
                    for (int k = startIndex; k <= num2; k++)
                    {
                        if (UnsafeCompareChars(this._data, k, this._length - k, chArray, 0, length) == 0)
                        {
                            return k;
                        }
                    }
                }
                else
                {
                    for (int m = startIndex; m <= num2; m++)
                    {
                        if (UnsafeCompareCharsIgnoreCase(this._data, m, this._length - m, chArray, 0, length) == 0)
                        {
                            return m;
                        }
                    }
                }
            }
            return -1;
        }

        public int IndexOfAny(char[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException();
            }
            if (this._length != 0)
            {
                int length = chars.Length;
                for (int i = 0; i < length; i++)
                {
                    int num2 = UnsafeIndexOf(this._data, 0, this._length, chars[i]);
                    if (num2 != -1)
                    {
                        return num2;
                    }
                }
            }
            return -1;
        }

        public void Insert(int index, char c)
        {
            if ((index < 0) || (index > this._length))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.EnsureCapacity(this._length + 1);
            this.ShiftCharsRight(this._data, index, this._length, 1);
            this._data[index] = c;
            this._length++;
            this.MakeDirty();
        }

        public void Insert(int index, string s)
        {
            this.Insert(index, s.ToCharArray());
        }

        public void Insert(int index, char[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            this.Insert(index, chars, 0, chars.Length);
        }

        public void Insert(int index, char[] chars, int start, int length)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            if ((index < 0) || (index > this._length))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.EnsureCapacity(this._length + length);
            this.ShiftCharsRight(this._data, index, this._length, length);
            Array.Copy(chars, start, this._data, index, length);
            this._length += length;
            this.MakeDirty();
        }

        private bool IsWordChar(char c)
        {
            if (c != '_')
            {
                return char.IsLetterOrDigit(c);
            }
            return true;
        }

        public int LastIndexOf(char ch)
        {
            if (this._length != 0)
            {
                return UnsafeLastIndexOf(this._data, 0, this._length, ch);
            }
            return -1;
        }

        public int LastIndexOf(string s, bool matchCase)
        {
            return this.IndexOf(s, 0, matchCase, false);
        }

        public int LastIndexOf(string s, bool matchCase, bool wholeWord)
        {
            return this.IndexOf(s, 0, matchCase, wholeWord);
        }

        public int LastIndexOf(string s, int startIndex, bool matchCase, bool wholeWord)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            int length = s.Length;
            if (length == 0)
            {
                return 0;
            }
            if (this._length >= length)
            {
                char[] chArray = s.ToCharArray();
                int num2 = Math.Min(this._length - length, startIndex);
                if (wholeWord)
                {
                    if (matchCase)
                    {
                        for (int i = num2; i >= 0; i--)
                        {
                            if ((UnsafeCompareChars(this._data, i, this._length - i, chArray, 0, length) == 0) && (((i == 0) || (this.IsWordChar(s[0]) && !this.IsWordChar(this._data[i - 1]))) || (!this.IsWordChar(s[0]) && this.IsWordChar(this._data[i - 1]))))
                            {
                                if (((i + length) == this._length) || (this.IsWordChar(s[length - 1]) && !this.IsWordChar(this._data[i + length])))
                                {
                                    return i;
                                }
                                if (!this.IsWordChar(s[length - 1]) && this.IsWordChar(this._data[i + length]))
                                {
                                    return i;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = num2; j >= 0; j--)
                        {
                            if ((UnsafeCompareCharsIgnoreCase(this._data, j, this._length - j, chArray, 0, length) == 0) && (((j == 0) || (this.IsWordChar(s[0]) && !this.IsWordChar(this._data[j - 1]))) || (!this.IsWordChar(s[0]) && this.IsWordChar(this._data[j - 1]))))
                            {
                                if (((j + length) == this._length) || (this.IsWordChar(s[length - 1]) && !this.IsWordChar(this._data[j + length])))
                                {
                                    return j;
                                }
                                if (!this.IsWordChar(s[length - 1]) && this.IsWordChar(this._data[j + length]))
                                {
                                    return j;
                                }
                            }
                        }
                    }
                }
                else if (matchCase)
                {
                    for (int k = num2; k >= 0; k--)
                    {
                        if (UnsafeCompareChars(this._data, k, this._length - k, chArray, 0, length) == 0)
                        {
                            return k;
                        }
                    }
                }
                else
                {
                    for (int m = num2; m >= 0; m--)
                    {
                        if (UnsafeCompareCharsIgnoreCase(this._data, m, this._length - m, chArray, 0, length) == 0)
                        {
                            return m;
                        }
                    }
                }
            }
            return -1;
        }

        public int LastIndexOfAny(char[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException();
            }
            if (this._length != 0)
            {
                int length = chars.Length;
                for (int i = 0; i < length; i++)
                {
                    int num2 = UnsafeLastIndexOf(this._data, 0, this._length, chars[i]);
                    if (num2 != -1)
                    {
                        return num2;
                    }
                }
            }
            return -1;
        }

        private void MakeDirty()
        {
            this._cachedString = null;
            this._attributesDirty = true;
        }

        internal void SetAttributesDirty(bool dirty)
        {
            this._attributesDirty = dirty;
        }

        internal void SetColorState(int newState)
        {
            this._colorState = newState;
        }

        private void ShiftCharsLeft(char[] chars, int startIndex, int endIndex, int shiftAmount)
        {
            Array.Copy(chars, startIndex + shiftAmount, chars, startIndex, endIndex - (shiftAmount + startIndex));
        }

        private void ShiftCharsRight(char[] chars, int startIndex, int endIndex, int shiftAmount)
        {
            Array.Copy(chars, startIndex, chars, startIndex + shiftAmount, endIndex - startIndex);
        }

        public string Substring(int startIndex, int length)
        {
            return new string(this.ToCharArray(startIndex, length));
        }

        public char[] ToCharArray()
        {
            return this.ToCharArrayInternal(0, this._length);
        }

        public char[] ToCharArray(int startIndex, int length)
        {
            if ((startIndex < 0) || (this._length < (startIndex + length)))
            {
                throw new ArgumentOutOfRangeException();
            }
            return this.ToCharArrayInternal(startIndex, length);
        }

        private char[] ToCharArrayInternal(int startIndex, int length)
        {
            if (length == 0)
            {
                return new char[0];
            }
            char[] destChars = new char[length];
            UnsafeCopyChars(destChars, 0, this._data, startIndex, length);
            return destChars;
        }

        public override string ToString()
        {
            if (this._cachedString == null)
            {
                if (this._length != 0)
                {
                    this._cachedString = new string(this._data, 0, this._length);
                }
                else
                {
                    this._cachedString = string.Empty;
                }
            }
            return this._cachedString;
        }

        public void Truncate(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (this._length > length)
            {
                this._length = length;
                this.MakeDirty();
            }
        }

        private static unsafe int UnsafeCompareChars(char[] chars1, int startIndex1, int length1, char[] chars2, int startIndex2, int length2)
        {
            fixed (char* chRef = chars1)
            {
                fixed (char* chRef2 = chars2)
                {
                    char* chPtr = chRef + startIndex1;
                    char* chPtr2 = chRef2 + startIndex2;
                    int num = 0;
                    int num2 = 0;
                    while ((num < length1) && (num2 < length2))
                    {
                        if (chPtr[0] != chPtr2[0])
                        {
                            return (chPtr[0] - chPtr2[0]);
                        }
                        num++;
                        num2++;
                        chPtr++;
                        chPtr2++;
                    }
                    if (num == num2)
                    {
                        return 0;
                    }
                    if (num < length1)
                    {
                        return 1;
                    }
                    return -1;
                }
            }
        }

        private static unsafe int UnsafeCompareCharsIgnoreCase(char[] chars1, int startIndex1, int length1, char[] chars2, int startIndex2, int length2)
        {
            fixed (char* chRef = chars1)
            {
                fixed (char* chRef2 = chars2)
                {
                    char* chPtr = chRef + startIndex1;
                    char* chPtr2 = chRef2 + startIndex2;
                    int num = 0;
                    int num2 = 0;
                    while ((num < length1) && (num2 < length2))
                    {
                        if (char.ToUpper(chPtr[0]) != char.ToUpper(chPtr2[0]))
                        {
                            return (chPtr[0] - chPtr2[0]);
                        }
                        num++;
                        num2++;
                        chPtr++;
                        chPtr2++;
                    }
                    if (num == num2)
                    {
                        return 0;
                    }
                    if (num < length1)
                    {
                        return 1;
                    }
                    return -1;
                }
            }
        }

        private static unsafe void UnsafeCopyBytes(byte[] destBytes, int destStartIndex, byte[] srcBytes, int srcStartIndex, int length)
        {
            fixed (byte* numRef = destBytes)
            {
                fixed (byte* numRef2 = srcBytes)
                {
                    byte* numPtr = numRef + destStartIndex;
                    byte* numPtr2 = numRef2 + srcStartIndex;
                    for (int i = 0; i < length; i++)
                    {
                        *(numPtr++) = *(numPtr2++);
                    }
                }
            }
        }

        private static unsafe void UnsafeCopyChars(char[] destChars, int destStartIndex, char[] srcChars, int srcStartIndex, int length)
        {
            fixed (char* chRef = destChars)
            {
                fixed (char* chRef2 = srcChars)
                {
                    char* chPtr = chRef + destStartIndex;
                    char* chPtr2 = chRef2 + srcStartIndex;
                    for (int i = 0; i < length; i++)
                    {
                        chPtr[0] = chPtr2[0];
                        chPtr++;
                        chPtr2++;

                        //NOTE: reflector反编译出来的如下:
                        //但第一个字节没有拷贝, 导致每行前面都有一个 \0
                        //所以我把指针自增的代码后移, 就可以了
                        //chPtr++;
                        //chPtr2++;
                        //chPtr[0] = chPtr2[0];
                    }
                }
            }
        }

        private static unsafe int UnsafeIndexOf(char[] chars, int startIndex, int length, char ch)
        {
            fixed (char* chRef = chars)
            {
                char* chPtr = chRef + startIndex;
                int num = startIndex;
                while (num < length)
                {
                    if (chPtr[0] == ch)
                    {
                        return num;
                    }
                    num++;
                    chPtr++;
                }
            }
            return -1;
        }

        private static unsafe int UnsafeLastIndexOf(char[] chars, int startIndex, int length, char ch)
        {
            fixed (char* chRef = chars)
            {
                char* chPtr = chRef + (length - 1);
                int num = length - 1;
                while (num >= startIndex)
                {
                    if (chPtr[0] == ch)
                    {
                        return num;
                    }
                    num--;
                    chPtr--;
                }
            }
            return -1;
        }

        private static unsafe void UnsafeShiftCharsLeft(char[] chars, int startIndex, int endIndex, int shiftAmount)
        {
            fixed (char* chRef = chars)
            {
                char* chPtr = chRef + startIndex;
                char* chPtr2 = (chRef + endIndex) - (shiftAmount * 2);
                while (chPtr < chPtr2)
                {
                    chPtr[0] = chPtr[shiftAmount];
                    chPtr++;
                }
            }
        }

        private static unsafe void UnsafeShiftCharsRight(char[] chars, int startIndex, int endIndex, int shiftAmount)
        {
            fixed (char* chRef = chars)
            {
                char* chPtr = (chRef + startIndex) + (shiftAmount * 2);
                for (char* chPtr2 = ((chRef + endIndex) + (shiftAmount * 2)) - (1 * 2); chPtr2 >= chPtr; chPtr2--)
                {
                    chPtr2[0] = *(chPtr2 - shiftAmount);
                }
            }
        }

        internal byte[] Attributes
        {
            get
            {
                return this._attributes;
            }
        }

        internal bool AttributesDirty
        {
            get
            {
                return this._attributesDirty;
            }
        }

        public int ColorState
        {
            get
            {
                return this._colorState;
            }
        }

        internal char[] Data
        {
            get
            {
                return this._data;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this._length == 0);
            }
        }

        public char this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this._length))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return this._data[index];
            }
            set
            {
                if ((index < 0) || (index >= this._length))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._data[index] = value;
                this.MakeDirty();
            }
        }

        public int Length
        {
            get
            {
                return this._length;
            }
        }

        internal TextLine Next
        {
            get
            {
                return this._next;
            }
            set
            {
                this._next = value;
            }
        }

        internal TextLine Previous
        {
            get
            {
                return this._previous;
            }
            set
            {
                this._previous = value;
            }
        }

        public int UserData
        {
            get
            {
                return this._userData;
            }
            set
            {
                this._userData = value;
            }
        }
    }
}


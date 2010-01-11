namespace Microsoft.Matrix.Utility
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class MruList : ICollection, IEnumerable
    {
        private bool _caseInsensitive;
        private int _maxEntries;
        private string[] _mruEntries;
        private int _mruEntryCount;
        private int _version;
        public static readonly string[] MenuPrefixes = new string[] { "  &1 ", "  &2 ", "  &3 ", "  &4 ", "  &5 ", "  &6 ", "  &7 ", "  &8 ", "  &9 ", "1&0 " };

        public MruList(int maxEntries) : this(maxEntries, null, false)
        {
        }

        public MruList(int maxEntries, string[] initialEntries) : this(maxEntries, initialEntries, false)
        {
        }

        public MruList(int maxEntries, string[] initialEntries, bool caseInsensitive)
        {
            if (maxEntries < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            this._maxEntries = maxEntries;
            this._mruEntries = new string[this._maxEntries];
            this._mruEntryCount = 0;
            if (initialEntries != null)
            {
                this.Load(initialEntries);
            }
            this._caseInsensitive = caseInsensitive;
        }

        public string AddEntry(string entry)
        {
            string str = null;
            int index = 0;
            if (this._mruEntryCount > 0)
            {
                int num2;
                index = 0;
                while (index < this._mruEntryCount)
                {
                    if (string.Compare(this._mruEntries[index], entry, this._caseInsensitive) == 0)
                    {
                        break;
                    }
                    index++;
                }
                if (index == 0)
                {
                    return null;
                }
                if (index < this._mruEntryCount)
                {
                    num2 = index;
                }
                else
                {
                    num2 = this._mruEntryCount;
                    if (num2 == this.MaximumEntries)
                    {
                        num2--;
                    }
                }
                str = this._mruEntries[num2];
                for (int i = num2; i > 0; i--)
                {
                    this._mruEntries[i] = this._mruEntries[i - 1];
                }
            }
            this._mruEntries[0] = entry;
            if ((index == this._mruEntryCount) && (this._mruEntryCount < this._mruEntries.Length))
            {
                this._mruEntryCount++;
            }
            this._version++;
            return str;
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(this._mruEntries, 0, array, index, this._mruEntryCount);
        }

        public IEnumerator GetEnumerator()
        {
            return new MruEnumerator(this, this._version);
        }

        public void Load(string[] entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException();
            }
            int num = Math.Min(this.MaximumEntries, entries.Length);
            for (int i = 0; i < num; i++)
            {
                this._mruEntries[i] = entries[i];
            }
            this._mruEntryCount = num;
            this._version++;
        }

        public void RemoveEntry(string entry)
        {
            int index = 0;
            while (index < this._mruEntryCount)
            {
                if (string.Compare(this._mruEntries[index], entry, this._caseInsensitive) == 0)
                {
                    break;
                }
                index++;
            }
            if (index < this._mruEntryCount)
            {
                for (int i = index + 1; i < this._mruEntryCount; i++)
                {
                    this._mruEntries[i - 1] = this._mruEntries[i];
                }
            }
            this._mruEntryCount--;
            this._version++;
        }

        public string[] Save()
        {
            string[] strArray = new string[this._mruEntryCount];
            for (int i = 0; i < this._mruEntryCount; i++)
            {
                strArray[i] = this._mruEntries[i];
            }
            return strArray;
        }

        public int Count
        {
            get
            {
                return this._mruEntryCount;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public string this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this._mruEntryCount))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return this._mruEntries[index];
            }
        }

        public int MaximumEntries
        {
            get
            {
                return this._mruEntries.Length;
            }
            set
            {
                if ((value < 1) || (value > 10))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                if (this._maxEntries != value)
                {
                    string[] strArray = new string[value];
                    int num = Math.Min(value, this._maxEntries);
                    for (int i = 0; i < num; i++)
                    {
                        strArray[i] = this._mruEntries[i];
                    }
                    this._mruEntries = strArray;
                    this._maxEntries = value;
                    this._mruEntryCount = Math.Min(this._mruEntryCount, this._maxEntries);
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        private sealed class MruEnumerator : IEnumerator
        {
            private int _index;
            private MruList _owner;
            private int _version;

            public MruEnumerator(MruList owner, int version)
            {
                this._owner = owner;
                this._index = -1;
                this._version = version;
            }

            public bool MoveNext()
            {
                if (this._owner._version != this._version)
                {
                    throw new InvalidOperationException();
                }
                this._index++;
                return (this._index < this._owner._mruEntryCount);
            }

            public void Reset()
            {
                this._index = -1;
            }

            public object Current
            {
                get
                {
                    if (this._owner._version != this._version)
                    {
                        throw new InvalidOperationException();
                    }
                    return this._owner._mruEntries[this._index];
                }
            }
        }
    }
}


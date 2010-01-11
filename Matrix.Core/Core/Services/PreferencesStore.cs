namespace Microsoft.Matrix.Core.Services
{
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class PreferencesStore : ISerializable
    {
        private HybridDictionary _store;

        internal PreferencesStore()
        {
        }

        private PreferencesStore(SerializationInfo info, StreamingContext context)
        {
            int num = info.GetInt32("Count");
            if (num != 0)
            {
                IDictionary store = this.Store;
                for (int i = 0; i < num; i++)
                {
                    string key = info.GetString("n" + i);
                    object obj2 = info.GetValue("v" + i, typeof(object));
                    store.Add(key, obj2);
                }
            }
        }

        public void Clear()
        {
            if (!this.IsEmpty)
            {
                this._store.Clear();
            }
        }

        public void ClearValue(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            if (this._store != null)
            {
                this._store.Remove(name);
            }
        }

        public bool ContainsValue(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            return ((this._store != null) && this._store.Contains(name));
        }

        public object GetValue(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            object obj2 = null;
            if (this._store != null)
            {
                obj2 = this._store[name];
            }
            return obj2;
        }

        public bool GetValue(string name, bool defaultValue)
        {
            object obj2 = this.GetValue(name);
            if (obj2 != null)
            {
                return (bool) obj2;
            }
            return defaultValue;
        }

        public int GetValue(string name, int defaultValue)
        {
            object obj2 = this.GetValue(name);
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return defaultValue;
        }

        public string GetValue(string name, string defaultValue)
        {
            object obj2 = this.GetValue(name);
            if (obj2 != null)
            {
                return (string) obj2;
            }
            return defaultValue;
        }

        public string GetValue(string name, string defaultValue, PreferenceEncryption encryption)
        {
            string data = this.GetValue(name, (string) null);
            if (data == null)
            {
                return defaultValue;
            }
            if (data.Length == 0)
            {
                return data;
            }
            try
            {
                if (encryption == PreferenceEncryption.CurrentUser)
                {
                    return DataProtection.DecryptUserData(data);
                }
                return DataProtection.DecryptData(data);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public void SetValue(string name, object value)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            if (!(value is ISerializable))
            {
                throw new ArgumentException("Object must implement ISerializable", "value");
            }
            this.Store[name] = value;
        }

        public void SetValue(string name, bool value, bool defaultValue)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            if (value == defaultValue)
            {
                if (!this.IsEmpty)
                {
                    this._store.Remove(name);
                }
            }
            else
            {
                this.Store[name] = value;
            }
        }

        public void SetValue(string name, int value, int defaultValue)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            if (value == defaultValue)
            {
                if (!this.IsEmpty)
                {
                    this._store.Remove(name);
                }
            }
            else
            {
                this.Store[name] = value;
            }
        }

        public void SetValue(string name, string value, string defaultValue)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            if (value == defaultValue)
            {
                if (!this.IsEmpty)
                {
                    this._store.Remove(name);
                }
            }
            else
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.Store[name] = value;
            }
        }

        public void SetValue(string name, string value, string defaultValue, PreferenceEncryption encryption)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            if (value == defaultValue)
            {
                if (!this.IsEmpty)
                {
                    this._store.Remove(name);
                }
            }
            else
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (encryption == PreferenceEncryption.CurrentUser)
                {
                    value = DataProtection.EncryptUserData(value);
                }
                else
                {
                    value = DataProtection.EncryptData(value);
                }
                this.Store[name] = value;
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            int count = 0;
            if (this._store != null)
            {
                count = this._store.Count;
            }
            info.AddValue("Count", count);
            if (count != 0)
            {
                int num2 = 0;
                IDictionaryEnumerator enumerator = this._store.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    info.AddValue("n" + num2, current.Key);
                    info.AddValue("v" + num2, current.Value);
                    num2++;
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (this._store != null)
                {
                    return (this._store.Count == 0);
                }
                return true;
            }
        }

        private IDictionary Store
        {
            get
            {
                if (this._store == null)
                {
                    this._store = new HybridDictionary(false);
                }
                return this._store;
            }
        }
    }
}


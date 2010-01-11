namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ElementCollection : IList, ICollection, IEnumerable
    {
        private ArrayList _elements;
        private IDictionary _namedElements;
        private Element _owner;

        public ElementCollection(Element owner)
        {
            this._owner = owner;
        }

        public void Add(Element element)
        {
            this.InsertInternal(-1, element);
        }

        public void Clear()
        {
            if (this._elements != null)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    this.RemoveAt(0);
                }
            }
        }

        private void CollectNamedElements(Element parentElement)
        {
            ElementCollection elements = parentElement.Elements;
            if (elements.Count != 0)
            {
                foreach (Element element in elements)
                {
                    if (element.Name != null)
                    {
                        this._namedElements[element.Name] = element;
                        if (!(element is INameScopeElement))
                        {
                            this.CollectNamedElements(element);
                        }
                    }
                }
            }
        }

        public bool Contains(Element element)
        {
            if (this._elements == null)
            {
                return false;
            }
            return (this.IndexOf(element) != -1);
        }

        public void CopyTo(Array dest, int index)
        {
            if (this.Count > 0)
            {
                this._elements.CopyTo(dest, index);
            }
        }

        internal void DirtyNamedElements()
        {
            if (this._owner is INameScopeElement)
            {
                this._namedElements = null;
            }
            else
            {
                Element nameScope = this._owner.NameScope;
                if (nameScope != null)
                {
                    nameScope.Elements.DirtyNamedElements();
                }
            }
        }

        private void EnsureNamedElements()
        {
            if (this._namedElements == null)
            {
                this._namedElements = new Hashtable();
                if (this.Count != 0)
                {
                    foreach (Element element in this._elements)
                    {
                        if (element.Name != null)
                        {
                            this._namedElements[element.Name] = element;
                        }
                        if (!(element is INameScopeElement))
                        {
                            this.CollectNamedElements(element);
                        }
                    }
                }
            }
        }

        private void EnsureStorage()
        {
            if (this._elements == null)
            {
                if (this.IsReadOnly)
                {
                    throw new NotSupportedException();
                }
                this._elements = new ArrayList();
            }
        }

        public IEnumerator GetEnumerator()
        {
            if (this.Count != 0)
            {
                return this._elements.GetEnumerator();
            }
            return new Element[0].GetEnumerator();
        }

        public int IndexOf(Element element)
        {
            if ((element != null) && (this._elements != null))
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i] == element)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public void Insert(int index, Element element)
        {
            this.InsertInternal(index, element);
        }

        protected virtual void InsertInternal(int index, Element element)
        {
            this.EnsureStorage();
            if ((index < -1) || (index >= this._elements.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element.Parent != null)
            {
                element.Parent.Elements.Remove(element);
            }
            element.SetParent(this._owner);
            if (index == -1)
            {
                this._elements.Add(element);
            }
            else
            {
                this._elements.Insert(index, element);
            }
            if (element.Name != null)
            {
                this.DirtyNamedElements();
            }
            this._owner.OnElementsChanged();
        }

        public void Remove(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            int index = this.IndexOf(element);
            if (index == -1)
            {
                throw new ArgumentException();
            }
            this.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            Element element = (Element) this._elements[index];
            element.SetParent(null);
            this._elements.RemoveAt(index);
            if (element.Name != null)
            {
                this.DirtyNamedElements();
            }
            this._owner.OnElementsChanged();
        }

        int IList.Add(object value)
        {
            Element element = value as Element;
            if (element == null)
            {
                throw new ArgumentException();
            }
            this.InsertInternal(-1, element);
            return (this._elements.Count - 1);
        }

        void IList.Clear()
        {
            this.Clear();
        }

        bool IList.Contains(object value)
        {
            Element element = value as Element;
            return ((element != null) && this.Contains(element));
        }

        int IList.IndexOf(object value)
        {
            Element element = value as Element;
            if (element != null)
            {
                return this.IndexOf(element);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            Element element = value as Element;
            if (element == null)
            {
                throw new ArgumentException();
            }
            this.InsertInternal(index, element);
        }

        void IList.Remove(object value)
        {
            Element element = value as Element;
            if (element != null)
            {
                this.Remove(element);
            }
        }

        public int Count
        {
            get
            {
                if (this._elements == null)
                {
                    return 0;
                }
                return this._elements.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Element this[int index]
        {
            get
            {
                if (this._elements == null)
                {
                    throw new IndexOutOfRangeException();
                }
                return (Element) this._elements[index];
            }
        }

        public Element this[string name]
        {
            get
            {
                if (this._owner is INameScopeElement)
                {
                    this.EnsureNamedElements();
                    return (Element) this._namedElements[name];
                }
                Element nameScope = this._owner.NameScope;
                if (nameScope != null)
                {
                    return nameScope.Elements[name];
                }
                return null;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}


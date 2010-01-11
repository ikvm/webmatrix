namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Collections;
    using System.ComponentModel;

    public sealed class TypeDocument : Document
    {
        private ArrayList _constructors;
        private TypeDocumentItem _currentItem;
        private Page _descriptionPage;
        private ArrayList _events;
        private ArrayList _fields;
        private TypeDocumentFilter _filter;
        private ArrayList _methods;
        private ArrayList _properties;

        public TypeDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
            this._filter = TypeDocumentFilter.Declared | TypeDocumentFilter.Obsolete | TypeDocumentFilter.Current | TypeDocumentFilter.Public;
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new TypeDocumentStorage(this);
        }

        internal void SetCurrentItem(TypeDocumentItem item)
        {
            this._currentItem = item;
        }

        internal void SetFilter(TypeDocumentFilter filter)
        {
            if (this._filter != filter)
            {
                this._filter = filter;
                this.UpdateItems();
            }
        }

        private void UpdateItems()
        {
            TypeDocumentStorage storage = (TypeDocumentStorage) base.Storage;
            ICollection fields = storage.Fields;
            if (fields != null)
            {
                if (this._fields != null)
                {
                    this._fields.Clear();
                }
                else
                {
                    this._fields = new ArrayList();
                }
                foreach (TypeDocumentItem item in fields)
                {
                    if (item.MatchFilter(this._filter))
                    {
                        this._fields.Add(item);
                    }
                }
            }
            ICollection constructors = storage.Constructors;
            if (constructors != null)
            {
                if (this._constructors != null)
                {
                    this._constructors.Clear();
                }
                else
                {
                    this._constructors = new ArrayList();
                }
                foreach (TypeDocumentItem item2 in constructors)
                {
                    if (item2.MatchFilter(this._filter))
                    {
                        this._constructors.Add(item2);
                    }
                }
            }
            ICollection properties = storage.Properties;
            if (properties != null)
            {
                if (this._properties != null)
                {
                    this._properties.Clear();
                }
                else
                {
                    this._properties = new ArrayList();
                }
                foreach (TypeDocumentItem item3 in properties)
                {
                    if (item3.MatchFilter(this._filter))
                    {
                        this._properties.Add(item3);
                    }
                }
            }
            ICollection events = storage.Events;
            if (events != null)
            {
                if (this._events != null)
                {
                    this._events.Clear();
                }
                else
                {
                    this._events = new ArrayList();
                }
                foreach (TypeDocumentItem item4 in events)
                {
                    if (item4.MatchFilter(this._filter))
                    {
                        this._events.Add(item4);
                    }
                }
            }
            ICollection methods = storage.Methods;
            if (methods != null)
            {
                if (this._methods != null)
                {
                    this._methods.Clear();
                }
                else
                {
                    this._methods = new ArrayList();
                }
                foreach (TypeDocumentItem item5 in methods)
                {
                    if (item5.MatchFilter(this._filter))
                    {
                        this._methods.Add(item5);
                    }
                }
            }
        }

        public ICollection ConstructorItems
        {
            get
            {
                return this._constructors;
            }
        }

        public TypeDocumentItem CurrentItem
        {
            get
            {
                return this._currentItem;
            }
        }

        internal Page DescriptionPage
        {
            get
            {
                return this._descriptionPage;
            }
            set
            {
                this._descriptionPage = value;
            }
        }

        public ICollection EventItems
        {
            get
            {
                return this._events;
            }
        }

        public ICollection FieldItems
        {
            get
            {
                return this._fields;
            }
        }

        public TypeDocumentFilter Filter
        {
            get
            {
                return this._filter;
            }
        }

        public ICollection MethodItems
        {
            get
            {
                return this._methods;
            }
        }

        public ICollection PropertyItems
        {
            get
            {
                return this._properties;
            }
        }

        [Browsable(false)]
        public System.Type Type
        {
            get
            {
                return ((TypeDocumentStorage) base.Storage).Type;
            }
            set
            {
                ((TypeDocumentStorage) base.Storage).Type = value;
                this.UpdateItems();
            }
        }
    }
}


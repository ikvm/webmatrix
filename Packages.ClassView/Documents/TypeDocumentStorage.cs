namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    internal sealed class TypeDocumentStorage : IDocumentStorage, IDisposable
    {
        private ArrayList _constructors;
        private ArrayList _events;
        private ArrayList _fields;
        private ArrayList _methods;
        private TypeDocument _owner;
        private ArrayList _properties;
        private System.Type _type;

        public TypeDocumentStorage(TypeDocument owner)
        {
            this._owner = owner;
        }

        private void LoadItems()
        {
            int num;
            BindingFlags bindingAttr = BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            IComparer comparer = new MemberComparer();
            if (!this._type.IsInterface)
            {
                this._fields = new ArrayList();
                if (!this._type.IsEnum)
                {
                    this._constructors = new ArrayList();
                }
            }
            if (!this._type.IsEnum)
            {
                this._properties = new ArrayList();
                this._events = new ArrayList();
                this._methods = new ArrayList();
            }
            if (this._fields != null)
            {
                FieldInfo[] fields = this._type.GetFields(bindingAttr);
                if ((fields != null) && ((num = fields.Length) != 0))
                {
                    Array.Sort(fields, comparer);
                    for (int i = 0; i < num; i++)
                    {
                        FieldInfo fi = fields[i];
                        if (!fi.IsSpecialName)
                        {
                            this._fields.Add(new FieldItem(this._owner, fi));
                        }
                    }
                }
            }
            if (this._constructors != null)
            {
                ConstructorInfo[] constructors = this._type.GetConstructors(bindingAttr);
                if ((constructors != null) && ((num = constructors.Length) != 0))
                {
                    for (int j = 0; j < num; j++)
                    {
                        ConstructorInfo method = constructors[j];
                        this._constructors.Add(new MethodItem(this._owner, method));
                    }
                }
            }
            if (this._properties != null)
            {
                PropertyInfo[] properties = this._type.GetProperties(bindingAttr);
                if ((properties != null) && ((num = properties.Length) != 0))
                {
                    Array.Sort(properties, comparer);
                    for (int k = 0; k < num; k++)
                    {
                        try
                        {
                            PropertyInfo pi = properties[k];
                            MethodInfo getMethod = pi.GetGetMethod(true);
                            MethodInfo setMethod = pi.GetSetMethod(true);
                            MethodInfo underlyingMethod = (getMethod != null) ? getMethod : setMethod;
                            if (underlyingMethod != null)
                            {
                                this._properties.Add(new PropertyItem(this._owner, pi, underlyingMethod));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            if (this._events != null)
            {
                EventInfo[] events = this._type.GetEvents(bindingAttr);
                if ((events != null) && ((num = events.Length) != 0))
                {
                    Array.Sort(events, comparer);
                    for (int m = 0; m < num; m++)
                    {
                        try
                        {
                            EventInfo ei = events[m];
                            MethodInfo addMethod = ei.GetAddMethod(true);
                            MethodInfo removeMethod = ei.GetRemoveMethod(true);
                            MethodInfo info10 = (addMethod != null) ? addMethod : removeMethod;
                            if (info10 != null)
                            {
                                this._events.Add(new EventItem(this._owner, ei, info10));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            if (this._methods != null)
            {
                MethodInfo[] methods = this._type.GetMethods(bindingAttr);
                if ((methods != null) && ((num = methods.Length) != 0))
                {
                    Array.Sort(methods, comparer);
                    for (int n = 0; n < num; n++)
                    {
                        try
                        {
                            MethodInfo info11 = methods[n];
                            if (!info11.IsSpecialName)
                            {
                                this._methods.Add(new MethodItem(this._owner, info11));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        void IDocumentStorage.Load(Stream contentStream)
        {
            this._type = ((TypeProjectItem) this._owner.ProjectItem).Type;
            this.LoadItems();
        }

        void IDocumentStorage.Save(Stream contentStream)
        {
            throw new NotSupportedException();
        }

        void IDisposable.Dispose()
        {
            this._type = null;
            this._owner = null;
        }

        public ICollection Constructors
        {
            get
            {
                return this._constructors;
            }
        }

        public ICollection Events
        {
            get
            {
                return this._events;
            }
        }

        public ICollection Fields
        {
            get
            {
                return this._fields;
            }
        }

        public ICollection Methods
        {
            get
            {
                return this._methods;
            }
        }

        public ICollection Properties
        {
            get
            {
                return this._properties;
            }
        }

        public System.Type Type
        {
            get
            {
                return this._type;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._type = value;
            }
        }

        private sealed class MemberComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.Compare(((MemberInfo) x).Name, ((MemberInfo) y).Name);
            }
        }
    }
}


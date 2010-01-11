namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Reflection;

    public abstract class TypeDocumentItem
    {
        private Page _descriptionPage;
        private TypeDocument _document;
        private string _ilCode;
        private MemberInfo _member;
        private MethodInfo _memberMethod;
        private TypeDocumentFilter _statusFilter;
        private TypeDocumentFilter _typeFilter;
        private TypeDocumentFilter _visibilityFilter;

        protected TypeDocumentItem(TypeDocument document, MemberInfo member) : this(document, member, null)
        {
        }

        protected TypeDocumentItem(TypeDocument document, MemberInfo member, MethodInfo memberMethod)
        {
            this._document = document;
            this._member = member;
            this._memberMethod = memberMethod;
            this._visibilityFilter = this.GetVisibilityFilter();
            this._statusFilter = TypeDocumentFilter.Current;
            try
            {
                object[] customAttributes = this._member.GetCustomAttributes(typeof(ObsoleteAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length != 0))
                {
                    this._statusFilter = TypeDocumentFilter.Obsolete;
                }
            }
            catch
            {
            }
            if (member.DeclaringType == member.ReflectedType)
            {
                this._typeFilter = TypeDocumentFilter.Declared;
            }
            else
            {
                this._typeFilter = TypeDocumentFilter.Inherited;
            }
        }

        protected abstract TypeDocumentFilter GetVisibilityFilter();
        protected TypeDocumentFilter GetVisibilityFilter(MethodBase mb)
        {
            TypeDocumentFilter @public = TypeDocumentFilter.Public;
            if (mb.IsFamily || mb.IsFamilyAndAssembly)
            {
                return TypeDocumentFilter.Protected;
            }
            if (!mb.IsAssembly && !mb.IsPrivate)
            {
                return @public;
            }
            return TypeDocumentFilter.Private;
        }

        internal bool MatchFilter(TypeDocumentFilter filter)
        {
            TypeDocumentFilter filter2 = filter & (TypeDocumentFilter.Private | TypeDocumentFilter.Protected | TypeDocumentFilter.Public);
            if ((filter2 & this._visibilityFilter) == 0)
            {
                return false;
            }
            filter2 = filter & (TypeDocumentFilter.Obsolete | TypeDocumentFilter.Current);
            if ((filter2 & this._statusFilter) == 0)
            {
                return false;
            }
            filter2 = filter & (TypeDocumentFilter.Inherited | TypeDocumentFilter.Declared);
            if ((filter2 & this._typeFilter) == 0)
            {
                return false;
            }
            return true;
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

        public TypeDocument Document
        {
            get
            {
                return this._document;
            }
        }

        internal string ILCode
        {
            get
            {
                return this._ilCode;
            }
            set
            {
                this._ilCode = value;
            }
        }

        public bool IsObsolete
        {
            get
            {
                return ((this._statusFilter & TypeDocumentFilter.Obsolete) != 0);
            }
        }

        public MemberInfo Member
        {
            get
            {
                return this._member;
            }
        }

        protected MethodInfo MemberMethod
        {
            get
            {
                return this._memberMethod;
            }
        }

        public virtual string Text
        {
            get
            {
                return this._member.Name;
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.Design;
    using System.Reflection;
    using System.Web.UI;

    public sealed class RegisterDirectiveCollection : ICollection, IEnumerable
    {
        private string _registerDirectivesText;
        private HybridDictionary _registerDirectiveTable = new HybridDictionary();
        private const string AspAssembly = "System.Web";
        private const string AspNamespace = "System.Web.UI.WebControls";
        public const string AspTagPrefix = "asp";

        public event DirectiveEventHandler DirectiveAdded;

        internal void AddParsedRegisterDirective(RegisterDirective directive)
        {
            this.AddRegisterDirective(directive, false);
        }

        public void AddRegisterDirective(RegisterDirective directive)
        {
            this.AddRegisterDirective(directive, true);
        }

        private void AddRegisterDirective(RegisterDirective directive, bool raiseEvent)
        {
            string tagPrefix = directive.TagPrefix;
            if (directive.IsUserControl)
            {
                tagPrefix = tagPrefix + ":" + directive.TagName;
            }
            bool flag = this._registerDirectiveTable.Contains(tagPrefix.ToLower());
            this._registerDirectiveTable[tagPrefix.ToLower()] = directive;
            this._registerDirectivesText = null;
            if ((!flag && raiseEvent) && (this.DirectiveAdded != null))
            {
                this.DirectiveAdded(this, new DirectiveEventArgs(directive));
            }
        }

        public void Clear()
        {
            this._registerDirectiveTable.Clear();
            this._registerDirectivesText = null;
        }

        public void CopyTo(Array array, int index)
        {
            this._registerDirectiveTable.Values.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._registerDirectiveTable.Values.GetEnumerator();
        }

        internal Type GetObjectType(IServiceProvider serviceProvider, string tagPrefix, string typeName)
        {
            if (string.Compare(tagPrefix, "asp", true) == 0)
            {
                return Type.GetType("System.Web.UI.WebControls." + typeName + ", System.Web", false, true);
            }
            RegisterDirective registerDirective = this.GetRegisterDirective(tagPrefix, typeName);
            if (registerDirective == null)
            {
                return null;
            }
            if (!registerDirective.IsUserControl)
            {
                string name = registerDirective.NamespaceName + "." + typeName + ", " + registerDirective.AssemblyName;
                ITypeResolutionService service = (ITypeResolutionService) serviceProvider.GetService(typeof(ITypeResolutionService));
                return service.GetType(name, false, true);
            }
            return typeof(UserControl);
        }

        private RegisterDirective GetRegisterDirective(string tagPrefix, string tagName)
        {
            tagPrefix = tagPrefix.ToLower();
            if (tagName != null)
            {
                tagName = tagName.ToLower();
            }
            RegisterDirective directive = (RegisterDirective) this._registerDirectiveTable[tagPrefix];
            if (((directive == null) && (tagName != null)) && (tagName.Length != 0))
            {
                directive = (RegisterDirective) this._registerDirectiveTable[tagPrefix + ":" + tagName];
            }
            return directive;
        }

        internal string GetTagPrefix(Type objectType)
        {
            string strB = objectType.Namespace;
            string name = objectType.Assembly.GetName().Name;
            if (((strB != null) && strB.Equals("System.Web.UI.WebControls")) && name.Equals("System.Web"))
            {
                return "asp";
            }
            foreach (RegisterDirective directive in this._registerDirectiveTable.Values)
            {
                if ((!directive.IsUserControl && (string.Compare(directive.NamespaceName, strB, true) == 0)) && (string.Compare(directive.AssemblyName, name, true) == 0))
                {
                    return directive.TagPrefix;
                }
            }
            return null;
        }

        public override string ToString()
        {
            if (this._registerDirectivesText == null)
            {
                this._registerDirectivesText = string.Empty;
                foreach (RegisterDirective directive in this._registerDirectiveTable.Values)
                {
                    this._registerDirectivesText = this._registerDirectivesText + directive.ToString();
                }
            }
            return this._registerDirectivesText;
        }

        public int Count
        {
            get
            {
                return this._registerDirectiveTable.Count;
            }
        }

        public bool IsReadOnly
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

        public RegisterDirective this[string tagPrefix, string tagName]
        {
            get
            {
                return this.GetRegisterDirective(tagPrefix, tagName);
            }
        }

        public RegisterDirective this[string tagPrefix]
        {
            get
            {
                return this.GetRegisterDirective(tagPrefix, null);
            }
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }
    }
}


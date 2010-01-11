namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Packages.Web.Documents;
    using System;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.Design;

    public class WebFormsReferenceManager : IWebFormReferenceManager, IDisposable
    {
        private RegisterDirectiveCollection _registerDirectives;
        private IServiceProvider _serviceProvider;
        private int _tagPrefixCounter;

        public WebFormsReferenceManager(IServiceProvider serviceProvider, RegisterDirectiveCollection registerDirectives)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            if (registerDirectives == null)
            {
                throw new ArgumentNullException("registerDirectives");
            }
            this._serviceProvider = serviceProvider;
            this._registerDirectives = registerDirectives;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
            this._registerDirectives = null;
        }

        Type IWebFormReferenceManager.GetObjectType(string tagPrefix, string typeName)
        {
            return this._registerDirectives.GetObjectType(this._serviceProvider, tagPrefix, typeName);
        }

        string IWebFormReferenceManager.GetRegisterDirectives()
        {
            return this._registerDirectives.ToString();
        }

        string IWebFormReferenceManager.GetTagPrefix(Type objectType)
        {
            string tagPrefix = this._registerDirectives.GetTagPrefix(objectType);
            if (tagPrefix == null)
            {
                Assembly assembly = objectType.Assembly;
                object[] customAttributes = assembly.GetCustomAttributes(typeof(TagPrefixAttribute), true);
                string namespaceName = objectType.Namespace;
                bool flag = false;
                foreach (TagPrefixAttribute attribute in customAttributes)
                {
                    if (namespaceName.Equals(attribute.NamespaceName))
                    {
                        tagPrefix = attribute.TagPrefix;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    tagPrefix = "n" + this._tagPrefixCounter++;
                }
                string assemblyName = null;
                if (assembly.GlobalAssemblyCache)
                {
                    assemblyName = assembly.GetName().FullName;
                }
                else
                {
                    assemblyName = assembly.GetName().Name;
                }
                this._registerDirectives.AddRegisterDirective(new RegisterDirective(tagPrefix, namespaceName, assemblyName));
            }
            return tagPrefix;
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Html.WebForms
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Web.UI;
    using System.Web.UI.Design;

    internal sealed class NamespaceManager : IDisposable, Interop.IElementBehaviorFactory, Interop.IElementNamespaceFactory, Interop.IElementNamespaceFactoryCallback
    {
        private WebFormsEditor _editor;
        private Interop.IElementNamespaceTable _namespaceTable;
        private IWebFormReferenceManager _referenceManager;
        private IDictionary _registeredNamespaces;

        internal NamespaceManager(WebFormsEditor editor)
        {
            this._editor = editor;
            this._registeredNamespaces = new HybridDictionary();
        }

        Interop.IElementBehavior Interop.IElementBehaviorFactory.FindBehavior(string bstrBehavior, string bstrBehaviorUrl, Interop.IElementBehaviorSite pSite)
        {
            Behavior behavior = null;
            if (this._referenceManager != null)
            {
                string scopeName = ((Interop.IHTMLElement2) pSite.GetElement()).GetScopeName();
                string typeName = bstrBehavior;
                this._referenceManager.GetObjectType(scopeName, typeName);
            }
            if (behavior == null)
            {
                behavior = new Behavior(this._editor);
            }
            return behavior;
        }

        void Interop.IElementNamespaceFactory.Create(Interop.IElementNamespace pNamespace)
        {
        }

        void Interop.IElementNamespaceFactoryCallback.Resolve(string namespaceName, string tagName, string attributes, Interop.IElementNamespace pNamespace)
        {
            namespaceName = namespaceName.ToLower();
            if (this._registeredNamespaces.Contains(namespaceName))
            {
                if (this._referenceManager == null)
                {
                    IServiceProvider serviceProvider = this._editor.ServiceProvider;
                    this._referenceManager = (IWebFormReferenceManager) serviceProvider.GetService(typeof(IWebFormReferenceManager));
                }
                if (this._referenceManager != null)
                {
                    Type objectType = this._referenceManager.GetObjectType(namespaceName, tagName);
                    if (objectType != null)
                    {
                        int lFlags = 0;
                        PersistChildrenAttribute attribute = (PersistChildrenAttribute) TypeDescriptor.GetAttributes(objectType)[typeof(PersistChildrenAttribute)];
                        if (!attribute.Persist || (objectType == typeof(UserControl)))
                        {
                            lFlags |= 2;
                        }
                        pNamespace.AddTag(tagName, lFlags);
                    }
                }
            }
        }

        public void RegisterNamespace(string namespaceName)
        {
            namespaceName = namespaceName.ToLower();
            if (!this._registeredNamespaces.Contains(namespaceName) && this.RegisterNamespaceWithEditor(namespaceName))
            {
                this._registeredNamespaces[namespaceName] = string.Empty;
            }
        }

        private bool RegisterNamespaceWithEditor(string namespaceName)
        {
            if (this._namespaceTable == null)
            {
                IntPtr ptr;
                Interop.IOleServiceProvider mSHTMLDocument = this._editor.MSHTMLDocument as Interop.IOleServiceProvider;
                Guid gUID = typeof(Interop.IElementNamespaceTable).GUID;
                Guid riid = gUID;
                if ((mSHTMLDocument.QueryService(ref gUID, ref riid, out ptr) == 0) && (ptr != Interop.NullIntPtr))
                {
                    this._namespaceTable = (Interop.IElementNamespaceTable) Marshal.GetObjectForIUnknown(ptr);
                    Marshal.Release(ptr);
                }
            }
            if (this._namespaceTable == null)
            {
                return false;
            }
            object factory = this;
            try
            {
                this._namespaceTable.AddNamespace(namespaceName, null, 2, ref factory);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        void IDisposable.Dispose()
        {
            this._namespaceTable = null;
            this._registeredNamespaces = null;
            this._referenceManager = null;
            this._editor = null;
        }

        public ICollection NamespaceList
        {
            get
            {
                return this._registeredNamespaces.Keys;
            }
        }
    }
}


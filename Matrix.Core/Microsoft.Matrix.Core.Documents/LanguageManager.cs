namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Services;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Configuration;

    internal sealed class LanguageManager : ILanguageManager, IDisposable
    {
        private CodeDocumentLanguage _codeLanguage;
        private IDictionary _languages;
        private IServiceProvider _serviceProvider;

        public LanguageManager(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._languages = new HybridDictionary(true);
        }

        public void LoadDocumentLanguages()
        {
            CodeDocumentLanguage language = null;
            PreferencesStore store;
            ICollection config = (ICollection) ConfigurationSettings.GetConfig("microsoft.matrix/documentLanguages");
            if (config != null)
            {
                foreach (string str in config)
                {
                    try
                    {
                        DocumentLanguage language2 = (DocumentLanguage) Activator.CreateInstance(Type.GetType(str, true));
                        language2.Initialize(this._serviceProvider);
                        this._languages.Add(language2.Name, language2);
                        if (language == null)
                        {
                            language = language2 as CodeDocumentLanguage;
                        }
                        continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
            if ((service != null) && service.GetPreferencesStore(typeof(LanguageManager), out store))
            {
                string str2 = store.GetValue("CodeLanguage", string.Empty);
                if (str2.Length != 0)
                {
                    this._codeLanguage = this._languages[str2] as CodeDocumentLanguage;
                }
            }
            if (this._codeLanguage == null)
            {
                this._codeLanguage = language;
            }
        }

        DocumentLanguage ILanguageManager.GetDocumentLanguage(string name)
        {
            return (DocumentLanguage) this._languages[name];
        }

        public void SetCurrentCodeLanguage(CodeDocumentLanguage codeLanguage)
        {
            this._codeLanguage = codeLanguage;
        }

        void IDisposable.Dispose()
        {
            if (this._languages != null)
            {
                foreach (DocumentLanguage language in this._languages.Values)
                {
                    ((IDisposable) language).Dispose();
                }
                this._languages = null;
            }
            if (this._codeLanguage != null)
            {
                IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
                if (service != null)
                {
                    service.GetPreferencesStore(typeof(LanguageManager)).SetValue("CodeLanguage", this._codeLanguage.Name, string.Empty);
                }
                this._codeLanguage = null;
            }
            this._serviceProvider = null;
        }

        DocumentLanguage ILanguageManager.DefaultCodeLanguage
        {
            get
            {
                return this._codeLanguage;
            }
        }

        DocumentLanguage ILanguageManager.DefaultTextLanguage
        {
            get
            {
                return TextDocumentLanguage.Instance;
            }
        }

        ICollection ILanguageManager.DocumentLanguages
        {
            get
            {
                return this._languages.Values;
            }
        }
    }
}


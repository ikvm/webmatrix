namespace Microsoft.Matrix.Core.Documents.Text
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Windows.Forms;

    public class TextDocumentLanguage : DocumentLanguage, ITextLanguage
    {
        private ITextColorizer _colorizer;
        private TextOptions _options;
        private const string ConvertTabsToSpacesPreference = "ConvertTabsToSpaces";
        private const bool DefaultConvertTabsToSpaces = true;
        private const bool DefaultShowLineNumbers = false;
        private const bool DefaultShowWhitespace = false;
        private const int DefaultTabSize = 4;
        private const bool DefaultTrimTrailingWhitespace = true;
        internal static TextDocumentLanguage Instance;
        private const string ShowLineNumbersPreference = "ShowLineNumbers";
        private const string ShowWhitespacePreference = "ShowWhitespace";
        private const string TabSizePreference = "TabSize";
        private const string TrimTrailingWhitespacePreference = "TrimTrailingWhitespace";

        public TextDocumentLanguage() : this("Text")
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        protected TextDocumentLanguage(string name) : this(name, new TextOptions())
        {
        }

        protected TextDocumentLanguage(string name, TextOptions options) : base(name)
        {
            this._options = options;
        }

        protected override void Dispose()
        {
            if (this.SupportsCustomOptions)
            {
                IPreferencesService service = (IPreferencesService) base.ServiceProvider.GetService(typeof(IPreferencesService));
                if (service != null)
                {
                    PreferencesStore preferencesStore = service.GetPreferencesStore(base.GetType());
                    this._options.Save(preferencesStore);
                }
            }
            base.Dispose();
        }

        protected virtual ITextColorizer GetColorizer(IServiceProvider provider)
        {
            if (this._colorizer == null)
            {
                this._colorizer = new PlainTextColorizer();
            }
            return this._colorizer;
        }

        protected virtual IDataObject GetDataObjectFromText(string text)
        {
            DataObject obj2 = new DataObject();
            obj2.SetData(DataFormats.Text, text);
            return obj2;
        }

        protected virtual ITextControlHost GetTextControlHost(TextControl control, IServiceProvider serviceProvider)
        {
            return null;
        }

        protected virtual string GetTextFromDataObject(IDataObject dataObject, IServiceProvider serviceProvider)
        {
            return (dataObject.GetData(DataFormats.Text) as string);
        }

        public virtual TextOptions GetTextOptions(IServiceProvider serviceProvider)
        {
            if (!this.SupportsCustomOptions)
            {
                throw new InvalidOperationException();
            }
            return this._options;
        }

        protected virtual TextBufferSpan GetWordSpan(TextBufferLocation location, WordType type)
        {
            return TextLanguage.Instance.GetWordSpan(location, type);
        }

        public override void Initialize(IServiceProvider serviceProvider)
        {
            base.Initialize(serviceProvider);
            if (this.SupportsCustomOptions)
            {
                PreferencesStore store;
                IPreferencesService service = (IPreferencesService) serviceProvider.GetService(typeof(IPreferencesService));
                if ((service != null) && service.GetPreferencesStore(base.GetType(), out store))
                {
                    this._options.Load(store);
                }
            }
        }

        ITextColorizer ITextLanguage.GetColorizer(IServiceProvider serviceProvider)
        {
            return this.GetColorizer(serviceProvider);
        }

        IDataObject ITextLanguage.GetDataObjectFromText(string text)
        {
            return this.GetDataObjectFromText(text);
        }

        ITextControlHost ITextLanguage.GetTextControlHost(TextControl control, IServiceProvider serviceProvider)
        {
            return this.GetTextControlHost(control, serviceProvider);
        }

        string ITextLanguage.GetTextFromDataObject(IDataObject dataObject, IServiceProvider serviceProvider)
        {
            return this.GetTextFromDataObject(dataObject, serviceProvider);
        }

        TextBufferSpan ITextLanguage.GetWordSpan(TextBufferLocation location, WordType type)
        {
            return this.GetWordSpan(location, type);
        }

        void ITextLanguage.ShowHelp(IServiceProvider serviceProvider, TextBufferLocation location)
        {
            this.ShowHelp(serviceProvider, location);
        }

        bool ITextLanguage.SupportsDataObject(IServiceProvider provider, IDataObject dataObject)
        {
            return this.SupportsDataObject(provider, dataObject);
        }

        protected virtual void ShowHelp(IServiceProvider serviceProvider, TextBufferLocation location)
        {
        }

        protected virtual bool SupportsDataObject(IServiceProvider serviceProvider, IDataObject dataObject)
        {
            return dataObject.GetDataPresent(DataFormats.Text);
        }

        public bool SupportsCustomOptions
        {
            get
            {
                return (this._options != null);
            }
        }
    }
}


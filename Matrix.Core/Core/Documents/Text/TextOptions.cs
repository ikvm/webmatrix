namespace Microsoft.Matrix.Core.Documents.Text
{
    using Microsoft.Matrix.Core.Services;
    using System;

    public class TextOptions
    {
        private bool _convertTabsToSpaces;
        private bool _showLineNumbers;
        private bool _showWhitespace;
        private int _tabSize;
        private bool _trimTrailingWhitespace;
        private const string ConvertTabsToSpacesPreference = "ConvertTabsToSpaces";
        private const bool DefaultConvertTabsToSpaces = true;
        private const bool DefaultShowLineNumbers = false;
        private const bool DefaultShowWhitespace = false;
        private const int DefaultTabSize = 4;
        private const bool DefaultTrimTrailingWhitespace = true;
        private const string ShowLineNumbersPreference = "ShowLineNumbers";
        private const string ShowWhitespacePreference = "ShowWhitespace";
        private const string TabSizePreference = "TabSize";
        private const string TrimTrailingWhitespacePreference = "TrimTrailingWhitespace";

        public TextOptions()
        {
            this.ConvertTabsToSpaces = true;
            this.ShowLineNumbers = false;
            this.ShowWhitespace = false;
            this.TabSize = 4;
            this.TrimTrailingWhitespace = true;
        }

        public virtual void Load(PreferencesStore prefStore)
        {
            if (prefStore == null)
            {
                throw new ArgumentNullException("prefStore");
            }
            this.ConvertTabsToSpaces = prefStore.GetValue("ConvertTabsToSpaces", true);
            this.ShowWhitespace = prefStore.GetValue("ShowWhitespace", false);
            this.ShowLineNumbers = prefStore.GetValue("ShowLineNumbers", false);
            this.TabSize = prefStore.GetValue("TabSize", 4);
            this.TrimTrailingWhitespace = prefStore.GetValue("TrimTrailingWhitespace", true);
        }

        public virtual void Save(PreferencesStore prefStore)
        {
            if (prefStore == null)
            {
                throw new ArgumentNullException("prefStore");
            }
            prefStore.SetValue("ConvertTabsToSpaces", this.ConvertTabsToSpaces, true);
            prefStore.SetValue("ShowWhitespace", this.ShowWhitespace, false);
            prefStore.SetValue("ShowLineNumbers", this.ShowLineNumbers, false);
            prefStore.SetValue("TabSize", this.TabSize, 4);
            prefStore.SetValue("TrimTrailingWhitespace", this.TrimTrailingWhitespace, true);
        }

        public virtual bool ConvertTabsToSpaces
        {
            get
            {
                return this._convertTabsToSpaces;
            }
            set
            {
                this._convertTabsToSpaces = value;
            }
        }

        public virtual bool ShowLineNumbers
        {
            get
            {
                return this._showLineNumbers;
            }
            set
            {
                this._showLineNumbers = value;
            }
        }

        public virtual bool ShowWhitespace
        {
            get
            {
                return this._showWhitespace;
            }
            set
            {
                this._showWhitespace = value;
            }
        }

        public virtual int TabSize
        {
            get
            {
                return this._tabSize;
            }
            set
            {
                this._tabSize = value;
            }
        }

        public virtual bool TrimTrailingWhitespace
        {
            get
            {
                return this._trimTrailingWhitespace;
            }
            set
            {
                this._trimTrailingWhitespace = value;
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Services
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    public sealed class TemplateEditingFrame : ITemplateEditingFrame, IDisposable
    {
        private IDictionary _changeTable;
        private Style _controlStyle;
        private string _name;
        private string[] _templateNames;
        private Style[] _templateStyles;
        private TemplateEditingVerb _verb;

        internal TemplateEditingFrame(string frameName, string[] templateNames, Style controlStyle, Style[] templateStyles)
        {
            this._name = frameName;
            this._templateNames = templateNames;
            this._controlStyle = controlStyle;
            this._templateStyles = templateStyles;
            this._changeTable = new HybridDictionary(false);
        }

        public void Close(bool saveChanges)
        {
        }

        public void Dispose()
        {
            this._verb = null;
            this._changeTable = null;
        }

        public void Open()
        {
        }

        public void Resize(int width, int height)
        {
        }

        public void Save()
        {
        }

        public void UpdateControlName(string newName)
        {
        }

        public IDictionary ChangeTable
        {
            get
            {
                return this._changeTable;
            }
        }

        public Style ControlStyle
        {
            get
            {
                return this._controlStyle;
            }
            set
            {
                this._controlStyle = value;
            }
        }

        public int InitialHeight
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public int InitialWidth
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string[] TemplateNames
        {
            get
            {
                return this._templateNames;
            }
            set
            {
                this._templateNames = value;
            }
        }

        public Style[] TemplateStyles
        {
            get
            {
                return this._templateStyles;
            }
            set
            {
                this._templateStyles = value;
            }
        }

        public TemplateEditingVerb Verb
        {
            get
            {
                return this._verb;
            }
            set
            {
                this._verb = value;
            }
        }
    }
}


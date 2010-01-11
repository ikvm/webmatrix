namespace Microsoft.Matrix.Packages.Web.Html.WebForms
{
    using Microsoft.Matrix.Packages.Web.Html;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using System;
    using System.Web.UI;

    internal sealed class WebFormsEditor : HtmlEditor
    {
        private Control _defaultControlParent;
        private bool _isDirty;
        private WebFormsEditorMode _mode;
        private Microsoft.Matrix.Packages.Web.Html.WebForms.NamespaceManager _namespaceManager;

        public WebFormsEditor(IServiceProvider serviceProvider) : this(serviceProvider, WebFormsEditorMode.Page)
        {
        }

        public WebFormsEditor(IServiceProvider serviceProvider, WebFormsEditorMode mode) : base(serviceProvider, mode == WebFormsEditorMode.Page)
        {
            if ((mode < WebFormsEditorMode.Page) || (mode > WebFormsEditorMode.Template))
            {
                throw new ArgumentOutOfRangeException("mode");
            }
            this._mode = mode;
            this._namespaceManager = new Microsoft.Matrix.Packages.Web.Html.WebForms.NamespaceManager(this);
        }

        protected override string CreateHtmlContent(string content, string style)
        {
            if (this.EditorMode == WebFormsEditorMode.Template)
            {
                string format = "<html><head>{0}</head>\r\n<body contenteditable=\"false\" style=\"PADDING: 0px; margin: 0px\">\r\n<span style=\"WIDTH: 100%; HEIGHT: 100%\">\r\n<div contenteditable=\"true\" style=\"PADDING: 8px; WIDTH: 100%; HEIGHT: 100%\">\r\n<body contentEditable=\"true\">{1}</body>\r\n</div>\r\n</span>\r\n</body>\r\n</html>";
                return string.Format(format, style, content);
            }
            return base.CreateHtmlContent(content, style);
        }

        protected override EditorSelection CreateSelection()
        {
            return new WebFormsEditorSelection(this);
        }

        protected internal override Element GetContentElement(Element bodyElement)
        {
            if (this.EditorMode == WebFormsEditorMode.Template)
            {
                Element child = bodyElement.GetChild(0);
                if ((child != null) && (child is SpanElement))
                {
                    Element element2 = child.GetChild(0);
                    if ((element2 != null) && (element2 is DivElement))
                    {
                        return element2;
                    }
                }
            }
            return base.GetContentElement(bodyElement);
        }

        protected override void OnAfterLoad()
        {
            this._isDirty = false;
        }

        protected override void OnBeforeLoad()
        {
            base.OnBeforeLoad();
            this.RegisterNamespace("asp");
        }

        protected internal override void OnReadyStateComplete(EventArgs args)
        {
            base.OnReadyStateComplete(args);
            base.ClearDirtyState();
        }

        public void RegisterNamespace(string namespaceName)
        {
            this.NamespaceManager.RegisterNamespace(namespaceName);
        }

        internal void SetDirty()
        {
            this._isDirty = true;
        }

        public Control DefaultControlParent
        {
            get
            {
                return this._defaultControlParent;
            }
            set
            {
                this._defaultControlParent = value;
            }
        }

        public WebFormsEditorMode EditorMode
        {
            get
            {
                return this._mode;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return (this._isDirty | base.IsDirty);
            }
        }

        public Microsoft.Matrix.Packages.Web.Html.WebForms.NamespaceManager NamespaceManager
        {
            get
            {
                return this._namespaceManager;
            }
        }
    }
}


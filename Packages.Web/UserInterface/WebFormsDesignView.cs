namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Packages.Web.Html;
    using Microsoft.Matrix.Packages.Web.Html.WebForms;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Windows.Forms;

    public class WebFormsDesignView : HtmlDesignView, ISelectionContainer, ISearchableDocumentView
    {
        private WebFormsEditorMode _editorMode;
        private bool _fixedSelfClosedTags;
        private TemplateEditingDialog _templateEditingDialog;
        private static readonly Regex selfClosedTagsRegex = new Regex(@"=\s*(?<attrValue>\w+)\s*/\s*>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.Multiline);

        public WebFormsDesignView(IServiceProvider serviceProvider) : this(serviceProvider, WebFormsEditorMode.Page)
        {
        }

        public WebFormsDesignView(IServiceProvider serviceProvider, WebFormsEditorMode editorMode) : base(serviceProvider)
        {
            this._editorMode = editorMode;
        }

        protected override DataObjectConverter CreateDataObjectConverter()
        {
            return new WebFormsDataObjectConverter(base.ServiceProvider);
        }

        protected override HtmlEditor CreateEditor()
        {
            return new WebFormsEditor(base.ServiceProvider, this._editorMode);
        }

        private void EnterTemplateMode()
        {
            this.OnTemplateModeChanged(EventArgs.Empty);
            this._templateEditingDialog.BringToFront();
            this._templateEditingDialog.Show();
        }

        private void ExitTemplateMode()
        {
            this._templateEditingDialog = null;
            this.OnTemplateModeChanged(EventArgs.Empty);
        }

        protected override ICollection GetOutline(object selectedObject)
        {
            if (this.InTemplateMode)
            {
                return ((ISelectionOutlineProvider) this._templateEditingDialog.DesignView).GetOutline(selectedObject);
            }
            return base.GetOutline(selectedObject);
        }

        protected override bool HandleCommand(Command command)
        {
            bool flag = false;
            if (this.InTemplateMode)
            {
                return ((ICommandHandler) this._templateEditingDialog.DesignView).HandleCommand(command);
            }
            if ((command.CommandGroup == typeof(WebCommands)) && (command.CommandID == 240))
            {
                ISelectionService service = (ISelectionService) base.ServiceProvider.GetService(typeof(ISelectionService));
                if (service.SelectionCount == 1)
                {
                    System.Web.UI.Control primarySelection = service.PrimarySelection as System.Web.UI.Control;
                    if (primarySelection != null)
                    {
                        IDesignerHost host = base.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
                        TemplatedControlDesigner designer = host.GetDesigner(primarySelection) as TemplatedControlDesigner;
                        if (designer != null)
                        {
                            TemplateEditingVerb[] templateEditingVerbs = designer.GetTemplateEditingVerbs();
                            if ((templateEditingVerbs != null) && (templateEditingVerbs.Length > 0))
                            {
                                base.EnableSelectionTracking(false);
                                DocumentWindow window = (DocumentWindow) base.ServiceProvider.GetService(typeof(DocumentWindow));
                                foreach (System.Windows.Forms.Control control2 in window.Controls)
                                {
                                    control2.Enabled = false;
                                }
                                this._templateEditingDialog = new TemplateEditingDialog(designer, (WebFormsEditor) base.Editor, base.ServiceProvider);
                                this._templateEditingDialog.Closed += new EventHandler(this.OnTemplateEditingDialogClose);
                                this._templateEditingDialog.Location = new Point(0, 0);
                                this._templateEditingDialog.Size = new Size(500, 250);
                                this._templateEditingDialog.TopLevel = false;
                                this._templateEditingDialog.Parent = window;
                                IServiceContainer container = (ServiceContainer) base.ServiceProvider.GetService(typeof(IServiceContainer));
                                container.RemoveService(typeof(IDesignView));
                                container.AddService(typeof(IDesignView), this._templateEditingDialog.DesignView);
                                this.EnterTemplateMode();
                            }
                        }
                    }
                }
                flag = true;
            }
            if (!flag)
            {
                flag = base.HandleCommand(command);
            }
            return flag;
        }

        protected override void LoadFromDocument(Microsoft.Matrix.Packages.Web.Documents.HtmlDocument document)
        {
            WebFormsEditor editor = base.Editor as WebFormsEditor;
            IDocumentDesignerHost service = (IDocumentDesignerHost) base.ServiceProvider.GetService(typeof(IDocumentDesignerHost));
            foreach (RegisterDirective directive in ((WebFormsDocument) service.Document).RegisterDirectives)
            {
                string tagPrefix = directive.TagPrefix;
                if (tagPrefix.Length > 0)
                {
                    this.RegisterNamespace(tagPrefix);
                }
            }
            string html = ((WebFormsDocument) document).Html;
            if (html != null)
            {
                base.Editor.LoadHtml(html, document.ProjectItem.Url);
            }
        }

        protected override void OnBeforeLoadFromDocument(Microsoft.Matrix.Packages.Web.Documents.HtmlDocument document)
        {
            base.OnBeforeLoadFromDocument(document);
            string text = document.Text;
            this._fixedSelfClosedTags = false;
            text = selfClosedTagsRegex.Replace(text, new MatchEvaluator(this.SelfClosedTagEvaluator));
            if (this._fixedSelfClosedTags)
            {
                document.Text = text;
            }
        }

        protected void OnTemplateEditingDialogClose(object sender, EventArgs e)
        {
            this.ExitTemplateMode();
            IServiceContainer service = (ServiceContainer) base.ServiceProvider.GetService(typeof(IServiceContainer));
            service.RemoveService(typeof(IDesignView));
            service.AddService(typeof(IDesignView), this);
            DocumentWindow window = (DocumentWindow) base.ServiceProvider.GetService(typeof(DocumentWindow));
            foreach (System.Windows.Forms.Control control in window.Controls)
            {
                control.Enabled = true;
            }
            base.EnableSelectionTracking(true);
            base.UpdateSelection();
        }

        protected virtual void OnTemplateModeChanged(EventArgs e)
        {
        }

        public override void OnToolboxDataItemPicked(ToolboxDataItem dataItem)
        {
            if (this.InTemplateMode)
            {
                ((IToolboxClient) this._templateEditingDialog.DesignView).OnToolboxDataItemPicked(dataItem);
            }
            else
            {
                base.OnToolboxDataItemPicked(dataItem);
            }
        }

        protected override bool PerformFind(string searchString, FindReplaceOptions options)
        {
            if (this.InTemplateMode)
            {
                return ((ISearchableDocumentView) this._templateEditingDialog.DesignView).PerformFind(searchString, options);
            }
            return base.PerformFind(searchString, options);
        }

        protected override bool PerformReplace(string searchString, string replaceString, FindReplaceOptions options)
        {
            if (this.InTemplateMode)
            {
                return ((ISearchableDocumentView) this._templateEditingDialog.DesignView).PerformReplace(searchString, replaceString, options);
            }
            return base.PerformReplace(searchString, replaceString, options);
        }

        public void RegisterNamespace(string namespaceName)
        {
            (base.Editor as WebFormsEditor).RegisterNamespace(namespaceName);
        }

        protected override bool SaveToDocument()
        {
            if (this.InTemplateMode && this._templateEditingDialog.IsDirty)
            {
                this._templateEditingDialog.SaveTemplates();
            }
            DocumentWindow service = (DocumentWindow) base.ServiceProvider.GetService(typeof(DocumentWindow));
            WebFormsDocument document = (WebFormsDocument) service.Document;
            document.Html = base.Editor.SaveHtml();
            return true;
        }

        private string SelfClosedTagEvaluator(Match match)
        {
            this._fixedSelfClosedTags = true;
            StringBuilder builder = new StringBuilder("=\"");
            builder.Append(match.Groups["attrValue"].ToString());
            builder.Append("\"/>");
            return builder.ToString();
        }

        protected override void SetSelectedObject(object o)
        {
            if (this.InTemplateMode)
            {
                ((ISelectionContainer) this._templateEditingDialog.DesignView).SetSelectedObject(o);
            }
            else
            {
                base.SetSelectedObject(o);
            }
        }

        public override bool SupportsToolboxSection(ToolboxSection section)
        {
            if (this.InTemplateMode)
            {
                return ((IToolboxClient) this._templateEditingDialog.DesignView).SupportsToolboxSection(section);
            }
            if (!base.SupportsToolboxSection(section) && (section.GetType() != typeof(WebFormsToolboxSection)))
            {
                return (section.GetType() == typeof(CustomControlsToolboxSection));
            }
            return true;
        }

        protected override bool UpdateCommand(Command command)
        {
            bool flag = false;
            if (this.InTemplateMode)
            {
                return ((ICommandHandler) this._templateEditingDialog.DesignView).UpdateCommand(command);
            }
            if ((command.CommandGroup == typeof(WebCommands)) && (command.CommandID == 240))
            {
                base.UpdateDesignState();
                command.Enabled = false;
                ISelectionService service = (ISelectionService) base.ServiceProvider.GetService(typeof(ISelectionService));
                if (service.SelectionCount == 1)
                {
                    System.Web.UI.Control primarySelection = service.PrimarySelection as System.Web.UI.Control;
                    if (primarySelection != null)
                    {
                        IDesignerHost host = base.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
                        TemplatedControlDesigner designer = host.GetDesigner(primarySelection) as TemplatedControlDesigner;
                        if (designer != null)
                        {
                            TemplateEditingVerb[] templateEditingVerbs = designer.GetTemplateEditingVerbs();
                            command.Enabled = (templateEditingVerbs != null) && (templateEditingVerbs.Length > 0);
                        }
                    }
                }
                flag = true;
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }

        protected override bool CanDeactivate
        {
            get
            {
                if (this.InTemplateMode)
                {
                    if (this._templateEditingDialog.IsDirty)
                    {
                        IMxUIService service = (IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService));
                        switch (service.ShowMessage("The template contains changes that have not been saved.\n\nDo you want to save the changes?", "Template Editor", MessageBoxIcon.Question, MessageBoxButtons.YesNoCancel, MessageBoxDefaultButton.Button1))
                        {
                            case DialogResult.Cancel:
                                return false;

                            case DialogResult.Yes:
                                this._templateEditingDialog.SaveTemplates();
                                base.UpdateDesignState();
                                break;
                        }
                    }
                    this._templateEditingDialog.Close();
                }
                return base.CanDeactivate;
            }
        }

        public override ToolboxSection DefaultToolboxSection
        {
            get
            {
                if (this.InTemplateMode)
                {
                    return this._templateEditingDialog.DesignView.DefaultToolboxSection;
                }
                return WebFormsToolboxSection.WebForms;
            }
        }

        protected override FindReplaceOptions FindSupport
        {
            get
            {
                if (this.InTemplateMode)
                {
                    return this._templateEditingDialog.DesignView.FindSupport;
                }
                return base.FindSupport;
            }
        }

        protected internal bool InTemplateMode
        {
            get
            {
                return (this._templateEditingDialog != null);
            }
        }

        //protected override bool IsDirty //NOTE: changed
        public override bool IsDirty
        {
            get
            {
                return (base.IsDirty || (this.InTemplateMode && this._templateEditingDialog.IsDirty));
            }
        }

        protected override string OutlineTitle
        {
            get
            {
                if (this.InTemplateMode)
                {
                    return this._templateEditingDialog.DesignView.OutlineTitle;
                }
                return base.OutlineTitle;
            }
        }

        protected override FindReplaceOptions ReplaceSupport
        {
            get
            {
                if (this.InTemplateMode)
                {
                    return this._templateEditingDialog.DesignView.ReplaceSupport;
                }
                return base.ReplaceSupport;
            }
        }

        protected override DocumentViewType ViewType
        {
            get
            {
                if (this.InTemplateMode)
                {
                    return this._templateEditingDialog.DesignView.ViewType;
                }
                return base.ViewType;
            }
        }

        private class WebFormsDataObjectConverter : DataObjectConverter
        {
            private IServiceProvider _serviceProvider;

            public WebFormsDataObjectConverter(IServiceProvider serviceProvider)
            {
                this._serviceProvider = serviceProvider;
            }

            public override DataObjectConverterInfo CanConvertToHtml(IDataObject dataObject)
            {
                DataObjectConverterInfo info = base.CanConvertToHtml(dataObject);
                if (info != DataObjectConverterInfo.Unhandled)
                {
                    return info;
                }
                IDataObjectMappingService service = (IDataObjectMappingService) this._serviceProvider.GetService(typeof(IDataObjectMappingService));
                if (service == null)
                {
                    return info;
                }
                IDataObjectMapper dataObjectMapper = service.GetDataObjectMapper(dataObject, DataFormats.Html);
                if (dataObjectMapper == null)
                {
                    return info;
                }
                if (dataObjectMapper.CanMapDataObject(this._serviceProvider, dataObject))
                {
                    return DataObjectConverterInfo.CanConvert;
                }
                return DataObjectConverterInfo.Disabled;
            }

            public override bool ConvertToHtml(IDataObject originalDataObject, DataObject newDataObject)
            {
                IDataObjectMappingService service = (IDataObjectMappingService) this._serviceProvider.GetService(typeof(IDataObjectMappingService));
                bool flag = service.GetDataObjectMapper(originalDataObject, DataFormats.Html).PerformMapping(this._serviceProvider, originalDataObject, newDataObject);
                if (!flag)
                {
                    flag = base.ConvertToHtml(originalDataObject, newDataObject);
                }
                return flag;
            }
        }
    }
}


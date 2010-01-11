namespace Microsoft.Matrix.Packages.Web.Documents.Design
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Packages.Web.Html.WebForms;
    using Microsoft.Matrix.Packages.Web.Services;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    public class WebFormsDocumentDesigner : HtmlDocumentDesigner, ICommandHandler, IEventBindingService
    {
        private IDictionary _eventProperties;
        private Microsoft.Matrix.Packages.Web.Designer.WebFormsReferenceManager _referenceManager;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IComponentChangeService service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
                service.ComponentChanged -= new ComponentChangedEventHandler(this.OnComponentChanged);
                IServiceContainer container = (IServiceContainer) this.GetService(typeof(IServiceContainer));
                if (container != null)
                {
                    container.RemoveService(typeof(IWebFormReferenceManager));
                    container.RemoveService(typeof(IEventBindingService));
                    container.RemoveService(typeof(ITypeResolutionService));
                    container.RemoveService(typeof(ITemplateEditingService));
                }
                if (this._referenceManager != null)
                {
                    ((IDisposable) this._referenceManager).Dispose();
                    this._referenceManager = null;
                }
            }
            base.Dispose(disposing);
        }

        private CodeDocumentLanguage GetCodeDocumentLanguage()
        {
            IDocumentWithCode component = base.Component as IDocumentWithCode;
            return component.Code.Language;
        }

        private CodeView GetCodeView(IMultiViewDocumentWindow window)
        {
            return (window.GetViewByType(DocumentViewType.Code) as CodeView);
        }

        private IMultiViewDocumentWindow GetDocumentWindow()
        {
            return (this.GetService(typeof(DocumentWindow)) as IMultiViewDocumentWindow);
        }

        private string GetEventDescriptorHashCode(EventDescriptor eventDesc)
        {
            StringBuilder builder = new StringBuilder(eventDesc.Name);
            builder.Append(eventDesc.EventType.GetHashCode().ToString());
            foreach (Attribute attribute in eventDesc.Attributes)
            {
                builder.Append(attribute.GetHashCode().ToString());
            }
            return builder.ToString();
        }

        protected override bool HandleCommand(Command command)
        {
            bool flag = false;
            bool flag2 = false;
            if (command.CommandGroup == typeof(WebCommands))
            {
                ISelectionService service = (ISelectionService) this.GetService(typeof(ISelectionService));
                if (service.SelectionCount == 1)
                {
                    WebControl primarySelection = service.PrimarySelection as WebControl;
                    if (primarySelection != null)
                    {
                        IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                        ControlDesigner designer = (ControlDesigner) host.GetDesigner(primarySelection);
                        if ((designer != null) && designer.ReadOnly)
                        {
                            IComponentChangeService service2 = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
                            DesignerTransaction transaction = host.CreateTransaction();
                            bool flag3 = false;
                            PropertyDescriptor member = null;
                            object oldValue = null;
                            object color = null;
                            switch (command.CommandID)
                            {
                                case 100:
                                case 0x65:
                                case 0x66:
                                case 0x69:
                                case 0x79:
                                case 0x7a:
                                    flag3 = true;
                                    break;

                                case 0x6a:
                                    member = TypeDescriptor.GetProperties(primarySelection)["ForeColor"];
                                    oldValue = member.GetValue(primarySelection);
                                    flag3 = true;
                                    break;

                                case 0x6b:
                                    member = TypeDescriptor.GetProperties(primarySelection)["BackColor"];
                                    oldValue = member.GetValue(primarySelection);
                                    flag3 = true;
                                    break;
                            }
                            if (flag3)
                            {
                                service2.OnComponentChanging(primarySelection, member);
                            }
                            try
                            {
                                PropertyDescriptor descriptor7;
                                object xXSmall;
                                switch (command.CommandID)
                                {
                                    case 100:
                                        TypeDescriptor.GetProperties(typeof(FontInfo))["Bold"].SetValue(primarySelection.Font, !primarySelection.Font.Bold);
                                        flag2 = true;
                                        flag = true;
                                        goto Label_041E;

                                    case 0x65:
                                        TypeDescriptor.GetProperties(typeof(FontInfo))["Italic"].SetValue(primarySelection.Font, !primarySelection.Font.Italic);
                                        flag2 = true;
                                        flag = true;
                                        goto Label_041E;

                                    case 0x66:
                                        TypeDescriptor.GetProperties(typeof(FontInfo))["Underline"].SetValue(primarySelection.Font, !primarySelection.Font.Underline);
                                        flag2 = true;
                                        flag = true;
                                        goto Label_041E;

                                    case 0x69:
                                        TypeDescriptor.GetProperties(typeof(FontInfo))["Strikeout"].SetValue(primarySelection.Font, !primarySelection.Font.Strikeout);
                                        flag2 = true;
                                        flag = true;
                                        goto Label_041E;

                                    case 0x6a:
                                    case 0x6b:
                                    {
                                        ColorDialog dialog = new ColorDialog();
                                        if (dialog.ShowDialog() != DialogResult.OK)
                                        {
                                            goto Label_03EC;
                                        }
                                        color = dialog.Color;
                                        member.SetValue(primarySelection, color);
                                        goto Label_03EF;
                                    }
                                    case 0x79:
                                        TypeDescriptor.GetProperties(typeof(FontInfo))["Name"].SetValue(primarySelection.Font, ((ComboBoxToolBarButton) command.CommandUI).ComboBox.Text);
                                        flag2 = true;
                                        flag = true;
                                        goto Label_041E;

                                    case 0x7a:
                                    {
                                        descriptor7 = TypeDescriptor.GetProperties(typeof(FontInfo))["Size"];
                                        int selectedIndex = ((ComboBoxToolBarButton) command.CommandUI).ComboBox.SelectedIndex;
                                        xXSmall = null;
                                        switch (selectedIndex)
                                        {
                                            case 1:
                                                goto Label_0356;

                                            case 2:
                                                goto Label_0364;

                                            case 3:
                                                goto Label_0372;

                                            case 4:
                                                goto Label_0380;

                                            case 5:
                                                goto Label_038E;

                                            case 6:
                                                goto Label_039C;
                                        }
                                        goto Label_03A8;
                                    }
                                    default:
                                        goto Label_041E;
                                }
                                xXSmall = FontUnit.XXSmall;
                                goto Label_03A8;
                            Label_0356:
                                xXSmall = FontUnit.XSmall;
                                goto Label_03A8;
                            Label_0364:
                                xXSmall = FontUnit.Small;
                                goto Label_03A8;
                            Label_0372:
                                xXSmall = FontUnit.Medium;
                                goto Label_03A8;
                            Label_0380:
                                xXSmall = FontUnit.Large;
                                goto Label_03A8;
                            Label_038E:
                                xXSmall = FontUnit.XLarge;
                                goto Label_03A8;
                            Label_039C:
                                xXSmall = FontUnit.XXLarge;
                            Label_03A8:
                                if (xXSmall != null)
                                {
                                    descriptor7.SetValue(primarySelection.Font, xXSmall);
                                    flag2 = true;
                                }
                                flag = true;
                                goto Label_041E;
                            Label_03EC:
                                flag3 = false;
                            Label_03EF:
                                flag = true;
                            }
                            finally
                            {
                                if (flag3)
                                {
                                    service2.OnComponentChanged(primarySelection, member, oldValue, color);
                                    if (transaction != null)
                                    {
                                        transaction.Commit();
                                    }
                                }
                                else if (transaction != null)
                                {
                                    transaction.Cancel();
                                }
                            }
                        }
                    }
                }
            }
        Label_041E:
            if (flag2)
            {
                ((ICommandManager) this.GetService(typeof(ICommandManager))).UpdateCommands(false);
            }
            return flag;
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            IComponentChangeService service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
            service.ComponentChanged += new ComponentChangedEventHandler(this.OnComponentChanged);
            IServiceContainer serviceProvider = (IServiceContainer) this.GetService(typeof(IServiceContainer));
            if (serviceProvider != null)
            {
                ITypeResolutionService serviceInstance = (ITypeResolutionService) this.GetService(typeof(ITypeResolutionService));
                this._referenceManager = new Microsoft.Matrix.Packages.Web.Designer.WebFormsReferenceManager(serviceProvider, ((WebFormsDocument) component).RegisterDirectives);
                serviceProvider.AddService(typeof(IWebFormReferenceManager), this._referenceManager);
                serviceProvider.AddService(typeof(IEventBindingService), this);
                serviceProvider.AddService(typeof(ITypeResolutionService), serviceInstance);
                serviceProvider.AddService(typeof(ITemplateEditingService), new Microsoft.Matrix.Packages.Web.Services.TemplateEditingService(serviceProvider));
            }
        }

        protected void OnComponentChanged(object source, ComponentChangedEventArgs args)
        {
            System.Web.UI.Control component = args.Component as System.Web.UI.Control;
            if (component != null)
            {
                IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                ControlDesigner designer = service.GetDesigner(component) as ControlDesigner;
                if (designer != null)
                {
                    designer.OnComponentChanged(source, args);
                }
            }
        }

        string IEventBindingService.CreateUniqueMethodName(IComponent component, EventDescriptor e)
        {
            TextBuffer buffer;
            ICodeBehindDocumentLanguage codeDocumentLanguage = this.GetCodeDocumentLanguage() as ICodeBehindDocumentLanguage;
            if (codeDocumentLanguage == null)
            {
                return string.Empty;
            }
            IMultiViewDocumentWindow documentWindow = this.GetDocumentWindow();
            CodeView codeView = this.GetCodeView(documentWindow);
            if (codeView.Text.Length != 0)
            {
                buffer = codeView.Buffer;
            }
            else
            {
                IDocumentWithCode code = base.Component as IDocumentWithCode;
                buffer = new TextBuffer(code.Code.Text);
            }
            return codeDocumentLanguage.GenerateEventHandlerName(buffer, component, e);
        }

        ICollection IEventBindingService.GetCompatibleMethods(EventDescriptor e)
        {
            TextBuffer buffer;
            ICodeBehindDocumentLanguage codeDocumentLanguage = this.GetCodeDocumentLanguage() as ICodeBehindDocumentLanguage;
            if (codeDocumentLanguage == null)
            {
                return new string[0];
            }
            IMultiViewDocumentWindow documentWindow = this.GetDocumentWindow();
            CodeView codeView = this.GetCodeView(documentWindow);
            if (codeView.Text.Length != 0)
            {
                buffer = codeView.Buffer;
            }
            else
            {
                IDocumentWithCode component = base.Component as IDocumentWithCode;
                buffer = new TextBuffer(component.Code.Text);
            }
            return codeDocumentLanguage.GetEventHandlers(buffer, e);
        }

        EventDescriptor IEventBindingService.GetEvent(PropertyDescriptor p)
        {
            foreach (PropertyDescriptor descriptor in this.EventProperties.Values)
            {
                if ((descriptor is EventPropertyDescriptor) && (descriptor == p))
                {
                    return ((EventPropertyDescriptor) descriptor).EventDescriptor;
                }
            }
            return null;
        }

        PropertyDescriptorCollection IEventBindingService.GetEventProperties(EventDescriptorCollection e)
        {
            if (!(this.GetCodeDocumentLanguage() is ICodeBehindDocumentLanguage))
            {
                return new PropertyDescriptorCollection(null);
            }
            PropertyDescriptor[] properties = new PropertyDescriptor[e.Count];
            for (int i = 0; i < e.Count; i++)
            {
                properties[i] = ((IEventBindingService) this).GetEventProperty(e[i]);
            }
            return new PropertyDescriptorCollection(properties);
        }

        PropertyDescriptor IEventBindingService.GetEventProperty(EventDescriptor e)
        {
            PropertyDescriptor descriptor = (PropertyDescriptor) this.EventProperties[this.GetEventDescriptorHashCode(e)];
            if (descriptor == null)
            {
                descriptor = new EventPropertyDescriptor(e);
                this.EventProperties[this.GetEventDescriptorHashCode(e)] = descriptor;
            }
            return descriptor;
        }

        bool IEventBindingService.ShowCode()
        {
            IMultiViewDocumentWindow documentWindow = this.GetDocumentWindow();
            documentWindow.ActivateView(this.GetCodeView(documentWindow));
            return true;
        }

        bool IEventBindingService.ShowCode(int lineNum)
        {
            IMultiViewDocumentWindow documentWindow = this.GetDocumentWindow();
            CodeView codeView = this.GetCodeView(documentWindow);
            documentWindow.ActivateView(codeView);
            codeView.CaretLineIndex = lineNum;
            return true;
        }

        bool IEventBindingService.ShowCode(IComponent component, EventDescriptor e)
        {
            bool flag;
            ICodeBehindDocumentLanguage codeDocumentLanguage = this.GetCodeDocumentLanguage() as ICodeBehindDocumentLanguage;
            if (codeDocumentLanguage == null)
            {
                return false;
            }
            IMultiViewDocumentWindow documentWindow = this.GetDocumentWindow();
            HtmlDesignView viewByType = documentWindow.GetViewByType(DocumentViewType.Design) as HtmlDesignView;
            CodeView codeView = this.GetCodeView(documentWindow);
            PropertyDescriptor eventProperty = ((IEventBindingService) this).GetEventProperty(e);
            string str = eventProperty.GetValue(component) as string;
            if (str == null)
            {
                str = ((IEventBindingService) this).CreateUniqueMethodName(component, e);
                eventProperty.SetValue(component, str);
            }
            viewByType.UpdateDesignState();
            documentWindow.ActivateView(codeView);
            int num = codeDocumentLanguage.GenerateEventHandler(codeView.Buffer, str, e, out flag);
            if (num == -1)
            {
                num = 0;
            }
            codeView.CaretLineIndex = num;
            codeView.CaretColumnIndex = 0;
            if (flag)
            {
                codeView.CenterCaret();
            }
            return true;
        }

        protected override bool UpdateCommand(Command command)
        {
            int num;
            bool flag = false;
            if (command.CommandGroup == typeof(WebCommands))
            {
                ISelectionService service = (ISelectionService) this.GetService(typeof(ISelectionService));
                if (service.SelectionCount != 1)
                {
                    return flag;
                }
                WebControl primarySelection = service.PrimarySelection as WebControl;
                if (primarySelection != null)
                {
                    switch (command.CommandID)
                    {
                        case 100:
                            command.Checked = primarySelection.Font.Bold;
                            return true;

                        case 0x65:
                            command.Checked = primarySelection.Font.Italic;
                            return true;

                        case 0x66:
                            command.Checked = primarySelection.Font.Underline;
                            return true;

                        case 0x67:
                        case 0x68:
                            return flag;

                        case 0x69:
                            command.Checked = primarySelection.Font.Strikeout;
                            return true;

                        case 0x6a:
                        case 0x6b:
                            return true;

                        case 0x79:
                            command.Text = primarySelection.Font.Name;
                            return true;

                        case 0x7a:
                            num = -1;
                            switch (primarySelection.Font.Size.Type)
                            {
                                case FontSize.NotSet:
                                    num = -1;
                                    goto Label_0180;

                                case FontSize.AsUnit:
                                case FontSize.Smaller:
                                case FontSize.Larger:
                                    goto Label_0179;

                                case FontSize.XXSmall:
                                    num = 0;
                                    goto Label_0180;

                                case FontSize.XSmall:
                                    num = 1;
                                    goto Label_0180;

                                case FontSize.Small:
                                    num = 2;
                                    goto Label_0180;

                                case FontSize.Medium:
                                    num = 3;
                                    goto Label_0180;

                                case FontSize.Large:
                                    num = 4;
                                    goto Label_0180;

                                case FontSize.XLarge:
                                    num = 5;
                                    goto Label_0180;

                                case FontSize.XXLarge:
                                    num = 6;
                                    goto Label_0180;
                            }
                            goto Label_0179;
                    }
                }
            }
            return flag;
        Label_0179:
            command.Enabled = false;
        Label_0180:
            ((ToolBarComboBoxCommand) command).SelectedIndex = num;
            return true;
        }

        private IDictionary EventProperties
        {
            get
            {
                if (this._eventProperties == null)
                {
                    this._eventProperties = new HybridDictionary();
                }
                return this._eventProperties;
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.Mobile.Design
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Documents.Design;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Web.UI;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms;

    public class MobileWebFormsDocumentDesigner : WebFormsDocumentDesigner, IMobileWebFormServices
    {
        private HybridDictionary _cachedObjects;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    service.RemoveService(typeof(IMobileWebFormServices));
                }
            }
            base.Dispose(disposing);
        }

        private BooleanOption GetNegatedBooleanOption(BooleanOption oldValue)
        {
            switch (oldValue)
            {
                case -1:
                    return 1;

                case 0:
                    return 1;

                case 1:
                    return 0;
            }
            return -1;
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
                    MobileControl primarySelection = service.PrimarySelection as MobileControl;
                    if ((primarySelection != null) && !(primarySelection is StyleSheet))
                    {
                        IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
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
                            case 0x79:
                            case 0x7a:
                                member = TypeDescriptor.GetProperties(primarySelection)["Font"];
                                flag3 = true;
                                break;

                            case 0x66:
                            case 0x69:
                                flag3 = false;
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

                            case 0x6c:
                            case 0x6d:
                            case 110:
                                member = TypeDescriptor.GetProperties(primarySelection)["Alignment"];
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
                            PropertyDescriptor descriptor5;
                            object obj4;
                            switch (command.CommandID)
                            {
                                case 100:
                                    TypeDescriptor.GetProperties(typeof(FontInfo))["Bold"].SetValue(primarySelection.get_Font(), this.GetNegatedBooleanOption(primarySelection.get_Font().get_Bold()));
                                    flag2 = true;
                                    flag = true;
                                    goto Label_03F4;

                                case 0x65:
                                    TypeDescriptor.GetProperties(typeof(FontInfo))["Italic"].SetValue(primarySelection.get_Font(), this.GetNegatedBooleanOption(primarySelection.get_Font().get_Italic()));
                                    flag2 = true;
                                    flag = true;
                                    goto Label_03F4;

                                case 0x66:
                                case 0x69:
                                    flag2 = false;
                                    flag = false;
                                    goto Label_03F4;

                                case 0x6a:
                                case 0x6b:
                                {
                                    ColorDialog dialog = new ColorDialog();
                                    if (dialog.ShowDialog() != DialogResult.OK)
                                    {
                                        goto Label_0374;
                                    }
                                    color = dialog.Color;
                                    member.SetValue(primarySelection, color);
                                    goto Label_0377;
                                }
                                case 0x6c:
                                    member.SetValue(primarySelection, (Alignment) 1);
                                    color = (Alignment) 1;
                                    flag = true;
                                    goto Label_03F4;

                                case 0x6d:
                                    member.SetValue(primarySelection, (Alignment) 3);
                                    color = (Alignment) 3;
                                    flag = true;
                                    goto Label_03F4;

                                case 110:
                                    member.SetValue(primarySelection, (Alignment) 2);
                                    color = (Alignment) 2;
                                    flag = true;
                                    goto Label_03F4;

                                case 0x79:
                                {
                                    PropertyDescriptor descriptor4 = TypeDescriptor.GetProperties(typeof(FontInfo))["Name"];
                                    string text = ((ComboBoxToolBarButton) command.CommandUI).ComboBox.Text;
                                    descriptor4.SetValue(primarySelection.get_Font(), text);
                                    color = text;
                                    flag2 = true;
                                    flag = true;
                                    goto Label_03F4;
                                }
                                case 0x7a:
                                {
                                    descriptor5 = TypeDescriptor.GetProperties(typeof(FontInfo))["Size"];
                                    int selectedIndex = ((ComboBoxToolBarButton) command.CommandUI).ComboBox.SelectedIndex;
                                    obj4 = null;
                                    switch (selectedIndex)
                                    {
                                        case 3:
                                        case 4:
                                            goto Label_031A;

                                        case 5:
                                        case 6:
                                            goto Label_0324;
                                    }
                                    goto Label_032C;
                                }
                                default:
                                    goto Label_03F4;
                            }
                            obj4 = (FontSize) 2;
                            goto Label_032C;
                        Label_031A:
                            obj4 = (FontSize) 1;
                            goto Label_032C;
                        Label_0324:
                            obj4 = (FontSize) 3;
                        Label_032C:
                            if (obj4 != null)
                            {
                                descriptor5.SetValue(primarySelection.get_Font(), obj4);
                                color = obj4;
                                flag2 = true;
                            }
                            flag = true;
                            goto Label_03F4;
                        Label_0374:
                            flag3 = false;
                        Label_0377:
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
        Label_03F4:
            if (flag2)
            {
                ((ICommandManager) this.GetService(typeof(ICommandManager))).UpdateCommands(false);
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                service.AddService(typeof(IMobileWebFormServices), this);
            }
        }

        void IMobileWebFormServices.ClearUndoStack()
        {
        }

        object IMobileWebFormServices.GetCache(string controlID, object key)
        {
            if (((controlID == null) || (controlID.Length <= 0)) || (key == null))
            {
                return null;
            }
            IDictionary dictionary = (IDictionary) this.CachedObjects[controlID];
            if (dictionary == null)
            {
                return null;
            }
            return dictionary[key];
        }

        void IMobileWebFormServices.RefreshPageView()
        {
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            MobilePage rootComponent = service.RootComponent as MobilePage;
            if (rootComponent != null)
            {
                this.UpdateRenderingRecursive(rootComponent);
            }
        }

        void IMobileWebFormServices.SetCache(string controlID, object key, object value)
        {
            if (((controlID != null) && (controlID.Length > 0)) && (key != null))
            {
                IDictionary dictionary = (IDictionary) this.CachedObjects[controlID];
                if (value != null)
                {
                    if (dictionary == null)
                    {
                        dictionary = new HybridDictionary(false);
                        this.CachedObjects[controlID] = dictionary;
                    }
                    dictionary[key] = value;
                }
                else if (dictionary != null)
                {
                    dictionary.Remove(key);
                }
            }
        }

        void IMobileWebFormServices.UpdateRenderingRecursive(Control rootControl)
        {
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            foreach (Control control in rootControl.Controls)
            {
                IMobileDesigner designer = service.GetDesigner(control) as IMobileDesigner;
                if (designer != null)
                {
                    try
                    {
                        designer.UpdateRendering();
                    }
                    catch (Exception)
                    {
                    }
                }
                this.UpdateRenderingRecursive(control);
            }
        }

        protected override bool UpdateCommand(Command command)
        {
            int num;
            bool flag = false;
            if (command.CommandGroup != typeof(WebCommands))
            {
                goto Label_0287;
            }
            ISelectionService service = (ISelectionService) this.GetService(typeof(ISelectionService));
            if (service.SelectionCount == 1)
            {
                MobileControl primarySelection = service.PrimarySelection as MobileControl;
                if ((primarySelection != null) && !(primarySelection is StyleSheet))
                {
                    switch (command.CommandID)
                    {
                        case 100:
                            command.Checked = primarySelection.get_Font().get_Bold() == 1;
                            flag = true;
                            goto Label_018A;

                        case 0x65:
                            command.Checked = primarySelection.get_Font().get_Italic() == 1;
                            flag = true;
                            goto Label_018A;

                        case 0x66:
                        case 0x69:
                            flag = false;
                            goto Label_018A;

                        case 0x67:
                        case 0x68:
                            goto Label_018A;

                        case 0x6a:
                        case 0x6b:
                            flag = true;
                            goto Label_018A;

                        case 0x6c:
                            command.Checked = primarySelection.get_Alignment() == 1;
                            flag = true;
                            goto Label_018A;

                        case 0x6d:
                            command.Checked = primarySelection.get_Alignment() == 3;
                            flag = true;
                            goto Label_018A;

                        case 110:
                            command.Checked = primarySelection.get_Alignment() == 2;
                            flag = true;
                            goto Label_018A;

                        case 0x79:
                            command.Text = primarySelection.get_Font().get_Name();
                            flag = true;
                            goto Label_018A;

                        case 0x7a:
                            num = -1;
                            switch (primarySelection.get_Font().get_Size())
                            {
                                case 0:
                                    num = -1;
                                    goto Label_013F;

                                case 1:
                                    num = 3;
                                    goto Label_013F;

                                case 2:
                                    num = 0;
                                    goto Label_013F;

                                case 3:
                                    num = 5;
                                    goto Label_013F;
                            }
                            command.Enabled = false;
                            goto Label_013F;
                    }
                }
            }
            goto Label_018A;
        Label_013F:
            ((ToolBarComboBoxCommand) command).SelectedIndex = num;
            flag = true;
        Label_018A:
            if (!flag)
            {
                switch (command.CommandID)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 0x67:
                    case 0x68:
                    case 0x6c:
                    case 0x6d:
                    case 110:
                    case 0x6f:
                    case 0x70:
                    case 0x71:
                    case 0x72:
                    case 120:
                    case 0x7b:
                    case 140:
                    case 0x8d:
                    case 0x8e:
                    case 0x92:
                    case 0x93:
                    case 0x94:
                    case 0x95:
                    case 150:
                    case 0x97:
                    case 0x98:
                    case 200:
                    case 0xc9:
                    case 0xca:
                    case 0xcb:
                    case 220:
                        command.Enabled = false;
                        flag = true;
                        goto Label_0287;

                    case 0x69:
                    case 0x6a:
                    case 0x6b:
                    case 0x73:
                    case 0x74:
                    case 0x75:
                    case 0x76:
                    case 0x77:
                    case 0x79:
                    case 0x7a:
                    case 0x8f:
                    case 0x90:
                    case 0x91:
                        goto Label_0287;
                }
            }
        Label_0287:
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }

        private IDictionary CachedObjects
        {
            get
            {
                if (this._cachedObjects == null)
                {
                    this._cachedObjects = new HybridDictionary(false);
                }
                return this._cachedObjects;
            }
        }
    }
}


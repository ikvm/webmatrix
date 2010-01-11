namespace Microsoft.Matrix.Core.Packages
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class CorePackage : IPackage, IDisposable, ICommandHandler
    {
        private Microsoft.Matrix.Core.Plugins.AddInManager _addInManager;
        private MruList _askQuestionMruList;
        private MruList _findMruList;
        private FindReplaceOptions _lastFindReplaceOptions;
        private MruList _replaceMruList;
        private IServiceProvider _serviceProvider;
        private MxTextManager _textManager;

        private Document GetActiveDocument()
        {
            IDocumentManager service = (IDocumentManager) this._serviceProvider.GetService(typeof(IDocumentManager));
            if (service == null)
            {
                return null;
            }
            return service.ActiveDocument;
        }

        private IDocumentView GetActiveDocumentView()
        {
            Document activeDocument = this.GetActiveDocument();
            if (activeDocument == null)
            {
                return null;
            }
            DocumentWindow service = (DocumentWindow) activeDocument.Site.GetService(typeof(DocumentWindow));
            if (service == null)
            {
                return null;
            }
            return service.DocumentView;
        }

        OptionsPage[] IPackage.GetOptionsPages()
        {
            ArrayList list = new ArrayList();
            ILanguageManager service = (ILanguageManager) this._serviceProvider.GetService(typeof(ILanguageManager));
            if (service != null)
            {
                list.Add(new LanguageOptionsPage(this._serviceProvider));
                foreach (DocumentLanguage language in service.DocumentLanguages)
                {
                    TextDocumentLanguage language2 = language as TextDocumentLanguage;
                    if ((language2 != null) && language2.SupportsCustomOptions)
                    {
                        list.Add(new TextLanguageOptionsPage(language2, this._serviceProvider));
                    }
                }
            }
            IApplicationIdentity identity = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
            if ((identity.ApplicationType & ApplicationType.Workspace) != ApplicationType.Generic)
            {
                list.Add(new FontOptionsPage(this._textManager, this._serviceProvider));
            }
            return (OptionsPage[]) list.ToArray(typeof(OptionsPage));
        }

        void IPackage.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            IServiceContainer service = this._serviceProvider.GetService(typeof(IServiceContainer)) as IServiceContainer;
            IApplicationIdentity identity = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
            if ((identity.ApplicationType & ApplicationType.Workspace) != ApplicationType.Generic)
            {
                this._textManager = new MxTextManager(serviceProvider);
                service.AddService(typeof(TextManager), this._textManager);
            }
            ICommandManager manager = (ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager));
            if (manager != null)
            {
                manager.AddGlobalCommandHandler(this);
            }
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            ISearchableDocumentView view;
            bool flag = false;
            bool flag2 = false;
            if (command.CommandGroup.Equals(typeof(GlobalCommands)))
            {
                switch (command.CommandID)
                {
                    case 0x6c:
                        goto Label_02E2;

                    case 0x6d:
                    {
                        ISearchableDocumentView activeDocumentView = this.GetActiveDocumentView() as ISearchableDocumentView;
                        if (activeDocumentView != null)
                        {
                            ReplaceDialog dialog2 = new ReplaceDialog(this._serviceProvider, activeDocumentView, this._lastFindReplaceOptions, activeDocumentView.InitialSearchString, this.FindMruList, this.ReplaceMruList);
                            IUIService service7 = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                            if (service7 != null)
                            {
                                service7.ShowDialog(dialog2);
                                this._lastFindReplaceOptions = dialog2.FindReplaceOptions;
                            }
                            flag2 = true;
                        }
                        flag = true;
                        goto Label_052C;
                    }
                    case 110:
                    {
                        ISearchableDocumentView view3 = this.GetActiveDocumentView() as ISearchableDocumentView;
                        if (((view3 != null) && (command.Text != null)) && (command.Text.Length > 0))
                        {
                            this.FindMruList.AddEntry(command.Text);
                            flag2 = true;
                            if (!view3.PerformFind(command.Text, this._lastFindReplaceOptions & (FindReplaceOptions.WholeWord | FindReplaceOptions.MatchCase)))
                            {
                                IUIService service6 = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                                if (service6 != null)
                                {
                                    service6.ShowMessage("Couldn't find '" + command.Text + "'", string.Empty, MessageBoxButtons.OK);
                                }
                            }
                        }
                        flag = true;
                        goto Label_052C;
                    }
                    case 8:
                    {
                        IPrintService service = (IPrintService) this._serviceProvider.GetService(typeof(IPrintService));
                        if (service != null)
                        {
                            service.ConfigurePrintSettings();
                        }
                        flag = true;
                        goto Label_052C;
                    }
                    case 300:
                    case 0x12d:
                    case 0x12e:
                    case 0x12f:
                    case 0x130:
                    case 0x131:
                    case 0x132:
                    case 0x133:
                    case 0x134:
                    case 0x135:
                    case 320:
                    case 0x141:
                        ((ICommandHandler) this.AddInManager).HandleCommand(command);
                        flag = true;
                        goto Label_052C;

                    case 310:
                    case 0x137:
                    case 0x138:
                    case 0x139:
                    case 0x13a:
                    case 0x13b:
                    case 0x13c:
                    case 0x13d:
                    case 0x13e:
                    case 0x13f:
                    case 0x142:
                    case 0x143:
                    case 0x259:
                    case 0x25d:
                    case 0x25f:
                    case 0x260:
                    case 0x261:
                        goto Label_052C;

                    case 0x144:
                    {
                        OptionsDialog dialog3 = new OptionsDialog(this._serviceProvider);
                        ((IUIService) this._serviceProvider.GetService(typeof(IUIService))).ShowDialog(dialog3);
                        flag = true;
                        goto Label_052C;
                    }
                    case 0x145:
                    case 0x146:
                    case 0x147:
                    {
                        IToolboxService service8 = this._serviceProvider.GetService(typeof(IToolboxService)) as IToolboxService;
                        ToolboxSection activeSection = service8.ActiveSection;
                        if (activeSection != null)
                        {
                            activeSection.Customize(command.CommandID - 0x145, this._serviceProvider);
                        }
                        flag = true;
                        goto Label_052C;
                    }
                    case 180:
                    {
                        ISearchableDocumentView view2 = this.GetActiveDocumentView() as ISearchableDocumentView;
                        if (view2 != null)
                        {
                            if (this.FindMruList.Count == 0)
                            {
                                goto Label_02E2;
                            }
                            view2.PerformFind(this.FindMruList[0], this._lastFindReplaceOptions & (FindReplaceOptions.WholeWord | FindReplaceOptions.MatchCase));
                            flag2 = true;
                        }
                        flag = true;
                        goto Label_052C;
                    }
                    case 600:
                        this.OnCommandHelpTopics();
                        flag = true;
                        goto Label_052C;

                    case 0x25a:
                    case 0x25e:
                    {
                        ApplicationInfoDialog dialog = new ApplicationInfoDialog(this._serviceProvider, command.CommandID == 0x25e);
                        IUIService service4 = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                        if (service4 == null)
                        {
                            dialog.ShowDialog();
                        }
                        else
                        {
                            service4.ShowDialog(dialog);
                        }
                        flag = true;
                        goto Label_052C;
                    }
                    case 0x25b:
                    case 0x25c:
                    {
                        string format = ConfigurationSettings.AppSettings["IDE.AskQuestionUrl"];
                        if ((format == null) || (format.Length == 0))
                        {
                            ((IUIService) this._serviceProvider.GetService(typeof(IUIService))).ShowMessage("Unable to bring up launch the answer page.", "Ask Question");
                        }
                        else
                        {
                            string entry = string.Empty;
                            if (command.CommandID == 0x25c)
                            {
                                entry = command.Text;
                                if (entry != null)
                                {
                                    this.AskQuestionMruList.AddEntry(entry);
                                    format = string.Format(format, entry);
                                    flag2 = true;
                                }
                            }
                            ((IWebBrowsingService) this._serviceProvider.GetService(typeof(IWebBrowsingService))).BrowseUrl(format);
                        }
                        flag = true;
                        goto Label_052C;
                    }
                    case 610:
                    case 0x263:
                    case 0x264:
                    case 0x265:
                    case 0x266:
                    case 0x267:
                    case 0x268:
                    case 0x269:
                    case 0x26a:
                    case 0x26b:
                    {
                        IDictionary webLinks = this.WebLinks;
                        if (webLinks != null)
                        {
                            int num = command.CommandID - 610;
                            string str = "Help" + num;
                            WebLink link = (WebLink) webLinks[str];
                            if (link != null)
                            {
                                this.OnCommandBrowseUrl(link.Url);
                            }
                        }
                        flag = true;
                        goto Label_052C;
                    }
                }
            }
            goto Label_052C;
        Label_02E2:
            view = this.GetActiveDocumentView() as ISearchableDocumentView;
            if (view != null)
            {
                SearchForm form = new SearchForm(this._serviceProvider, view, this._lastFindReplaceOptions, view.InitialSearchString, this.FindMruList);
                IUIService service5 = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                if (service5 != null)
                {
                    service5.ShowDialog(form);
                    this._lastFindReplaceOptions = form.FindReplaceOptions;
                }
                flag2 = true;
            }
            flag = true;
        Label_052C:
            if (flag2)
            {
                ((ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager))).UpdateCommands(false);
            }
            return flag;
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            ISearchableDocumentView view;
            bool flag = false;
            if (command.CommandGroup.Equals(typeof(GlobalCommands)))
            {
                int commandID = command.CommandID;
                if (commandID <= 0x6f)
                {
                    switch (commandID)
                    {
                        case 0x6c:
                        case 110:
                            goto Label_0214;

                        case 0x6d:
                        {
                            ISearchableDocumentView activeDocumentView = this.GetActiveDocumentView() as ISearchableDocumentView;
                            if (activeDocumentView == null)
                            {
                                command.Enabled = false;
                            }
                            else
                            {
                                command.Enabled = activeDocumentView.ReplaceSupport != FindReplaceOptions.None;
                            }
                            return true;
                        }
                        case 0x6f:
                            ((ToolBarComboBoxCommand) command).Items = this.FindMruList.Save();
                            return true;

                        case 8:
                            return true;
                    }
                    return flag;
                }
                switch (commandID)
                {
                    case 300:
                    case 0x12d:
                    case 0x12e:
                    case 0x12f:
                    case 0x130:
                    case 0x131:
                    case 0x132:
                    case 0x133:
                    case 0x134:
                    case 0x135:
                    case 320:
                    case 0x141:
                        ((ICommandHandler) this.AddInManager).UpdateCommand(command);
                        return true;

                    case 310:
                    case 0x137:
                    case 0x138:
                    case 0x139:
                    case 0x13a:
                    case 0x13b:
                    case 0x13c:
                    case 0x13d:
                    case 0x13e:
                    case 0x13f:
                    case 0x142:
                    case 0x143:
                        return flag;

                    case 0x144:
                        command.Enabled = true;
                        return true;

                    case 0x145:
                    case 0x146:
                    case 0x147:
                    {
                        IToolboxService service = this._serviceProvider.GetService(typeof(IToolboxService)) as IToolboxService;
                        ToolboxSection activeSection = service.ActiveSection;
                        if (activeSection == null)
                        {
                            if (command.CommandID == 0x145)
                            {
                                command.Text = "Customize Toolbox...";
                                command.Visible = true;
                                command.Enabled = false;
                            }
                            else
                            {
                                command.Visible = false;
                                command.Enabled = false;
                            }
                        }
                        else
                        {
                            string customizationText = activeSection.GetCustomizationText(command.CommandID - 0x145);
                            if (customizationText == null)
                            {
                                if (command.CommandID == 0x145)
                                {
                                    command.Text = "Customize Toolbox...";
                                    command.Visible = true;
                                    command.Enabled = false;
                                }
                                else
                                {
                                    command.Visible = false;
                                    command.Enabled = false;
                                }
                            }
                            else
                            {
                                command.Text = customizationText;
                                command.Visible = true;
                                command.Enabled = true;
                            }
                        }
                        return true;
                    }
                    case 180:
                        goto Label_0214;

                    case 600:
                        return true;

                    case 0x259:
                    case 0x25f:
                    case 0x260:
                    case 0x261:
                        return flag;

                    case 0x25a:
                    case 0x25e:
                        return true;

                    case 0x25b:
                    case 0x25c:
                        if ((command.CommandID == 0x25c) && (this.AskQuestionMruList.Count != 0))
                        {
                            command.Text = this.AskQuestionMruList[0];
                        }
                        return true;

                    case 0x25d:
                        ((ToolBarComboBoxCommand) command).Items = this.AskQuestionMruList.Save();
                        return true;

                    case 610:
                    case 0x263:
                    case 0x264:
                    case 0x265:
                    case 0x266:
                    case 0x267:
                    case 0x268:
                    case 0x269:
                    case 0x26a:
                    case 0x26b:
                    {
                        IDictionary webLinks = this.WebLinks;
                        WebLink link = null;
                        if (webLinks != null)
                        {
                            int num = command.CommandID - 610;
                            string str = "Help" + num;
                            link = (WebLink) webLinks[str];
                        }
                        if (link != null)
                        {
                            command.Text = link.Title;
                        }
                        else
                        {
                            command.Enabled = false;
                            command.Visible = false;
                        }
                        return true;
                    }
                }
            }
            return flag;
        Label_0214:
            view = this.GetActiveDocumentView() as ISearchableDocumentView;
            if (view != null)
            {
                command.Enabled = view.FindSupport != FindReplaceOptions.None;
            }
            else
            {
                command.Enabled = false;
            }
            if ((command.CommandID == 110) && (this.FindMruList.Count != 0))
            {
                command.Text = this.FindMruList[0];
            }
            return true;
        }

        private void OnCommandBrowseUrl(string url)
        {
            IWebBrowsingService service = (IWebBrowsingService) this._serviceProvider.GetService(typeof(IWebBrowsingService));
            if (service != null)
            {
                service.BrowseUrl(url);
            }
        }

        private void OnCommandHelpTopics()
        {
            IApplicationIdentity service = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
            if (service != null)
            {
                service.ShowHelpTopics();
            }
        }

        void IDisposable.Dispose()
        {
            Microsoft.Matrix.Core.Services.PreferencesStore preferencesStore = this.PreferencesStore;
            if (this._findMruList != null)
            {
                preferencesStore.SetValue("FindMruCount", this._findMruList.Count, 0);
                if (this._findMruList.Count != 0)
                {
                    string[] strArray = this._findMruList.Save();
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        preferencesStore.SetValue("FindMru" + i, strArray[i], string.Empty);
                    }
                }
            }
            if (this._replaceMruList != null)
            {
                preferencesStore.SetValue("ReplaceMruCount", this._replaceMruList.Count, 0);
                if (this._replaceMruList.Count != 0)
                {
                    string[] strArray2 = this._replaceMruList.Save();
                    for (int j = 0; j < strArray2.Length; j++)
                    {
                        preferencesStore.SetValue("ReplaceMru" + j, strArray2[j], string.Empty);
                    }
                }
            }
            if (this._askQuestionMruList != null)
            {
                preferencesStore.SetValue("AskQMruCount", this._askQuestionMruList.Count, 0);
                if (this._askQuestionMruList.Count != 0)
                {
                    string[] strArray3 = this._askQuestionMruList.Save();
                    for (int k = 0; k < strArray3.Length; k++)
                    {
                        preferencesStore.SetValue("AskQMru" + k, strArray3[k], string.Empty);
                    }
                }
            }
            IServiceContainer service = this._serviceProvider.GetService(typeof(IServiceContainer)) as IServiceContainer;
            if ((service != null) && (this._textManager != null))
            {
                service.RemoveService(typeof(TextManager));
                ((IDisposable) this._textManager).Dispose();
                this._textManager = null;
            }
            if (this._addInManager != null)
            {
                ((IDisposable) this._addInManager).Dispose();
                this._addInManager = null;
            }
            this._serviceProvider = null;
        }

        private Microsoft.Matrix.Core.Plugins.AddInManager AddInManager
        {
            get
            {
                if (this._addInManager == null)
                {
                    this._addInManager = new Microsoft.Matrix.Core.Plugins.AddInManager(this._serviceProvider);
                }
                return this._addInManager;
            }
        }

        private MruList AskQuestionMruList
        {
            get
            {
                if (this._askQuestionMruList == null)
                {
                    this._askQuestionMruList = new MruList(10);
                    Microsoft.Matrix.Core.Services.PreferencesStore preferencesStore = this.PreferencesStore;
                    int num = preferencesStore.GetValue("AskQMruCount", 0);
                    if (num != 0)
                    {
                        string[] entries = new string[num];
                        for (int i = 0; i < num; i++)
                        {
                            entries[i] = preferencesStore.GetValue("AskQMru" + i, string.Empty);
                        }
                        this._askQuestionMruList.Load(entries);
                    }
                }
                return this._askQuestionMruList;
            }
        }

        private MruList FindMruList
        {
            get
            {
                if (this._findMruList == null)
                {
                    this._findMruList = new MruList(10, null, false);
                    Microsoft.Matrix.Core.Services.PreferencesStore preferencesStore = this.PreferencesStore;
                    int num = preferencesStore.GetValue("FindMruCount", 0);
                    if (num != 0)
                    {
                        string[] entries = new string[num];
                        for (int i = 0; i < num; i++)
                        {
                            entries[i] = preferencesStore.GetValue("FindMru" + i, string.Empty);
                        }
                        this._findMruList.Load(entries);
                    }
                }
                return this._findMruList;
            }
        }

        private Microsoft.Matrix.Core.Services.PreferencesStore PreferencesStore
        {
            get
            {
                IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
                if (service != null)
                {
                    return service.GetPreferencesStore(typeof(CorePackage));
                }
                return null;
            }
        }

        private MruList ReplaceMruList
        {
            get
            {
                if (this._replaceMruList == null)
                {
                    this._replaceMruList = new MruList(10, null, false);
                    Microsoft.Matrix.Core.Services.PreferencesStore preferencesStore = this.PreferencesStore;
                    int num = preferencesStore.GetValue("ReplaceMruCount", 0);
                    if (num != 0)
                    {
                        string[] entries = new string[num];
                        for (int i = 0; i < num; i++)
                        {
                            entries[i] = preferencesStore.GetValue("ReplaceMru" + i, string.Empty);
                        }
                        this._replaceMruList.Load(entries);
                    }
                }
                return this._replaceMruList;
            }
        }

        private IDictionary WebLinks
        {
            get
            {
                IApplicationIdentity service = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
                if (service == null)
                {
                    return null;
                }
                return service.WebLinks;
            }
        }
    }
}


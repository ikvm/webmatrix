namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class AddInManager : IDisposable, ICommandHandler
    {
        private ArrayList _addIns;
        private ArrayList _menuAddIns;
        private IServiceProvider _serviceProvider;

        public AddInManager(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.ReadAddInsFromPreferences();
            if (this._addIns == null)
            {
                this._addIns = (ArrayList) ConfigurationSettings.GetConfig("microsoft.matrix/addIns");
                if (this._addIns != null)
                {
                    int num = 0;
                    foreach (AddInEntry entry in this._addIns)
                    {
                        entry.IncludeInMenu = true;
                        num++;
                        if (num == 10)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    this._addIns = new ArrayList();
                }
            }
            this.UpdateMenuAddIns();
        }

        private bool CanRunNow(AddInEntry entry)
        {
            if (entry.Scope == AddInScope.Document)
            {
                IDocumentManager service = (IDocumentManager) this._serviceProvider.GetService(typeof(IDocumentManager));
                if (service.ActiveDocument == null)
                {
                    return false;
                }
            }
            return true;
        }

        private ArrayList GetRunnableAddIns()
        {
            ArrayList list = new ArrayList();
            foreach (AddInEntry entry in this._addIns)
            {
                if (this.CanRunNow(entry))
                {
                    list.Add(entry);
                }
            }
            return list;
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
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
                    {
                        int num = command.CommandID - 300;
                        AddInEntry entry = (AddInEntry) this._menuAddIns[num];
                        this.RunAddIn(entry);
                        return true;
                    }
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
                        return flag;

                    case 320:
                    {
                        IUIService service = this._serviceProvider.GetService(typeof(IUIService)) as IUIService;
                        if (service != null)
                        {
                            AddInRunSelector form = new AddInRunSelector(this._serviceProvider, this.GetRunnableAddIns());
                            if (service.ShowDialog(form) == DialogResult.OK)
                            {
                                this.RunAddIn(form.SelectedEntry);
                            }
                        }
                        return true;
                    }
                    case 0x141:
                    {
                        IUIService service2 = this._serviceProvider.GetService(typeof(IUIService)) as IUIService;
                        if (service2 != null)
                        {
                            AddInOrganizer organizer = new AddInOrganizer(this._serviceProvider, this._addIns);
                            if (service2.ShowDialog(organizer) == DialogResult.OK)
                            {
                                this._addIns.Clear();
                                this._addIns.AddRange(organizer.AddIns);
                                this.UpdateMenuAddIns();
                            }
                        }
                        return true;
                    }
                }
            }
            return flag;
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
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
                    {
                        int num = command.CommandID - 300;
                        if (num >= this._menuAddIns.Count)
                        {
                            command.Enabled = false;
                            command.Visible = false;
                        }
                        else
                        {
                            AddInEntry entry = (AddInEntry) this._menuAddIns[num];
                            command.Text = entry.Name;
                            command.Enabled = this.CanRunNow(entry);
                            command.Visible = true;
                            command.HelpText = entry.Description;
                        }
                        return true;
                    }
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
                        return flag;

                    case 320:
                        command.Enabled = this._addIns.Count != 0;
                        return true;

                    case 0x141:
                        return true;
                }
            }
            return flag;
        }

        private void ReadAddInsFromPreferences()
        {
            IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(AddInManager));
                if (preferencesStore != null)
                {
                    int capacity = preferencesStore.GetValue("Count", -1);
                    if (capacity == -1)
                    {
                        this._addIns = null;
                    }
                    else
                    {
                        this._addIns = new ArrayList(capacity);
                        for (int i = 0; i < capacity; i++)
                        {
                            this._addIns.Add((AddInEntry) preferencesStore.GetValue("AddIn" + i));
                        }
                    }
                }
            }
        }

        private void RunAddIn(AddInEntry entry)
        {
            IServiceProvider serviceProvider = this._serviceProvider;
            if (entry.Scope == AddInScope.Document)
            {
                IDocumentManager service = (IDocumentManager) this._serviceProvider.GetService(typeof(IDocumentManager));
                serviceProvider = (IServiceProvider) service.ActiveDocument.Site.GetService(typeof(IDesignerHost));
            }
            new AddInHost(serviceProvider).RunAddIn(entry.TypeName);
        }

        void IDisposable.Dispose()
        {
            if (this._addIns != null)
            {
                this.WriteAddInsToPreferences();
                this._addIns = null;
                this._menuAddIns = null;
            }
        }

        private void UpdateMenuAddIns()
        {
            if (this._menuAddIns == null)
            {
                this._menuAddIns = new ArrayList();
            }
            else
            {
                this._menuAddIns.Clear();
            }
            int num = 0;
            foreach (AddInEntry entry in this._addIns)
            {
                if (entry.IncludeInMenu)
                {
                    this._menuAddIns.Add(entry);
                    num++;
                    if (num == 10)
                    {
                        break;
                    }
                }
            }
        }

        private void WriteAddInsToPreferences()
        {
            IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                service.ResetPreferencesStore(typeof(AddInManager));
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(AddInManager));
                if (preferencesStore != null)
                {
                    preferencesStore.SetValue("Count", this._addIns.Count, -1);
                    int num = 0;
                    foreach (object obj2 in this._addIns)
                    {
                        preferencesStore.SetValue("AddIn" + num, obj2);
                        num++;
                    }
                }
            }
        }
    }
}


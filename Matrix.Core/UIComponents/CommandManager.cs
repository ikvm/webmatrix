namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class CommandManager : Component, ICommandManager
    {
        private ICommandHandler _activeUICommandHandler;
        private ICommandHandler _activeUIContextCommandHandler;
        private ICommandHandler[] _commandHandlerList;
        private string _commandHelpText;
        private EventHandler _commandHelpTextChangedHandler;
        private IDictionary _commands;
        private bool _commandUpdatePending;
        private int _commandUpdateSuspendCount;
        private ICommandHandlerWithContext _contextCommandHandler;
        private IDictionary _contextMenus;
        private object _contextObject;
        private bool _firstResume;
        private ArrayList _globalCommandHandlers;
        private ArrayList _menuItems;
        private ICommandHandler _ownerCommandHandler;
        private IServiceProvider _serviceProvider;
        private ArrayList _statusBarPanels;
        private ArrayList _toolBarButtons;
        private ArrayList _toolBars;

        public CommandManager(IServiceProvider serviceProvider, ICommandHandler ownerCommandHandler)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            if (ownerCommandHandler == null)
            {
                throw new ArgumentNullException("ownerCommandHandler");
            }
            this._serviceProvider = serviceProvider;
            this._ownerCommandHandler = ownerCommandHandler;
            this.SuspendCommandUpdate();
            this._commandUpdatePending = true;
            this._firstResume = true;
            this._commands = new HybridDictionary();
            this._toolBars = new ArrayList();
            this._toolBarButtons = new ArrayList(0x40);
            this._menuItems = new ArrayList(0x80);
            this._statusBarPanels = new ArrayList();
            this._commandHandlerList = new ICommandHandler[] { this._ownerCommandHandler };
        }

        private void AddCommand(Command command)
        {
            Hashtable hashtable = (Hashtable) this._commands[command.CommandGroup];
            if (hashtable == null)
            {
                hashtable = new Hashtable();
                this._commands[command.CommandGroup] = hashtable;
            }
            hashtable[command.CommandID] = command;
        }

        public void AddCommand(ComboBoxToolBarButton button, Type commandGroup, int commandID)
        {
            this.AddCommand(button, commandGroup, commandID, 0);
        }

        public void AddCommand(MxMenuItem menuItem, Type commandGroup, int commandID)
        {
            this.AddCommand(menuItem, commandGroup, commandID, false);
        }

        public void AddCommand(MxToolBarButton button, Type commandGroup, int commandID)
        {
            ToolBarButtonCommand command = new ToolBarButtonCommand(commandGroup, commandID, button);
            button.Command = command;
            this._toolBarButtons.Add(button);
            this.AddCommand(command);
        }

        public void AddCommand(ComboBoxToolBarButton button, Type commandGroup, int commandID, int fillCommandID)
        {
            ToolBarComboBoxCommand command = new ToolBarComboBoxCommand(commandGroup, commandID, fillCommandID, button);
            button.Command = command;
            this._toolBarButtons.Add(button);
            this.AddCommand(command);
        }

        public void AddCommand(MxMenuItem menuItem, Type commandGroup, int commandID, bool isContextMenu)
        {
            MenuItemCommand command = new MenuItemCommand(commandGroup, commandID, menuItem);
            menuItem.Command = command;
            menuItem.Click += new EventHandler(this.OnMenuItemClick);
            if (this._commandHelpTextChangedHandler != null)
            {
                menuItem.Select += new EventHandler(this.OnMenuItemSelect);
            }
            if (!isContextMenu)
            {
                this._menuItems.Add(menuItem);
                this.AddCommand(command);
            }
        }

        public void AddMenu(MxMenuItem menu)
        {
            menu.Popup += new EventHandler(this.OnMenuItemPopup);
        }

        public void AddMenu(ContextMenu menu)
        {
            menu.Popup += new EventHandler(this.OnContextMenuPopup);
        }

        public void AddMenu(MxContextMenu menu, Type commandGroup, int menuID)
        {
            if (this._contextMenus == null)
            {
                this._contextMenus = new HybridDictionary();
            }
            IDictionary dictionary = (IDictionary) this._contextMenus[commandGroup];
            if (dictionary == null)
            {
                dictionary = new HybridDictionary();
                this._contextMenus[commandGroup] = dictionary;
            }
            dictionary[menuID] = menu;
            menu.Popup += new EventHandler(this.OnContextMenuPopup);
            menu.Close += new EventHandler(this.OnContextMenuClose);
        }

        public void AddStatusBarPanel(EditorStatusBarPanel panel, Type commandGroup, int commandID)
        {
            EditorStatusBarPanelCommand command = new EditorStatusBarPanelCommand(commandGroup, commandID, panel);
            panel.Command = command;
            this._statusBarPanels.Add(panel);
            this.AddCommand(command);
        }

        public void AddStatusBarPanel(MxStatusBarPanel panel, Type commandGroup, int commandID)
        {
            StatusBarPanelCommand command = new StatusBarPanelCommand(commandGroup, commandID, panel);
            panel.Command = command;
            this._statusBarPanels.Add(panel);
            this.AddCommand(command);
        }

        public void AddToolBar(MxToolBar toolBar)
        {
            toolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.OnToolBarButtonClicked);
            toolBar.ComboBoxCreated += new ToolBarComboBoxButtonEventHandler(this.OnToolBarComboBoxCreated);
            this._toolBars.Add(toolBar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.Idle -= new EventHandler(this.OnApplicationIdle);
                this._serviceProvider = null;
                this._ownerCommandHandler = null;
            }
            base.Dispose(disposing);
        }

        private Command FindCommand(Type commandGroup, int commandID)
        {
            Command command = null;
            Hashtable hashtable = (Hashtable) this._commands[commandGroup];
            if (hashtable != null)
            {
                command = (Command) hashtable[commandID];
            }
            return command;
        }

        void ICommandManager.AddGlobalCommandHandler(ICommandHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException();
            }
            if (this._globalCommandHandlers == null)
            {
                this._globalCommandHandlers = new ArrayList(0x10);
            }
            this._globalCommandHandlers.Add(handler);
            this.UpdateCommandHandlerList();
        }

        bool ICommandManager.InvokeCommand(Type commandGroup, int commandID)
        {
            if (commandGroup == null)
            {
                throw new ArgumentNullException("commandGroup");
            }
            if (commandID <= 0)
            {
                throw new ArgumentOutOfRangeException("commandID");
            }
            Command command = this.FindCommand(commandGroup, commandID);
            if (command == null)
            {
                throw new ArgumentException();
            }
            command.UpdateCommand(this._commandHandlerList);
            return command.InvokeCommand();
        }

        void ICommandManager.RemoveGlobalCommandHandler(ICommandHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException();
            }
            if (this._globalCommandHandlers != null)
            {
                this._globalCommandHandlers.Remove(handler);
                this.UpdateCommandHandlerList();
            }
        }

        void ICommandManager.ResumeCommandUpdate()
        {
            this.ResumeCommandUpdate();
        }

        void ICommandManager.ShowContextMenu(Type commandGroup, int menuID, ICommandHandlerWithContext commandHandler, object context, Control referenceControl, Point location)
        {
            if ((commandGroup == null) || (this._contextMenus == null))
            {
                throw new ArgumentException("commandGroup");
            }
            IDictionary dictionary = (IDictionary) this._contextMenus[commandGroup];
            if (dictionary == null)
            {
                throw new ArgumentException("commandGroup");
            }
            MxContextMenu menu = (MxContextMenu) dictionary[menuID];
            if (menu == null)
            {
                throw new ArgumentException("menuID");
            }
            this._contextCommandHandler = commandHandler;
            this._contextObject = context;
            menu.Show(referenceControl, location.X, location.Y);
        }

        void ICommandManager.SuspendCommandUpdate()
        {
            this.SuspendCommandUpdate();
        }

        void ICommandManager.UpdateActiveUICommandHandler(ICommandHandler activeUICommandHandler, ICommandHandler activeUIContextCommandHandler)
        {
            this._activeUICommandHandler = activeUICommandHandler;
            this._activeUIContextCommandHandler = activeUIContextCommandHandler;
            this.UpdateCommandHandlerList();
        }

        void ICommandManager.UpdateCommands(bool immediate)
        {
            this._commandUpdatePending = true;
            if (immediate)
            {
                this.OnApplicationIdle(null, EventArgs.Empty);
            }
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            if (this._commandUpdateSuspendCount == 0)
            {
                if (this._commandUpdatePending)
                {
                    this._commandUpdatePending = false;
                    this.RunUpdateLoop();
                }
                foreach (MxStatusBarPanel panel in this._statusBarPanels)
                {
                    Command command = panel.Command;
                    command.Enabled = true;
                    try
                    {
                        command.UpdateCommand(this._commandHandlerList);
                        command.UpdateCommandUI();
                        continue;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private void OnContextMenuClose(object sender, EventArgs e)
        {
            this._contextCommandHandler = null;
            this._contextObject = null;
        }

        private void OnContextMenuPopup(object sender, EventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            MxMenuItem item = new MxMenuItem("");
            menu.MenuItems.Add(item);
            menu.MenuItems.Remove(item);
            this.RunUpdateLoop(menu);
        }

        private void OnMenuItemClick(object sender, EventArgs e)
        {
            MxMenuItem item = sender as MxMenuItem;
            try
            {
                bool flag = item.Command.InvokeCommand();
            }
            catch (Exception)
            {
            }
        }

        private void OnMenuItemPopup(object sender, EventArgs e)
        {
            MxMenuItem menu = sender as MxMenuItem;
            MxMenuItem item = new MxMenuItem("");
            //menu.MenuItems.Add(item);
            //menu.MenuItems.Remove(item);
            this.RunUpdateLoop(menu);
        }

        private void OnMenuItemSelect(object sender, EventArgs e)
        {
            if (this._commandHelpTextChangedHandler != null)
            {
                this._commandHelpText = ((MxMenuItem) sender).HelpText;
                this._commandHelpTextChangedHandler(this, EventArgs.Empty);
            }
        }

        private void OnToolBarButtonClicked(object sender, ToolBarButtonClickEventArgs e)
        {
            MxToolBarButton button = e.Button as MxToolBarButton;
            try
            {
                button.Command.InvokeCommand();
            }
            catch (Exception)
            {
            }
        }

        private void OnToolBarComboBoxCloseDropDown(object sender, EventArgs e)
        {
            this.ResumeCommandUpdate();
        }

        private void OnToolBarComboBoxCreated(object sender, ToolBarComboBoxButtonEventArgs e)
        {
            ToolBarComboBox comboBox = e.Button.ComboBox;
            comboBox.GotFocus += new EventHandler(this.OnToolBarComboBoxGotFocus);
            comboBox.LostFocus += new EventHandler(this.OnToolBarComboBoxLostFocus);
            comboBox.DropDown += new EventHandler(this.OnToolBarComboBoxDropDown);
            comboBox.CloseDropDown += new EventHandler(this.OnToolBarComboBoxCloseDropDown);
            comboBox.SelectedIndexChanged += new EventHandler(this.OnToolBarComboBoxSelectedIndexChanged);
            if (comboBox.DropDownStyle == ComboBoxStyle.DropDown)
            {
                comboBox.TextBoxKeyPress += new KeyPressEventHandler(this.OnToolBarComboBoxKeyPress);
            }
            comboBox.Command = (ToolBarComboBoxCommand) e.Button.Command;
            ((ICommandManager) this).UpdateCommands(false);
        }

        private void OnToolBarComboBoxDropDown(object sender, EventArgs e)
        {
            ToolBarComboBox box = (ToolBarComboBox) sender;
            ToolBarComboBoxCommand fillCommand = box.Command.FillCommand;
            if (fillCommand != null)
            {
                fillCommand.UpdateCommand(this._commandHandlerList);
                fillCommand.UpdateCommandUI();
            }
            this.SuspendCommandUpdate();
        }

        private void OnToolBarComboBoxGotFocus(object sender, EventArgs e)
        {
        }

        private void OnToolBarComboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                ToolBarComboBox box = sender as ToolBarComboBox;
                ToolBarComboBoxCommand command = box.Command;
                command.Text = box.Text;
                command.EnterKeyPressed = true;
                e.Handled = true;
                try
                {
                    command.InvokeCommand();
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnToolBarComboBoxLostFocus(object sender, EventArgs e)
        {
        }

        private void OnToolBarComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            ToolBarComboBox box = sender as ToolBarComboBox;
            ToolBarComboBoxCommand command = box.Command;
            command.Text = box.SelectedItem as string;
            command.EnterKeyPressed = false;
            try
            {
                command.InvokeCommand();
            }
            catch (Exception)
            {
            }
        }

        private void ResumeCommandUpdate()
        {
            if (this._firstResume)
            {
                this._firstResume = false;
                Application.Idle += new EventHandler(this.OnApplicationIdle);
            }
            if (this._commandUpdateSuspendCount > 0)
            {
                this._commandUpdateSuspendCount--;
            }
        }

        private void RunUpdateLoop()
        {
            foreach (MxToolBarButton button in this._toolBarButtons)
            {
                Command command = button.Command;
                command.Enabled = true;
                command.Checked = false;
                try
                {
                    command.UpdateCommand(this._commandHandlerList);
                    command.UpdateCommandUI();
                    continue;
                }
                catch
                {
                    continue;
                }
            }
        }

        private void RunUpdateLoop(MxMenuItem menu)
        {
            this.RunUpdateLoop(menu.MenuItems, this._contextCommandHandler != null);
        }

        private void RunUpdateLoop(ContextMenu menu)
        {
            this.RunUpdateLoop(menu.MenuItems, this._contextCommandHandler != null);
        }

        private void RunUpdateLoop(Menu.MenuItemCollection menuItems, bool useContextHandler)
        {
            if (!useContextHandler)
            {
                foreach (MxMenuItem item in menuItems)
                {
                    Command command = item.Command;
                    if (command != null)
                    {
                        command.Enabled = true;
                        command.Checked = false;
                        try
                        {
                            item.Command.UpdateCommand(this._commandHandlerList);
                            item.Command.UpdateCommandUI();
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            else
            {
                foreach (MxMenuItem item2 in menuItems)
                {
                    Command command2 = item2.Command;
                    if (command2 != null)
                    {
                        command2.Enabled = true;
                        command2.Checked = false;
                        try
                        {
                            item2.Command.UpdateCommand(this._commandHandlerList, this._contextCommandHandler, this._contextObject);
                            item2.Command.UpdateCommandUI();
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        internal void SetCommandHelpTextChangedHandler(EventHandler handler)
        {
            this._commandHelpTextChangedHandler = handler;
        }

        private void SuspendCommandUpdate()
        {
            this._commandUpdateSuspendCount++;
        }

        private void UpdateCommandHandlerList()
        {
            int num = 1;
            int count = 0;
            if (this._globalCommandHandlers != null)
            {
                count = this._globalCommandHandlers.Count;
                num += count;
            }
            if (this._activeUIContextCommandHandler != null)
            {
                num++;
            }
            if (this._activeUICommandHandler != null)
            {
                num++;
            }
            if ((this._commandHandlerList == null) || (this._commandHandlerList.Length != num))
            {
                this._commandHandlerList = new ICommandHandler[num];
            }
            int index = 0;
            if (this._activeUICommandHandler != null)
            {
                this._commandHandlerList[index] = this._activeUICommandHandler;
                index++;
            }
            if (this._activeUIContextCommandHandler != null)
            {
                this._commandHandlerList[index] = this._activeUIContextCommandHandler;
                index++;
            }
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    this._commandHandlerList[index] = (ICommandHandler) this._globalCommandHandlers[i];
                    index++;
                }
            }
            this._commandHandlerList[index] = this._ownerCommandHandler;
            ((ICommandManager) this).UpdateCommands(false);
        }

        internal string CommandHelpText
        {
            get
            {
                return this._commandHelpText;
            }
        }
    }
}


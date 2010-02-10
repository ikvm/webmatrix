namespace Microsoft.Matrix.WebIDE
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.ClassView.UserInterface;
    //using Microsoft.Matrix.Packages.Community.UserInterface;
    //using Microsoft.Matrix.Packages.DBAdmin.UserInterface;
    //using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class ApplicationWindow : MxApplicationWindow
    {
        //private ClassViewToolWindow _classViewTool;
        //private CommunityToolWindow _communityTool;
        //private DataToolWindow _dataTool;
        private DockingContainer leftContainer;
        //private OpenDocumentsToolWindow _openDocsTool;
        private PropertyBrowserToolWindow _propBrowserTool;
        private ToolboxToolWindow _toolboxTool;
        private MdiWindowManager _windowManager;
        private WorkspaceToolWindow _workspaceTool;
        private OpenDocumentsTabControl _openDocsTabControl;

        public ApplicationWindow(System.IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeUserInterface();
            this.InitializeCommandBar();
            ((IServiceContainer) this.GetService(typeof(IServiceContainer))).AddService(typeof(IWindowManager), this._windowManager);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    service.RemoveService(typeof(IWindowManager));
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeCommandBar()
        {
            TempCommandUI dui = new TempCommandUI();
            dui.mainForm = this;
            dui.cm = base.CommandManager;
            dui.mainToolBar = (MxToolBar) base.CommandBar.Controls[0];
            dui.helpToolBar = (MxToolBar) base.CommandBar.Controls[1];
            //dui.documentToolBar = (MxToolBar) base.CommandBar.Controls[2];
            dui.mainStatusBar = base.StatusBar;
            dui.menuBar = base.Menu;
            dui.commandImages = dui.mainToolBar.ImageList.Images;
            dui.InitializeCommandUI();
        }

        private void InitializeUserInterface()
        {
            this._workspaceTool = new WorkspaceToolWindow(base.ServiceProvider);
            //this._openDocsTool = new OpenDocumentsToolWindow(base.ServiceProvider);
            this._toolboxTool = new ToolboxToolWindow(base.ServiceProvider);
            this._propBrowserTool = new PropertyBrowserToolWindow(base.ServiceProvider);
            this._openDocsTabControl = new OpenDocumentsTabControl(base.ServiceProvider);
            _openDocsTabControl.Dock = DockStyle.Top;
            _openDocsTabControl.Mode = TabControlMode.TextOnly;

            //this._dataTool = new DataToolWindow(base.ServiceProvider);
            //this._classViewTool = new ClassViewToolWindow(base.ServiceProvider);
            //this._communityTool = new CommunityToolWindow(base.ServiceProvider);
            ImageList list = new ImageList();
            CommandBar commandBar = new CommandBar();
            MxToolBar mainToolBar = new MxToolBar();
            MxToolBar helpToolBar = new MxToolBar();
            //MxToolBar documentToolBar = new MxToolBar();
            MxStatusBar statusBar = new MxStatusBar();
            this.leftContainer = new DockingContainer();
            DockingContainer rightContainer = new DockingContainer();
            Splitter splitter = new Splitter();
            Splitter splitter2 = new Splitter();
            MainMenu mainMenu = new MainMenu();
            MxStatusBarPanel panel = new MxStatusBarPanel();
            ProgressStatusBarPanel panel2 = new ProgressStatusBarPanel();
            EditorStatusBarPanel panel3 = new EditorStatusBarPanel();
            MxStatusBarPanel panel4 = new MxStatusBarPanel();
            MxStatusBarPanel panel5 = new MxStatusBarPanel();
            commandBar.SuspendLayout();
            base.SuspendLayout();
            ((ISupportInitialize) statusBar).BeginInit();
            list.ImageSize = new Size(0x10, 0x10);
            list.TransparentColor = Color.Lime;
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.AddStrip(new Bitmap(typeof(ApplicationWindow), "Commands.bmp"));
            mainToolBar.Appearance = ToolBarAppearance.Flat;
            mainToolBar.Divider = false;
            mainToolBar.DropDownArrows = true;
            mainToolBar.ShowToolTips = true;
            mainToolBar.TabIndex = 1;
            bool b = mainToolBar.TabStop = false;
            mainToolBar.TextAlign = ToolBarTextAlign.Right;
            mainToolBar.Wrappable = false;
            mainToolBar.ImageList = list;
            helpToolBar.Appearance = ToolBarAppearance.Flat;
            helpToolBar.Divider = false;
            helpToolBar.DropDownArrows = true;
            helpToolBar.ShowToolTips = true;
            helpToolBar.TabIndex = 1;
            helpToolBar.TabStop = false;
            helpToolBar.TextAlign = ToolBarTextAlign.Right;
            helpToolBar.Wrappable = false;
            helpToolBar.ImageList = list;
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            helpToolBar.Width = (((int) graphics.MeasureString(new string('_', 30), this.Font).Width) + 0x18) + 6;
            graphics.Dispose();
            //documentToolBar.Appearance = ToolBarAppearance.Flat;
            //documentToolBar.Divider = false;
            //documentToolBar.DropDownArrows = true;
            //documentToolBar.ShowToolTips = true;
            //documentToolBar.TabIndex = 0;
            //documentToolBar.TabStop = false;
            //documentToolBar.TextAlign = ToolBarTextAlign.Right;
            //documentToolBar.Wrappable = false;
            //documentToolBar.ImageList = list;
            commandBar.Dock = DockStyle.Top;
            commandBar.SendToBack();
            commandBar.MenuBar = mainMenu;
            commandBar.TabIndex = 1;
            commandBar.TabStop = false;
            commandBar.Controls.Add(mainToolBar);
            commandBar.Controls.Add(helpToolBar);
            //commandBar.Controls.Add(documentToolBar);
            commandBar.TopRightToolBar = helpToolBar;
            this.leftContainer.AssociatedSplitter = splitter;
            this.leftContainer.Dock = DockStyle.Left;
            this.leftContainer.Size = new Size(0x7d, 0x1fb);
            this.leftContainer.TabIndex = 3;
            this.leftContainer.TabStop = false;
            splitter.Size = new Size(4, 4);
            splitter.TabIndex = 4;
            splitter.TabStop = false;
            rightContainer.AssociatedSplitter = splitter2;
            rightContainer.Dock = DockStyle.Right;
            rightContainer.Size = new Size(230, 0x1fb);
            rightContainer.TabIndex = 5;
            rightContainer.TabStop = false;
            splitter2.Dock = DockStyle.Right;
            splitter2.Size = new Size(4, 4);
            splitter2.TabIndex = 6;
            splitter2.TabStop = false;
            panel.AutoSize = StatusBarPanelAutoSize.Spring;
            panel2.MinWidth = 0x80;
            panel2.Width = 0x80;
            panel3.Alignment = HorizontalAlignment.Center;
            panel3.Width = 0x80;
            panel3.MinWidth = 0x80;
            panel4.Alignment = HorizontalAlignment.Center;
            panel4.Width = 0x23;
            panel5.Alignment = HorizontalAlignment.Center;
            panel5.Width = 0x23;
            statusBar.Size = new Size(0x318, 20);
            statusBar.TabIndex = 2;
            statusBar.TabStop = false;
            statusBar.ShowPanels = true;
            statusBar.Panels.AddRange(new StatusBarPanel[] { panel, panel2, panel3, panel4, panel5 });
            this.AutoScaleBaseSize = new Size(5, 13);
            base.Icon = new Icon(typeof(ApplicationWindow), "WebIDE.ico");
            base.IsMdiContainer = true;
            base.Menu = mainMenu;
            base.StatusBar = statusBar;
            base.CommandBar = commandBar;
            base.MinimumSize = new Size(640, 480);
            this.Text = "Microsoft ASP.NET Web Matrix";
            base.Controls.AddRange(new Control[] { splitter2, rightContainer, splitter, this.leftContainer, statusBar, this._openDocsTabControl, commandBar});
            this._windowManager = new MdiWindowManager(this);
            this._windowManager.EnableDocking(DockStyle.Left, this.leftContainer);
            this._windowManager.EnableDocking(DockStyle.Right, rightContainer);
            IWindowManager manager = this._windowManager;
            manager.AddToolWindow(this._toolboxTool, DockStyle.Left);
            manager.AddToolWindow(this._propBrowserTool, DockStyle.Right);
            //manager.AddToolWindow(this._classViewTool, DockStyle.Right, 0);
            //manager.AddToolWindow(this._communityTool, DockStyle.Right, 0);
            manager.AddToolWindow(this._workspaceTool, DockStyle.Right, -1);
            //manager.AddToolWindow(this._dataTool, DockStyle.Right, 1);
            //manager.AddToolWindow(this._openDocsTool, DockStyle.Right, 1);
            ((ISupportInitialize) statusBar).EndInit();
            commandBar.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override bool IsSupportedDocumentType(string extension)
        {
            return true;
        }

        private void OnCommandViewToolbox()
        {
            base.SuspendLayout();
            try
            {
                if (this.leftContainer.Visible)
                {
                    this.leftContainer.AssociatedSplitter.Visible = false;
                    this.leftContainer.Visible = false;
                }
                else
                {
                    this.leftContainer.AssociatedSplitter.Visible = true;
                    this.leftContainer.Visible = true;
                }
            }
            finally
            {
                base.ResumeLayout(true);
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            SplashScreen.Current.Hide(this);
            base.OnInitialActivated(e);
        }

        protected override bool PerformStartupAction()
        {
            ICommandManager service = (ICommandManager) this.GetService(typeof(ICommandManager));
            service.UpdateCommands(true);
            IApplicationIdentity identity = (IApplicationIdentity) base.ServiceProvider.GetService(typeof(IApplicationIdentity));
            CommandLine commandLine = identity.CommandLine;
            if (commandLine.Arguments.Length > 0)
            {
                base.OpenDocuments(commandLine.Arguments);
                return true;
            }
            string setting = identity.GetSetting("startup");
            if (setting == null)
            {
                setting = string.Empty;
            }
            int commandID = -1;
            if ((setting.Length == 0) || (setting == Enum.GetName(typeof(StartupAction), StartupAction.NewFile)))
            {
                commandID = 1;
            }
            else if (setting == Enum.GetName(typeof(StartupAction), StartupAction.OpenFile))
            {
                commandID = 2;
            }
            if (commandID != -1)
            {
                service.InvokeCommand(typeof(GlobalCommands), commandID);
                return true;
            }
            return false;
        }

        protected override bool HandleCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            bool commandHandled = false;
            bool needUpdateClient = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                int commandID = command.CommandID;
                if (commandID != 5)
                {
                    switch (commandID)
                    {
                        case 400:
                            base.LayoutMdi(MdiLayout.Cascade);
                            commandHandled = true;
                            goto Label_00B5;

                        case 0x191:
                            base.LayoutMdi(MdiLayout.TileHorizontal);
                            commandHandled = true;
                            goto Label_00B5;

                        case 0x192:
                            base.LayoutMdi(MdiLayout.TileVertical);
                            commandHandled = true;
                            goto Label_00B5;

                        case 0x193:
                        {
                            Form[] mdiChildren = base.MdiChildren;
                            if ((mdiChildren != null) && (mdiChildren.Length != 0))
                            {
                                for (int i = mdiChildren.Length - 1; i >= 0; i--)
                                {
                                    mdiChildren[i].Close();
                                }
                            }
                            commandHandled = true;
                            goto Label_00B5;
                        }
                        case 0xd6:
                            this.OnCommandViewToolbox();
                            commandHandled = true;
                            needUpdateClient = true;
                            goto Label_00B5;
                    }
                }
                else
                {
                    base.ActiveMdiChild.Close();
                    commandHandled = true;
                    needUpdateClient = true;
                }
            }
        Label_00B5:
            if (!commandHandled)
            {
                return base.HandleCommand(command);
            }
            if (needUpdateClient)
            {
                ((ICommandManager) this.GetService(typeof(ICommandManager))).UpdateCommands(false);
            }
            return commandHandled;
        }

        protected override bool UpdateCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 400:
                    case 0x191:
                    case 0x192:
                    case 0x193:
                    case 5:
                        command.Enabled = base.MdiChildren.Length != 0;
                        flag = true;
                        break;

                    case 0xd6:
                        command.Checked = this.leftContainer.Visible;
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }

        private sealed class TempCommandUI
        {
            public CommandManager cm;
            //public MxToolBar documentToolBar;
            public ImageList.ImageCollection commandImages;
            public MxToolBar helpToolBar;
            public ApplicationWindow mainForm;
            public MxStatusBar mainStatusBar;
            public MxToolBar mainToolBar;
            public MainMenu menuBar;
            private Hashtable globalCommandTable;
            //private Hashtable webCommandTable;
            private Hashtable contextCommandTable;
            //private Hashtable htmlDesignerContextMenuTable;
            //private Hashtable htmlDesignerTableDesignerContextMenuTable;
            private Hashtable toolBarButtonTable1;
            private Hashtable toolBarButtonTable2;

            #region ImageIndex constants members
            private const int DeleteTableColumnImageIndex = 0x47;
            private const int DeleteTableRowImageIndex = 70;
            private const int EditCopyImageIndex = 7;
            private const int EditCutImageIndex = 6;
            private const int EditEditTagImageIndex = 0x36;
            private const int EditFindImageIndex = 40;
            private const int EditPasteImageIndex = 8;
            private const int EditRedoImageIndex = 5;
            private const int EditReplaceImageIndex = 0x22;
            private const int EditUndoImageIndex = 4;
            private const int FileNewImageIndex = 1;
            private const int FileNewProjectIndex = 0x37;
            private const int FileOpenImageIndex = 2;
            private const int FilePrintImageIndex = 0x1f;
            private const int FilePrintPreviewImageIndex = 30;
            private const int FileSaveImageIndex = 3;
            private const int FmtBackColorImageIndex = 0x21;
            private const int FmtBoldImageIndex = 12;
            private const int FmtCenterImageIndex = 0x12;
            private const int FmtForeColorImageIndex = 0x20;
            private const int FmtIndentImageIndex = 0x17;
            private const int FmtItalicImageIndex = 13;
            private const int FmtLeftAlignImageIndex = 0x11;
            private const int FmtOrderedListImageIndex = 20;
            private const int FmtRightAlignImageIndex = 0x13;
            private const int FmtStrikeImageIndex = 0x18;
            private const int FmtSubscriptImageIndex = 0x10;
            private const int FmtSuperscriptImageIndex = 15;
            private const int FmtUnderlineImageIndex = 14;
            private const int FmtUnindentImageIndex = 0x16;
            private const int FmtUnorderedListImageIndex = 0x15;
            private const int HelpAppInfoImageIndex = 0x38;
            private const int HelpHelpTopicsImageIndex = 0x1b;
            private const int HelpHelpUrl = 0x1c;
            private const int HelpSendFeedbackImageIndex = 0x39;
            private const int HtmlAbsolutePositionImageIndex = 0x19;
            private const int HtmlAlignBottomImageIndex = 0x30;
            private const int HtmlAlignCenterImageIndex = 0x2c;
            private const int HtmlAlignLeftImageIndex = 0x2b;
            private const int HtmlAlignMiddleImageIndex = 0x2f;
            private const int HtmlAlignRightImageIndex = 0x2d;
            private const int HtmlAlignTopImageIndex = 0x2e;
            private const int HtmlInsHyperLinkImageIndex = 0x1a;
            private const int HtmlLockImageIndex = 0x27;
            private const int HtmlSendBackwardImageIndex = 0x2a;
            private const int HtmlSendForwardImageIndex = 0x29;
            private const int HtmlSizeBothImageIndex = 0x33;
            private const int HtmlSizeHeightImageIndex = 50;
            private const int HtmlSizeWidthImageIndex = 0x31;
            private const int HtmlTableInsertImageIndex = 0x34;
            private const int InsertTableColumnImageIndex = 0x45;
            private const int InsertTableRowImageIndex = 0x44;
            private const int MergeCellDownImageIndex = 0x4b;
            private const int MergeCellLeftImageIndex = 0x48;
            private const int MergeCellRightImageIndex = 0x49;
            private const int MergeCellUpImageIndex = 0x4a;
            private const int PlaceHolderImageIndex = 0;
            private const int SplitCellHorizontalImageIndex = 0x4c;
            private const int SplitCellVerticalImageIndex = 0x4d;
            private const int ToolsOrganizeAddInsImageIndex = 0x35;
            private const int ToolsPreferencesImageIndex = 0x3f;
            private const int ViewBordersImageIndex = 0x25;
            private const int ViewCommandWindowImageIndex = 0x1d;
            private const int ViewGlyphsImageIndex = 0x26;
            private const int ViewGridImageIndex = 0x23;
            private const int ViewOpenDocumentsImageIndex = 0x41;
            private const int ViewPropertyBrowserImageIndex = 0x42;
            private const int ViewSnapGridImageIndex = 0x24;
            private const int ViewStartImageIndex = 0x3e;
            private const int ViewToolboxImageIndex = 0x43;
            private const int ViewWorkspaceImageIndex = 0x40;
            private const int WindowCascadeImageIndex = 9;
            private const int WindowTileHorizImageIndex = 10;
            private const int WindowTileVertImageIndex = 11;
            private const int WorkspaceAddFolderImageIndex = 0x3d;
            private const int WorkspaceDeleteImageIndex = 0x3b;
            private const int WorkspaceOpenImageIndex = 0x3a;
            private const int WorkspaceRefreshImageIndex = 60;
            #endregion

            private void AddMenuItem(Hashtable table, System.Type commandGroup, int commandID, string text, string helpText, Shortcut shortcut, int imageIndex)
            {
                this.AddMenuItem(table, commandGroup, commandID, text, helpText, shortcut, imageIndex, false);
            }

            private void AddMenuItem(Hashtable table, System.Type commandGroup, int commandID, string text, string helpText, Shortcut shortcut, int imageIndex, bool isContextMenu)
            {
                Image glyph = null;
                if (imageIndex != -1)
                {
                    glyph = this.commandImages[imageIndex];
                }
                MxMenuItem menuItem = new MxMenuItem(text, helpText, shortcut, glyph);
                table[commandID] = menuItem;
                this.cm.AddCommand(menuItem, commandGroup, commandID, isContextMenu);
            }

            private void AddToolBarButton(Hashtable table, System.Type commandGroup, int commandID, string helpText, int imageIndex)
            {
                MxToolBarButton button = new MxToolBarButton();
                button.ImageIndex = imageIndex;
                button.ToolTipText = helpText;
                button.Enabled = false;
                table[commandID] = button;
                this.cm.AddCommand(button, commandGroup, commandID);
            }

            private void AddToolBarComboBoxButton(Hashtable table, System.Type commandGroup, int commandID, string helpText, int width, ComboBoxStyle style, object[] items)
            {
                this.AddToolBarComboBoxButton(table, commandGroup, commandID, 0, helpText, width, style, items, null);
            }

            private void AddToolBarComboBoxButton(Hashtable table, System.Type commandGroup, int commandID, int fillCommandID, string helpText, int width, ComboBoxStyle style, object[] items, string initialText)
            {
                ComboBoxToolBarButton button = new ComboBoxToolBarButton();
                button.DropDownStyle = style;
                button.ComboBoxItems = items;
                button.ToolTipText = helpText;
                button.Text = new string('_', width);
                button.InitialText = initialText;
                button.ImageIndex = 0;
                table[commandID] = button;
                this.cm.AddCommand(button, commandGroup, commandID, fillCommandID);
            }

            private void AddToolBarFontComboBoxButton(Hashtable table, System.Type commandGroup, int commandID, string helpText, int width)
            {
                FontComboBoxToolBarButton button = new FontComboBoxToolBarButton();
                button.ToolTipText = helpText;
                button.Text = new string('_', width);
                button.DropDownWidth = 220;
                button.ImageIndex = 0;
                table[commandID] = button;
                this.cm.AddCommand((ComboBoxToolBarButton) button, commandGroup, commandID);
            }

            private void CreateMenuItems()
            {
                int num;
                System.Type globalCommandGroup = typeof(GlobalCommands);
                //System.Type webCommandGroup = typeof(WebCommands);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 1, "&New File...", "Create a new file", Shortcut.CtrlN, 1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 2, "&Open Files...", "Open existing files", Shortcut.CtrlO, 2);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 3, "&Save File", "Save the current file", Shortcut.CtrlS, 3);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 4, "Save File &As...", "Save the current file to a new location", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 5, "&Close", "Close the current file", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 10, "New Pro&ject...", "Create a new project", Shortcut.None, 0x37);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 6, "&Print...", "Print the current file", Shortcut.CtrlP, 0x1f);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 7, "P&rint Preview...", "Print-preview the current file", Shortcut.None, 30);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 8, "Pr&int Settings...", "Set up printer defaults and settings...", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 9, "E&xit", "Close the application", Shortcut.None, -1);
                for (num = 0; num < 10; num++)
                {
                    this.AddMenuItem(this.globalCommandTable, globalCommandGroup, num + 20, "", "Open this file", Shortcut.None, -1);
                }
                for (num = 0; num < 5; num++)
                {
                    this.AddMenuItem(this.globalCommandTable, globalCommandGroup, num + 40, "", "Open this project", Shortcut.None, -1);
                }
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 100, "&Undo", "Undo the last change", Shortcut.CtrlZ, 4);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x65, "&Redo", "Reverse the last undo", Shortcut.CtrlY, 5);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x66, "Cu&t", "Remove the selection and place it on the clipboard", Shortcut.CtrlX, 6);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x67, "&Copy", "Copy the selection to the clipboard", Shortcut.CtrlC, 7);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x68, "&Paste", "Insert the contents of the clipboard", Shortcut.CtrlV, 8);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x6a, "Select &All", "Select everything in the file", Shortcut.CtrlA, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x6b, "&Go To...", "Go to the specific line in the current file", Shortcut.CtrlG, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x6c, "&Find...", "Search for the specified text", Shortcut.CtrlF, 40);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 180, "Find &Next", "Search for the next occurence", Shortcut.F3, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x6d, "R&eplace...", "Search for and replace the specified text", Shortcut.CtrlH, 0x22);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 120, "A&dd Snippet", "Copy the selected text as a snippet", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 250, "For&mat Document", "Format the entire document", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 260, "Ed&it Tag", "Edit the source of the selected tag", Shortcut.CtrlT, 0x36);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 240, "Edit Temp&lates...", "Edit the templates of the selected control", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x8d, "C&omment Selection", "Comment the selected text", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x8e, "U&ncomment Selection", "Uncomment the selected text", Shortcut.None, -1);
                for (num = 0; num < 4; num++)
                {
                    this.AddMenuItem(this.globalCommandTable, globalCommandGroup, num + 200, "", "Switch to this editor view", Shortcut.None, -1);
                }
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 210, "&Start...", "Run the current file", Shortcut.F5, 0x3e);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0xd6, "&Toolbox", "Toggle the Toolbox Window", Shortcut.F2, 0x43);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 1, "&Glyphs", "Toggle glyphs in the current file", Shortcut.None, 0x26);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 2, "&Borders", "Toggle design-time borders in the current file", Shortcut.None, 0x25);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 3, "G&rid", "Toggle the design-time grid in the current file", Shortcut.None, 0x23);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 4, "S&nap to Grid", "Toggle the snap to grid behavior in the current file", Shortcut.None, 0x24);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 100, "&Bold", "Make the selected text bold", Shortcut.CtrlB, 12);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x65, "&Italic", "Make the selected text italic", Shortcut.CtrlI, 13);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x66, "&Underline", "Underline the selected text", Shortcut.CtrlU, 14);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x67, "&Superscript", "Convert the selected text into superscript text", Shortcut.None, 15);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x68, "Subscri&pt", "Convert the selected text into subscript text", Shortcut.None, 0x10);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x69, "S&trike", "Strikeout the selected text", Shortcut.None, 0x18);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x6a, "&Foreground Color...", "Select the foreground color", Shortcut.None, 0x20);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x6b, "B&ackground Color...", "Select the backgbround color", Shortcut.None, 0x21);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x6c, "Align &Left", "Align the selected text to the left", Shortcut.None, 0x11);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 110, "Align &Center", "Center the selected text", Shortcut.None, 0x12);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x6d, "Align &Right", "Align the selected text to the right", Shortcut.None, 0x13);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x6f, "&Ordered List", "Convert the selected text into ordered numbered list", Shortcut.None, 20);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x70, "U&nordered List", "Convert the selected text into bulleted list", Shortcut.None, 0x15);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x71, "In&dent", "Indent the selected text in", Shortcut.None, 0x17);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x72, "Unind&ent", "Unindent the selected text out", Shortcut.None, 0x16);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x7b, "St&yle...", "Edit the style and formatting attributes of the selection", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 130, "&Normal", "Apply the normal format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x83, "&Formatted", "Apply the formatted format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x84, "Heading &1", "Apply the heading 1 format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x85, "Heading &2", "Apply the heading 2 format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x86, "Heading &3", "Apply the heading 3 format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x87, "Heading &4", "Apply the heading 4 format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x88, "Heading &5", "Apply the heading 5 format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x89, "Heading &6", "Apply the heading 6 format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x8a, "&Paragraph", "Apply the paragraph format to the selected text.", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 140, "&Absolute Position", "Toggle absolute positioning of the selected elements", Shortcut.None, 0x19);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x8f, "Align &Left Edges", "Align the left edges of the selected elements", Shortcut.None, 0x2b);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x90, "Align &Center", "Align the centers of the selected elements", Shortcut.None, 0x2c);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x91, "Align &Right Edges", "Align the right edges of the selected elements", Shortcut.None, 0x2d);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x92, "Align &Top Edges", "Align the top edges of the selected elements", Shortcut.None, 0x2e);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x93, "Align &Middle", "Align the middle of all the selected elements", Shortcut.None, 0x2f);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x94, "Align &Bottom Edges", "Align the bottom edges of the selected elements", Shortcut.None, 0x30);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x95, "Make Same &Width", "Make all the selected elements the same width", Shortcut.None, 0x31);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 150, "Make Same &Height", "Make all the selected elements the same height", Shortcut.None, 50);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x97, "Make Same &Size", "Make all the selected elements elements the same size", Shortcut.None, 0x33);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x8d, "Send &Forward", "Bring the selected element forward in z-order", Shortcut.None, 0x29);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x8e, "Send Bac&kward", "Send the selected element backward in z-order", Shortcut.None, 0x2a);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0x98, "L&ocked", "Lock or unlock the position of the selected element", Shortcut.None, 0x27);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 200, "Insert &HyperLink...", "Insert an hyperLink into the current file", Shortcut.None, 0x1a);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xc9, "&Remove HyperLink", "Remove the selected hyperLink", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 220, "Insert &Table...", "Insert a table into the current file", Shortcut.None, 0x34);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xca, "Wrap in &Span", "Wrap the current selection with <span> tag", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xcb, "Wrap in &Div", "Wrap the current selection with <div> tag", Shortcut.None, -1);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xdd, "&Insert Table Row", "Insert a table row after the currently selected table cell", Shortcut.None, 0x44);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xde, "I&nsert Table Column", "Insert a table column after the currently selected table cell", Shortcut.None, 0x45);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xdf, "&Delete Table Row", "Delete the currently selected table row", Shortcut.None, 70);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xe0, "D&elete Table Column", "Delete the currently selected table column", Shortcut.None, 0x47);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xe1, "&Merge Cell Left", "Merge the currently selected table cell with the cell to the left", Shortcut.None, 0x48);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xe2, "Me&rge Cell Right", "Merge the currently selected table cell with the cell to the right", Shortcut.None, 0x49);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xe3, "Mer&ge Cell Up", "Merge the currently selected table cell with the cell above", Shortcut.None, 0x4a);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xe4, "Merge &Cell Down", "Merge the currently selected table cell with the cell below", Shortcut.None, 0x4b);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xe5, "&Split Cell Horizontally", "Split the currently selected table cell horizontally", Shortcut.None, 0x4c);
                //this.AddMenuItem(this.webCommandTable, webCommandGroup, 0xe7, "S&plit Cell Vertically", "Split the currently selected table cell vertically", Shortcut.None, 0x4d);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x145, "", "Customize the active toolbox section", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x146, "", "Customize the active toolbox section", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x147, "", "Customize the active toolbox section", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x144, "&Preferences...", "Change preferences and options", Shortcut.None, 0x3f);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 320, "&Run Add-in...", "Run a particular add-in", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x141, "&Organize Add-ins...", "Organize the add-in list", Shortcut.None, 0x35);
                for (num = 0; num < 10; num++)
                {
                    this.AddMenuItem(this.globalCommandTable, globalCommandGroup, num + 300, "", "Run this add-in", (Shortcut) (0x30070 + num), -1);
                }
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 400, "&Cascade", "Arrange the current windows diagonally starting from the left top", Shortcut.None, 9);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x191, "Tile &Horizontal", "Tile the current windows horizontally", Shortcut.None, 10);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x192, "Tile &Vertical", "Tile the current windows vertically", Shortcut.None, 11);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x193, "Close &All", "Close all open windows", Shortcut.None, -1);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 600, "&Help Topics", "View help topics", Shortcut.F1, 0x1b);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x25e, "Send &Feedback...", "Submit suggestions, bug reports or general feedback", Shortcut.None, 0x39);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x25a, "Application &Information...", "Information about this application and process", Shortcut.None, 0x38);
                this.AddMenuItem(this.globalCommandTable, globalCommandGroup, 0x259, "About Microsoft ASP.NET &Web Matrix...", "About this application", Shortcut.None, -1);
                for (num = 0; num < 10; num++)
                {
                    this.AddMenuItem(this.globalCommandTable, globalCommandGroup, num + 610, "", "Browse this help URL", Shortcut.None, 0x1c);
                }
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x2c7, "Add New File...", "", Shortcut.None, 1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x2c8, "Add New Folder...", "", Shortcut.None, 0x3d, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x2c9, "Delete", "", Shortcut.None, 0x3b, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x2c6, "Open Files", "", Shortcut.None, 0x3a, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x2ca, "Refresh", "", Shortcut.None, 60, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 10, "New Project...", "", Shortcut.None, 0x37, true);
                for (num = 0; num < 8; num++)
                {
                    this.AddMenuItem(this.contextCommandTable, globalCommandGroup, num + 720, "", "", Shortcut.None, -1, true);
                }
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x145, "Customize...", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x146, "Customize...", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x147, "Customize...", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x148, "Rename", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x149, "Remove", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x14b, "Sort by Name", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 330, "Reset Toolbox", "", Shortcut.None, -1, true);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 120, "A&dd Snippet", "Copy the selected text as a snippet", Shortcut.None, -1);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x66, "Cu&t", "Remove the selection and place it on the clipboard", Shortcut.CtrlX, 6);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x67, "&Copy", "Copy the selection to the clipboard", Shortcut.CtrlC, 7);
                this.AddMenuItem(this.contextCommandTable, globalCommandGroup, 0x68, "&Paste", "Insert the contents of the clipboard", Shortcut.CtrlV, 8);
            }

            private ToolBarButton CreateSeparatorButton()
            {
                ToolBarButton button = new MxToolBarButton();
                button.Style = ToolBarButtonStyle.Separator;
                return button;
            }

            private void CreateToolBarButtons()
            {
                System.Type commandGroup = typeof(GlobalCommands);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 1, "New File", 1);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 2, "Open Files", 2);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 3, "Save File", 3);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 10, "New Project", 0x37);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 6, "Print File", 0x1f);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x66, "Cut", 6);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x67, "Copy", 7);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x68, "Paste", 8);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 100, "Undo", 4);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0x65, "Redo", 5);
                this.AddToolBarComboBoxButton(this.toolBarButtonTable1, commandGroup, 110, 0x6f, "Find", 20, ComboBoxStyle.DropDown, null, "Enter a search string");
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 210, "Start", 0x3e);
                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 0xd6, "Toggle Toolbox", 0x43);

                this.AddToolBarButton(this.toolBarButtonTable1, commandGroup, 600, "Help Topics", 0x1b);
                string[] items = new string[] { "Normal", "Formatted", "Heading 1", "Heading 2", "Heading 3", "Heading 4", "Heading 5", "Heading 6", "Paragraph" };
                string[] strArray2 = new string[] { "1", "2", "3", "4", "5", "6", "7" };

            }

            public void InitializeCommandUI()
            {
                this.globalCommandTable = new Hashtable(100);
                this.contextCommandTable = new Hashtable(50);
                this.toolBarButtonTable1 = new Hashtable(0x19);
                this.toolBarButtonTable2 = new Hashtable(0x19);
                this.CreateMenuItems();
                this.CreateToolBarButtons();
                this.InitializeMainToolBar();
                this.InitializeHelpToolBar();
                //this.InitializeDocumentToolBar();
                this.InitializeMenuBar();
                this.InitializeStatusBar();
            }

            private void InitializeHelpToolBar()
            {
                ComboBoxToolBarButton button = new ComboBoxToolBarButton();
                button.DropDownStyle = ComboBoxStyle.DropDown;
                button.InitialText = "Type keywords to search online help";
                button.ToolTipText = "Search for help by keywords";
                button.Text = new string('_', 30);
                button.ImageIndex = 0;
                this.cm.AddCommand(button, typeof(GlobalCommands), 0x25c, 0x25d);
                this.helpToolBar.Buttons.Add(button);
                this.cm.AddToolBar(this.helpToolBar);
            }

            private void InitializeMainToolBar()
            {
                this.mainToolBar.Buttons.AddRange(new ToolBarButton[] { 
                    (ToolBarButton) this.toolBarButtonTable1[1], (ToolBarButton) this.toolBarButtonTable1[2], (ToolBarButton) this.toolBarButtonTable1[3], (ToolBarButton) this.toolBarButtonTable1[10], this.CreateSeparatorButton(), (ToolBarButton) this.toolBarButtonTable1[6], this.CreateSeparatorButton(), (ToolBarButton) this.toolBarButtonTable1[0x66], (ToolBarButton) this.toolBarButtonTable1[0x67], (ToolBarButton) this.toolBarButtonTable1[0x68], this.CreateSeparatorButton(), (ToolBarButton) this.toolBarButtonTable1[100], (ToolBarButton) this.toolBarButtonTable1[0x65], this.CreateSeparatorButton(), (ToolBarButton) this.toolBarButtonTable1[110], this.CreateSeparatorButton(), 
                    (ToolBarButton) this.toolBarButtonTable1[210], this.CreateSeparatorButton(), (ToolBarButton) this.toolBarButtonTable1[0xd6], this.CreateSeparatorButton(), this.CreateSeparatorButton(), (ToolBarButton) this.toolBarButtonTable1[600]
                 });
                this.cm.AddToolBar(this.mainToolBar);
            }

            private void InitializeMenuBar()
            {
                int num;
                MenuItem[] items = new MenuItem[10];
                for (num = 0; num < 10; num++)
                {
                    items[num] = (MenuItem) this.globalCommandTable[num + 20];
                }
                MxMenuItem item = new MxMenuItem("Recent &Files", items);
                this.cm.AddMenu(item);
                MenuItem[] itemArray2 = new MenuItem[5];
                for (num = 0; num < 5; num++)
                {
                    itemArray2[num] = (MenuItem) this.globalCommandTable[num + 40];
                }
                MxMenuItem item2 = new MxMenuItem("Recent &Projects", itemArray2);
                this.cm.AddMenu(item2);
                MenuItem[] itemArray3 = new MenuItem[] { (MenuItem) this.globalCommandTable[1], (MenuItem) this.globalCommandTable[2], (MenuItem) this.globalCommandTable[3], (MenuItem) this.globalCommandTable[4], (MenuItem) this.globalCommandTable[5], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[10], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[6], (MenuItem) this.globalCommandTable[7], (MenuItem) this.globalCommandTable[8], new MxMenuItem("-"), item, new MxMenuItem("-"), (MenuItem) this.globalCommandTable[9] };
                MxMenuItem item3 = new MxMenuItem("&File", itemArray3, false);
                this.cm.AddMenu(item3);
                MenuItem[] itemArray4 = new MenuItem[] { 
                    (MenuItem) this.globalCommandTable[100], (MenuItem) this.globalCommandTable[0x65], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0x66], (MenuItem) this.globalCommandTable[0x67], (MenuItem) this.globalCommandTable[0x68], (MenuItem) this.globalCommandTable[0x6a], (MenuItem) this.globalCommandTable[120], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0x6b], (MenuItem) this.globalCommandTable[0x6c], (MenuItem) this.globalCommandTable[180], (MenuItem) this.globalCommandTable[0x6d], new MxMenuItem("-"), 
                    new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0x8d], (MenuItem) this.globalCommandTable[0x8e]
                 };
                MxMenuItem item4 = new MxMenuItem("&Edit", itemArray4, false);
                this.cm.AddMenu(item4);
                MenuItem[] itemArray5 = new MenuItem[] { (MenuItem) this.globalCommandTable[200], (MenuItem) this.globalCommandTable[0xc9], (MenuItem) this.globalCommandTable[0xca], (MenuItem) this.globalCommandTable[0xcb], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[210], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0xd6], new MxMenuItem("-")};
                MxMenuItem item5 = new MxMenuItem("&View", itemArray5, false);
                this.cm.AddMenu(item5);
                MenuItem[] itemArray11 = new MenuItem[] { 
                    (MenuItem) this.globalCommandTable[300], (MenuItem) this.globalCommandTable[0x12d], (MenuItem) this.globalCommandTable[0x12e], (MenuItem) this.globalCommandTable[0x12f], (MenuItem) this.globalCommandTable[0x130], (MenuItem) this.globalCommandTable[0x131], (MenuItem) this.globalCommandTable[0x132], (MenuItem) this.globalCommandTable[0x133], (MenuItem) this.globalCommandTable[0x134], (MenuItem) this.globalCommandTable[0x135], (MenuItem) this.globalCommandTable[320], (MenuItem) this.globalCommandTable[0x141], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0x145], (MenuItem) this.globalCommandTable[0x146], (MenuItem) this.globalCommandTable[0x147], 
                    new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0x144]
                 };
                MxMenuItem item11 = new MxMenuItem("&Tools", itemArray11, false);
                this.cm.AddMenu(item11);
                MenuItem[] itemArray12 = new MenuItem[] { (MenuItem) this.globalCommandTable[400], (MenuItem) this.globalCommandTable[0x191], (MenuItem) this.globalCommandTable[0x192], (MenuItem) this.globalCommandTable[0x193] };
                MxMenuItem item12 = new MxMenuItem("&Window", itemArray12, false);
                item12.MdiList = true;
                this.cm.AddMenu(item12);
                MenuItem[] itemArray13 = new MenuItem[] { (MenuItem) this.globalCommandTable[600], (MenuItem) this.globalCommandTable[610], (MenuItem) this.globalCommandTable[0x263], (MenuItem) this.globalCommandTable[0x264], (MenuItem) this.globalCommandTable[0x265], (MenuItem) this.globalCommandTable[0x266], (MenuItem) this.globalCommandTable[0x267], (MenuItem) this.globalCommandTable[0x268], (MenuItem) this.globalCommandTable[0x269], (MenuItem) this.globalCommandTable[0x26a], (MenuItem) this.globalCommandTable[0x26b], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0x25e], new MxMenuItem("-"), (MenuItem) this.globalCommandTable[0x25a], (MenuItem) this.globalCommandTable[0x259] };
                MxMenuItem item13 = new MxMenuItem("&Help", itemArray13, false);
                this.cm.AddMenu(item13);
                this.menuBar.MenuItems.AddRange(new MenuItem[] { item3, item4, item5, item11, item12, item13 });
                MenuItem[] menuItems = new MenuItem[] { 
                    (MenuItem) this.contextCommandTable[710], new MxMenuItem("-"), (MenuItem) this.contextCommandTable[0x2c7], (MenuItem) this.contextCommandTable[0x2c8], new MxMenuItem("-"), (MenuItem) this.contextCommandTable[0x2c9], (MenuItem) this.contextCommandTable[0x2ca], new MxMenuItem("-"), (MenuItem) this.contextCommandTable[720], (MenuItem) this.contextCommandTable[0x2d1], (MenuItem) this.contextCommandTable[0x2d2], (MenuItem) this.contextCommandTable[0x2d3], (MenuItem) this.contextCommandTable[0x2d4], (MenuItem) this.contextCommandTable[0x2d5], (MenuItem) this.contextCommandTable[0x2d6], (MenuItem) this.contextCommandTable[0x2d7], 
                    new MxMenuItem("-"), (MenuItem) this.contextCommandTable[10]
                 };
                MxContextMenu menu = new MxContextMenu(menuItems);
                this.cm.AddMenu(menu, typeof(GlobalCommands), 1);
                MenuItem[] itemArray15 = new MenuItem[] { (MenuItem) this.contextCommandTable[0x145], (MenuItem) this.contextCommandTable[0x146], (MenuItem) this.contextCommandTable[0x147], new MxMenuItem("-"), (MenuItem) this.contextCommandTable[0x148], (MenuItem) this.contextCommandTable[0x149], (MenuItem) this.contextCommandTable[0x14b], new MxMenuItem("-"), (MenuItem) this.contextCommandTable[330] };
                MxContextMenu menu2 = new MxContextMenu(itemArray15);
                this.cm.AddMenu(menu2, typeof(GlobalCommands), 2);
                MenuItem[] itemArray18 = new MenuItem[] { (MenuItem) this.contextCommandTable[0x66], (MenuItem) this.contextCommandTable[0x67], (MenuItem) this.contextCommandTable[0x68], new MxMenuItem("-"), (MenuItem) this.contextCommandTable[120] };
                MxContextMenu menu5 = new MxContextMenu(itemArray18);
                this.cm.AddMenu(menu5, typeof(GlobalCommands), 3);
            }

            private void InitializeStatusBar()
            {
                System.Type commandGroup = typeof(GlobalCommands);
                this.cm.AddStatusBarPanel((MxStatusBarPanel) this.mainStatusBar.Panels[0], commandGroup, 0x3e8);
                this.cm.AddStatusBarPanel((EditorStatusBarPanel) this.mainStatusBar.Panels[2], commandGroup, 0x3e9);
                this.cm.AddStatusBarPanel((MxStatusBarPanel) this.mainStatusBar.Panels[3], commandGroup, 0x3ea);
                this.cm.AddStatusBarPanel((MxStatusBarPanel) this.mainStatusBar.Panels[4], commandGroup, 0x3eb);
            }
        }
    }
}


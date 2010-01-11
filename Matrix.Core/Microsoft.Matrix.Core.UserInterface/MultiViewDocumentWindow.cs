namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public abstract class MultiViewDocumentWindow : DocumentWindow, IMultiViewDocumentWindow
    {
        private EventHandler _currentViewChangedHandler;
        private ChildViewHost _viewHost;

        event EventHandler IMultiViewDocumentWindow.CurrentViewChanged
        {
            add
            {
                this._currentViewChangedHandler = (EventHandler) Delegate.Combine(this._currentViewChangedHandler, value);
            }
            remove
            {
                if (this._currentViewChangedHandler != null)
                {
                    this._currentViewChangedHandler = (EventHandler) Delegate.Remove(this._currentViewChangedHandler, value);
                }
            }
        }

        public MultiViewDocumentWindow(IServiceProvider serviceProvider, Document document) : base(serviceProvider, document)
        {
        }

        protected void AddDocumentView(Control viewControl, bool isDefault)
        {
            this._viewHost.AddChildView(viewControl, isDefault);
        }

        protected override IDocumentView CreateDocumentView()
        {
            this._viewHost = new ChildViewHost(this, this.ViewBorderStyle);
            this.CreateDocumentViews();
            this._viewHost.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedViewHost);
            this._viewHost.Dock = DockStyle.Fill;
            base.ViewContainer.Controls.Add(this._viewHost);
            return this._viewHost;
        }

        protected abstract void CreateDocumentViews();
        protected override void DisposeDocumentView()
        {
        }

        protected override bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 200:
                    case 0xc9:
                    case 0xca:
                    case 0xcb:
                    {
                        int viewIndex = command.CommandID - 200;
                        this._viewHost.SwitchToView(viewIndex);
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                flag = base.HandleCommand(command);
            }
            return flag;
        }

        void IMultiViewDocumentWindow.ActivateView(IDocumentView documentView)
        {
            this._viewHost.SwitchToView(documentView);
        }

        IDocumentView IMultiViewDocumentWindow.GetViewByIndex(int index)
        {
            return this._viewHost.GetViewByIndex(index);
        }

        IDocumentView IMultiViewDocumentWindow.GetViewByType(DocumentViewType viewType)
        {
            return this._viewHost.GetViewByType(viewType);
        }

        protected virtual void OnCurrentViewChanged(EventArgs e)
        {
            if (this._currentViewChangedHandler != null)
            {
                this._currentViewChangedHandler(this, EventArgs.Empty);
            }
        }

        private void OnSelectedIndexChangedViewHost(object sender, EventArgs e)
        {
            this.OnCurrentViewChanged(EventArgs.Empty);
        }

        protected override bool UpdateCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 200:
                    case 0xc9:
                    case 0xca:
                    case 0xcb:
                    {
                        int num = command.CommandID - 200;
                        if (num >= this._viewHost.Views.Count)
                        {
                            command.Enabled = false;
                            command.Visible = false;
                        }
                        else
                        {
                            ViewInfo info = (ViewInfo) this._viewHost.Views[num];
                            command.Text = info.view.ViewName;
                            command.Glyph = info.view.ViewImage;
                            command.Visible = true;
                        }
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }

        IDocumentView IMultiViewDocumentWindow.CurrentView
        {
            get
            {
                return this._viewHost.CurrentDocumentView;
            }
        }

        int IMultiViewDocumentWindow.ViewCount
        {
            get
            {
                return this._viewHost.Views.Count;
            }
        }

        protected virtual BorderStyle ViewBorderStyle
        {
            get
            {
                return BorderStyle.FixedSingle;
            }
        }

        private sealed class ChildViewHost : Microsoft.Matrix.UIComponents.TabControl, IDocumentView, ICommandHandler, IToolboxClient, ISelectionContainer, IPropertyBrowserClient, ISearchableDocumentView
        {
            private MultiViewDocumentWindow.ViewInfo _currentView;
            private int _defaultViewIndex;
            private EventHandler _documentChangedHandler;
            private MultiViewDocumentWindow _owner;
            private BorderStyle _viewBorderStyle;
            private ArrayList _views;

            event EventHandler IDocumentView.DocumentChanged
            {
                add
                {
                    this._documentChangedHandler = (EventHandler) Delegate.Combine(this._documentChangedHandler, value);
                }
                remove
                {
                    if (this._documentChangedHandler != null)
                    {
                        this._documentChangedHandler = (EventHandler) Delegate.Remove(this._documentChangedHandler, value);
                    }
                }
            }

            public ChildViewHost(MultiViewDocumentWindow owner, BorderStyle viewBorderStyle)
            {
                this._owner = owner;
                this._viewBorderStyle = viewBorderStyle;
                ImageList list = new ImageList();
                list.ImageSize = new Size(0x10, 0x10);
                base.ImageList = list;
                base.Mode = TabControlMode.TextAndImage;
                base.TabPlacement = TabPlacement.Bottom;
                this._views = new ArrayList();
                this._defaultViewIndex = -1;
            }

            private void ActivateCurrentView(bool viewSwitch)
            {
                if (this._currentView != null)
                {
                    this._currentView.view.LoadFromDocument(this._owner.Document);
                    this._currentView.view.Activate(viewSwitch);
                }
            }

            public void AddChildView(Control viewControl, bool isDefault)
            {
                if (viewControl == null)
                {
                    throw new ArgumentNullException("viewControl");
                }
                IDocumentView view = viewControl as IDocumentView;
                if (view == null)
                {
                    throw new ArgumentException("viewControl must implement IDocumentView", "viewControl");
                }
                MultiViewDocumentWindow.ViewInfo info = new MultiViewDocumentWindow.ViewInfo();
                Image viewImage = view.ViewImage;
                int num = -1;
                if (viewImage != null)
                {
                    base.ImageList.Images.Add(viewImage);
                    num = base.ImageList.Images.Count - 1;
                }
                string viewName = view.ViewName;
                if (viewName == null)
                {
                    viewName = string.Empty;
                }
                viewControl.Dock = DockStyle.Fill;
                Microsoft.Matrix.UIComponents.TabPage page = new Microsoft.Matrix.UIComponents.TabPage();
                page.Text = viewName;
                page.ImageIndex = num;
                if (this._viewBorderStyle == BorderStyle.FixedSingle)
                {
                    page.BackColor = SystemColors.ControlDark;
                    page.DockPadding.All = 1;
                }
                page.Controls.Add(viewControl);
                base.Tabs.Add(page);
                info.view = view;
                info.viewTab = page;
                info.viewCommandHandler = view as ICommandHandler;
                info.viewToolboxClient = view as IToolboxClient;
                info.viewSelectionContainer = view as ISelectionContainer;
                info.view.DocumentChanged += new EventHandler(this.OnViewDocumentChanged);
                this._views.Add(info);
                if (isDefault || (base.Tabs.Count == 1))
                {
                    this._currentView = info;
                    this._defaultViewIndex = base.Tabs.Count - 1;
                }
            }

            private bool DeactivateCurrentView()
            {
                if (this._currentView == null)
                {
                    return true;
                }
                bool flag = false;
                if (this._currentView.view.CanDeactivate)
                {
                    bool flag2 = true;
                    if (this._currentView.view.IsDirty)
                    {
                        flag2 = this._currentView.view.SaveToDocument();
                    }
                    if (flag2)
                    {
                        this._currentView.view.Deactivate(false);
                        this._currentView = null;
                        flag = true;
                    }
                }
                return flag;
            }

            public IDocumentView GetViewByIndex(int index)
            {
                if ((index < 0) || (index > this._views.Count))
                {
                    throw new ArgumentOutOfRangeException("Invalid index into ChildViewHost");
                }
                return (IDocumentView) this._views[index];
            }

            public IDocumentView GetViewByType(DocumentViewType viewType)
            {
                if ((viewType < DocumentViewType.Default) || (viewType > DocumentViewType.Composite))
                {
                    throw new ArgumentOutOfRangeException("Invalid DocumentViewType");
                }
                foreach (MultiViewDocumentWindow.ViewInfo info in this._views)
                {
                    if (info.view.ViewType == viewType)
                    {
                        return info.view;
                    }
                }
                return null;
            }

            void IDocumentView.Activate(bool viewSwitch)
            {
                if (this._defaultViewIndex != -1)
                {
                    this._currentView = (MultiViewDocumentWindow.ViewInfo) this._views[this._defaultViewIndex];
                    this.SelectedIndex = this._defaultViewIndex;
                    this._defaultViewIndex = -1;
                }
                if (this._currentView != null)
                {
                    this._currentView.view.Activate(viewSwitch);
                }
            }

            void IDocumentView.Deactivate(bool closing)
            {
                if (this._currentView != null)
                {
                    this._currentView.view.Deactivate(closing);
                    this._currentView = null;
                }
            }

            void IDocumentView.LoadFromDocument(Document document)
            {
                if (this._currentView != null)
                {
                    this._currentView.view.LoadFromDocument(document);
                }
            }

            bool IDocumentView.SaveToDocument()
            {
                if (this._currentView != null)
                {
                    return this._currentView.view.SaveToDocument();
                }
                return true;
            }

            bool ISearchableDocumentView.PerformFind(string searchString, FindReplaceOptions options)
            {
                return ((this._currentView.view is ISearchableDocumentView) && ((ISearchableDocumentView) this._currentView.view).PerformFind(searchString, options));
            }

            bool ISearchableDocumentView.PerformReplace(string searchString, string replaceString, FindReplaceOptions options)
            {
                return ((this._currentView.view is ISearchableDocumentView) && ((ISearchableDocumentView) this._currentView.view).PerformReplace(searchString, replaceString, options));
            }

            void ISelectionContainer.SetSelectedObject(object o)
            {
                if ((this._currentView != null) && (this._currentView.viewSelectionContainer != null))
                {
                    this._currentView.viewSelectionContainer.SetSelectedObject(o);
                }
            }

            void IToolboxClient.OnToolboxDataItemPicked(ToolboxDataItem dataItem)
            {
                if ((this._currentView != null) && (this._currentView.viewToolboxClient != null))
                {
                    this._currentView.viewToolboxClient.OnToolboxDataItemPicked(dataItem);
                }
            }

            bool IToolboxClient.SupportsToolboxSection(ToolboxSection section)
            {
                return (((this._currentView != null) && (this._currentView.viewToolboxClient != null)) && this._currentView.viewToolboxClient.SupportsToolboxSection(section));
            }

            bool ICommandHandler.HandleCommand(Command command)
            {
                return (((this._currentView != null) && (this._currentView.viewCommandHandler != null)) && this._currentView.viewCommandHandler.HandleCommand(command));
            }

            bool ICommandHandler.UpdateCommand(Command command)
            {
                return (((this._currentView != null) && (this._currentView.viewCommandHandler != null)) && this._currentView.viewCommandHandler.UpdateCommand(command));
            }

            protected override void OnSelectedIndexChanged(EventArgs e)
            {
                this._currentView = null;
                int selectedIndex = this.SelectedIndex;
                if ((selectedIndex != -1) && (selectedIndex < this._views.Count))
                {
                    this._currentView = (MultiViewDocumentWindow.ViewInfo) this._views[selectedIndex];
                    this.ActivateCurrentView(true);
                }
                base.OnSelectedIndexChanged(e);
            }

            protected override void OnSelectedIndexChanging(CancelEventArgs e)
            {
                base.OnSelectedIndexChanging(e);
                if (!this.DeactivateCurrentView())
                {
                    e.Cancel = true;
                }
            }

            private void OnViewDocumentChanged(object sender, EventArgs e)
            {
                if (this._documentChangedHandler != null)
                {
                    this._documentChangedHandler(this, EventArgs.Empty);
                }
            }

            public void SwitchToView(IDocumentView documentView)
            {
                if (documentView == null)
                {
                    throw new ArgumentNullException("Can't switch to a null IDocumentView");
                }
                for (int i = 0; i < this._views.Count; i++)
                {
                    MultiViewDocumentWindow.ViewInfo info = this._views[i] as MultiViewDocumentWindow.ViewInfo;
                    if (info.view == documentView)
                    {
                        this.SwitchToView(i);
                        return;
                    }
                }
            }

            public void SwitchToView(int viewIndex)
            {
                if (!base.IsHandleCreated)
                {
                    this._defaultViewIndex = viewIndex;
                }
                MultiViewDocumentWindow.ViewInfo info = (MultiViewDocumentWindow.ViewInfo) this._views[viewIndex];
                if (this._currentView != info)
                {
                    this.SelectedIndex = viewIndex;
                }
            }

            public IDocumentView CurrentDocumentView
            {
                get
                {
                    if (this._currentView != null)
                    {
                        return this._currentView.view;
                    }
                    return null;
                }
            }

            bool IDocumentView.CanDeactivate
            {
                get
                {
                    if (this._currentView != null)
                    {
                        return this._currentView.view.CanDeactivate;
                    }
                    return true;
                }
            }

            bool IDocumentView.IsDirty
            {
                get
                {
                    return ((this._currentView != null) && this._currentView.view.IsDirty);
                }
            }

            Image IDocumentView.ViewImage
            {
                get
                {
                    return null;
                }
            }

            string IDocumentView.ViewName
            {
                get
                {
                    return null;
                }
            }

            DocumentViewType IDocumentView.ViewType
            {
                get
                {
                    return DocumentViewType.Default;
                }
            }

            bool IPropertyBrowserClient.SupportsPropertyBrowser
            {
                get
                {
                    if (this._currentView != null)
                    {
                        IPropertyBrowserClient view = this._currentView.view as IPropertyBrowserClient;
                        if (view != null)
                        {
                            return view.SupportsPropertyBrowser;
                        }
                    }
                    return false;
                }
            }

            FindReplaceOptions ISearchableDocumentView.FindSupport
            {
                get
                {
                    if (this._currentView.view is ISearchableDocumentView)
                    {
                        return ((ISearchableDocumentView) this._currentView.view).FindSupport;
                    }
                    return FindReplaceOptions.None;
                }
            }

            string ISearchableDocumentView.InitialSearchString
            {
                get
                {
                    if (this._currentView.view is ISearchableDocumentView)
                    {
                        return ((ISearchableDocumentView) this._currentView.view).InitialSearchString;
                    }
                    return string.Empty;
                }
            }

            FindReplaceOptions ISearchableDocumentView.ReplaceSupport
            {
                get
                {
                    if (this._currentView.view is ISearchableDocumentView)
                    {
                        return ((ISearchableDocumentView) this._currentView.view).ReplaceSupport;
                    }
                    return FindReplaceOptions.None;
                }
            }

            ToolboxSection IToolboxClient.DefaultToolboxSection
            {
                get
                {
                    if ((this._currentView != null) && (this._currentView.viewToolboxClient != null))
                    {
                        return this._currentView.viewToolboxClient.DefaultToolboxSection;
                    }
                    return null;
                }
            }

            public override int SelectedIndex
            {
                get
                {
                    return base.SelectedIndex;
                }
                set
                {
                    if (this.SelectedIndex != value)
                    {
                        CancelEventArgs e = new CancelEventArgs();
                        this.OnSelectedIndexChanging(e);
                        if (!e.Cancel)
                        {
                            base.SelectedIndex = value;
                            this.OnSelectedIndexChanged(EventArgs.Empty);
                        }
                    }
                }
            }

            public ArrayList Views
            {
                get
                {
                    return this._views;
                }
            }
        }

        private sealed class ViewInfo
        {
            public IDocumentView view;
            public ICommandHandler viewCommandHandler;
            public ISelectionContainer viewSelectionContainer;
            public Microsoft.Matrix.UIComponents.TabPage viewTab;
            public IToolboxClient viewToolboxClient;
        }
    }
}


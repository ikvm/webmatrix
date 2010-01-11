namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class PropertyBrowserToolWindow : ToolWindow
    {
        private IDesignerHost _activeDesigner;
        private MxComboBox _componentsCombo;
        private bool _internalSelectionChange;
        private MxComboBox _outlineCombo;
        private PropertyGrid _propGrid;
        private PropertyGridToolBarSubclasser _toolBarSubclasser;
        private const PropertySort defaultPropertySort = PropertySort.Alphabetical;

        public PropertyBrowserToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            this.LoadPreferences();
            IDesignerEventService service = (IDesignerEventService) this.GetService(typeof(IDesignerEventService));
            if (service != null)
            {
                service.SelectionChanged += new EventHandler(this.OnSelectionChanged);
                service.ActiveDesignerChanged += new ActiveDesignerEventHandler(this.OnActiveDesignerChanged);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._activeDesigner != null)
                {
                    IComponentChangeService service = (IComponentChangeService) this._activeDesigner.GetService(typeof(IComponentChangeService));
                    if (service != null)
                    {
                        service.ComponentAdded -= new ComponentEventHandler(this.OnComponentCollectionChanged);
                        service.ComponentRemoved -= new ComponentEventHandler(this.OnComponentCollectionChanged);
                    }
                    this._activeDesigner = null;
                }
                IDesignerEventService service2 = (IDesignerEventService) this.GetService(typeof(IDesignerEventService));
                if (service2 != null)
                {
                    service2.SelectionChanged -= new EventHandler(this.OnSelectionChanged);
                    service2.ActiveDesignerChanged -= new ActiveDesignerEventHandler(this.OnActiveDesignerChanged);
                }
                this.SavePreferences();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._componentsCombo = new MxComboBox();
            this._outlineCombo = new MxComboBox();
            this._propGrid = new PropertyGrid();
            this._propGrid.Site = new PropertyGridSite(this._propGrid);
            this._propGrid.PropertyTabs.AddTabType(typeof(EventsTab));
            this._propGrid.SuspendLayout();
            base.SuspendLayout();
            this._componentsCombo.Dock = DockStyle.Top;
            this._componentsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._componentsCombo.MaxDropDownItems = 10;
            this._componentsCombo.Size = new Size(150, 0x15);
            this._componentsCombo.Sorted = true;
            this._componentsCombo.TabIndex = 0;
            this._componentsCombo.FlatAppearance = true;
            this._componentsCombo.Enabled = false;
            this._componentsCombo.DrawMode = DrawMode.OwnerDrawFixed;
            this._componentsCombo.DrawItem += new DrawItemEventHandler(this.OnDrawItemComponentsCombo);
            this._componentsCombo.SelectedIndexChanged += new EventHandler(this.OnSelectionChangedComponentsCombo);
            this._outlineCombo.Dock = DockStyle.Top;
            this._outlineCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._outlineCombo.Size = new Size(150, 0x15);
            this._outlineCombo.TabIndex = 1;
            this._outlineCombo.FlatAppearance = true;
            this._outlineCombo.Enabled = false;
            this._outlineCombo.SelectedIndexChanged += new EventHandler(this.OnSelectionChangedOutlineCombo);
            this._propGrid.CommandsVisibleIfAvailable = true;
            this._propGrid.Dock = DockStyle.Fill;
            this._propGrid.LargeButtons = false;
            this._propGrid.LineColor = SystemColors.Control;
            this._propGrid.Location = new Point(0, 0x2a);
            this._propGrid.Size = new Size(150, 0xc6);
            this._propGrid.TabIndex = 2;
            this._propGrid.PropertySort = PropertySort.Alphabetical;
            base.Controls.AddRange(new Control[] { this._propGrid, this._outlineCombo, this._componentsCombo });
            base.Size = new Size(150, 240);
            this.Text = "Properties";
            base.Icon = new Icon(typeof(PropertyBrowserToolWindow), "PropertyBrowserToolWindow.ico");
            this._propGrid.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadPreferences()
        {
            PreferencesStore store;
            IPreferencesService service = (IPreferencesService) this.GetService(typeof(IPreferencesService));
            if ((service != null) && service.GetPreferencesStore(typeof(PropertyBrowserToolWindow), out store))
            {
                PropertySort sort = (PropertySort) store.GetValue("PropertySort", 1);
                this._propGrid.PropertySort = sort;
            }
        }

        private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
        {
            ComponentEventHandler handler = new ComponentEventHandler(this.OnComponentCollectionChanged);
            if (this._activeDesigner != null)
            {
                IComponentChangeService service = (IComponentChangeService) this._activeDesigner.GetService(typeof(IComponentChangeService));
                if (service != null)
                {
                    service.ComponentAdded -= handler;
                    service.ComponentRemoved -= handler;
                }
            }
            this._activeDesigner = e.NewDesigner;
            if (this._activeDesigner != null)
            {
                IComponentChangeService service2 = (IComponentChangeService) this._activeDesigner.GetService(typeof(IComponentChangeService));
                if (service2 != null)
                {
                    service2.ComponentAdded += handler;
                    service2.ComponentRemoved += handler;
                }
                IPropertyBrowserClient client = e.NewDesigner.GetService(typeof(DocumentWindow)) as IPropertyBrowserClient;
                if (client != null)
                {
                    base.Enabled = client.SupportsPropertyBrowser;
                    if (!client.SupportsPropertyBrowser)
                    {
                        this._propGrid.SelectedObject = null;
                    }
                }
            }
            this.UpdateSelection();
        }

        private void OnComponentCollectionChanged(object sender, ComponentEventArgs e)
        {
            this.UpdateSelection();
        }

        private void OnDrawItemComponentsCombo(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                PropertyBrowserEntry entry = (PropertyBrowserEntry) this._componentsCombo.Items[e.Index];
                if (entry != null)
                {
                    Font prototype = this.Font;
                    Font font = new Font(prototype, FontStyle.Bold);
                    e.DrawBackground();
                    e.DrawFocusRectangle();
                    Rectangle bounds = e.Bounds;
                    Brush brush = new SolidBrush(e.ForeColor);
                    StringFormat format = new StringFormat();
                    format.LineAlignment = StringAlignment.Center;
                    string entryName = entry.GetEntryName();
                    e.Graphics.DrawString(entryName, font, brush, bounds, format);
                    if (entry.ShowTypeName)
                    {
                        SizeF ef = e.Graphics.MeasureString(entryName, font);
                        bounds.X += ((int) ef.Width) + 8;
                        e.Graphics.DrawString(entry.GetEntryTypeName(), prototype, brush, bounds, format);
                    }
                    format.Dispose();
                    brush.Dispose();
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ToolBar toolBar = this._propGrid.Controls[0] as ToolBar;
            if (toolBar != null)
            {
                this._toolBarSubclasser = new PropertyGridToolBarSubclasser(toolBar);
                this._toolBarSubclasser.Subclass();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (this._toolBarSubclasser != null)
            {
                this._toolBarSubclasser.Unsubclass();
                this._toolBarSubclasser = null;
            }
            base.OnHandleDestroyed(e);
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            this.UpdateSelection();
        }

        private void OnSelectionChangedComponentsCombo(object sender, EventArgs e)
        {
            PropertyBrowserEntry selectedItem = (PropertyBrowserEntry) this._componentsCombo.SelectedItem;
            if (selectedItem == null)
            {
                this._propGrid.SelectedObject = null;
                this._outlineCombo.Items.Clear();
                this._outlineCombo.Enabled = false;
            }
            else
            {
                this._propGrid.SelectedObject = selectedItem.Entry;
                DocumentWindow service = (DocumentWindow) this._activeDesigner.GetService(typeof(DocumentWindow));
                ISelectionContainer container = service;
                if (!this._internalSelectionChange)
                {
                    container.SetSelectedObject(this._propGrid.SelectedObject);
                }
                this._outlineCombo.Items.Clear();
                bool flag = false;
                ISelectionOutlineProvider provider = (ISelectionOutlineProvider) this._activeDesigner.GetService(typeof(ISelectionOutlineProvider));
                if (provider != null)
                {
                    this._outlineCombo.Enabled = true;
                    ICollection outline = provider.GetOutline(this._propGrid.SelectedObject);
                    if ((outline != null) && (outline.Count > 0))
                    {
                        flag = true;
                        this._internalSelectionChange = true;
                        try
                        {
                            this._outlineCombo.Items.Add(provider.OutlineTitle);
                            foreach (object obj2 in outline)
                            {
                                this._outlineCombo.Items.Add(new PropertyBrowserEntry(obj2));
                            }
                            this._outlineCombo.SelectedIndex = 0;
                        }
                        finally
                        {
                            this._internalSelectionChange = false;
                        }
                    }
                }
                this._outlineCombo.Enabled = flag;
            }
        }

        private void OnSelectionChangedOutlineCombo(object sender, EventArgs e)
        {
            if (!this._internalSelectionChange)
            {
                PropertyBrowserEntry selectedItem = this._outlineCombo.SelectedItem as PropertyBrowserEntry;
                if (selectedItem != null)
                {
                    DocumentWindow service = (DocumentWindow) this._activeDesigner.GetService(typeof(DocumentWindow));
                    ISelectionContainer container = service;
                    container.SetSelectedObject(selectedItem.Entry);
                }
            }
        }

        private void SavePreferences()
        {
            IPreferencesService service = (IPreferencesService) this.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                service.GetPreferencesStore(typeof(PropertyBrowserToolWindow)).SetValue("PropertySort", (int) this._propGrid.PropertySort, 1);
            }
        }

        private void UpdateSelection()
        {
            ICollection is3;
            this._componentsCombo.Items.Clear();
            this._outlineCombo.Items.Clear();
            if (!base.Enabled)
            {
                return;
            }
            object[] array = null;
            int count = 0;
            PropertyBrowserEntry entry = null;
            if (this._activeDesigner != null)
            {
                ICollection selectedComponents = ((ISelectionService) this._activeDesigner.GetService(typeof(ISelectionService))).GetSelectedComponents();
                if (selectedComponents != null)
                {
                    count = selectedComponents.Count;
                    array = new object[count];
                    selectedComponents.CopyTo(array, 0);
                    for (int i = 0; i < count; i++)
                    {
                        PropertyBrowserEntry entry2;
                        IComponent component = array[i] as IComponent;
                        if (component != null)
                        {
                            entry2 = new PropertyBrowserComponentEntry(component);
                        }
                        else
                        {
                            entry2 = new PropertyBrowserEntry(array[i]);
                        }
                        this._componentsCombo.Items.Add(entry2);
                        if (i == 0)
                        {
                            entry = entry2;
                        }
                    }
                }
                foreach (IComponent component2 in this._activeDesigner.Container.Components)
                {
                    bool flag = false;
                    for (int j = 0; j < count; j++)
                    {
                        if (array[j] == component2)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        PropertyBrowserEntry item = new PropertyBrowserComponentEntry(component2);
                        this._componentsCombo.Items.Add(item);
                    }
                }
            }
            if (this._componentsCombo.Items.Count == 0)
            {
                this._componentsCombo.Enabled = false;
                this._outlineCombo.Enabled = false;
                this._propGrid.SelectedObject = null;
            }
            else
            {
                this._componentsCombo.Enabled = true;
                switch (count)
                {
                    case 0:
                        this._componentsCombo.SelectedIndex = -1;
                        this._propGrid.SelectedObject = null;
                        goto Label_021E;

                    case 1:
                        this._internalSelectionChange = true;
                        try
                        {
                            this._componentsCombo.SelectedItem = entry;
                        }
                        catch
                        {
                        }
                        finally
                        {
                            this._internalSelectionChange = false;
                        }
                        this._propGrid.SelectedObject = array[0];
                        goto Label_021E;
                }
                try
                {
                    this._internalSelectionChange = true;
                    this._componentsCombo.SelectedIndex = -1;
                }
                catch
                {
                }
                finally
                {
                    this._internalSelectionChange = false;
                }
                this._propGrid.SelectedObjects = array;
            }
        Label_021E:
            is3 = this._componentsCombo.Items;
            int num4 = 0;
            if (is3.Count > 0)
            {
                Graphics graphics = base.CreateGraphics();
                Font prototype = this.Font;
                Font font = new Font(prototype, FontStyle.Bold);
                foreach (PropertyBrowserEntry entry4 in is3)
                {
                    int width = (int) graphics.MeasureString(entry4.GetEntryName(), font).Width;
                    if (entry4.ShowTypeName)
                    {
                        width += 10 + ((int) graphics.MeasureString(entry4.GetEntryTypeName(), prototype).Width);
                    }
                    if (width > num4)
                    {
                        num4 = width;
                    }
                }
                if (is3.Count >= this._componentsCombo.MaxDropDownItems)
                {
                    num4 += SystemInformation.VerticalScrollBarWidth;
                }
                graphics.Dispose();
            }
            num4 += 10;
            this._componentsCombo.DropDownWidth = num4;
        }

        private sealed class MxEventsTab : EventsTab
        {
            private System.Drawing.Bitmap _bitmap;

            public MxEventsTab(IServiceProvider serviceProvider) : base(serviceProvider)
            {
            }

            public override System.Drawing.Bitmap Bitmap
            {
                get
                {
                    if (this._bitmap == null)
                    {
                        try
                        {
                            this._bitmap = new System.Drawing.Bitmap(typeof(EventsTab), "EventsTab.bmp");
                        }
                        catch
                        {
                        }
                    }
                    return this._bitmap;
                }
            }
        }

        private class PropertyBrowserComponentEntry : PropertyBrowserToolWindow.PropertyBrowserEntry
        {
            public PropertyBrowserComponentEntry(IComponent component) : base(component)
            {
            }

            public override string GetEntryName()
            {
                IComponent entry = (IComponent) base.Entry;
                if (entry.Site != null)
                {
                    return entry.Site.Name;
                }
                return entry.ToString();
            }
        }

        private class PropertyBrowserEntry
        {
            private object _entry;
            private bool _showTypeName;

            public PropertyBrowserEntry(object entry)
            {
                this._entry = entry;
                this._showTypeName = true;
                object[] customAttributes = entry.GetType().GetCustomAttributes(typeof(DesignOnlyAttribute), true);
                if ((customAttributes != null) && (customAttributes.Length != 0))
                {
                    this._showTypeName = !((DesignOnlyAttribute) customAttributes[0]).IsDesignOnly;
                }
            }

            public virtual string GetEntryName()
            {
                return this._entry.ToString();
            }

            public virtual string GetEntryTypeName()
            {
                return this._entry.GetType().FullName;
            }

            public override string ToString()
            {
                return this.GetEntryName();
            }

            public object Entry
            {
                get
                {
                    return this._entry;
                }
            }

            public bool ShowTypeName
            {
                get
                {
                    return this._showTypeName;
                }
            }
        }

        private sealed class PropertyGridSite : ISite, IServiceProvider
        {
            private PropertyGrid _propGrid;

            public PropertyGridSite(PropertyGrid propGrid)
            {
                this._propGrid = propGrid;
            }

            object IServiceProvider.GetService(Type type)
            {
                object[] selectedObjects = this._propGrid.SelectedObjects;
                if ((selectedObjects.Length == 1) && typeof(IComponent).IsAssignableFrom(selectedObjects[0].GetType()))
                {
                    IComponent component = selectedObjects[0] as IComponent;
                    if (component.Site != null)
                    {
                        return component.Site.GetService(type);
                    }
                }
                return null;
            }

            IComponent ISite.Component
            {
                get
                {
                    return null;
                }
            }

            IContainer ISite.Container
            {
                get
                {
                    return null;
                }
            }

            bool ISite.DesignMode
            {
                get
                {
                    return true;
                }
            }

            string ISite.Name
            {
                get
                {
                    return "PropertyGrid";
                }
                set
                {
                }
            }
        }

        private sealed class PropertyGridToolBarSubclasser : MxToolBarSubclasser
        {
            private const int WM_PAINT = 15;

            public PropertyGridToolBarSubclasser(ToolBar toolBar) : base(toolBar)
            {
            }

            protected override void OnMessage(ref Message m)
            {
                if (m.Msg == 15)
                {
                    base.OnMessage(ref m);
                    this.RestoreBorders();
                }
                else
                {
                    base.OnMessage(ref m);
                }
            }

            private void RestoreBorders()
            {
                Graphics graphics = base.ToolBar.CreateGraphics();
                int height = base.ToolBar.Height;
                int width = base.ToolBar.Width;
                graphics.DrawLine(SystemPens.Control, 0, 0, width - 1, 0);
                graphics.DrawLine(SystemPens.Control, 0, height - 1, width - 1, height - 1);
                graphics.DrawLine(SystemPens.Control, 0, 0, 0, height - 1);
                graphics.DrawLine(SystemPens.Control, width - 1, 0, width - 1, height - 1);
                graphics.Dispose();
            }
        }
    }
}


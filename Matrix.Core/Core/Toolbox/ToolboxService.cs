namespace Microsoft.Matrix.Core.Toolbox
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    internal sealed class ToolboxService : IToolboxService, IDisposable
    {
        private ToolboxSection _activeSection;
        private ToolboxSectionEventHandler _activeSectionChangedHandler;
        private bool _initialReadConfig;
        private ToolboxSectionEventHandler _sectionAddedHandler;
        private ToolboxSectionEventHandler _sectionRemovedHandler;
        private ArrayList _sections;
        private byte[] _serializedToolboxSections;
        private IServiceProvider _serviceProvider;

        event ToolboxSectionEventHandler IToolboxService.ActiveSectionChanged
        {
            add
            {
                this._activeSectionChangedHandler = (ToolboxSectionEventHandler) Delegate.Combine(this._activeSectionChangedHandler, value);
            }
            remove
            {
                if (this._activeSectionChangedHandler != null)
                {
                    this._activeSectionChangedHandler = (ToolboxSectionEventHandler) Delegate.Remove(this._activeSectionChangedHandler, value);
                }
            }
        }

        event ToolboxSectionEventHandler IToolboxService.SectionAdded
        {
            add
            {
                this._sectionAddedHandler = (ToolboxSectionEventHandler) Delegate.Combine(this._sectionAddedHandler, value);
            }
            remove
            {
                if (this._sectionAddedHandler != null)
                {
                    this._sectionAddedHandler = (ToolboxSectionEventHandler) Delegate.Remove(this._sectionAddedHandler, value);
                }
            }
        }

        event ToolboxSectionEventHandler IToolboxService.SectionRemoved
        {
            add
            {
                this._sectionRemovedHandler = (ToolboxSectionEventHandler) Delegate.Combine(this._sectionRemovedHandler, value);
            }
            remove
            {
                if (this._sectionRemovedHandler != null)
                {
                    this._sectionRemovedHandler = (ToolboxSectionEventHandler) Delegate.Remove(this._sectionRemovedHandler, value);
                }
            }
        }

        public ToolboxService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._sections = new ArrayList();
            this._initialReadConfig = true;
        }

        public void LoadToolbox()
        {
            PreferencesStore store;
            IPreferencesService service = this._serviceProvider.GetService(typeof(IPreferencesService)) as IPreferencesService;
            if (service.GetPreferencesStore(typeof(IToolboxService), out store))
            {
                int num = store.GetValue("Count", 0);
                if (num > 0)
                {
                    IToolboxService service2 = this;
                    for (int i = 0; i < num; i++)
                    {
                        ToolboxSection section = store.GetValue("Section" + i) as ToolboxSection;
                        if (section != null)
                        {
                            service2.AddToolboxSection(section);
                        }
                    }
                }
            }
            else
            {
                ICollection is2 = this.ReadToolboxFromConfig();
                if (is2 != null)
                {
                    IToolboxService service3 = this;
                    foreach (ToolboxSection section2 in is2)
                    {
                        service3.AddToolboxSection(section2);
                    }
                }
            }
        }

        void IToolboxService.AddToolboxSection(ToolboxSection section)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }
            this._sections.Add(section);
            if (this._sectionAddedHandler != null)
            {
                this._sectionAddedHandler(this, new ToolboxSectionEventArgs(section));
            }
        }

        void IToolboxService.RemoveToolboxSection(ToolboxSection section)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }
            int index = -1;
            for (int i = this._sections.Count - 1; i >= 0; i--)
            {
                if (this._sections[i] == section)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new ArgumentException("Unknown section");
            }
            this._sections.RemoveAt(index);
            if (this._sectionRemovedHandler != null)
            {
                this._sectionRemovedHandler(this, new ToolboxSectionEventArgs(section));
            }
        }

        void IToolboxService.ResetToolbox()
        {
            IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
            if (service.ShowMessage("All toolbox customizations in all sections will be lost.\r\nAre you sure you want to continue?", "Reset Toolbox", MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                IToolboxService service2 = this;
                ICollection is2 = this.ReadToolboxFromConfig();
                IDictionary dictionary = new HybridDictionary(true);
                foreach (ToolboxSection section in this._sections)
                {
                    dictionary[section.Name] = section;
                }
                foreach (ToolboxSection section2 in is2)
                {
                    ToolboxSection section3 = (ToolboxSection) dictionary[section2.Name];
                    if (section3 != null)
                    {
                        section3.ClearToolboxDataItems();
                        foreach (ToolboxDataItem item in section2.ToolboxDataItems)
                        {
                            section3.AddToolboxDataItem(item, false);
                        }
                        section3.FireItemsChanged();
                        dictionary.Remove(section2.Name);
                        continue;
                    }
                    service2.AddToolboxSection(section2);
                }
                bool flag = false;
                foreach (ToolboxSection section4 in dictionary.Values)
                {
                    if (section4 == service2.ActiveSection)
                    {
                        flag = true;
                    }
                    service2.RemoveToolboxSection(section4);
                }
                if (flag)
                {
                    if (this._sections.Count > 0)
                    {
                        service2.ActiveSection = (ToolboxSection) this._sections[0];
                    }
                    else
                    {
                        service2.ActiveSection = null;
                    }
                }
            }
        }

        private void OnActiveSectionChanged(object sender, ToolboxSectionEventArgs e)
        {
            if (this._activeSectionChangedHandler != null)
            {
                this._activeSectionChangedHandler(this, e);
            }
        }

        private ICollection ReadToolboxFromConfig()
        {
            ArrayList graph = null;
            if (this._initialReadConfig)
            {
                try
                {
                    graph = ConfigurationSettings.GetConfig("microsoft.matrix/toolbox") as ArrayList;
                    if (graph != null)
                    {
                        MemoryStream serializationStream = new MemoryStream();
                        new BinaryFormatter().Serialize(serializationStream, graph);
                        this._serializedToolboxSections = serializationStream.ToArray();
                    }
                    this._initialReadConfig = false;
                }
                catch (Exception)
                {
                }
                return graph;
            }
            if (this._serializedToolboxSections != null)
            {
                MemoryStream stream2 = new MemoryStream(this._serializedToolboxSections);
                BinaryFormatter formatter2 = new BinaryFormatter();
                graph = (ArrayList) formatter2.Deserialize(stream2);
            }
            return graph;
        }

        public void SaveToolbox()
        {
            if (this._serviceProvider != null)
            {
                IPreferencesService service = this._serviceProvider.GetService(typeof(IPreferencesService)) as IPreferencesService;
                if (service != null)
                {
                    PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(IToolboxService));
                    int count = this._sections.Count;
                    preferencesStore.SetValue("Count", count, 0);
                    for (int i = 0; i < count; i++)
                    {
                        preferencesStore.SetValue("Section" + i, this._sections[i]);
                    }
                }
            }
        }

        void IDisposable.Dispose()
        {
            this.SaveToolbox();
            this._serviceProvider = null;
        }

        ToolboxSection IToolboxService.ActiveSection
        {
            get
            {
                return this._activeSection;
            }
            set
            {
                if (this._activeSection != value)
                {
                    this._activeSection = value;
                    this.OnActiveSectionChanged(this, new ToolboxSectionEventArgs(this._activeSection));
                }
            }
        }

        ICollection IToolboxService.Sections
        {
            get
            {
                return this._sections;
            }
        }
    }
}


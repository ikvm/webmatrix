namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Services;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Reflection;
    using System.Runtime.Serialization;

    internal sealed class ClassViewProjectData
    {
        private ArrayList _assemblyEntries = new ArrayList();
        private ArrayList _groups = new ArrayList();
        private bool _saveOnExit = false;
        private bool _showNonPublicMembers;
        private ClassViewProjectSortMode _sortMode;
        private ClassViewProjectViewMode _viewMode;
        private bool defaultShowNonPublicMembers = false;
        private ClassViewProjectSortMode defaultSortMode = ClassViewProjectSortMode.Alphabetical;
        private ClassViewProjectViewMode defaultViewMode = ClassViewProjectViewMode.Namespace;

        public AssemblyEntry AddAssemblyEntry(AssemblyName assemblyReference)
        {
            AssemblyEntry entry = new AssemblyEntry(assemblyReference);
            this._assemblyEntries.Add(entry);
            this.SetSaveOnExit();
            return entry;
        }

        public AssemblyEntry AddAssemblyEntry(string name, string displayName)
        {
            AssemblyEntry entry = new AssemblyEntry(name, displayName);
            this._assemblyEntries.Add(entry);
            this.SetSaveOnExit();
            return entry;
        }

        public GroupEntry AddGroupEntry(string name, ArrayList items)
        {
            GroupEntry entry = new GroupEntry(name, items);
            this._groups.Add(entry);
            this.SetSaveOnExit();
            return entry;
        }

        public void ClearAssemblyEntries()
        {
            this._assemblyEntries.Clear();
            this.SetSaveOnExit();
        }

        public static string GetNamespaceFromType(Type type)
        {
            Type declaringType = type.DeclaringType;
            while (true)
            {
                Type type3 = declaringType.DeclaringType;
                if (type3 == null)
                {
                    break;
                }
                declaringType = type3;
            }
            return declaringType.Namespace;
        }

        public void Load(IServiceProvider serviceProvider)
        {
            this._assemblyEntries.Clear();
            this._groups.Clear();
            IPreferencesService service = (IPreferencesService) serviceProvider.GetService(typeof(IPreferencesService));
            bool flag = true;
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(ClassViewProject));
                int num = preferencesStore.GetValue("AssemblyEntryCount", 0);
                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        AssemblyEntry entry = (AssemblyEntry) preferencesStore.GetValue("AssemblyEntry" + i);
                        if (entry != null)
                        {
                            this._assemblyEntries.Add(entry);
                        }
                    }
                    int num3 = preferencesStore.GetValue("GroupEntryCount", 0);
                    for (int j = 0; j < num3; j++)
                    {
                        GroupEntry entry2 = (GroupEntry) preferencesStore.GetValue("GroupEntry" + j);
                        if (entry2 != null)
                        {
                            this._groups.Add(entry2);
                        }
                    }
                    flag = false;
                    this._viewMode = (ClassViewProjectViewMode) preferencesStore.GetValue("ViewMode", (int) this.defaultViewMode);
                    this._sortMode = (ClassViewProjectSortMode) preferencesStore.GetValue("SortMode", (int) this.defaultSortMode);
                    this._showNonPublicMembers = preferencesStore.GetValue("ShowNonPublic", this.defaultShowNonPublicMembers);
                }
            }
            if (flag)
            {
                ClassViewProjectData config = (ClassViewProjectData) ConfigurationSettings.GetConfig("microsoft.matrix/classView");
                this._assemblyEntries.AddRange(config.AssemblyEntries);
                this._groups.AddRange(config.Groups);
                this._viewMode = this.defaultViewMode;
                this._sortMode = this.defaultSortMode;
                this._showNonPublicMembers = this.defaultShowNonPublicMembers;
                this.SetSaveOnExit();
            }
        }

        public void Save(IServiceProvider serviceProvider)
        {
            IPreferencesService service = (IPreferencesService) serviceProvider.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                if (this._saveOnExit)
                {
                    service.ResetPreferencesStore(typeof(ClassViewProject));
                }
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(ClassViewProject));
                preferencesStore.SetValue("ViewMode", (int) this._viewMode, (int) this.defaultViewMode);
                preferencesStore.SetValue("SortMode", (int) this._sortMode, (int) this.defaultSortMode);
                preferencesStore.SetValue("ShowNonPublic", this._showNonPublicMembers, this.defaultShowNonPublicMembers);
                if (this._saveOnExit)
                {
                    int num = 0;
                    foreach (AssemblyEntry entry in this._assemblyEntries)
                    {
                        preferencesStore.SetValue("AssemblyEntry" + num, entry);
                        num++;
                    }
                    preferencesStore.SetValue("AssemblyEntryCount", num, 0);
                    int num2 = 0;
                    foreach (GroupEntry entry2 in this._groups)
                    {
                        preferencesStore.SetValue("GroupEntry" + num2, entry2);
                        num2++;
                    }
                    preferencesStore.SetValue("GroupEntryCount", num2, 0);
                }
            }
        }

        private void SetSaveOnExit()
        {
            this._saveOnExit = true;
        }

        public ICollection AssemblyEntries
        {
            get
            {
                return this._assemblyEntries;
            }
        }

        public ICollection Groups
        {
            get
            {
                return this._groups;
            }
            set
            {
                this._groups.Clear();
                this._groups.AddRange(value);
            }
        }

        public bool SaveOnExit
        {
            get
            {
                return this._saveOnExit;
            }
        }

        public bool ShowNonPublicMembers
        {
            get
            {
                return this._showNonPublicMembers;
            }
            set
            {
                if (this._showNonPublicMembers != value)
                {
                    this._showNonPublicMembers = value;
                }
            }
        }

        public ClassViewProjectSortMode SortMode
        {
            get
            {
                return this._sortMode;
            }
            set
            {
                if (this._sortMode != value)
                {
                    this._sortMode = value;
                }
            }
        }

        public ClassViewProjectViewMode ViewMode
        {
            get
            {
                return this._viewMode;
            }
            set
            {
                if (this._viewMode != value)
                {
                    this._viewMode = value;
                }
            }
        }

        [Serializable]
        public sealed class AssemblyEntry : ISerializable
        {
            private System.Reflection.AssemblyName _assemblyName;
            private bool _gac;
            private string _locator;
            private string _name;

            public AssemblyEntry(System.Reflection.AssemblyName assemblyName)
            {
                this._assemblyName = assemblyName;
                this._name = assemblyName.Name;
                Assembly assembly = Assembly.Load(assemblyName);
                this._gac = assembly.GlobalAssemblyCache;
                this._locator = this._gac ? this._assemblyName.FullName : this._assemblyName.CodeBase.Replace("file:///", "");
            }

            private AssemblyEntry(SerializationInfo info, StreamingContext context)
            {
                this._gac = info.GetBoolean("GAC");
                this._locator = info.GetString("Locator");
                this._name = info.GetString("Name");
            }

            public AssemblyEntry(string fullName, string name)
            {
                this._locator = fullName;
                this._gac = true;
                this._name = name;
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("GAC", this._gac);
                info.AddValue("Locator", this._locator);
                info.AddValue("Name", this._name);
            }

            public Assembly Load()
            {
                return Assembly.Load(this.AssemblyName);
            }

            public System.Reflection.AssemblyName AssemblyName
            {
                get
                {
                    if (this._assemblyName == null)
                    {
                        this._assemblyName = this._gac ? Assembly.Load(this._locator).GetName() : System.Reflection.AssemblyName.GetAssemblyName(this._locator);
                    }
                    return this._assemblyName;
                }
            }

            public string Name
            {
                get
                {
                    return this._name;
                }
            }
        }

        [Serializable]
        public sealed class GroupEntry : ISerializable
        {
            private ArrayList _items;
            private string _name;

            private GroupEntry(SerializationInfo info, StreamingContext context)
            {
                this._name = info.GetString("GroupName");
                int capacity = info.GetInt32("Count");
                this._items = new ArrayList(capacity);
                for (int i = 0; i < capacity; i++)
                {
                    string name = info.GetString("Name" + i);
                    string typeName = info.GetString("TypeName" + i);
                    this._items.Add(new ClassViewProjectData.GroupItem(name, typeName));
                }
            }

            public GroupEntry(string name, ArrayList items)
            {
                this._name = name;
                this._items = items;
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("GroupName", this._name);
                int num = 0;
                foreach (ClassViewProjectData.GroupItem item in this._items)
                {
                    info.AddValue("Name" + num, item.Name);
                    info.AddValue("TypeName" + num, item.TypeName);
                    num++;
                }
                info.AddValue("Count", num);
            }

            public ICollection Items
            {
                get
                {
                    return this._items;
                }
            }

            public string Name
            {
                get
                {
                    return this._name;
                }
            }
        }

        public sealed class GroupItem
        {
            private string _name;
            private string _typeName;

            public GroupItem(string name, string typeName)
            {
                this._name = name;
                this._typeName = typeName;
            }

            public string Name
            {
                get
                {
                    return this._name;
                }
            }

            public string TypeName
            {
                get
                {
                    return this._typeName;
                }
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class AssemblyProjectItem : FolderProjectItem
    {
        private System.Reflection.Assembly _assembly;
        private ClassViewProjectData.AssemblyEntry _assemblyEntry;

        public AssemblyProjectItem(ClassViewProjectData.AssemblyEntry assemblyEntry) : base(assemblyEntry.Name)
        {
            base.SetIconIndex(2);
            this._assemblyEntry = assemblyEntry;
        }

        protected override void CreateChildItems()
        {
            System.Reflection.Assembly assembly = this.Assembly;
            if (assembly != null)
            {
                Type[] types;
                ArrayList list = new ArrayList();
                Hashtable hashtable = new Hashtable();
                if (((ClassViewProject) this.Project).ProjectData.ShowNonPublicMembers)
                {
                    types = assembly.GetTypes();
                }
                else
                {
                    types = assembly.GetExportedTypes();
                }
                int length = types.Length;
                for (int i = 0; i < length; i++)
                {
                    Type type = types[i];
                    if (type.DeclaringType == null)
                    {
                        string nameSpace = type.Namespace;
                        if (nameSpace != null)
                        {
                            NamespaceProjectItem item = (NamespaceProjectItem) hashtable[nameSpace];
                            if (item == null)
                            {
                                item = new NamespaceProjectItem(nameSpace);
                                hashtable[nameSpace] = item;
                                list.Add(item);
                            }
                            TypeProjectItem item2 = item.AddType(type);
                        }
                    }
                }
                if (list.Count != 0)
                {
                    list.Sort(new NamespaceProjectItem.NamespaceProjectItemComparer());
                    foreach (NamespaceProjectItem item3 in list)
                    {
                        base.AddChildItem(item3);
                    }
                }
            }
            base.CreateChildItems();
        }

        public System.Reflection.Assembly Assembly
        {
            get
            {
                if (this._assembly == null)
                {
                    try
                    {
                        this._assembly = this._assemblyEntry.Load();
                    }
                    catch
                    {
                    }
                }
                return this._assembly;
            }
        }

        public override string Description
        {
            get
            {
                return ("assembly\r\n" + base.Caption);
            }
        }

        public sealed class AssemblyProjectItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.Compare(((AssemblyProjectItem) x).Caption, ((AssemblyProjectItem) y).Caption);
            }
        }
    }
}


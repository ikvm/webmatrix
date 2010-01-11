namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class ClassBrowserProjectItem : RootProjectItem
    {
        public ClassBrowserProjectItem(Project owningProject) : base(owningProject, "Class Browser")
        {
            base.SetFlags(ProjectItemFlags.NotDeletable, true);
        }

        protected override void CreateChildItems()
        {
            ClassViewProjectData projectData = ((ClassViewProject) this.Project).ProjectData;
            ICollection groups = projectData.Groups;
            if (groups != null)
            {
                foreach (ClassViewProjectData.GroupEntry entry in groups)
                {
                    GroupProjectItem item = new GroupProjectItem(entry);
                    base.AddChildItem(item);
                }
            }
            ICollection assemblyEntries = projectData.AssemblyEntries;
            if (projectData.ViewMode == ClassViewProjectViewMode.Assembly)
            {
                ArrayList list = new ArrayList();
                foreach (ClassViewProjectData.AssemblyEntry entry2 in assemblyEntries)
                {
                    AssemblyProjectItem item2 = new AssemblyProjectItem(entry2);
                    list.Add(item2);
                }
                list.Sort(new AssemblyProjectItem.AssemblyProjectItemComparer());
                foreach (AssemblyProjectItem item3 in list)
                {
                    base.AddChildItem(item3);
                }
            }
            else
            {
                Hashtable hashtable = new Hashtable();
                ArrayList list2 = new ArrayList();
                foreach (ClassViewProjectData.AssemblyEntry entry3 in assemblyEntries)
                {
                    Type[] types;
                    Assembly assembly = null;
                    try
                    {
                        assembly = entry3.Load();
                    }
                    catch
                    {
                        continue;
                    }
                    if (projectData.ShowNonPublicMembers)
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
                                NamespaceProjectItem item4 = (NamespaceProjectItem) hashtable[nameSpace];
                                if (item4 == null)
                                {
                                    item4 = new NamespaceProjectItem(nameSpace);
                                    hashtable[nameSpace] = item4;
                                    list2.Add(item4);
                                }
                                item4.AddType(type);
                            }
                        }
                    }
                }
                if (list2.Count != 0)
                {
                    list2.Sort(new NamespaceProjectItem.NamespaceProjectItemComparer());
                    foreach (NamespaceProjectItem item5 in list2)
                    {
                        base.AddChildItem(item5);
                    }
                }
            }
            base.CreateChildItems();
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }
    }
}


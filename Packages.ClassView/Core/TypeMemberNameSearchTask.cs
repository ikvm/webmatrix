namespace Microsoft.Matrix.Packages.ClassView.Core
{
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class TypeMemberNameSearchTask : TypeSearchTask
    {
        private string _searchValue;

        public TypeMemberNameSearchTask(ClassViewProjectData projectData, string searchValue) : base(projectData, 1)
        {
            this._searchValue = searchValue.ToLower();
        }

        protected override void PerformSearch()
        {
            ICollection assemblies = base.GetAssemblies(null);
            base.SearchItemsCount = assemblies.Count;
            if (base.SearchItemsCount != 0)
            {
                BindingFlags flags = BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
                foreach (Assembly assembly in assemblies)
                {
                    Type[] types;
                    if (base.ProjectData.ShowNonPublicMembers)
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
                        MemberInfo member = this.TypeContainsMember(type, flags);
                        if (member != null)
                        {
                            base.AddResult(member);
                            if (base.IsCanceled)
                            {
                                break;
                            }
                        }
                    }
                    if (base.IsCanceled)
                    {
                        break;
                    }
                    base.SearchItemsProcessed++;
                    base.PostResults(false);
                }
            }
            base.PostResults(true);
        }

        private MemberInfo TypeContainsMember(Type type, BindingFlags flags)
        {
            MemberInfo[] members = null;
            try
            {
                members = type.GetMembers(flags);
            }
            catch
            {
            }
            if ((members != null) && (members.Length != 0))
            {
                foreach (MemberInfo info in members)
                {
                    MethodInfo info2 = info as MethodInfo;
                    if (((info2 == null) || !info2.IsSpecialName) && (info.Name.ToLower().IndexOf(this._searchValue) >= 0))
                    {
                        return info;
                    }
                }
            }
            return null;
        }
    }
}


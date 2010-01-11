namespace Microsoft.Matrix.Packages.ClassView.Core
{
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class TypeNameSearchTask : TypeSearchTask
    {
        private string _searchValue;
        private bool _useFullName;

        internal TypeNameSearchTask(ClassViewProjectData projectData, string searchValue) : base(projectData)
        {
            this._searchValue = searchValue.ToLower();
            this._useFullName = searchValue.IndexOf('.') >= 0;
        }

        protected override void PerformSearch()
        {
            ICollection assemblies = base.GetAssemblies(null);
            base.SearchItemsCount = assemblies.Count;
            if (base.SearchItemsCount != 0)
            {
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
                        Type member = types[i];
                        if (member.Namespace != null)
                        {
                            string str = this._useFullName ? member.FullName : member.Name;
                            if (str.ToLower().IndexOf(this._searchValue) >= 0)
                            {
                                base.AddResult(member);
                                if (base.IsCanceled)
                                {
                                    break;
                                }
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
    }
}


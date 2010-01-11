namespace Microsoft.Matrix.Packages.ClassView.Core
{
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class TypeImplementsSearchTask : TypeSearchTask
    {
        private Type _searchType;

        public TypeImplementsSearchTask(ClassViewProjectData projectData, Type searchType) : base(projectData)
        {
            this._searchType = searchType;
        }

        protected override void PerformSearch()
        {
            ICollection assemblies = base.GetAssemblies(this._searchType.Assembly);
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
                        Type c = types[i];
                        if ((c.Namespace != null) && this._searchType.IsAssignableFrom(c))
                        {
                            base.AddResult(c);
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
    }
}


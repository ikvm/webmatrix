namespace Microsoft.Matrix.Packages.ClassView.Core
{
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class TypeBaseTypeSearchTask : TypeSearchTask
    {
        private bool _directOnly;
        private Type _searchType;

        public TypeBaseTypeSearchTask(ClassViewProjectData projectData, Type searchType, bool directOnly) : base(projectData)
        {
            this._searchType = searchType;
            this._directOnly = directOnly;
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
                        if (c.Namespace != null)
                        {
                            bool flag = false;
                            if (this._directOnly)
                            {
                                flag = c.BaseType == this._searchType;
                            }
                            else
                            {
                                flag = this._searchType.IsAssignableFrom(c);
                            }
                            if (flag)
                            {
                                base.AddResult(c);
                            }
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


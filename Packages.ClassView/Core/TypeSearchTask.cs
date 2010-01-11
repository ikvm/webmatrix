namespace Microsoft.Matrix.Packages.ClassView.Core
{
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.Reflection;

    internal abstract class TypeSearchTask : AsyncTask
    {
        private ClassViewProjectData _projectData;
        private ArrayList _resultsList;
        private int _searchItemsCount;
        private int _searchItemsProcessed;
        private int _searchResultsIncrement;
        public const string NameSearchSyntax = "<TypeName> - Search for types by their name.\r\n<Namespace>.<TypeName> - Search for types by their full name.\r\n::<MemberName> - Search for types containing a matching field, property, event or method.";

        protected TypeSearchTask(ClassViewProjectData projectData) : this(projectData, 5)
        {
        }

        protected TypeSearchTask(ClassViewProjectData projectData, int searchResultsIncrement)
        {
            this._projectData = projectData;
            this._searchResultsIncrement = searchResultsIncrement;
        }

        protected void AddResult(MemberInfo member)
        {
            if (this._resultsList == null)
            {
                this._resultsList = new ArrayList();
            }
            this._resultsList.Add(member);
            if (this._resultsList.Count >= this._searchResultsIncrement)
            {
                this.PostResults(false);
            }
        }

        public static TypeSearchTask CreateReferenceSearchTask(ClassViewProjectData projectData, Type searchType, ReferenceSearchMode mode)
        {
            if (searchType != null)
            {
                switch (mode)
                {
                    case ReferenceSearchMode.BaseType:
                        return new TypeBaseTypeSearchTask(projectData, searchType, false);

                    case ReferenceSearchMode.BaseTypeDirect:
                        return new TypeBaseTypeSearchTask(projectData, searchType, true);

                    case ReferenceSearchMode.Implements:
                        return new TypeImplementsSearchTask(projectData, searchType);

                    case ReferenceSearchMode.Member:
                        return new TypeMemberTypeSearchTask(projectData, searchType);
                }
            }
            return null;
        }

        public static TypeSearchTask CreateSearchTask(ClassViewProjectData projectData, string searchValue)
        {
            if ((searchValue == null) || (searchValue.Length == 0))
            {
                return null;
            }
            if (!searchValue.StartsWith("::"))
            {
                return new TypeNameSearchTask(projectData, searchValue);
            }
            searchValue = searchValue.Substring(2);
            if (searchValue.Length == 0)
            {
                return null;
            }
            return new TypeMemberNameSearchTask(projectData, searchValue);
        }

        protected ICollection GetAssemblies(Assembly localAssembly)
        {
            ICollection assemblyEntries = this.ProjectData.AssemblyEntries;
            ArrayList list = new ArrayList(assemblyEntries.Count);
            if (localAssembly != null)
            {
                list.Add(localAssembly);
            }
            foreach (ClassViewProjectData.AssemblyEntry entry in assemblyEntries)
            {
                try
                {
                    Assembly assembly = entry.Load();
                    if (assembly != localAssembly)
                    {
                        list.Add(assembly);
                    }
                    continue;
                }
                catch
                {
                    continue;
                }
            }
            return list;
        }

        protected abstract void PerformSearch();
        protected override void PerformTask()
        {
            this.PerformSearch();
        }

        protected void PostResults(bool completed)
        {
            ArrayList data = this._resultsList;
            this._resultsList = null;
            if (!base.IsCanceled)
            {
                int num;
                if (completed)
                {
                    num = 100;
                }
                else
                {
                    num = (this.SearchItemsProcessed * 100) / this.SearchItemsCount;
                }
                base.PostResults(data, num, completed);
            }
        }

        protected ClassViewProjectData ProjectData
        {
            get
            {
                return this._projectData;
            }
        }

        protected int SearchItemsCount
        {
            get
            {
                return this._searchItemsCount;
            }
            set
            {
                this._searchItemsCount = value;
            }
        }

        protected int SearchItemsProcessed
        {
            get
            {
                return this._searchItemsProcessed;
            }
            set
            {
                this._searchItemsProcessed = value;
            }
        }
    }
}


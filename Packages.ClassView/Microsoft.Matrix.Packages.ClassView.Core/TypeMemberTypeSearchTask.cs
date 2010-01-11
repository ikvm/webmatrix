namespace Microsoft.Matrix.Packages.ClassView.Core
{
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class TypeMemberTypeSearchTask : TypeSearchTask
    {
        private Type _searchType;

        internal TypeMemberTypeSearchTask(ClassViewProjectData projectData, Type searchType) : base(projectData, 1)
        {
            this._searchType = searchType;
        }

        protected override void PerformSearch()
        {
            ICollection assemblies = base.GetAssemblies(this._searchType.Assembly);
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
                        MemberInfo member = this.TypeMemberContainsReference(type, flags);
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

        private MemberInfo TypeMemberContainsReference(Type type, BindingFlags flags)
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
                    switch (info.MemberType)
                    {
                        case MemberTypes.Constructor:
                            break;

                        case MemberTypes.Event:
                            if (((EventInfo) info).EventHandlerType != this._searchType)
                            {
                                goto Label_0116;
                            }
                            return info;

                        case MemberTypes.Field:
                            if (((FieldInfo) info).FieldType != this._searchType)
                            {
                                goto Label_0116;
                            }
                            return info;

                        case MemberTypes.Method:
                        {
                            MethodInfo info2 = (MethodInfo) info;
                            if (info2.IsSpecialName)
                            {
                                goto Label_0116;
                            }
                            if (info2.ReturnType == this._searchType)
                            {
                                return info;
                            }
                            break;
                        }
                        case MemberTypes.Property:
                            if (((PropertyInfo) info).PropertyType != this._searchType)
                            {
                                goto Label_0116;
                            }
                            return info;

                        default:
                            goto Label_0116;
                    }
                    ParameterInfo[] parameters = ((MethodBase) info).GetParameters();
                    if ((parameters != null) && (parameters.Length != 0))
                    {
                        foreach (ParameterInfo info3 in parameters)
                        {
                            if (info3.ParameterType == this._searchType)
                            {
                                return info;
                            }
                        }
                    }
                Label_0116:;
                }
            }
            return null;
        }
    }
}


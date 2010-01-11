namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;
    using System.Reflection;

    internal class TypeProjectItem : DocumentProjectItem
    {
        private TypeProjectItemClassType _classType;
        private System.Type _type;
        private TypeProjectItemVisibility _visibility;
        private const int AssemblyVisibilityIconOffset = 1;
        private const int FamilyVisibilityIconOffset = 3;
        private const int PrivateVisibilityIconOffset = 4;
        private const int PublicVisibilityIconOffset = 0;

        public TypeProjectItem(System.Type type) : this(type.Name, type)
        {
        }

        public TypeProjectItem(string alias, System.Type type) : base(alias)
        {
            if ((type.BaseType != null) || (type.GetInterfaces().Length != 0))
            {
                base.SetFlags(ProjectItemFlags.NonExpandable, false);
            }
            this._type = type;
        }

        protected override void CreateChildItems()
        {
            int num;
            BindingFlags @public = BindingFlags.Public;
            if (((ClassViewProject) this.Project).ProjectData.ShowNonPublicMembers)
            {
                @public |= BindingFlags.NonPublic;
            }
            System.Type[] nestedTypes = this._type.GetNestedTypes(@public);
            if ((nestedTypes != null) && ((num = nestedTypes.Length) != 0))
            {
                TypeProjectItem[] array = new TypeProjectItem[nestedTypes.Length];
                for (int i = 0; i < num; i++)
                {
                    array[i] = new TypeProjectItem(nestedTypes[i]);
                    array[i].EnsureTypeInformation();
                }
                Array.Sort(array, GetComparer(((ClassViewProject) this.Project).ProjectData.SortMode));
                for (int j = 0; j < num; j++)
                {
                    base.AddChildItem(array[j]);
                }
            }
            base.CreateChildItems();
        }

        public void EnsureTypeInformation()
        {
            int index = 4;
            this._classType = TypeProjectItemClassType.Class;
            if (this._type.IsEnum)
            {
                index = 14;
                this._classType = TypeProjectItemClassType.Enum;
            }
            else if (this._type.IsValueType)
            {
                index = 0x18;
                this._classType = TypeProjectItemClassType.ValueType;
            }
            else if (this._type.IsInterface)
            {
                index = 0x13;
                this._classType = TypeProjectItemClassType.Interface;
            }
            else if (this._type.IsSubclassOf(typeof(Delegate)))
            {
                index = 9;
                this._classType = TypeProjectItemClassType.Delegate;
            }
            this._visibility = TypeProjectItemVisibility.Public;
            if (this._type.DeclaringType == null)
            {
                if (this._type.IsNotPublic)
                {
                    index++;
                    this._visibility = TypeProjectItemVisibility.Assembly;
                }
            }
            else if (this._type.IsNestedFamily)
            {
                index += 3;
                this._visibility = TypeProjectItemVisibility.Family;
            }
            else if (this._type.IsNestedPrivate)
            {
                index += 4;
                this._visibility = TypeProjectItemVisibility.Private;
            }
            else if (this._type.IsNestedAssembly)
            {
                index++;
                this._visibility = TypeProjectItemVisibility.Assembly;
            }
            base.SetIconIndex(index);
        }

        public static IComparer GetComparer(ClassViewProjectSortMode sortMode)
        {
            switch (sortMode)
            {
                case ClassViewProjectSortMode.ByClassType:
                    return new TypeProjectItemClassTypeComparer();

                case ClassViewProjectSortMode.ByClassVisibility:
                    return new TypeProjectItemVisibilityComparer();
            }
            return new TypeProjectItemNameComparer();
        }

        public TypeProjectItemClassType ClassType
        {
            get
            {
                return this._classType;
            }
        }

        public override string Description
        {
            get
            {
                string str;
                string str2;
                switch (this._visibility)
                {
                    case TypeProjectItemVisibility.Family:
                        str = "protected ";
                        break;

                    case TypeProjectItemVisibility.Assembly:
                        str = "internal ";
                        break;

                    case TypeProjectItemVisibility.Private:
                        str = "private ";
                        break;

                    default:
                        str = "public ";
                        break;
                }
                switch (this._classType)
                {
                    case TypeProjectItemClassType.ValueType:
                        str2 = "struct";
                        break;

                    case TypeProjectItemClassType.Interface:
                        str2 = "interface";
                        break;

                    case TypeProjectItemClassType.Enum:
                        str2 = "enum";
                        break;

                    case TypeProjectItemClassType.Delegate:
                        str2 = "delegate";
                        break;

                    default:
                        str2 = "class";
                        break;
                }
                return (str + str2 + "\r\n" + this._type.FullName);
            }
        }

        protected virtual bool IncludeBaseTypesItem
        {
            get
            {
                return true;
            }
        }

        public System.Type Type
        {
            get
            {
                return this._type;
            }
        }

        public TypeProjectItemVisibility Visibility
        {
            get
            {
                return this._visibility;
            }
        }

        private sealed class TypeProjectItemClassTypeComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                TypeProjectItem item = (TypeProjectItem) x;
                TypeProjectItem item2 = (TypeProjectItem) y;
                if (item.ClassType == item2.ClassType)
                {
                    return string.Compare(item.Type.Name, item2.Type.Name);
                }
                return (int) (item.ClassType - item2.ClassType);
            }
        }

        private sealed class TypeProjectItemNameComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.Compare(((TypeProjectItem) x).Type.Name, ((TypeProjectItem) y).Type.Name);
            }
        }

        private sealed class TypeProjectItemVisibilityComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                TypeProjectItem item = (TypeProjectItem) x;
                TypeProjectItem item2 = (TypeProjectItem) y;
                if (item.Visibility == item2.Visibility)
                {
                    return string.Compare(item.Type.Name, item2.Type.Name);
                }
                return (int) (item.Visibility - item2.Visibility);
            }
        }
    }
}


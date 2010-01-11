namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using System;

    internal sealed class BaseTypeProjectItem : TypeProjectItem
    {
        public BaseTypeProjectItem(Type type) : base(type)
        {
        }

        protected override void CreateChildItems()
        {
            Type type = base.Type;
            if (!type.IsInterface)
            {
                BaseTypeProjectItem item = new BaseTypeProjectItem(type.BaseType);
                item.EnsureTypeInformation();
                base.AddChildItem(item);
                Type[] interfaces = type.GetInterfaces();
                int num = 0;
                int num2 = 0;
                if ((interfaces != null) && ((num = interfaces.Length) != 0))
                {
                    num2 = num;
                    Type[] typeArray2 = null;
                    int num3 = 0;
                    Type baseType = type.BaseType;
                    if (baseType != null)
                    {
                        typeArray2 = baseType.GetInterfaces();
                    }
                    if ((typeArray2 != null) && ((num3 = typeArray2.Length) != 0))
                    {
                        for (int i = 0; i < num; i++)
                        {
                            for (int j = 0; j < num3; j++)
                            {
                                if (interfaces[i] == typeArray2[j])
                                {
                                    interfaces[i] = null;
                                    num2--;
                                    goto Label_00A0;
                                }
                            }
                        Label_00A0:;
                        }
                    }
                    if (num2 != 0)
                    {
                        for (int k = 0; k < num; k++)
                        {
                            Type type3 = interfaces[k];
                            if (type3 != null)
                            {
                                typeArray2 = type3.GetInterfaces();
                                if ((typeArray2 != null) && ((num3 = typeArray2.Length) != 0))
                                {
                                    for (int m = 0; m < num; m++)
                                    {
                                        for (int n = 0; n < num3; n++)
                                        {
                                            if (interfaces[m] == typeArray2[n])
                                            {
                                                interfaces[m] = null;
                                                num2--;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (num2 != 0)
                {
                    BaseTypeProjectItem[] array = new BaseTypeProjectItem[num2];
                    int index = 0;
                    for (int num10 = 0; num10 < num; num10++)
                    {
                        if (interfaces[num10] != null)
                        {
                            array[index] = new BaseTypeProjectItem(interfaces[num10]);
                            array[index].EnsureTypeInformation();
                            index++;
                        }
                    }
                    Array.Sort(array, TypeProjectItem.GetComparer(((ClassViewProject) this.Project).ProjectData.SortMode));
                    for (int num11 = 0; num11 < num2; num11++)
                    {
                        base.AddChildItem(array[num11]);
                    }
                }
            }
            else
            {
                Type[] typeArray3 = type.GetInterfaces();
                if ((typeArray3 != null) && (typeArray3.Length != 0))
                {
                    BaseTypeProjectItem item2 = new BaseTypeProjectItem(typeArray3[0]);
                    item2.EnsureTypeInformation();
                    base.AddChildItem(item2);
                }
            }
            base.CreateChildItems();
        }

        protected override bool IncludeBaseTypesItem
        {
            get
            {
                return false;
            }
        }
    }
}


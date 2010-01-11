namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Packages.ClassView.Documents;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    internal sealed class OutlineView : MxRichTextBox
    {
        private int _codeFontIndex;
        private int _commentColorIndex;
        private bool _ignoreColor;
        private int _keywordColorIndex;
        private int _obsoleteColorIndex;
        private System.Type _type;
        private RtfTextWriter _writer;

        public OutlineView(IServiceProvider serviceProvider)
        {
            base.ReadOnly = true;
            base.BorderStyle = BorderStyle.None;
            base.WordWrap = false;
            base.ShowSelectionMargin = true;
        }

        public void AddEnumField(FieldItem item)
        {
            FieldInfo member = (FieldInfo) item.Member;
            this._writer.Write("        ");
            if (item.IsObsolete)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._obsoleteColorIndex);
                this._ignoreColor = true;
            }
            this._writer.Write(member.Name);
            if (item.IsObsolete)
            {
                this._writer.WriteEndGroup();
                this._ignoreColor = false;
            }
            this._writer.WriteLine(",");
        }

        public void AddEvent(EventItem item)
        {
            EventInfo member = (EventInfo) item.Member;
            MethodInfo underlyingMethod = item.UnderlyingMethod;
            this._writer.Write("        ");
            if (item.IsObsolete)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._obsoleteColorIndex);
                this._ignoreColor = true;
            }
            this.GenerateMethodModifiers(underlyingMethod, false);
            this.GenerateColoredText("event ", this._keywordColorIndex);
            this._writer.Write(member.EventHandlerType.Name);
            this._writer.Write(" ");
            this._writer.Write(member.Name);
            this._writer.Write(" { ");
            if (member.GetAddMethod(true) != null)
            {
                this.GenerateColoredText("add", this._keywordColorIndex);
                this._writer.Write("; ");
            }
            if (member.GetRemoveMethod(true) != null)
            {
                this.GenerateColoredText("remove", this._keywordColorIndex);
                this._writer.Write("; ");
            }
            this._writer.WriteLine("}");
            if (item.IsObsolete)
            {
                this._writer.WriteEndGroup();
                this._ignoreColor = false;
            }
        }

        public void AddField(FieldItem item)
        {
            FieldInfo member = (FieldInfo) item.Member;
            this._writer.Write("        ");
            if (item.IsObsolete)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._obsoleteColorIndex);
                this._ignoreColor = true;
            }
            else
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._keywordColorIndex);
            }
            if (member.IsFamilyAndAssembly || member.IsFamilyOrAssembly)
            {
                this._writer.Write("protected internal ");
            }
            else if (member.IsFamily)
            {
                this._writer.Write("protected ");
            }
            else if (member.IsAssembly)
            {
                this._writer.Write("internal ");
            }
            else if (member.IsPrivate)
            {
                this._writer.Write("private ");
            }
            else
            {
                this._writer.Write("public ");
            }
            if (member.IsLiteral)
            {
                this._writer.Write("const ");
            }
            else if (member.IsStatic)
            {
                this._writer.Write("static ");
            }
            if (member.IsInitOnly)
            {
                this._writer.Write("readonly ");
            }
            if (!item.IsObsolete)
            {
                this._writer.WriteEndGroup();
            }
            this.GenerateTypeReference(member.FieldType, false, false);
            this._writer.Write(" ");
            this._writer.Write(member.Name);
            this._writer.WriteLine(";");
            if (item.IsObsolete)
            {
                this._writer.WriteEndGroup();
                this._ignoreColor = false;
            }
        }

        public void AddMethod(MethodItem item)
        {
            MethodBase member = (MethodBase) item.Member;
            this._writer.Write("        ");
            if (item.IsObsolete)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._obsoleteColorIndex);
                this._ignoreColor = true;
            }
            this.GenerateMethodModifiers(member, item.IsConstructor);
            if (!item.IsConstructor)
            {
                MethodInfo info = (MethodInfo) member;
                if (info.ReturnType == typeof(void))
                {
                    this._writer.WriteBeginGroup();
                    this._writer.WriteForeColorAttribute(this._keywordColorIndex);
                    this._writer.Write("void ");
                    this._writer.WriteEndGroup();
                }
                else
                {
                    this.GenerateTypeReference(info.ReturnType, false, false);
                    this._writer.Write(" ");
                }
                this._writer.Write(info.Name);
            }
            else
            {
                this._writer.Write(this._type.Name);
            }
            this._writer.Write("(");
            ParameterInfo[] parameters = member.GetParameters();
            if (parameters != null)
            {
                this.GenerateParamList(parameters);
            }
            this._writer.WriteLine(");");
            if (item.IsObsolete)
            {
                this._writer.WriteEndGroup();
                this._ignoreColor = false;
            }
        }

        public void AddProperty(PropertyItem item)
        {
            PropertyInfo member = (PropertyInfo) item.Member;
            MethodInfo underlyingMethod = item.UnderlyingMethod;
            this._writer.Write("        ");
            if (item.IsObsolete)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._obsoleteColorIndex);
                this._ignoreColor = true;
            }
            this.GenerateMethodModifiers(underlyingMethod, false);
            this.GenerateTypeReference(member.PropertyType, false, false);
            this._writer.Write(" ");
            this._writer.Write(member.Name);
            ParameterInfo[] indexParameters = member.GetIndexParameters();
            if ((indexParameters != null) && (indexParameters.Length != 0))
            {
                this._writer.Write("[");
                this.GenerateParamList(indexParameters);
                this._writer.Write("]");
            }
            this._writer.Write(" { ");
            if (member.GetGetMethod(true) != null)
            {
                this.GenerateColoredText("get", this._keywordColorIndex);
                this._writer.Write("; ");
            }
            if (member.GetSetMethod(true) != null)
            {
                this.GenerateColoredText("set", this._keywordColorIndex);
                this._writer.Write("; ");
            }
            this._writer.WriteLine("}");
            if (item.IsObsolete)
            {
                this._writer.WriteEndGroup();
                this._ignoreColor = false;
            }
        }

        public void BeginOutline(System.Type type)
        {
            base.Clear();
            this._type = type;
            StringBuilder sb = new StringBuilder(0x400);
            this._writer = new RtfTextWriter(new StringWriter(sb));
            this._writer.WriteBeginRtf();
            this._writer.WriteBeginFontTable();
            this._codeFontIndex = this._writer.WriteFont(new Font("Lucida Console", 8f));
            this._writer.WriteEndFontTable();
            this._writer.WriteBeginColorTable();
            this._commentColorIndex = this._writer.WriteColor(Color.Green);
            this._keywordColorIndex = this._writer.WriteColor(Color.Blue);
            this._obsoleteColorIndex = this._writer.WriteColor(Color.Gray);
            this._writer.WriteEndColorTable();
            this._writer.WriteFontAttribute(this._codeFontIndex, 8);
            this.GenerateHeader();
        }

        public void EndOutline()
        {
            this.GenerateFooter();
            this._writer.WriteEndRtf();
            base.Rtf = this._writer.ToString();
        }

        private void GenerateColoredText(string text, int colorIndex)
        {
            if (!this._ignoreColor)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(colorIndex);
            }
            this._writer.Write(text);
            if (!this._ignoreColor)
            {
                this._writer.WriteEndGroup();
            }
        }

        private void GenerateFooter()
        {
            this._writer.WriteLine("    }");
            this._writer.WriteLine("}");
            this._writer.WriteLine();
        }

        private void GenerateHeader()
        {
            this._writer.WriteLine();
            this.GenerateColoredText("namespace ", this._keywordColorIndex);
            this._writer.WriteLine(this._type.Namespace + " {");
            this._writer.WriteLine();
            this._writer.Write("    ");
            this._writer.WriteBeginGroup();
            this._writer.WriteForeColorAttribute(this._keywordColorIndex);
            if (this._type.DeclaringType != null)
            {
                if (this._type.IsNestedFamANDAssem)
                {
                    this._writer.Write("protected internal ");
                }
                else if (this._type.IsNestedAssembly)
                {
                    this._writer.Write("internal ");
                }
                else if (this._type.IsNestedFamily)
                {
                    this._writer.Write("protected ");
                }
                else if (this._type.IsNestedPrivate)
                {
                    this._writer.Write("private ");
                }
                else if (this._type.IsNestedPublic)
                {
                    this._writer.Write("internal ");
                }
            }
            else if (this._type.IsPublic)
            {
                this._writer.Write("public ");
            }
            else
            {
                this._writer.Write("internal ");
            }
            if (this._type.IsClass)
            {
                if (this._type.IsSealed)
                {
                    this._writer.Write("sealed ");
                }
                else if (this._type.IsAbstract)
                {
                    this._writer.Write("abstract ");
                }
                this._writer.Write("class ");
            }
            else if (this._type.IsEnum)
            {
                this._writer.Write("enum ");
            }
            else if (this._type.IsInterface)
            {
                this._writer.Write("interface ");
            }
            else if (this._type.IsValueType)
            {
                this._writer.Write("struct ");
            }
            this._writer.WriteEndGroup();
            this._writer.Write(this._type.Name);
            if ((!this._type.IsEnum && !this._type.IsValueType) && (this._type != typeof(object)))
            {
                bool flag = false;
                System.Type[] interfaces = this._type.GetInterfaces();
                if ((this._type.BaseType != typeof(object)) && (this._type.BaseType != null))
                {
                    flag = true;
                    System.Type[] typeArray2 = this._type.BaseType.GetInterfaces();
                    if ((typeArray2 != null) && (typeArray2.Length != 0))
                    {
                        ArrayList list = new ArrayList();
                        foreach (System.Type type in interfaces)
                        {
                            foreach (System.Type type2 in typeArray2)
                            { 
                                if(type.Equals(type2))
                                {
                                    list.Add(type);
                                    break;
                                }
                            }

                            //NOTE: ÊÖ¶¯ÐÞ¸Ä
                            //if (!typeArray2.Contains(type))
                            //{
                            //    list.Add(type);
                            //}
                        }
                        if (list.Count != 0)
                        {
                            interfaces = (System.Type[]) list.ToArray(typeof(System.Type));
                        }
                        else
                        {
                            interfaces = null;
                        }
                    }
                }
                if (flag || (interfaces.Length != 0))
                {
                    bool flag2 = false;
                    this._writer.Write(" : ");
                    if (flag)
                    {
                        this._writer.Write(this._type.BaseType.Name);
                        flag2 = true;
                    }
                    if ((interfaces != null) && (interfaces.Length != 0))
                    {
                        foreach (System.Type type2 in interfaces)
                        {
                            if (flag2)
                            {
                                this._writer.Write(", ");
                            }
                            this._writer.Write(type2.Name);
                            flag2 = true;
                        }
                    }
                }
            }
            this._writer.WriteLine(" {");
        }

        private void GenerateMethodModifiers(MethodBase mb, bool isCtor)
        {
            if (!this._ignoreColor)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._keywordColorIndex);
            }
            if (mb.IsFamilyAndAssembly || mb.IsFamilyOrAssembly)
            {
                this._writer.Write("protected internal ");
            }
            else if (mb.IsFamily)
            {
                this._writer.Write("protected ");
            }
            else if (mb.IsAssembly)
            {
                this._writer.Write("internal ");
            }
            else if (mb.IsPrivate)
            {
                this._writer.Write("private ");
            }
            else if (!this._type.IsInterface)
            {
                this._writer.Write("public ");
            }
            if ((!isCtor && mb.IsVirtual) && (mb.DeclaringType != ((MethodInfo) mb).GetBaseDefinition().DeclaringType))
            {
                if (mb.IsFinal)
                {
                    this._writer.Write("sealed ");
                }
                else if (mb.IsAbstract)
                {
                    this._writer.Write("abstract ");
                }
                this._writer.Write("override ");
            }
            else if (mb.IsAbstract)
            {
                if (!mb.DeclaringType.IsInterface)
                {
                    this._writer.Write("abstract ");
                }
            }
            else if (mb.IsStatic)
            {
                this._writer.Write("static ");
            }
            else if (mb.IsVirtual && !mb.IsFinal)
            {
                this._writer.Write("virtual ");
            }
            if (!this._ignoreColor)
            {
                this._writer.WriteEndGroup();
            }
        }

        private void GenerateParamList(ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo info = parameters[i];
                if (i != 0)
                {
                    this._writer.Write(", ");
                }
                this.GenerateTypeReference(info.ParameterType, true, info.IsOut);
                this._writer.Write(" ");
                this._writer.Write(info.Name);
            }
        }

        private void GenerateTypeReference(System.Type type, bool isParam, bool isOutParam)
        {
            bool flag = false;
            string name = null;
            if (type.IsByRef && isParam)
            {
                if (!this._ignoreColor)
                {
                    this._writer.WriteBeginGroup();
                    this._writer.WriteForeColorAttribute(this._keywordColorIndex);
                }
                if (isOutParam)
                {
                    this._writer.Write("out ");
                }
                else
                {
                    this._writer.Write("ref ");
                }
                if (!this._ignoreColor)
                {
                    this._writer.WriteEndGroup();
                }
            }
            System.Type elementType = type;
            while (!elementType.IsEnum && (elementType.GetElementType() != null))
            {
                elementType = elementType.GetElementType();
            }
            if (!elementType.IsEnum)
            {
                switch (System.Type.GetTypeCode(elementType))
                {
                    case TypeCode.Object:
                        if (elementType == typeof(object))
                        {
                            name = "object";
                        }
                        break;

                    case TypeCode.Boolean:
                        name = "bool";
                        break;

                    case TypeCode.Char:
                        name = "char";
                        break;

                    case TypeCode.SByte:
                        name = "sbyte";
                        break;

                    case TypeCode.Byte:
                        name = "byte";
                        break;

                    case TypeCode.Int16:
                        name = "short";
                        break;

                    case TypeCode.UInt16:
                        name = "ushort";
                        break;

                    case TypeCode.Int32:
                        name = "int";
                        break;

                    case TypeCode.UInt32:
                        name = "uint";
                        break;

                    case TypeCode.Int64:
                        name = "long";
                        break;

                    case TypeCode.UInt64:
                        name = "ulong";
                        break;

                    case TypeCode.Single:
                        name = "float";
                        break;

                    case TypeCode.Double:
                        name = "double";
                        break;

                    case TypeCode.String:
                        name = "string";
                        break;
                }
                if (elementType == typeof(void))
                {
                    name = "void";
                }
            }
            if (name != null)
            {
                flag = true;
            }
            else
            {
                name = elementType.Name;
            }
            if (flag && !this._ignoreColor)
            {
                this._writer.WriteBeginGroup();
                this._writer.WriteForeColorAttribute(this._keywordColorIndex);
            }
            this._writer.Write(name);
            if (flag && !this._ignoreColor)
            {
                this._writer.WriteEndGroup();
            }
            if (elementType != type)
            {
                string str2 = string.Empty;
                int count = 0;
                System.Type type3 = type;
                do
                {
                    if (type3.IsPointer)
                    {
                        count++;
                    }
                    if (type3.IsArray)
                    {
                        int arrayRank = type3.GetArrayRank();
                        if (arrayRank == 1)
                        {
                            str2 = str2 + "[]";
                        }
                        else
                        {
                            str2 = (str2 + "[") + new string(',', arrayRank - 1) + "]";
                        }
                    }
                    type3 = type3.GetElementType();
                }
                while ((type3 != null) && ((type3.IsArray || type3.IsPointer) || type3.IsByRef));
                if (count != 0)
                {
                    this._writer.Write(new string('*', count));
                }
                this._writer.Write(str2);
            }
            if (type.IsByRef && !isParam)
            {
                this._writer.Write("&");
            }
        }

        public void StartConstructors()
        {
            this._writer.WriteLine();
            this.GenerateColoredText("        // Constructors", this._commentColorIndex);
            this._writer.WriteLine();
        }

        public void StartEvents()
        {
            this._writer.WriteLine();
            this.GenerateColoredText("        // Events", this._commentColorIndex);
            this._writer.WriteLine();
        }

        public void StartFields()
        {
            this._writer.WriteLine();
            this.GenerateColoredText("        // Fields", this._commentColorIndex);
            this._writer.WriteLine();
        }

        public void StartMethods()
        {
            this._writer.WriteLine();
            this.GenerateColoredText("        // Methods", this._commentColorIndex);
            this._writer.WriteLine();
        }

        public void StartProperties()
        {
            this._writer.WriteLine();
            this.GenerateColoredText("        // Properties", this._commentColorIndex);
            this._writer.WriteLine();
        }
    }
}


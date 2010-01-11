namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Packages.ClassView.Documents;
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Drawing;
    using System.Reflection;

    internal sealed class MemberPageBuilder : ClassViewPageBuilder
    {
        private TypeDocumentItem _item;

        public MemberPageBuilder(ClassViewProjectData projectData, TypeDocument document, TypeDocumentItem item) : base(projectData, document)
        {
            this._item = item;
        }

        protected override void GenerateDeclaration()
        {
            base.BeginNewSection(false).FontSize = 10f;
            switch (this.CurrentMember.MemberType)
            {
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                    this.GenerateMethodDeclaration();
                    break;

                case MemberTypes.Event:
                    this.GenerateEventDeclaration();
                    break;

                case MemberTypes.Field:
                    this.GenerateFieldDeclaration();
                    break;

                case MemberTypes.Property:
                    this.GeneratePropertyDeclaration();
                    break;
            }
            base.EndCurrentSection();
            try
            {
                object[] customAttributes = this.CurrentMember.GetCustomAttributes(typeof(ObsoleteAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length != 0))
                {
                    ObsoleteAttribute attribute = (ObsoleteAttribute) customAttributes[0];
                    string message = attribute.Message;
                    bool isError = attribute.IsError;
                    base.BeginNewSection(false);
                    base.AddImage(isError ? ClassViewPageBuilder.ErrorGlyph : ClassViewPageBuilder.WarningGlyph).Padding = new BoxEdges(0, 0, 4, 0);
                    base.PushForeColor(Color.Red);
                    base.AddText("This member is marked as obsolete.");
                    base.PopForeColor();
                    if ((message != null) && (message.Length != 0))
                    {
                        base.AddLineBreak();
                        base.AddText("       ").PreserveLeadingSpace = true;
                        base.AddText(message);
                    }
                    base.EndCurrentSection();
                }
            }
            catch
            {
            }
        }

        protected override void GenerateDocumentation()
        {
            base.GenerateExpandableHeader("Documentation", 0, true, "Documentation", false);
            base.BeginNewSection(false, 15, "Documentation").Visible = false;
            base.EndCurrentSection();
        }

        private void GenerateEventDeclaration()
        {
            EventInfo currentMember = (EventInfo) this.CurrentMember;
            MethodInfo addMethod = currentMember.GetAddMethod(true);
            MethodInfo removeMethod = currentMember.GetRemoveMethod(true);
            this.GenerateMethodModifiers((addMethod != null) ? addMethod : removeMethod);
            base.AddText("event ");
            base.GenerateTypeReference(currentMember.EventHandlerType);
            base.GenerateDeclarationName(currentMember.Name, true);
            base.AddText(" { ");
            base.AddLineBreak();
            base.AddText("    ").PreserveLeadingSpace = true;
            if (addMethod != null)
            {
                base.AddText("add; ");
            }
            if (removeMethod != null)
            {
                base.AddText("remove;");
            }
            base.AddLineBreak();
            base.AddText("}");
        }

        private void GenerateFieldDeclaration()
        {
            FieldInfo currentMember = (FieldInfo) this.CurrentMember;
            if (currentMember.IsFamilyAndAssembly || currentMember.IsFamilyOrAssembly)
            {
                base.AddText("protected internal ");
            }
            else if (currentMember.IsFamily)
            {
                base.AddText("protected ");
            }
            else if (currentMember.IsAssembly)
            {
                base.AddText("internal ");
            }
            else if (currentMember.IsPrivate)
            {
                base.AddText("private ");
            }
            else
            {
                base.AddText("public ");
            }
            if (currentMember.IsLiteral)
            {
                base.AddText("const ");
            }
            else if (currentMember.IsStatic)
            {
                base.AddText("static ");
            }
            if (currentMember.IsInitOnly)
            {
                base.AddText("readonly ");
            }
            base.GenerateTypeReference(currentMember.FieldType);
            base.GenerateDeclarationName(currentMember.Name, true);
        }

        protected override void GenerateHelpLinks()
        {
            string name = this.CurrentMember.Name;
            if (this.CurrentMember.MemberType == MemberTypes.Constructor)
            {
                name = "ctor";
            }
            string typeName = this.CurrentMember.DeclaringType.FullName.Replace(".", string.Empty);
            if (base.CurrentType.DeclaringType != null)
            {
                typeName = typeName.Replace("+", string.Empty);
            }
            base.BeginNewSection(false);
            string userData = base.CreateOnlineHelpUrl(typeName, name);
            HyperLinkElement element = base.AddHyperLink("Online MSDN Documentation", userData);
            element.HoverColor = Color.Red;
            element.LinkColor = Color.Blue;
            base.AddText(" | ");
            userData = base.CreateLocalHelpUrl(typeName, name);
            element = base.AddHyperLink("Local SDK Documentation", userData);
            element.HoverColor = Color.Red;
            element.LinkColor = Color.Blue;
            base.EndCurrentSection();
        }

        protected override bool GenerateInformation()
        {
            Type declaringType = this.CurrentMember.DeclaringType;
            bool flag = declaringType != base.CurrentType;
            Type type = null;
            bool flag2 = false;
            if (!flag)
            {
                MethodInfo currentMember = this.CurrentMember as MethodInfo;
                if (currentMember == null)
                {
                    if (this.CurrentMember.MemberType == MemberTypes.Property)
                    {
                        currentMember = ((PropertyItem) this._item).UnderlyingMethod;
                    }
                    else if (this.CurrentMember.MemberType == MemberTypes.Event)
                    {
                        currentMember = ((EventItem) this._item).UnderlyingMethod;
                    }
                }
                if ((currentMember != null) && currentMember.IsVirtual)
                {
                    type = currentMember.GetBaseDefinition().DeclaringType;
                    flag2 = declaringType != type;
                }
            }
            object[] customAttributes = this.CurrentMember.GetCustomAttributes(true);
            bool flag3 = (customAttributes != null) && (customAttributes.Length != 0);
            if ((!flag && !flag2) && !flag3)
            {
                return false;
            }
            base.BeginNewSection(false);
            TextElement element1 = base.AddText("Member Information");
            element1.FontStyle |= FontStyle.Bold;
            base.EndCurrentSection();
            if (flag)
            {
                base.BeginNewSection(false, 15);
                base.AddText("Inherited from ");
                base.GenerateTypeReference(declaringType);
                base.EndCurrentSection();
            }
            if (flag2)
            {
                base.BeginNewSection(false, 15);
                base.AddText("Declared in ");
                base.GenerateTypeReference(type);
                base.EndCurrentSection();
            }
            if (flag3)
            {
                base.GenerateAttributes(customAttributes);
            }
            return true;
        }

        private void GenerateMethodDeclaration()
        {
            MethodBase currentMember = (MethodBase) this.CurrentMember;
            MethodInfo info = currentMember as MethodInfo;
            this.GenerateMethodModifiers(currentMember);
            if (info != null)
            {
                base.GenerateTypeReference(info.ReturnType);
                base.GenerateDeclarationName(info.Name, true);
            }
            else
            {
                base.GenerateDeclarationName(currentMember.DeclaringType.Name, false);
            }
            base.AddText("(");
            ParameterInfo[] parameters = currentMember.GetParameters();
            if ((parameters != null) && (parameters.Length != 0))
            {
                this.GenerateParamList(parameters, true);
            }
            base.AddText(");");
        }

        private void GenerateMethodModifiers(MethodBase mb)
        {
            if (mb.IsFamilyAndAssembly || mb.IsFamilyOrAssembly)
            {
                base.AddText("protected internal ");
            }
            else if (mb.IsFamily)
            {
                base.AddText("protected ");
            }
            else if (mb.IsAssembly)
            {
                base.AddText("internal ");
            }
            else if (mb.IsPrivate)
            {
                base.AddText("private ");
            }
            else if (!mb.DeclaringType.IsInterface)
            {
                base.AddText("public ");
            }
            if (((mb is MethodInfo) && mb.IsVirtual) && (mb.DeclaringType != ((MethodInfo) mb).GetBaseDefinition().DeclaringType))
            {
                if (mb.IsFinal)
                {
                    base.AddText("sealed ");
                }
                else if (mb.IsAbstract)
                {
                    base.AddText("abstract ");
                }
                base.AddText("override ");
            }
            else if (mb.IsAbstract)
            {
                if (!mb.DeclaringType.IsInterface)
                {
                    base.AddText("abstract ");
                }
            }
            else if (mb.IsStatic)
            {
                base.AddText("static ");
            }
            else if (mb.IsVirtual && !mb.IsFinal)
            {
                base.AddText("virtual ");
            }
        }

        private void GenerateParamList(ParameterInfo[] parameters, bool lineBreaks)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo info = parameters[i];
                if (lineBreaks)
                {
                    base.AddLineBreak();
                    base.AddText("    ").PreserveLeadingSpace = true;
                }
                base.GenerateTypeReference(info.ParameterType);
                base.AddText(" ");
                base.AddText(info.Name);
                if (i < (parameters.Length - 1))
                {
                    base.AddText(",");
                }
            }
        }

        private void GeneratePropertyDeclaration()
        {
            PropertyInfo currentMember = (PropertyInfo) this.CurrentMember;
            MethodInfo getMethod = currentMember.GetGetMethod(true);
            MethodInfo setMethod = currentMember.GetSetMethod(true);
            this.GenerateMethodModifiers((getMethod != null) ? getMethod : setMethod);
            base.GenerateTypeReference(currentMember.PropertyType);
            base.GenerateDeclarationName(currentMember.Name, true);
            ParameterInfo[] indexParameters = currentMember.GetIndexParameters();
            if ((indexParameters != null) && (indexParameters.Length != 0))
            {
                base.AddText("[");
                this.GenerateParamList(indexParameters, false);
                base.AddText("]");
            }
            base.AddText(" { ");
            base.AddLineBreak();
            base.AddText("    ").PreserveLeadingSpace = true;
            if (getMethod != null)
            {
                base.AddText("get; ");
            }
            if (setMethod != null)
            {
                base.AddText("set;");
            }
            base.AddLineBreak();
            base.AddText("}");
        }

        protected override void InitializePage(Page page)
        {
            base.InitializePage(page);
            ((MemberPage) page).SetItem(this._item);
        }

        private MemberInfo CurrentMember
        {
            get
            {
                return this._item.Member;
            }
        }
    }
}


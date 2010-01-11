namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Packages.ClassView.Core;
    using Microsoft.Matrix.Packages.ClassView.Documents;
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Collections;
    using System.Drawing;

    internal sealed class TypePageBuilder : ClassViewPageBuilder
    {
        public TypePageBuilder(ClassViewProjectData projectData, TypeDocument document) : base(projectData, document)
        {
        }

        protected override void GenerateDeclaration()
        {
            base.BeginNewSection(false).FontSize = 10f;
            Type declaringType = base.CurrentType.DeclaringType;
            bool flag = declaringType != null;
            string text = string.Empty;
            if (flag)
            {
                if (base.CurrentType.IsNestedFamANDAssem || base.CurrentType.IsNestedFamORAssem)
                {
                    text = "protected internal ";
                }
                else if (base.CurrentType.IsNestedAssembly)
                {
                    text = "internal ";
                }
                else if (base.CurrentType.IsNestedFamily)
                {
                    text = "protected ";
                }
                else if (base.CurrentType.IsNestedPrivate)
                {
                    text = "private ";
                }
                else if (base.CurrentType.IsNestedPublic)
                {
                    text = "internal ";
                }
            }
            else if (base.CurrentType.IsPublic)
            {
                text = "public ";
            }
            else
            {
                text = "internal ";
            }
            if (base.CurrentType.IsClass)
            {
                if (base.CurrentType.IsSealed)
                {
                    text = text + "sealed ";
                }
                text = text + "class ";
            }
            else if (base.CurrentType.IsEnum)
            {
                text = text + "enum ";
            }
            else if (base.CurrentType.IsInterface)
            {
                text = text + "interface ";
            }
            else if (base.CurrentType.IsValueType)
            {
                text = text + "struct ";
            }
            base.AddTextSpan(text);
            if (flag)
            {
                base.GenerateTypeReference(declaringType);
                base.AddText(".");
            }
            base.GenerateDeclarationName(base.CurrentType.Name, false);
            base.EndCurrentSection();
            try
            {
                object[] customAttributes = base.CurrentType.GetCustomAttributes(typeof(ObsoleteAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length != 0))
                {
                    ObsoleteAttribute attribute = (ObsoleteAttribute) customAttributes[0];
                    string message = attribute.Message;
                    bool isError = attribute.IsError;
                    base.BeginNewSection(false);
                    base.AddImage(isError ? ClassViewPageBuilder.ErrorGlyph : ClassViewPageBuilder.WarningGlyph).Padding = new BoxEdges(0, 0, 4, 0);
                    base.PushForeColor(Color.Red);
                    base.AddText("This type is marked as obsolete.");
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

        protected override void GenerateHelpLinks()
        {
            string typeName = base.CurrentType.FullName.Replace(".", string.Empty);
            if (base.CurrentType.DeclaringType != null)
            {
                typeName = typeName.Replace("+", string.Empty);
            }
            base.BeginNewSection(false);
            string userData = base.CreateOnlineHelpUrl(typeName, string.Empty);
            base.AddLink("Online MSDN Documentation", userData);
            base.AddText(" | ");
            userData = base.CreateLocalHelpUrl(typeName, string.Empty);
            base.AddLink("Local SDK Documentation", userData);
            base.EndCurrentSection();
        }

        protected override bool GenerateInformation()
        {
            base.BeginNewSection(false);
            TextElement element1 = base.AddText("Type Information");
            element1.FontStyle |= FontStyle.Bold;
            base.EndCurrentSection();
            LabelElement element = new LabelElement("Namespace:");
            LabelElement element3 = new LabelElement("Assembly:");
            element.Width = 70;
            element3.Width = 70;
            base.BeginNewSection(false, 15);
            base.AddElement(element, null, true);
            base.AddText(base.CurrentType.Namespace);
            base.AddLineBreak();
            base.AddElement(element3, null, true);
            base.AddText(base.CurrentType.Assembly.GetName().Name);
            base.EndCurrentSection();
            if (!base.CurrentType.IsEnum && (base.CurrentType != typeof(object)))
            {
                Element element4 = base.BeginNewSection(false, 15);
                base.AddText("Hierarchy");
                base.EndCurrentSection();
                Element element5 = base.BeginNewSection(false, 30, "Hierarchy");
                ArrayList list = new ArrayList();
                IList list2 = null;
                bool flag = false;
                for (Type type = base.CurrentType; type != null; type = type.BaseType)
                {
                    list.Add(type);
                }
                list.Reverse();
                string text = string.Empty;
                foreach (Type type2 in list)
                {
                    bool flag2 = true;
                    if (text.Length != 0)
                    {
                        flag = true;
                        base.AddLineBreak();
                        base.AddText(text).PreserveLeadingSpace = true;
                    }
                    if (type2 == base.CurrentType)
                    {
                        base.AddText(type2.Name);
                    }
                    else
                    {
                        base.GenerateTypeReference(type2);
                    }
                    Type[] interfaces = type2.GetInterfaces();
                    if ((interfaces != null) && (interfaces.Length != 0))
                    {
                        foreach (Type type3 in interfaces)
                        {
                            if (!base.ShowNonPublicTypes)
                            {
                                bool flag3 = false;
                                if (type3.DeclaringType != null)
                                {
                                    flag3 = !type3.DeclaringType.IsPublic || (!type3.IsNestedFamily && !type3.IsNestedPublic);
                                }
                                else
                                {
                                    flag3 = !type3.IsPublic;
                                }
                                if (flag3)
                                {
                                    goto Label_024E;
                                }
                            }
                            if ((list2 == null) || !list2.Contains(type3))
                            {
                                if (flag2)
                                {
                                    base.AddText(" : ");
                                    flag2 = false;
                                    flag = true;
                                }
                                else
                                {
                                    base.AddText(", ");
                                }
                                base.GenerateTypeReference(type3);
                            }
                        Label_024E:;
                        }
                    }
                    list2 = interfaces;
                    text = text + "  ";
                }
                if (!flag)
                {
                    element4.Visible = false;
                    element5.Visible = false;
                }
                base.EndCurrentSection();
            }
            object[] customAttributes = base.CurrentType.GetCustomAttributes(true);
            if ((customAttributes != null) && (customAttributes.Length != 0))
            {
                base.GenerateAttributes(customAttributes);
            }
            base.EndCurrentSection();
            return true;
        }

        protected override void GenerateSearchLinks()
        {
            TypeSearchTask task;
            base.AddSectionBreak();
            bool flag = (base.CurrentType.IsClass && !base.CurrentType.IsSealed) && (base.CurrentType != typeof(object));
            bool isInterface = base.CurrentType.IsInterface;
            base.BeginNewSection(false);
            TextElement element1 = base.AddText("Where Used");
            element1.FontStyle |= FontStyle.Bold;
            base.EndCurrentSection();
            base.BeginNewSection(false, 15);
            if (flag)
            {
                task = TypeSearchTask.CreateReferenceSearchTask(base.ProjectData, base.CurrentType, ReferenceSearchMode.BaseTypeDirect);
                base.AddLink("Search", task);
                base.AddText(" for types extending " + base.CurrentType.Name);
                base.AddLineBreak();
                task = TypeSearchTask.CreateReferenceSearchTask(base.ProjectData, base.CurrentType, ReferenceSearchMode.BaseType);
                base.AddLink("Search", task);
                base.AddText(" for types extending " + base.CurrentType.Name + " (directly or indirectly)");
                base.AddLineBreak();
            }
            if (isInterface)
            {
                task = TypeSearchTask.CreateReferenceSearchTask(base.ProjectData, base.CurrentType, ReferenceSearchMode.Implements);
                base.AddLink("Search", task);
                base.AddText(" for types implementing " + base.CurrentType.Name);
                base.AddLineBreak();
            }
            task = TypeSearchTask.CreateReferenceSearchTask(base.ProjectData, base.CurrentType, ReferenceSearchMode.Member);
            base.AddLink("Search", task);
            base.AddText(" for types with fields, properties, methods or events referring to " + base.CurrentType.Name);
            base.EndCurrentSection();
        }
    }
}


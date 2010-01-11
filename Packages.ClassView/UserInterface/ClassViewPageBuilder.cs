namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Packages.ClassView.Documents;
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Text;

    internal abstract class ClassViewPageBuilder : PageBuilder
    {
        private TypeDocument _document;
        private ClassViewProjectData _projectData;
        private static Image errorGlyph;
        private static string localHelpCollection;
        private static Image minusGlyph;
        private static Image plusGlyph;
        private static Image warningGlyph;

        public ClassViewPageBuilder(ClassViewProjectData projectData, TypeDocument document)
        {
            this._projectData = projectData;
            this._document = document;
            base.SetPageColors(Color.Black, Color.LightGoldenrodYellow, Color.DarkGreen, Color.Green);
            base.SetPageMargins(new BoxEdges(10, 10, 10, 10));
            base.SetPageFont("Tahoma", 8f);
        }

        protected void AddHorizontalLine()
        {
            base.BeginNewSection();
            base.AddDivider().ForeColor = Color.Tan;
            base.EndCurrentSection();
        }

        protected void AddLink(string text, object userData)
        {
            HyperLinkElement element = base.AddHyperLink(text, userData);
            element.HoverColor = Color.Red;
            element.LinkColor = Color.Blue;
        }

        protected void AddSectionBreak()
        {
            base.BeginNewSection();
            base.AddDivider(0);
            base.EndCurrentSection();
        }

        protected string CreateLocalHelpUrl(string typeName, string memberName)
        {
            string str = string.Format("frlrf{0}Class{1}Topic", typeName, memberName);
            if (localHelpCollection == null)
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(typeof(object).Module.FullyQualifiedName);
                if ((versionInfo.FileMajorPart == 1) && (versionInfo.FileMinorPart == 0))
                {
                    localHelpCollection = "ms-help://MS.NETFrameworkSDK/cpref/html/";
                }
                else
                {
                    localHelpCollection = string.Concat(new object[] { "ms-help://MS.NETFrameworkSDKv", versionInfo.FileMajorPart, ".", versionInfo.FileMinorPart, "/cpref/html/" });
                }
            }
            return (localHelpCollection + str + ".htm");
        }

        protected string CreateOnlineHelpUrl(string typeName, string memberName)
        {
            string str = string.Format("frlrf{0}Class{1}Topic", typeName, memberName);
            return ("http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/" + str + ".asp");
        }

        protected void GenerateAttributes(object[] attributes)
        {
            this.GenerateExpandableHeader("Attributes", 15, false, "Attributes", false);
            base.BeginNewSection(false, 30, "Attributes").Visible = false;
            Array.Sort(attributes, new ObjectTypeNameComparer());
            bool flag = true;
            foreach (object obj2 in attributes)
            {
                Type type = obj2.GetType();
                if (!type.Namespace.Equals("System.Runtime.CompilerServices"))
                {
                    StringBuilder builder = new StringBuilder();
                    PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    builder.Append(this.GetTypeDisplayFullName(type));
                    if (properties.Length > 0)
                    {
                        foreach (PropertyInfo info in properties)
                        {
                            if (!info.Name.Equals("TypeId"))
                            {
                                object component = null;
                                try
                                {
                                    component = info.GetValue(obj2, null);
                                }
                                catch
                                {
                                    goto Label_0180;
                                }
                                string str = null;
                                if (info.PropertyType == typeof(string))
                                {
                                    str = '"' + ((string) component) + '"';
                                }
                                else if (component == null)
                                {
                                    str = "(null)";
                                }
                                else
                                {
                                    try
                                    {
                                        TypeConverter converter = TypeDescriptor.GetConverter(component);
                                        if ((converter != null) && converter.CanConvertTo(typeof(string)))
                                        {
                                            str = converter.ConvertToString(component);
                                        }
                                        else
                                        {
                                            str = component.ToString();
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (str != null)
                                {
                                    builder.Append("\r\n");
                                    builder.Append(info.Name);
                                    builder.Append(" = ");
                                    builder.Append(str);
                                }
                            Label_0180:;
                            }
                        }
                    }
                    if (!flag)
                    {
                        base.AddLineBreak();
                    }
                    this.GenerateTypeReference(type, builder.ToString());
                    flag = false;
                }
            }
            base.EndCurrentSection();
        }

        protected abstract void GenerateDeclaration();
        protected void GenerateDeclarationName(string name, bool spaceBefore)
        {
            if (spaceBefore)
            {
                base.AddText(" ");
            }
            base.PushBold();
            base.AddText(name);
            base.PopBold();
        }

        protected abstract void GenerateDocumentation();
        protected void GenerateExpandableHeader(string text, int indentation, bool bold, string contentID, bool initialExpanded)
        {
            base.BeginNewSection(false, indentation);
            ToggleImageElement element = new ToggleImageElement(MinusGlyph, PlusGlyph);
            element.ToolTip = "Click to toggle";
            element.BehaveAsButton = true;
            element.Condition = initialExpanded;
            element.Padding = new BoxEdges(0, 1, 4, 1);
            element.UserData = contentID;
            base.AddElement(element, null, false);
            TextElement element2 = base.AddText(text);
            if (bold)
            {
                element2.FontStyle |= FontStyle.Bold;
            }
            base.EndCurrentSection();
        }

        protected abstract void GenerateHelpLinks();
        protected abstract bool GenerateInformation();
        protected virtual void GenerateSearchLinks()
        {
        }

        protected void GenerateTypeReference(Type type)
        {
            this.GenerateTypeReference(type, null);
        }

        protected void GenerateTypeReference(Type type, string toolTip)
        {
            Type elementType = type;
            while (!elementType.IsEnum && (elementType.GetElementType() != null))
            {
                elementType = elementType.GetElementType();
            }
            string name = type.Name;
            string text = null;
            if (type.Name.StartsWith(elementType.Name))
            {
                name = elementType.Name;
                text = type.Name.Substring(elementType.Name.Length);
            }
            HyperLinkElement element = base.AddHyperLink(name, type);
            if (toolTip == null)
            {
                toolTip = this.GetTypeDisplayFullName(elementType);
            }
            element.ToolTip = toolTip;
            element.FontStyle = FontStyle.Regular;
            if (text != null)
            {
                base.AddText(text);
            }
        }

        private string GetTypeDisplayFullName(Type type)
        {
            string fullName = type.FullName;
            if (type.DeclaringType != null)
            {
                return fullName.Replace('+', '.');
            }
            return fullName;
        }

        protected override void InitializePage(Page page)
        {
            this.GenerateDeclaration();
            this.AddHorizontalLine();
            this.AddSectionBreak();
            if (this.GenerateInformation())
            {
                this.AddSectionBreak();
            }
            this.GenerateSearchLinks();
            this.AddHorizontalLine();
            this.GenerateHelpLinks();
            base.InitializePage(page);
            ((ClassViewPage) page).CurrentType = this.CurrentType;
        }

        protected Type CurrentType
        {
            get
            {
                return this._document.Type;
            }
        }

        protected static Image ErrorGlyph
        {
            get
            {
                if (errorGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(MemberPageBuilder), "ErrorGlyph.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    errorGlyph = bitmap;
                }
                return errorGlyph;
            }
        }

        private static Image MinusGlyph
        {
            get
            {
                if (minusGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(ClassViewPageBuilder), "MinusGlyph.bmp");
                    bitmap.MakeTransparent(Color.White);
                    minusGlyph = bitmap;
                }
                return minusGlyph;
            }
        }

        private static Image PlusGlyph
        {
            get
            {
                if (plusGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(ClassViewPageBuilder), "PlusGlyph.bmp");
                    bitmap.MakeTransparent(Color.White);
                    plusGlyph = bitmap;
                }
                return plusGlyph;
            }
        }

        protected ClassViewProjectData ProjectData
        {
            get
            {
                return this._projectData;
            }
        }

        protected bool ShowNonPublicTypes
        {
            get
            {
                return ((this._projectData != null) && this._projectData.ShowNonPublicMembers);
            }
        }

        protected static Image WarningGlyph
        {
            get
            {
                if (warningGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(MemberPageBuilder), "WarningGlyph.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    warningGlyph = bitmap;
                }
                return warningGlyph;
            }
        }

        protected class ObjectTypeNameComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                Type type = x.GetType();
                Type type2 = y.GetType();
                return string.Compare(type.Name, type2.Name);
            }
        }

        protected class TypeNameComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                Type type = (Type) x;
                Type type2 = (Type) y;
                return string.Compare(type.Name, type2.Name);
            }
        }
    }
}


namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Projects.FileSystem;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web.Documents;
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class UserControlDataObjectMapper : IDataObjectMapper
    {
        public bool CanMapDataObject(IServiceProvider serviceProvider, IDataObject dataObject)
        {
            return (dataObject.GetDataPresent(typeof(ProjectItem).Name) && this.IsValidDataObject(serviceProvider, dataObject));
        }

        private bool IsValidDataObject(IServiceProvider serviceProvider, IDataObject dataObject)
        {
            IDocumentDesignerHost service = (IDocumentDesignerHost) serviceProvider.GetService(typeof(IDocumentDesignerHost));
            Project project = service.Document.ProjectItem.Project;
            ProjectItem data = (ProjectItem) dataObject.GetData(typeof(ProjectItem).Name);
            if ((project != data.Project) && (!(project is FileSystemProject) || !(data.Project is FileSystemProject)))
            {
                return false;
            }
            if (!(data is DocumentProjectItem))
            {
                return false;
            }
            DocumentProjectItem item2 = (DocumentProjectItem) data;
            if (item2.DocumentType.Extension.ToLower() != "ascx")
            {
                return false;
            }
            return true;
        }

        public bool PerformMapping(IServiceProvider serviceProvider, IDataObject originalDataObject, DataObject mappedDataObject)
        {
            if (!this.IsValidDataObject(serviceProvider, originalDataObject))
            {
                return false;
            }
            IDocumentDesignerHost service = (IDocumentDesignerHost) serviceProvider.GetService(typeof(IDocumentDesignerHost));
            DocumentProjectItem data = (DocumentProjectItem) originalDataObject.GetData(typeof(ProjectItem).Name);
            bool flag = false;
            if (service == null)
            {
                return flag;
            }
            bool flag2 = false;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(data.Path);
            string fullPath = Path.GetFullPath(data.Path);
            string tagPrefix = string.Empty;
            WebFormsDocument document = service.Document as WebFormsDocument;
            string str4 = Path.GetFullPath(document.DocumentPath);
            if (document == null)
            {
                return flag;
            }
            int num = 0;
            foreach (RegisterDirective directive in document.RegisterDirectives)
            {
                if (!directive.IsUserControl)
                {
                    string str5 = directive.TagPrefix;
                    if (str5.StartsWith("UC") && !str5.Equals("UC"))
                    {
                        try
                        {
                            int num2 = int.Parse(str5.Substring(2));
                            if (num2 >= num)
                            {
                                num = num2 + 1;
                            }
                        }
                        catch
                        {
                        }
                    }
                    continue;
                }
                if (Path.Combine(str4, directive.SourceFile) == fullPath)
                {
                    flag2 = true;
                    tagPrefix = directive.TagPrefix;
                    fileNameWithoutExtension = directive.TagName;
                    break;
                }
            }
            if (!flag2)
            {
                tagPrefix = "uc" + num;
                Uri uri = new Uri(str4);
                Uri toUri = new Uri(fullPath);
                RegisterDirective directive2 = new RegisterDirective(tagPrefix, fileNameWithoutExtension, uri.MakeRelative(toUri), true);
                document.RegisterDirectives.AddRegisterDirective(directive2);
                WebFormsDesignView view = serviceProvider.GetService(typeof(IDesignView)) as WebFormsDesignView;
                if (view != null)
                {
                    view.RegisterNamespace(tagPrefix);
                }
            }
            StringBuilder builder = new StringBuilder();
            builder.Append('<');
            builder.Append(tagPrefix);
            builder.Append(':');
            builder.Append(fileNameWithoutExtension);
            builder.Append(" runat=\"server\" />");
            string str6 = builder.ToString();
            mappedDataObject.SetData(DataFormats.Html, str6);
            mappedDataObject.SetData(DataFormats.Text, str6);
            return true;
        }
    }
}


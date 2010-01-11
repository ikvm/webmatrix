namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Web.UI;
    using System.Windows.Forms;

    [Serializable]
    public class WebFormsToolboxSection : TypeToolboxSection
    {
        public static WebFormsToolboxSection WebForms;

        protected WebFormsToolboxSection()
        {
        }

        public WebFormsToolboxSection(string name) : base(name)
        {
            base.Icon = new Icon(typeof(WebFormsToolboxSection), "WebFormsToolboxSection.ico");
            if (WebForms == null)
            {
                WebForms = this;
            }
        }

        private WebFormsToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
            if (WebForms == null)
            {
                WebForms = this;
            }
        }

        protected override void AddComponents(AssemblyName[] assemblyNames, IServiceProvider serviceProvider)
        {
            IApplicationIdentity service = serviceProvider.GetService(typeof(IApplicationIdentity)) as IApplicationIdentity;
            if (!Directory.Exists(service.ComponentsPath))
            {
                Directory.CreateDirectory(service.ComponentsPath);
            }
            bool flag = false;
            Assembly assembly = null;
            string assemblyFileName = string.Empty;
            bool installToGac = false;
            bool flag3 = false;
            int num = 0;
            for (int i = 0; i < assemblyNames.Length; i++)
            {
                AssemblyName assemblyName = assemblyNames[i];
                if (!Gac.ContainsAssembly(assemblyName))
                {
                    Uri uri = new Uri(assemblyName.CodeBase);
                    string localPath = uri.LocalPath;
                    string path = Path.Combine(service.ComponentsPath, Path.GetFileName(localPath));
                    if (localPath.ToLower() != path.ToLower())
                    {
                        if (File.Exists(path))
                        {
                            FileInfo info = new FileInfo(path);
                            FileInfo info2 = new FileInfo(localPath);
                            if (base.ShowMessage("Customize Toolbox", string.Concat(new object[] { "The Components folder already contains a file called '", Path.GetFileName(path), "'\r\n\r\nWould you like to replace the existing file \r\n\r\n", path, "\r\n", info.Length, " bytes\r\nmodified: ", info.LastWriteTime, "\r\n\r\nwith this one?\r\n\r\n", localPath, "\r\n", info2.Length, " bytes\r\nmodified: ", info2.LastWriteTime, "\r\n\r\nNote: If these files contain different toolbox items, this may cause some of your toolbox items to stop working." }), MessageBoxButtons.YesNo, serviceProvider) != DialogResult.Yes)
                            {
                                continue;
                            }
                            File.Delete(path);
                        }
                        File.Copy(localPath, path);
                    }
                    assemblyName.CodeBase = string.Empty;
                    assemblyFileName = path;
                    installToGac = this.GetInstallToGac(serviceProvider);
                }
                else
                {
                    flag3 = true;
                }
                try
                {
                    if (installToGac)
                    {
                        flag3 = this.InstallToGac(assemblyFileName);
                    }
                    assembly = Assembly.Load(assemblyName);
                    bool flag4 = base.ProcessAssembly(assembly);
                    num++;
                    flag |= flag4;
                }
                catch
                {
                    base.ShowMessage("Customize Toolbox", "The assembly '" + assemblyName.Name + "' could not be loaded!", serviceProvider);
                }
            }
            if (num > 0)
            {
                string caption = string.Empty;
                string text = string.Empty;
                MessageBoxIcon asterisk = MessageBoxIcon.Asterisk;
                if (flag)
                {
                    caption = "Web controls from the assembly have been added to the toolbox.";
                    if (!flag3)
                    {
                        if (installToGac)
                        {
                            text = "There was an error adding the assembly to the Global Assembly Cache.\n";
                            asterisk = MessageBoxIcon.Hand;
                        }
                        else
                        {
                            text = "The assembly was not added to the Global Assembly Cache.\n";
                            asterisk = MessageBoxIcon.Exclamation;
                        }
                        text = text + "To run a web page that uses this assembly, you will need to copy the assembly located at\n'" + assemblyFileName + @"' into the '\bin' directory of your web application.";
                    }
                }
                else
                {
                    caption = "No valid Web controls were found in the assembly.";
                }
                base.ShowMessage(caption, text, MessageBoxButtons.OK, asterisk, MessageBoxDefaultButton.Button1, serviceProvider);
            }
        }

        protected override void AddComponents(Library library, IServiceProvider serviceProvider)
        {
            string text = string.Empty;
            string caption = string.Empty;
            MessageBoxIcon asterisk = MessageBoxIcon.Asterisk;
            bool installToGac = this.GetInstallToGac(serviceProvider);
            ArrayList copyList = new ArrayList();
            if (this.ProcessComponentSection(library, Library.RuntimeFilesSection, installToGac, copyList, serviceProvider))
            {
                caption = "Web controls from the library have been added to the toolbox.";
            }
            else
            {
                caption = "No valid Web controls were found in the library.";
            }
            if (this.ProcessComponentSection(library, Library.DesignTimeFilesSection, installToGac, copyList, serviceProvider))
            {
                text = text + "\nWeb control designers from the library have been installed.";
            }
            if (copyList.Count > 0)
            {
                if (installToGac)
                {
                    text = text + "\nThere was an error adding the following {0} to the Global Assembly Cache:\n\n";
                    asterisk = MessageBoxIcon.Hand;
                }
                else
                {
                    text = text + "\nThe following {0} {1} not added to the Global Assembly Cache:\n\n";
                    asterisk = MessageBoxIcon.Exclamation;
                }
                foreach (string str3 in copyList)
                {
                    text = text + str3 + "\n";
                }
                text = string.Format(text + "\nTo run a web page that uses these assemblies, you will need to copy the {0} listed above to the '\\bin' directory of your web application.", (copyList.Count > 1) ? "assemblies" : "assembly", (copyList.Count > 1) ? "were" : "was");
            }
            base.ShowMessage(caption, text, MessageBoxButtons.OK, asterisk, MessageBoxDefaultButton.Button1, serviceProvider);
            if (library.Sections.Contains(Library.ClientFilesSection))
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"ASP.NET\Client Files\");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                library.UnpackFiles(Library.ClientFilesSection, path);
            }
        }

        public override ToolboxDataItem CreateToolboxDataItem(string toolboxData)
        {
            return new WebFormsToolboxDataItem(toolboxData);
        }

        private bool GetInstallToGac(IServiceProvider serviceProvider)
        {
            return (base.ShowMessage("Install To GAC", "You can install the contained Web controls into the Global Assembly Cache (GAC) or\nplace the assemblies into the '\\bin' directory of each Web application that uses the components.\nNote that assemblies place in the GAC run with full trust.\nDo you want to install the contained Web controls into the GAC?\n", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, serviceProvider) == DialogResult.Yes);
        }

        private bool InstallToGac(string assemblyFileName)
        {
            bool flag = false;
            try
            {
                flag = Gac.AddAssembly(assemblyFileName);
            }
            catch (Exception)
            {
            }
            return flag;
        }

        protected override bool IsValidToolboxComponent(Type type)
        {
            if (base.IsValidToolboxComponent(type))
            {
                object[] customAttributes = type.GetCustomAttributes(typeof(ToolboxItemAttribute), true);
                if (((customAttributes != null) && (customAttributes.Length > 0)) && (customAttributes[0] != null))
                {
                    ToolboxItemAttribute attribute = (ToolboxItemAttribute) customAttributes[0];
                    return (attribute.ToolboxItemType != null);
                }
            }
            return false;
        }

        private bool ProcessComponentSection(Library library, string sectionName, bool installToGac, ArrayList copyList, IServiceProvider serviceProvider)
        {
            IApplicationIdentity service = serviceProvider.GetService(typeof(IApplicationIdentity)) as IApplicationIdentity;
            IDictionary dictionary = (IDictionary) library.Sections[sectionName];
            bool flag = false;
            if ((dictionary != null) && (dictionary.Count > 0))
            {
                if (!Directory.Exists(service.ComponentsPath))
                {
                    Directory.CreateDirectory(service.ComponentsPath);
                }
                foreach (string str in library.UnpackFiles(sectionName, service.ComponentsPath))
                {
                    bool flag2 = false;
                    if (installToGac)
                    {
                        flag2 = this.InstallToGac(str);
                    }
                    if (!flag2)
                    {
                        copyList.Add(str);
                    }
                    Assembly assembly = Assembly.Load(AssemblyName.GetAssemblyName(str));
                    flag |= base.ProcessAssembly(assembly);
                }
            }
            return flag;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(System.Web.UI.Control);
            }
        }
    }
}


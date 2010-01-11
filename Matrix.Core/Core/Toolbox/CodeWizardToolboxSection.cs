namespace Microsoft.Matrix.Core.Toolbox
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Plugins;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public sealed class CodeWizardToolboxSection : TypeToolboxSection, ISerializable
    {
        public static CodeWizardToolboxSection CodeWizards;

        public CodeWizardToolboxSection() : this("Code Wizards")
        {
        }

        public CodeWizardToolboxSection(string name) : base(name)
        {
            base.Icon = new Icon(typeof(CodeWizardToolboxSection), "CodeWizardToolboxSection.ico");
            if (CodeWizards == null)
            {
                CodeWizards = this;
            }
        }

        private CodeWizardToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
            if (CodeWizards == null)
            {
                CodeWizards = this;
            }
        }

        protected override void AddComponents(AssemblyName[] assemblyNames, IServiceProvider serviceProvider)
        {
            IApplicationIdentity service = serviceProvider.GetService(typeof(IApplicationIdentity)) as IApplicationIdentity;
            if (!Directory.Exists(service.PluginsPath))
            {
                Directory.CreateDirectory(service.PluginsPath);
            }
            bool flag = false;
            Assembly assembly = null;
            for (int i = 0; i < assemblyNames.Length; i++)
            {
                AssemblyName assemblyName = assemblyNames[i];
                if (!Gac.ContainsAssembly(assemblyName))
                {
                    Uri uri = new Uri(assemblyName.CodeBase);
                    string localPath = uri.LocalPath;
                    string path = Path.Combine(service.PluginsPath, Path.GetFileName(localPath));
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
                    assemblyName.CodeBase = string.Empty;
                }
                try
                {
                    assembly = Assembly.Load(assemblyName);
                    flag |= base.ProcessAssembly(assembly);
                }
                catch
                {
                    base.ShowMessage("Customize Toolbox", "The assembly '" + assemblyName.Name + "' could not be loaded!", serviceProvider);
                }
            }
            string text = string.Empty;
            if (!flag)
            {
                text = "No valid CodeWizards were found.";
            }
            else
            {
                text = "CodeWizards have been added to the toolbox.";
            }
            base.ShowMessage("Customize Toolbox", text, serviceProvider);
        }

        protected override void AddComponents(Library library, IServiceProvider serviceProvider)
        {
            IApplicationIdentity service = serviceProvider.GetService(typeof(IApplicationIdentity)) as IApplicationIdentity;
            IDictionary dictionary = (IDictionary) library.Sections[Library.RuntimeFilesSection];
            string text = string.Empty;
            if ((dictionary != null) && (dictionary.Count > 0))
            {
                if (!Directory.Exists(service.PluginsPath))
                {
                    Directory.CreateDirectory(service.PluginsPath);
                }
                bool flag = false;
                foreach (string str2 in library.UnpackFiles(Library.RuntimeFilesSection, service.PluginsPath))
                {
                    Assembly assembly = Assembly.LoadFrom(str2);
                    flag |= base.ProcessAssembly(assembly);
                }
                if (!flag)
                {
                    text = "No valid CodeWizards were found.";
                }
                else
                {
                    text = "CodeWizards have added to the toolbox.";
                }
            }
            else
            {
                text = "No CodeWizards were found in the library.";
            }
            base.ShowMessage("Customize Toolbox", text, serviceProvider);
        }

        public override ToolboxDataItem CreateToolboxDataItem(string toolboxData)
        {
            return new CodeWizardToolboxDataItem(toolboxData);
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(CodeWizard);
            }
        }
    }
}


namespace Microsoft.Matrix.Core.Toolbox
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    public abstract class TypeToolboxSection : ToolboxSection
    {
        private const int CustomizeLocal = 1;
        private const int CustomizeOnline = 0;
        private const int CustomizeShowAll = 2;

        protected TypeToolboxSection()
        {
        }

        public TypeToolboxSection(string name) : base(name)
        {
        }

        private TypeToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        protected abstract void AddComponents(AssemblyName[] assemblyNames, IServiceProvider serviceProvider);
        protected abstract void AddComponents(Library library, IServiceProvider serviceProvider);
        public override ToolboxDataItem CreateToolboxDataItem(string toolboxData)
        {
            return new TypeToolboxDataItem(toolboxData);
        }

        public override bool Customize(int command, IServiceProvider serviceProvider)
        {
            switch (command)
            {
                case 0:
                {
                    IComponentGalleryService service = (IComponentGalleryService) serviceProvider.GetService(typeof(IComponentGalleryService));
                    try
                    {
                        Library library = service.BrowseGallery(this.ComponentType.FullName);
                        if (library != null)
                        {
                            this.AddComponents(library, serviceProvider);
                        }
                    }
                    catch (UnauthorizedAccessException exception)
                    {
                        this.ShowMessage("Error", exception.Message + Environment.NewLine + "The assembly may be in use.  Please save your work, restart the application, and try again.", serviceProvider);
                    }
                    catch (Exception exception2)
                    {
                        this.ShowMessage("Error", exception2.Message, serviceProvider);
                    }
                    return true;
                }
                case 1:
                {
                    AssemblySelectionDialog dialog = new AssemblySelectionDialog(serviceProvider);
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            this.AddComponents(dialog.SelectedAssemblies, serviceProvider);
                            Library[] selectedLibraries = dialog.SelectedLibraries;
                            for (int i = 0; i < selectedLibraries.Length; i++)
                            {
                                this.AddComponents(selectedLibraries[i], serviceProvider);
                            }
                        }
                        catch (UnauthorizedAccessException exception3)
                        {
                            this.ShowMessage("Error", exception3.Message + Environment.NewLine + "The assembly may be in use. Please save your work, restart the application, and try again.", serviceProvider);
                        }
                        catch (Exception exception4)
                        {
                            this.ShowMessage("Error", exception4.Message, serviceProvider);
                        }
                    }
                    return true;
                }
                case 2:
                {
                    ArrayList list = new ArrayList(base.ToolboxDataItems);
                    this.ClearToolboxDataItems();
                    Hashtable hashtable = new Hashtable(base.ToolboxDataItems.Count);
                    foreach (ToolboxDataItem item in list)
                    {
                        Type type = Type.GetType(item.ToolboxData, false, true);
                        if (type != null)
                        {
                            Assembly key = type.Assembly;
                            if (!hashtable.Contains(key))
                            {
                                foreach (Type type2 in key.GetTypes())
                                {
                                    if (this.IsValidToolboxComponent(type2))
                                    {
                                        ToolboxDataItem item2 = this.CreateToolboxDataItem(type2.AssemblyQualifiedName);
                                        base.AddToolboxDataItem(item2);
                                    }
                                }
                                hashtable[key] = string.Empty;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public override string GetCustomizationText(int command)
        {
            switch (command)
            {
                case 0:
                    return "Add Online Toolbox Components...";

                case 1:
                    return "Add Local Toolbox Components...";

                case 2:
                    if (base.ToolboxDataItems.Count > 0)
                    {
                        return "Show All Toolbox Components";
                    }
                    break;
            }
            return null;
        }

        protected virtual bool IsValidToolboxComponent(Type type)
        {
            return ((type.IsPublic && !type.IsAbstract) && this.ComponentType.IsAssignableFrom(type));
        }

        protected bool ProcessAssembly(Assembly assembly)
        {
            bool flag = false;
            Type componentType = this.ComponentType;
            foreach (Type type in assembly.GetExportedTypes())
            {
                if (this.IsValidToolboxComponent(type))
                {
                    ToolboxDataItem item = this.CreateToolboxDataItem(type.AssemblyQualifiedName);
                    base.AddToolboxDataItem(item);
                    flag = true;
                }
            }
            return flag;
        }

        protected DialogResult ShowMessage(string caption, string text, IServiceProvider serviceProvider)
        {
            return this.ShowMessage(caption, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, serviceProvider);
        }

        protected DialogResult ShowMessage(string caption, string text, MessageBoxButtons buttons, IServiceProvider serviceProvider)
        {
            return this.ShowMessage(caption, text, buttons, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, serviceProvider);
        }

        protected DialogResult ShowMessage(string caption, string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, IServiceProvider serviceProvider)
        {
            IMxUIService service = (IMxUIService) serviceProvider.GetService(typeof(IMxUIService));
            return service.ShowMessage(text, caption, icon, buttons, defaultButton);
        }

        public override bool CanRemove
        {
            get
            {
                return true;
            }
        }

        public override bool CanReorder
        {
            get
            {
                return true;
            }
        }

        public virtual Type ComponentType
        {
            get
            {
                return typeof(IComponent);
            }
        }

        public override bool IsCustomizable
        {
            get
            {
                return true;
            }
        }
    }
}


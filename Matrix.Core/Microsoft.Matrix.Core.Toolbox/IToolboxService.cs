namespace Microsoft.Matrix.Core.Toolbox
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public interface IToolboxService
    {
        event ToolboxSectionEventHandler ActiveSectionChanged;

        event ToolboxSectionEventHandler SectionAdded;

        event ToolboxSectionEventHandler SectionRemoved;

        void AddToolboxSection(ToolboxSection section);
        void RemoveToolboxSection(ToolboxSection section);
        void ResetToolbox();

        ToolboxSection ActiveSection { get; set; }

        ICollection Sections { get; }
    }
}


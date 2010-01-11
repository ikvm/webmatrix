namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Toolbox;
    using System;

    public interface IToolboxClient
    {
        void OnToolboxDataItemPicked(ToolboxDataItem dataItem);
        bool SupportsToolboxSection(ToolboxSection section);

        ToolboxSection DefaultToolboxSection { get; }
    }
}


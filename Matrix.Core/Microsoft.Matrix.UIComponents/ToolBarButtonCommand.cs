namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class ToolBarButtonCommand : Command
    {
        public ToolBarButtonCommand(Type commandGroup, int commandID, MxToolBarButton button) : base(commandGroup, commandID, button)
        {
        }

        public override void UpdateCommandUI()
        {
            MxToolBarButton commandUI = base.CommandUI as MxToolBarButton;
            commandUI.Pushed = this.Checked;
            commandUI.Enabled = this.Enabled;
            if (base.GlyphChanged && commandUI.Parent.IsHandleCreated)
            {
                commandUI.ImageIndex = this.GlyphIndex;
            }
        }
    }
}


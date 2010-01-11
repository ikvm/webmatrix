namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class StatusBarPanelCommand : Command
    {
        public StatusBarPanelCommand(Type commandGroup, int commandID, MxStatusBarPanel panel) : base(commandGroup, commandID, panel)
        {
        }

        protected internal override bool InvokeCommand()
        {
            return false;
        }

        public override void UpdateCommandUI()
        {
            MxStatusBarPanel commandUI = base.CommandUI as MxStatusBarPanel;
            if (!this.Enabled)
            {
                commandUI.Text = string.Empty;
            }
            else if (base.TextChanged)
            {
                base.TextChanged = false;
                commandUI.Text = this.Text;
            }
        }
    }
}


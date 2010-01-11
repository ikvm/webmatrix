namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class MenuItemCommand : Command
    {
        public MenuItemCommand(Type commandGroup, int commandID, MxMenuItem menuItem) : base(commandGroup, commandID, menuItem)
        {
            this.Visible = menuItem.Visible;
            base.VisibleChanged = false;
        }

        public override void UpdateCommandUI()
        {
            MxMenuItem commandUI = base.CommandUI as MxMenuItem;
            if (base.VisibleChanged)
            {
                commandUI.Visible = this.Visible;
                base.VisibleChanged = false;
            }
            commandUI.Checked = this.Checked;
            commandUI.Enabled = this.Enabled;
            if (base.TextChanged)
            {
                base.TextChanged = false;
                commandUI.Text = this.Text;
            }
            if (base.HelpTextChanged || base.GlyphChanged)
            {
                if (base.HelpTextChanged)
                {
                    base.HelpTextChanged = false;
                    commandUI.HelpText = this.HelpText;
                }
                if (base.GlyphChanged)
                {
                    base.GlyphChanged = false;
                    commandUI.Glyph = this.Glyph;
                }
            }
        }
    }
}


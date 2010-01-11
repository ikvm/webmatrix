namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class EditorStatusBarPanelCommand : StatusBarPanelCommand
    {
        private int _column;
        private int _line;

        public EditorStatusBarPanelCommand(Type commandGroup, int commandID, EditorStatusBarPanel panel) : base(commandGroup, commandID, panel)
        {
        }

        protected internal override bool UpdateCommand(ICommandHandler[] handlers)
        {
            this._line = -1;
            this._column = -1;
            return base.UpdateCommand(handlers);
        }

        public override void UpdateCommandUI()
        {
            EditorStatusBarPanel commandUI = base.CommandUI as EditorStatusBarPanel;
            if (this.Enabled)
            {
                commandUI.SetCurrentLineAndColumn(this._line, this._column);
            }
            else
            {
                commandUI.SetCurrentLineAndColumn(-1, -1);
            }
        }

        public int Column
        {
            get
            {
                return this._column;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._column = value;
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                if (!value)
                {
                    this._line = -1;
                    this._column = -1;
                }
            }
        }

        public int Line
        {
            get
            {
                return this._line;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._line = value;
            }
        }
    }
}


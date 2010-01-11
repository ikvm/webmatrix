namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;

    public abstract class Command
    {
        private Type _commandGroup;
        private int _commandID;
        private object _commandUI;
        private ICommandHandlerWithContext _contextHandler;
        private object _contextObject;
        private Image _glyph;
        private int _glyphIndex;
        private ICommandHandler _handler;
        private string _helpText;
        private int _state;
        private string _text;
        private const int StateChecked = 1;
        private const int StateEnabled = 2;
        private const int StateGlyphChanged = 0x400;
        private const int StateHelpTextChanged = 0x200;
        private const int StateInternalChange = 0x10000;
        private const int StateTextChanged = 0x100;
        private const int StateVisible = 4;
        private const int StateVisibleChanged = 0x800;

        public Command(Type commandGroup, int commandID, object commandUI)
        {
            if ((commandGroup == null) || (commandUI == null))
            {
                throw new ArgumentNullException();
            }
            this._commandGroup = commandGroup;
            this._commandID = commandID;
            this._commandUI = commandUI;
            this._glyphIndex = -1;
        }

        protected internal virtual bool InvokeCommand()
        {
            if (((this._handler != null) || (this._contextHandler != null)) && !this.InternalChange)
            {
                if (this._contextHandler != null)
                {
                    return this._contextHandler.HandleCommand(this, this._contextObject);
                }
                return this._handler.HandleCommand(this);
            }
            return false;
        }

        protected internal virtual bool UpdateCommand(ICommandHandler[] handlers)
        {
            this._handler = null;
            this._contextHandler = null;
            this._contextObject = null;
            for (int i = 0; i < handlers.Length; i++)
            {
                if (handlers[i].UpdateCommand(this))
                {
                    this._handler = handlers[i];
                    return true;
                }
            }
            this.Enabled = false;
            return false;
        }

        protected internal bool UpdateCommand(ICommandHandler[] handlers, ICommandHandlerWithContext contextHandler, object context)
        {
            this._handler = null;
            this._contextHandler = null;
            this._contextObject = null;
            if (contextHandler.UpdateCommand(this, context))
            {
                this._contextHandler = contextHandler;
                this._contextObject = context;
                return true;
            }
            return this.UpdateCommand(handlers);
        }

        public abstract void UpdateCommandUI();

        public virtual bool Checked
        {
            get
            {
                return ((this._state & 1) != 0);
            }
            set
            {
                if (value)
                {
                    this._state |= 1;
                }
                else
                {
                    this._state &= -2;
                }
            }
        }

        public Type CommandGroup
        {
            get
            {
                return this._commandGroup;
            }
        }

        public int CommandID
        {
            get
            {
                return this._commandID;
            }
        }

        public object CommandUI
        {
            get
            {
                return this._commandUI;
            }
        }

        public virtual bool Enabled
        {
            get
            {
                return ((this._state & 2) != 0);
            }
            set
            {
                if (value)
                {
                    this._state |= 2;
                }
                else
                {
                    this._state &= -3;
                }
            }
        }

        public virtual Image Glyph
        {
            get
            {
                return this._glyph;
            }
            set
            {
                if (this._glyph != value)
                {
                    this._glyph = value;
                    this.GlyphChanged = true;
                }
            }
        }

        protected bool GlyphChanged
        {
            get
            {
                return ((this._state & 0x400) != 0);
            }
            set
            {
                if (value)
                {
                    this._state |= 0x400;
                }
                else
                {
                    this._state &= -1025;
                }
            }
        }

        public virtual int GlyphIndex
        {
            get
            {
                return this._glyphIndex;
            }
            set
            {
                if (this._glyphIndex != value)
                {
                    this._glyphIndex = value;
                    this.GlyphChanged = true;
                }
            }
        }

        internal ICommandHandler Handler
        {
            get
            {
                return this._handler;
            }
        }

        public virtual string HelpText
        {
            get
            {
                return this._helpText;
            }
            set
            {
                if (string.Compare(this._helpText, value) != 0)
                {
                    this._helpText = value;
                    this.HelpTextChanged = true;
                }
            }
        }

        protected bool HelpTextChanged
        {
            get
            {
                return ((this._state & 0x200) != 0);
            }
            set
            {
                if (value)
                {
                    this._state |= 0x200;
                }
                else
                {
                    this._state &= -513;
                }
            }
        }

        protected bool InternalChange
        {
            get
            {
                return ((this._state & 0x10000) != 0);
            }
            set
            {
                if (value)
                {
                    this._state |= 0x10000;
                }
                else
                {
                    this._state &= -65537;
                }
            }
        }

        public virtual string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (string.Compare(this._text, value) != 0)
                {
                    this._text = value;
                    this.TextChanged = true;
                }
            }
        }

        protected bool TextChanged
        {
            get
            {
                return ((this._state & 0x100) != 0);
            }
            set
            {
                if (value)
                {
                    this._state |= 0x100;
                }
                else
                {
                    this._state &= -257;
                }
            }
        }

        public virtual bool Visible
        {
            get
            {
                return ((this._state & 4) != 0);
            }
            set
            {
                if (value != this.Visible)
                {
                    if (value)
                    {
                        this._state |= 4;
                    }
                    else
                    {
                        this._state &= -5;
                    }
                    this.VisibleChanged = true;
                }
            }
        }

        protected bool VisibleChanged
        {
            get
            {
                return ((this._state & 0x800) != 0);
            }
            set
            {
                if (value)
                {
                    this._state |= 0x800;
                }
                else
                {
                    this._state &= -2049;
                }
            }
        }
    }
}


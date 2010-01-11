namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public sealed class MxDataGridCancelEventArgs : CancelEventArgs
    {
        private object commandSource;
        private Hashtable fieldValues;
        private MxDataGridItem item;

        public MxDataGridCancelEventArgs(MxDataGridCommandEventArgs commandEventArgs, bool cancel) : base(cancel)
        {
            this.item = commandEventArgs.Item;
            this.commandSource = commandEventArgs.CommandSource;
        }

        public MxDataGridCancelEventArgs(MxDataGridItem item, object commandSource, bool cancel) : base(cancel)
        {
            this.item = item;
            this.commandSource = commandSource;
        }

        public object CommandSource
        {
            get
            {
                return this.commandSource;
            }
        }

        public IDictionary FieldValues
        {
            get
            {
                if (this.fieldValues == null)
                {
                    this.fieldValues = new Hashtable();
                }
                return this.fieldValues;
            }
        }

        public MxDataGridItem Item
        {
            get
            {
                return this.item;
            }
        }
    }
}


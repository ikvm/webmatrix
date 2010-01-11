namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public sealed class MxDataGridUpdateEventArgs : CancelEventArgs
    {
        private object commandSource;
        private MxDataGridItem item;
        private Hashtable newValues;
        private Hashtable selectionFilter;

        public MxDataGridUpdateEventArgs(MxDataGridCommandEventArgs commandEventArgs, bool cancel) : base(cancel)
        {
            this.item = commandEventArgs.Item;
            this.commandSource = commandEventArgs.CommandSource;
        }

        public MxDataGridUpdateEventArgs(MxDataGridItem item, object commandSource, bool cancel) : base(cancel)
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

        public MxDataGridItem Item
        {
            get
            {
                return this.item;
            }
        }

        public IDictionary NewValues
        {
            get
            {
                if (this.newValues == null)
                {
                    this.newValues = new Hashtable();
                }
                return this.newValues;
            }
        }

        public IDictionary SelectionFilter
        {
            get
            {
                if (this.selectionFilter == null)
                {
                    this.selectionFilter = new Hashtable();
                }
                return this.selectionFilter;
            }
        }
    }
}


namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;

    public sealed class MxDataGridStatusEventArgs : EventArgs
    {
        private int affectedRecords;
        private object commandSource;
        private MxDataGridItem item;

        public MxDataGridStatusEventArgs(MxDataGridCancelEventArgs cancelEventArgs, int affectedRecords)
        {
            this.item = cancelEventArgs.Item;
            this.commandSource = cancelEventArgs.CommandSource;
            this.affectedRecords = affectedRecords;
        }

        public MxDataGridStatusEventArgs(MxDataGridUpdateEventArgs updateEventArgs, int affectedRecords)
        {
            this.item = updateEventArgs.Item;
            this.commandSource = updateEventArgs.CommandSource;
            this.affectedRecords = affectedRecords;
        }

        public MxDataGridStatusEventArgs(MxDataGridItem item, object commandSource, int affectedRecords)
        {
            this.item = item;
            this.commandSource = commandSource;
            this.affectedRecords = affectedRecords;
        }

        public int AffectedRecords
        {
            get
            {
                return this.affectedRecords;
            }
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
    }
}


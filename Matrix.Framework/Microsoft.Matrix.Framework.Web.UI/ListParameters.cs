namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Collections;
    using System.Web.UI;

    public class ListParameters : IStateManager
    {
        private Hashtable _filters;
        private bool _sortAscending = true;
        private string _sortExpression;
        private bool marked;

        public virtual void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[]) savedState;
                this._sortAscending = (bool) objArray[0];
                this._sortExpression = (string) objArray[1];
                this._filters = (Hashtable) objArray[2];
            }
        }

        public virtual object SaveViewState()
        {
            return new object[] { this._sortAscending, this._sortExpression, this._filters };
        }

        public virtual void TrackViewState()
        {
            this.marked = true;
        }

        public IDictionary Filters
        {
            get
            {
                if (this._filters == null)
                {
                    this._filters = new Hashtable();
                }
                return this._filters;
            }
        }

        public bool IsTrackingViewState
        {
            get
            {
                return this.marked;
            }
        }

        public bool SortAscending
        {
            get
            {
                return this._sortAscending;
            }
            set
            {
                this._sortAscending = value;
            }
        }

        public string SortExpression
        {
            get
            {
                return this._sortExpression;
            }
            set
            {
                if (this.SortExpression != string.Empty)
                {
                    if (this.SortExpression == value)
                    {
                        this.SortAscending = !this.SortAscending;
                    }
                    else
                    {
                        this.SortAscending = true;
                    }
                }
                this._sortExpression = value;
            }
        }
    }
}


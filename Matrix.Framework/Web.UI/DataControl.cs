namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework.Web.UI.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Web.UI;

    [Designer(typeof(DataControlDesigner))]
    public abstract class DataControl : Control, IListSource
    {
        protected Hashtable _listParametersCollection = new Hashtable();
        private const string defaultListKey = "__!default";
        private static readonly object EventDataSourceChanged = new object();

        [WebCategory("Action"), WebSysDescription("DataControl_OnDataSourceChanged")]
        public event EventHandler DataSourceChanged
        {
            add
            {
                base.Events.AddHandler(EventDataSourceChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventDataSourceChanged, value);
            }
        }

        protected DataControl()
        {
        }

        public virtual void AddFilter(string listName, string filterName, object filterValue)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters == null)
            {
                this._listParametersCollection.Add(listName, this.CreateListParameters());
                parameters = (ListParameters) this._listParametersCollection[listName];
            }
            if (parameters != null)
            {
                parameters.Filters.Add(filterName, filterValue);
                this.OnDataSourceChanged(new EventArgs());
            }
        }

        public virtual void ClearFilters(string listName)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters != null)
            {
                parameters.Filters.Clear();
                this.OnDataSourceChanged(new EventArgs());
            }
        }

        private ListParameters CreateListParameters()
        {
            ListParameters parameters = new ListParameters();
            parameters.TrackViewState();
            return parameters;
        }

        public abstract int Delete(string listName, IDictionary selectionFilters);
        public abstract IEnumerable GetDataSource(string listName);
        public virtual string GetFilter(string listName, string filterName)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters != null)
            {
                return (string) parameters.Filters[filterName];
            }
            return string.Empty;
        }

        protected virtual IDictionary GetFilters(string listName)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters != null)
            {
                return parameters.Filters;
            }
            return null;
        }

        protected virtual string GetSortExpression(string listName)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters != null)
            {
                return parameters.SortExpression;
            }
            return string.Empty;
        }

        public abstract int Insert(string listName, IDictionary values);
        protected virtual bool IsSortDirectionAscending(string listName)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters != null)
            {
                return parameters.SortAscending;
            }
            return true;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[]) savedState;
                if (objArray[0] != null)
                {
                    base.LoadViewState(objArray[0]);
                }
                if (objArray[1] != null)
                {
                    object[] objArray2 = (object[]) objArray[1];
                    if (objArray2 != null)
                    {
                        for (int i = 0; i < objArray2.Length; i++)
                        {
                            object[] objArray3 = (object[]) objArray2[i];
                            ListParameters parameters = this.CreateListParameters();
                            parameters.LoadViewState(objArray3[1]);
                            this._listParametersCollection.Add(objArray3[0], parameters);
                        }
                    }
                }
            }
        }

        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventDataSourceChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public virtual void RemoveFilter(string listName, string filterName)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters != null)
            {
                parameters.Filters.Remove(filterName);
                this.OnDataSourceChanged(new EventArgs());
            }
        }

        protected override object SaveViewState()
        {
            object[] objArray = new object[2];
            int count = this._listParametersCollection.Count;
            int index = 0;
            objArray[0] = base.SaveViewState();
            if ((this._listParametersCollection != null) && (count > 0))
            {
                object[] objArray2 = new object[count];
                IDictionaryEnumerator enumerator = this._listParametersCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    objArray2[index] = new object[] { current.Key, ((ListParameters) current.Value).SaveViewState() };
                    index++;
                }
                objArray[1] = objArray2;
            }
            return objArray;
        }

        public virtual void SetSortExpression(string listName, string sortExpression)
        {
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                listName = "__!default";
            }
            ListParameters parameters = (ListParameters) this._listParametersCollection[listName];
            if (parameters == null)
            {
                this._listParametersCollection.Add(listName, this.CreateListParameters());
                parameters = (ListParameters) this._listParametersCollection[listName];
            }
            if (parameters != null)
            {
                parameters.SortExpression = sortExpression;
                this.OnDataSourceChanged(new EventArgs());
            }
        }

        IList IListSource.GetList()
        {
            object dataSource = this.DataSource;
            IListSource source = dataSource as IListSource;
            if (source != null)
            {
                return source.GetList();
            }
            IList list = dataSource as IList;
            if (list != null)
            {
                return list;
            }
            return null;
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            IDictionaryEnumerator enumerator = this._listParametersCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                ((ListParameters) current.Value).TrackViewState();
            }
        }

        public abstract int Update(string listName, IDictionary selectionFilters, IDictionary newValues);

        [DefaultValue(true), WebCategory("Behavior"), WebSysDescription("DataControl_AutoGenerateDeleteCommand")]
        public virtual bool AutoGenerateDeleteCommand
        {
            get
            {
                object obj2 = this.ViewState["AutoGenerateDeleteCommand"];
                if (obj2 != null)
                {
                    return (bool) obj2;
                }
                return true;
            }
            set
            {
                this.ViewState["AutoGenerateDeleteCommand"] = value;
            }
        }

        [WebSysDescription("DataControl_AutoGenerateInsertCommand"), WebCategory("Behavior"), DefaultValue(true)]
        public virtual bool AutoGenerateInsertCommand
        {
            get
            {
                object obj2 = this.ViewState["AutoGenerateInsertCommand"];
                if (obj2 != null)
                {
                    return (bool) obj2;
                }
                return true;
            }
            set
            {
                this.ViewState["AutoGenerateInsertCommand"] = value;
            }
        }

        [WebSysDescription("DataControl_AutoGenerateUpdateCommand"), WebCategory("Behavior"), DefaultValue(true)]
        public virtual bool AutoGenerateUpdateCommand
        {
            get
            {
                object obj2 = this.ViewState["AutoGenerateUpdateCommand"];
                if (obj2 != null)
                {
                    return (bool) obj2;
                }
                return true;
            }
            set
            {
                this.ViewState["AutoGenerateUpdateCommand"] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), WebSysDescription("DataControl_CanDelete"), Browsable(false)]
        public abstract bool CanDelete { get; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), WebSysDescription("DataControl_CanFilter"), Browsable(false)]
        public abstract bool CanFilter { get; }

        [Browsable(false), WebSysDescription("DataControl_CanInsert"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public abstract bool CanInsert { get; }

        [WebSysDescription("DataControl_CanSort"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public abstract bool CanSort { get; }

        [WebSysDescription("DataControl_CanUpdate"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public abstract bool CanUpdate { get; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public abstract object DataSource { get; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), WebSysDescription("DataControl_ListNames")]
        public virtual ICollection ListNames
        {
            get
            {
                if (this._listParametersCollection != null)
                {
                    return this._listParametersCollection.Keys;
                }
                return null;
            }
        }

        bool IListSource.ContainsListCollection
        {
            get
            {
                IListSource dataSource = this.DataSource as IListSource;
                return ((dataSource != null) && dataSource.ContainsListCollection);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public abstract Type UnderlyingDataSourceType { get; }

        [DefaultValue(false), Bindable(true), Browsable(false)]
        public override bool Visible
        {
            get
            {
                return false;
            }
        }
    }
}


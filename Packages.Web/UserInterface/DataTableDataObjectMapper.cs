namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Framework.Web.UI;
    using Microsoft.Matrix.Packages.DBAdmin;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    public class DataTableDataObjectMapper : IDataObjectMapper
    {
        private static string[] reservedWords;

        public bool CanMapDataObject(IServiceProvider serviceProvider, IDataObject dataObject)
        {
            return dataObject.GetDataPresent(DBAdminPackage.MxDataTableDataFormat);
        }

        private string CreateSelectScript(string tableName)
        {
            return ("SELECT * FROM [" + tableName + "]");
        }

        private string CreateUpdateScript(string tableName, ICollection columns, string[] primaryKey)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE [");
            builder.Append(tableName);
            builder.Append("] SET ");
            bool flag = true;
            int num = 0;
            foreach (Column column in columns)
            {
                if (column.IsIdentity)
                {
                    continue;
                }
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append(',');
                }
                string name = column.Name;
                builder.Append('[');
                builder.Append(name);
                builder.Append(']');
                builder.Append('=');
                if (IsRegularIdentifier(name))
                {
                    builder.Append('@');
                    builder.Append(name);
                }
                else
                {
                    builder.Append("@param");
                    builder.Append(num);
                }
                num++;
            }
            flag = true;
            if (primaryKey.Length > 0)
            {
                builder.Append(" WHERE ");
                for (int i = 0; i < primaryKey.Length; i++)
                {
                    if (flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        builder.Append(" AND ");
                    }
                    string str2 = primaryKey[i];
                    builder.Append('[');
                    builder.Append(str2);
                    builder.Append(']');
                    builder.Append('=');
                    if (IsRegularIdentifier(str2))
                    {
                        builder.Append('@');
                        builder.Append(str2);
                    }
                    else
                    {
                        builder.Append("@param");
                        builder.Append((int) (num + i));
                    }
                }
            }
            return builder.ToString();
        }

        private static bool IsRegularIdentifier(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if ((s.Length == 0) || (s.Length > 0x80))
            {
                return false;
            }
            char c = s[0];
            char ch1 = s[s.Length - 1];
            if ((c == '$') || char.IsDigit(c))
            {
                return false;
            }
            CharEnumerator enumerator = s.GetEnumerator();
            while (enumerator.MoveNext())
            {
                char current = enumerator.Current;
                if (((!char.IsLetterOrDigit(current) && (current != '@')) && ((current != '$') && (current != '#'))) && (current != '_'))
                {
                    return false;
                }
            }
            if (IsReservedWord(s))
            {
                return false;
            }
            return true;
        }

        private static bool IsReservedWord(string s)
        {
            if ((s == null) || (s.Length == 0))
            {
                throw new ArgumentNullException("s");
            }
            if (reservedWords == null)
            {
                reservedWords = new string[] { 
                    "absolute", "action", "add", "admin", "after", "aggregate", "alias", "all", "allocate", "alter", "and", "any", "are", "array", "as", "asc", 
                    "assertion", "at", "authorization", "backup", "before", "begin", "between", "binary", "bit", "blob", "boolean", "both", "breadth", "break", "browse", "bulk", 
                    "by", "call", "cascade", "cascaded", "case", "cast", "catalog", "char", "character", "check", "checkpoint", "class", "clob", "close", "clustered", "coalesce", 
                    "collate", "collation", "column", "commit", "completion", "compute", "connect", "connection", "constraint", "constraints", "constructor", "contains", "containstable", "continue", "convert", "corresponding", 
                    "create", "cross", "cube", "current", "current_date", "current_path", "current_role", "current_time", "current_timestamp", "current_user", "cursor", "cycle", "data", "database", "date", "day", 
                    "dbcc", "deallocate", "dec", "decimal", "declare", "default", "deferrable", "deferred", "delete", "deny", "depth", "deref", "desc", "describe", "descriptor", "destroy", 
                    "destructor", "deterministic", "diagnostics", "dictionary", "disconnect", "disk", "distinct", "distributed", "domain", "double", "drop", "dummy", "dump", "dynamic", "each", "else", 
                    "end", "end-exec", "equals", "errlvl", "escape", "every", "except", "exception", "exec", "execute", "exists", "exit", "external", "false", "fetch", "file", 
                    "fillfactor", "first", "float", "for", "foreign", "found", "free", "freetext", "freetexttable", "from", "full", "function", "general", "get", "global", "go", 
                    "goto", "grant", "group", "grouping", "having", "holdlock", "host", "hour", "identity", "identity_insert", "identitycol", "if", "ignore", "immediate", "in", "index", 
                    "indicator", "initialize", "initially", "inner", "inout", "input", "insert", "int", "integer", "intersect", "interval", "into", "is", "isolation", "iterate", "join", 
                    "key", "kill", "language", "large", "last", "lateral", "leading", "left", "less", "level", "like", "limit", "lineno", "load", "local", "localtime", 
                    "localtimestamp", "locator", "map", "match", "minute", "modifies", "modify", "module", "month", "names", "national", "natural", "nchar", "nclob", "new", "next", 
                    "no", "nocheck", "nonclustered", "none", "not", "null", "nullif", "numeric", "object", "of", "off", "offsets", "old", "on", "only", "open", 
                    "opendatasource", "openquery", "openrowset", "openxml", "operation", "option", "or", "order", "ordinality", "out", "outer", "output", "over", "pad", "parameter", "parameters", 
                    "partial", "path", "percent", "plan", "postfix", "precision", "prefix", "preorder", "prepare", "preserve", "primary", "print", "prior", "privileges", "proc", "procedure", 
                    "public", "raiserror", "read", "reads", "readtext", "real", "reconfigure", "recursive", "ref", "references", "referencing", "relative", "replication", "restore", "restrict", "result", 
                    "return", "returns", "revoke", "right", "role", "rollback", "rollup", "routine", "row", "rowcount", "rowguidcol", "rows", "rule", "save", "savepoint", "schema", 
                    "scope", "scroll", "search", "second", "section", "select", "sequence", "session", "session_user", "set", "sets", "setuser", "shutdown", "size", "smallint", "some", 
                    "space", "specific", "specifictype", "sql", "sqlexception", "sqlstate", "sqlwarning", "start", "state", "statement", "static", "statistics", "structure", "system_user", "table", "temporary", 
                    "terminate", "textsize", "than", "then", "time", "timestamp", "timezone_hour", "timezone_minute", "to", "top", "trailing", "tran", "transaction", "translation", "treat", "trigger", 
                    "true", "truncate", "tsequal", "under", "union", "unique", "unknown", "unnest", "update", "updatetext", "usage", "use", "user", "using", "value", "values", 
                    "varchar", "variable", "varying", "view", "waitfor", "when", "whenever", "where", "while", "with", "without", "work", "write", "writetext", "year", "zone"
                 };
            }
            return (Array.BinarySearch(reservedWords, s.ToLower(), new OrdinalStringComparer()) >= 0);
        }

        public bool PerformMapping(IServiceProvider serviceProvider, IDataObject originalDataObject, DataObject mappedDataObject)
        {
            INameCreationService service;
            Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table data = (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table) originalDataObject.GetData(DBAdminPackage.MxDataTableDataFormat);
            if (data == null)
            {
                return false;
            }
            IDataProviderDatabase database = data.Database as IDataProviderDatabase;
            if (database == null)
            {
                return false;
            }
            IDesignerHost host = (IDesignerHost) serviceProvider.GetService(typeof(IDesignerHost));
            bool flag = false;
            if (host == null)
            {
                return flag;
            }
            Type type = null;
            try
            {
                type = Type.GetType("Microsoft.Matrix.Framework.Web.UI.MxDataGrid, Microsoft.Matrix.Framework");
            }
            catch
            {
                return false;
            }
            Type type2 = null;
            string providerName = database.ProviderName;
            if (providerName == null)
            {
                goto Label_00C4;
            }
            providerName = string.IsInterned(providerName);
            if (providerName != "Sql")
            {
                if (providerName == "OleDb")
                {
                    goto Label_00AD;
                }
                goto Label_00C4;
            }
            try
            {
                type2 = Type.GetType("Microsoft.Matrix.Framework.Web.UI.SqlDataSourceControl, Microsoft.Matrix.Framework");
                goto Label_00C4;
            }
            catch
            {
                return false;
            }
        Label_00AD:
            try
            {
                type2 = Type.GetType("Microsoft.Matrix.Framework.Web.UI.AccessDataSourceControl, Microsoft.Matrix.Framework");
            }
            catch
            {
                return false;
            }
        Label_00C4:
            service = (INameCreationService) host.GetService(typeof(INameCreationService));
            Key key = null;
            foreach (Key key2 in data.Keys)
            {
                if (key2.KeyType == KeyType.Primary)
                {
                    key = key2;
                    break;
                }
            }
            string[] strArray = null;
            if (key != null)
            {
                strArray = new string[key.ReferencedColumns.Count];
                int index = 0;
                foreach (string str in key.ReferencedColumns)
                {
                    strArray[index] = str;
                    index++;
                }
            }
            else
            {
                ((IMxUIService) host.GetService(typeof(IMxUIService))).ShowMessage("The dropped table does not have a primary key and cannot be used.", "ASP.NET Web Matrix");
                return true;
            }
            ICollection columns = data.Columns;
            DataControl control = null;
            switch (database.ProviderName)
            {
                case "Sql":
                {
                    SqlDataSourceControl control2 = (SqlDataSourceControl) Activator.CreateInstance(type2);
                    control2.ID = service.CreateName(host.Container, control2.GetType());
                    control2.ConnectionString = database.ConnectionString;
                    control2.SelectCommand = this.CreateSelectScript(data.Name);
                    control = control2;
                    break;
                }
                case "OleDb":
                {
                    AccessDataSourceControl control3 = (AccessDataSourceControl) Activator.CreateInstance(type2);
                    control3.ID = service.CreateName(host.Container, control3.GetType());
                    control3.ConnectionString = database.ConnectionString;
                    control3.SelectCommand = this.CreateSelectScript(data.Name);
                    control = control3;
                    break;
                }
            }
            MxDataGrid grid = (MxDataGrid) Activator.CreateInstance(type);
            grid.ID = service.CreateName(host.Container, grid.GetType());
            grid.DataSourceControlID = control.ID;
            grid.DataMember = data.Name;
            grid.AllowSorting = true;
            grid.AllowPaging = true;
            if (strArray.Length == 1)
            {
                grid.DataKeyField = strArray[0];
            }
            grid.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            grid.BorderWidth = 1;
            grid.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xcc);
            grid.BackColor = Color.White;
            grid.CellPadding = 3;
            grid.FooterStyle.ForeColor = Color.FromArgb(0, 0, 0x66);
            grid.FooterStyle.BackColor = Color.White;
            grid.HeaderStyle.Font.Bold = true;
            grid.HeaderStyle.ForeColor = Color.White;
            grid.HeaderStyle.BackColor = Color.FromArgb(0, 0x66, 0x99);
            grid.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            grid.PagerStyle.ForeColor = Color.FromArgb(0, 0, 0x66);
            grid.PagerStyle.BackColor = Color.White;
            grid.PagerStyle.Mode = PagerMode.NumericPages;
            grid.SelectedItemStyle.Font.Bold = true;
            grid.SelectedItemStyle.ForeColor = Color.White;
            grid.SelectedItemStyle.BackColor = Color.FromArgb(0x66, 0x99, 0x99);
            grid.ItemStyle.ForeColor = Color.FromArgb(0, 0, 0x66);
            string tagPrefix = ((IWebFormReferenceManager) host.GetService(typeof(IWebFormReferenceManager))).GetTagPrefix(type);
            WebFormsDesignView view = host.GetService(typeof(IDesignView)) as WebFormsDesignView;
            if (view != null)
            {
                view.RegisterNamespace(tagPrefix);
            }
            StringWriter sw = new StringWriter();
            ControlPersister.PersistControl(sw, control, host);
            ControlPersister.PersistControl(sw, grid, host);
            string input = sw.ToString();
            sw = new StringWriter();
            new HtmlFormatter().Format(input, sw, new HtmlFormatterOptions(' ', 4, 80, HtmlFormatterCase.LowerCase, HtmlFormatterCase.LowerCase, true));
            input = sw.ToString();
            mappedDataObject.SetData(DataFormats.Html, input);
            mappedDataObject.SetData(DataFormats.Text, input);
            return true;
        }

        private class OrdinalStringComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.CompareOrdinal((string) x, (string) y);
            }
        }
    }
}


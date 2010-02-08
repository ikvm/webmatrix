using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using DateExplorer.Core.Controllers;

namespace DateExplorer.Core.Models_Reversed
{
    [Serializable]
    public class ModelBase:Dictionary<string, string>
    {
        public ModelBase():base()
        {
        }

        public ModelBase(int id)
            : base()
        {
            this.Load(id);
        }

       protected ModelBase(SerializationInfo info, StreamingContext context)
        {
        }

        private string m_tableName;
        public string TableName
        {
            get { return m_tableName; }
            set { m_tableName = value;}
        }

        private string m_keyField;
        public string KeyField
        {
            get { return m_keyField; }
            set { m_keyField = value;}
        }

        private bool m_IsNew = true;
        public bool IsNew
        {
            get { return m_IsNew; }
        }

        private bool m_IsDirty = false;
        public bool IsDirty
        {
            get { return m_IsDirty; }
            set { m_IsDirty = value; }
        }

        public int KeyValue
        {
            get { return this[this.KeyField] == null ? 0 : int.Parse(this[this.KeyField]); }
        }

        //重载索引属性, 当键值不存在的时候返回空值, 当值改变的时候设置isDirty属性
        public new string this[string key]
        {
            get { return base.ContainsKey(key) ? base[key] : ""; }
            set 
            {
                if (!this.ContainsKey(key) || base[key] != key)
                { 
                    m_IsDirty = true;
                    base[key] = value == null ? "" : value;
                }
            }
        }

        public virtual bool Save()
        {
            string sql;
            int rowsAffected;
            if (m_IsNew)
            {
                sql = BuildInsertCommandText();
                object ret = DatabaseController.ExecuteScalar(sql);
                if (ret != null)
                {
                    int newID = int.Parse(ret.ToString());
                    this[this.KeyField] = newID.ToString();

                    this.m_IsDirty = false;
                    this.m_IsNew = false;
                    return true;
                }
            }
            else
            {
                if (m_IsDirty)
                { 
                    sql = BuildUpdateCommandText();
                    rowsAffected = DatabaseController.ExecuteNonQuery(sql);

                    this.m_IsDirty = false;
                    this.m_IsNew = false;
                }

                return true;
            }

            return false;
        }

        public virtual bool Load(int id)
        {
            string fieldName, fieldValue;
            string sql = string.Format("select * from `{0}` where `{1}`={2};", this.TableName, this.KeyField, id);

            using (DataSet ds = DatabaseController.ExecuteDataSet(sql))
            {
                DataTable data = ds.Tables[0];
                bool haveData = data.Rows.Count > 0;

                if (haveData)
                {
                    //把一个数据行的各个列的列名和对应的值存到字典里(相当于进行了一次行列转换), 方便后面使用
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        DataColumn col = data.Columns[i];
                        fieldName = col.ColumnName;
                        fieldValue = !haveData ? "" : (data.Rows[0][i] == null ? "" : data.Rows[0][i].ToString());
                        this[fieldName] = fieldValue;
                    }

                    m_IsNew = false;
                    m_IsDirty = false;
                    return true;
                }
                else
                {
                    m_IsNew = true;
                    m_IsDirty = true;
                    return false;
                }
            }


        }

        public bool Reload()
        {
            int id = this.KeyValue;
            this.Clear();
            return this.Load(id);

        }

        //根据提供的ID删除TableName表里指定的项
        public virtual int Delete()
        {
            //TODO: 历史记录功能待调试
            //if (!LogChanges(id))
            //    return 0;
            if(m_IsNew)
                return 0 ;

            string sql = string.Format("delete from `{0}` where `{1}`={2}", this.TableName, this.KeyField, this.KeyValue.ToString());
            int rowsAffected = DatabaseController.ExecuteNonQuery(sql);
            this.Clear();
            this.InspectModel();
            m_IsDirty = false;
            m_IsNew = true;

            return rowsAffected;
        }

        protected virtual void InspectModel()
        {
            string fieldName;
            string sql = string.Format("select * from `{0}` where 1=0;", TableName);

            using (DataSet ds = DatabaseController.ExecuteDataSet(sql))
            {
                DataTable data = ds.Tables[0];

                //把一个数据行的各个列的列名和对应的值存到字典里(相当于进行了一次行列转换), 方便后面使用
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    DataColumn col = data.Columns[i];
                    fieldName = col.ColumnName;
                    this[fieldName] = "";
                }
            }
        }

        public bool Lock(ModelBase locker)
        {
            string lockerID = this["LockerID"];
            if (lockerID == locker["ID"])
            {
                return true;
            }
            else if (lockerID == "")
            {
                this["LockerID"] = lockerID;
                return (this.Save());
            }
            else
            {
                return false;
            }
        }

        public bool Unlock()
        {
            if (this["LockerID"] == "")
                return false;

            this["LockerID"] = "";
            return (this.Save());
        }

        //public bool LogChanges(Guid? id)
        //{
        //    //TODO: 在添加,更新,删除条目前把旧版本的条目保存到历史记录中
        //    //TODO: 整理数据库的历史记录表

        //    if (Application.OpenForms["MainForm"] == null)
        //    {
        //        return false;
        //    }

        //    MainForm frm = (MainForm)Application.OpenForms["MainForm"];

        //    BaseModel oldEntry = Select(id);
        //    if (oldEntry["fs2guid"] != null && oldEntry["fs2guid"].Trim() != "")
        //    {
        //        oldEntry.Remove("LockerID");

        //        oldEntry["editor"] = frm.CurrentUser["Name"];
        //        oldEntry["Modified"] = DateTime.Now.ToString();
        //        oldEntry["log_guid"] = Guid.NewGuid().ToString();
        //        //TODO: 添加revision信息

        //        oldEntry.TableName = "_log_" + oldEntry.TableName;

        //        oldEntry.IsDirty = false;
        //        //oldEntry["fs2guid"] = Guid.NewGuid().ToString();
        //        string sql = BuildInsertCommandText(oldEntry);
        //        return DatabaseController.ExecuteNonQuery(sql) == 1;
        //    }

        //    return false;

        //}

        //根据数据模型自动创建insert SQL脚本
        protected string BuildInsertCommandText()
        {
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();

            fields.AppendFormat("insert into `{0}` (", this.TableName);
            values.Append(" values(");
            foreach (string key in this.Keys)
            {
                fields.AppendFormat("`{0}`,", key);
                values.AppendFormat("{0},", this[key].Trim() == "" ? "null" : "'" + this[key].Replace("'", "''") + "'");
            }

            string fieldSql = fields.ToString();
            fieldSql = fieldSql.TrimEnd(new char[]{','});
            fieldSql += ") ";

            string valueSql = values.ToString();
            valueSql = valueSql.TrimEnd(new char[] { ',' });
            valueSql += ")";

            string sql = fieldSql + valueSql;
            sql += ";select LAST_INSERT_ID();";

            return sql;
        }

        //根据数据模型自动创建update SQL脚本
        protected string BuildUpdateCommandText()
        {
            string value;
            StringBuilder fields = new StringBuilder();

            if (this.m_IsNew || this.KeyValue == 0)
                throw new ArgumentNullException("尚未保存过, 无法构建SQL更新语句");

            fields.AppendFormat("update `{0}` set ", this.TableName);
            foreach (string key in this.Keys)
            {
                if (key != this.KeyField)
                {
                    value = string.Format("{0}", this[key].Trim() == "" ? "null" : "'" + this[key].Replace("'", "''") + "'");
                    fields.AppendFormat("`{0}`={1},", key, value);
                }
            }

            string fieldSql = fields.ToString();
            fieldSql = fieldSql.TrimEnd(new char[] { ',' });

            string sql = string.Format(fieldSql + " where `{0}` = {1}", this.KeyField, this.KeyValue);
            return sql;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace DateExplorer.Core.Controllers
{
    public static class DatabaseController
    {
        private static string SERVER_ADDR = "192.168.8.241";
        private static string USERNAME = "dev";
        private static string PASSWORD = "dev99";
        private static string DATABASE = "fscraft_skill";
        private static string m_connectionString = null;
        static DatabaseController()
        {
            //TODO:从设置里读取连接字符串
            //m_connectionString = "Server=192.168.8.54;User Id=dev;Password=dev99;Database=fs2worldsettings;";

            m_connectionString = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false", SERVER_ADDR, USERNAME, PASSWORD, DATABASE);
        }

        
        public static MySqlDataReader ExecuteReader(string sql)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(m_connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    return cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                Logger.LogEvent(ex);
                throw ex;
            }
        }

        public static object ExecuteScalar(string sql)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(m_connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    object ret = cmd.ExecuteScalar();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                Logger.LogEvent(ex);
                throw ex;
            }
        }

        public static int ExecuteNonQuery(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(m_connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.LogEvent(ex);
                    throw ex;
                }
            }
        }

        public static DataSet ExecuteDataSet(string sql)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(m_connectionString))
                {
                    DataSet ds = new DataSet();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    conn.Open();
                    adapter.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                Logger.LogEvent(ex);
                throw ex;
            }
        }



    }
}

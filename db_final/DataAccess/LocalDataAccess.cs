using db_final.DataAccess.DataEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;

namespace db_final.DataAccess
{
    public class LocalDataAccess
    {
        private static LocalDataAccess instance;
        private LocalDataAccess() { }
        public static LocalDataAccess GetInstance()
        {
            return instance ?? (instance = new LocalDataAccess());
        }

        MySqlConnection conn;
        MySqlCommand comm;
        MySqlDataAdapter adapter;

        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        private bool DBConnection()
        {

            string connstr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            if (conn == null)
            {
                conn = new MySqlConnection(connstr);
            }
            try
            {
                conn.Open();
                // MessageBox.Show("hahah");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            try
            {
                if (DBConnection())
                {
                    string userSql = "select * from users where UserName=@UserName and PassWord = @PassWord";
                    adapter = new MySqlDataAdapter(userSql, conn);
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar)
                    {
                        Value = userName
                    });
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("@PassWord", MySqlDbType.VarChar)
                    {
                        Value = pwd
                    });

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);

                    if (count <= 0)
                        throw new Exception("用户名密码不正确！");

                    DataRow dr = table.Rows[0];

                    UserEntity userInfo = new UserEntity();
                    userInfo.UserName = dr.Field<string>("UserName");
                    userInfo.Password = dr.Field<string>("PassWord");
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
            return null;

        }

    }
}

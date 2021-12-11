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
using db_final.model;

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

        public List<BookModel> GetBookInfos(string Category="全部",string State="全部")
        {
            try
            {
                List<BookModel> bookModels = new List<BookModel>();
                if (DBConnection())
                {
                    string sql = "";
                    if (Category=="全部" && State == "全部")
                    {
                        sql = "SELECT * FROM final_db.bookinfo";
                    }
                    else if (Category!="全部" && State!="全部")
                    {
                        sql = "select * from final_db.bookinfo , borrow_table where bookinfo.BookNumber = borrow_table.BookNumber and ";
                        sql += "Category=\"" + Category + "\" and ";
                        sql += "available=" + ((State == "已借出") ? "0" : "1");
                    }else if (Category == "全部" )
                    {
                        sql = "select * from final_db.bookinfo , borrow_table where bookinfo.BookNumber = borrow_table.BookNumber ";
                        sql += "and ";
                        sql += "available=" + ((State == "已借出") ? "0" : "1");
                    }else if (State=="全部")
                    {
                        sql = "select * from final_db.bookinfo , borrow_table where bookinfo.BookNumber = borrow_table.BookNumber ";
                        sql += "and ";
                        sql += " Category=\"" + Category + "\"";
                    }

                    adapter = new MySqlDataAdapter(sql, conn);
                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);
                    BookModel bookinfos_fromdb = null;

                    if (count > 0)
                    {
                        foreach (var item in table.AsEnumerable())
                        {
                            bookinfos_fromdb = new BookModel();
                            bookinfos_fromdb.BookAuthor = "Author name : "+item.Field<string>("BookAuthor");
                            bookinfos_fromdb.BookName = item.Field<string>("BookName");
                            bookinfos_fromdb.BookUrl = item.Field<string>("BookUrl");
                            bookinfos_fromdb.BookDescription = item.Field<string>("BookDescription");
                            bookModels.Add(bookinfos_fromdb);
                        }
                    }
                    return bookModels;
                }
            return null;
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        public void SendBook2DB(BookModel BookAddInfo)
        {
            string name = BookAddInfo.BookName;
            string author = BookAddInfo.BookAuthor;
            string descp = BookAddInfo.BookDescription;
            string Url = BookAddInfo.BookUrl;
            string category = BookAddInfo.Category;

            try
            {
                if (DBConnection())
                {
                    string parameter = " @BookName , @BookAuthor , @BookDescription , @BookUrl , @Category ";
                    string sql = "INSERT INTO `final_db`.`bookinfo` (`BookName`, `BookAuthor`, `BookDescription`, `BookUrl`, `Category`) VALUES (" + parameter + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@BookName", MySqlDbType.VarChar)
                    {
                        Value = name
                    });
                    cmd.Parameters.Add(new MySqlParameter("@BookAuthor", MySqlDbType.VarChar)
                    {
                        Value = author
                    });
                    cmd.Parameters.Add(new MySqlParameter("@BookDescription", MySqlDbType.VarChar)
                    {
                        Value = descp
                    });
                    cmd.Parameters.Add(new MySqlParameter("@BookUrl", MySqlDbType.VarChar)
                    {
                        Value = Url
                    });
                    cmd.Parameters.Add(new MySqlParameter("@Category", MySqlDbType.VarChar)
                    {
                        Value = category
                    });
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

    }
}

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
                        sql += "available" + ((State == "已借出") ? "=0" : ">0");
                    }else if (Category == "全部" )
                    {
                        sql = "select * from final_db.bookinfo , borrow_table where bookinfo.BookNumber = borrow_table.BookNumber ";
                        sql += "and ";
                        sql += "available" + ((State == "已借出") ? "=0" : ">1");
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
        public void DeleteBook(string BookName)
        {
            try
            {
                if (DBConnection())
                {
                    string sql = "delete from bookinfo where BookName = @BookName";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@BookName", MySqlDbType.VarChar)
                    {
                        Value = BookName
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

        public List<StudentModel> GetStudentInfo()
        {
            try
            {
                List<StudentModel> StudentInfoList = new List<StudentModel>();
                if(DBConnection())
                {
                    string sql = "select * from studentinfo";
                    adapter = new MySqlDataAdapter(sql, conn);
                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);
                    if (count <= 0)
                        throw new Exception("查询失败！");
                    
                    foreach (var item in table.AsEnumerable())
                    {
                        StudentModel newstudent = new StudentModel();
                        newstudent.Name = item.Field<string>("Name");
                        newstudent.StudentID = item.Field<string>("StudentID");
                        newstudent.Depart = item.Field<string>("Depart");
                        newstudent.BorrowNumber = item.Field<int>("BorrowNumber"); // 以列名的方式，得到列索引
                        StudentInfoList.Add(newstudent);
                    }
                    List<StudentModel> result = StudentInfoList;
                    return result;

                }
                return null;
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


        public void SendStudent2DB(StudentModel StudentInfo)
        {
            string name = StudentInfo.Name;
            string StudentID = StudentInfo.StudentID;
            string Depart = StudentInfo.Depart;
            int BorrowNumber = StudentInfo.BorrowNumber;
            if(name == "请输入姓名"|| Depart == "请输入学院"|| StudentID == "请输入学生ID")
            {
                throw new Exception("请插入正确学生信息");
            }
            try
            {
                if (DBConnection())
                {
                    string parameter = " @Name , @StudentID , @Depart , @BorrowNumber ";
                    string sql = "INSERT INTO `final_db`.`studentinfo` (`Name`, `StudentID`, `Depart`, `BorrowNumber`) VALUES (" + parameter + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar)
                    {
                        Value = name
                    });
                    cmd.Parameters.Add(new MySqlParameter("@StudentID", MySqlDbType.VarChar)
                    {
                        Value = StudentID
                    });
                    cmd.Parameters.Add(new MySqlParameter("@Depart", MySqlDbType.VarChar)
                    {
                        Value = Depart
                    });
                    cmd.Parameters.Add(new MySqlParameter("@BorrowNumber", MySqlDbType.Int32)
                    {
                        Value = BorrowNumber
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


        public List<string> BorrowBook_Info(string studentID)
        {
            List<string> result;
            List<string> BookNameList = new List<string>();
            try
            {
                if (DBConnection())
                {
                    string sql = @"select distinct(BookName) from ss_borrow_table,borrow_table,bookinfo
where ss_borrow_table.SsID = @StudendID and ss_borrow_table.BookNumber = bookinfo.BookNumber and borrow_table.BookNumber = ss_borrow_table.BookNumber; ";
                    adapter = new MySqlDataAdapter(sql, conn);
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("@StudendID", MySqlDbType.VarChar)
                    {
                        Value = studentID
                    });

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);


                    if (count > 0)
                    {
                        foreach (var item in table.AsEnumerable())
                        {
                            string BookName;
                            BookName = item.Field<string>("BookName");
                            //bookinfos_fromdb.BookAuthor = "Author name : " + item.Field<string>("BookAuthor");
                            //bookinfos_fromdb.BookName = item.Field<string>("BookName");
                            //bookinfos_fromdb.BookUrl = item.Field<string>("BookUrl");
                            //bookinfos_fromdb.BookDescription = item.Field<string>("BookDescription");
                            BookNameList.Add(BookName);
                        }
                        result = BookNameList;
                        return result;
                    }
                }
                return null;
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

        public void DeleteBorrowBook(string BookName,string StudentID)
        {
            try
            {
                if (DBConnection())
                {
                    string sql = "delete from ss_borrow_table where ss_borrow_table.SsID = @StudentID and ss_borrow_table.BookNumber in (select BookNumber from bookinfo where bookinfo.BookName = @BookName);";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@BookName", MySqlDbType.VarChar)
                    {
                        Value = BookName
                    });

                    cmd.Parameters.Add(new MySqlParameter("@StudentID", MySqlDbType.VarChar)
                    {
                        Value = StudentID
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


        public void SendBorrowInfo2DB(string ID,string BookInfo)
        {
            try
            {
                if (DBConnection())
                {
                    int booknumber = 0;
                    string usersql1 = "select BookNumber from bookinfo where BookName = @BookName";
                    MySqlCommand cmd = new MySqlCommand(usersql1, conn);
                    cmd.Parameters.Add(new MySqlParameter("@BookName", MySqlDbType.VarChar)
                    {
                        Value = BookInfo
                    });
                    MySqlDataReader read = cmd.ExecuteReader();
                    if (read.Read()==false)
                    {
                        throw new Exception(" 书籍:"+ BookInfo + " 不存在");
                    }
                    booknumber = read.GetFieldValue<int>(0);
                    cmd.Dispose();
                    read.Close();

                    string parameter = " @SsID , @BookNumber ";
                    string sql = "INSERT INTO `final_db`.`ss_borrow_table` (`SsID`, `BookNumber`) VALUES (" + parameter + ");";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@SsID", MySqlDbType.VarChar)
                    {
                        Value = ID
                    });
                    cmd.Parameters.Add(new MySqlParameter("@BookNumber", MySqlDbType.Int32)
                    {
                        Value = booknumber
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

        public void DeleteFromSsLib(string Name)
        {
            try
            {
                if (DBConnection())
                {
                    string sql = "delete from studentinfo where Name=@Name";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar)
                    {
                        Value = Name
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
        public string GetSsID(string userName)
        {
            try
            {
                if (DBConnection())
                {
                    string userSql = "select StudentID from studentinfo where Name = @userName";
                    adapter = new MySqlDataAdapter(userSql, conn);
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar)
                    {
                        Value = userName
                    });
                    

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);

                    

                    DataRow dr = table.Rows[0];

                    
                    string SsID;
                    SsID = dr.Field<string>("StudentID");
                    
                    return SsID;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }


        public StudentModel GetStudentInfo(string Sname)
        {
            try
            {
                StudentModel StudentInfoList = new StudentModel();
                if (DBConnection())
                {
                    string sql = "select * from studentinfo where Name = @Name ";
                    adapter = new MySqlDataAdapter(sql, conn);
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("@Name", MySqlDbType.VarChar)
                    {
                        Value = Sname
                    });

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);
                    if (count <= 0)
                        throw new Exception("查询失败！");
                    StudentModel newstudent = new StudentModel();
                    foreach (var item in table.AsEnumerable())
                    {
                        newstudent.Name = item.Field<string>("Name");
                        newstudent.StudentID = item.Field<string>("StudentID");
                        newstudent.Depart = item.Field<string>("Depart");
                        newstudent.BorrowNumber = item.Field<int>("BorrowNumber"); // 以列名的方式，得到列索引
                    }
                    return newstudent;
                }
                return null;
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

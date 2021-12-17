using db_final.Common;
using db_final.DataAccess;
using db_final.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace db_final.viewmodel
{

    
    public class ShowSsInfoViewModel
    {
        public StudentModel StudentModel { get; set; } = new StudentModel();
        public CommandBase Btn_DeleteBorrowBook { get; set; }
        public CommandBase Btn_addBookInfo { get; set; }
        public CommandBase BtnOKPressed { get; set; }
        
        private ObservableCollection<booklistModel> BookInfos = new ObservableCollection<booklistModel>();


        private ICollectionView bookinfoview_;

        private List<string> BooksfromDB = new List<string>();

        private bool modify_flag = false;

        private int add_book_num = 0;
        public ICollectionView BookInfoListView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(BookInfos);
            }
            set { bookinfoview_ = value; }
        }

        public ShowSsInfoViewModel()
        {
            //this.StudentModel.Name = "111";
            this.Btn_DeleteBorrowBook = new CommandBase();
            this.Btn_DeleteBorrowBook.DoCanExecute = new Func<object, bool>((o) => true);
            this.Btn_DeleteBorrowBook.DoExecute = new Action<object>(DeleteBookBorrowInfo);
            this.BookInfos.Add(new booklistModel("113"));
        }


        public ShowSsInfoViewModel(StudentModel SsInfo)  // 管理员界面信息
        {
            //this.StudentModel.Name = "111";
            this.Btn_DeleteBorrowBook = new CommandBase();
            this.Btn_DeleteBorrowBook.DoCanExecute = new Func<object,bool>((o) => true);
            this.Btn_DeleteBorrowBook.DoExecute = new Action<object>(DeleteBookBorrowInfo);


            this.Btn_addBookInfo = new CommandBase();
            this.Btn_addBookInfo.DoCanExecute = new Func<object, bool>((o) => true);
            this.Btn_addBookInfo.DoExecute = new Action<object>(AddBookBorrowInfo);

            this.BtnOKPressed = new CommandBase();
            this.BtnOKPressed.DoCanExecute = new Func<object, bool>((o) => true);
            this.BtnOKPressed.DoExecute = new Action<object>(BookBorrowOK);

            this.modify_flag = false;
            this.add_book_num = 0;
            //StudentModel = SsInfo;
            StudentModel = LocalDataAccess.GetInstance().GetStudentInfo(SsInfo.Name); // 刷新数据信息
            BooksfromDB = LocalDataAccess.GetInstance().BorrowBook_Info(StudentModel.StudentID);
            if (BooksfromDB == null)
            {
                ;
            }
            else
            {
                foreach(var item in BooksfromDB)
                {
                    this.BookInfos.Add(new booklistModel(item));
                }

            }
        }

        public ShowSsInfoViewModel(string Sname) // 用户界面的数据信息
        {
            //this.StudentModel.Name = "111";
            this.Btn_DeleteBorrowBook = new CommandBase();
            this.Btn_DeleteBorrowBook.DoCanExecute = new Func<object, bool>((o) => true);
            this.Btn_DeleteBorrowBook.DoExecute = new Action<object>(DeleteBookBorrowInfo);


            this.Btn_addBookInfo = new CommandBase();
            this.Btn_addBookInfo.DoCanExecute = new Func<object, bool>((o) => true);
            this.Btn_addBookInfo.DoExecute = new Action<object>(AddBookBorrowInfo);

            this.BtnOKPressed = new CommandBase();
            this.BtnOKPressed.DoCanExecute = new Func<object, bool>((o) => true);
            this.BtnOKPressed.DoExecute = new Action<object>(UserPage_BookBorrowOK);

            this.modify_flag = false;
            this.add_book_num = 0;
            StudentModel = LocalDataAccess.GetInstance().GetStudentInfo(Sname); // 刷新数据信息
            //StudentModel.Name = Sname;
            string SID = LocalDataAccess.GetInstance().GetSsID(Sname);
            BooksfromDB = LocalDataAccess.GetInstance().BorrowBook_Info(SID);
            foreach (var item in BooksfromDB)
            {
                this.BookInfos.Add(new booklistModel(item));
            }
        }



        //private void DeleteBookBorrowInfo(object o)
        //{
        //    LocalDataAccess.GetInstance().DeleteBorrowBook(BookName: o.ToString(), StudentID: StudentModel.StudentID);
        //}

        private void DeleteBookBorrowInfo(object o)
        {
            LocalDataAccess.GetInstance().DeleteBorrowBook(BookName: o.ToString(), StudentID: StudentModel.StudentID);
            this.BookInfos.Clear();

            BooksfromDB = LocalDataAccess.GetInstance().BorrowBook_Info(StudentModel.StudentID);
            if (BooksfromDB != null)
            {
                foreach (var item in BooksfromDB)
                {
                    this.BookInfos.Add(new booklistModel(item));
                }
            }
            else
            {

            }
            StudentModel.BorrowNumber = LocalDataAccess.GetInstance().GetStudentInfo(this.StudentModel.Name).BorrowNumber; // 刷新数据信息
            modify_flag = true;
        }

        private void AddBookBorrowInfo(object o)
        {
            add_book_num++;
            modify_flag = true;
            this.BookInfos.Add(new booklistModel("请输入书名"));
        }

        private void BookBorrowOK(object o)
        {
            //MessageBox.Show("若不需要修改，请点击取消按键");

            if (modify_flag == false)
            {
                MessageBox.Show("若不需要修改，请点击取消按键");
                return;
            }

            int len = BookInfos.Count();
            List<int> removenum = new List<int>();
            for (int i = len; i > len - add_book_num; i--)
            {
                booklistModel newss = BookInfos.ElementAt(i - 1);
                //MessageBox.Show(newss.Name+" "+newss.Depart);
                try
                {
                    LocalDataAccess.GetInstance().SendBorrowInfo2DB(StudentModel.StudentID,newss.bookname);
                    this.StudentModel.BorrowNumber++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("插入失败"+ex.Message);
                    removenum.Add(i-1);
                }
            }
            if (removenum.Count() != 0)
            {
                foreach (var item in removenum)
                {
                    BookInfos.RemoveAt(item - 1);
                }
            }


            add_book_num = 0;
            (o as Window).Close();
            //this.BookInfos.Add(new booklistModel("请输入书名"));

        }

        private void UserPage_BookBorrowOK(object o)
        {
            //MessageBox.Show("若不需要修改，请点击取消按键");

            if (modify_flag == false)
            {
                MessageBox.Show("若不需要修改，请点击取消按键");
                return;
            }

            int len = BookInfos.Count();
            List<int> removenum = new List<int>();
            for (int i = len; i > len - add_book_num; i--)
            {
                booklistModel newss = BookInfos.ElementAt(i - 1);
                //MessageBox.Show(newss.Name+" "+newss.Depart);
                try
                {
                    LocalDataAccess.GetInstance().SendBorrowInfo2DB(StudentModel.StudentID, newss.bookname);
                    this.StudentModel.BorrowNumber++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("插入失败" + ex.Message);
                    removenum.Add(i - 1);
                }
            }
            if (removenum.Count() != 0)
            {
                foreach (var item in removenum)
                {
                    BookInfos.RemoveAt(item );
                }
            }


            add_book_num = 0;
            //this.BookInfos.Add(new booklistModel("请输入书名"));

        }


    }

    
}

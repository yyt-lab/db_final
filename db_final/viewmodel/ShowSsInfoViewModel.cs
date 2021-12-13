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
using System.Windows.Data;

namespace db_final.viewmodel
{

    
    public class ShowSsInfoViewModel
    {
        public StudentModel StudentModel { get; set; } = new StudentModel();
        public CommandBase Btn_DeleteBorrowBook { get; set; }

        private ObservableCollection<booklistModel> BookInfos = new ObservableCollection<booklistModel>();


        private ICollectionView bookinfoview_;

        private List<string> BooksfromDB = new List<string>();
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


        public ShowSsInfoViewModel(StudentModel SsInfo)
        {
            //this.StudentModel.Name = "111";
            this.Btn_DeleteBorrowBook = new CommandBase();
            this.Btn_DeleteBorrowBook.DoCanExecute = new Func<object,bool>((o) => true);
            this.Btn_DeleteBorrowBook.DoExecute = new Action<object>(DeleteBookBorrowInfo);

            StudentModel = SsInfo;
            BooksfromDB = LocalDataAccess.GetInstance().BorrowBook_Info(StudentModel.StudentID);
            foreach(var item in BooksfromDB)
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
            foreach (var item in BooksfromDB)
            {
                this.BookInfos.Add(new booklistModel(item));
            }
        }


    }

    
}

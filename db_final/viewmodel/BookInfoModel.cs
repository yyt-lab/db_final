using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using db_final.Common;
using db_final.DataAccess;
using db_final.model;

namespace db_final.viewmodel
{
    public class BookInfoModel 
    {
        public ObservableCollection <CateItemModel> CategoryBooks { get; set; }
        public ObservableCollection <CateItemModel> IsBorrowed { get; set; }
        public ObservableCollection <BookModel> BookInfoList_FromDB { get; set; }
        public CommandBase Category_MenuChanged { get; set; }
        public CommandBase Available_MenuChanged { get; set; }
        private string CurrentCategory;
        private string CurrentState;
        public BookInfoModel()
        {
            this.Category_MenuChanged = new CommandBase();
            this.Category_MenuChanged.DoCanExecute = new Func<object, bool>((o) => true);
            this.Category_MenuChanged.DoExecute = new Action<object>(DoCategoryfilter);

            this.Available_MenuChanged = new CommandBase();
            this.Available_MenuChanged.DoCanExecute = new Func<object, bool>((o) => true);
            this.Available_MenuChanged.DoExecute = new Action<object>(DoAvailablefilter);

            this.CurrentCategory = "";
            this.CurrentState = "";

            InitCategory();
            InitBookInfo();
        }
        private void DoCategoryfilter(object o)
        {
            CurrentCategory = o.ToString();
            BookInfoList_FromDB.Clear();

            foreach (var item in LocalDataAccess.GetInstance().GetBookInfos(Category: CurrentCategory))
            {
                BookInfoList_FromDB.Add(item);
            }
        }
        private void DoAvailablefilter(object o)
        {
            CurrentState = o.ToString();
            BookInfoList_FromDB.Clear();
            //BookInfoList_FromDB = new ObservableCollection<BookModel>(LocalDataAccess.GetInstance().GetBookInfos(State: CurrentState));
            foreach (var item in LocalDataAccess.GetInstance().GetBookInfos(State: CurrentState))
            {
                BookInfoList_FromDB.Add(item);
            }
        }
        private void InitBookInfo()
        {
            BookInfoList_FromDB = new ObservableCollection<BookModel>(LocalDataAccess.GetInstance().GetBookInfos());

        }

        private void InitCategory()
        {
            this.CategoryBooks = new ObservableCollection<CateItemModel>();
            this.CategoryBooks.Add(new CateItemModel("全部", true));
            this.CategoryBooks.Add(new CateItemModel("理工类"));
            this.CategoryBooks.Add(new CateItemModel("文学类"));
            this.CategoryBooks.Add(new CateItemModel("教材类"));

            this.IsBorrowed = new ObservableCollection<CateItemModel>();
            this.IsBorrowed.Add(new CateItemModel("全部", true));
            this.IsBorrowed.Add(new CateItemModel("已借出"));
            this.IsBorrowed.Add(new CateItemModel("未借出"));
        }

        public void UpdateBookInfo()
        {
        }
    }

}

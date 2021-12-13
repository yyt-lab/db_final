using db_final.Common;
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

        private ObservableCollection<booklistModel> BookInfos = new ObservableCollection<booklistModel>();


        private ICollectionView bookinfoview_;

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

            this.BookInfos.Add(new booklistModel("113"));
        }


        public ShowSsInfoViewModel(StudentModel SsInfo)
        {
            //this.StudentModel.Name = "111";

            //this.BookInfos.Add(new booklistModel("113"));
            StudentModel = SsInfo;

        }


    }

    
}

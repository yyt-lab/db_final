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
    public class StudentBorrowModel
    {
        private ObservableCollection<StudentModel> studentCollection = new ObservableCollection<StudentModel>();

        public CommandBase btn_add { get; set; }
        public CommandBase btn_modifyok  { get; set; }

        private ICollectionView studentlistview_;

        private bool btn_Modifyok_valid = false;
        private int modify_num = 0;
        public ICollectionView StudnetListView
        {
            get { 
                    return CollectionViewSource.GetDefaultView(studentCollection);
                }
            set { studentlistview_ = value; }
        }

        public StudentBorrowModel()
        {
            this.btn_add = new CommandBase();
            this.btn_add.DoCanExecute = new Func<object,bool>(CanAddEmpty);
            this.btn_add.DoExecute = new Action<object>(newEmptyStudent);

            this.btn_modifyok = new CommandBase();
            this.btn_modifyok.DoCanExecute = new Func<object, bool>(CanModify);
            this.btn_modifyok.DoExecute = new Action<object>(SetModify2DB);

            loaddata();
        }


        private void loaddata()
        {
            //StudentModel ssmodel = new StudentModel();
            //ssmodel.Name = "yyt";
            //ssmodel.StudentID = "202078";
            //ssmodel.Depart = "cs";
            //ssmodel.BorrowNumber = 1;
            List<StudentModel> sslist =  LocalDataAccess.GetInstance().GetStudentInfo();
            foreach (var item in sslist)
            {
                studentCollection.Add(item);
            }
        }

        public void newEmptyStudent(object o)
        {
            modify_num++;
            StudentModel newstudent = new StudentModel();
            studentCollection.Add(newstudent);
        }

        public bool CanAddEmpty(object o)
        {
            this.btn_Modifyok_valid = true;
            return true;
        }

        public bool CanModify(object o)
        {
            return true;
        }

        private void SetModify2DB(object o)
        {
            if (modify_num==0)
            {
                MessageBox.Show("请插入数据再点击确认");
                return;
            }
            int len = studentCollection.Count();
            List<int> removenum = new List<int>();
            for (int i=len;i > len- modify_num; i--)
            {
                StudentModel newss = studentCollection.ElementAt(i-1);
                //MessageBox.Show(newss.Name+" "+newss.Depart);
                try
                {
                    LocalDataAccess.GetInstance().SendStudent2DB(newss);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("插入失败,姓名学号不能为空");
                    removenum.Add(i);
                }
            }
            if (removenum.Count() != 0)
            {
                foreach(var item in removenum)
                {
                    studentCollection.RemoveAt(item-1);
                }
            }

            
            modify_num = 0;
        }
    }

   
}

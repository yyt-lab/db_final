using db_final.Common;
using db_final.DataAccess;
using db_final.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace db_final.viewmodel
{

    public class AddBookViewModel
    {
        public BookModel BookAddInfo { get; set; } = new BookModel();

        public CommandBase BtnOKPressed { get; set; } = new CommandBase();
        public AddBookViewModel()
        {
            this.BookAddInfo.BookAuthor = "yyt";
            this.BookAddInfo.BookUrl = "yyt";
            this.BookAddInfo.BookDescription = "yyt";
            this.BtnOKPressed.DoCanExecute = new Func<object, bool>((o) => true);
            this.BtnOKPressed.DoExecute = new Action<object>(Send2DB);
        }

        public void Send2DB(object o)
        {
            try
            {
                LocalDataAccess.GetInstance().SendBook2DB(BookAddInfo);
                Application.Current.Dispatcher.Invoke(new Action(() =>  // 操作窗口线程的控件，必须这样写
                {
                    (o as Window).Close();
                }));
            }
            catch (Exception e)
            {
                MessageBox.Show("插入失败，请重新尝试\n"+e.Message);
            }

        }

    }

       

    
}

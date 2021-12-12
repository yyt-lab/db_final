using db_final.model;
using db_final.viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace db_final.view
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>


    //public class booklist
    //{
    //    string bookname;

    //    public booklist()
    //    {

    //    }

    //    public booklist(string name)
    //    {
    //        this.bookname = name;
    //    }
    //}

    public partial class ShowStudentInfoView : Window
    {

        
        
        public ShowStudentInfoView()
        {
            //context  = new ShowSsInfoViewModel();
            InitializeComponent();
            this.DataContext = new ShowSsInfoViewModel();
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

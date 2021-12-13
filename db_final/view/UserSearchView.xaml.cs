using db_final.viewmodel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace db_final.view
{
    /// <summary>
    /// UserSearchView.xaml 的交互逻辑
    /// </summary>
    public partial class UserSearchView : UserControl
    {
        public UserSearchView()
        {
            InitializeComponent();
            this.DataContext = new StudentBorrowModel();
        }

        private void Button_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}

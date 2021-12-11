using db_final.viewmodel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// BooksInfo.xaml 的交互逻辑
    /// </summary>
    public partial class BooksInfo : UserControl
    {
        public BooksInfo()
        {
            InitializeComponent();

            this.DataContext = new BookInfoModel();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainView().ShowDialog();
        }
    }
}

using db_final.Common;
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
using System.Windows.Shapes;

namespace db_final.view
{
    /// <summary>
    /// adminView.xaml 的交互逻辑
    /// </summary>
    public partial class adminView : Window
    {
        public adminView()
        {
            InitializeComponent();
            MainViewModel model = new MainViewModel();
            this.DataContext = model;
            model.UserInfo.UserName = "UserID : "+GlobalValues.UserInfo.UserName;
            this.MaxHeight = SystemParameters.PrimaryScreenHeight;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void btnMin_click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMax_click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else 
                this.WindowState = WindowState.Maximized;
        }

        private void btnClose_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

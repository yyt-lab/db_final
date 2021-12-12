using db_final.Common;
using db_final.view;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
//using db_final.view;
namespace db_final
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (new LoginView().ShowDialog() == true)  //之后关闭该view时才会返回
            {
                if (GlobalValues.UserInfo.UserName == "admin")  //打开管理员视图
                {
                    new adminView().ShowDialog();
                }
                else
                {
                    ;
                    //new MainView().ShowDialog();  // 打开MainView,等待返回
                } 
            }
            // 只有主窗口关闭的时候，才会关闭这个窗口
            Application.Current.Shutdown();
        }
    }
}

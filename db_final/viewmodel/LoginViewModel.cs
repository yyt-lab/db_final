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
    class LoginViewModel : NotifyBase
    {
        public CommandBase CloseWindowCommand { get; set; }
        public CommandBase LoginCommand { get; set; }
        public LoginModel LoginModel { get; set; } = new LoginModel();

        private string _errormassage;

        public string ErrorMessage
        {
            get { return _errormassage; }
            set { _errormassage = value; this.DoNotify(); }
        }

        public LoginViewModel()
        {
            //this.LoginModel.UserName = "yyyt";
            //this.LoginModel.PassWord = "123123";
            this.CloseWindowCommand = new CommandBase();
            this.CloseWindowCommand.DoExecute = new Action<object>((o) => {  // 匿名委托
                (o as Window).Close();
            });
            this.CloseWindowCommand.DoCanExecute = new Func<object, bool>((o) => { return true;});
            this.LoginCommand = new CommandBase();
            this.LoginCommand.DoExecute = new Action<object>(DoLogin); // 封装一个带参数的方法，并用DoLogin实例化
            this.LoginCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });//封装一个参数&一个返回值的方法
        }
        private void DoLogin(object o)   // 处理登录逻辑
        {
            this.ErrorMessage = "";
            //this.ShowProgress = Visibility.Visible;
            if (String.IsNullOrEmpty(LoginModel.UserName))
            {
                this.ErrorMessage = "请输入用户名!";
                return;
            }

            if (String.IsNullOrEmpty(LoginModel.PassWord))
            {
                this.ErrorMessage = "请输入密码!";
                return;
            }

            try
            {
                var user = LocalDataAccess.GetInstance().CheckUserInfo(LoginModel.UserName, pwd: LoginModel.PassWord);
                if (user == null)
                {
                    throw new Exception("登陆失败！用户名获密码错误");
                }
                GlobalValues.UserInfo = user;

                Application.Current.Dispatcher.Invoke(new Action(() =>  // 操作窗口线程的控件，必须这样写
                {
                    (o as Window).DialogResult = true;
                }));
                   

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}

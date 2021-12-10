using db_final.Common;
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
            this.LoginModel.PassWord = "123123";
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
            this.ErrorMessage ="";
            if (String.IsNullOrEmpty(LoginModel.PassWord))
            {
                this.ErrorMessage = "请输入密码!";
                return;
            }
            if (String.IsNullOrEmpty(LoginModel.UserName))
            {
                this.ErrorMessage = "请输入用户名!";
                return;
            }

            MessageBox.Show("hhhaha");
        }

    }
}

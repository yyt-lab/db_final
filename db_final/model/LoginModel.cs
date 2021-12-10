using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_final.model
{
    public class LoginModel : NotifyBase
    {
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set {
                _userName = value; // 默认值
                this.DoNotify();
            }
        }

        private string _password;

        public string PassWord
        {
            get { return _password; }
            set { _password = value;
                this.DoNotify();
            }
        }
    }
}

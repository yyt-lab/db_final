using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_final.model
{
    public class UserModel : NotifyBase
    {
        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                this.DoNotify();
            }
        }
    }
}

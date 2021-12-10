using db_final.DataAccess.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_final.Common
{
    public class GlobalValues
    {
        public static UserEntity UserInfo {get; set;}//设置全局静态变量，可以直接使用
    }
}

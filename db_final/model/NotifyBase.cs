using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace db_final.model
{
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // 创建带有通知属性的输入输出
        public void DoNotify([CallerMemberName] string propName="" )  //参数默认值为caller member的值
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName)); // 只有非NULL的情况，才会赋值
        }
    }
}

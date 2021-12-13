using db_final.Common;
using db_final.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace db_final.viewmodel
{
    class MainViewModel: NotifyBase
    {
        public UserModel UserInfo { get; set; } = new UserModel();

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; this.DoNotify(); }
        }

        private FrameworkElement _mainContent;

        public FrameworkElement MainContent
        {
            get { return _mainContent; }
            set { _mainContent = value; this.DoNotify(); }
        }

        public CommandBase NavCommandChanged { get; set; }

        public MainViewModel()
        {
            this.NavCommandChanged = new CommandBase();
            this.NavCommandChanged.DoExecute = new Action<object>(DoNavChanged);
            this.NavCommandChanged.DoCanExecute = new Func<object, bool>((o) => true);

            DoNavChanged("FirstPage");
        }

        public MainViewModel(string UserPageName)
        {
            this.NavCommandChanged = new CommandBase();
            this.NavCommandChanged.DoExecute = new Action<object>(DoNavChanged);
            this.NavCommandChanged.DoCanExecute = new Func<object, bool>((o) => true);

            DoNavChanged(UserPageName);
        }


        private void DoNavChanged(object obj)
        {
            Type type = Type.GetType("db_final.view." + obj.ToString());
            ConstructorInfo cti = type.GetConstructor(System.Type.EmptyTypes);
            this.MainContent = (FrameworkElement)cti.Invoke(null);
        }
    }
}

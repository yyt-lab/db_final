using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_final.Common
{
   
    public class booklistModel
    {
        public string bookname { get; set; }

        public booklistModel()
        {

        }

        public booklistModel(string name)
        {
            this.bookname = name;
        }
    }
}

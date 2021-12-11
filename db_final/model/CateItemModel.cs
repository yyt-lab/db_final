using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_final.model
{
   public class CateItemModel
    {
        public CateItemModel()
        {
        }

        public CateItemModel(string name ,bool state = false)
        {
            this.IsSelected = state;
            this.CategoryName = name;
        }
        public bool IsSelected { get; set; }
        public string CategoryName { get; set; }
    }
}

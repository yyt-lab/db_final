using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db_final.model
{
    public class StudentModel
    {
        public string Name { get; set; }
        public string Depart { get; set; }
        public string StudentID { get; set; }
        public int BorrowNumber { get; set; }

        public List<string> BookNameList { get; set; }

        public StudentModel()
        {
            this.Name = "请输入姓名";
            this.Depart = "请输入学院";
            this.StudentID = "请输入学生ID";
            this.BookNameList = new List<string>();
        }
    }
}

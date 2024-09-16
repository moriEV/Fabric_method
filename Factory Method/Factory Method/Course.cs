using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method
{
    internal class Course : Product
    {
        public List<int> studentsId { get; set; } = new List<int>();
        public int teacherId { get; set; }
       
    }
}

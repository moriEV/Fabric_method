using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method
{
    internal class Student : Product
    {
        public List<int> Courses { get; set; } = new List<int>();
    }
}

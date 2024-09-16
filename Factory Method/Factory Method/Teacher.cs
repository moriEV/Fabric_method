using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method
{
    internal class Teacher : Product
    {
        public int Experience { get; set; }
        public List<int> Courses { get; set; } = new List<int>();
    }
}

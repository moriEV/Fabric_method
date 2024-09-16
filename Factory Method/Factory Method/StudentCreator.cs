using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method
{
    internal class StudentCreator : Creator
    {
        public override Product FactoryMethod(string[] parametrs)
        {
            return new Student
            {
                Id = int.Parse(parametrs[0]),
                Name = parametrs[1],
                Courses = new List<int>()
            };
        }
    }
}

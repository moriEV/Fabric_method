using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method
{
    internal class CourseCreator : Creator
    {
        public override Product FactoryMethod(string[] parametrs)
        {
            return new Course
            {
                Id = int.Parse(parametrs[0]),
                Name = parametrs[1],
                teacherId = int.Parse(parametrs[2]),
                studentsId = new List<int>()
            };
        }
    }
}

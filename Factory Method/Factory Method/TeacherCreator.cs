using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method
{
    internal class TeacherCreator : Creator
    {
        public override Product FactoryMethod(string[] parametrs)
        {
            return new Teacher
            {
                Id = int.Parse(parametrs[0]),
                Name = parametrs[1],
                Experience = int.Parse(parametrs[2]),
                Courses = new List<int>()
            };
        }
    }
}

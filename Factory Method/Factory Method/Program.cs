using Factory_Method;

internal class Program
{
    static List<Product> products = new List<Product>();
    static void Main(string[] args)
    {
        string filePath = "data.txt";
        LoadFromFile(filePath);

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Добавить учителя");
            Console.WriteLine("3. Добавить курс");
            Console.WriteLine("4. Сохранить данные");
            Console.WriteLine("5. Показать все данные");
            Console.WriteLine("6. Показать связанные объекты");
            Console.WriteLine("7. Выход");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    AddTeacher();
                    break;
                case "3":
                    AddCourse();
                    break;
                case "4":
                    SaveToFile(filePath);
                    Console.WriteLine("Данные сохранены.");
                    break;
                case "5":
                    ShowAllProducts();
                    break;
                case "6":
                    ShowRelatedProducts();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    break;
            }
        }
    }

    static void AddStudent()
    {
        Console.Write("Введите ID студента: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Введите имя студента: ");
        string name = Console.ReadLine();

        var studentCreator = new StudentCreator();
        studentCreator.AnOperation(new string[] { id.ToString(), name });

        Console.Write("Введите ID курсов (через запятую): ");
        var coursesInput = Console.ReadLine();
        var coursesIds = Array.ConvertAll(coursesInput.Split(','), int.Parse);

        var student = (Student)studentCreator.GetProduct();
        student.Courses.AddRange(coursesIds);

        products.Add(student);
    }

    static void AddTeacher()
    {
        Console.Write("Введите ID учителя: ");
        int teacherId = int.Parse(Console.ReadLine());

        Console.Write("Введите имя учителя: ");
        string name = Console.ReadLine();

        Console.Write("Введите опыт работы учителя (в годах): ");
        int experience = int.Parse(Console.ReadLine());

        Console.Write("Введите ID курса, который будет вести учитель: ");
        int courseId = int.Parse(Console.ReadLine());


        var existingCourse = products.OfType<Course>().FirstOrDefault(c => c.Id == courseId);
        if (existingCourse == null)
        {
            Console.WriteLine($"Курс с ID {courseId} не найден. Учитель не может быть добавлен.");
            return;
        }

        var teacher = new Teacher
        {
            Id = teacherId,
            Name = name,
            Experience = experience,
            Courses = new List<int> { courseId } 
        };

        products.Add(teacher);
        Console.WriteLine($"Учитель {name} успешно добавлен.");
    }


    static void AddCourse()
{
    Console.Write("Введите ID курса: ");
    int id = int.Parse(Console.ReadLine());

    Console.Write("Введите название курса: ");
    string name = Console.ReadLine();

    Console.Write("Введите ID учителя: ");
    int teacherId = int.Parse(Console.ReadLine());

    var existingTeacher = products.OfType<Teacher>().FirstOrDefault(t => t.Id == teacherId);
    if (existingTeacher != null && existingTeacher.Courses.Count > 0)
    {
        Console.WriteLine("Этот учитель уже ведет курс. Каждый учитель может вести только один курс.");
        return;
    }

    var courseCreator = new CourseCreator();
    courseCreator.AnOperation(new string[] { id.ToString(), name, teacherId.ToString() });

    Console.Write("Введите ID студентов (через запятую): ");
    var studentsInput = Console.ReadLine();
    var studentsIds = Array.ConvertAll(studentsInput.Split(','), int.Parse);

    var course = (Course)courseCreator.GetProduct();
    course.studentsId.AddRange(studentsIds);

    if (existingTeacher != null)
    {
        existingTeacher.Courses.Add(id);
    }

    products.Add(course);
}


    static void SaveToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var product in products)
            {
                switch (product)
                {
                    case Student student:
                        writer.WriteLine($"student,{student.Id},{student.Name}," + string.Join(";", student.Courses));
                        break;
                    case Teacher teacher:
                        writer.WriteLine($"teacher,{teacher.Id},{teacher.Name},{teacher.Experience}," + string.Join(";", teacher.Courses));
                        break;
                    case Course course:
                        writer.WriteLine($"course,{course.Id},{course.Name},{course.teacherId}," + string.Join(";", course.studentsId));
                        break;
                }
            }
        }
    }

    static void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return;

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                switch (parts[0])
                {
                    case "student":
                        var studentCreator = new StudentCreator();
                        studentCreator.AnOperation(new string[] { parts[1], parts[2] });
                        var student = (Student)studentCreator.GetProduct();

                        if (parts.Length > 3)
                        {
                            var coursesIds = Array.ConvertAll(parts[3].Split(';'), int.Parse);
                            student.Courses.AddRange(coursesIds);
                        }

                        products.Add(student);
                        break;

                    case "teacher":
                        var teacherCreator = new TeacherCreator();
                        teacherCreator.AnOperation(new string[] { parts[1], parts[2], parts[3] });
                        var teacher = (Teacher)teacherCreator.GetProduct();

                        if (parts.Length > 4)
                        {
                            var coursesIds = Array.ConvertAll(parts[4].Split(';'), int.Parse);
                            teacher.Courses.AddRange(coursesIds);
                        }

                        products.Add(teacher);
                        break;

                    case "course":
                        var courseCreator = new CourseCreator();
                        var studentsIds = parts.Length > 4 ? Array.ConvertAll(parts[4].Split(';'), int.Parse) : new int[0];
                        courseCreator.AnOperation(new string[] { parts[1], parts[2], parts[3] });
                        var course = (Course)courseCreator.GetProduct();
                        course.studentsId.AddRange(studentsIds);
                        products.Add(course);
                        break;
                }
            }
        }
    }

    static void ShowAllProducts()
    {
        foreach (var product in products)
        {
            switch (product)
            {
                case Student student:
                    Console.WriteLine($"Студент: ID = {student.Id}, Имя = {student.Name}, Курсы = {string.Join(", ", student.Courses)}");
                    break;
                case Teacher teacher:
                    Console.WriteLine($"Учитель: ID = {teacher.Id}, Имя = {teacher.Name}, Опыт = {teacher.Experience}, Курсы = {string.Join(", ", teacher.Courses)}");
                    break;
                case Course course:
                    Console.WriteLine($"Курс: ID = {course.Id}, Название = {course.Name}, Учитель ID = {course.teacherId}, Студенты = {string.Join(", ", course.studentsId)}");
                    break;
            }
        }
    }

    static void ShowRelatedProducts()
    {
        Console.Write("Введите тип объекта (студент, учитель, курс): ");
        string objectType = Console.ReadLine()?.Trim().ToLower();

        Console.Write("Введите ID объекта для показа связанных объектов: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Неверный формат ID.");
            return;
        }

        switch (objectType)
        {
            case "студент":
                var student = products.OfType<Student>().FirstOrDefault(s => s.Id == id);
                if (student != null)
                {
                    Console.WriteLine($"Студент: ID = {student.Id}, Имя = {student.Name}");
                    Console.WriteLine($"Курсы: {string.Join(", ", student.Courses)}");
                    return;
                }
                break;

            case "учитель":
                var teacher = products.OfType<Teacher>().FirstOrDefault(t => t.Id == id);
                if (teacher != null)
                {
                    Console.WriteLine($"Учитель: ID = {teacher.Id}, Имя = {teacher.Name}");
                    Console.WriteLine($"Курсы: {string.Join(", ", teacher.Courses)}");
                    return;
                }
                break;

            case "курс":
                var course = products.OfType<Course>().FirstOrDefault(c => c.Id == id);
                if (course != null)
                {
                    Console.WriteLine($"Курс: ID = {course.Id}, Название = {course.Name}");
                    Console.WriteLine($"Учитель ID: {course.teacherId}");
                    Console.WriteLine($"Студенты: {string.Join(", ", course.studentsId)}");
                    return;
                }
                break;

            default:
                Console.WriteLine("Неверный тип объекта. Пожалуйста, введите 'студент', 'учитель' или 'курс'.");
                return;
        }

        Console.WriteLine("Объект не найден.");
    }



}

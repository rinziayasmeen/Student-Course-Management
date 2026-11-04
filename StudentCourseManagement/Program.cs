
using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int CourseId { get; set; }

    public Student(int id, string name, int age, int courseId)
    {
        Id = id;
        Name = name;
        Age = age;
        CourseId = courseId;
    }
}
class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }

    public Course(int id, string name)
    {
        CourseId = id;
        CourseName = name;
    }
}

// LOGGING DELEGATE
delegate void LogAction(string message);
class CourseManager
{
    private Dictionary<int, Course> courses = new Dictionary<int, Course>();
    public void AddCourse(Course course)
    {
        if (courses.ContainsKey(course.CourseId))
        {
            Console.WriteLine("Course ID already exists");
            return;
        }
        courses.Add(course.CourseId, course);
        Console.WriteLine("Course added successfully");
    }

    public void ListCourses()
    {
        Console.WriteLine("\n--- COURSE LIST ---");
        foreach (Course c in courses.Values)
        {
            Console.WriteLine(c.CourseId + " - " + c.CourseName);
        }
    }

    public Course GetCourse(int id)
    {
        courses.TryGetValue(id, out Course course);
        return course;
    }
}

class StudentManager
{
    private List<Student> students = new List<Student>();
    private CourseManager courseManager;
    private LogAction logger;
    public StudentManager(CourseManager cm, LogAction log)
    {
        courseManager = cm;
        logger = log;
    }

    public void AddStudent(Student s)
    {
        if (students.Any(x => x.Id == s.Id))
        {
            Console.WriteLine("Student ID already exists");
            return;
        }
        if (courseManager.GetCourse(s.CourseId) == null)
        {
            Console.WriteLine("Invalid Course ID");
            return;
        }
        students.Add(s);
        Console.WriteLine("Student added successfully");
        logger?.Invoke("Added: " + s.Name);
    }

    public void ListStudents()
    {
        Console.WriteLine("\n--- STUDENT LIST ---");
        foreach (Student s in students)
        {
            Course c = courseManager.GetCourse(s.CourseId);
            Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, Age: {s.Age}, " + $"Course: {c.CourseName}");
        }
    }

    public void UpdateStudent(int id, string name, int age, int courseId)
    {
        Student s = students.FirstOrDefault(x => x.Id == id);
        if (s == null)
        {
            Console.WriteLine("Student not found");
            return;
        }
        if (courseManager.GetCourse(courseId) == null)
        {
            Console.WriteLine("Invalid Course ID");
            return;
        }

        s.Name = name;
        s.Age = age;
        s.CourseId = courseId;
        Console.WriteLine("Student updated successfully");
        logger?.Invoke("Updated: " + s.Name);
    }

    // DELETE STUDENT
    public void DeleteStudent(int id)
    {
        Student s = students.FirstOrDefault(x => x.Id == id);
        if (s == null)
        {
            Console.WriteLine("Student not found");
            return;
        }
        students.Remove(s);
        Console.WriteLine("Student deleted successfully");
        logger?.Invoke("Deleted: " + s.Name);
    }

    // LINQ: FILTER BY AGE
    public void FilterStudentsByAge(int age)
    {
        var result = students.Where(x => x.Age > age);

        Console.WriteLine("\n--- STUDENTS ABOVE AGE " + age + " ---");

        foreach (Student s in result)
        {
            Console.WriteLine(s.Name + " - " + s.Age);
        }
    }

    // LINQ: FILTER BY COURSE
    public void GetStudentsByCourse(int courseId)
    {
        Course c = courseManager.GetCourse(courseId);

        if (c == null)
        {
            Console.WriteLine("Course not found");
            return;
        }

        var result = students.Where(x => x.CourseId == courseId);

        Console.WriteLine("\n--- STUDENTS IN " + c.CourseName + " ---");

        foreach (Student s in result)
        {
            Console.WriteLine(s.Name);
        }
    }
}

// MAIN PROGRAM
class Program
{
    // DELEGATE CALLBACK METHOD
    static void Log(string message)
    {
        Console.WriteLine("[LOG] " + message);
    }

    static void Main()
    {
        // Create managers
        CourseManager cm = new CourseManager();
        StudentManager sm = new StudentManager(cm, Log);

        // Add courses
        cm.AddCourse(new Course(101, "C#"));
        cm.AddCourse(new Course(102, "Python"));
        cm.AddCourse(new Course(103, "Java"));

        // Add students
        sm.AddStudent(new Student(1, "Arun", 21, 101));
        sm.AddStudent(new Student(2, "Anu", 19, 102));
        sm.AddStudent(new Student(3, "Rahul", 23, 101));
        sm.AddStudent(new Student(4, "Meera", 20, 103));
        sm.AddStudent(new Student(5, "John", 25, 102));

        cm.ListCourses();
        sm.ListStudents();
        sm.UpdateStudent(1, "Arun Kumar", 22, 101);
        sm.DeleteStudent(5);
        sm.FilterStudentsByAge(20);
        sm.GetStudentsByCourse(101);
        sm.ListStudents();
    }
}
